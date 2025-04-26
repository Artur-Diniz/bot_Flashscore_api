using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly DataContext _context;

        public PartidaController(DataContext context)
        {
            _context = context;
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
            try
            {
                await _context.TB_PARTIDAS.AddAsync(partida);
                await _context.SaveChangesAsync();

                return Ok(partida.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Relatorio")]
        public async Task<IActionResult> PostGerarRelatorio()
        {
            try
            {//é só pra verificar se teve partida pra mandar gerar relatorio  por isso ta chumbado
                List<Partida> partidas = await _context
                .TB_PARTIDAS.Where(p => p.Id==1).ToListAsync();

                if (partidas.Count() > 0)
                    throw new System.Exception("Sem Partidas Analisadas Hoje");

                return Ok(relatorioMensage());
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put(Partida p)
        {
            try
            {
                _context.TB_PARTIDAS.Update(p);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delte(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Partida p = await _context.TB_PARTIDAS
              .FirstOrDefaultAsync(pa => pa.Id == id);

                if (p == null)
                    throw new System.Exception("Partida Não Encontrada");
                _context.TB_PARTIDAS.Remove(p);


                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<string> relatorioMensage()
        {
            string mensagem = "";
            List<Palpites> palpites = await _context
               .TB_PALPITES.ToListAsync();


            List<Partida> partidas = await _context
            .TB_PARTIDAS.Where(p => p.PartidaAnalise == true).ToListAsync();

            List<Partida> totalPartidas = await _context
            .TB_PARTIDAS.Where(p => p.PartidaAnalise == false).ToListAsync();


            List<ErrosLogs> Erros = await _context
            .TB_ERROSLOGS.ToListAsync();

            List<ErrosLogs> partidasDescontadas = await _context
            .TB_ERROSLOGS.Where(e => e.QualPageFoi == "Obter_Ultimos_jogos.py").ToListAsync();

            List<ErrosLogs> ultimas = await _context
            .TB_ERROSLOGS.Where(e => e.QualPageFoi == "ObterJogosEspecificos.py").ToListAsync();

            int num_partidasEsperadas = partidas.Count() * 15;
            double eficienciaPalpites = palpites.Count() / partidas.Count();
            double eficienciaErros = 100 - (Erros.Count() / partidas.Count());
            double eficienciaEstatistica = totalPartidas.Count() / num_partidasEsperadas;
            string qtPalpites = $" Hoje foi gerado {palpites.Count()} por {partidas.Count()} num de partidas" +
              $"gerando uma eficiencia de {eficienciaPalpites}% . \n" +
              $" também foi gerado {Erros.Count()} Erros na Automação, sendo uma eficiencia de acertos {eficienciaErros}% ";


            if (partidasDescontadas.Count() > 0)
                qtPalpites = qtPalpites + $",porém perdemos {partidasDescontadas.Count()} partidas que poderiam trazer palpites bons.";


            if (ultimas.Count() > 0)
                qtPalpites = qtPalpites + $"porém perdemos {ultimas.Count()} Estatisticas que poderiam trazer Estatisticas que melhorariam os palpites.";



            qtPalpites = qtPalpites + $"Além disso era esperado num de estatisticas de: {num_partidasEsperadas} porém foram lidas apenas {totalPartidas.Count() - partidas.Count()} gerando " +
              $"uma diferença de {num_partidasEsperadas - totalPartidas.Count()} estatisticas trazendo uma eficencia de {eficienciaEstatistica}% ";

            return mensagem;

        }
    }
}