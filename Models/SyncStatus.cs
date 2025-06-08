using System.ComponentModel.DataAnnotations;

namespace botAPI.Models
{
    public class SyncStatus
    {

        [Key] // Define que esta é a chave primária
        public Guid Id { get; set; } // O tipo Guid é ideal para identificadores únicos


        public string Status { get; set; } = "not_started";
        public int Progress { get; set; } = 0;
        public string CurrentOperation { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Error { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}