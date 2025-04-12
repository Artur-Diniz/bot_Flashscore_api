using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;
using System.Threading.Tasks;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstatisticaTimesController : ControllerBase
    {
        private readonly DataContext _context;

        public EstatisticaTimesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Estatistica Não Encontrada");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Estatistica_Times e = await _context.TB_ESTATISTICA_TIME
                .FirstOrDefaultAsync(es => es.Id == id);
                if (e == null)
                    throw new System.Exception("Estatistica do Times Não Encontrada");

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
                List<Estatistica_Times> estatisticas = await _context
                .TB_ESTATISTICA_TIME.ToListAsync();

                return Ok(estatisticas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Estatistica_Times e)
        {
            try
            {
                await _context.TB_ESTATISTICA_TIME.AddAsync(e);
                await _context.SaveChangesAsync();

                return Ok(e.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GerarEstatistica/{nomeTime}")]
        public async Task<IActionResult> GerarEstatisticaMedia(string nomeTime)
        {
            try
            {
                Partida partidaAnalisada = await _context.TB_PARTIDAS
                .FirstOrDefaultAsync(p => p.PartidaAnalise == true && p.NomeTimeCasa.ToLower().Contains(nomeTime.ToLower()) ||
                 p.PartidaAnalise == true && p.NomeTimeFora.ToLower().Contains(nomeTime.ToLower()));

                if (partidaAnalisada == null)
                    throw new System.Exception("não encontrado time com esse Nome");

                Estatistica_Times e = await Buscarpartidas(nomeTime);


                await _context.TB_ESTATISTICA_TIME.AddAsync(e);
                await _context.SaveChangesAsync();

                return Ok(e.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put(Estatistica_Times e)
        {
            try
            {
                _context.TB_ESTATISTICA_TIME.Update(e);
                int linhasAfetadas = await _context.SaveChangesAsync();

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
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Estatistica_Times e = await _context.TB_ESTATISTICA_TIME
                .FirstOrDefaultAsync(es => es.Id == id);
                if (e == null)
                    throw new System.Exception("Estatistica Não Encontrada");
                _context.TB_ESTATISTICA_TIME.Remove(e);


                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<Estatistica_Times> Buscarpartidas(string nomeTime)
        {
            Partida partidaAnalisada = await _context.TB_PARTIDAS.FirstOrDefaultAsync(p => p.PartidaAnalise == true && p.NomeTimeCasa.ToLower().Contains(nomeTime.ToLower()) ||
             p.PartidaAnalise == true && p.NomeTimeFora.ToLower().Contains(nomeTime.ToLower()));




            List<Estatistica> estatisticasRival = new List<Estatistica>();
            List<Estatistica> estatisticas = new List<Estatistica>();

            if (partidaAnalisada.NomeTimeCasa.ToLower().Contains(nomeTime.ToLower()))
            {
                estatisticas = await _context.TB_ESTATISTICA.Where(n => n.NomeTime.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Casa").ToListAsync();
                estatisticasRival = await _context.TB_ESTATISTICA.Where(n => n.NomeTimeRival.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Casa").ToListAsync();

            }
            else
            {
                estatisticas = await _context.TB_ESTATISTICA.Where(n => n.NomeTime.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Fora").ToListAsync();
                estatisticasRival = await _context.TB_ESTATISTICA.Where(n => n.NomeTimeRival.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Fora").ToListAsync();
            }
            Estatistica_Times time = gerarMediaByTime(estatisticas, estatisticasRival);

            string rival = "";

            if (nomeTime == partidaAnalisada.NomeTimeCasa)
            {
                time.NomeTime = partidaAnalisada.NomeTimeCasa;
                rival = partidaAnalisada.NomeTimeFora;
            }
            else
            {
                time.NomeTime = partidaAnalisada.NomeTimeFora;
                rival = partidaAnalisada.NomeTimeCasa;
            }


            time = await GerarConfrontoDireto(time, rival);

            return time;

        }

        private Estatistica_Times gerarMediaByTime(List<Estatistica> principal, List<Estatistica> rival)
        {
            Estatistica_Times estatisticas = new Estatistica_Times
            {
                Gol = (float)principal.Average(p => p.Gol),
                GolSofrido = (float)principal.Average(p => p.GolSofrido),
                Posse_Bola = (float)principal.Average(p => p.Posse_Bola),
                Total_Finalizacao = (float)principal.Average(p => p.Total_Finalizacao),
                Chances_Claras = (float)principal.Average(p => p.Chances_Claras),
                Escanteios = (float)principal.Average(p => p.Escanteios),
                Bolas_trave = (float)principal.Average(p => p.Bolas_trave),
                Gols_de_cabeça = (float)principal.Average(p => p.Gols_de_cabeça),
                Defesas_Goleiro = (float)principal.Average(p => p.Defesas_Goleiro),
                Impedimentos = (float)principal.Average(p => p.Impedimentos),
                Faltas = (float)principal.Average(p => p.Faltas),
                Cartoes_Amarelos = (float)principal.Average(p => p.Cartoes_Amarelos),
                Cartoes_Vermelhos = (float)principal.Average(p => p.Cartoes_Vermelhos),
                Laterais_Cobrados = (float)principal.Average(p => p.Laterais_Cobrados),
                Toque_Area_Adversaria = (float)principal.Average(p => p.Toque_Area_Adversaria),
                Passes = (float)principal.Average(p => p.Passes),
                Passes_Totais = (float)principal.Average(p => p.Passes_Totais),
                Precisao_Passes = (float)principal.Average(p => p.Precisao_Passes),
                Passes_terco_Final = (float)principal.Average(p => p.Passes_terco_Final),
                Cruzamentos = (float)principal.Average(p => p.Cruzamentos),
                Desarmes = (float)principal.Average(p => p.Desarmes),
                Bolas_Afastadas = (float)principal.Average(p => p.Bolas_Afastadas),
                Interceptacoes = (float)principal.Average(p => p.Interceptacoes),


                Gol_Adversaria = (float)rival.Average(r => r.Gol),
                GolSofrido_Adversaria = (float)rival.Average(r => r.GolSofrido),
                Posse_Bola_Adversaria = (float)rival.Average(r => r.Posse_Bola),
                Total_Finalizacao_Adversaria = (float)rival.Average(r => r.Total_Finalizacao),
                Chances_Claras_Adversaria = (float)rival.Average(r => r.Chances_Claras),
                Escanteios_Adversaria = (float)rival.Average(r => r.Escanteios),
                Bolas_trave_Adversaria = (float)rival.Average(r => r.Bolas_trave),
                Gols_de_cabeça_Adversaria = (float)rival.Average(r => r.Gols_de_cabeça),
                Defesas_Goleiro_Adversaria = (float)rival.Average(r => r.Defesas_Goleiro),
                Impedimentos_Adversaria = (float)rival.Average(r => r.Impedimentos),
                Faltas_Adversaria = (float)rival.Average(r => r.Faltas),
                Cartoes_Amarelos_Adversaria = (float)rival.Average(r => r.Cartoes_Amarelos),
                Cartoes_Vermelhos_Adversaria = (float)rival.Average(r => r.Cartoes_Vermelhos),
                Laterais_Cobrados_Adversaria = (float)rival.Average(r => r.Laterais_Cobrados),
                Toque_Area_Adversaria_Adversaria = (float)rival.Average(r => r.Toque_Area_Adversaria),
                Passes_Adversaria = (float)rival.Average(r => r.Passes),
                Passes_Totais_Adversaria = (float)rival.Average(r => r.Passes_Totais),
                Precisao_Passes_Adversaria = (float)rival.Average(r => r.Precisao_Passes),
                Passes_terco_Final_Adversaria = (float)rival.Average(r => r.Passes_terco_Final),
                Cruzamentos_Adversaria = (float)rival.Average(r => r.Cruzamentos),
                Desarmes_Adversaria = (float)rival.Average(r => r.Desarmes),
                Bolas_Afastadas_Adversaria = (float)rival.Average(r => r.Bolas_Afastadas),
                Interceptacoes_Adversaria = (float)rival.Average(r => r.Interceptacoes),
            };
            return estatisticas;
        }

        private async Task<Estatistica_Times> GerarConfrontoDireto(Estatistica_Times estastitica, string Rival)
        {


            List<Estatistica> jogos = await _context.TB_ESTATISTICA.Where(e => e.NomeTime == estastitica.NomeTime && e.NomeTimeRival==Rival ).ToListAsync();
            List<Estatistica> jogosFora = await _context.TB_ESTATISTICA.Where(e => e.NomeTimeRival == estastitica.NomeTime && e.NomeTime==Rival ).ToListAsync();

            jogos.AddRange(jogosFora);


            estastitica.Gol_Confrontos = (float)jogos.Average(j => j.Gol);
            estastitica.GolSofrido_Confrontos = (float)jogos.Average(j => j.GolSofrido);
            estastitica.Posse_Bola_Confrontos = (float)jogos.Average(j => j.Posse_Bola);
            estastitica.Total_Finalizacao_Confrontos = (float)jogos.Average(j => j.Total_Finalizacao);
            estastitica.Chances_Claras_Confrontos = (float)jogos.Average(j => j.Chances_Claras);
            estastitica.Escanteios_Confrontos = (float)jogos.Average(j => j.Escanteios);
            estastitica.Bolas_trave_Confrontos = (float)jogos.Average(j => j.Bolas_trave);
            estastitica.Gols_de_cabeça_Confrontos = (float)jogos.Average(j => j.Gols_de_cabeça);
            estastitica.Defesas_Goleiro_Confrontos = (float)jogos.Average(j => j.Defesas_Goleiro);
            estastitica.Impedimentos_Confrontos = (float)jogos.Average(j => j.Impedimentos);
            estastitica.Faltas_Confrontos = (float)jogos.Average(j => j.Faltas);
            estastitica.Cartoes_Amarelos_Confrontos = (float)jogos.Average(j => j.Cartoes_Amarelos);
            estastitica.Cartoes_Vermelhos_Confrontos = (float)jogos.Average(j => j.Cartoes_Vermelhos);
            estastitica.Laterais_Cobrados_Confrontos = (float)jogos.Average(j => j.Laterais_Cobrados);
            estastitica.Toque_Area_Adversaria_Confrontos = (float)jogos.Average(j => j.Toque_Area_Adversaria);
            estastitica.Passes_Confrontos = (float)jogos.Average(j => j.Passes);
            estastitica.Passes_Totais_Confrontos = (float)jogos.Average(j => j.Passes_Totais);
            estastitica.Precisao_Passes_Confrontos = (float)jogos.Average(j => j.Precisao_Passes);
            estastitica.Passes_terco_Final_Confrontos = (float)jogos.Average(j => j.Passes_terco_Final);
            estastitica.Cruzamentos_Confrontos = (float)jogos.Average(j => j.Cruzamentos);
            estastitica.Desarmes_Confrontos = (float)jogos.Average(j => j.Desarmes);
            estastitica.Bolas_Afastadas_Confrontos = (float)jogos.Average(j => j.Bolas_Afastadas);
            estastitica.Interceptacoes_Confrontos = (float)jogos.Average(j => j.Interceptacoes);

            return estastitica;
        }
    }
}