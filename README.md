# YMH219 - Speech-to-Text ve Text-to-Speech Uygulaması

## Proje Açıklaması
Nesne Tabanlı Programlama (YMH 219) dersi kapsamında .NET `System.Speech` kütüphanesi kullanılarak geliştirilmiş, metni sesli okuyan (TTS) ve mikrofondan alınan konuşmayı metne çeviren (STT) Windows Forms uygulamasıdır.

## Kullanılan Teknoloji ve Paketler
- C# / .NET 8.0 (Windows Forms)
- NuGet Paketi: `System.Speech` (v8.0.0)

## Kurulum ve Çalıştırma Adımları
1. Depoyu klonlayın:
   ```bash
   git clone https://github.com/255541002AsimGokalp/YMH219-SpeechApp.git
   ```
2. `YMH219-SpeechApp.sln` dosyasını Visual Studio 2022 ile açın (NuGet paketleri otomatik geri yüklenir).
3. `F5` ile uygulamayı çalıştırın.

## Uygulamanın Temel Özellikleri
- **Text-to-Speech:** Metin kutusundaki metni okuma, Duraklat/Devam Et, Durdur, Temizle, sistem sesi seçimi ve konuşma hızı ayarı
- **Speech-to-Text:** Mikrofondan canlı konuşma tanıma, zaman damgası ve güven skoru ile metin alanına aktarma, `.txt` olarak kaydetme
- Üst bilgi çubuğunda aktif işlem durum göstergesi
- Mikrofon veya konuşma paketi erişim hatalarında açıklayıcı hata mesajları

## Ekran Görüntüleri
![Text-to-Speech Kullanımı](screenshots/screenshot1.png)
![Speech-to-Text Kullanımı](screenshots/screenshot2.png)

## Bilinen Sınırlamalar / Geliştirme Notları
- `System.Speech.Recognition`, Windows üzerinde kurulu konuşma tanıma dil paketlerini kullanır. Sistemde konuşma tanıma paketi kurulu değilse kullanıcıya açıklayıcı uyarı gösterilir.

## Öğrenci Bilgileri
- **Ad-Soyad:** Asım Gökalp
- **Öğrenci Numarası:** 255541002
