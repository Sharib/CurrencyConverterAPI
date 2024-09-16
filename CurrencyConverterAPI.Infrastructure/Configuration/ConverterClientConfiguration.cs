namespace CurrencyConverterAPI.Infrastructure.Configuration
{
    public class ConverterClientConfiguration
    {
        public string BaseApiUrl { get; set; }
        public bool ThrowOnAnyError { get; set; }
        public int MaxTimeout { get; set; }

        public ConverterClientConfiguration(string baseUrl)
        {
            BaseApiUrl = baseUrl;

            SetupDefaultConfigs();
        }

        public ConverterClientConfiguration()
        {
            BaseApiUrl = "https://api.frankfurter.app/";

            SetupDefaultConfigs();
        }

        private void SetupDefaultConfigs()
        {
            MaxTimeout = 100000;
            ThrowOnAnyError = true;
        }
    }
}
