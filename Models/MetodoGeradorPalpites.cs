using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class MetodoGeradorPalpites
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Versao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Condicoes { get; set; } = string.Empty;
    }
}