using botAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using botAPI.Models;


public class MLDataSyncService
{
    private readonly DataContext _apiDb;

    private readonly MLDbContext _mlDb;
    private readonly ILogger<MLDataSyncService> _logger;

    public MLDataSyncService(DataContext apiDb, MLDbContext mlDb, ILogger<MLDataSyncService> logger)
    {
        _apiDb = apiDb;
        _mlDb = mlDb;
        _logger = logger;
    }

    public async Task SyncAllDataAsync()
    {
        try
        {
            _logger.LogInformation("Iniciando sincronização com banco ML...");

            await SyncBaseStatistics();

            await SyncExpectedStatistics();

            await SyncMatches();

            await SyncMatchExpectedStats();

            _logger.LogInformation("Sincronização completa com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a sincronização com o banco ML");
            throw;
        }
    }

    private async Task SyncBaseStatistics()
    {
        _logger.LogInformation("Sincronizando estatísticas base...");

        // Obter estatísticas que estão vinculadas a partidas marcadas para análise
        var statsToSync = await _apiDb.TB_ESTATISTICA_BASEMODEL
            .Where(s => _apiDb.TB_PARTIDA_ESTAITSTICA_ESPERADAS
                .Any(pe => pe.Partida.PartidaAnalise &&
                          (pe.Id_Partida_FT == s.Id ||
                           pe.Id_Partida_HT == s.Id ||
                           pe.Id_Partida_FT_Confronto == s.Id ||
                           pe.Id_Partida_HT_Confronto == s.Id)))
            .AsNoTracking()
            .ToListAsync();

        foreach (var stat in statsToSync)
        {
            if (!await _mlDb.estatistica_basemodel.AnyAsync(s => s.Id == stat.Id))
            {
                var mlStat = new Estatistica_BaseModel
                {
                    Id = stat.Id,
                    Gol = stat.Gol,
                    Gol_Slope = stat.Gol_Slope,
                    Gol_DP = stat.Gol_DP,
                    GolSofrido = stat.GolSofrido,
                    GolSofrido_Slope = stat.GolSofrido_Slope,
                    GolSofrido_DP = stat.GolSofrido_DP,
                    Posse_Bola = stat.Posse_Bola,
                    Posse_Bola_Slope = stat.Posse_Bola_Slope,
                    Posse_Bola_DP = stat.Posse_Bola_DP,
                    Total_Finalizacao = stat.Total_Finalizacao,
                    Total_Finalizacao_Slope = stat.Total_Finalizacao_Slope,
                    Total_Finalizacao_DP = stat.Total_Finalizacao_DP,
                    Chances_Claras = stat.Chances_Claras,
                    Escanteios = stat.Escanteios,
                    Escanteios_Slope = stat.Escanteios_Slope,
                    Escanteios_DP = stat.Escanteios_DP,
                    Bolas_trave = stat.Bolas_trave,
                    Gols_de_cabeca = stat.Gols_de_cabeca,
                    Defesas_Goleiro = stat.Defesas_Goleiro,
                    Impedimentos = stat.Impedimentos,
                    Impedimentos_Slope = stat.Impedimentos_Slope,
                    Impedimentos_DP = stat.Impedimentos_DP,
                    Faltas = stat.Faltas,
                    Faltas_Slope = stat.Faltas_Slope,
                    Faltas_DP = stat.Faltas_DP,
                    Cartoes_Amarelos = stat.Cartoes_Amarelos,
                    Cartoes_Amarelos_Slope = stat.Cartoes_Amarelos_Slope,
                    Cartoes_Amarelos_DP = stat.Cartoes_Amarelos_DP,
                    Cartoes_Vermelhos = stat.Cartoes_Vermelhos,
                    Cartoes_Vermelhos_Slope = stat.Cartoes_Vermelhos_Slope,
                    Cartoes_Vermelhos_DP = stat.Cartoes_Vermelhos_DP,
                    Laterais_Cobrados = stat.Laterais_Cobrados,
                    Toque_Area_Adversaria = stat.Toque_Area_Adversaria,
                    Passes = stat.Passes,
                    Passes_Totais = stat.Passes_Totais,
                    Precisao_Passes = stat.Precisao_Passes,
                    Passes_terco_Final = stat.Passes_terco_Final,
                    Cruzamentos = stat.Cruzamentos,
                    Desarmes = stat.Desarmes,
                    Desarmes_Slope = stat.Desarmes_Slope,
                    Desarmes_DP = stat.Desarmes_DP,
                    Bolas_Afastadas = stat.Bolas_Afastadas,
                    Interceptacoes = stat.Interceptacoes
                };
                await _mlDb.estatistica_basemodel.AddAsync(mlStat);
            }
        }
        await _mlDb.SaveChangesAsync();
    }

    private async Task SyncExpectedStatistics()
    {
        _logger.LogInformation("Sincronizando estatísticas esperadas...");

        var expectedStats = await _apiDb.TB_ESTATISTICA_ESPERADAS
            .Where(es => _apiDb.TB_PARTIDA_ESTAITSTICA_ESPERADAS
                .Any(pe => pe.Partida.PartidaAnalise &&
                          (pe.Id_Estatisticas_Esperadas_Casa == es.Id || pe.Id_Estatisticas_Esperadas_Fora == es.Id)))
            .AsNoTracking()
            .ToListAsync();

        foreach (var stat in expectedStats)
        {
            if (!await _mlDb.tb_estatistica_esperadas.AnyAsync(e => e.Id == stat.Id))
            {
                var mlStat = new Estatistica_Esperadas
                {
                    Id = stat.Id,
                    NomeTime = stat.NomeTime,
                    FT_Id = stat.FT_Id,
                    HT_Id = stat.HT_Id,
                    FT_Adversario_Id = stat.FT_Adversario_Id,
                    HT_Adversario_Id = stat.HT_Adversario_Id,
                    FT_Confronto_Id = stat.FT_Confronto_Id,
                    HT_Confronto_Id = stat.HT_Confronto_Id,
                };
                await _mlDb.tb_estatistica_esperadas.AddAsync(mlStat);
            }
        }
        await _mlDb.SaveChangesAsync();
    }

    private async Task SyncMatches()
    {
        _logger.LogInformation("Sincronizando partidas...");

        var matchesToSync = await _apiDb.TB_PARTIDAS
            .Where(p => p.PartidaAnalise)
            .AsNoTracking()
            .ToListAsync();

        foreach (var match in matchesToSync)
        {
            if (!await _mlDb.tb_partidas.AnyAsync(p => p.Id == match.Id))
            {
                var mlMatch = new Partida
                {
                    Id = match.Id,
                    NomeTimeCasa = match.NomeTimeCasa,
                    NomeTimeFora = match.NomeTimeFora,
                    DataPartida = match.DataPartida,
                    Campeonato = match.Campeonato,
                    PartidaAnalise = match.PartidaAnalise,
                    TipoPartida = match.TipoPartida,
                    Url_Partida = match.Url_Partida
                };
                await _mlDb.tb_partidas.AddAsync(mlMatch);
            }
        }
        await _mlDb.SaveChangesAsync();
    }

    private async Task SyncMatchExpectedStats()
    {
        _logger.LogInformation("Sincronizando vínculos partida-estatísticas...");

        var linksToSync = await _apiDb.TB_PARTIDA_ESTAITSTICA_ESPERADAS
            .Include(pe => pe.Partida)
            .Where(pe => pe.Partida.PartidaAnalise)
            .AsNoTracking()
            .ToListAsync();

        foreach (var link in linksToSync)
        {
            if (!await _mlDb.tb_partida_estatistica_esperadas.AnyAsync(p => p.Id == link.Id))
            {
                var mlLink = new Partida_Estatistica_Esperadas
                {
                    Id = link.Id,
                    Id_Partida = link.Id_Partida,
                    Id_Estatisticas_Esperadas_Casa = link.Id_Estatisticas_Esperadas_Casa,
                    Id_Estatisticas_Esperadas_Fora = link.Id_Estatisticas_Esperadas_Fora,
                    Id_Partida_FT = link.Id_Partida_FT,
                    Id_Partida_HT = link.Id_Partida_HT,
                    Id_Partida_FT_Confronto = link.Id_Partida_FT_Confronto,
                    Id_Partida_HT_Confronto = link.Id_Partida_HT_Confronto
                };
                await _mlDb.tb_partida_estatistica_esperadas.AddAsync(mlLink);
            }
        }
        await _mlDb.SaveChangesAsync();
    }
}