using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ColoredConsoleLog;
using DevExpress.Mvvm;

namespace TestApp;

public class MainWindowViewModel : ViewModelBase
{
    public LogSettings LogSettings { get; set; } = new();
    public ObservableCollection<string> LogText { get; set; } = new();
    public ICommand TestCommand
    {
        get
        {
            return new AsyncCommand(() =>
            {
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

                return Task.CompletedTask;
            });
        }
    }
}