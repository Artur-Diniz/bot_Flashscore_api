using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;
using System.Transactions;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstatisticaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MLDbContext _MLDb;

        public EstatisticaController(DataContext context, MLDbContext mLDb)
        {
            _context = context;
            _MLDb = mLDb;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Estatistica Não Encontrada");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Estatistica e = await _MLDb.TB_ESTATISTICA
                .FirstOrDefaultAsync(es => es.Id_Estatistica == id);
                if (e == null)
                    throw new System.Exception("Estatistica Não Encontrada");

                return Ok(e);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Estatistica> estatisticas = await _context
                .TB_ESTATISTICA.ToListAsync();

                return Ok(estatisticas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllComplete")]
        public async Task<IActionResult> GetAllComplete()
        {
            try
            {
                List<Estatistica> estatisticas = await _MLDb
                .TB_ESTATISTICA.ToListAsync();

                return Ok(estatisticas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Estatistica e)
        {
            try
            {
                // 1. Insere no primeiro banco (_MLDb) e salva para obter o ID
                await _MLDb.TB_ESTATISTICA.AddAsync(e);
                await _MLDb.SaveChangesAsync();
                int idPrincipal = e.Id_Estatistica;

                // 2. Prepara a entidade para o segundo banco
                _MLDb.Entry(e).State = EntityState.Detached;
                e.Id_Estatistica = idPrincipal;
                _context.Entry(e).State = EntityState.Added;

                // 3. Tenta salvar no segundo banco
                await _context.SaveChangesAsync();

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idPrincipal });
            }
            catch (Exception ex)
            {
                try
                {
                    // Estratégia de compensação: remove do primeiro banco se o segundo falhar
                    var inserted = await _MLDb.TB_ESTATISTICA.FindAsync(e.Id_Estatistica);
                    if (inserted != null)
                    {
                        _MLDb.TB_ESTATISTICA.Remove(inserted);
                        await _MLDb.SaveChangesAsync();
                    }
                }
                catch (Exception rollbackEx)
                {
                    return BadRequest($"Erro ao inserir estatística: {ex.Message} | Erro ao desfazer no primeiro banco: {rollbackEx.Message}");
                }

                return BadRequest($"Erro ao inserir estatística: {ex.Message}. Inserção no primeiro banco foi revertida com sucesso.");
            }
        }


        [HttpPost("Partida")]
        public async Task<IActionResult> Post(Partida_Estatistica_DTO dTO)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _MLDb.TB_ESTATISTICA.AddRangeAsync(dTO.EstatisticaCasa, dTO.EstatisticaFora);

                await _MLDb.SaveChangesAsync();

                dTO.Partida.Id_EstatisticaCasa = dTO.EstatisticaCasa.Id_Estatistica;
                dTO.Partida.Id_EstatisticaFora = dTO.EstatisticaFora.Id_Estatistica;

                await _MLDb.TB_PARTIDAS.AddAsync(dTO.Partida);

                await _MLDb.SaveChangesAsync();

                dTO.EstatisticaCasa.Id_Partida = dTO.Partida.Id;
                dTO.EstatisticaFora.Id_Partida = dTO.Partida.Id;
                await _MLDb.SaveChangesAsync();

                _MLDb.TB_ESTATISTICA.UpdateRange(dTO.EstatisticaCasa, dTO.EstatisticaFora);

                _MLDb.TB_PARTIDAS.Update(dTO.Partida);

                await _MLDb.SaveChangesAsync();


                _MLDb.Entry(dTO.Partida).State = EntityState.Detached;
                _MLDb.Entry(dTO.EstatisticaFora).State = EntityState.Detached;
                _MLDb.Entry(dTO.EstatisticaCasa).State = EntityState.Detached;
                dTO.EstatisticaCasa.Id_Estatistica = 0;
                dTO.EstatisticaCasa.Id_Partida = 0;
                dTO.EstatisticaFora.Id_Estatistica = 0;
                dTO.EstatisticaFora.Id_Partida = 0;
                dTO.Partida.Id = 0;
                dTO.Partida.Id_EstatisticaCasa = 0;
                dTO.Partida.Id_EstatisticaFora = 0;

                _context.Entry(dTO.Partida).State = EntityState.Added;
                _context.Entry(dTO.EstatisticaFora).State = EntityState.Added;
                _context.Entry(dTO.EstatisticaCasa).State = EntityState.Added;
                dTO.EstatisticaCasa.Id_Partida = dTO.Partida.Id;
                dTO.EstatisticaFora.Id_Partida = dTO.Partida.Id;
                dTO.Partida.Id_EstatisticaCasa = dTO.EstatisticaCasa.Id_Estatistica;
                dTO.Partida.Id_EstatisticaFora = dTO.EstatisticaFora.Id_Estatistica;

                _context.Entry(dTO.Partida).State = EntityState.Added;
                _context.Entry(dTO.EstatisticaFora).State = EntityState.Added;
                _context.Entry(dTO.EstatisticaCasa).State = EntityState.Added;

                await _context.SaveChangesAsync();


                scope.Complete();


                string mensagem = $"Estatísticas salvas - Casa: {dTO.EstatisticaCasa.Id_Estatistica}, " +
                                 $"Fora: {dTO.EstatisticaFora.Id_Estatistica}, " +
                                 $"Partida ID: {dTO.Partida.Id}";
                return Ok(mensagem);

            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return BadRequest(inner.Message);
            }
        }

[HttpPut("{id}")]
public async Task<IActionResult> Put(int id, Estatistica estatisticaAtualizada)
{
    try
    {
        if (id == 0)
            return BadRequest("O Id não pode ser igual a zero");

        var estatisticaML = await _MLDb.TB_ESTATISTICA
            .FirstOrDefaultAsync(es => es.Id_Estatistica == id);

        var estatisticaSecundaria = await _context.TB_ESTATISTICA
            .FirstOrDefaultAsync(es => es.Id_Estatistica == id);

        if (estatisticaML == null || estatisticaSecundaria == null)
            return NotFound("Estatística não encontrada em um dos bancos.");

        // Atualiza dados
        _MLDb.Entry(estatisticaML).CurrentValues.SetValues(estatisticaAtualizada);
        _context.Entry(estatisticaSecundaria).CurrentValues.SetValues(estatisticaAtualizada);

        estatisticaML.Id_Estatistica = id;
        estatisticaSecundaria.Id_Estatistica = id;

        // Salva no primeiro banco
        await _MLDb.SaveChangesAsync();

        try
        {
            // Salva no segundo banco
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Tenta desfazer alteração no primeiro banco
            _MLDb.Entry(estatisticaML).State = EntityState.Unchanged;
            return BadRequest($"Erro ao salvar no segundo banco: {ex.Message}");
        }

        return Ok("Estatística atualizada com sucesso.");
    }
    catch (Exception ex)
    {
        return BadRequest($"Erro ao atualizar estatística: {ex.Message}");
    }
}



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0)
                    throw new Exception("O Id não pode ser igual a zero");

                // Busca a entidade no primeiro banco
                Estatistica e = await _MLDb.TB_ESTATISTICA
                    .FirstOrDefaultAsync(es => es.Id_Estatistica == id);

                // Busca a entidade no segundo banco
                Estatistica e_repeat = await _context.TB_ESTATISTICA
                    .FirstOrDefaultAsync(es => es.Id_Estatistica == id);

                if (e == null || e_repeat == null)
                    throw new Exception("Estatística não encontrada em um dos bancos.");

                // Remove do respectivo contexto
                _MLDb.TB_ESTATISTICA.Remove(e);
                _context.TB_ESTATISTICA.Remove(e_repeat);

                // Salva em ambos
                await _MLDb.SaveChangesAsync();
                int linhasAfetadas = await _context.SaveChangesAsync();


                return Ok(linhasAfetadas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar: {ex.Message}");
            }
        }

    }
}