using Exchange.Service.Extensions;
using Exchange.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Exchange.App.Utils;

try
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.RegisterAppServices(builder.Configuration);

    using var host = builder.Build();

    Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange> | type 'exit' to exit application");

    while (true)
    {
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Input not found!");

            continue;
        }

        if (input.ToLower() == "exit")
        {
            break;
        }

        if (!InputUtility.ValidateInput(input, out var fromIsoCode, out var toIsoCode, out var amount))
        {
            Console.WriteLine("Invalid input!");

            continue;
        }

        try
        {
            using var serviceScope = host.Services.CreateScope();

            var exchangeService = serviceScope.ServiceProvider.GetRequiredService<IExchangeService>();

            var exchangeAmount = await exchangeService.GetExchangeAmount(fromIsoCode, toIsoCode, amount);

            Console.WriteLine(exchangeAmount);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
catch (Exception)
{
    Console.WriteLine("App terminated unexpectedly");
    Environment.ExitCode = 1;
}