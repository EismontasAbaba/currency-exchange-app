namespace Exchange.Service.Interfaces;

public interface IExchangeService
{
    Task<decimal> GetExchangeAmount(string fromIsoCode, string toIsoCode, decimal amount);
}