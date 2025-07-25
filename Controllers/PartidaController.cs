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
    public class PartidaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MLDbContext _mlDb;

        public PartidaController(DataContext context, MLDbContext mLDb)
        {
            _context = context;
            _mlDb = mLDb;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getsingle(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Partida p = await _mlDb.TB_PARTIDAS
                .FirstOrDefaultAsync(pa => pa.Id == id);

                if (p == null)
                    throw new System.Exception("Partida Não Encontrada");
                return Ok(p);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Relatorio")]
        public async Task<IActionResult> GetGerarRelatorio()
        {   
            try
            {//é só pra verificar se teve partida pra mandar gerar relatorio  por isso ta chumbado
                Partida partidas = await _context.TB_PARTIDAS.OrderBy(p => p.Id)
    .FirstOrDefaultAsync();

                if (partidas == null)
                    throw new System.Exception("Sem Partidas Analisadas Hoje");

                string relatorio = await relatorioMensage();
                return Ok(relatorio);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Partida> partidas = await _context
                .TB_PARTIDAS.ToListAsync();
                if (partidas == null)
                    throw new System.Exception("Partida Não Encontrada");

                return Ok(partidas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllComplete")]
        public async Task<IActionResult> GetComplete()
        {
            try
            {
                List<Partida> partidas = await _mlDb
                .TB_PARTIDAS.ToListAsync();
                if (partidas == null)
                    throw new System.Exception("Partida Não Encontrada");

                return Ok(partidas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PartidasAnalisadas")]
        public async Task<IActionResult> GetAnalise()
        {
            try
            {
                List<Partida> partidas = await _context
                .TB_PARTIDAS.Where(p => p.PartidaAnalise == true).ToListAsync();
                if (partidas == null)
                    throw new System.Exception("Partida Não Encontrada");

                return Ok(partidas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetComfirmarPalpites")]
        public async Task<IActionResult> GetComfirmarPalpites()
        {
            try
            {
                List<Partida>  partidas = await _mlDb.TB_PARTIDAS
                    .Where(p => p.PartidaAnalise == true && p.Id_EstatisticaCasa == 0 && p.Id_EstatisticaFora == 0)
                    .OrderBy(p => p.Id)
                    .ToListAsync();

                if (partidas == null)
                    throw new System.Exception("Partida Não Encontrada");

                return Ok(partidas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMultiPartidas")]
        public async Task<IActionResult> GetMultiPartidas([FromQuery] string ids)
        {
            try
            {
                List<int> listaIds = ids.Split(',').Select(int.Parse).ToList();

                List<Partida> partidas = await _mlDb
                .TB_PARTIDAS.Where(p => listaIds.Contains(p.Id)).ToListAsync();
                
                if (partidas.Count == 0)
                    throw new System.Exception("Partida Não Encontrada");

                return Ok(partidas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(Partida partida)
        {
            try
            {

                await _mlDb.TB_PARTIDAS.AddAsync(partida);
                await _mlDb.SaveChangesAsync();
                int idPrincipal = partida.Id;

                _mlDb.Entry(partida).State = EntityState.Detached;
                _context.Entry(partida).State = EntityState.Added;
                await _context.SaveChangesAsync();
                int idSecundario = partida.Id;

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idSecundario });
            }
            catch (Exception ex)
            {
                try
                {
                    var inserted = await _mlDb.TB_PARTIDAS.FindAsync(partida.Id);
                    if (inserted != null)
                    {
                        _mlDb.TB_PARTIDAS.Remove(inserted);
                        await _mlDb.SaveChangesAsync();
                    }
                }
                catch (Exception rollbackEx)
                {
                    return BadRequest($"Erro ao inserir estatística: {ex.Message} | Erro ao desfazer no primeiro banco: {rollbackEx.Message}");
                }

                return BadRequest($"Erro ao inserir estatística: {ex.Message}. Inserção no primeiro banco foi revertida com sucesso.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Partida p)
        {
            try
            {
                if (id == 0)
                    return BadRequest("O Id não pode ser igual a zero");

                var PartidaML = await _mlDb.TB_PARTIDAS
                .FirstOrDefaultAsync(es => es.Id == id);

                var partidaSecundaria = await _context.TB_PARTIDAS
                .FirstOrDefaultAsync(es => es.Id == id);


                if (PartidaML == null || partidaSecundaria == null)
                    return NotFound("Estatística não encontrada em um dos bancos.");



                p.Id = id;

                _mlDb.Entry(PartidaML).CurrentValues.SetValues(p);
                _context.Entry(partidaSecundaria).CurrentValues.SetValues(p);

                await _mlDb.SaveChangesAsync();
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _mlDb.Entry(PartidaML).State = EntityState.Unchanged;
                    return BadRequest($"Erro ao salvar no segundo banco: {ex.Message}");
                }

                return Ok("Estatística atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delte(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Partida p = await _mlDb.TB_PARTIDAS
              .FirstOrDefaultAsync(pa => pa.Id == id);
                Partida part = await _context.TB_PARTIDAS
              .FirstOrDefaultAsync(pa => pa.Id == id);


                if (p == null || part == null)
                    throw new System.Exception("Partida Não Encontrada");

                _mlDb.TB_PARTIDAS.Remove(p);
                _context.TB_PARTIDAS.Remove(part);


                await _context.SaveChangesAsync();
                int linhasAfetadas = await _mlDb.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("Apague")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                int linhasAfetadas = await _mlDb.Database.ExecuteSqlRawAsync("DELETE FROM [TB_PARTIDAS] WHERE [PartidaAnalise] = 0;");
                return Ok($"{linhasAfetadas} Partidas foram apagadas.");
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Erro ao apagar logs: {ex.Message}");
            }
        }

        private async Task<string> relatorioMensage()
        {
            List<Palpites> palpites = await _context.TB_PALPITES.ToListAsync();
            List<Partida> partidas = await _context.TB_PARTIDAS.Where(p => p.PartidaAnalise == true).ToListAsync();
            List<Partida> totalPartidas = await _context.TB_PARTIDAS.Where(p => p.PartidaAnalise == false).ToListAsync();
            List<ErrosLogs> Erros = await _context.TB_ERROSLOGS.ToListAsync();
            List<ErrosLogs> partidasDescontadas = await _context.TB_ERROSLOGS.Where(e => e.QualPageFoi == "Obter_Ultimos_jogos.py").ToListAsync();
            List<ErrosLogs> ultimas = await _context.TB_ERROSLOGS.Where(e => e.QualPageFoi == "Obter_Estatisticas.py").ToListAsync();

            int num_partidasEsperadas = partidas.Count() * 15;

            // Correções principais aqui - adicionando conversão para float e multiplicando por 100
            float eficienciaPalpites = partidas.Count > 0 ? (float)palpites.Count() / partidas.Count() * 100 : 0;
            float eficienciaErros = totalPartidas.Count > 0 ? 100 - ((float)Erros.Count() / totalPartidas.Count() * 100) : 100;
            float eficienciaEstatistica = num_partidasEsperadas > 0 ? (float)totalPartidas.Count() / num_partidasEsperadas * 100 : 0;

            string qtPalpites = $"Hoje foi gerado {palpites.Count()} palpites para {partidas.Count()} partidas " +
                               $"gerando uma eficiência de {eficienciaPalpites:F2}%. " +
                               $"Também foram gerados {Erros.Count()} erros na automação, com eficiência de acertos de {eficienciaErros:F2}%.";

            if (partidasDescontadas.Count() > 0)
                qtPalpites += $"\n\nPerdemos {partidasDescontadas.Count()} partidas que poderiam trazer bons palpites.";

            if (ultimas.Count() > 0)
                qtPalpites += $"\n\nPerdemos {ultimas.Count()} estatísticas que poderiam melhorar a qualidade dos palpites.";

            qtPalpites += $"\n\nEra esperado um total de {num_partidasEsperadas} estatísticas, " +
                         $"porém foram lidas {totalPartidas.Count() - partidas.Count()}, " +
                         $"gerando uma diferença de {num_partidasEsperadas - totalPartidas.Count()} " +
                         $"e uma eficiência de {eficienciaEstatistica:F2}%.";

            return qtPalpites;
        }
    }
}