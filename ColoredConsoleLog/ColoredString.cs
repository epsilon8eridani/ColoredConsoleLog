using System.Windows.Media;

namespace ColoredConsoleLog;

public class ColoredString
{
    public string Text { get; }
    public SolidColorBrush Brush { get; }

    public ColoredString(string text, SolidColorBrush brush)
    {
        Text = text;
        Brush = brush;
    }
}