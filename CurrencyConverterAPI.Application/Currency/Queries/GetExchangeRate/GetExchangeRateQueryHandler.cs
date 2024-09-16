using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Application.Users.Queries.CurrencyConvert;
using CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;
using CurrencyConverterAPI.Domain;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyConverterAPI.Application.Users.Queries.GetUser;

public class GetExchangeRateQueryHandler(ICurrencyConverterClient currencyConverterClient) : IRequestHandler<GetExchangeRateQuery, GetExchangeRateResult>
{

    public async Task<GetExchangeRateResult> Handle(GetExchangeRateQuery request, CancellationToken cancellationToken = default)
    {
        var exchangeRates = await currencyConverterClient.GetExchangeRatesForCurrency(request.currencyName);

        return new GetExchangeRateResult(exchangeRates);
    }
}
