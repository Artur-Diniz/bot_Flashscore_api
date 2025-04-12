
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Palpites
    {
        [Key]
        public int Id { get; set; }
        public int IdPartida { get; set; }
        public TipoAposta TipoAposta { get; set; }
        public double Num { get; set; }   //exemplo 2 gols casa que seria 1.5 +
        public string Descricao { get; set; } = string.Empty;

    }
}