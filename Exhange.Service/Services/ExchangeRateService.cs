using Exchange.Service.Exceptions;
using Exchange.Service.Interfaces;
using ExchangeRate.Client.Interface;

namespace Exchange.Service.Services;

public class ExchangeRateService(
    IExchangeRateClient exchangeRateClient,
    ICacheService cacheService) : IExchangeRateService
{
    public async Task<decimal> GetExchangeRate(string fromIsoCode, string toIsoCode)
    {
        var exchangeRates = await FetchExchangeRates();

        var hasFromRate = exchangeRates.TryGetValue(fromIsoCode.ToUpperInvariant(), out var fromRate);
        var hasToRate = exchangeRates.TryGetValue(toIsoCode.ToUpperInvariant(), out var toRate);

        if (!hasFromRate || !hasToRate)
        {
            throw new BusinessException("Exchange rates not found or currency is not supported!");
        }

        return toRate / fromRate;
    }

    private async Task<IDictionary<string, decimal>> FetchExchangeRates()
    {
        try
        {
            var exchangeRates = await cacheService.GetOrCreateAsync("ExchangeRates", exchangeRateClient.GetExchangeRates);

            if (exchangeRates is not { Success: true })
            {
                throw new BusinessException("Exchange rates are not available at the moment!");
            }

            return exchangeRates.Rates;
        }
        catch (Exception exception)
        {
            if (exception is BusinessException)
            {
                throw;
            }

            throw new Exception("Exchange rate fetch error", exception);
        }
    }
}