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

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = e.Id_Estatistica });
            }
            catch (Exception ex)
            {
                try
                {
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
        public async Task<IActionResult> Post(Partida_Estatistica_DTO dados)
        {
            try
            {
                // 1. Insere estatísticas no primeiro contexto (_MLDb)
                await _MLDb.TB_ESTATISTICA.AddRangeAsync(dados.EstatisticaCasa, dados.EstatisticaFora);
                await _MLDb.SaveChangesAsync();

                // 2. Atualiza a partida com os IDs das estatísticas
                dados.Partida.Id_EstatisticaCasa = dados.EstatisticaCasa.Id_Estatistica;
                dados.Partida.Id_EstatisticaFora = dados.EstatisticaFora.Id_Estatistica;

                // 3. Insere a partida no primeiro contexto
                await _MLDb.TB_PARTIDAS.AddAsync(dados.Partida);
                await _MLDb.SaveChangesAsync();

                // 4. Atualiza as estatísticas com o ID da partida
                dados.EstatisticaCasa.Id_Partida = dados.Partida.Id;
                dados.EstatisticaFora.Id_Partida = dados.Partida.Id;

                _MLDb.TB_ESTATISTICA.UpdateRange(dados.EstatisticaCasa, dados.EstatisticaFora);
                await _MLDb.SaveChangesAsync();

                // 5. Desanexar do primeiro contexto
                _MLDb.Entry(dados.Partida).State = EntityState.Detached;
                _MLDb.Entry(dados.EstatisticaCasa).State = EntityState.Detached;
                _MLDb.Entry(dados.EstatisticaFora).State = EntityState.Detached;

                try
                {
                    // 6. Inserir no segundo contexto
                    await _context.TB_PARTIDAS.AddAsync(dados.Partida);
                    await _context.TB_ESTATISTICA.AddRangeAsync(dados.EstatisticaCasa, dados.EstatisticaFora);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exSecundario)
                {
                    // ⚠️ Rollback manual no primeiro contexto (_MLDb)
                    var estatisticas = await _MLDb.TB_ESTATISTICA
                        .Where(e => e.Id_Estatistica == dados.EstatisticaCasa.Id_Estatistica
                                 || e.Id_Estatistica == dados.EstatisticaFora.Id_Estatistica)
                        .ToListAsync();

                    _MLDb.TB_ESTATISTICA.RemoveRange(estatisticas);

                    var partidaSalva = await _MLDb.TB_PARTIDAS.FindAsync(dados.Partida.Id);
                    if (partidaSalva != null)
                        _MLDb.TB_PARTIDAS.Remove(partidaSalva);

                    await _MLDb.SaveChangesAsync();

                    return BadRequest($"Erro ao salvar no segundo banco: {exSecundario.Message}. Alterações no primeiro banco foram revertidas.");
                }

                return Ok(new
                {
                    Mensagem = "Partida e estatísticas salvas com sucesso nos dois bancos.",
                    IdPartida = dados.Partida.Id,
                    IdEstatisticaCasa = dados.EstatisticaCasa.Id_Estatistica,
                    IdEstatisticaFora = dados.EstatisticaFora.Id_Estatistica
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar no primeiro banco: {ex.Message}");
            }
        }

        [HttpPut("PartidaAnalisada")]
        public async Task<IActionResult> PutAnalidasdas(Partida_Estatistica_DTO dados)
        {
            try
            {
                // 1. Insere estatísticas no primeiro contexto (_MLDb)
                await _MLDb.TB_ESTATISTICA.AddRangeAsync(dados.EstatisticaCasa, dados.EstatisticaFora);
                await _MLDb.SaveChangesAsync();

                // 2. Atualiza a partida com os IDs das estatísticas
                dados.Partida.Id_EstatisticaCasa = dados.EstatisticaCasa.Id_Estatistica;
                dados.Partida.Id_EstatisticaFora = dados.EstatisticaFora.Id_Estatistica;

                Partida PartidaML = await _MLDb.TB_PARTIDAS
                .FirstOrDefaultAsync(es => es.NomeTimeCasa == dados.Partida.NomeTimeCasa && es.NomeTimeFora == dados.Partida.NomeTimeFora
                && es.Url_Partida == dados.Partida.Url_Partida
                && es.DataPartida == dados.Partida.DataPartida
                && es.Campeonato == dados.Partida.Campeonato
                && es.TipoPartida == dados.Partida.TipoPartida
                && es.PartidaAnalise == true);

                Partida partidaSecundaria = await _context.TB_PARTIDAS
                .FirstOrDefaultAsync(es => es.Id == PartidaML.Id);

                if (PartidaML == null || partidaSecundaria == null)
                    return NotFound("Estatística não encontrada em um dos bancos.");

                dados.Partida.PartidaAnalise = false;//ela ja foi analisada ent não é mais uma partida analise

                _MLDb.Entry(PartidaML).CurrentValues.SetValues(dados.Partida);
                _context.Entry(partidaSecundaria).CurrentValues.SetValues(dados.Partida);

                await _MLDb.SaveChangesAsync();
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _MLDb.Entry(PartidaML).State = EntityState.Unchanged;
                    return BadRequest($"Erro ao salvar no segundo banco: {ex.Message}");
                }


                // 4. Atualiza as estatísticas com o ID da partida
                dados.EstatisticaCasa.Id_Partida = dados.Partida.Id;
                dados.EstatisticaFora.Id_Partida = dados.Partida.Id;

                _MLDb.TB_ESTATISTICA.UpdateRange(dados.EstatisticaCasa, dados.EstatisticaFora);
                await _MLDb.SaveChangesAsync();

                // 5. Desanexar do primeiro contexto
                _MLDb.Entry(dados.Partida).State = EntityState.Detached;
                _MLDb.Entry(dados.EstatisticaCasa).State = EntityState.Detached;
                _MLDb.Entry(dados.EstatisticaFora).State = EntityState.Detached;

                try
                {
                    // 6. Inserir no segundo contexto
                    await _context.TB_PARTIDAS.AddAsync(dados.Partida);
                    await _context.TB_ESTATISTICA.AddRangeAsync(dados.EstatisticaCasa, dados.EstatisticaFora);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exSecundario)
                {
                    // ⚠️ Rollback manual no primeiro contexto (_MLDb)
                    var estatisticas = await _MLDb.TB_ESTATISTICA
                        .Where(e => e.Id_Estatistica == dados.EstatisticaCasa.Id_Estatistica
                                 || e.Id_Estatistica == dados.EstatisticaFora.Id_Estatistica)
                        .ToListAsync();

                    _MLDb.TB_ESTATISTICA.RemoveRange(estatisticas);

                    var partidaSalva = await _MLDb.TB_PARTIDAS.FindAsync(dados.Partida.Id);
                    if (partidaSalva != null)
                        _MLDb.TB_PARTIDAS.Remove(partidaSalva);

                    await _MLDb.SaveChangesAsync();

                    return BadRequest($"Erro ao salvar no segundo banco: {exSecundario.Message}. Alterações no primeiro banco foram revertidas.");
                }

                return Ok(new
                {
                    Mensagem = "Partida e estatísticas salvas com sucesso nos dois bancos.",
                    IdPartida = dados.Partida.Id,
                    IdEstatisticaCasa = dados.EstatisticaCasa.Id_Estatistica,
                    IdEstatisticaFora = dados.EstatisticaFora.Id_Estatistica
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar no primeiro banco: {ex.Message}");
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

                Estatistica e = await _MLDb.TB_ESTATISTICA
                    .FirstOrDefaultAsync(es => es.Id_Estatistica == id);

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

        [HttpDelete("Apague")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                int linhasAfetadas = await _MLDb.Database.ExecuteSqlRawAsync("DELETE FROM TB_ESTATISTICA");
                return Ok($"{linhasAfetadas} Estatisticas foram apagadas.");
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao apagar logs: {ex.Message}");
            }
        }
    }
}