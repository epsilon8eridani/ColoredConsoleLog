using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using ColoredConsoleLog;
using DevExpress.Mvvm;

namespace TestApp;

public class MainWindowViewModel : ViewModelBase
{
    public LogSettings LogSettings { get; set; } = new();
    public ObservableCollection<string> LogText { get; set; } = new();

    public MainWindowViewModel()
    {
        LogSettings.RegexColor.Add(
            new Regex("(==.*?==)"), 
            new SolidColorBrush("#A78295".ColorFromHex()));
    }
    public ICommand TestCommand
    {
        get
        {
            return new AsyncCommand(() =>
            {
                const string testText = "test text";
                for (var i = 1; i <= 50; i++)
                {
                    LogText.Add($"==custom {testText}==");
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

                return Task.CompletedTask;
            });
        }
    }
}