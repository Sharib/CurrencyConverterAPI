using CurrencyConverterAPI.Domain.Enums;

namespace CurrencyConverterAPI.Domain.Entities
{
    public class MaintenanceIncident
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public IncidentType Type { get; set; }
        public string ServiceCentre { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public IncidentStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? ReviewNotes { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string? ReviewedBy { get; set; }
    }
}
