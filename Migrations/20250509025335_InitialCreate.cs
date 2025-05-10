using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace botAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estatistica_BaseModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gol = table.Column<float>(type: "real", nullable: true),
                    Gol_Slope = table.Column<float>(type: "real", nullable: true),
                    Gol_DP = table.Column<float>(type: "real", nullable: true),
                    GolSofrido = table.Column<float>(type: "real", nullable: true),
                    GolSofrido_Slope = table.Column<float>(type: "real", nullable: true),
                    GolSofrido_DP = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola_Slope = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola_DP = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao_Slope = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao_DP = table.Column<float>(type: "real", nullable: true),
                    Chances_Claras = table.Column<float>(type: "real", nullable: true),
                    Escanteios = table.Column<float>(type: "real", nullable: true),
                    Escanteios_Slope = table.Column<float>(type: "real", nullable: true),
                    Escanteios_DP = table.Column<float>(type: "real", nullable: true),
                    Bolas_trave = table.Column<float>(type: "real", nullable: true),
                    Gols_de_cabeça = table.Column<float>(type: "real", nullable: true),
                    Defesas_Goleiro = table.Column<float>(type: "real", nullable: true),
                    Impedimentos = table.Column<float>(type: "real", nullable: true),
                    Impedimentos_Slope = table.Column<float>(type: "real", nullable: true),
                    Impedimentos_DP = table.Column<float>(type: "real", nullable: true),
                    Faltas = table.Column<float>(type: "real", nullable: true),
                    Faltas_Slope = table.Column<float>(type: "real", nullable: true),
                    Faltas_DP = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos_Slope = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos_DP = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos_Slope = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos_DP = table.Column<float>(type: "real", nullable: true),
                    Laterais_Cobrados = table.Column<float>(type: "real", nullable: true),
                    Toque_Area_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Passes = table.Column<float>(type: "real", nullable: true),
                    Passes_Totais = table.Column<float>(type: "real", nullable: true),
                    Precisao_Passes = table.Column<float>(type: "real", nullable: true),
                    Passes_terco_Final = table.Column<float>(type: "real", nullable: true),
                    Cruzamentos = table.Column<float>(type: "real", nullable: true),
                    Desarmes = table.Column<float>(type: "real", nullable: true),
                    Desarmes_Slope = table.Column<float>(type: "real", nullable: true),
                    Desarmes_DP = table.Column<float>(type: "real", nullable: true),
                    Bolas_Afastadas = table.Column<float>(type: "real", nullable: true),
                    Interceptacoes = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estatistica_BaseModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_ERROSLOGS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    horaErro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QualPageFoi = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    QualUrl = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    OqueProvavelmenteAConteceu = table.Column<string>(type: "varchar(510)", maxLength: 510, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ERROSLOGS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_ESTATISTICA",
                columns: table => new
                {
                    Id_Estatistica = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Partida = table.Column<int>(type: "int", nullable: false),
                    CasaOuFora = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    TipoPartida = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    NomeTime = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    NomeTimeRival = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Gol = table.Column<int>(type: "int", nullable: true),
                    GolSofrido = table.Column<int>(type: "int", nullable: true),
                    Posse_Bola = table.Column<int>(type: "int", nullable: true),
                    Total_Finalizacao = table.Column<int>(type: "int", nullable: true),
                    Chances_Claras = table.Column<int>(type: "int", nullable: true),
                    Escanteios = table.Column<int>(type: "int", nullable: true),
                    Bolas_trave = table.Column<int>(type: "int", nullable: true),
                    Gols_de_cabeça = table.Column<int>(type: "int", nullable: true),
                    Defesas_Goleiro = table.Column<int>(type: "int", nullable: true),
                    Impedimentos = table.Column<int>(type: "int", nullable: true),
                    Faltas = table.Column<int>(type: "int", nullable: true),
                    Cartoes_Amarelos = table.Column<int>(type: "int", nullable: true),
                    Cartoes_Vermelhos = table.Column<int>(type: "int", nullable: true),
                    Laterais_Cobrados = table.Column<int>(type: "int", nullable: true),
                    Toque_Area_Adversaria = table.Column<int>(type: "int", nullable: true),
                    Passes = table.Column<int>(type: "int", nullable: true),
                    Passes_Totais = table.Column<int>(type: "int", nullable: true),
                    Precisao_Passes = table.Column<int>(type: "int", nullable: true),
                    Passes_terco_Final = table.Column<int>(type: "int", nullable: true),
                    Cruzamentos = table.Column<int>(type: "int", nullable: true),
                    Desarmes = table.Column<int>(type: "int", nullable: true),
                    Bolas_Afastadas = table.Column<int>(type: "int", nullable: true),
                    Interceptacoes = table.Column<int>(type: "int", nullable: true),
                    Gol_HT = table.Column<int>(type: "int", nullable: true),
                    GolSofrido_HT = table.Column<int>(type: "int", nullable: true),
                    Posse_Bola_HT = table.Column<int>(type: "int", nullable: true),
                    Total_Finalizacao_HT = table.Column<int>(type: "int", nullable: true),
                    Chances_Claras_HT = table.Column<int>(type: "int", nullable: true),
                    Escanteios_HT = table.Column<int>(type: "int", nullable: true),
                    Bolas_trave_HT = table.Column<int>(type: "int", nullable: true),
                    Gols_de_cabeça_HT = table.Column<int>(type: "int", nullable: true),
                    Defesas_Goleiro_HT = table.Column<int>(type: "int", nullable: true),
                    Impedimentos_HT = table.Column<int>(type: "int", nullable: true),
                    Faltas_HT = table.Column<int>(type: "int", nullable: true),
                    Cartoes_Amarelos_HT = table.Column<int>(type: "int", nullable: true),
                    Cartoes_Vermelhos_HT = table.Column<int>(type: "int", nullable: true),
                    Laterais_Cobrados_HT = table.Column<int>(type: "int", nullable: true),
                    Toque_Area_Adversaria_HT = table.Column<int>(type: "int", nullable: true),
                    Passes_HT = table.Column<int>(type: "int", nullable: true),
                    Passes_Totais_HT = table.Column<int>(type: "int", nullable: true),
                    Precisao_Passes_HT = table.Column<int>(type: "int", nullable: true),
                    Passes_terco_Final_HT = table.Column<int>(type: "int", nullable: true),
                    Cruzamentos_HT = table.Column<int>(type: "int", nullable: true),
                    Desarmes_HT = table.Column<int>(type: "int", nullable: true),
                    Bolas_Afastadas_HT = table.Column<int>(type: "int", nullable: true),
                    Interceptacoes_HT = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ESTATISTICA", x => x.Id_Estatistica);
                });

            migrationBuilder.CreateTable(
                name: "TB_ESTATISTICA_TIME",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasaOuFora = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    NomeTime = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Gol = table.Column<float>(type: "real", nullable: true),
                    GolSofrido = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao = table.Column<float>(type: "real", nullable: true),
                    Chances_Claras = table.Column<float>(type: "real", nullable: true),
                    Escanteios = table.Column<float>(type: "real", nullable: true),
                    Bolas_trave = table.Column<float>(type: "real", nullable: true),
                    Gols_de_cabeça = table.Column<float>(type: "real", nullable: true),
                    Defesas_Goleiro = table.Column<float>(type: "real", nullable: true),
                    Impedimentos = table.Column<float>(type: "real", nullable: true),
                    Faltas = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos = table.Column<float>(type: "real", nullable: true),
                    Laterais_Cobrados = table.Column<float>(type: "real", nullable: true),
                    Toque_Area_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Passes = table.Column<float>(type: "real", nullable: true),
                    Passes_Totais = table.Column<float>(type: "real", nullable: true),
                    Precisao_Passes = table.Column<float>(type: "real", nullable: true),
                    Passes_terco_Final = table.Column<float>(type: "real", nullable: true),
                    Cruzamentos = table.Column<float>(type: "real", nullable: true),
                    Desarmes = table.Column<float>(type: "real", nullable: true),
                    Bolas_Afastadas = table.Column<float>(type: "real", nullable: true),
                    Interceptacoes = table.Column<float>(type: "real", nullable: true),
                    Gol_Adversaria = table.Column<float>(type: "real", nullable: true),
                    GolSofrido_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Chances_Claras_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Escanteios_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Bolas_trave_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Gols_de_cabeça_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Defesas_Goleiro_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Impedimentos_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Faltas_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Laterais_Cobrados_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Toque_Area_Adversaria_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Passes_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Passes_Totais_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Precisao_Passes_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Passes_terco_Final_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Cruzamentos_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Desarmes_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Bolas_Afastadas_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Interceptacoes_Adversaria = table.Column<float>(type: "real", nullable: true),
                    Gol_Confrontos = table.Column<float>(type: "real", nullable: true),
                    GolSofrido_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Chances_Claras_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Escanteios_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Bolas_trave_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Gols_de_cabeça_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Defesas_Goleiro_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Impedimentos_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Faltas_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Laterais_Cobrados_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Toque_Area_Adversaria_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Passes_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Passes_Totais_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Precisao_Passes_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Passes_terco_Final_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Cruzamentos_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Desarmes_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Bolas_Afastadas_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Interceptacoes_Confrontos = table.Column<float>(type: "real", nullable: true),
                    Gol_HT = table.Column<float>(type: "real", nullable: true),
                    GolSofrido_HT = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola_HT = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao_HT = table.Column<float>(type: "real", nullable: true),
                    Chances_Claras_HT = table.Column<float>(type: "real", nullable: true),
                    Escanteios_HT = table.Column<float>(type: "real", nullable: true),
                    Bolas_trave_HT = table.Column<float>(type: "real", nullable: true),
                    Gols_de_cabeça_HT = table.Column<float>(type: "real", nullable: true),
                    Defesas_Goleiro_HT = table.Column<float>(type: "real", nullable: true),
                    Impedimentos_HT = table.Column<float>(type: "real", nullable: true),
                    Faltas_HT = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos_HT = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos_HT = table.Column<float>(type: "real", nullable: true),
                    Laterais_Cobrados_HT = table.Column<float>(type: "real", nullable: true),
                    Toque_Area_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_Totais_HT = table.Column<float>(type: "real", nullable: true),
                    Precisao_Passes_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_terco_Final_HT = table.Column<float>(type: "real", nullable: true),
                    Cruzamentos_HT = table.Column<float>(type: "real", nullable: true),
                    Desarmes_HT = table.Column<float>(type: "real", nullable: true),
                    Bolas_Afastadas_HT = table.Column<float>(type: "real", nullable: true),
                    Interceptacoes_HT = table.Column<float>(type: "real", nullable: true),
                    Gol_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    GolSofrido_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Chances_Claras_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Escanteios_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Bolas_trave_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Gols_de_cabeça_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Defesas_Goleiro_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Impedimentos_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Faltas_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Laterais_Cobrados_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Toque_Area_Adversaria_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_Totais_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Precisao_Passes_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_terco_Final_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Cruzamentos_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Desarmes_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Bolas_Afastadas_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Interceptacoes_Adversaria_HT = table.Column<float>(type: "real", nullable: true),
                    Gol_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    GolSofrido_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Posse_Bola_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Total_Finalizacao_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Chances_Claras_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Escanteios_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Bolas_trave_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Gols_de_cabeça_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Defesas_Goleiro_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Impedimentos_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Faltas_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Amarelos_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Cartoes_Vermelhos_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Laterais_Cobrados_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Toque_Area_Adversaria_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_Totais_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Precisao_Passes_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Passes_terco_Final_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Cruzamentos_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Desarmes_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Bolas_Afastadas_Confrontos_HT = table.Column<float>(type: "real", nullable: true),
                    Interceptacoes_Confrontos_HT = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ESTATISTICA_TIME", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_PALPITES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPartida = table.Column<int>(type: "int", nullable: false),
                    TipoAposta = table.Column<int>(type: "int", nullable: false),
                    Num = table.Column<double>(type: "float", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(510)", maxLength: 510, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PALPITES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_PARTIDAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_EstatisticaCasa = table.Column<int>(type: "int", nullable: false),
                    Id_EstatisticaFora = table.Column<int>(type: "int", nullable: false),
                    NomeTimeCasa = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    NomeTimeFora = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Url_Partida = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    DataPartida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Campeonato = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PartidaAnalise = table.Column<bool>(type: "bit", nullable: false),
                    TipoPartida = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PARTIDAS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_ESTATISTICA_ESPERADAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTime = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    FTId = table.Column<int>(type: "int", nullable: true),
                    HTId = table.Column<int>(type: "int", nullable: true),
                    FT_AdversarioId = table.Column<int>(type: "int", nullable: true),
                    HT_AdversarioId = table.Column<int>(type: "int", nullable: true),
                    FT_ConfrontoId = table.Column<int>(type: "int", nullable: true),
                    HT_ConfrontoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ESTATISTICA_ESPERADAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ESTATISTICA_ESPERADAS_Estatistica_BaseModel_FTId",
                        column: x => x.FTId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_ESTATISTICA_ESPERADAS_Estatistica_BaseModel_FT_AdversarioId",
                        column: x => x.FT_AdversarioId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_ESTATISTICA_ESPERADAS_Estatistica_BaseModel_FT_ConfrontoId",
                        column: x => x.FT_ConfrontoId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_ESTATISTICA_ESPERADAS_Estatistica_BaseModel_HTId",
                        column: x => x.HTId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_ESTATISTICA_ESPERADAS_Estatistica_BaseModel_HT_AdversarioId",
                        column: x => x.HT_AdversarioId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_ESTATISTICA_ESPERADAS_Estatistica_BaseModel_HT_ConfrontoId",
                        column: x => x.HT_ConfrontoId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Partida = table.Column<int>(type: "int", nullable: false),
                    PartidaId = table.Column<int>(type: "int", nullable: true),
                    Id_Estatisticas_Esperadas_Casa = table.Column<int>(type: "int", nullable: false),
                    Time_CasaId = table.Column<int>(type: "int", nullable: true),
                    Id_Estatisticas_Esperadas_Fora = table.Column<int>(type: "int", nullable: false),
                    Time_ForaId = table.Column<int>(type: "int", nullable: true),
                    Partida_FTId = table.Column<int>(type: "int", nullable: true),
                    Partida_HTId = table.Column<int>(type: "int", nullable: true),
                    Partida_FT_ConfrontoId = table.Column<int>(type: "int", nullable: true),
                    Partida_HT_ConfrontoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PARTIDA_ESTAITSTICA_ESPERADAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Estatistica_BaseModel_Partida_FTId",
                        column: x => x.Partida_FTId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Estatistica_BaseModel_Partida_FT_ConfrontoId",
                        column: x => x.Partida_FT_ConfrontoId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Estatistica_BaseModel_Partida_HTId",
                        column: x => x.Partida_HTId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Estatistica_BaseModel_Partida_HT_ConfrontoId",
                        column: x => x.Partida_HT_ConfrontoId,
                        principalTable: "Estatistica_BaseModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_ESPERADAS_Time_CasaId",
                        column: x => x.Time_CasaId,
                        principalTable: "TB_ESTATISTICA_ESPERADAS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_ESTATISTICA_ESPERADAS_Time_ForaId",
                        column: x => x.Time_ForaId,
                        principalTable: "TB_ESTATISTICA_ESPERADAS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_PARTIDA_ESTAITSTICA_ESPERADAS_TB_PARTIDAS_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "TB_PARTIDAS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTATISTICA_ESPERADAS_FT_AdversarioId",
                table: "TB_ESTATISTICA_ESPERADAS",
                column: "FT_AdversarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTATISTICA_ESPERADAS_FT_ConfrontoId",
                table: "TB_ESTATISTICA_ESPERADAS",
                column: "FT_ConfrontoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTATISTICA_ESPERADAS_FTId",
                table: "TB_ESTATISTICA_ESPERADAS",
                column: "FTId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTATISTICA_ESPERADAS_HT_AdversarioId",
                table: "TB_ESTATISTICA_ESPERADAS",
                column: "HT_AdversarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTATISTICA_ESPERADAS_HT_ConfrontoId",
                table: "TB_ESTATISTICA_ESPERADAS",
                column: "HT_ConfrontoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTATISTICA_ESPERADAS_HTId",
                table: "TB_ESTATISTICA_ESPERADAS",
                column: "HTId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_FT_ConfrontoId",
                table: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                column: "Partida_FT_ConfrontoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_FTId",
                table: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                column: "Partida_FTId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_HT_ConfrontoId",
                table: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                column: "Partida_HT_ConfrontoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Partida_HTId",
                table: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                column: "Partida_HTId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_PartidaId",
                table: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Time_CasaId",
                table: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                column: "Time_CasaId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PARTIDA_ESTAITSTICA_ESPERADAS_Time_ForaId",
                table: "TB_PARTIDA_ESTAITSTICA_ESPERADAS",
                column: "Time_ForaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ERROSLOGS");

            migrationBuilder.DropTable(
                name: "TB_ESTATISTICA");

            migrationBuilder.DropTable(
                name: "TB_ESTATISTICA_TIME");

            migrationBuilder.DropTable(
                name: "TB_PALPITES");

            migrationBuilder.DropTable(
                name: "TB_PARTIDA_ESTAITSTICA_ESPERADAS");

            migrationBuilder.DropTable(
                name: "TB_ESTATISTICA_ESPERADAS");

            migrationBuilder.DropTable(
                name: "TB_PARTIDAS");

            migrationBuilder.DropTable(
                name: "Estatistica_BaseModel");
        }
    }
}
