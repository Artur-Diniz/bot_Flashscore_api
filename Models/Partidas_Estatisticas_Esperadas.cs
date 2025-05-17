
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Partida_Estatistica_Esperadas
    {
        public int Id { get; set; }


        public int Id_Partida { get; set; }
        public Partida Partida { get; set; }

        public int Id_Estatisticas_Esperadas_Casa { get; set; }
        public Estatistica_Esperadas Estatisticas_Esperadas_Casa { get; set; }


        public int Id_Estatisticas_Esperadas_Fora { get; set; }
        public Estatistica_Esperadas Estatisticas_Esperadas_Fora { get; set; }

        public int Id_Partida_FT { get; set; }
        public Estatistica_BaseModel Partida_FT { get; set; }
        public int Id_Partida_HT { get; set; }
        public Estatistica_BaseModel Partida_HT { get; set; }
        public int Id_Partida_FT_Confronto { get; set; }
        public Estatistica_BaseModel Partida_FT_Confronto { get; set; }
        public int Id_Partida_HT_Confronto { get; set; }
        public Estatistica_BaseModel Partida_HT_Confronto { get; set; }





    }
}