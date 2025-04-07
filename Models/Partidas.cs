
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Partida
    {
        [Key]
        public int Id { get; set; }
        public int Id_EstatisticaCasa { get; set; }
        public int Id_EstatisticaFora { get; set; }
        public string NomeTimeCasa { get; set; }= string.Empty;
        public string NomeTimeFora { get; set; }= string.Empty;
        public DateTime DataPartida { get; set; }
        public string Campeonato { get; set; }= string.Empty;
        public bool PartidaAnalise { get; set; }
        public string TipoPartida { get; set; }= string.Empty;
    }
}