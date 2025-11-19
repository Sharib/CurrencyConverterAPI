using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Application.MaintenanceIncident.Commands.ApproveIncident;
using CurrencyConverterAPI.Application.MaintenanceIncident.Commands.CreateIncident;
using CurrencyConverterAPI.Application.MaintenanceIncident.Queries.GetIncident;
using CurrencyConverterAPI.Application.MaintenanceIncident.Queries.ListIncidents;
using CurrencyConverterAPI.Domain.Enums;
using CurrencyConverterAPI.Infrastructure.Repositories;

namespace CurrencyConverter.UnitTests
{
    public class MaintenanceIncidentTests
    {
        private readonly IMaintenanceIncidentRepository _repository;

        public MaintenanceIncidentTests()
        {
            _repository = new InMemoryMaintenanceIncidentRepository();
        }

        [Xunit.Fact]
        public async Task CreateIncident_Success()
        {
            // Arrange
            var command = new CreateIncidentCommand(
                "Engine failure",
                IncidentType.Repair,
                "Sydney Service Centre",
                5000m
            );
            var handler = new CreateIncidentCommandHandler(_repository);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Xunit.Assert.NotEqual(Guid.Empty, result.IncidentId);
            Xunit.Assert.Equal("Incident created successfully", result.Message);
        }

        [Xunit.Fact]
        public async Task GetIncident_ReturnsCreatedIncident()
        {
            // Arrange
            var createCommand = new CreateIncidentCommand(
                "Warranty claim",
                IncidentType.WarrantyClaim,
                "Melbourne Service Centre",
                3000m
            );
            var createHandler = new CreateIncidentCommandHandler(_repository);
            var createResult = await createHandler.Handle(createCommand, default);

            var getQuery = new GetIncidentQuery(createResult.IncidentId);
            var getHandler = new GetIncidentQueryHandler(_repository);

            // Act
            var result = await getHandler.Handle(getQuery, default);

            // Assert
            Xunit.Assert.NotNull(result.Incident);
            Xunit.Assert.Equal("Warranty claim", result.Incident.Description);
            Xunit.Assert.Equal(IncidentStatus.Draft, result.Incident.Status);
        }

        [Xunit.Fact]
        public async Task ApproveIncident_Success()
        {
            // Arrange
            var createCommand = new CreateIncidentCommand(
                "Part failure",
                IncidentType.PartFailure,
                "Brisbane Service Centre",
                2500m
            );
            var createHandler = new CreateIncidentCommandHandler(_repository);
            var createResult = await createHandler.Handle(createCommand, default);

            var approveCommand = new ApproveIncidentCommand(
                createResult.IncidentId,
                "John Manager",
                "Approved for reimbursement"
            );
            var approveHandler = new ApproveIncidentCommandHandler(_repository);

            // Act
            var result = await approveHandler.Handle(approveCommand, default);

            // Assert
            Xunit.Assert.True(result.Success);
            Xunit.Assert.Equal("Incident approved successfully", result.Message);

            var getQuery = new GetIncidentQuery(createResult.IncidentId);
            var getHandler = new GetIncidentQueryHandler(_repository);
            var incident = await getHandler.Handle(getQuery, default);
            
            Xunit.Assert.Equal(IncidentStatus.Approved, incident.Incident.Status);
            Xunit.Assert.Equal("John Manager", incident.Incident.ReviewedBy);
        }

        [Xunit.Fact]
        public async Task ListIncidents_ReturnsAllIncidents()
        {
            // Arrange
            var handler = new CreateIncidentCommandHandler(_repository);
            await handler.Handle(new CreateIncidentCommand("Incident 1", IncidentType.Repair, "Centre A", 1000m), default);
            await handler.Handle(new CreateIncidentCommand("Incident 2", IncidentType.WarrantyClaim, "Centre B", 2000m), default);

            var listQuery = new ListIncidentsQuery();
            var listHandler = new ListIncidentsQueryHandler(_repository);

            // Act
            var result = await listHandler.Handle(listQuery, default);

            // Assert
            Xunit.Assert.True(result.Incidents.Count() >= 2);
        }

        [Xunit.Fact]
        public async Task ListIncidents_FilterByStatus()
        {
            // Arrange
            var createHandler = new CreateIncidentCommandHandler(_repository);
            var incident1 = await createHandler.Handle(
                new CreateIncidentCommand("Incident 1", IncidentType.Repair, "Centre A", 1000m), default);
            var incident2 = await createHandler.Handle(
                new CreateIncidentCommand("Incident 2", IncidentType.WarrantyClaim, "Centre B", 2000m), default);

            var approveHandler = new ApproveIncidentCommandHandler(_repository);
            await approveHandler.Handle(
                new ApproveIncidentCommand(incident1.IncidentId, "Manager", "OK"), default);

            var listQuery = new ListIncidentsQuery(IncidentStatus.Approved);
            var listHandler = new ListIncidentsQueryHandler(_repository);

            // Act
            var result = await listHandler.Handle(listQuery, default);

            // Assert
            Xunit.Assert.Single(result.Incidents.Where(i => i.Status == IncidentStatus.Approved));
        }
    }
}
