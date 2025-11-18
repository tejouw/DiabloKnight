# Türk Hayatı - Hayat Simülasyonu Oyunu

BitLife tarzında, tamamen Türkçe ve Türk kültürüne uyarlanmış metin tabanlı hayat simülasyonu oyunu.

## Oyun Hakkında

Oyuncu, doğumdan ölüme kadar hayatın her aşamasında kararlar vererek karakterinin kaderini şekillendirir. Eğitim, iş, ilişkiler, sağlık, para yönetimi ve daha fazlası...

### Özellikler

- **Türk Kültürü**: Gerçek Türk isimleri, şehirler, meslekler ve sosyal dinamikler
- **Stat Sistemi**: Sağlık, mutluluk, zeka, görünüş ve şöhret
- **Karar Ağacı**: Her kararın farklı sonuçları olan detaylı olay sistemi
- **Kariyer**: 20+ farklı meslek, YKS sınavı, üniversite seçimi
- **İlişkiler**: Aile, arkadaşlık, romantik ilişkiler
- **Yaş Dönemleri**: Bebeklik, çocukluk, ergenlik, yetişkinlik, yaşlılık
- **Kaydetme/Yükleme**: Birden fazla kayıt slotu

## Teknik Detaylar

### Platform
- Unity 2021.3+
- Android ve iOS desteği

### Mimari
- MVC + Event-Driven Architecture
- Singleton pattern ile merkezi managerlar
- JSON tabanlı içerik sistemi
- Tamamen kod ile oluşturulan UI

### Klasör Yapısı

```
Assets/
├── Scripts/
│   ├── Core/           # Temel sistemler (Singleton, EventBus)
│   ├── Character/      # Karakter ve stat sistemi
│   ├── Events/         # Olay sistemi
│   ├── UI/             # UI kontrolcüleri
│   ├── Managers/       # GameManager, UIManager vb.
│   └── Data/           # Veri modelleri
├── Resources/
│   ├── Events/         # JSON olay dosyaları
│   ├── Names/          # Türk isimleri
│   └── Localization/   # Dil dosyaları
```

## Kurulum

1. Unity Hub'dan Unity 2021.3 veya üstü yükleyin
2. Projeyi klonlayın:
   ```bash
   git clone <repo-url>
   ```
3. Unity ile projeyi açın
4. Herhangi bir sahneyi açın ve Play'e basın

## Nasıl Oynanır

1. **Yeni Oyun**: Rastgele bir karakter ile başlayın
2. **Yaşla**: Her yıl ilerlemek için "Yaşla" butonuna basın
3. **Kararlar**: Karşınıza çıkan olaylarda seçim yapın
4. **Statlar**: Sağlık, mutluluk ve diğer statları takip edin
5. **İlişkiler**: Aile ve arkadaşlarınızla ilişkilerinizi yönetin

## Geliştirme

### Yeni Olay Ekleme

`Assets/Resources/Events/` klasörüne JSON dosyası ekleyin:

```json
{
    "events": [
        {
            "id": "unique_event_id",
            "title": "Olay Başlığı",
            "description": "Olay açıklaması...",
            "ageRange": {"min": 18, "max": 30},
            "category": 4,
            "probability": 0.5,
            "choices": [
                {
                    "text": "Seçenek metni",
                    "outcomes": [
                        {
                            "type": 1,
                            "targetStat": "Happiness",
                            "minValue": 5,
                            "maxValue": 10,
                            "probability": 1.0,
                            "resultText": "Sonuç metni"
                        }
                    ]
                }
            ]
        }
    ]
}
```

### Yeni Meslek Ekleme

`Assets/Scripts/Managers/DataManager.cs` dosyasında `GetDefaultJobData()` methoduna ekleyin.

## Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Branch'i push edin (`git push origin feature/YeniOzellik`)
5. Pull Request açın

## Lisans

Tüm hakları saklıdır. Ticari kullanım için izin gereklidir.

## Ekran Görüntüleri

*Oyun geliştirme aşamasındadır.*

## İletişim

Sorularınız için issue açabilirsiniz.

---

**Versiyon**: 1.0.0
**Son Güncelleme**: 2025
