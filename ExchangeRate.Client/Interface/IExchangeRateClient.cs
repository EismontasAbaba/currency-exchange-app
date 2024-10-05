using ExchangeRate.Client.Models;

namespace ExchangeRate.Client.Interface;

public interface IExchangeRateClient
{
    Task<ExchangeRateResponse?> GetExchangeRates();
}