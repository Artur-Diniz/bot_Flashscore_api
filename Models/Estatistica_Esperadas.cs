
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Estatistica_Esperadas
    {
        public int Id { get; set; }
        public string NomeTime { get; set; }
        public Estatistica_BaseModel FT { get; set; }
        public Estatistica_BaseModel HT { get; set; }
        public Estatistica_BaseModel FT_Adversario { get; set; }
        public Estatistica_BaseModel HT_Adversario{ get; set; }
        public Estatistica_BaseModel FT_Confronto { get; set; }
        public Estatistica_BaseModel HT_Confronto{ get; set; }

    }
}