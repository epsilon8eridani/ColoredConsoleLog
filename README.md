# ColoredConsoleLog

Make the log output in your WPF application beautiful!

This is a small UserControl for WPF to output logs in different colors, depending on preferences, configurable with regular expressions.

By default, changes the color of text in `''`, `""`, `Windows File Path`, `URL`, `[]`, or if the string contains the text `Error` or `Warning`

[![NuGet stable version](https://badgen.net/nuget/v/ColoredConsoleLog)](https://www.nuget.org/packages/ColoredConsoleLog/)

# How to use

## XAML

```XML
<ColoredConsoleLog LogText="{Binding LogText}" />
```

## Code

```C#
public ObservableCollection<string> LogText { get; set; } = new();
```

## Add log

```C#
const string testText = "test text";
for (var i = 1; i <= 50; i++)
{
    LogText.Add($"default {testText} N{i}");
    LogText.Add($"'{testText}'");
    LogText.Add($@"""{testText}""");
    LogText.Add($@"C:\Windows\{testText}.txt");
    LogText.Add("https://google.com");
    LogText.Add($"[{testText}]");
    LogText.Add($"warning {testText}");
    LogText.Add($"error {testText}");
    LogText.Add("");
}
```

# Add custom text pattern

## Create settings instance

```C#
 public LogSettings LogSettings { get; set; } = new();
```

## Add new regex and color

```C#
LogSettings.RegexColor.Add(
            new Regex("(==.*?==)"),
            new SolidColorBrush("#A78295".ColorFromHex()));
```

## And add settings instance to XAML

```XML
<ColoredConsoleLog
            LogText="{Binding LogText}"
            Settings="{Binding LogSettings}" />
```

# Watch example in TestApp

![TestLog](https://github.com/epsilon8eridani/ColoredConsoleLog/raw/main/Images/TestLog.png)
