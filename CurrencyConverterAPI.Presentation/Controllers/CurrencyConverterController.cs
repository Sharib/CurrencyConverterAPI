using CleanArchitecture.Presentation.Controllers;
using CurrencyConverterAPI.Application.Currency.Queries.GetExchangeRatesByDateInterval;
using CurrencyConverterAPI.Application.Users.Queries.CurrencyConvert;
using CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConverterController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<GetExchangeRateResult>> GetExchangeRatesByCurrency([FromQuery] GetExchangeRateQuery query) => await Mediator.Send(query);

        [HttpGet]
        public async Task<ActionResult<GetCurrencyConvertResult>> GetCurrencyExchangeRates([FromQuery] GetCurrencyConvertQuery query) => await Mediator.Send(query);

        [HttpGet]
        public async Task<ActionResult<GetExchangeRatesByDateIntervalResult>> GetExchangeRatesByDateInterval([FromQuery] GetExchangeRatesByDateIntervalQuery query) => await Mediator.Send(query);
    }
}