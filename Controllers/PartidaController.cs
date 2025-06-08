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
                Partida p = await _context.TB_PARTIDAS
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
                List<Partida> partidas = await _context
                .TB_PARTIDAS.Where(p => p.Id == 1).ToListAsync();

                if (partidas.Count() == 0)
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


        [HttpPost]
        public async Task<IActionResult> Post(Partida partida)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                await _mlDb.TB_PARTIDAS.AddAsync(partida);
                await _mlDb.SaveChangesAsync();
                int idPrincipal = partida.Id;

                // Desanexa do primeiro contexto
                _mlDb.Entry(partida).State = EntityState.Detached;



                _context.TB_PARTIDAS.Add(partida);
                await _context.SaveChangesAsync();

                int idSecundario = partida.Id;

                scope.Complete();
                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idSecundario });
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
        public async Task<IActionResult> Put(Partida p)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                _mlDb.TB_PARTIDAS.Update(p);
                int linhasAfetadas = await _mlDb.SaveChangesAsync();


                _mlDb.Entry(p).State = EntityState.Detached;

                _context.TB_PARTIDAS.Update(p);
                await _context.SaveChangesAsync();
                scope.Complete();

                return Ok(linhasAfetadas);
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