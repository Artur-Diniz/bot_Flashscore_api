
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Estatistica_Times
    {
        [Key]
        public int Id { get; set; }

        public string CasaOuFora { get; set; } = string.Empty;
        public string NomeTime { get; set; } = string.Empty;
        public bool Analisada { get; set; } = false;

        
        public float? Gol { get; set; }
        public float? GolSofrido { get; set; }
        public float? Posse_Bola { get; set; }
        public float? Total_Finalizacao { get; set; }
        public float? Chances_Claras { get; set; }
        public float? Escanteios { get; set; }
        public float? Bolas_trave { get; set; }
        public float? Gols_de_cabeça { get; set; }
        public float? Defesas_Goleiro { get; set; }
        public float? Impedimentos { get; set; }
        public float? Faltas { get; set; }
        public float? Cartoes_Amarelos { get; set; }
        public float? Cartoes_Vermelhos { get; set; }
        public float? Laterais_Cobrados { get; set; }
        public float? Toque_Area_Adversaria { get; set; }
        public float? Passes { get; set; }
        public float? Passes_Totais { get; set; }
        public float? Precisao_Passes { get; set; }
        public float? Passes_terco_Final { get; set; }
        public float? Cruzamentos { get; set; }
        public float? Desarmes { get; set; }
        public float? Bolas_Afastadas { get; set; }
        public float? Interceptacoes { get; set; }

        public float? Gol_Adversaria { get; set; }
        public float? GolSofrido_Adversaria { get; set; }
        public float? Posse_Bola_Adversaria { get; set; }
        public float? Total_Finalizacao_Adversaria { get; set; }
        public float? Chances_Claras_Adversaria { get; set; }
        public float? Escanteios_Adversaria { get; set; }
        public float? Bolas_trave_Adversaria { get; set; }
        public float? Gols_de_cabeça_Adversaria { get; set; }
        public float? Defesas_Goleiro_Adversaria { get; set; }
        public float? Impedimentos_Adversaria { get; set; }
        public float? Faltas_Adversaria { get; set; }
        public float? Cartoes_Amarelos_Adversaria { get; set; }
        public float? Cartoes_Vermelhos_Adversaria { get; set; }
        public float? Laterais_Cobrados_Adversaria { get; set; }
        public float? Toque_Area_Adversaria_Adversaria { get; set; }
        public float? Passes_Adversaria { get; set; }
        public float? Passes_Totais_Adversaria { get; set; }
        public float? Precisao_Passes_Adversaria { get; set; }
        public float? Passes_terco_Final_Adversaria { get; set; }
        public float? Cruzamentos_Adversaria { get; set; }
        public float? Desarmes_Adversaria { get; set; }
        public float? Bolas_Afastadas_Adversaria { get; set; }
        public float? Interceptacoes_Adversaria { get; set; }

        public float? Gol_Confrontos { get; set; }
        public float? GolSofrido_Confrontos { get; set; }
        public float? Posse_Bola_Confrontos { get; set; }
        public float? Total_Finalizacao_Confrontos { get; set; }
        public float? Chances_Claras_Confrontos { get; set; }
        public float? Escanteios_Confrontos { get; set; }
        public float? Bolas_trave_Confrontos { get; set; }
        public float? Gols_de_cabeça_Confrontos { get; set; }
        public float? Defesas_Goleiro_Confrontos { get; set; }
        public float? Impedimentos_Confrontos { get; set; }
        public float? Faltas_Confrontos { get; set; }
        public float? Cartoes_Amarelos_Confrontos { get; set; }
        public float? Cartoes_Vermelhos_Confrontos { get; set; }
        public float? Laterais_Cobrados_Confrontos { get; set; }
        public float? Toque_Area_Adversaria_Confrontos { get; set; }
        public float? Passes_Confrontos { get; set; }
        public float? Passes_Totais_Confrontos { get; set; }
        public float? Precisao_Passes_Confrontos { get; set; }
        public float? Passes_terco_Final_Confrontos { get; set; }
        public float? Cruzamentos_Confrontos { get; set; }
        public float? Desarmes_Confrontos { get; set; }
        public float? Bolas_Afastadas_Confrontos { get; set; }
        public float? Interceptacoes_Confrontos { get; set; }



        public float? Gol_HT { get; set; }
        public float? GolSofrido_HT { get; set; }
        public float? Posse_Bola_HT { get; set; }
        public float? Total_Finalizacao_HT { get; set; }
        public float? Chances_Claras_HT { get; set; }
        public float? Escanteios_HT { get; set; }
        public float? Bolas_trave_HT { get; set; }
        public float? Gols_de_cabeça_HT { get; set; }
        public float? Defesas_Goleiro_HT { get; set; }
        public float? Impedimentos_HT { get; set; }
        public float? Faltas_HT { get; set; }
        public float? Cartoes_Amarelos_HT { get; set; }
        public float? Cartoes_Vermelhos_HT { get; set; }
        public float? Laterais_Cobrados_HT { get; set; }
        public float? Toque_Area_Adversaria_HT { get; set; }
        public float? Passes_HT { get; set; }
        public float? Passes_Totais_HT { get; set; }
        public float? Precisao_Passes_HT { get; set; }
        public float? Passes_terco_Final_HT { get; set; }
        public float? Cruzamentos_HT { get; set; }
        public float? Desarmes_HT { get; set; }
        public float? Bolas_Afastadas_HT { get; set; }
        public float? Interceptacoes_HT { get; set; }

        public float? Gol_Adversaria_HT { get; set; }
        public float? GolSofrido_Adversaria_HT { get; set; }
        public float? Posse_Bola_Adversaria_HT { get; set; }
        public float? Total_Finalizacao_Adversaria_HT { get; set; }
        public float? Chances_Claras_Adversaria_HT { get; set; }
        public float? Escanteios_Adversaria_HT { get; set; }
        public float? Bolas_trave_Adversaria_HT { get; set; }
        public float? Gols_de_cabeça_Adversaria_HT { get; set; }
        public float? Defesas_Goleiro_Adversaria_HT { get; set; }
        public float? Impedimentos_Adversaria_HT { get; set; }
        public float? Faltas_Adversaria_HT { get; set; }
        public float? Cartoes_Amarelos_Adversaria_HT { get; set; }
        public float? Cartoes_Vermelhos_Adversaria_HT { get; set; }
        public float? Laterais_Cobrados_Adversaria_HT { get; set; }
        public float? Toque_Area_Adversaria_Adversaria_HT { get; set; }
        public float? Passes_Adversaria_HT { get; set; }
        public float? Passes_Totais_Adversaria_HT { get; set; }
        public float? Precisao_Passes_Adversaria_HT { get; set; }
        public float? Passes_terco_Final_Adversaria_HT { get; set; }
        public float? Cruzamentos_Adversaria_HT { get; set; }
        public float? Desarmes_Adversaria_HT { get; set; }
        public float? Bolas_Afastadas_Adversaria_HT { get; set; }
        public float? Interceptacoes_Adversaria_HT { get; set; }

        public float? Gol_Confrontos_HT { get; set; }
        public float? GolSofrido_Confrontos_HT { get; set; }
        public float? Posse_Bola_Confrontos_HT { get; set; }
        public float? Total_Finalizacao_Confrontos_HT { get; set; }
        public float? Chances_Claras_Confrontos_HT { get; set; }
        public float? Escanteios_Confrontos_HT { get; set; }
        public float? Bolas_trave_Confrontos_HT { get; set; }
        public float? Gols_de_cabeça_Confrontos_HT { get; set; }
        public float? Defesas_Goleiro_Confrontos_HT { get; set; }
        public float? Impedimentos_Confrontos_HT { get; set; }
        public float? Faltas_Confrontos_HT { get; set; }
        public float? Cartoes_Amarelos_Confrontos_HT { get; set; }
        public float? Cartoes_Vermelhos_Confrontos_HT { get; set; }
        public float? Laterais_Cobrados_Confrontos_HT { get; set; }
        public float? Toque_Area_Adversaria_Confrontos_HT { get; set; }
        public float? Passes_Confrontos_HT { get; set; }
        public float? Passes_Totais_Confrontos_HT { get; set; }
        public float? Precisao_Passes_Confrontos_HT { get; set; }
        public float? Passes_terco_Final_Confrontos_HT { get; set; }
        public float? Cruzamentos_Confrontos_HT { get; set; }
        public float? Desarmes_Confrontos_HT { get; set; }
        public float? Bolas_Afastadas_Confrontos_HT { get; set; }
        public float? Interceptacoes_Confrontos_HT { get; set; }
    }
}