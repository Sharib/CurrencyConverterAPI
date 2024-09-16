using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Application.Users.Queries.CurrencyConvert;
using CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyConverterAPI.Application.Users.Queries.GetUser;

public class GetCurrencyConvertQueryHandler(ICurrencyConverterClient currencyConverterClient) : IRequestHandler<GetCurrencyConvertQuery, GetCurrencyConvertResult>
{

    public async Task<GetCurrencyConvertResult> Handle(GetCurrencyConvertQuery request, CancellationToken cancellationToken = default)
    {
        var exchangeRates = await currencyConverterClient.CurrencyConvertAsync(request.amount, request.from, request.to);

        return new GetCurrencyConvertResult(exchangeRates);
    }
}
