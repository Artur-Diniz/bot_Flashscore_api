
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace botAPI.Models
{
    public class ml_features
    {
        public int id { get; set; }
        public int partida_id { get; set; }
        public string time_id { get; set; }
        public string features { get; set; } // JSONB no PostgreSQL
        public double? target { get; set; }
        public string versao_modelo { get; set; }
        public DateTime data_processamento { get; set; }

        [ForeignKey("Id_Partida")]
        public virtual Partida partida { get; set; }
    }
}