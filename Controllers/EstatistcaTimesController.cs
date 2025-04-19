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
                estatisticas = await _context.TB_ESTATISTICA.Where(n => n.NomeTime.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Casa" && n.TipoPartida == "Casa").ToListAsync();
                estatisticasRival = await _context.TB_ESTATISTICA.Where(n => n.NomeTimeRival.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Fora" && n.TipoPartida == "Casa").ToListAsync();

            }
            else
            {
                estatisticasRival = await _context.TB_ESTATISTICA.Where(n => n.NomeTime.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Casa" && n.TipoPartida == "Fora").ToListAsync();
                estatisticas = await _context.TB_ESTATISTICA.Where(n => n.NomeTimeRival.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Fora" && n.TipoPartida == "Fora").ToListAsync();
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
            // Função auxiliar para calcular a média com tratamento de nulos
            float SafeAverage(IEnumerable<Estatistica> list, Func<Estatistica, float?> selector)
            {
                return (float)list.Select(selector).Average(x => x ?? 0);
            }

            Estatistica_Times estatisticas = new Estatistica_Times
            {
                Gol = SafeAverage(principal, p => p.Gol),
                GolSofrido = SafeAverage(principal, p => p.GolSofrido),
                Posse_Bola = SafeAverage(principal, p => p.Posse_Bola),
                Total_Finalizacao = SafeAverage(principal, p => p.Total_Finalizacao),
                Chances_Claras = SafeAverage(principal, p => p.Chances_Claras),
                Escanteios = SafeAverage(principal, p => p.Escanteios),
                Bolas_trave = SafeAverage(principal, p => p.Bolas_trave),
                Gols_de_cabeça = SafeAverage(principal, p => p.Gols_de_cabeça),
                Defesas_Goleiro = SafeAverage(principal, p => p.Defesas_Goleiro),
                Impedimentos = SafeAverage(principal, p => p.Impedimentos),
                Faltas = SafeAverage(principal, p => p.Faltas),
                Cartoes_Amarelos = SafeAverage(principal, p => p.Cartoes_Amarelos),
                Cartoes_Vermelhos = SafeAverage(principal, p => p.Cartoes_Vermelhos),
                Laterais_Cobrados = SafeAverage(principal, p => p.Laterais_Cobrados),
                Toque_Area_Adversaria = SafeAverage(principal, p => p.Toque_Area_Adversaria),
                Passes = SafeAverage(principal, p => p.Passes),
                Passes_Totais = SafeAverage(principal, p => p.Passes_Totais),
                Precisao_Passes = SafeAverage(principal, p => p.Precisao_Passes),
                Passes_terco_Final = SafeAverage(principal, p => p.Passes_terco_Final),
                Cruzamentos = SafeAverage(principal, p => p.Cruzamentos),
                Desarmes = SafeAverage(principal, p => p.Desarmes),
                Bolas_Afastadas = SafeAverage(principal, p => p.Bolas_Afastadas),
                Interceptacoes = SafeAverage(principal, p => p.Interceptacoes),

                Gol_Adversaria = SafeAverage(rival, r => r.Gol),
                GolSofrido_Adversaria = SafeAverage(rival, r => r.GolSofrido),
                Posse_Bola_Adversaria = SafeAverage(rival, r => r.Posse_Bola),
                Total_Finalizacao_Adversaria = SafeAverage(rival, r => r.Total_Finalizacao),
                Chances_Claras_Adversaria = SafeAverage(rival, r => r.Chances_Claras),
                Escanteios_Adversaria = SafeAverage(rival, r => r.Escanteios),
                Bolas_trave_Adversaria = SafeAverage(rival, r => r.Bolas_trave),
                Gols_de_cabeça_Adversaria = SafeAverage(rival, r => r.Gols_de_cabeça),
                Defesas_Goleiro_Adversaria = SafeAverage(rival, r => r.Defesas_Goleiro),
                Impedimentos_Adversaria = SafeAverage(rival, r => r.Impedimentos),
                Faltas_Adversaria = SafeAverage(rival, r => r.Faltas),
                Cartoes_Amarelos_Adversaria = SafeAverage(rival, r => r.Cartoes_Amarelos),
                Cartoes_Vermelhos_Adversaria = SafeAverage(rival, r => r.Cartoes_Vermelhos),
                Laterais_Cobrados_Adversaria = SafeAverage(rival, r => r.Laterais_Cobrados),
                Toque_Area_Adversaria_Adversaria = SafeAverage(rival, r => r.Toque_Area_Adversaria),
                Passes_Adversaria = SafeAverage(rival, r => r.Passes),
                Passes_Totais_Adversaria = SafeAverage(rival, r => r.Passes_Totais),
                Precisao_Passes_Adversaria = SafeAverage(rival, r => r.Precisao_Passes),
                Passes_terco_Final_Adversaria = SafeAverage(rival, r => r.Passes_terco_Final),
                Cruzamentos_Adversaria = SafeAverage(rival, r => r.Cruzamentos),
                Desarmes_Adversaria = SafeAverage(rival, r => r.Desarmes),
                Bolas_Afastadas_Adversaria = SafeAverage(rival, r => r.Bolas_Afastadas),
                Interceptacoes_Adversaria = SafeAverage(rival, r => r.Interceptacoes)
            };

            return estatisticas;
        }

        private async Task<Estatistica_Times> GerarConfrontoDireto(Estatistica_Times estastitica, string Rival)
        {


            List<Estatistica> jogos = await _context.TB_ESTATISTICA.Where(e => e.NomeTime == estastitica.NomeTime && e.TipoPartida == "Confronto Direto").ToListAsync();
            List<Estatistica> jogosFora = await _context.TB_ESTATISTICA.Where(e => e.NomeTimeRival == estastitica.NomeTime && e.TipoPartida == "Confronto Direto").ToListAsync();

            jogos.AddRange(jogosFora);

            float SafeAverage(IEnumerable<Estatistica> list, Func<Estatistica, float?> selector)
            {
                return (float)list.Select(selector).Average(x => x ?? 0);
            }


            estastitica.Gol_Confrontos = SafeAverage(jogos,j => j.Gol);
            estastitica.GolSofrido_Confrontos = SafeAverage(jogos,j => j.GolSofrido);
            estastitica.Posse_Bola_Confrontos = SafeAverage(jogos,j => j.Posse_Bola);
            estastitica.Total_Finalizacao_Confrontos = SafeAverage(jogos,j => j.Total_Finalizacao);
            estastitica.Chances_Claras_Confrontos = SafeAverage(jogos,j => j.Chances_Claras);
            estastitica.Escanteios_Confrontos = SafeAverage(jogos,j => j.Escanteios);
            estastitica.Bolas_trave_Confrontos = SafeAverage(jogos,j => j.Bolas_trave);
            estastitica.Gols_de_cabeça_Confrontos = SafeAverage(jogos,j => j.Gols_de_cabeça);
            estastitica.Defesas_Goleiro_Confrontos = SafeAverage(jogos,j => j.Defesas_Goleiro);
            estastitica.Impedimentos_Confrontos = SafeAverage(jogos,j => j.Impedimentos);
            estastitica.Faltas_Confrontos = SafeAverage(jogos,j => j.Faltas);
            estastitica.Cartoes_Amarelos_Confrontos = SafeAverage(jogos,j => j.Cartoes_Amarelos);
            estastitica.Cartoes_Vermelhos_Confrontos = SafeAverage(jogos,j => j.Cartoes_Vermelhos);
            estastitica.Laterais_Cobrados_Confrontos = SafeAverage(jogos,j => j.Laterais_Cobrados);
            estastitica.Toque_Area_Adversaria_Confrontos = SafeAverage(jogos,j => j.Toque_Area_Adversaria);
            estastitica.Passes_Confrontos = SafeAverage(jogos,j => j.Passes);
            estastitica.Passes_Totais_Confrontos = SafeAverage(jogos,j => j.Passes_Totais);
            estastitica.Precisao_Passes_Confrontos = SafeAverage(jogos,j => j.Precisao_Passes);
            estastitica.Passes_terco_Final_Confrontos = SafeAverage(jogos,j => j.Passes_terco_Final);
            estastitica.Cruzamentos_Confrontos = SafeAverage(jogos,j => j.Cruzamentos);
            estastitica.Desarmes_Confrontos = SafeAverage(jogos,j => j.Desarmes);
            estastitica.Bolas_Afastadas_Confrontos = SafeAverage(jogos,j => j.Bolas_Afastadas);
            estastitica.Interceptacoes_Confrontos = SafeAverage(jogos,j => j.Interceptacoes);

            return estastitica;
        }
    }
}