using CurrencyConverterAPI.Domain;

namespace CurrencyConverterAPI.Application.Currency.Queries.GetExchangeRatesByDateInterval
{

    public record GetExchangeRatesByDateIntervalQuery(CurrencyCode from, DateTime startDate, DateTime endDate) : IRequest<GetExchangeRatesByDateIntervalResult>;
}
