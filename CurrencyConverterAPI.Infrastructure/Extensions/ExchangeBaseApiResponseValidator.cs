using CurrencyConverterAPI.Infrastructure.DTO.Response;

namespace CurrencyConverterAPI.Infrastructure.Extensions
{
    internal static class ExchangeBaseApiResponseValidator
    {
        internal static bool IsNull(this ExchangeBaseApiResponse exchangeBaseApiResponse)
        {
            if (exchangeBaseApiResponse?.Rates == null) return true;
            if (exchangeBaseApiResponse.Amount == decimal.Zero) return true;
            return exchangeBaseApiResponse.Currency == null;
        }
    }
}
