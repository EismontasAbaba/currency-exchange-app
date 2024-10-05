using ExchangeRate.Client.Interface;
using ExchangeRate.Client.Models;
using Newtonsoft.Json;

namespace ExchangeRate.Client;

public class ExchangeRateClient(HttpClient httpClient, string accessKey) : IExchangeRateClient
{
    public const string ClientName = "ExchangeRateClient";

    public async Task<ExchangeRateResponse?> GetExchangeRates()
    {
        var response = await httpClient.GetAsync($"latest?access_key={accessKey}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        return string.IsNullOrEmpty(responseContent)
            ? null
            : JsonConvert.DeserializeObject<ExchangeRateResponse>(responseContent);
    }
}