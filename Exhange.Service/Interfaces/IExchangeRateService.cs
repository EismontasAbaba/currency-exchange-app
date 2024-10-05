namespace Exchange.Service.Interfaces;

public interface IExchangeRateService
{
    Task<decimal> GetExchangeRate(string fromIsoCode, string toIsoCode);
}