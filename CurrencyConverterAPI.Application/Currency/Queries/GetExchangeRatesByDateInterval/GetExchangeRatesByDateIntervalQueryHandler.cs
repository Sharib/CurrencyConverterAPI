using CurrencyConverterAPI.Application.Common.Interfaces;

namespace CurrencyConverterAPI.Application.Currency.Queries.GetExchangeRatesByDateInterval
{

    public class GetExchangeRatesByDateIntervalQueryHandler(ICurrencyConverterClient currencyConverterClient) : IRequestHandler<GetExchangeRatesByDateIntervalQuery, GetExchangeRatesByDateIntervalResult>
    {

        public async Task<GetExchangeRatesByDateIntervalResult> Handle(GetExchangeRatesByDateIntervalQuery request, CancellationToken cancellationToken = default)
        {
            var exchangeRates = await currencyConverterClient.GetExchangeRatesByDateIntervalAsync(request.from, request.startDate, request.endDate);

            return new GetExchangeRatesByDateIntervalResult(exchangeRates);
        }

    }
}
