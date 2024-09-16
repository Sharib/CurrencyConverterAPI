using CurrencyConverterAPI.Domain;
using System.Diagnostics.Tracing;
using System.Security.Principal;

namespace CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;

public record GetExchangeRateResult(IEnumerable<Exchange> exchangeRates);