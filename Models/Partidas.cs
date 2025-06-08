using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace botAPI.Models
{
    public class Partida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Adicione esta linha
        [Column("id")]
        public int Id { get; set; }
        public int Id_EstatisticaCasa { get; set; }
        public int Id_EstatisticaFora { get; set; }
        public string NomeTimeCasa { get; set; } = string.Empty;
        public string NomeTimeFora { get; set; } = string.Empty;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Url_Partida { get; set; } = string.Empty;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public DateTime DataPartida { get; set; }
        public string Campeonato { get; set; } = string.Empty;
        public bool PartidaAnalise { get; set; }
        public string TipoPartida { get; set; } = string.Empty;
    }
}