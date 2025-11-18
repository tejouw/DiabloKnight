# Türk Hayat Simülasyonu - Detaylı Geliştirme Planı

## Genel Bakış
Bu belge, projenin faz faz nasıl geliştirileceğini detaylı şekilde açıklar. Her faz tamamlanmadan bir sonraki faza geçilmeyecektir.

---

## FAZ 1: Proje Kurulumu ve Temel Mimari

### Amaç
Unity projesi için temel altyapıyı oluşturmak ve tüm sistemlerin üzerine inşa edileceği mimariyi kurmak.

### Görevler

#### 1.1 Proje Yapısı
- [x] Unity proje klasör yapısını oluştur
- [x] Assembly Definition dosyalarını ayarla
- [x] .gitignore dosyası

#### 1.2 Temel Managerlar
- [x] **GameManager**: Oyun durumu kontrolü, sahne yönetimi
- [x] **UIManager**: Tüm UI operasyonları için merkezi kontrol
- [x] **EventManager**: Event bus sistemi
- [x] **AudioManager**: Ses efektleri ve müzik (temel yapı)
- [x] **DataManager**: Veri yükleme ve kaydetme

#### 1.3 Singleton Pattern
```csharp
// Tüm managerlar için kullanılacak generic singleton
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
```

#### 1.4 Event Bus Sistemi
```csharp
// Observer pattern implementasyonu
public static class EventBus
{
    public static event Action<GameEvent> OnGameEvent;
    public static void Publish(GameEvent gameEvent);
    public static void Subscribe(Action<GameEvent> handler);
}
```

### Çıktılar
- Çalışan boş Unity projesi
- Tüm manager scriptleri
- Temel event sistemi

### Tamamlanma Kriterleri
- Proje hatasız derlenir
- Managerlar sahnede çalışır
- Event sistemi test edilir

---

## FAZ 2: Karakter Sistemi ve Statlar

### Amaç
Oyuncunun karakterini temsil eden veri yapısını ve stat sistemini oluşturmak.

### Görevler

#### 2.1 CharacterData Class
```csharp
[System.Serializable]
public class CharacterData
{
    public string firstName;
    public string lastName;
    public Gender gender;
    public int age;
    public DateTime birthDate;
    public string birthCity;
    public CharacterStats stats;
    public List<Relationship> relationships;
    public EducationData education;
    public CareerData career;
    public FinancialData finances;
}
```

#### 2.2 Stat Sistemi
```csharp
[System.Serializable]
public class CharacterStats
{
    [Range(0, 100)] public int health;
    [Range(0, 100)] public int happiness;
    [Range(0, 100)] public int intelligence;
    [Range(0, 100)] public int appearance;
    [Range(0, 100)] public int fame;

    public void ModifyStat(StatType type, int amount);
    public int GetStat(StatType type);
}
```

#### 2.3 Stat Değişim Mekanikleri
- Her yaş geçişinde doğal değişimler
- Olaylara bağlı değişimler
- Min/Max sınırları (0-100)
- Statlar arası etkileşimler (örn: düşük sağlık → düşük mutluluk)

#### 2.4 Karakter Oluşturma
- Rastgele isim havuzu (Türk isimleri)
- Cinsiyet seçimi
- Başlangıç statları (kısmen rastgele)
- Aile ataması (anne, baba, kardeşler)

#### 2.5 Türk İsim Veritabanı
```json
{
    "maleNames": ["Ahmet", "Mehmet", "Mustafa", "Ali", "Hüseyin", ...],
    "femaleNames": ["Fatma", "Ayşe", "Emine", "Hatice", "Zeynep", ...],
    "surnames": ["Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", ...]
}
```

### Çıktılar
- CharacterData ve ilgili tüm data classları
- Stat yönetim sistemi
- Türk isim veritabanı
- Karakter oluşturma logic'i

### Tamamlanma Kriterleri
- Karakter oluşturulabilir
- Statlar değiştirilebilir
- Veriler serialize edilebilir

---

## FAZ 3: Olay Sistemi ve Karar Ağaçları

### Amaç
Oyunun temelini oluşturan olay sistemi ve oyuncu seçimlerini yöneten karar ağacı yapısını kurmak.

### Görevler

#### 3.1 Event Base Class
```csharp
[System.Serializable]
public class GameEvent
{
    public string id;
    public string title;
    public string description;
    public AgeRange ageRange;
    public List<EventChoice> choices;
    public List<EventCondition> conditions;
    public EventCategory category;
    public float probability;
}
```

#### 3.2 Event Choice Yapısı
```csharp
[System.Serializable]
public class EventChoice
{
    public string text;
    public List<EventOutcome> outcomes;
    public List<ChoiceCondition> requirements;
}

[System.Serializable]
public class EventOutcome
{
    public OutcomeType type;
    public string targetStat;
    public int minValue;
    public int maxValue;
    public float probability;
    public string resultText;
}
```

#### 3.3 JSON Event Loader
```csharp
public class EventLoader
{
    public List<GameEvent> LoadEventsFromResources(string category);
    public GameEvent GetRandomEvent(int age, CharacterData character);
}
```

#### 3.4 Örnek Event JSON
```json
{
    "id": "school_bully_001",
    "title": "Okul Zorbalığı",
    "description": "Okulda bir çocuk sana laf attı ve arkadaşlarının önünde seni küçük düşürdü.",
    "ageRange": {"min": 7, "max": 14},
    "category": "school",
    "probability": 0.3,
    "choices": [
        {
            "text": "Görmezden gel ve uzaklaş",
            "outcomes": [
                {
                    "type": "stat_change",
                    "targetStat": "happiness",
                    "minValue": -5,
                    "maxValue": -10,
                    "probability": 1.0,
                    "resultText": "İçine attın ama moralin bozuldu."
                }
            ]
        },
        {
            "text": "Öğretmene şikayet et",
            "outcomes": [
                {
                    "type": "stat_change",
                    "targetStat": "happiness",
                    "minValue": 5,
                    "maxValue": 10,
                    "probability": 0.6,
                    "resultText": "Öğretmen duruma el attı ve zorba ceza aldı."
                },
                {
                    "type": "stat_change",
                    "targetStat": "happiness",
                    "minValue": -15,
                    "maxValue": -20,
                    "probability": 0.4,
                    "resultText": "Öğretmen pek ciddiye almadı, zorba daha da azdı."
                }
            ]
        },
        {
            "text": "Karşılık ver",
            "outcomes": [
                {
                    "type": "stat_change",
                    "targetStat": "happiness",
                    "minValue": 10,
                    "maxValue": 15,
                    "probability": 0.4,
                    "resultText": "Zorba geri adım attı, kendine güvenin arttı!"
                },
                {
                    "type": "stat_change",
                    "targetStat": "health",
                    "minValue": -10,
                    "maxValue": -20,
                    "probability": 0.6,
                    "resultText": "Kavga çıktı ve dayak yedin."
                }
            ]
        }
    ]
}
```

#### 3.5 Event Kategorileri
- **Aile**: Ebeveyn, kardeş, akraba olayları
- **Okul**: Eğitim, öğretmen, sınav olayları
- **Sağlık**: Hastalık, kaza, tedavi
- **Sosyal**: Arkadaşlık, aşk, toplumsal
- **Kariyer**: İş, terfi, işsizlik
- **Finansal**: Para, yatırım, borç
- **Rastgele**: Şans, talihsizlik, sürpriz
- **Suç**: Yasadışı aktiviteler
- **Askerlik**: Askerlik dönemi olayları

### Çıktılar
- Tüm event data classları
- JSON loader sistemi
- En az 50 temel event
- Event seçim algoritması

### Tamamlanma Kriterleri
- Eventler JSON'dan yüklenebilir
- Yaşa göre event filtreleme çalışır
- Seçimler sonuç üretir

---

## FAZ 4: Yaş Dönemleri ve Yaşlanma Sistemi

### Amaç
Karakterin yaşlanma sürecini, yaş dönemlerini ve ölüm mekaniklerini implement etmek.

### Görevler

#### 4.1 Life Stage Enum
```csharp
public enum LifeStage
{
    Baby,       // 0-4
    Child,      // 5-11
    Teen,       // 12-17
    YoungAdult, // 18-29
    Adult,      // 30-59
    Senior      // 60+
}
```

#### 4.2 Age Progression Sistemi
```csharp
public class AgeManager
{
    public void ProgressAge(CharacterData character);
    public LifeStage GetLifeStage(int age);
    public List<string> GetAgeEvents(int age);
}
```

#### 4.3 Yaşa Bağlı Değişimler
- **Bebek (0-4)**: Sadece aile olayları, yavaş stat değişimi
- **Çocuk (5-11)**: Okul başlar, arkadaşlıklar
- **Ergen (12-17)**: Hızlı stat değişimi, isyan, aşk
- **Genç Yetişkin (18-29)**: Kariyer, evlilik, büyük kararlar
- **Yetişkin (30-59)**: Stabilite veya kriz
- **Yaşlı (60+)**: Sağlık düşüşü, emeklilik

#### 4.4 Ölüm Mekanikleri
```csharp
public class DeathManager
{
    public bool CheckNaturalDeath(CharacterData character);
    public bool CheckAccidentalDeath(GameEvent currentEvent);
    public DeathCause GetDeathCause();
    public string GenerateEpitaph(CharacterData character);
}
```

#### 4.5 Ölüm Nedenleri
- Doğal ölüm (yaşlılık)
- Hastalık
- Kaza
- Suç/Cinayet
- İntihar (hassas içerik, dikkatli ele alınacak)

#### 4.6 Yaş Milestone Eventleri
- 6 yaş: İlkokul başlangıcı
- 12 yaş: Ortaokul
- 15 yaş: Lise
- 18 yaş: Üniversite/İş/Askerlik kararı
- 20-21: Askerlik (erkekler)
- 65: Emeklilik

### Çıktılar
- Life stage sistemi
- Age progression logic
- Ölüm sistemi
- Milestone eventleri

### Tamamlanma Kriterleri
- Yaş ilerlemesi düzgün çalışır
- Doğru life stage belirlenir
- Ölüm koşulları tetiklenir

---

## FAZ 5: Kariyer ve Eğitim Sistemleri

### Amaç
Türk eğitim sistemi ve kariyer yollarını implement etmek.

### Görevler

#### 5.1 Eğitim Sistemi
```csharp
public class EducationData
{
    public EducationLevel currentLevel;
    public string schoolName;
    public float gpa;
    public List<string> achievements;
    public bool isGraduated;
}

public enum EducationLevel
{
    None,
    PrimarySchool,      // İlkokul
    MiddleSchool,       // Ortaokul
    HighSchool,         // Lise
    University,         // Üniversite
    Masters,            // Yüksek Lisans
    Doctorate           // Doktora
}
```

#### 5.2 Lise Türleri
- Anadolu Lisesi
- Fen Lisesi
- Meslek Lisesi
- İmam Hatip Lisesi
- Özel Lise

#### 5.3 YKS Sınav Sistemi
```csharp
public class ExamSystem
{
    public ExamResult TakeYKS(CharacterData character);
    public List<University> GetAvailableUniversities(ExamResult result);
}
```

#### 5.4 Üniversite ve Bölümler
```json
{
    "universities": [
        {
            "name": "Boğaziçi Üniversitesi",
            "city": "İstanbul",
            "minScore": 480,
            "departments": ["Bilgisayar Mühendisliği", "İşletme", "Psikoloji"]
        }
    ]
}
```

#### 5.5 Kariyer Sistemi
```csharp
public class CareerData
{
    public Job currentJob;
    public int yearsInJob;
    public decimal salary;
    public int performanceRating;
    public List<Job> jobHistory;
}

public class Job
{
    public string title;
    public string company;
    public JobCategory category;
    public decimal baseSalary;
    public List<string> requirements;
    public List<string> promotionPath;
}
```

#### 5.6 Meslek Kategorileri ve Örnekler

**Kamu Sektörü**
- Devlet memuru, öğretmen, polis, asker, hakim

**Sağlık**
- Doktor, hemşire, eczacı, diş hekimi

**Hukuk**
- Avukat, noter, savcı

**Mühendislik**
- İnşaat, bilgisayar, makine, elektrik

**Ticaret**
- Esnaf, tüccar, emlakçı, sigortacı

**Hizmet**
- Taksici, kuaför, garson, temizlikçi

**Sanat/Medya**
- Oyuncu, şarkıcı, gazeteci, youtuber

**Tarım**
- Çiftçi, hayvancı, balıkçı

#### 5.7 Maaş ve Terfi Sistemi
- Yıllık zam (enflasyona göre)
- Performans bazlı prim
- Terfi koşulları
- İşten çıkarılma riski

### Çıktılar
- Tam eğitim sistemi
- YKS mekanikleri
- 50+ meslek tanımı
- Maaş/terfi algoritmaları

### Tamamlanma Kriterleri
- Eğitim akışı çalışır
- YKS sonucu üniversite belirler
- İş bulma/kaybetme çalışır

---

## FAZ 6: İlişkiler ve Aile Sistemi

### Amaç
NPC karakterler, aile ilişkileri, evlilik ve sosyal bağları implement etmek.

### Görevler

#### 6.1 Relationship Sistemi
```csharp
public class Relationship
{
    public string npcId;
    public string npcName;
    public RelationType type;
    public int intimacy;        // 0-100
    public int trust;           // 0-100
    public RelationshipStatus status;
    public List<string> memories;
}

public enum RelationType
{
    Parent, Sibling, Child, Spouse,
    Friend, Enemy, Colleague,
    Boyfriend, Girlfriend, Ex
}
```

#### 6.2 NPC Generator
```csharp
public class NPCGenerator
{
    public NPC GenerateParent(Gender gender);
    public NPC GenerateSibling();
    public NPC GenerateFriend(int playerAge);
    public NPC GenerateLoveInterest(int playerAge, Gender preference);
}
```

#### 6.3 Evlilik Sistemi
```csharp
public class MarriageSystem
{
    public bool ProposeMarriage(Relationship relationship);
    public void GetMarried(NPC spouse);
    public void GetDivorced();
    public float CalculateDivorceRisk();
}
```

#### 6.4 Çocuk Sistemi
```csharp
public class FamilySystem
{
    public NPC HaveChild();
    public void AdoptChild();
    public List<NPC> GetChildren();
}
```

#### 6.5 Sosyal Etkileşimler
- Sohbet et
- Hediye ver
- Birlikte vakit geçir
- Tartış
- Barış
- İlişkiyi bitir

#### 6.6 Aile Dinamikleri (Türkiye'ye Özgü)
- Ailevi baskı (evlilik, kariyer)
- Miras sistemi
- Bayram ziyaretleri
- Düğün organizasyonu
- Başlık parası (opsiyonel)

### Çıktılar
- NPC sistemi
- Relationship yönetimi
- Evlilik/boşanma
- Çocuk mekanikleri

### Tamamlanma Kriterleri
- NPC'ler oluşturulur
- İlişki statları değişir
- Evlilik/çocuk çalışır

---

## FAZ 7: Kaydetme/Yükleme Sistemi

### Amaç
Oyun ilerlemesinin kaydedilmesi ve yüklenmesi için persistent data sistemi.

### Görevler

#### 7.1 Save Data Yapısı
```csharp
[System.Serializable]
public class SaveData
{
    public string saveId;
    public DateTime saveDate;
    public CharacterData character;
    public List<NPC> npcs;
    public GameState gameState;
    public List<string> eventHistory;
    public Dictionary<string, object> customData;
}
```

#### 7.2 Save/Load Manager
```csharp
public class SaveManager : Singleton<SaveManager>
{
    public void SaveGame(int slotIndex);
    public SaveData LoadGame(int slotIndex);
    public void DeleteSave(int slotIndex);
    public List<SaveSlotInfo> GetAllSaves();
    public void AutoSave();
}
```

#### 7.3 JSON Serialization
```csharp
public class SaveSerializer
{
    public string Serialize(SaveData data);
    public SaveData Deserialize(string json);
}
```

#### 7.4 Save Slot Sistemi
- 5 manuel save slot
- 1 auto-save slot
- Save metadata (tarih, yaş, isim, thumbnail)

#### 7.5 Cloud Save (Gelecek için hazırlık)
- Interface tanımı
- Local implementasyon
- Firebase/PlayGames entegrasyon noktası

### Çıktılar
- SaveManager
- JSON serialization
- Save slot UI hazırlığı
- Auto-save mekanizması

### Tamamlanma Kriterleri
- Oyun kaydedilir
- Oyun yüklenir
- Veriler korunur

---

## FAZ 8: UI Implementasyonu (Kod ile)

### Amaç
Tüm kullanıcı arayüzünü tamamen kod ile oluşturmak, hiçbir manuel atama yapmamak.

### Görevler

#### 8.1 UI Factory Sistemi
```csharp
public class UIFactory
{
    public Canvas CreateCanvas(string name, RenderMode mode);
    public Button CreateButton(Transform parent, string text, Action onClick);
    public Text CreateText(Transform parent, string content, TextStyle style);
    public Panel CreatePanel(Transform parent, PanelStyle style);
    public ScrollView CreateScrollView(Transform parent);
}
```

#### 8.2 UI Style Sabitleri
```csharp
public static class UIStyles
{
    // Renkler
    public static Color PrimaryColor = new Color(0.2f, 0.6f, 0.9f);
    public static Color SecondaryColor = new Color(0.9f, 0.4f, 0.3f);
    public static Color BackgroundColor = new Color(0.1f, 0.1f, 0.15f);

    // Fontlar
    public static int TitleFontSize = 36;
    public static int BodyFontSize = 24;
    public static int SmallFontSize = 18;

    // Spacing
    public static float DefaultPadding = 20f;
    public static float ButtonHeight = 60f;
}
```

#### 8.3 Ana Menü
- Yeni Oyun butonu
- Devam Et butonu
- Ayarlar butonu
- Çıkış butonu
- Animasyonlar (kod ile)

#### 8.4 Oyun Ekranı
```
┌─────────────────────────────┐
│  İsim Soyisim    Yaş: 25   │
├─────────────────────────────┤
│ ♥ Sağlık    [████████░░] 80%│
│ ☺ Mutluluk  [██████░░░░] 60%│
│ ★ Zeka      [███████░░░] 70%│
│ $ Para      12.500 TL       │
├─────────────────────────────┤
│                             │
│   [EVENT DESCRIPTION]       │
│                             │
├─────────────────────────────┤
│  [Seçenek 1]                │
│  [Seçenek 2]                │
│  [Seçenek 3]                │
├─────────────────────────────┤
│ [Profil] [Yaşla] [İlişkiler]│
└─────────────────────────────┘
```

#### 8.5 Karakter Profil Ekranı
- Detaylı stat görünümü
- Eğitim geçmişi
- İş geçmişi
- Varlıklar
- Başarımlar

#### 8.6 İlişkiler Ekranı
- İlişki listesi
- İlişki detayları
- Etkileşim butonları

#### 8.7 Ayarlar Menüsü
- Ses ayarları
- Bildirim ayarları
- Kaydetme/Yükleme
- Hakkında

#### 8.8 Popup Sistemi
```csharp
public class PopupManager
{
    public void ShowConfirmation(string message, Action onConfirm, Action onCancel);
    public void ShowInfo(string title, string message);
    public void ShowEventResult(EventOutcome outcome);
}
```

#### 8.9 Responsive Tasarım
- Anchor/pivot hesaplamaları
- Farklı ekran boyutları
- Safe area desteği (notch)

### Çıktılar
- UIManager
- UIFactory
- Tüm ekran implementasyonları
- Popup sistemi
- Animasyon sistemi

### Tamamlanma Kriterleri
- Tüm UI kod ile çalışır
- Menüler arası geçiş
- Responsive görünüm

---

## FAZ 9: Test ve Polish

### Amaç
Oyunu test etmek, hataları düzeltmek ve son rötuşları yapmak.

### Görevler

#### 9.1 Balans Ayarları
- Stat değişim oranları
- Event olasılıkları
- Para ekonomisi
- Ölüm oranları

#### 9.2 İçerik Zenginleştirme
- Daha fazla event ekleme (toplam 200+)
- Daha fazla meslek (toplam 100+)
- Özel olaylar (Türkiye güncel olayları)
- Easter egg'ler

#### 9.3 Bug Fixing
- Null reference kontrolleri
- Edge case handling
- Memory leak kontrolü
- Save/Load hataları

#### 9.4 Performans Optimizasyonu
- Object pooling
- Lazy loading
- GC optimizasyonu
- UI batching

#### 9.5 Türkçe Metin Kontrolü
- Yazım hataları
- Tutarlılık kontrolü
- Ton/üslup kontrolü

#### 9.6 Mobil Test
- Touch responsiveness
- Farklı cihaz boyutları
- Batarya kullanımı
- Hafıza kullanımı

### Çıktılar
- Stabil oyun
- Optimize performans
- Zengin içerik
- Hatasız Türkçe

### Tamamlanma Kriterleri
- Crash yok
- Smooth oynanış
- İçerik yeterli
- Türkçe hatasız

---

## Zaman Çizelgesi (Tahmini)

| Faz | Süre | Kümülatif |
|-----|------|-----------|
| Faz 1 | 2-3 saat | 3 saat |
| Faz 2 | 2-3 saat | 6 saat |
| Faz 3 | 3-4 saat | 10 saat |
| Faz 4 | 2-3 saat | 13 saat |
| Faz 5 | 3-4 saat | 17 saat |
| Faz 6 | 3-4 saat | 21 saat |
| Faz 7 | 2-3 saat | 24 saat |
| Faz 8 | 4-5 saat | 29 saat |
| Faz 9 | 3-4 saat | 33 saat |

**Toplam Tahmini Süre**: 30-35 saat

---

## Risk Yönetimi

### Potansiyel Riskler
1. **Kapsam genişlemesi**: Özellik listesi kontrol altında tutulacak
2. **Balans sorunları**: İteratif test ile çözülecek
3. **Performans**: Erken optimizasyon yapılacak
4. **İçerik yetersizliği**: Minimum içerik hedefleri belirlenecek

### Mitigation Stratejileri
- Her fazın sonunda çalışır prototip
- Düzenli test
- Modüler mimari
- Extensible tasarım

---

## Başarı Kriterleri

Proje aşağıdaki durumlarda başarılı sayılacaktır:

1. ✅ Tüm fazlar tamamlanmış
2. ✅ Oyun baştan sona oynanabilir
3. ✅ En az 100 unique event var
4. ✅ En az 50 meslek var
5. ✅ Save/Load çalışıyor
6. ✅ UI tamamen kod ile oluşturulmuş
7. ✅ Türkçe içerik hatasız
8. ✅ Mobil platformlarda çalışıyor

---

## Sonraki Adımlar

Bu plan onaylandıktan sonra FAZ 1 ile geliştirmeye başlanacaktır.
