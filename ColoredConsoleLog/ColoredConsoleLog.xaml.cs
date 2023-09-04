using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ColoredConsoleLog;

public partial class ColoredConsoleLog : UserControl
{
    public ColoredConsoleLog()
    {
        InitializeComponent();
    }

    public ObservableCollection<string> LogText
    {
        get => (ObservableCollection<string>) GetValue(LogTextProperty);
        set => SetValue(LogTextProperty, value);
    }

    public LogSettings Settings
    {
        get => (LogSettings) GetValue(SettingsProperty);
        set => SetValue(SettingsProperty, value);
    }

    public static readonly DependencyProperty LogTextProperty =
        DependencyProperty.Register("LogText", typeof(ObservableCollection<string>),
            typeof(ColoredConsoleLog),
            new FrameworkPropertyMetadata(new ObservableCollection<string>(),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

    public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(nameof(Settings),
        typeof(LogSettings), typeof(ColoredConsoleLog), new PropertyMetadata(new LogSettings()));

    private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not ColoredConsoleLog logger) return;

        if (e.OldValue is INotifyCollectionChanged oldValue)
        {
            oldValue.CollectionChanged -= logger.ColorTextsChanged;
        }

        if (e.NewValue is INotifyCollectionChanged newValue)
        {
            newValue.CollectionChanged += logger.ColorTextsChanged;
        }
    }

    public void ColorTextsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (Dispatcher.CheckAccess())
        {
            HandleText(e);
        }
        else
        {
            Dispatcher.Invoke(() => { HandleText(e); });
        }
    }

    public void HandleText(NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Add) return;
        if (e.NewItems == null) return;

        foreach (var item in e.NewItems)
        {
            if (item is not string text) continue;

            AddColoredTextToConsole(text);

            LogText.Remove(text);
        }

        while (Console.Document.Blocks.Count > Settings.MaxLogLines)
        {
            var fb = Console.Document.Blocks.FirstBlock;
            if (fb != null)
            {
                Console.Document.Blocks.Remove(fb);
            }
        }

        Console.ScrollToEnd();
    }

    public void AddColoredTextToConsole(string text)
    {
        var coloredStrings = ColorText(text);

        var paragraph = new Paragraph();
        foreach (var inline in coloredStrings)
        {
            paragraph.Inlines.Add(new Run(inline.Text) {Foreground = inline.Brush});
        }

        Console.Document.Blocks.Add(paragraph);
    }

    public List<ColoredString> ColorText(string text)
    {
        var coloredStrings = new List<ColoredString>();
        var currentIndex = 0;

        while (currentIndex < text.Length)
        {
            var matches = Settings.RegexColor
                .SelectMany(pair => pair.Key.Matches(text)
                    .Select(m => new {Match = m, Color = pair.Value}))
                .Where(m => m.Match.Index >= currentIndex)
                .OrderBy(m => m.Match.Index).ToList();

            if (matches.Any())
            {
                var firstMatch = matches.First();

                if (firstMatch.Match.Index > currentIndex)
                {
                    var precedingText = text.Substring(currentIndex, firstMatch.Match.Index - currentIndex);
                    coloredStrings.Add(new ColoredString(precedingText, Settings.DefaultBrush));
                }

                var specialColorText = firstMatch.Match.Value;
                coloredStrings.Add(new ColoredString(specialColorText, firstMatch.Color));
                currentIndex = firstMatch.Match.Index + firstMatch.Match.Length;
            }
            else
            {
                var remainingText = text[currentIndex..];
                coloredStrings.Add(new ColoredString(remainingText, Settings.DefaultBrush));
                break;
            }
        }

        return coloredStrings;
    }
}