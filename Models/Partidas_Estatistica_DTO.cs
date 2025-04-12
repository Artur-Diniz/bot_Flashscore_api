
using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class Partida_Estatistica_DTO
    {
        public Estatistica EstatisticaCasa { get; set; }
        public Estatistica EstatisticaFora { get; set; }
        public Partida Partida { get; set; }

    }
}