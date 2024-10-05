using Exchange.Service.Exceptions;
using Exchange.Service.Interfaces;
using Exchange.Service.Services;
using ExchangeRate.Client.Interface;
using ExchangeRate.Client.Models;
using Moq;

namespace Exchange.Tests;

public class ExchangeRateServiceTests
{
    [Fact]
    public async Task ShouldReturnExchangeRateEurToUsd()
    {
        var exchangeRateService = GetMockedService();

        var rate = await exchangeRateService.GetExchangeRate("EUR", "USD");

        Assert.Equal(1.25m, rate);
    }

    [Fact]
    public async Task ShouldReturnExchangeRateUsdToEur()
    {
        var exchangeRateService = GetMockedService();

        var rate = await exchangeRateService.GetExchangeRate("USD", "EUR");

        Assert.Equal(0.8m, rate);
    }

    [Fact]
    public async Task ShouldThrowNotFoundCurrencyException()
    {
        var exchangeRateService = GetMockedService();

        var exception = await Record.ExceptionAsync(() => exchangeRateService.GetExchangeRate("DKK", "USD"));

        Assert.IsType<BusinessException>(exception);
        Assert.Equal("Exchange rates not found or currency is not supported!", exception.Message);
    }

    [Fact]
    public async Task ShouldThrowRatesNotAvailableException()
    {
        var exchangeRateService = GetMockedService(false);

        var exception = await Record.ExceptionAsync(() => exchangeRateService.GetExchangeRate("EUR", "USD"));

        Assert.IsType<BusinessException>(exception);
        Assert.Equal("Exchange rates are not available at the moment!", exception.Message);
    }

    [Fact]
    public async Task ShouldThrowExchangeRateClientException()
    {
        var exchangeRateService = GetMockedService(false, true);

        var exception = await Record.ExceptionAsync(() => exchangeRateService.GetExchangeRate("EUR", "USD"));

        Assert.IsType<Exception>(exception);
        Assert.Equal("Exchange rate fetch error", exception.Message);
    }

    private static IExchangeRateService GetMockedService(bool isSuccessfulResponse = true, bool shouldThrowException = false)
    {
        var exchangeRateClient = new Mock<IExchangeRateClient>();
        var cacheService = new Mock<ICacheService>();

        if (shouldThrowException)
        {
            exchangeRateClient
                .Setup(client => client.GetExchangeRates())
                .ThrowsAsync(new Exception("Internal server error"));

            cacheService
                .Setup(service =>
                    service.GetOrCreateAsync(It.IsAny<string>(), exchangeRateClient.Object.GetExchangeRates, 60))
                .ThrowsAsync(new Exception("Internal server error"));
        }
        else
        {
            var response = new ExchangeRateResponse
            {
                Success = isSuccessfulResponse,
                Rates = new Dictionary<string, decimal>
                {
                    {"USD", 1.25m},
                    {"EUR", 1m}
                }
            };

            exchangeRateClient
                .Setup(client => client.GetExchangeRates())
                .ReturnsAsync(response);

            cacheService
                .Setup(service => service.GetOrCreateAsync(It.IsAny<string>(), exchangeRateClient.Object.GetExchangeRates, 60))
                .ReturnsAsync(response);
        }

        return new ExchangeRateService(exchangeRateClient.Object, cacheService.Object);
    }
}