using Exchange.Service.Exceptions;
using Exchange.Service.Interfaces;
using Exchange.Service.Services;
using Moq;

namespace Exchange.Tests;

public class ExchangeServiceTests
{
    [Fact]
    public async Task ShouldReturnCalculatedResult()
    {
        var exchangeService = GetMockedService();

        var exchangeResult = await exchangeService.GetExchangeAmount("EUR", "USD", 1);

        Assert.Equal(1.5m, exchangeResult);
    }

    [Fact]
    public async Task ShouldThrowInvalidAmountException()
    {
        var exchangeService = GetMockedService();

        var exception = await Record.ExceptionAsync(() => exchangeService.GetExchangeAmount("EUR", "USD", 0));

        Assert.IsType<BusinessException>(exception);
        Assert.Equal("Amount should be greater than 0!", exception.Message);
    }

    private static IExchangeService GetMockedService()
    {
        var exchangeRateServiceMock = new Mock<IExchangeRateService>();

        exchangeRateServiceMock
            .Setup(service => service.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(1.5m);

        return new ExchangeService(exchangeRateServiceMock.Object);
    }
}