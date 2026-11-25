using System.Speech.Recognition;

namespace YMH219_SpeechApp.Services;

public class SpeechToTextService : IDisposable
{
    private SpeechRecognitionEngine? _recognizer;
    public bool IsListening { get; private set; }

    public event Action<string, float>? OnSpeechRecognized;
    public event Action<string>? OnError;

    public void StartListening()
    {
        if (IsListening) return;

        var recognizers = SpeechRecognitionEngine.InstalledRecognizers();
        if (recognizers.Count == 0)
        {
            throw new InvalidOperationException(
                "Bu bilgisayarda yüklü bir Windows Konuşma Tanıma (Speech Recognition) dil paketi bulunamadı. " +
                "Windows Ayarlar > Zaman ve Dil > Konuşma bölümünden bir dil paketi yükleyin.");
        }

        _recognizer?.Dispose();
        _recognizer = new SpeechRecognitionEngine(recognizers[0].Culture);
        _recognizer.LoadGrammar(new DictationGrammar());
        _recognizer.SetInputToDefaultAudioDevice();

        _recognizer.SpeechRecognized += (_, e) =>
        {
            if (e.Result != null && !string.IsNullOrWhiteSpace(e.Result.Text))
            {
                OnSpeechRecognized?.Invoke(e.Result.Text, e.Result.Confidence);
            }
        };

        _recognizer.RecognizeCompleted += (_, e) =>
        {
            if (e.Error != null)
            {
                OnError?.Invoke(e.Error.Message);
            }
            IsListening = false;
        };

        _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        IsListening = true;
    }

    public void StopListening()
    {
        if (!IsListening || _recognizer == null) return;
        _recognizer.RecognizeAsyncStop();
        IsListening = false;
    }

    public void Dispose()
    {
        StopListening();
        _recognizer?.Dispose();
    }
}
