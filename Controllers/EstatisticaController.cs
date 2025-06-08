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
                Estatistica e = await _context.TB_ESTATISTICA
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

        [HttpPost]
        public async Task<IActionResult> Post(Estatistica e)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                await _MLDb.TB_ESTATISTICA.AddAsync(e);
                await _MLDb.SaveChangesAsync();

                int idPrincipal = e.Id_Estatistica;

                _MLDb.Entry(e).State = EntityState.Detached;


                _context.TB_ESTATISTICA.Add(e);
                await _context.SaveChangesAsync();

                int idSecundario = e.Id_Estatistica;

                // Confirma a transação em AMBOS os bancos
                scope.Complete();

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idSecundario });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
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


        [HttpPut]
        public async Task<IActionResult> Put(Estatistica e)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                _MLDb.TB_ESTATISTICA.Update(e);
                int linhasAfetadas = await _MLDb.SaveChangesAsync();

                _MLDb.Entry(e).State = EntityState.Detached;


                _context.TB_ESTATISTICA.Update(e);
                await _context.SaveChangesAsync();
                scope.Complete();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Estatistica e = await _MLDb.TB_ESTATISTICA
                .FirstOrDefaultAsync(es => es.Id_Estatistica == id);

                Estatistica e_repeat = await _context.TB_ESTATISTICA
                .FirstOrDefaultAsync(es => es.Id_Estatistica == id);
                if (e == null || e_repeat == null)
                    throw new System.Exception("Estatistica Não Encontrada");


                _context.TB_ESTATISTICA.Remove(e);
                _MLDb.TB_ESTATISTICA.Remove(e_repeat);


                await _MLDb.SaveChangesAsync();
                int linhasAfetadas = await _context.SaveChangesAsync();
                scope.Complete();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}