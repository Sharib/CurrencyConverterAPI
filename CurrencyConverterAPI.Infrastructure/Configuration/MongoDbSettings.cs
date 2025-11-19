namespace CurrencyConverterAPI.Infrastructure.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "MaintenanceIncidentDb";
    public string IncidentsCollectionName { get; set; } = "Incidents";
}
