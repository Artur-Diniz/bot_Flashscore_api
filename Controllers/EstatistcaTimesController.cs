using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstatisticaTimesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MLDbContext _MLDb;

        public EstatisticaTimesController(DataContext context, MLDbContext mLDb)
        {
            _context = context;
            _MLDb = mLDb;
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
            // Habilita transação distribuída (se os bancos suportarem)
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                // 1. Insere no banco PRINCIPAL (_MLDb)
                await _MLDb.TB_ESTATISTICA_TIME.AddAsync(e);
                await _MLDb.SaveChangesAsync();
                int idPrincipal = e.Id;

                // 2. Desanexa para evitar conflitos
                _MLDb.Entry(e).State = EntityState.Detached;

                // 3. Insere no banco SECUNDÁRIO (_context)
                _context.TB_ESTATISTICA_TIME.Add(e);
                await _context.SaveChangesAsync();
                int idSecundario = e.Id;

                // Confirma a transação em AMBOS os bancos
                scope.Complete();

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idSecundario });
            }
            catch (Exception ex)
            {
                // Rollback automático (se configurado corretamente)
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("GerarEstatistica/{nomeTime}")]
        public async Task<IActionResult> GerarEstatisticaMedia(string nomeTime)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                Partida partidaAnalisada = await _context.TB_PARTIDAS
                .FirstOrDefaultAsync(p => p.PartidaAnalise == true && p.NomeTimeCasa.ToLower().Contains(nomeTime.ToLower()) ||
                 p.PartidaAnalise == true && p.NomeTimeFora.ToLower().Contains(nomeTime.ToLower()));

                if (partidaAnalisada == null)
                    throw new System.Exception("não encontrado time com esse Nome");

                Estatistica_Times e = await Buscarpartidas(nomeTime);

                await _MLDb.TB_ESTATISTICA_TIME.AddAsync(e);
                await _MLDb.SaveChangesAsync();
                int idPrincipal = e.Id;


                _context.TB_ESTATISTICA_TIME.Add(e);
                await _context.SaveChangesAsync();

                int idSecundario = e.Id;

                scope.Complete();

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idSecundario });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Estatistica_Times e)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                // Atualiza no primeiro banco
                _MLDb.TB_ESTATISTICA_TIME.Update(e);
                await _MLDb.SaveChangesAsync();

                // Desanexa a entidade do primeiro contexto
                _MLDb.Entry(e).State = EntityState.Detached;

                // Atualiza no segundo banco
                _context.TB_ESTATISTICA_TIME.Update(e);
                await _context.SaveChangesAsync();


                return Ok(new { Message = "Atualização realizada com sucesso em ambos os bancos" });
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

                Estatistica_Times e = await _MLDb.TB_ESTATISTICA_TIME
                .FirstOrDefaultAsync(es => es.Id == id);
                Estatistica_Times e_repeat = await _context.TB_ESTATISTICA_TIME
                .FirstOrDefaultAsync(es => es.Id == id);

                if (e == null || e_repeat == null)
                    throw new System.Exception("Estatistica Não Encontrada");

                _context.TB_ESTATISTICA_TIME.Remove(e_repeat);
                _MLDb.TB_ESTATISTICA_TIME.Remove(e);

                await _MLDb.SaveChangesAsync();
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
                estatisticas = await _context.TB_ESTATISTICA.Where(n => n.NomeTimeRival.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Casa" && n.TipoPartida == "Fora").ToListAsync();
                estatisticasRival = await _context.TB_ESTATISTICA.Where(n => n.NomeTime.ToLower().Contains(nomeTime.ToLower()) && n.CasaOuFora == "Fora" && n.TipoPartida == "Fora").ToListAsync();
            }
            if (estatisticasRival.Count() == 0 || estatisticas.Count() == 0)
                throw new System.Exception("Não achei time com esse nome ai");

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
                Interceptacoes_Adversaria = SafeAverage(rival, r => r.Interceptacoes),

                Gol_HT = SafeAverage(principal, p => p.Gol_HT),
                GolSofrido_HT = SafeAverage(principal, p => p.GolSofrido_HT),
                Posse_Bola_HT = SafeAverage(principal, p => p.Posse_Bola_HT),
                Total_Finalizacao_HT = SafeAverage(principal, p => p.Total_Finalizacao_HT),
                Chances_Claras_HT = SafeAverage(principal, p => p.Chances_Claras_HT),
                Escanteios_HT = SafeAverage(principal, p => p.Escanteios_HT),
                Bolas_trave_HT = SafeAverage(principal, p => p.Bolas_trave_HT),
                Gols_de_cabeça_HT = SafeAverage(principal, p => p.Gols_de_cabeça_HT),
                Defesas_Goleiro_HT = SafeAverage(principal, p => p.Defesas_Goleiro_HT),
                Impedimentos_HT = SafeAverage(principal, p => p.Impedimentos_HT),
                Faltas_HT = SafeAverage(principal, p => p.Faltas_HT),
                Cartoes_Amarelos_HT = SafeAverage(principal, p => p.Cartoes_Amarelos_HT),
                Cartoes_Vermelhos_HT = SafeAverage(principal, p => p.Cartoes_Vermelhos_HT),
                Laterais_Cobrados_HT = SafeAverage(principal, p => p.Laterais_Cobrados_HT),
                Toque_Area_Adversaria_HT = SafeAverage(principal, p => p.Toque_Area_Adversaria_HT),
                Passes_HT = SafeAverage(principal, p => p.Passes_HT),
                Passes_Totais_HT = SafeAverage(principal, p => p.Passes_Totais_HT),
                Precisao_Passes_HT = SafeAverage(principal, p => p.Precisao_Passes_HT),
                Passes_terco_Final_HT = SafeAverage(principal, p => p.Passes_terco_Final_HT),
                Cruzamentos_HT = SafeAverage(principal, p => p.Cruzamentos_HT),
                Desarmes_HT = SafeAverage(principal, p => p.Desarmes_HT),
                Bolas_Afastadas_HT = SafeAverage(principal, p => p.Bolas_Afastadas_HT),
                Interceptacoes_HT = SafeAverage(principal, p => p.Interceptacoes_HT),

                Gol_Adversaria_HT = SafeAverage(rival, r => r.Gol_HT),
                GolSofrido_Adversaria_HT = SafeAverage(rival, r => r.GolSofrido_HT),
                Posse_Bola_Adversaria_HT = SafeAverage(rival, r => r.Posse_Bola_HT),
                Total_Finalizacao_Adversaria_HT = SafeAverage(rival, r => r.Total_Finalizacao_HT),
                Chances_Claras_Adversaria_HT = SafeAverage(rival, r => r.Chances_Claras_HT),
                Escanteios_Adversaria_HT = SafeAverage(rival, r => r.Escanteios_HT),
                Bolas_trave_Adversaria_HT = SafeAverage(rival, r => r.Bolas_trave_HT),
                Gols_de_cabeça_Adversaria_HT = SafeAverage(rival, r => r.Gols_de_cabeça_HT),
                Defesas_Goleiro_Adversaria_HT = SafeAverage(rival, r => r.Defesas_Goleiro_HT),
                Impedimentos_Adversaria_HT = SafeAverage(rival, r => r.Impedimentos_HT),
                Faltas_Adversaria_HT = SafeAverage(rival, r => r.Faltas_HT),
                Cartoes_Amarelos_Adversaria_HT = SafeAverage(rival, r => r.Cartoes_Amarelos_HT),
                Cartoes_Vermelhos_Adversaria_HT = SafeAverage(rival, r => r.Cartoes_Vermelhos_HT),
                Laterais_Cobrados_Adversaria_HT = SafeAverage(rival, r => r.Laterais_Cobrados_HT),
                Toque_Area_Adversaria_Adversaria_HT = SafeAverage(rival, r => r.Toque_Area_Adversaria_HT),
                Passes_Adversaria_HT = SafeAverage(rival, r => r.Passes_HT),
                Passes_Totais_Adversaria_HT = SafeAverage(rival, r => r.Passes_Totais_HT),
                Precisao_Passes_Adversaria_HT = SafeAverage(rival, r => r.Precisao_Passes_HT),
                Passes_terco_Final_Adversaria_HT = SafeAverage(rival, r => r.Passes_terco_Final_HT),
                Cruzamentos_Adversaria_HT = SafeAverage(rival, r => r.Cruzamentos_HT),
                Desarmes_Adversaria_HT = SafeAverage(rival, r => r.Desarmes_HT),
                Bolas_Afastadas_Adversaria_HT = SafeAverage(rival, r => r.Bolas_Afastadas_HT),
                Interceptacoes_Adversaria_HT = SafeAverage(rival, r => r.Interceptacoes_HT)
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


            estastitica.Gol_Confrontos = SafeAverage(jogos, j => j.Gol);
            estastitica.Gol_Confrontos_HT = SafeAverage(jogos, j => j.Gol_HT);
            estastitica.GolSofrido_Confrontos = SafeAverage(jogos, j => j.GolSofrido);
            estastitica.GolSofrido_Confrontos_HT = SafeAverage(jogos, j => j.GolSofrido_HT);
            estastitica.Posse_Bola_Confrontos = SafeAverage(jogos, j => j.Posse_Bola);
            estastitica.Posse_Bola_Confrontos_HT = SafeAverage(jogos, j => j.Posse_Bola_HT);
            estastitica.Total_Finalizacao_Confrontos = SafeAverage(jogos, j => j.Total_Finalizacao);
            estastitica.Total_Finalizacao_Confrontos_HT = SafeAverage(jogos, j => j.Total_Finalizacao_HT);
            estastitica.Chances_Claras_Confrontos = SafeAverage(jogos, j => j.Chances_Claras);
            estastitica.Chances_Claras_Confrontos_HT = SafeAverage(jogos, j => j.Chances_Claras_HT);
            estastitica.Escanteios_Confrontos = SafeAverage(jogos, j => j.Escanteios);
            estastitica.Escanteios_Confrontos_HT = SafeAverage(jogos, j => j.Escanteios_HT);
            estastitica.Bolas_trave_Confrontos = SafeAverage(jogos, j => j.Bolas_trave);
            estastitica.Bolas_trave_Confrontos_HT = SafeAverage(jogos, j => j.Bolas_trave_HT);
            estastitica.Gols_de_cabeça_Confrontos = SafeAverage(jogos, j => j.Gols_de_cabeça);
            estastitica.Gols_de_cabeça_Confrontos_HT = SafeAverage(jogos, j => j.Gols_de_cabeça_HT);
            estastitica.Defesas_Goleiro_Confrontos = SafeAverage(jogos, j => j.Defesas_Goleiro);
            estastitica.Defesas_Goleiro_Confrontos_HT = SafeAverage(jogos, j => j.Defesas_Goleiro_HT);
            estastitica.Impedimentos_Confrontos = SafeAverage(jogos, j => j.Impedimentos);
            estastitica.Impedimentos_Confrontos_HT = SafeAverage(jogos, j => j.Impedimentos_HT);
            estastitica.Faltas_Confrontos = SafeAverage(jogos, j => j.Faltas);
            estastitica.Faltas_Confrontos_HT = SafeAverage(jogos, j => j.Faltas_HT);
            estastitica.Cartoes_Amarelos_Confrontos = SafeAverage(jogos, j => j.Cartoes_Amarelos);
            estastitica.Cartoes_Amarelos_Confrontos_HT = SafeAverage(jogos, j => j.Cartoes_Amarelos_HT);
            estastitica.Cartoes_Vermelhos_Confrontos = SafeAverage(jogos, j => j.Cartoes_Vermelhos);
            estastitica.Cartoes_Vermelhos_Confrontos_HT = SafeAverage(jogos, j => j.Cartoes_Vermelhos_HT);
            estastitica.Laterais_Cobrados_Confrontos = SafeAverage(jogos, j => j.Laterais_Cobrados);
            estastitica.Laterais_Cobrados_Confrontos_HT = SafeAverage(jogos, j => j.Laterais_Cobrados_HT);
            estastitica.Toque_Area_Adversaria_Confrontos = SafeAverage(jogos, j => j.Toque_Area_Adversaria);
            estastitica.Toque_Area_Adversaria_Confrontos_HT = SafeAverage(jogos, j => j.Toque_Area_Adversaria_HT);
            estastitica.Passes_Confrontos = SafeAverage(jogos, j => j.Passes);
            estastitica.Passes_Confrontos_HT = SafeAverage(jogos, j => j.Passes_HT);
            estastitica.Passes_Totais_Confrontos = SafeAverage(jogos, j => j.Passes_Totais);
            estastitica.Passes_Totais_Confrontos_HT = SafeAverage(jogos, j => j.Passes_Totais_HT);
            estastitica.Precisao_Passes_Confrontos = SafeAverage(jogos, j => j.Precisao_Passes);
            estastitica.Precisao_Passes_Confrontos_HT = SafeAverage(jogos, j => j.Precisao_Passes_HT);
            estastitica.Passes_terco_Final_Confrontos = SafeAverage(jogos, j => j.Passes_terco_Final);
            estastitica.Passes_terco_Final_Confrontos_HT = SafeAverage(jogos, j => j.Passes_terco_Final_HT);
            estastitica.Cruzamentos_Confrontos = SafeAverage(jogos, j => j.Cruzamentos);
            estastitica.Cruzamentos_Confrontos_HT = SafeAverage(jogos, j => j.Cruzamentos_HT);
            estastitica.Desarmes_Confrontos = SafeAverage(jogos, j => j.Desarmes);
            estastitica.Desarmes_Confrontos_HT = SafeAverage(jogos, j => j.Desarmes_HT);
            estastitica.Bolas_Afastadas_Confrontos = SafeAverage(jogos, j => j.Bolas_Afastadas);
            estastitica.Bolas_Afastadas_Confrontos_HT = SafeAverage(jogos, j => j.Bolas_Afastadas_HT);
            estastitica.Interceptacoes_Confrontos = SafeAverage(jogos, j => j.Interceptacoes);
            estastitica.Interceptacoes_Confrontos_HT = SafeAverage(jogos, j => j.Interceptacoes_HT);

            return estastitica;
        }
    }
}