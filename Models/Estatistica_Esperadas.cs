
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Estatistica_Esperadas
    {
        public int Id { get; set; }
        public string NomeTime { get; set; }
          public int FT_Id { get; set; }
        public Estatistica_BaseModel FT { get; set; }
        public int HT_Id { get; set; }
        public Estatistica_BaseModel HT { get; set; }
        public int FT_Adversario_Id { get; set; }
        public Estatistica_BaseModel FT_Adversario { get; set; }
        public int HT_Adversario_Id { get; set; }
        public Estatistica_BaseModel HT_Adversario { get; set; }
        public int FT_Confronto_Id { get; set; }
        public Estatistica_BaseModel FT_Confronto { get; set; }
        public int HT_Confronto_Id { get; set; }
        public Estatistica_BaseModel HT_Confronto { get; set; }

    }
}