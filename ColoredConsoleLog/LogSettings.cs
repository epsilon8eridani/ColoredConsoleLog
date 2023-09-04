using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace ColoredConsoleLog;

public class LogSettings
{
    public Dictionary<Regex, SolidColorBrush> RegexColor { get; set; }
    public RegexOptions RegexOptions { get; set; } = RegexOptions.Compiled | RegexOptions.IgnoreCase;

    public SolidColorBrush DefaultBrush { get; set; } = new("#73BBC9".ColorFromHex());
    public SolidColorBrush OneQuotesBrush { get; set; } = new("#F07DEA".ColorFromHex());
    public SolidColorBrush TwoQuotesBrush { get; set; } = new("#B1AFFF".ColorFromHex());
    public SolidColorBrush BracketsBrush { get; set; } = new("#F7C8E0".ColorFromHex());
    public SolidColorBrush FilesBrush { get; set; } = new("#B3E5BE".ColorFromHex());
    public SolidColorBrush UrlsBrush { get; set; } = new("#BA94D1".ColorFromHex());
    public SolidColorBrush WarningBrush { get; set; } = new("#FFD966".ColorFromHex());
    public SolidColorBrush ErrorBrush { get; set; } = new("#C02739".ColorFromHex());
    public int MaxLogLines { get; set; } = 1000;

    public LogSettings()
    {
        RegexColor = new Dictionary<Regex, SolidColorBrush>
        {
            // 'test text'
            {
                new Regex(@"(\'.*?\')", RegexOptions), OneQuotesBrush
            },
            // "test text"
            {
                new Regex("""(".*?")""", RegexOptions), TwoQuotesBrush
            },
            // C:\Windows\test.txt
            {
                new Regex(@"[a-zA-Z]:\\[^/:\*\?<>|]+\.\w+", RegexOptions), FilesBrush
            },
            // https://google.com
            {
                new Regex(@"https:\/\/[a-zA-Z0-9\-.]+\.[a-zA-Z]{2,}(\:[0-9]{1,5})?(\/\S*)?", RegexOptions), UrlsBrush
            },
            // [test text]
            {
                new Regex(@"\[(.*?)\]", RegexOptions), BracketsBrush
            },
            // warning test text
            {
                new Regex(".*warning.*", RegexOptions), WarningBrush
            },
            // error test text
            {
                new Regex(".*error.*", RegexOptions), ErrorBrush
            }
        };
    }
}