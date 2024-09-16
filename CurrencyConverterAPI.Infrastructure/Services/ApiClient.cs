using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Infrastructure.Configuration;
using Polly;
using RestSharp;
using System.Net.Http;
using System.Net;

namespace CurrencyConverterAPI.Infrastructure.Services
{
    public class ApiClient : IApiClient
    {
        private readonly RestClient _client;
        private readonly ConverterClientConfiguration _configuration;

        //Set some defaults 
        private static int _maxRetryAttempts = 5;
        private static TimeSpan _pauseBetweenFailures = TimeSpan.FromSeconds(2);


        public ApiClient(ConverterClientConfiguration configuration)
        {
            _configuration = configuration;
            _client = new RestClient(GetConfigurations());
        }

        public ApiClient()
        {
            _configuration = new ConverterClientConfiguration();
            _client = new RestClient(GetConfigurations());
        }

        public ApiClient(string baseUrl)
        {
            _configuration = new ConverterClientConfiguration(baseUrl);
            _client = new RestClient(GetConfigurations());
        }

        public Task<T> GetAsync<T>(RestRequest request)
        {
            return _client.GetAsync<T>(request);
        }

        // Polly for retry mechanism
        //public Task<T> GetAsync<T>(RestRequest request)
        //{
        //    return _client.ExecuteAsync<T>(request);

        //    //var retryPolicy = Policy
        //    //        .HandleResult<RestResponse<T>>(x => !x.IsSuccessful)
        //    //        .WaitAndRetryAsync(_maxRetryAttempts, x => _pauseBetweenFailures, (iRestResponse, timeSpan, retryCount, context) =>
        //    //        {
        //    //            throw new Exception($"The request failed. HttpStatusCode={iRestResponse.Result.StatusCode}. Waiting {timeSpan} seconds before retry. Number attempt {retryCount}. Uri={iRestResponse.Result.ResponseUri}; RequestResponse={iRestResponse.Result.Content}");
        //    //        });

        //    //var response = retryPolicy.ExecuteAsync(() => _client.ExecuteAsync<T>(request));

        //    //return Task.FromResult<T>(response.Result);
        //}

        public string GetBaseUrl()
        {
            return _configuration.BaseApiUrl;
        }

        private RestClientOptions GetConfigurations()
        {
            return new RestClientOptions(_configuration.BaseApiUrl)
            {
                ThrowOnAnyError = _configuration.ThrowOnAnyError,
                Timeout = TimeSpan.FromMinutes(10)
            };
        }
    }
}
