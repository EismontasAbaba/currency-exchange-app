using Exchange.Service.Exceptions;
using Exchange.Service.Interfaces;

namespace Exchange.Service.Services;

public class ExchangeService(IExchangeRateService exchangeRateService) : IExchangeService
{
    public async Task<decimal> GetExchangeAmount(string fromIsoCode, string toIsoCode, decimal amount)
    {
        if (amount <= 0)
        {
            throw new BusinessException("Amount should be greater than 0!");
        }

        var exchangeRate = await exchangeRateService.GetExchangeRate(fromIsoCode, toIsoCode);

        return Math.Round(amount * exchangeRate, 4);
    }
}