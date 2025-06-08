using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace botAPI.Models
{
    public class Estatistica_BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public float? Gol { get; set; }
        public float? Gol_Slope { get; set; }
        public float? Gol_DP { get; set; }
        public float? GolSofrido { get; set; }
        public float? GolSofrido_Slope { get; set; }
        public float? GolSofrido_DP { get; set; }
        public float? Posse_Bola { get; set; }
        public float? Posse_Bola_Slope { get; set; }
        public float? Posse_Bola_DP { get; set; }
        public float? Total_Finalizacao { get; set; }
        public float? Total_Finalizacao_Slope { get; set; }
        public float? Total_Finalizacao_DP { get; set; }
        public float? Chances_Claras { get; set; }
        public float? Escanteios { get; set; }
        public float? Escanteios_Slope { get; set; }
        public float? Escanteios_DP { get; set; }
        public float? Bolas_trave { get; set; }
        public float? Gols_de_cabeca { get; set; }
        public float? Defesas_Goleiro { get; set; }
        public float? Impedimentos { get; set; }
        public float? Impedimentos_Slope { get; set; }
        public float? Impedimentos_DP { get; set; }
        public float? Faltas { get; set; }
        public float? Faltas_Slope { get; set; }
        public float? Faltas_DP { get; set; }
        public float? Cartoes_Amarelos { get; set; }
        public float? Cartoes_Amarelos_Slope { get; set; }
        public float? Cartoes_Amarelos_DP { get; set; }
        public float? Cartoes_Vermelhos { get; set; }
        public float? Cartoes_Vermelhos_Slope { get; set; }
        public float? Cartoes_Vermelhos_DP { get; set; }
        public float? Laterais_Cobrados { get; set; }
        public float? Toque_Area_Adversaria { get; set; }
        public float? Passes { get; set; }
        public float? Passes_Totais { get; set; }
        public float? Precisao_Passes { get; set; }
        public float? Passes_terco_Final { get; set; }
        public float? Cruzamentos { get; set; }
        public float? Desarmes { get; set; }
        public float? Desarmes_Slope { get; set; }
        public float? Desarmes_DP { get; set; }
        public float? Bolas_Afastadas { get; set; }
        public float? Interceptacoes { get; set; }

    }
}