using YMH219_SpeechApp.Forms;

namespace YMH219_SpeechApp;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
