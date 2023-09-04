using System.Windows.Media;

namespace ColoredConsoleLog;

public static class Utils
{
    public static Color ColorFromHex(this string hex)
    {
        var color = (Color) ColorConverter.ConvertFromString(hex);
        return color;
    }
}