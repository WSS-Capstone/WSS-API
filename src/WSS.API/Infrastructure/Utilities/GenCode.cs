using System.Text.RegularExpressions;

namespace WSS.API.Infrastructure.Utilities;

public class GenCode
{
    public static string NextId(string? originalId, string defaultPrefix = "P", int defaultLength = 7,
        int start = 1)
    {
        if (string.IsNullOrEmpty(originalId))
        {
            return defaultPrefix + start.ToString("D" + defaultLength);
        }

        // Use regular expression to separate prefix and numeric part
        Match match = Regex.Match(originalId, @"^(\D+)(\d+)$", RegexOptions.None, TimeSpan.FromMilliseconds(1000));

        if (match.Success)
        {
            var prefix = match.Groups[1].Value;
            var numericPart = match.Groups[2].Value;
            int numericValue = int.Parse(numericPart) + 1;
            var incrementedNumericPart = numericValue.ToString("D" + numericPart.Length);
            var newId = prefix + incrementedNumericPart;
            return newId;
        }

        // If the ID format is invalid, return the original ID
        return originalId;
    }
}