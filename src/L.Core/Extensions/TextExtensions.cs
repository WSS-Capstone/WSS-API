namespace L.Core.Extensions;

public static class TextExtensions
{
    public static string? CenterText(this string text, int totalWidth)
    {
        if (text.Length >= totalWidth)
        {
            return text; // No need to center if the text is longer than the total width
        }

        int padding = (totalWidth - text.Length) / 2; // Calculate the padding required on each side

        return text.PadLeft(padding + text.Length).PadRight(totalWidth);
    }
}