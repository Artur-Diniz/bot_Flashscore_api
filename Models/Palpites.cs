
using System.ComponentModel.DataAnnotations;
using botAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace botAPI.Models
{
    public class Palpites
    {
        [Key]
        public int Id { get; set; }
        public int IdPartida { get; set; }
        public TipoAposta TipoAposta { get; set; }
        public double Num { get; set; }   //exemplo 2 gols casa que seria 1.5 +
        [MaxLength(510)]
        public string Descricao { get; set; } = string.Empty;

        public string GreenRed { get; set; } = string.Empty;
        public float? ODD { get; set; } 

        public DateTime DataPalpite { get; set; }

        public int MetodoGeradorPalpite_Id { get; set; }
        [ForeignKey("MetodoGeradorPalpite_Id")]
        public MetodoGeradorPalpites MetodoGerador { get; set; }



    }
}