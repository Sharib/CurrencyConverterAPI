using CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;

namespace CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;

public record GetExchangeRateQuery(string currencyName) : IRequest<GetExchangeRateResult>;