using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;
using System.Collections.Generic;
using System.Transactions;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class EstatisticaEsperadasController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MLDbContext _MLDb;

        public EstatisticaEsperadasController(DataContext context, MLDbContext mLDb)
        {
            _context = context;
            _MLDb = mLDb;
        }



        [HttpGet("id")]
        public async Task<IActionResult> getId(int id)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("Estatistica Não Encontrada");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Estatistica_Esperadas e = await _MLDb.TB_ESTATISTICA_ESPERADAS
                .Include(e => e.FT)
                .Include(e => e.FT_Adversario)
                .Include(e => e.FT_Confronto)
                .Include(e => e.HT)
                .Include(e => e.HT_Adversario)
                .Include(e => e.HT_Confronto)
                .FirstOrDefaultAsync(es => es.Id == id);


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
                List<Estatistica_Esperadas> estatisticas = await _context
                .TB_ESTATISTICA_ESPERADAS
                .Include(e => e.FT)
                .Include(e => e.FT_Adversario)
                .Include(e => e.FT_Confronto)
                .Include(e => e.HT)
                .Include(e => e.HT_Adversario)
                .Include(e => e.HT_Confronto)
                .ToListAsync();

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
                List<Estatistica_Esperadas> estatisticas = await _MLDb
                .TB_ESTATISTICA_ESPERADAS
                .Include(e => e.FT)
                .Include(e => e.FT_Adversario)
                .Include(e => e.FT_Confronto)
                .Include(e => e.HT)
                .Include(e => e.HT_Adversario)
                .Include(e => e.HT_Confronto)
                .ToListAsync();

                return Ok(estatisticas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Estatistica_Esperadas e)
        {

            try
            {
                await _MLDb.TB_ESTATISTICA_ESPERADAS.AddAsync(e);
                await _MLDb.SaveChangesAsync();
                int idPrincipal = e.Id;

                e.Id = idPrincipal;
                _MLDb.Entry(e).State = EntityState.Detached;
                _context.Entry(e).State = EntityState.Added;
                await _context.SaveChangesAsync();

                int idSecundario = e.Id;

                return Ok(new { PrimaryId = idPrincipal, SecondaryId = idSecundario });
            }
            catch (System.Exception ex)
            {
                try
                {
                    var inserted = await _MLDb.TB_ESTATISTICA_ESPERADAS.FindAsync(e.Id);
                    if (inserted != null)
                    {
                        _MLDb.TB_ESTATISTICA_ESPERADAS.Remove(inserted);
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

        [HttpPost("GerarEstatisticasEsperadas/{IdPartida}")]
        public async Task<IActionResult> GerarEstatistcasEsperadas(int IdPartida)
        {
            Estatistica_Esperadas c = null; // declara aqui fora pra usar no catch
            Estatistica_Esperadas f = null; // declara aqui fora pra usar no catch

            try
            {
                Partida partida = await _context.TB_PARTIDAS
                .FirstOrDefaultAsync(p => p.Id == IdPartida && p.PartidaAnalise == true);

                if (partida == null)
                    throw new System.Exception("Não foi encontrada partida Analise com esse Id");

                c = await BuscarPartidas(partida, partida.NomeTimeCasa);
                f = await BuscarPartidas(partida, partida.NomeTimeFora);

                c.NomeTime = partida.NomeTimeCasa;
                f.NomeTime = partida.NomeTimeFora;

                await _MLDb.TB_ESTATISTICA_ESPERADAS.AddRangeAsync(c, f);
                await _MLDb.SaveChangesAsync();


                _MLDb.Entry(c).State = EntityState.Detached;
                _MLDb.Entry(f).State = EntityState.Detached;
                _context.Entry(c).State = EntityState.Added;
                _context.Entry(f).State = EntityState.Added;
                await _context.SaveChangesAsync();

                string mensagem = $"foi gerado Estatisticas esperadas corretamente o ID de casa é{c.Id} ,  o ID de Fora é {f.Id} ";
                return Ok(mensagem);

            }
            catch (System.Exception ex)
            {
                if (c != null)
                {
                    try
                    {
                        var inserted = await _MLDb.TB_ESTATISTICA_ESPERADAS.FindAsync(c.Id);
                        if (inserted != null)
                        {
                            _MLDb.TB_ESTATISTICA_ESPERADAS.Remove(inserted);
                            await _MLDb.SaveChangesAsync();
                        }
                    }
                    catch (Exception rollbackEx)
                    {
                        return BadRequest($"Erro ao inserir estatística: {ex.Message} | Erro ao desfazer no primeiro banco: {rollbackEx.Message}");
                    }
                }
                if (f != null)
                {
                    try
                    {
                        var inserted = await _MLDb.TB_ESTATISTICA_ESPERADAS.FindAsync(c.Id);
                        if (inserted != null)
                        {
                            _MLDb.TB_ESTATISTICA_ESPERADAS.Remove(inserted);
                            await _MLDb.SaveChangesAsync();
                        }
                    }
                    catch (Exception rollbackEx)
                    {
                        return BadRequest($"Erro ao inserir estatística: {ex.Message} | Erro ao desfazer no primeiro banco: {rollbackEx.Message}");
                    }
                }


                return BadRequest($"Erro ao inserir estatística: {ex.Message}. Inserção no primeiro banco foi revertida com sucesso.");

            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Estatistica_Esperadas e)
        {
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Estatistica_Esperadas estatistica = await _context.TB_ESTATISTICA_ESPERADAS
                .FirstOrDefaultAsync(es => es.Id == id);

                Estatistica_Esperadas e_repeat = await _context.TB_ESTATISTICA_ESPERADAS
                .FirstOrDefaultAsync(es => es.Id == id);

                if (estatistica == null || e_repeat == null)
                    throw new System.Exception("Estatistica Não Encontrada");

                _MLDb.Entry(estatistica).CurrentValues.SetValues(e);
                _context.Entry(e_repeat).CurrentValues.SetValues(e);


                await _MLDb.SaveChangesAsync();

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _MLDb.Entry(estatistica).State = EntityState.Unchanged;
                    return BadRequest($"Erro ao salvar no segundo banco: {ex.Message}");
                }

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
            try
            {
                if (id == 0)
                    throw new System.Exception("O Id não pode ser igual a zero");

                Estatistica_Esperadas e = await _context.TB_ESTATISTICA_ESPERADAS
                .FirstOrDefaultAsync(es => es.Id == id);

                Estatistica_Esperadas e_repeat = await _context.TB_ESTATISTICA_ESPERADAS
                .FirstOrDefaultAsync(es => es.Id == id);

                if (e == null || e_repeat == null)
                    throw new System.Exception("Estatistica Não Encontrada");

                _MLDb.TB_ESTATISTICA_ESPERADAS.Remove(e);
                _context.TB_ESTATISTICA_ESPERADAS.Remove(e_repeat);
                
                await _context.SaveChangesAsync();
                int linhasAfetadas = await _MLDb.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


        private async Task<Estatistica_Esperadas> BuscarPartidas(Partida partidaAnalisada, string nomeTime)
        {
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
            Estatistica_Esperadas time = new Estatistica_Esperadas();
            string rival = "";
            time.NomeTime = nomeTime;
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

            List<Estatistica> confrontos = await _context.TB_ESTATISTICA.Where(e => e.NomeTime == time.NomeTime && e.TipoPartida == "Confronto Direto").ToListAsync();
            List<Estatistica> confrontosFora = await _context.TB_ESTATISTICA.Where(e => e.NomeTimeRival == time.NomeTime && e.TipoPartida == "Confronto Direto").ToListAsync();
            confrontos.AddRange(confrontosFora);


            time = await GerarEstatisticasByTime(estatisticas, estatisticasRival, confrontos);

            return time;
        }


        private async Task<Estatistica_Esperadas> GerarEstatisticasByTime(List<Estatistica> principal, List<Estatistica> rival, List<Estatistica> confronto)
        {
            List<Estatistica_BaseModel> estatisticas_Base = new List<Estatistica_BaseModel>();

            Estatistica_Esperadas estatisticas = new Estatistica_Esperadas
            {
                FT = CalcularMetricasTime(principal),
                HT = CalcularMetricasTime(principal, "HT"),
                FT_Adversario = CalcularMetricasTime(rival),
                HT_Adversario = CalcularMetricasTime(rival, "HT"),
                FT_Confronto = CalcularMetricasTime(confronto),
                HT_Confronto = CalcularMetricasTime(confronto, "HT"),
            };
            estatisticas_Base.AddRange(estatisticas.FT, estatisticas.HT);
            estatisticas_Base.AddRange(estatisticas.FT_Adversario, estatisticas.HT_Adversario);
            estatisticas_Base.AddRange(estatisticas.FT_Confronto, estatisticas.HT_Confronto);

            await _context.TB_ESTATISTICA_ESPERADAS.AddRangeAsync((IEnumerable<Estatistica_Esperadas>)estatisticas_Base);
            await _MLDb.TB_ESTATISTICA_ESPERADAS.AddRangeAsync((IEnumerable<Estatistica_Esperadas>)estatisticas_Base);
            await _MLDb.SaveChangesAsync();
            await _context.SaveChangesAsync();

            return estatisticas;
        }
        private static float SafeAverage(IEnumerable<Estatistica> list, Func<Estatistica, float?> selector)
        {
            return (float)list.Select(selector).Average(x => x ?? 0);
        }

        private static float SafeTrend(IEnumerable<Estatistica> list, Func<Estatistica, float?> selector)
        {
            // Pega os valores válidos e mantém a ordem original (assumindo que já está ordenado)
            var valores = list
                .Select(item => selector(item))
                .Where(valor => valor.HasValue)
                .Select(valor => valor.Value)
                .ToList();

            if (valores.Count < 2)
                return 0; // Não há dados suficientes para calcular tendência

            // Usa o índice como X (tempo: 1, 2, 3, ...)
            float n = valores.Count;
            float sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            for (int i = 0; i < valores.Count; i++)
            {
                float x = i + 1; // X = posição (1, 2, 3, ...)
                float y = valores[i]; // Y = valor (gols, etc.)

                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            // Fórmula do slope (inclinação da regressão linear)
            float slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);

            return slope;
        }

        private static float SafeStandardDeviation(IEnumerable<Estatistica> list, Func<Estatistica, float?> selector)
        {
            var values = list.Select(selector).Where(x => x.HasValue).Select(x => x.Value).ToList();

            if (values.Count == 0)
                return 0;

            float average = values.Average();
            float sumOfSquares = values.Sum(x => (x - average) * (x - average));
            float variance = sumOfSquares / values.Count;

            return (float)Math.Sqrt(variance);
        }


        private Estatistica_BaseModel CalcularMetricasTime(IEnumerable<Estatistica> stats, string suffix = "")
        {
            return new Estatistica_BaseModel
            {
                Gol = SafeAverage(stats, p => GetProp(p, "Gol", suffix)),
                Gol_DP = SafeStandardDeviation(stats, p => GetProp(p, "Gol", suffix)),
                Gol_Slope = SafeTrend(stats, p => GetProp(p, "Gol", suffix)),

                GolSofrido = SafeAverage(stats, p => GetProp(p, "GolSofrido", suffix)),
                GolSofrido_DP = SafeStandardDeviation(stats, p => GetProp(p, "GolSofrido", suffix)),
                GolSofrido_Slope = SafeTrend(stats, p => GetProp(p, "GolSofrido", suffix)),

                Posse_Bola = SafeAverage(stats, p => GetProp(p, "Posse_Bola", suffix)),
                Posse_Bola_DP = SafeStandardDeviation(stats, p => GetProp(p, "Posse_Bola", suffix)),
                Posse_Bola_Slope = SafeTrend(stats, p => GetProp(p, "Posse_Bola", suffix)),

                Total_Finalizacao = SafeAverage(stats, p => GetProp(p, "Total_Finalizacao", suffix)),
                Total_Finalizacao_DP = SafeStandardDeviation(stats, p => GetProp(p, "Total_Finalizacao", suffix)),
                Total_Finalizacao_Slope = SafeTrend(stats, p => GetProp(p, "Total_Finalizacao", suffix)),

                Escanteios = SafeAverage(stats, p => GetProp(p, "Escanteios", suffix)),
                Escanteios_DP = SafeStandardDeviation(stats, p => GetProp(p, "Escanteios", suffix)),
                Escanteios_Slope = SafeTrend(stats, p => GetProp(p, "Escanteios", suffix)),

                Chances_Claras = SafeAverage(stats, p => GetProp(p, "Chances_Claras", suffix)),
                Bolas_trave = SafeAverage(stats, p => GetProp(p, "Bolas_trave", suffix)),
                Gols_de_cabeca = SafeAverage(stats, p => GetProp(p, "Gols_de_cabeça", suffix)),
                Defesas_Goleiro = SafeAverage(stats, p => GetProp(p, "Defesas_Goleiro", suffix)),

                Impedimentos = SafeAverage(stats, p => GetProp(p, "Impedimentos", suffix)),
                Impedimentos_DP = SafeStandardDeviation(stats, p => GetProp(p, "Impedimentos", suffix)),
                Impedimentos_Slope = SafeTrend(stats, p => GetProp(p, "Impedimentos", suffix)),

                Faltas = SafeAverage(stats, p => GetProp(p, "Faltas", suffix)),
                Faltas_DP = SafeStandardDeviation(stats, p => GetProp(p, "Faltas", suffix)),
                Faltas_Slope = SafeTrend(stats, p => GetProp(p, "Faltas", suffix)),

                Cartoes_Amarelos = SafeAverage(stats, p => GetProp(p, "Cartoes_Amarelos", suffix)),
                Cartoes_Amarelos_DP = SafeStandardDeviation(stats, p => GetProp(p, "Cartoes_Amarelos", suffix)),
                Cartoes_Amarelos_Slope = SafeTrend(stats, p => GetProp(p, "Cartoes_Amarelos", suffix)),

                Cartoes_Vermelhos = SafeAverage(stats, p => GetProp(p, "Cartoes_Vermelhos", suffix)),
                Cartoes_Vermelhos_DP = SafeStandardDeviation(stats, p => GetProp(p, "Cartoes_Vermelhos", suffix)),
                Cartoes_Vermelhos_Slope = SafeTrend(stats, p => GetProp(p, "Cartoes_Vermelhos", suffix)),

                Laterais_Cobrados = SafeAverage(stats, p => GetProp(p, "Laterais_Cobrados", suffix)),
                Toque_Area_Adversaria = SafeAverage(stats, p => GetProp(p, "Toque_Area_Adversaria", suffix)),

                Passes = SafeAverage(stats, p => GetProp(p, "Passes", suffix)),
                Passes_Totais = SafeAverage(stats, p => GetProp(p, "Passes_Totais", suffix)),
                Precisao_Passes = SafeAverage(stats, p => GetProp(p, "Precisao_Passes", suffix)),
                Passes_terco_Final = SafeAverage(stats, p => GetProp(p, "Passes_terco_Final", suffix)),

                Cruzamentos = SafeAverage(stats, p => GetProp(p, "Cruzamentos", suffix)),

                Desarmes = SafeAverage(stats, p => GetProp(p, "Desarmes", suffix)),
                Desarmes_DP = SafeStandardDeviation(stats, p => GetProp(p, "Desarmes", suffix)),
                Desarmes_Slope = SafeTrend(stats, p => GetProp(p, "Desarmes", suffix)),

                Bolas_Afastadas = SafeAverage(stats, p => GetProp(p, "Bolas_Afastadas", suffix)),
                Interceptacoes = SafeAverage(stats, p => GetProp(p, "Interceptacoes", suffix))
            };
        }

        private static float? GetProp(Estatistica item, string propName, string suffix)
        {
            var fullName = string.IsNullOrEmpty(suffix) ? propName : $"{propName}_{suffix}";
            return fullName switch
            {
                // Gols
                "Gol" => item.Gol,
                "Gol_HT" => item.Gol_HT,
                "GolSofrido" => item.GolSofrido,
                "GolSofrido_HT" => item.GolSofrido_HT,

                // Posse
                "Posse_Bola" => item.Posse_Bola,
                "Posse_Bola_HT" => item.Posse_Bola_HT,

                // Finalizações
                "Total_Finalizacao" => item.Total_Finalizacao,
                "Total_Finalizacao_HT" => item.Total_Finalizacao_HT,

                // Escanteios
                "Escanteios" => item.Escanteios,
                "Escanteios_HT" => item.Escanteios_HT,

                // Chances claras, bolas na trave, gols de cabeça, defesas
                "Chances_Claras" => item.Chances_Claras,
                "Chances_Claras_HT" => item.Chances_Claras_HT,
                "Bolas_trave" => item.Bolas_trave,
                "Bolas_trave_HT" => item.Bolas_trave_HT,
                "Gols_de_cabeça" => item.Gols_de_cabeça,
                "Gols_de_cabeça_HT" => item.Gols_de_cabeça_HT,
                "Defesas_Goleiro" => item.Defesas_Goleiro,
                "Defesas_Goleiro_HT" => item.Defesas_Goleiro_HT,

                // Impedimentos
                "Impedimentos" => item.Impedimentos,
                "Impedimentos_HT" => item.Impedimentos_HT,

                // Faltas
                "Faltas" => item.Faltas,
                "Faltas_HT" => item.Faltas_HT,

                // Cartões
                "Cartoes_Amarelos" => item.Cartoes_Amarelos,
                "Cartoes_Amarelos_HT" => item.Cartoes_Amarelos_HT,
                "Cartoes_Vermelhos" => item.Cartoes_Vermelhos,
                "Cartoes_Vermelhos_HT" => item.Cartoes_Vermelhos_HT,

                // Laterais e toques na área
                "Laterais_Cobrados" => item.Laterais_Cobrados,
                "Laterais_Cobrados_HT" => item.Laterais_Cobrados_HT,
                "Toque_Area_Adversaria" => item.Toque_Area_Adversaria,
                "Toque_Area_Adversaria_HT" => item.Toque_Area_Adversaria_HT,

                // Passes
                "Passes" => item.Passes,
                "Passes_HT" => item.Passes_HT,
                "Passes_Totais" => item.Passes_Totais,
                "Passes_Totais_HT" => item.Passes_Totais_HT,
                "Precisao_Passes" => item.Precisao_Passes,
                "Precisao_Passes_HT" => item.Precisao_Passes_HT,
                "Passes_terco_Final" => item.Passes_terco_Final,
                "Passes_terco_Final_HT" => item.Passes_terco_Final_HT,

                // Cruzamentos
                "Cruzamentos" => item.Cruzamentos,
                "Cruzamentos_HT" => item.Cruzamentos_HT,

                // Desarmes
                "Desarmes" => item.Desarmes,
                "Desarmes_HT" => item.Desarmes_HT,

                // Defensivos
                "Bolas_Afastadas" => item.Bolas_Afastadas,
                "Bolas_Afastadas_HT" => item.Bolas_Afastadas_HT,
                "Interceptacoes" => item.Interceptacoes,
                "Interceptacoes_HT" => item.Interceptacoes_HT,

                _ => null
            };
        }


    }
}