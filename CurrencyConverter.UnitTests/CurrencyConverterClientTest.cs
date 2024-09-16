using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Infrastructure.Services;
using Moq;
using RestSharp;
using System.Text.Json.Nodes;
using Xunit;

namespace CurrencyConverter.UnitTests
{
    public class FrankfurterClientTest
    {
        private readonly ICurrencyConverterClient _client;
        private readonly Mock<IApiClient> _mockHttpClient;

        public FrankfurterClientTest()
        {
            _mockHttpClient = new Mock<IApiClient>();
            _client = new CurrencyConverterClient(_mockHttpClient.Object);
        }

        [Fact]
        public async void GetExchangeRatesForCurrency_Success()
        {
            _mockHttpClient.Setup(_ =>
                _.GetAsync<JsonObject>(It.IsAny<RestRequest>()))
                .ReturnsAsync(JsonObjectExtension.GenerateCurrenciesJsonObject());

            var exchangeRates = await _client.GetExchangeRatesForCurrency("USD");

            Xunit.Assert.NotNull(exchangeRates);;
        }
    }
}