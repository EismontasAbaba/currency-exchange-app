using ExchangeRate.Client.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Client.Extensions;

public static class ClientRegistrationHelper
{
    public static void RegisterExchangeRateClient(this IServiceCollection services, string apiUrl, string accessKey)
    {
        services.AddHttpClient(ExchangeRateClient.ClientName, (_, client) =>
        {
            client.BaseAddress = new Uri(apiUrl.AddApiEnding());
        });

        services.AddSingleton<IExchangeRateClient, ExchangeRateClient>(serviceProvider =>
        {
            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(ExchangeRateClient.ClientName);

            return new ExchangeRateClient(httpClient, accessKey);
        });
    }

    private static string AddApiEnding(this string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return url;
        }

        if (!url.EndsWith("/"))
        {
            url += "/";
        }

        return url;
    }
}