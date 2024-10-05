using Exchange.Service.Interfaces;
using Exchange.Service.Services;
using Exchange.Service.Settings;
using ExchangeRate.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Service.Extensions;

public static class ServiceRegistrationExtensions
{
    public static void RegisterAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = configuration.Get<AppSettings>();

        services.RegisterCommonService(appSettings);
        services.RegisterHttpClients(appSettings);
        services.RegisterBusinessLogicServices();
    }

    private static void RegisterCommonService(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton(appSettings);

        services.AddMemoryCache();

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void RegisterHttpClients(this IServiceCollection services, AppSettings appSettings)
    {
        services.RegisterExchangeRateClient(appSettings.ExchangeRateApiUrl, appSettings.ExchangeRateApiAccessKey);
    }

    private static void RegisterBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        services.AddScoped<IExchangeService, ExchangeService>();
    }
}