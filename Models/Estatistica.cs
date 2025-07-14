
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Estatistica
    {   
        [Key]
        public int Id_Estatistica { get; set; }
        public int Id_Partida { get; set; }

        public bool EstastiticaAnalise { get; set; } = false;

        public string CasaOuFora { get; set; } = string.Empty;
        public string TipoPartida { get; set; }= string.Empty;
       
        public string NomeTime { get; set; } = string.Empty;
        public string NomeTimeRival { get; set; }= string.Empty;

        public int? Gol { get; set; }
        public int? GolSofrido { get; set; }
        public int? Posse_Bola { get; set; }
        public int? Total_Finalizacao { get; set; }
        public int? Chances_Claras { get; set; }
        public int? Escanteios { get; set; }
        public int? Bolas_trave { get; set; }
        public int? Gols_de_cabeça { get; set; }
        public int? Defesas_Goleiro { get; set; }
        public int? Impedimentos { get; set; }
        public int? Faltas { get; set; }
        public int? Cartoes_Amarelos { get; set; }
        public int? Cartoes_Vermelhos { get; set; }
        public int? Laterais_Cobrados { get; set; }
        public int? Toque_Area_Adversaria { get; set; }
        public int? Passes { get; set; }
        public int? Passes_Totais { get; set; }
        public int? Precisao_Passes { get; set; }
        public int? Passes_terco_Final { get; set; }
        public int? Cruzamentos { get; set; }
        public int? Desarmes { get; set; }
        public int? Bolas_Afastadas { get; set; }
        public int? Interceptacoes { get; set; }


        
        public int? Gol_HT { get; set; }
        public int? GolSofrido_HT { get; set; }
        public int? Posse_Bola_HT { get; set; }
        public int? Total_Finalizacao_HT { get; set; }
        public int? Chances_Claras_HT { get; set; }
        public int? Escanteios_HT { get; set; }
        public int? Bolas_trave_HT { get; set; }
        public int? Gols_de_cabeça_HT { get; set; }
        public int? Defesas_Goleiro_HT { get; set; }
        public int? Impedimentos_HT { get; set; }
        public int? Faltas_HT { get; set; }
        public int? Cartoes_Amarelos_HT { get; set; }
        public int? Cartoes_Vermelhos_HT { get; set; }
        public int? Laterais_Cobrados_HT { get; set; }
        public int? Toque_Area_Adversaria_HT { get; set; }
        public int? Passes_HT { get; set; }
        public int? Passes_Totais_HT { get; set; }
        public int? Precisao_Passes_HT { get; set; }
        public int? Passes_terco_Final_HT { get; set; }
        public int? Cruzamentos_HT { get; set; }
        public int? Desarmes_HT { get; set; }
        public int? Bolas_Afastadas_HT { get; set; }
        public int? Interceptacoes_HT { get; set; }

    }
}