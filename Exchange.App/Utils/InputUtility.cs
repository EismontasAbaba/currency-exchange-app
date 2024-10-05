using System.Text.RegularExpressions;

namespace Exchange.App.Utils;

public static class InputUtility
{
    public static bool ValidateInput(string input, out string fromIsoCode, out string toIsoCode, out decimal amount)
    {
        fromIsoCode = string.Empty;
        toIsoCode = string.Empty;
        amount = 0;

        const string pattern = @"^(?<fromIsoCode>[^/]+)/(?<toIsoCode>[^ ]+) (?<amount>-?\d+([,.]\d+)?)$";

        var match = Regex.Match(input, pattern);

        if (!match.Success)
        {
            return false;
        }

        fromIsoCode = match.Groups["fromIsoCode"].Value;
        toIsoCode = match.Groups["toIsoCode"].Value;
        amount = decimal.Parse(match.Groups["amount"].Value);

        return true;
    }
}