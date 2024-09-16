using CurrencyConverterAPI.Domain;

namespace CurrencyConverterAPI.Application.Currency.Queries.GetExchangeRatesByDateInterval
{
    public record GetExchangeRatesByDateIntervalDto(CurrencyCode from, DateTime startDate, DateTime? endDate);
    public record GetExchangeRatesByDateIntervalResult(IEnumerable<Exchange> exchangeRates);
}
