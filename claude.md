# Türk Hayat Simülasyonu - BitLife Tarzı Oyun

## Proje Özeti
BitLife tarzında, tamamen Türkçe ve Türk kültürüne uyarlanmış metin tabanlı hayat simülasyonu oyunu. Oyuncu doğumdan ölüme kadar hayatın her aşamasında kararlar vererek karakterinin kaderini şekillendirir.

## Teknik Stack
- **Platform**: Unity (C#) - Android ve iOS için cross-platform
- **Mimari**: MVC + Event-Driven Architecture
- **UI**: Tamamen kod ile oluşturulan UI (manuel atama yok)
- **Veri**: JSON tabanlı event ve içerik sistemi
- **Kaydetme**: PlayerPrefs + JSON serialization

## Temel Özellikler

### Stat Sistemi
- **Sağlık** (0-100): Hastalıklar, spor, beslenme
- **Mutluluk** (0-100): Sosyal aktiviteler, başarılar, ilişkiler
- **Zeka** (0-100): Eğitim, okuma, deneyimler
- **Görünüş** (0-100): Genetik, bakım, sağlık
- **Para**: TL cinsinden finansal durum
- **Şöhret** (0-100): Sosyal medya, kariyer, olaylar

### Yaş Dönemleri
1. **Bebeklik** (0-4): Aile olayları, temel gelişim
2. **Çocukluk** (5-11): İlkokul, arkadaşlıklar, hobiler
3. **Ergenlik** (12-17): Ortaokul/Lise, ilk aşk, isyan dönemi
4. **Genç Yetişkin** (18-29): Üniversite, kariyer başlangıcı, evlilik
5. **Yetişkinlik** (30-59): Kariyer zirvesi, aile, orta yaş krizi
6. **Yaşlılık** (60+): Emeklilik, torunlar, sağlık sorunları

### Türkiye'ye Özgü İçerikler

#### Meslekler
- Devlet memuru, öğretmen, doktor, avukat
- Esnaf, taksici, emlakçı, inşaat işçisi
- Youtuber, influencer, e-ticaret
- Çiftçi, balıkçı, turizmci

#### Eğitim Sistemi
- İlkokul, ortaokul, lise (Anadolu/Fen/Meslek)
- YKS sınavı, üniversite tercihi
- Yüksek lisans, doktora
- Sertifika programları

#### Sosyal Dinamikler
- Askerlik (erkekler için zorunlu)
- Bayram/düğün/cenaze gelenekleri
- Komşuluk ilişkileri
- Kahvehane kültürü
- Yurt dışına göç seçeneği

#### Rastgele Olaylar
- Deprem, sel, yangın
- Ekonomik kriz, döviz dalgalanması
- Piyango, şans oyunları
- Trafik kazası, hırsızlık
- Viral sosyal medya anı

## Kod Standartları

### Naming Conventions
- **Classlar**: PascalCase (örn: `CharacterStats`, `EventManager`)
- **Methodlar**: PascalCase (örn: `CalculateHappiness()`)
- **Değişkenler**: camelCase (örn: `currentHealth`, `playerMoney`)
- **Constantlar**: SCREAMING_SNAKE_CASE (örn: `MAX_HEALTH`)
- **Private fieldlar**: _camelCase (örn: `_eventQueue`)

### Dosya Yapısı
```
Assets/
├── Scripts/
│   ├── Core/           # Temel sistemler
│   ├── Character/      # Karakter ve stat sistemi
│   ├── Events/         # Olay sistemi
│   ├── UI/             # UI kodları
│   ├── Data/           # Data modelleri
│   ├── Managers/       # Singleton managerlar
│   └── Utils/          # Yardımcı fonksiyonlar
├── Resources/
│   ├── Events/         # JSON event dosyaları
│   ├── Jobs/           # Meslek verileri
│   └── Localization/   # Türkçe metinler
└── Prefabs/            # UI prefabları (kod ile oluşturulacak)
```

### UI Kuralları
- Tüm UI elemanları kod ile instantiate edilecek
- Canvas ve UI hiyerarşisi runtime'da oluşturulacak
- Stil sabitleri merkezi bir class'ta tanımlanacak
- Responsive tasarım için anchor ve pivot hesaplamaları kod ile yapılacak

### Event Sistemi
- Observer pattern kullanılacak
- Loosely coupled tasarım
- ScriptableObject tabanlı event tanımları
- JSON'dan dinamik event yükleme

### Genişletilebilirlik
- Yeni olaylar JSON dosyası ekleyerek
- Yeni meslekler data dosyası ile
- Modüler sistem tasarımı
- Interface tabanlı bileşenler

## Geliştirme Fazları

### Faz 1: Proje Kurulumu ve Temel Mimari
- Unity projesi oluşturma
- Klasör yapısı
- Temel managerlar (GameManager, UIManager, EventManager)
- Singleton pattern implementasyonu

### Faz 2: Karakter Sistemi ve Statlar
- CharacterData class
- Stat sistemi (sağlık, mutluluk, zeka, para vb.)
- Stat değişim mekanikleri
- Karakter oluşturma (isim, cinsiyet, aile)

### Faz 3: Olay Sistemi ve Karar Ağaçları
- Event base class
- Decision tree yapısı
- JSON'dan event yükleme
- Sonuç hesaplama sistemi

### Faz 4: Yaş Dönemleri ve Yaşlanma
- Age progression sistemi
- Dönem bazlı event filtreleme
- Ölüm mekanikleri
- Yaşa bağlı stat değişimleri

### Faz 5: Kariyer ve Eğitim Sistemleri
- Okul sistemi (ilkokul → üniversite)
- Sınav mekanikleri (YKS)
- Meslek ağacı
- Maaş ve terfi sistemi

### Faz 6: İlişkiler ve Aile Sistemi
- NPC relationship sistemi
- Evlilik, boşanma
- Çocuk sahibi olma
- Arkadaşlık ve düşmanlık

### Faz 7: Kaydetme/Yükleme Sistemi
- Save data yapısı
- JSON serialization
- Auto-save
- Multiple save slots

### Faz 8: UI Implementasyonu
- Ana menü
- Oyun ekranı (statlar, olaylar, seçenekler)
- Karakter profili
- Ayarlar menüsü
- Tüm UI kod ile oluşturulacak

### Faz 9: Test ve Polish
- Balans ayarları
- Bug fixing
- Performans optimizasyonu
- Türkçe metin kontrolü

## Önemli Notlar

### Performans
- Object pooling kullanılacak
- Lazy loading tercih edilecek
- Gereksiz Update() çağrılarından kaçınılacak

### Mobil Optimizasyon
- Düşük bellek kullanımı
- Batarya dostu tasarım
- Touch-friendly UI boyutları

### Kod Kalitesi
- Her public method için XML documentation
- Unit testler için hazırlık
- SOLID prensipleri
- Clean code pratikleri

## Komutlar ve Araçlar

### Build
```bash
# Android build
Unity -executeMethod BuildScript.BuildAndroid

# iOS build
Unity -executeMethod BuildScript.BuildIOS
```

### Test
```bash
# Unit testleri çalıştır
Unity -runTests -testPlatform EditMode
```

## İletişim Dili
- Tüm kod içi yorumlar Türkçe
- Değişken ve class isimleri İngilizce
- UI metinleri Türkçe
- Hata mesajları Türkçe

## Lisans
Tüm hakları saklıdır. Ticari kullanım için izin gereklidir.
