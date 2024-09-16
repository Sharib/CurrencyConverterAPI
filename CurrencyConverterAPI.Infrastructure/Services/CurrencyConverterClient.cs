using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Domain;
using CurrencyConverterAPI.Domain.Common;
using CurrencyConverterAPI.Infrastructure.Configuration;
using CurrencyConverterAPI.Infrastructure.DTO.Response;
using CurrencyConverterAPI.Infrastructure.Extensions;

namespace CurrencyConverterAPI.Infrastructure.Services
{
    public class CurrencyConverterClient : BaseApiClient, ICurrencyConverterClient
    {
        public CurrencyConverterClient() : base() { }
        public CurrencyConverterClient(string baseUrl) : base(baseUrl) { }
        public CurrencyConverterClient(ConverterClientConfiguration configuration) : base(configuration) { }
        public CurrencyConverterClient(IApiClient restApiClient) : base(restApiClient) { }

        public async Task<IEnumerable<Exchange>> GetExchangeRatesForCurrency(string currencyName)
        {
            Endpoint.AppendPathSegment(Routes.LatestEndpoint);
            if (!string.IsNullOrEmpty(currencyName))
            {
                Endpoint.SetQueryParam("from", currencyName);
            }

            var response = await GetAsync<ExchangeBaseApiResponse>();

            return response?.ToExchangeList();
        }

        public async Task<Exchange> CurrencyConvertAsync(decimal amount, CurrencyCode from, CurrencyCode to)
        {
            Endpoint.AppendPathSegment(Routes.LatestEndpoint);

            if (amount > decimal.Zero) Endpoint.SetQueryParam("amount", amount);
            if (from != CurrencyCode.NONE) Endpoint.SetQueryParam("from", from.ToString());
            if (to != CurrencyCode.NONE) Endpoint.SetQueryParam("to", to.ToString());

            var response = await GetAsync<ExchangeBaseApiResponse>()
                .ConfigureAwait(false);

            return response?.ToExchange();
        }

        public async Task<IEnumerable<Exchange>> GetExchangeRatesByDateIntervalAsync(CurrencyCode from, DateTime startDate, DateTime endDate)
        {
            Endpoint.AppendPathSegment(Routes.RootEndpoint);

            Endpoint.AppendPathSegment(
                EndpointParameterBuilder.DateIntervalToString(startDate, endDate)
            );

            if (from != CurrencyCode.NONE) Endpoint.SetQueryParam("from", from.ToString());

            var response = await GetAsync<ExchangeBaseApiResponse>()
                .ConfigureAwait(false);

            return response.IsNull() ? null : response.ToExchangeList();
        }
    }
}
