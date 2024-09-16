using RestSharp;

namespace CurrencyConverterAPI.Application.Common.Interfaces
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(RestRequest request);
        string GetBaseUrl();
    }
}
