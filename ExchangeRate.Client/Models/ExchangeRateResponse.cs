namespace ExchangeRate.Client.Models;

public class ExchangeRateResponse
{
    public bool Success { get; set; }

    public string Base { get; set; }

    public IDictionary<string, decimal> Rates { get; set; }
}