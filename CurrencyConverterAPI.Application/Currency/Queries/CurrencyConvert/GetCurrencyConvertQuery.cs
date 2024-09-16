using CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;
using CurrencyConverterAPI.Domain;

namespace CurrencyConverterAPI.Application.Users.Queries.CurrencyConvert;

public record GetCurrencyConvertQuery(decimal amount, CurrencyCode from, CurrencyCode to) : IRequest<GetCurrencyConvertResult>;