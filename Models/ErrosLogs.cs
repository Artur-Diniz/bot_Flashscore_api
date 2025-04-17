using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class ErrosLogs
    {
        [Key]
        public int Id { get; set; }
        public string QualPageFoi { get; set; } = string.Empty;
        public string QualUrl { get; set; } = string.Empty;
        [MaxLength(510)]
        public string OqueProvavelmenteAConteceu { get; set; } = string.Empty;
   

    }
}