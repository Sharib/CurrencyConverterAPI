using CleanArchitecture.Presentation.Controllers;
using CurrencyConverterAPI.Application.MaintenanceIncident.Commands.ApproveIncident;
using CurrencyConverterAPI.Application.MaintenanceIncident.Commands.CreateIncident;
using CurrencyConverterAPI.Application.MaintenanceIncident.Queries.GetIncident;
using CurrencyConverterAPI.Application.MaintenanceIncident.Queries.ListIncidents;
using CurrencyConverterAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceIncidentController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<CreateIncidentResult>> CreateIncident([FromBody] CreateIncidentCommand command) 
            => await Mediator.Send(command);

        [HttpGet("{id}")]
        public async Task<ActionResult<GetIncidentResult>> GetIncident(Guid id) 
            => await Mediator.Send(new GetIncidentQuery(id));

        [HttpGet]
        public async Task<ActionResult<ListIncidentsResult>> ListIncidents([FromQuery] IncidentStatus? status = null) 
            => await Mediator.Send(new ListIncidentsQuery(status));

        [HttpPost("approve")]
        public async Task<ActionResult<ApproveIncidentResult>> ApproveIncident([FromBody] ApproveIncidentCommand command) 
            => await Mediator.Send(command);
    }
}
