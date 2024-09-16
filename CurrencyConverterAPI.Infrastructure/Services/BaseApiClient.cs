using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Infrastructure.Configuration;
using Flurl;
using RestSharp;

namespace CurrencyConverterAPI.Infrastructure.Services
{
    public abstract class BaseApiClient
    {
        private readonly IApiClient _httpClient;
        protected readonly Url Endpoint;

        protected BaseApiClient(ConverterClientConfiguration configuration)
        {
            _httpClient = new ApiClient(configuration);
            Endpoint = _httpClient.GetBaseUrl();
        }

        protected BaseApiClient(string baseUrl)
        {
            _httpClient = new ApiClient(baseUrl);
            Endpoint = _httpClient.GetBaseUrl();
        }


        protected BaseApiClient(IApiClient restApiClient)
        {
            _httpClient = restApiClient;
            Endpoint = _httpClient.GetBaseUrl();
        }

        protected BaseApiClient()
        {
            _httpClient = new ApiClient();
            Endpoint = _httpClient.GetBaseUrl();
        }

        protected Task<T> GetAsync<T>()
        {
            var restRequest = new RestRequest(Endpoint.ToString());
            RefreshEndpoint();

            return _httpClient.GetAsync<T>(restRequest);
        }

        private void RefreshEndpoint()
        {
            Endpoint.Reset();
        }
    }
}
