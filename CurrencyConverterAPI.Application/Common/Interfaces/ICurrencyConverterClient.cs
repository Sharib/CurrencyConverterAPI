using CurrencyConverterAPI.Domain;
using CurrencyConverterAPI.Domain.Entities;

namespace CurrencyConverterAPI.Application.Common.Interfaces;

public interface ICurrencyConverterClient
{
    Task<IEnumerable<Exchange>> GetExchangeRatesForCurrency(string currencyName);
    Task<Exchange> CurrencyConvertAsync(decimal amount, CurrencyCode from, CurrencyCode to);
    Task<IEnumerable<Exchange>> GetExchangeRatesByDateIntervalAsync(CurrencyCode from, DateTime startDate, DateTime endDate);
}
