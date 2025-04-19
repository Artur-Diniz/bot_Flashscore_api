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
                name: "TB_ERROSLOGS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Interceptacoes = table.Column<int>(type: "int", nullable: true)
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
                    Interceptacoes_Confrontos = table.Column<float>(type: "real", nullable: true)
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
                    Descricao = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
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
                    DataPartida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Campeonato = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PartidaAnalise = table.Column<bool>(type: "bit", nullable: false),
                    TipoPartida = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PARTIDAS", x => x.Id);
                });
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
                name: "TB_PARTIDAS");
        }
    }
}
