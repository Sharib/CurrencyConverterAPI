namespace CurrencyConverterAPI.Presentation
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
