using System.Speech.Synthesis;

namespace YMH219_SpeechApp.Services;

public class TextToSpeechService : IDisposable
{
    private readonly SpeechSynthesizer _synthesizer = new();

    public event Action? OnSpeakCompleted;

    public TextToSpeechService()
    {
        _synthesizer.SetOutputToDefaultAudioDevice();
        _synthesizer.SpeakCompleted += (_, _) => OnSpeakCompleted?.Invoke();
    }

    public List<string> GetInstalledVoiceNames()
    {
        return _synthesizer.GetInstalledVoices()
            .Where(v => v.Enabled)
            .Select(v => v.VoiceInfo.Name)
            .ToList();
    }

    public void SelectVoice(string voiceName)
    {
        if (!string.IsNullOrWhiteSpace(voiceName))
            _synthesizer.SelectVoice(voiceName);
    }

    public void SetRate(int rate)
    {
        _synthesizer.Rate = Math.Clamp(rate, -10, 10);
    }

    public void SpeakAsync(string text)
    {
        _synthesizer.SpeakAsyncCancelAll();
        _synthesizer.SpeakAsync(text);
    }

    public void Pause()
    {
        if (_synthesizer.State == SynthesizerState.Speaking)
            _synthesizer.Pause();
    }

    public void Resume()
    {
        if (_synthesizer.State == SynthesizerState.Paused)
            _synthesizer.Resume();
    }

    public void Stop()
    {
        _synthesizer.SpeakAsyncCancelAll();
    }

    public SynthesizerState State => _synthesizer.State;

    public void Dispose()
    {
        _synthesizer.SpeakAsyncCancelAll();
        _synthesizer.Dispose();
    }
}
