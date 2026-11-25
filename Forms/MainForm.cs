using System.Speech.Synthesis;
using YMH219_SpeechApp.Services;

namespace YMH219_SpeechApp.Forms;

public class MainForm : Form
{
    private readonly TextToSpeechService _tts = new();
    private readonly SpeechToTextService _stt = new();

    private readonly Label _lblStatus = new() { Text = "Durum: Hazır", Dock = DockStyle.Top, Height = 36, TextAlign = ContentAlignment.MiddleLeft, BackColor = Color.FromArgb(30, 41, 59), ForeColor = Color.White, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Padding = new Padding(12, 0, 0, 0) };

    // TTS Controls
    private readonly RichTextBox _rtbTts = new() { Location = new Point(14, 24), Size = new Size(640, 110), Font = new Font("Segoe UI", 10F), Text = "Merhaba, bu Nesne Tabanlı Programlama dersi için geliştirilmiş sesli okuma uygulamasıdır." };
    private readonly ComboBox _cmbVoices = new() { Location = new Point(62, 144), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TrackBar _trkRate = new() { Location = new Point(358, 140), Width = 140, Minimum = -5, Maximum = 5, Value = 0, TickFrequency = 1 };
    private readonly Button _btnSpeak = new() { Text = "Başlat (Oku)", Location = new Point(14, 185), Size = new Size(115, 32), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
    private readonly Button _btnPause = new() { Text = "Duraklat", Location = new Point(138, 185), Size = new Size(105, 32), BackColor = Color.FromArgb(245, 158, 11), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
    private readonly Button _btnStopTts = new() { Text = "Durdur", Location = new Point(252, 185), Size = new Size(95, 32), BackColor = Color.FromArgb(239, 68, 68), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
    private readonly Button _btnClearTts = new() { Text = "Temizle", Location = new Point(356, 185), Size = new Size(95, 32), BackColor = Color.FromArgb(100, 116, 139), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

    // STT Controls
    private readonly RichTextBox _rtbStt = new() { Location = new Point(14, 24), Size = new Size(640, 125), Font = new Font("Segoe UI", 10F), ReadOnly = false };
    private readonly Button _btnStartStt = new() { Text = "Dinlemeyi Başlat", Location = new Point(14, 160), Size = new Size(140, 32), BackColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
    private readonly Button _btnStopStt = new() { Text = "Dinlemeyi Durdur", Location = new Point(164, 160), Size = new Size(140, 32), BackColor = Color.FromArgb(239, 68, 68), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Enabled = false };
    private readonly Button _btnClearStt = new() { Text = "Temizle", Location = new Point(314, 160), Size = new Size(95, 32), BackColor = Color.FromArgb(100, 116, 139), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
    private readonly Button _btnSaveStt = new() { Text = ".txt Kaydet", Location = new Point(418, 160), Size = new Size(110, 32), BackColor = Color.FromArgb(15, 118, 110), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

    public MainForm()
    {
        Text = "YMH 219 - Speech-to-Text & Text-to-Speech";
        ClientSize = new Size(696, 515);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var grpTts = new GroupBox { Text = "Text-to-Speech (Metinden Sese)", Location = new Point(14, 48), Size = new Size(668, 230) };
        grpTts.Controls.AddRange(new Control[]
        {
            _rtbTts,
            new Label { Text = "Ses:", Location = new Point(14, 147), AutoSize = true },
            _cmbVoices,
            new Label { Text = "Hız:", Location = new Point(318, 147), AutoSize = true },
            _trkRate,
            _btnSpeak, _btnPause, _btnStopTts, _btnClearTts
        });

        var grpStt = new GroupBox { Text = "Speech-to-Text (Sesten Metne)", Location = new Point(14, 290), Size = new Size(668, 208) };
        grpStt.Controls.AddRange(new Control[]
        {
            _rtbStt, _btnStartStt, _btnStopStt, _btnClearStt, _btnSaveStt
        });

        Controls.AddRange(new Control[] { _lblStatus, grpTts, grpStt });

        LoadVoices();
        BindEvents();
    }

    private void LoadVoices()
    {
        try
        {
            var voices = _tts.GetInstalledVoiceNames();
            foreach (var v in voices) _cmbVoices.Items.Add(v);
            if (_cmbVoices.Items.Count > 0) _cmbVoices.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            SetStatus($"Sesler yüklenemedi: {ex.Message}");
        }
    }

    private void BindEvents()
    {
        _btnSpeak.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(_rtbTts.Text))
            {
                MessageBox.Show("Lütfen seslendirilecek bir metin girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (_cmbVoices.SelectedItem is string voice) _tts.SelectVoice(voice);
                _tts.SetRate(_trkRate.Value);
                _tts.SpeakAsync(_rtbTts.Text);
                _btnPause.Text = "Duraklat";
                SetStatus("Durum: Metin sesli okunuyor (Text-to-Speech aktif)...");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seslendirme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        _btnPause.Click += (_, _) =>
        {
            if (_tts.State == SynthesizerState.Speaking)
            {
                _tts.Pause();
                _btnPause.Text = "Devam Et";
                SetStatus("Durum: Sesli okuma duraklatıldı.");
            }
            else if (_tts.State == SynthesizerState.Paused)
            {
                _tts.Resume();
                _btnPause.Text = "Duraklat";
                SetStatus("Durum: Sesli okuma devam ediyor...");
            }
        };

        _btnStopTts.Click += (_, _) =>
        {
            _tts.Stop();
            _btnPause.Text = "Duraklat";
            SetStatus("Durum: Sesli okuma durduruldu.");
        };

        _btnClearTts.Click += (_, _) => _rtbTts.Clear();

        _tts.OnSpeakCompleted += () => SafeInvoke(() =>
        {
            _btnPause.Text = "Duraklat";
            SetStatus("Durum: Sesli okuma tamamlandı.");
        });

        _btnStartStt.Click += (_, _) =>
        {
            try
            {
                _stt.StartListening();
                _btnStartStt.Enabled = false;
                _btnStopStt.Enabled = true;
                SetStatus("Durum: Mikrofon dinleniyor (Speech-to-Text aktif)...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mikrofon veya konuşma tanıma servisi başlatılamadı:" + Environment.NewLine + ex.Message, "Konuşma Tanıma Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Durum: Konuşma tanıma başlatılamadı.");
            }
        };

        _btnStopStt.Click += (_, _) =>
        {
            _stt.StopListening();
            _btnStartStt.Enabled = true;
            _btnStopStt.Enabled = false;
            SetStatus("Durum: Mikrofon dinleme durduruldu.");
        };

        _btnClearStt.Click += (_, _) => _rtbStt.Clear();

        _btnSaveStt.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(_rtbStt.Text)) return;
            using var dlg = new SaveFileDialog { Filter = "Metin Dosyası (*.txt)|*.txt", FileName = "konusma_metni.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dlg.FileName, _rtbStt.Text);
                MessageBox.Show("Metin kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };

        _stt.OnSpeechRecognized += (text, conf) => SafeInvoke(() =>
        {
            _rtbStt.AppendText($"[{DateTime.Now:HH:mm:ss}] ({conf:P0}) {text}{Environment.NewLine}");
        });

        _stt.OnError += err => SafeInvoke(() =>
        {
            MessageBox.Show($"Konuşma tanıma hatası: {err}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _btnStartStt.Enabled = true;
            _btnStopStt.Enabled = false;
            SetStatus("Durum: Hata oluştu.");
        });

        FormClosing += (_, _) =>
        {
            _tts.Dispose();
            _stt.Dispose();
        };
    }

    private void SetStatus(string text) => _lblStatus.Text = text;

    private void SafeInvoke(Action action)
    {
        if (IsDisposed) return;
        if (InvokeRequired) Invoke(action);
        else action();
    }
}
