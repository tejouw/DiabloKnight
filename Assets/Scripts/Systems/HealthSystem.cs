using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Sağlık Sistemi - Hastalıklar, tedavi ve bağımlılıklar.
    /// </summary>
    public class HealthSystem : Singleton<HealthSystem>
    {
        private List<Disease> _allDiseases = new List<Disease>();
        private List<ActiveDisease> _activeConditions = new List<ActiveDisease>();
        private List<Addiction> _activeAddictions = new List<Addiction>();

        public List<ActiveDisease> ActiveConditions => _activeConditions;
        public List<Addiction> ActiveAddictions => _activeAddictions;

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeDiseases();
            Debug.Log("[HealthSystem] Initialized successfully.");
        }

        private void InitializeDiseases()
        {
            _allDiseases = new List<Disease>
            {
                // ÇOCUKLUK HASTALIKLARI
                new Disease
                {
                    id = "cold",
                    name = "Soğuk Algınlığı",
                    description = "Hafif bir soğuk algınlığı",
                    severity = DiseaseSeverity.Minor,
                    minAge = 0,
                    maxAge = 120,
                    baseChance = 0.15f,
                    healthImpact = -5,
                    treatmentCost = 50,
                    treatmentSuccessRate = 0.95f,
                    durationYears = 0,
                    symptoms = new string[] { "Burun akıntısı", "Hapşırık", "Hafif ateş" },
                    treatmentMessages = new string[]
                    {
                        "Bol sıvı ve dinlenme ile iyileştin.",
                        "Doktor vitamin verdi, geçti.",
                        "Anneannenin çorbası işe yaradı!"
                    },
                    funnyMessages = new string[]
                    {
                        "Herkes senden kaçıyor, bulaştıracaksın diye!",
                        "Hapşırırken gözlerin açık kaldı!",
                        "Mendil bitince kolunu kullandın..."
                    }
                },
                new Disease
                {
                    id = "flu",
                    name = "Grip",
                    description = "Mevsimsel grip",
                    severity = DiseaseSeverity.Minor,
                    minAge = 0,
                    maxAge = 120,
                    baseChance = 0.10f,
                    healthImpact = -10,
                    treatmentCost = 150,
                    treatmentSuccessRate = 0.90f,
                    durationYears = 0,
                    symptoms = new string[] { "Yüksek ateş", "Kas ağrısı", "Halsizlik" },
                    treatmentMessages = new string[]
                    {
                        "Grip aşısı ve ilaçlarla atlattın.",
                        "Bir hafta yattın ama iyisin artık.",
                        "Antibiyotik kürü işe yaradı."
                    }
                },
                new Disease
                {
                    id = "chickenpox",
                    name = "Su Çiçeği",
                    description = "Çocukluk hastalığı",
                    severity = DiseaseSeverity.Minor,
                    minAge = 1,
                    maxAge = 15,
                    baseChance = 0.05f,
                    healthImpact = -15,
                    treatmentCost = 200,
                    treatmentSuccessRate = 0.95f,
                    durationYears = 0,
                    symptoms = new string[] { "Kaşıntılı döküntüler", "Ateş", "Halsizlik" },
                    treatmentMessages = new string[]
                    {
                        "Kaşıntı kremi ve sabır. Geçti!",
                        "Okula gidemiyorsun ama Netflix var!",
                        "İzler kaldı ama geçici."
                    },
                    funnyMessages = new string[]
                    {
                        "Tüm vücudun polka noktalı oldu!",
                        "Kaşımaman gerekiyordu, kaşıdın, iz kaldı!",
                        "Arkadaşların seni 'Benekli' diye çağırıyor."
                    }
                },

                // CİDDİ HASTALIKLAR
                new Disease
                {
                    id = "appendicitis",
                    name = "Apandisit",
                    description = "Akut apandisit",
                    severity = DiseaseSeverity.Serious,
                    minAge = 5,
                    maxAge = 50,
                    baseChance = 0.02f,
                    healthImpact = -30,
                    treatmentCost = 15000,
                    treatmentSuccessRate = 0.98f,
                    durationYears = 0,
                    symptoms = new string[] { "Şiddetli karın ağrısı", "Bulantı", "Ateş" },
                    treatmentMessages = new string[]
                    {
                        "Ameliyat başarılı geçti. İyileşiyorsun.",
                        "Apandis alındı, bir haftada normale döneceksin.",
                        "Laparoskopik cerrahi hızlı iyileşme sağladı."
                    },
                    funnyMessages = new string[]
                    {
                        "Ameliyat izini savaş yarası olarak gösteriyorsun!",
                        "Anesteziden çıkarken saçmaladın, video çekildi!"
                    }
                },
                new Disease
                {
                    id = "diabetes",
                    name = "Diyabet",
                    description = "Tip 2 Diyabet",
                    severity = DiseaseSeverity.Chronic,
                    minAge = 30,
                    maxAge = 120,
                    baseChance = 0.03f,
                    healthImpact = -20,
                    treatmentCost = 500, // Aylık
                    treatmentSuccessRate = 0.80f, // Kontrol
                    durationYears = 99, // Ömür boyu
                    symptoms = new string[] { "Sürekli susuzluk", "Sık idrara çıkma", "Yorgunluk" },
                    treatmentMessages = new string[]
                    {
                        "İlaç ve diyet ile kontrol altında.",
                        "Kan şekerin dengelendi.",
                        "Yaşam tarzı değişikliği işe yarıyor."
                    }
                },
                new Disease
                {
                    id = "hypertension",
                    name = "Hipertansiyon",
                    description = "Yüksek tansiyon",
                    severity = DiseaseSeverity.Chronic,
                    minAge = 35,
                    maxAge = 120,
                    baseChance = 0.05f,
                    healthImpact = -15,
                    treatmentCost = 300,
                    treatmentSuccessRate = 0.85f,
                    durationYears = 99,
                    symptoms = new string[] { "Baş ağrısı", "Baş dönmesi", "Nefes darlığı" },
                    treatmentMessages = new string[]
                    {
                        "Tansiyon ilacı ile kontrol altında.",
                        "Tuz kısıtlaması işe yarıyor.",
                        "Düzenli ölçüm yapıyorsun."
                    }
                },
                new Disease
                {
                    id = "heart_attack",
                    name = "Kalp Krizi",
                    description = "Miyokard enfarktüsü",
                    severity = DiseaseSeverity.Critical,
                    minAge = 40,
                    maxAge = 120,
                    baseChance = 0.02f,
                    healthImpact = -50,
                    treatmentCost = 100000,
                    treatmentSuccessRate = 0.70f,
                    durationYears = 0,
                    canBeFatal = true,
                    symptoms = new string[] { "Göğüs ağrısı", "Sol kola yayılan ağrı", "Terleme" },
                    treatmentMessages = new string[]
                    {
                        "Stent takıldı, iyileşiyorsun.",
                        "By-pass ameliyatı başarılı.",
                        "Erken müdahale hayat kurtardı."
                    },
                    funnyMessages = new string[]
                    {
                        "Doktor 'döner yeme' dedi, döner yemeye devam!",
                        "Kalbin 'dur' dedi ama sen 'devam' dedin!"
                    }
                },
                new Disease
                {
                    id = "cancer",
                    name = "Kanser",
                    description = "Malign tümör",
                    severity = DiseaseSeverity.Critical,
                    minAge = 20,
                    maxAge = 120,
                    baseChance = 0.01f,
                    healthImpact = -60,
                    treatmentCost = 500000,
                    treatmentSuccessRate = 0.50f,
                    durationYears = 5,
                    canBeFatal = true,
                    symptoms = new string[] { "Kilo kaybı", "Yorgunluk", "Ağrılar" },
                    treatmentMessages = new string[]
                    {
                        "Kemoterapi işe yaradı, tümör küçüldü.",
                        "Ameliyatla alındı, remisyondasın.",
                        "Radyoterapi başarılı oldu."
                    }
                },
                new Disease
                {
                    id = "stroke",
                    name = "İnme",
                    description = "Beyin kanaması",
                    severity = DiseaseSeverity.Critical,
                    minAge = 50,
                    maxAge = 120,
                    baseChance = 0.015f,
                    healthImpact = -55,
                    treatmentCost = 200000,
                    treatmentSuccessRate = 0.60f,
                    durationYears = 0,
                    canBeFatal = true,
                    symptoms = new string[] { "Yüzde sarkma", "Konuşma bozukluğu", "Kol-bacak güçsüzlüğü" },
                    treatmentMessages = new string[]
                    {
                        "Fizik tedavi ile fonksiyonlar geri geliyor.",
                        "Erken müdahale hasarı sınırladı.",
                        "Rehabilitasyon süreci iyi gidiyor."
                    }
                },

                // BULAŞICI HASTALIKLAR
                new Disease
                {
                    id = "food_poisoning",
                    name = "Gıda Zehirlenmesi",
                    description = "Bozuk yiyecekten zehirlenme",
                    severity = DiseaseSeverity.Minor,
                    minAge = 0,
                    maxAge = 120,
                    baseChance = 0.08f,
                    healthImpact = -10,
                    treatmentCost = 100,
                    treatmentSuccessRate = 0.95f,
                    durationYears = 0,
                    symptoms = new string[] { "Mide bulantısı", "Kusma", "İshal" },
                    treatmentMessages = new string[]
                    {
                        "Serum ve dinlenme ile geçti.",
                        "24 saat içinde düzeldin.",
                        "Bir daha o kebapçıya gitmeyeceksin!"
                    },
                    funnyMessages = new string[]
                    {
                        "Tuvalet en yakın arkadaşın oldu!",
                        "Yatağı terk edemeden 3 gün geçti.",
                        "Mutfağı temizlemen gerekti... acil!"
                    }
                },
                new Disease
                {
                    id = "std",
                    name = "Cinsel Yolla Bulaşan Hastalık",
                    description = "CYBH",
                    severity = DiseaseSeverity.Serious,
                    minAge = 16,
                    maxAge = 80,
                    baseChance = 0.01f,
                    healthImpact = -25,
                    treatmentCost = 2000,
                    treatmentSuccessRate = 0.85f,
                    durationYears = 0,
                    symptoms = new string[] { "Çeşitli belirtiler" },
                    treatmentMessages = new string[]
                    {
                        "Antibiyotik tedavisi işe yaradı.",
                        "Kontrol testleri negatif çıktı.",
                        "Bir daha korunmasız olmayacaksın!"
                    },
                    funnyMessages = new string[]
                    {
                        "Partnerine nasıl açıklayacaksın?",
                        "Tinder'ı silme vakti!",
                        "Doktorun suratına bakamadın."
                    }
                },

                // PSİKOLOJİK
                new Disease
                {
                    id = "depression",
                    name = "Depresyon",
                    description = "Klinik depresyon",
                    severity = DiseaseSeverity.Serious,
                    minAge = 12,
                    maxAge = 120,
                    baseChance = 0.04f,
                    healthImpact = -10,
                    happinessImpact = -40,
                    treatmentCost = 1000,
                    treatmentSuccessRate = 0.70f,
                    durationYears = 2,
                    symptoms = new string[] { "Mutsuzluk", "İsteksizlik", "Uyku bozuklukları" },
                    treatmentMessages = new string[]
                    {
                        "Terapi ve ilaç kombinasyonu işe yarıyor.",
                        "Kendini daha iyi hissediyorsun.",
                        "Her gün biraz daha iyisin."
                    }
                },
                new Disease
                {
                    id = "anxiety",
                    name = "Anksiyete Bozukluğu",
                    description = "Kaygı bozukluğu",
                    severity = DiseaseSeverity.Serious,
                    minAge = 10,
                    maxAge = 120,
                    baseChance = 0.05f,
                    healthImpact = -5,
                    happinessImpact = -30,
                    treatmentCost = 800,
                    treatmentSuccessRate = 0.75f,
                    durationYears = 1,
                    symptoms = new string[] { "Sürekli endişe", "Panik ataklar", "Kalp çarpıntısı" },
                    treatmentMessages = new string[]
                    {
                        "Bilişsel davranış terapisi işe yarıyor.",
                        "Nefes egzersizleri yardımcı oluyor.",
                        "Anksiyete seviyen düşüyor."
                    }
                }
            };
        }

        /// <summary>
        /// Rastgele hastalık kontrolü (yıllık).
        /// </summary>
        public DiseaseCheckResult CheckForDisease(CharacterData character)
        {
            int age = character.Age;
            int health = character.Stats.Health;

            // Sağlık düşükse hastalık şansı artar
            float healthModifier = (100 - health) * 0.005f;

            foreach (var disease in _allDiseases)
            {
                if (age < disease.minAge || age > disease.maxAge) continue;
                if (_activeConditions.Exists(c => c.disease.id == disease.id)) continue;

                float chance = disease.baseChance + healthModifier;

                // Yaşlılıkta ciddi hastalık şansı artar
                if (age > 60 && disease.severity >= DiseaseSeverity.Serious)
                {
                    chance *= 1.5f;
                }

                if (Random.value < chance)
                {
                    // Hastalık yakalandı
                    var activeDisease = new ActiveDisease
                    {
                        disease = disease,
                        diagnosedYear = age,
                        yearsRemaining = disease.durationYears,
                        isTreated = false
                    };

                    _activeConditions.Add(activeDisease);

                    // Sağlık etkisi
                    character.Stats.ModifyStat(StatType.Health, disease.healthImpact);
                    if (disease.happinessImpact != 0)
                    {
                        character.Stats.ModifyStat(StatType.Happiness, disease.happinessImpact);
                    }

                    string message = $"{disease.name} teşhisi kondu!\n\nBelirtiler: {string.Join(", ", disease.symptoms)}";

                    if (disease.canBeFatal)
                    {
                        message += "\n\nBU CİDDİ BİR DURUM! Acil tedavi gerekli.";
                    }

                    return new DiseaseCheckResult
                    {
                        gotSick = true,
                        disease = disease,
                        message = message
                    };
                }
            }

            return new DiseaseCheckResult
            {
                gotSick = false,
                message = "Sağlıklısın!"
            };
        }

        /// <summary>
        /// Doktora git ve tedavi ol.
        /// </summary>
        public TreatmentResult SeekTreatment(string diseaseId, CharacterData character)
        {
            var activeDisease = _activeConditions.Find(c => c.disease.id == diseaseId);
            if (activeDisease == null)
            {
                return new TreatmentResult
                {
                    success = false,
                    message = "Bu hastalığın yok."
                };
            }

            var disease = activeDisease.disease;

            // Para kontrolü
            if (character.Finances.CurrentMoney < disease.treatmentCost)
            {
                return new TreatmentResult
                {
                    success = false,
                    message = $"Tedavi için {disease.treatmentCost:N0} TL gerekli. Yeterli paranız yok!"
                };
            }

            // Tedavi ücreti
            character.Finances.ModifyMoney(-disease.treatmentCost, $"{disease.name} tedavisi");

            // Tedavi başarısı
            bool cured = Random.value < disease.treatmentSuccessRate;

            if (cured)
            {
                string message;
                if (Random.value < 0.2f && disease.funnyMessages != null && disease.funnyMessages.Length > 0)
                {
                    message = disease.funnyMessages[Random.Range(0, disease.funnyMessages.Length)];
                }
                else
                {
                    message = disease.treatmentMessages[Random.Range(0, disease.treatmentMessages.Length)];
                }

                // Kronik değilse kaldır
                if (disease.severity != DiseaseSeverity.Chronic)
                {
                    _activeConditions.Remove(activeDisease);
                    character.Stats.ModifyStat(StatType.Health, -disease.healthImpact / 2); // Yarısı geri gelir
                }
                else
                {
                    activeDisease.isTreated = true;
                }

                character.Stats.ModifyStat(StatType.Happiness, 10);

                return new TreatmentResult
                {
                    success = true,
                    cured = true,
                    message = message
                };
            }
            else
            {
                string[] failMessages = new string[]
                {
                    "Tedavi işe yaramadı. Farklı bir yöntem denemeliyiz.",
                    "İlaçlar etkisiz kaldı. Daha güçlü tedavi gerekebilir.",
                    "Vücudun tedaviye cevap vermedi.",
                    "Hastalık dirençli çıktı."
                };

                character.Stats.ModifyStat(StatType.Happiness, -5);

                return new TreatmentResult
                {
                    success = true,
                    cured = false,
                    message = failMessages[Random.Range(0, failMessages.Length)]
                };
            }
        }

        /// <summary>
        /// Bağımlılık geliştir.
        /// </summary>
        public void DevelopAddiction(AddictionType type, CharacterData character)
        {
            if (_activeAddictions.Exists(a => a.type == type)) return;

            var addiction = new Addiction
            {
                type = type,
                severity = 1,
                yearsActive = 0
            };

            _activeAddictions.Add(addiction);

            string name = GetAddictionName(type);
            character.Stats.ModifyStat(StatType.Health, -10);
            character.Stats.ModifyStat(StatType.Happiness, -15);

            EventBus.Publish(new AddictionEvent
            {
                Type = type,
                EventType = AddictionEventType.Developed,
                Message = $"{name} bağımlılığı geliştirdin!"
            });
        }

        /// <summary>
        /// Bağımlılıktan kurtul.
        /// </summary>
        public RehabResult GoToRehab(AddictionType type, CharacterData character)
        {
            var addiction = _activeAddictions.Find(a => a.type == type);
            if (addiction == null)
            {
                return new RehabResult
                {
                    success = false,
                    message = "Bu bağımlılığın yok."
                };
            }

            decimal rehabCost = 50000;

            if (character.Finances.CurrentMoney < rehabCost)
            {
                return new RehabResult
                {
                    success = false,
                    message = "Rehabilitasyon için yeterli paranız yok!"
                };
            }

            character.Finances.ModifyMoney(-rehabCost, "Rehabilitasyon tedavisi");

            // Başarı şansı bağımlılık şiddetine göre
            float successChance = 0.7f - (addiction.severity * 0.1f);
            bool success = Random.value < successChance;

            if (success)
            {
                _activeAddictions.Remove(addiction);
                character.Stats.ModifyStat(StatType.Health, 15);
                character.Stats.ModifyStat(StatType.Happiness, 20);

                return new RehabResult
                {
                    success = true,
                    cured = true,
                    message = "Rehabilitasyon başarılı! Bağımlılıktan kurtuldun!"
                };
            }
            else
            {
                addiction.severity = Mathf.Max(1, addiction.severity - 1);

                string[] failMessages = new string[]
                {
                    "Rehabilitasyon tam işe yaramadı. Tekrar denemelsin.",
                    "İyileşme süreci zor geçiyor. Sabır gerekiyor.",
                    "Nüks ettin. Bir daha dene."
                };

                return new RehabResult
                {
                    success = true,
                    cured = false,
                    message = failMessages[Random.Range(0, failMessages.Length)]
                };
            }
        }

        /// <summary>
        /// Yıllık sağlık kontrolü.
        /// </summary>
        public void ProgressYear(CharacterData character)
        {
            // Aktif hastalıkların etkisini uygula
            foreach (var condition in _activeConditions)
            {
                if (!condition.isTreated)
                {
                    character.Stats.ModifyStat(StatType.Health, condition.disease.healthImpact / 4);
                }

                if (condition.yearsRemaining > 0)
                {
                    condition.yearsRemaining--;
                    if (condition.yearsRemaining <= 0 && condition.disease.severity != DiseaseSeverity.Chronic)
                    {
                        // Hastalık geçti
                        _activeConditions.Remove(condition);
                        break;
                    }
                }
            }

            // Bağımlılık etkileri
            foreach (var addiction in _activeAddictions)
            {
                addiction.yearsActive++;
                if (addiction.yearsActive > 5) addiction.severity = Mathf.Min(5, addiction.severity + 1);

                character.Stats.ModifyStat(StatType.Health, -5 * addiction.severity);
                character.Stats.ModifyStat(StatType.Happiness, -3 * addiction.severity);
            }
        }

        private string GetAddictionName(AddictionType type)
        {
            return type switch
            {
                AddictionType.Alcohol => "Alkol",
                AddictionType.Smoking => "Sigara",
                AddictionType.Drugs => "Uyuşturucu",
                AddictionType.Gambling => "Kumar",
                AddictionType.Gaming => "Oyun",
                AddictionType.SocialMedia => "Sosyal Medya",
                _ => "Bilinmeyen"
            };
        }

        public List<Disease> GetAllDiseases() => _allDiseases;
    }

    #region Data Structures

    [System.Serializable]
    public class Disease
    {
        public string id;
        public string name;
        public string description;
        public DiseaseSeverity severity;
        public int minAge;
        public int maxAge;
        public float baseChance;
        public int healthImpact;
        public int happinessImpact;
        public decimal treatmentCost;
        public float treatmentSuccessRate;
        public int durationYears;
        public bool canBeFatal;
        public string[] symptoms;
        public string[] treatmentMessages;
        public string[] funnyMessages;
    }

    public enum DiseaseSeverity
    {
        Minor,      // Hafif
        Serious,    // Ciddi
        Chronic,    // Kronik
        Critical    // Kritik
    }

    [System.Serializable]
    public class ActiveDisease
    {
        public Disease disease;
        public int diagnosedYear;
        public int yearsRemaining;
        public bool isTreated;
    }

    public class DiseaseCheckResult
    {
        public bool gotSick;
        public Disease disease;
        public string message;
    }

    public class TreatmentResult
    {
        public bool success;
        public bool cured;
        public string message;
    }

    [System.Serializable]
    public class Addiction
    {
        public AddictionType type;
        public int severity; // 1-5
        public int yearsActive;
    }

    public enum AddictionType
    {
        Alcohol,
        Smoking,
        Drugs,
        Gambling,
        Gaming,
        SocialMedia
    }

    public enum AddictionEventType
    {
        Developed,
        Worsened,
        Recovered
    }

    public class RehabResult
    {
        public bool success;
        public bool cured;
        public string message;
    }

    #endregion
}
