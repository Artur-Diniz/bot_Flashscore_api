
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Estatistica_Times
    {
        [Key]
        public int Id { get; set; }

        public string CasaOuFora { get; set; }= string.Empty;
        public string NomeTime { get; set; }= string.Empty;

        public float Gol { get; set; }
        public float GolSofrido { get; set; }
        public float Posse_Bola { get; set; }
        public float Total_Finalizacao { get; set; }
        public float Chances_Claras { get; set; }
        public float Escanteios { get; set; }
        public float Bolas_trave { get; set; }
        public float Gols_de_cabeça { get; set; }
        public float Defesas_Goleiro { get; set; }
        public float Impedimentos { get; set; }
        public float Faltas { get; set; }
        public float Cartoes_Amarelos { get; set; }
        public float Cartoes_Vermelhos { get; set; }
        public float Laterais_Cobrados { get; set; }
        public float Toque_Area_Adversaria { get; set; }
        public float Passes { get; set; }
        public float Passes_Totais { get; set; }
        public float Precisao_Passes { get; set; }
        public float Passes_terco_Final { get; set; }
        public float Cruzamentos { get; set; }
        public float Desarmes { get; set; }
        public float Bolas_Afastadas { get; set; }
        public float Interceptacoes { get; set; }

        public float Gol_Adversaria { get; set; }
        public float GolSofrido_Adversaria { get; set; }
        public float Posse_Bola_Adversaria { get; set; }
        public float Total_Finalizacao_Adversaria { get; set; }
        public float Chances_Claras_Adversaria { get; set; }
        public float Escanteios_Adversaria { get; set; }
        public float Bolas_trave_Adversaria { get; set; }
        public float Gols_de_cabeça_Adversaria { get; set; }
        public float Defesas_Goleiro_Adversaria { get; set; }
        public float Impedimentos_Adversaria { get; set; }
        public float Faltas_Adversaria { get; set; }
        public float Cartoes_Amarelos_Adversaria { get; set; }
        public float Cartoes_Vermelhos_Adversaria { get; set; }
        public float Laterais_Cobrados_Adversaria { get; set; }
        public float Toque_Area_Adversaria_Adversaria { get; set; }
        public float Passes_Adversaria { get; set; }
        public float Passes_Totais_Adversaria { get; set; }
        public float Precisao_Passes_Adversaria { get; set; }
        public float Passes_terco_Final_Adversaria { get; set; }
        public float Cruzamentos_Adversaria { get; set; }
        public float Desarmes_Adversaria { get; set; }
        public float Bolas_Afastadas_Adversaria { get; set; }
        public float Interceptacoes_Adversaria { get; set; }

        public float Gol_Confrontos { get; set; }
        public float GolSofrido_Confrontos { get; set; }
        public float Posse_Bola_Confrontos { get; set; }
        public float Total_Finalizacao_Confrontos { get; set; }
        public float Chances_Claras_Confrontos { get; set; }
        public float Escanteios_Confrontos { get; set; }
        public float Bolas_trave_Confrontos { get; set; }
        public float Gols_de_cabeça_Confrontos { get; set; }
        public float Defesas_Goleiro_Confrontos { get; set; }
        public float Impedimentos_Confrontos { get; set; }
        public float Faltas_Confrontos { get; set; }
        public float Cartoes_Amarelos_Confrontos { get; set; }
        public float Cartoes_Vermelhos_Confrontos { get; set; }
        public float Laterais_Cobrados_Confrontos { get; set; }
        public float Toque_Area_Adversaria_Confrontos { get; set; }
        public float Passes_Confrontos { get; set; }
        public float Passes_Totais_Confrontos { get; set; }
        public float Precisao_Passes_Confrontos { get; set; }
        public float Passes_terco_Final_Confrontos { get; set; }
        public float Cruzamentos_Confrontos { get; set; }
        public float Desarmes_Confrontos { get; set; }
        public float Bolas_Afastadas_Confrontos { get; set; }
        public float Interceptacoes_Confrontos { get; set; }
    }
}