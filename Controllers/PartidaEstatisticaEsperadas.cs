using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using botAPI.Models;
using botAPI.Data;
using Npgsql;
using Dapper;

namespace botAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PartidaEstatisticaEsperadasController : ControllerBase
    {
        private readonly DataContext _context;

        public PartidaEstatisticaEsperadasController(DataContext context)
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
                Partida_Estatistica_Esperadas p = await _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS
                .FirstOrDefaultAsync(pa => pa.Id == id);
                if (p == null)
                    throw new System.Exception("Partida  Estatistica Esperdas Não Encontrada");
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
                List<Partida_Estatistica_Esperadas> partidas = await _context
                .TB_PARTIDA_ESTAITSTICA_ESPERADAS.ToListAsync();
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
        public async Task<IActionResult> Post(Partida_Estatistica_Esperadas partida)
        {
            try
            {
                await _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.AddAsync(partida);
                await _context.SaveChangesAsync();

                return Ok(partida.Id);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("GerarEstatisticasEsperadas/{IdPartida}")]
        public async Task<IActionResult> GerarPartidasEstatisticasEsperadas(int IdPartida)
        {
            try
            {
                // 1. Obter a partida do banco principal
                var partidaAnalisada = await _context.TB_PARTIDAS

                    .FirstOrDefaultAsync(p => p.Id == IdPartida && p.PartidaAnalise == true);

                if (partidaAnalisada == null)
                    return NotFound("Partida não encontrada ou não marcada para análise");

                // 2. Configurar conexão com Supabase
                string connectionString = "Host=db.gvabzwwvqjxnezjxuabe.supabase.co;Port=5432;Username=postgres;Password=gytw rgmj rexc ocax;Database=postgres;SSL Mode=Require;Trust Server Certificate=true;Include Error Detail=true;";

                using var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();

                using var transaction = await conn.BeginTransactionAsync();

                try
                {
                    // 3. Criar estatísticas base vazias
                    var insertEstatisticaVazia = "INSERT INTO estatistica_basemodel DEFAULT VALUES RETURNING id;";
                    int idEstatisticaCasaVazia = await conn.QuerySingleAsync<int>(insertEstatisticaVazia, null, transaction);
                    int idEstatisticaForaVazia = await conn.QuerySingleAsync<int>(insertEstatisticaVazia, null, transaction);

                    // 4. Inserir/atualizar partida
                    var insertPartida = @"
                INSERT INTO tb_partidas 
                (id, id_estatisticacasa, id_estatisticafora, nome_time_casa, nome_time_fora, 
                 data_partida, campeonato, partida_analise, tipo_partida, url_partida) 
                VALUES 
                (@Id, @IdEstatisticaCasa, @IdEstatisticaFora, @NomeTimeCasa, @NomeTimeFora, 
                 @DataPartida, @Campeonato, @PartidaAnalise, @TipoPartida, @UrlPartida)
                ON CONFLICT (id) DO UPDATE SET
                nome_time_casa = EXCLUDED.nome_time_casa,
                nome_time_fora = EXCLUDED.nome_time_fora,
                data_partida = EXCLUDED.data_partida,
                campeonato = EXCLUDED.campeonato,
                partida_analise = EXCLUDED.partida_analise,
                tipo_partida = EXCLUDED.tipo_partida,
                url_partida = EXCLUDED.url_partida;";

                    await conn.ExecuteAsync(insertPartida, new
                    {
                        partidaAnalisada.Id,
                        IdEstatisticaCasa = idEstatisticaCasaVazia,
                        IdEstatisticaFora = idEstatisticaForaVazia,
                        partidaAnalisada.NomeTimeCasa,
                        partidaAnalisada.NomeTimeFora,
                        partidaAnalisada.DataPartida,
                        partidaAnalisada.Campeonato,
                        partidaAnalisada.PartidaAnalise,
                        partidaAnalisada.TipoPartida,
                        UrlPartida = partidaAnalisada.Url_Partida ?? string.Empty
                    }, transaction);

                    // 5. Gerar estatísticas esperadas
                    Partida_Estatistica_Esperadas partidaEstatisticas = await GerarPartidaEstatisticaEsperada(partidaAnalisada);

                    // 6. Inserir estatísticas esperadas
                    var insertEstatisticaEsperada = @"
                INSERT INTO tb_estatistica_esperadas 
                (nome_time, ftid, htid, ft_adversarioid, ht_adversarioid, 
                 ft_confrontoid, ht_confrontoid, is_previsao) 
                VALUES 
                (@NomeTime, @FtId, @HtId, @FtAdversarioId, @HtAdversarioId, 
                 @FtConfrontoId, @HtConfrontoId, true)
                RETURNING id;";

                    int idEstatisticaEsperadaCasa = await conn.QuerySingleAsync<int>(insertEstatisticaEsperada, new
                    {
                        NomeTime = partidaAnalisada.NomeTimeCasa,
                        FtId = idEstatisticaCasaVazia,
                        HtId = idEstatisticaCasaVazia,
                        FtAdversarioId = idEstatisticaForaVazia,
                        HtAdversarioId = idEstatisticaForaVazia,
                        FtConfrontoId = idEstatisticaCasaVazia,
                        HtConfrontoId = idEstatisticaCasaVazia
                    }, transaction);

                    int idEstatisticaEsperadaFora = await conn.QuerySingleAsync<int>(insertEstatisticaEsperada, new
                    {
                        NomeTime = partidaAnalisada.NomeTimeFora,
                        FtId = idEstatisticaForaVazia,
                        HtId = idEstatisticaForaVazia,
                        FtAdversarioId = idEstatisticaCasaVazia,
                        HtAdversarioId = idEstatisticaCasaVazia,
                        FtConfrontoId = idEstatisticaForaVazia,
                        HtConfrontoId = idEstatisticaForaVazia
                    }, transaction);

                    // 7. Vincular estatísticas à partida
                    var insertPartidaEstatisticas = @"
                INSERT INTO tb_partida_estatistica_esperadas 
                (id_partida, id_estatisticas_esperadas_casa, id_estatisticas_esperadas_fora,
                 partida_ftid, partida_htid, partida_ft_confrontoid, partida_ht_confrontoid) 
                VALUES 
                (@IdPartida, @IdEstatisticasEsperadasCasa, @IdEstatisticasEsperadasFora,
                 @PartidaFtId, @PartidaHtId, @PartidaFtConfrontoId, @PartidaHtConfrontoId)
                ON CONFLICT (id_partida) DO UPDATE SET
                id_estatisticas_esperadas_casa = EXCLUDED.id_estatisticas_esperadas_casa,
                id_estatisticas_esperadas_fora = EXCLUDED.id_estatisticas_esperadas_fora,
                partida_ftid = EXCLUDED.partida_ftid,
                partida_htid = EXCLUDED.partida_htid,
                partida_ft_confrontoid = EXCLUDED.partida_ft_confrontoid,
                partida_ht_confrontoid = EXCLUDED.partida_ht_confrontoid;";

                    await conn.ExecuteAsync(insertPartidaEstatisticas, new
                    {
                        IdPartida = partidaAnalisada.Id,
                        IdEstatisticasEsperadasCasa = idEstatisticaEsperadaCasa,
                        IdEstatisticasEsperadasFora = idEstatisticaEsperadaFora,
                        PartidaFtId = idEstatisticaCasaVazia,
                        PartidaHtId = idEstatisticaCasaVazia,
                        PartidaFtConfrontoId = idEstatisticaCasaVazia,
                        PartidaHtConfrontoId = idEstatisticaCasaVazia
                    }, transaction);

                    // 8. Salvar no banco principal
                    _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.Add(partidaEstatisticas);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return Ok("Dados salvos com sucesso em ambos os bancos");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Erro ao salvar no Supabase: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Partida_Estatistica_Esperadas p)
        {
            try
            {
                _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.Update(p);
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

                Partida_Estatistica_Esperadas p = await _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS
              .FirstOrDefaultAsync(pa => pa.Id == id);

                if (p == null)
                    throw new System.Exception("Partida Não Encontrada");
                _context.TB_PARTIDA_ESTAITSTICA_ESPERADAS.Remove(p);


                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<Partida_Estatistica_Esperadas> GerarPartidaEstatisticaEsperada(Partida partidaAnalisada)
        {
            Partida_Estatistica_Esperadas partida = new Partida_Estatistica_Esperadas();

            Estatistica_Esperadas estaTistica_Home = await _context.TB_ESTATISTICA_ESPERADAS
                .Include(e => e.FT)
                .Include(e => e.FT_Adversario)
                .Include(e => e.FT_Confronto)
                .Include(e => e.HT)
                .Include(e => e.HT_Adversario)
                .Include(e => e.HT_Confronto)
            .FirstOrDefaultAsync(p => p.NomeTime.ToLower().Contains(partidaAnalisada.NomeTimeCasa.ToLower()));


            Estatistica_Esperadas estaTistica_Fora = await _context.TB_ESTATISTICA_ESPERADAS
                .Include(e => e.FT)
                .Include(e => e.FT_Adversario)
                .Include(e => e.FT_Confronto)
                .Include(e => e.HT)
                .Include(e => e.HT_Adversario)
                .Include(e => e.HT_Confronto)
            .FirstOrDefaultAsync(p => p.NomeTime.ToLower().Contains(partidaAnalisada.NomeTimeFora.ToLower()));

            partida = GerarmediaPartida(estaTistica_Home, estaTistica_Fora);

            partida.Partida = partidaAnalisada;
            partida.Id_Partida = partidaAnalisada.Id;
            partida.Id_Estatisticas_Esperadas_Casa = estaTistica_Home.Id;
            partida.Id_Estatisticas_Esperadas_Fora = estaTistica_Fora.Id;
            partida.Estatisticas_Esperadas_Casa = estaTistica_Home;
            partida.Estatisticas_Esperadas_Fora = estaTistica_Fora;

            return partida;
        }
        private Partida_Estatistica_Esperadas GerarmediaPartida(Estatistica_Esperadas casa, Estatistica_Esperadas fora)
        {
            Partida_Estatistica_Esperadas Partida = new Partida_Estatistica_Esperadas();


            Estatistica_BaseModel FT_TEAM1 = CalcularMedia(casa.FT, fora.FT_Adversario);
            Estatistica_BaseModel FT_TEAM2 = CalcularMedia(casa.FT_Adversario, fora.FT);
            Estatistica_BaseModel HT_TEAM1 = CalcularMedia(casa.HT, fora.HT_Adversario);
            Estatistica_BaseModel HT_TEAM2 = CalcularMedia(casa.HT_Adversario, fora.HT);


            Partida.Partida_FT = CalcularMedia(FT_TEAM1, FT_TEAM2);
            Partida.Partida_HT = CalcularMedia(HT_TEAM1, HT_TEAM2);
            Partida.Partida_FT_Confronto = CalcularMedia(casa.FT_Confronto, fora.FT_Confronto);
            Partida.Partida_HT_Confronto = CalcularMedia(casa.HT_Confronto, fora.HT_Confronto);

            return Partida;
        }


        public static Estatistica_BaseModel CalcularMedia(Estatistica_BaseModel estatistica1, Estatistica_BaseModel estatistica2)
        {
            if (estatistica1 == null || estatistica2 == null)
                throw new ArgumentNullException("As estatísticas não podem ser nulas");

            return new Estatistica_BaseModel
            {
                Gol = CalcularMediaPropriedade(estatistica1.Gol, estatistica2.Gol),
                Gol_Slope = CalcularMediaPropriedade(estatistica1.Gol_Slope, estatistica2.Gol_Slope),
                Gol_DP = CalcularMediaPropriedade(estatistica1.Gol_DP, estatistica2.Gol_DP),
                GolSofrido = CalcularMediaPropriedade(estatistica1.GolSofrido, estatistica2.GolSofrido),
                GolSofrido_Slope = CalcularMediaPropriedade(estatistica1.GolSofrido_Slope, estatistica2.GolSofrido_Slope),
                GolSofrido_DP = CalcularMediaPropriedade(estatistica1.GolSofrido_DP, estatistica2.GolSofrido_DP),
                Posse_Bola = CalcularMediaPropriedade(estatistica1.Posse_Bola, estatistica2.Posse_Bola),
                Posse_Bola_Slope = CalcularMediaPropriedade(estatistica1.Posse_Bola_Slope, estatistica2.Posse_Bola_Slope),
                Posse_Bola_DP = CalcularMediaPropriedade(estatistica1.Posse_Bola_DP, estatistica2.Posse_Bola_DP),
                Total_Finalizacao = CalcularMediaPropriedade(estatistica1.Total_Finalizacao, estatistica2.Total_Finalizacao),
                Total_Finalizacao_Slope = CalcularMediaPropriedade(estatistica1.Total_Finalizacao_Slope, estatistica2.Total_Finalizacao_Slope),
                Total_Finalizacao_DP = CalcularMediaPropriedade(estatistica1.Total_Finalizacao_DP, estatistica2.Total_Finalizacao_DP),
                Chances_Claras = CalcularMediaPropriedade(estatistica1.Chances_Claras, estatistica2.Chances_Claras),
                Escanteios = CalcularMediaPropriedade(estatistica1.Escanteios, estatistica2.Escanteios),
                Escanteios_Slope = CalcularMediaPropriedade(estatistica1.Escanteios_Slope, estatistica2.Escanteios_Slope),
                Escanteios_DP = CalcularMediaPropriedade(estatistica1.Escanteios_DP, estatistica2.Escanteios_DP),
                Bolas_trave = CalcularMediaPropriedade(estatistica1.Bolas_trave, estatistica2.Bolas_trave),
                Gols_de_cabeça = CalcularMediaPropriedade(estatistica1.Gols_de_cabeça, estatistica2.Gols_de_cabeça),
                Defesas_Goleiro = CalcularMediaPropriedade(estatistica1.Defesas_Goleiro, estatistica2.Defesas_Goleiro),
                Impedimentos = CalcularMediaPropriedade(estatistica1.Impedimentos, estatistica2.Impedimentos),
                Impedimentos_Slope = CalcularMediaPropriedade(estatistica1.Impedimentos_Slope, estatistica2.Impedimentos_Slope),
                Impedimentos_DP = CalcularMediaPropriedade(estatistica1.Impedimentos_DP, estatistica2.Impedimentos_DP),
                Faltas = CalcularMediaPropriedade(estatistica1.Faltas, estatistica2.Faltas),
                Faltas_Slope = CalcularMediaPropriedade(estatistica1.Faltas_Slope, estatistica2.Faltas_Slope),
                Faltas_DP = CalcularMediaPropriedade(estatistica1.Faltas_DP, estatistica2.Faltas_DP),
                Cartoes_Amarelos = CalcularMediaPropriedade(estatistica1.Cartoes_Amarelos, estatistica2.Cartoes_Amarelos),
                Cartoes_Amarelos_Slope = CalcularMediaPropriedade(estatistica1.Cartoes_Amarelos_Slope, estatistica2.Cartoes_Amarelos_Slope),
                Cartoes_Amarelos_DP = CalcularMediaPropriedade(estatistica1.Cartoes_Amarelos_DP, estatistica2.Cartoes_Amarelos_DP),
                Cartoes_Vermelhos = CalcularMediaPropriedade(estatistica1.Cartoes_Vermelhos, estatistica2.Cartoes_Vermelhos),
                Cartoes_Vermelhos_Slope = CalcularMediaPropriedade(estatistica1.Cartoes_Vermelhos_Slope, estatistica2.Cartoes_Vermelhos_Slope),
                Cartoes_Vermelhos_DP = CalcularMediaPropriedade(estatistica1.Cartoes_Vermelhos_DP, estatistica2.Cartoes_Vermelhos_DP),
                Laterais_Cobrados = CalcularMediaPropriedade(estatistica1.Laterais_Cobrados, estatistica2.Laterais_Cobrados),
                Toque_Area_Adversaria = CalcularMediaPropriedade(estatistica1.Toque_Area_Adversaria, estatistica2.Toque_Area_Adversaria),
                Passes = CalcularMediaPropriedade(estatistica1.Passes, estatistica2.Passes),
                Passes_Totais = CalcularMediaPropriedade(estatistica1.Passes_Totais, estatistica2.Passes_Totais),
                Precisao_Passes = CalcularMediaPropriedade(estatistica1.Precisao_Passes, estatistica2.Precisao_Passes),
                Passes_terco_Final = CalcularMediaPropriedade(estatistica1.Passes_terco_Final, estatistica2.Passes_terco_Final),
                Cruzamentos = CalcularMediaPropriedade(estatistica1.Cruzamentos, estatistica2.Cruzamentos),
                Desarmes = CalcularMediaPropriedade(estatistica1.Desarmes, estatistica2.Desarmes),
                Desarmes_Slope = CalcularMediaPropriedade(estatistica1.Desarmes_Slope, estatistica2.Desarmes_Slope),
                Desarmes_DP = CalcularMediaPropriedade(estatistica1.Desarmes_DP, estatistica2.Desarmes_DP),
                Bolas_Afastadas = CalcularMediaPropriedade(estatistica1.Bolas_Afastadas, estatistica2.Bolas_Afastadas),
                Interceptacoes = CalcularMediaPropriedade(estatistica1.Interceptacoes, estatistica2.Interceptacoes)
            };
        }

        private static float? CalcularMediaPropriedade(float? valor1, float? valor2)
        {
            if (valor1.HasValue && valor2.HasValue)
                return (valor1.Value + valor2.Value) / 2f;

            if (valor1.HasValue)
                return valor1.Value;

            if (valor2.HasValue)
                return valor2.Value;

            return null;
        }
    }

}
