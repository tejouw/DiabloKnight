using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Aktivite Sistemi - Oyuncunun yapabileceği aktiviteleri yönetir.
    /// </summary>
    public class ActivitySystem : Singleton<ActivitySystem>
    {
        private List<Activity> _allActivities = new List<Activity>();

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeActivities();
            Debug.Log("[ActivitySystem] Initialized successfully.");
        }

        private void InitializeActivities()
        {
            _allActivities = new List<Activity>
            {
                // SPOR AKTİVİTELERİ
                new Activity
                {
                    id = "gym_workout",
                    name = "Spor Salonu",
                    description = "Egzersiz yaparak formda kal",
                    category = ActivityCategory.Sports,
                    minAge = 12,
                    cost = 50,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Health, Random.Range(2, 6) },
                        { StatType.Happiness, Random.Range(1, 4) },
                        { StatType.Appearance, Random.Range(1, 3) }
                    },
                    successMessages = new string[]
                    {
                        "Harika bir antrenman yaptın! Kasların şişti.",
                        "Ter içinde kaldın ama kendini harika hissediyorsun!",
                        "Personal trainer seni övdü. Gelişiyorsun!",
                        "Aynada kendine baktın ve gülümsedin.",
                        "Yeni bir kişisel rekor kırdın!"
                    },
                    failMessages = new string[]
                    {
                        "Ağırlıklar bugün çok ağır geldi.",
                        "Sakatlandın! Biraz dinlenmen gerek.",
                        "Motivasyonun düşük, erken ayrıldın."
                    },
                    funnyMessages = new string[]
                    {
                        "Koşu bandından düştün, herkes gördü. Utanç verici!",
                        "Osurdun ve herkes duydu. Salonu terk ettin.",
                        "Yanlışlıkla kadınlar bölümüne girdin!"
                    }
                },
                new Activity
                {
                    id = "jogging",
                    name = "Koşu",
                    description = "Parkta koşarak kardiyo yap",
                    category = ActivityCategory.Sports,
                    minAge = 8,
                    cost = 0,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Health, Random.Range(1, 4) },
                        { StatType.Happiness, Random.Range(1, 3) }
                    },
                    successMessages = new string[]
                    {
                        "Güzel bir koşu yaptın, temiz hava aldın.",
                        "Parkta tanıdıklarla selamlaştın.",
                        "Yeni bir rota keşfettin!"
                    },
                    failMessages = new string[]
                    {
                        "Ayak bileğini burktun.",
                        "Yağmura yakalandın."
                    },
                    funnyMessages = new string[]
                    {
                        "Bir köpek seni kovaladı!",
                        "Kuş kafana pisledi.",
                        "Bir ağaç köküne takılıp yere kapaklandın."
                    }
                },
                new Activity
                {
                    id = "swimming",
                    name = "Yüzme",
                    description = "Havuzda yüzerek egzersiz yap",
                    category = ActivityCategory.Sports,
                    minAge = 6,
                    cost = 30,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Health, Random.Range(2, 5) },
                        { StatType.Happiness, Random.Range(2, 4) }
                    },
                    successMessages = new string[]
                    {
                        "Suyun içinde özgür hissettin.",
                        "Birkaç tur attın, nefes kontrolün gelişti."
                    },
                    failMessages = new string[]
                    {
                        "Havuz çok kalabalıktı.",
                        "Kulağına su kaçtı."
                    },
                    funnyMessages = new string[]
                    {
                        "Mayonu kaybettin! Acil çıkış yaptın.",
                        "Can kurtaran seni kurtardı, utanç verici!"
                    }
                },

                // KÜLTÜR AKTİVİTELERİ
                new Activity
                {
                    id = "library",
                    name = "Kütüphane",
                    description = "Kitap okuyarak bilgini artır",
                    category = ActivityCategory.Culture,
                    minAge = 6,
                    cost = 0,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Intelligence, Random.Range(2, 5) },
                        { StatType.Happiness, Random.Range(0, 2) }
                    },
                    successMessages = new string[]
                    {
                        "Harika bir kitap buldun ve saatlerce okudun.",
                        "Yeni şeyler öğrendin!",
                        "Araştırma yaparak ödevini tamamladın.",
                        "Sessiz ortamda konsantrasyonun arttı."
                    },
                    failMessages = new string[]
                    {
                        "İstediğin kitap ödünçte çıktı.",
                        "Uyuyakaldın."
                    },
                    funnyMessages = new string[]
                    {
                        "Yanlışlıkla yetişkin bölümüne girdin!",
                        "Horladın ve kütüphaneden kovuldun.",
                        "Kitabın üzerine kahve döktün, para cezası yedin."
                    }
                },
                new Activity
                {
                    id = "cinema",
                    name = "Sinema",
                    description = "Film izleyerek eğlen",
                    category = ActivityCategory.Culture,
                    minAge = 5,
                    cost = 45,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Happiness, Random.Range(3, 7) }
                    },
                    successMessages = new string[]
                    {
                        "Harika bir film izledin!",
                        "Patlamış mısır ve kola ile muhteşem bir gece!",
                        "Film seni çok etkiledi.",
                        "Arkadaşlarınla güzel vakit geçirdin."
                    },
                    failMessages = new string[]
                    {
                        "Film berbattı, vakit kaybı oldu.",
                        "Önündeki kişi çok uzundu, göremeidn."
                    },
                    funnyMessages = new string[]
                    {
                        "Korku filminde çığlık attın, herkes güldü!",
                        "Yanlışlıkla çocuk filmine girdin.",
                        "Koltuğa kola döktün, yapış yapış oldun."
                    }
                },
                new Activity
                {
                    id = "theater",
                    name = "Tiyatro",
                    description = "Tiyatro oyunu izle",
                    category = ActivityCategory.Culture,
                    minAge = 10,
                    cost = 80,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Intelligence, Random.Range(1, 3) },
                        { StatType.Happiness, Random.Range(2, 5) }
                    },
                    successMessages = new string[]
                    {
                        "Etkileyici bir performans izledin.",
                        "Kültürel açıdan zenginleştin.",
                        "Oyuncuları alkışlamaktan ellerin acıdı."
                    },
                    failMessages = new string[]
                    {
                        "Oyun sıkıcıydı.",
                        "Koltuğun rahatsızdı."
                    },
                    funnyMessages = new string[]
                    {
                        "Telefonun çaldı, herkes sana baktı!",
                        "Uyuyakaldın ve horladın."
                    }
                },
                new Activity
                {
                    id = "museum",
                    name = "Müze",
                    description = "Müze ziyaret ederek kültürlen",
                    category = ActivityCategory.Culture,
                    minAge = 8,
                    cost = 25,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Intelligence, Random.Range(2, 4) },
                        { StatType.Happiness, Random.Range(1, 3) }
                    },
                    successMessages = new string[]
                    {
                        "Tarihi eserler çok etkileyiciydi.",
                        "Yeni şeyler öğrendin.",
                        "Rehberli tur çok bilgilendiriciydi."
                    },
                    failMessages = new string[]
                    {
                        "Müze çok kalabalıktı.",
                        "Ayakların yoruldu."
                    },
                    funnyMessages = new string[]
                    {
                        "Bir heykele dokundun ve alarm çaldı!",
                        "Kaybolup personelden yardım istedin."
                    }
                },

                // SOSYAL AKTİVİTELER
                new Activity
                {
                    id = "nightclub",
                    name = "Gece Kulübü",
                    description = "Dans et ve eğlen",
                    category = ActivityCategory.Social,
                    minAge = 18,
                    cost = 150,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Happiness, Random.Range(3, 8) },
                        { StatType.Health, Random.Range(-2, 1) }
                    },
                    successMessages = new string[]
                    {
                        "Gece boyunca dans ettin, harika eğlendin!",
                        "Yeni insanlarla tanıştın.",
                        "DJ muhteşemdi!",
                        "VIP bölümüne davet edildin!"
                    },
                    failMessages = new string[]
                    {
                        "Kulüp çok kalabalıktı.",
                        "Müzik berbattı."
                    },
                    funnyMessages = new string[]
                    {
                        "Sarhoş olup masanın üstünde dans ettin!",
                        "Yanlış kişiye yürüdün, tokadı yedin.",
                        "Bouncer seni dışarı attı.",
                        "Tuvalette sıkıştın, itfaiye geldi!"
                    }
                },
                new Activity
                {
                    id = "bar",
                    name = "Bar",
                    description = "Barda içki iç ve sosyalleş",
                    category = ActivityCategory.Social,
                    minAge = 18,
                    cost = 80,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Happiness, Random.Range(2, 5) },
                        { StatType.Health, Random.Range(-1, 1) }
                    },
                    successMessages = new string[]
                    {
                        "Barmen ile güzel sohbet ettin.",
                        "Arkadaşlarla eğlenceli bir gece geçirdin.",
                        "Yeni insanlarla tanıştın."
                    },
                    failMessages = new string[]
                    {
                        "Bar çok gürültülüydü.",
                        "İçkiler çok pahalıydı."
                    },
                    funnyMessages = new string[]
                    {
                        "Karaoke'de şarkı söyledin, herkes kulağını tıkadı!",
                        "Bar kavgasına karıştın!",
                        "Hesabı yanlışlıkla sen ödedin!"
                    }
                },
                new Activity
                {
                    id = "cafe",
                    name = "Kafe",
                    description = "Kafede oturup dinlen",
                    category = ActivityCategory.Social,
                    minAge = 12,
                    cost = 35,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Happiness, Random.Range(1, 4) }
                    },
                    successMessages = new string[]
                    {
                        "Güzel bir kahve içtin, rahatladan.",
                        "Kitap okuyarak huzurlu vakit geçirdin.",
                        "Arkadaşlarla sohbet ettin."
                    },
                    failMessages = new string[]
                    {
                        "Kafe çok kalabalıktı.",
                        "Kahve soğuktu."
                    },
                    funnyMessages = new string[]
                    {
                        "Kahveyi üstüne döktün!",
                        "WiFi şifresini kimse bilmiyordu."
                    }
                },
                new Activity
                {
                    id = "shopping",
                    name = "Alışveriş",
                    description = "AVM'de alışveriş yap",
                    category = ActivityCategory.Social,
                    minAge = 10,
                    cost = 200,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Happiness, Random.Range(2, 6) },
                        { StatType.Appearance, Random.Range(1, 3) }
                    },
                    successMessages = new string[]
                    {
                        "Harika kıyafetler buldun!",
                        "İndirimlerden yararlandın.",
                        "Yeni bir stil denedin, çok yakıştı!"
                    },
                    failMessages = new string[]
                    {
                        "Beğendiğin ürün bitmişti.",
                        "Çok para harcadın ama memnun kalmadın."
                    },
                    funnyMessages = new string[]
                    {
                        "Deneme kabininde kapı sıkıştı!",
                        "Yanlışlıkla çocuk bölümünden alışveriş yaptın.",
                        "Kredi kartın reddedildi, utanç verici!"
                    }
                },

                // KİŞİSEL BAKIM
                new Activity
                {
                    id = "spa",
                    name = "Spa & Masaj",
                    description = "Spa'da rahatlama seansı",
                    category = ActivityCategory.SelfCare,
                    minAge = 16,
                    cost = 300,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Happiness, Random.Range(4, 8) },
                        { StatType.Health, Random.Range(1, 3) },
                        { StatType.Appearance, Random.Range(1, 2) }
                    },
                    successMessages = new string[]
                    {
                        "Masaj muhteşemdi, tüm stres gitti!",
                        "Hamam deneyimi harikaydı.",
                        "Cildim bebek gibi oldu!"
                    },
                    failMessages = new string[]
                    {
                        "Masöz çok sert bastı, ağrıyor.",
                        "Ortam çok sıcaktı."
                    },
                    funnyMessages = new string[]
                    {
                        "Hamam da uyuyakaldın, herkes gitti!",
                        "Yanlış odaya girdin!"
                    }
                },
                new Activity
                {
                    id = "meditation",
                    name = "Meditasyon",
                    description = "Yoga ve meditasyon yap",
                    category = ActivityCategory.SelfCare,
                    minAge = 12,
                    cost = 40,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Happiness, Random.Range(2, 5) },
                        { StatType.Health, Random.Range(1, 3) }
                    },
                    successMessages = new string[]
                    {
                        "İç huzur buldun.",
                        "Nefesin düzenlendi, rahatladin.",
                        "Esnekliğin arttı."
                    },
                    failMessages = new string[]
                    {
                        "Konsantre olamadın.",
                        "Bacakların uyuştu."
                    },
                    funnyMessages = new string[]
                    {
                        "Yoga pozunda osurdun, herkes duydu!",
                        "Lotus pozisyonunda sıkıştın, kalkamadın!"
                    }
                },
                new Activity
                {
                    id = "hairsalon",
                    name = "Kuaför",
                    description = "Saçını yaptır",
                    category = ActivityCategory.SelfCare,
                    minAge = 10,
                    cost = 100,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Appearance, Random.Range(2, 5) },
                        { StatType.Happiness, Random.Range(1, 3) }
                    },
                    successMessages = new string[]
                    {
                        "Yeni saç modelin harika oldu!",
                        "Herkes saçını beğendi.",
                        "Kendini yenilenmiş hissettin."
                    },
                    failMessages = new string[]
                    {
                        "Kuaför istediğini anlamadı.",
                        "Saçın çok kısa kesildi."
                    },
                    funnyMessages = new string[]
                    {
                        "Saçın yeşile döndü! Felaket!",
                        "Kuaför kulağını kesti!",
                        "Fön makinesi saçını yaktı!"
                    }
                },
                new Activity
                {
                    id = "tattoo",
                    name = "Dövme Yaptır",
                    description = "Dövme stüdyosuna git",
                    category = ActivityCategory.SelfCare,
                    minAge = 18,
                    cost = 500,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Appearance, Random.Range(-1, 4) },
                        { StatType.Happiness, Random.Range(1, 4) }
                    },
                    successMessages = new string[]
                    {
                        "Dövmen harika oldu!",
                        "Anlamlı bir dövme yaptırdın.",
                        "Herkes dövmeni beğendi."
                    },
                    failMessages = new string[]
                    {
                        "Dövme düşündüğün gibi olmadı.",
                        "Çok acıdı!"
                    },
                    funnyMessages = new string[]
                    {
                        "Dövmede yazım hatası var!",
                        "Yanlış dövme yaptırdın, silikon sandılar!",
                        "Dövmeci bayıldı, yarım kaldı!"
                    }
                },

                // EĞİTİM AKTİVİTELERİ
                new Activity
                {
                    id = "study",
                    name = "Ders Çalış",
                    description = "Evde ders çalış",
                    category = ActivityCategory.Education,
                    minAge = 6,
                    cost = 0,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Intelligence, Random.Range(1, 4) },
                        { StatType.Happiness, Random.Range(-1, 1) }
                    },
                    successMessages = new string[]
                    {
                        "Verimli bir çalışma yaptın.",
                        "Zor konuyu anladın!",
                        "Sınavlara hazırsın."
                    },
                    failMessages = new string[]
                    {
                        "Konsantre olamadın.",
                        "Sosyal medya dikkatini dağıttı."
                    },
                    funnyMessages = new string[]
                    {
                        "Kitabın üstünde uyuyakaldın, salyalar aktı!",
                        "Anne yemek yap diye çalışmanı böldü."
                    }
                },
                new Activity
                {
                    id = "course",
                    name = "Kurs",
                    description = "Hobi veya meslek kursu al",
                    category = ActivityCategory.Education,
                    minAge = 12,
                    cost = 250,
                    statEffects = new Dictionary<StatType, int>
                    {
                        { StatType.Intelligence, Random.Range(2, 5) },
                        { StatType.Happiness, Random.Range(1, 3) }
                    },
                    successMessages = new string[]
                    {
                        "Yeni bir beceri öğrendin!",
                        "Sertifika aldın.",
                        "Hocan seni övdü."
                    },
                    failMessages = new string[]
                    {
                        "Kurs seviyesi çok zordu.",
                        "Öğretmen kötüydü."
                    },
                    funnyMessages = new string[]
                    {
                        "Yanlış kursa kaydoldun, yemek pişirme yerine karate!",
                        "Kurs arkadaşın seni stalklıyor!"
                    }
                }
            };
        }

        /// <summary>
        /// Tüm aktiviteleri getir.
        /// </summary>
        public List<Activity> GetAllActivities()
        {
            return _allActivities;
        }

        /// <summary>
        /// Kategoriye göre aktiviteleri getir.
        /// </summary>
        public List<Activity> GetActivitiesByCategory(ActivityCategory category)
        {
            return _allActivities.FindAll(a => a.category == category);
        }

        /// <summary>
        /// Yaşa uygun aktiviteleri getir.
        /// </summary>
        public List<Activity> GetAvailableActivities(int age)
        {
            return _allActivities.FindAll(a => a.minAge <= age);
        }

        /// <summary>
        /// Aktiviteyi gerçekleştir.
        /// </summary>
        public ActivityResult PerformActivity(string activityId, CharacterData character)
        {
            var activity = _allActivities.Find(a => a.id == activityId);
            if (activity == null)
            {
                return new ActivityResult
                {
                    success = false,
                    message = "Aktivite bulunamadı."
                };
            }

            // Yaş kontrolü
            if (character.Age < activity.minAge)
            {
                return new ActivityResult
                {
                    success = false,
                    message = $"Bu aktivite için en az {activity.minAge} yaşında olmalısın."
                };
            }

            // Para kontrolü
            if (character.Finances.CurrentMoney < activity.cost)
            {
                return new ActivityResult
                {
                    success = false,
                    message = "Yeterli paran yok!"
                };
            }

            // Para düş
            if (activity.cost > 0)
            {
                character.Finances.ModifyMoney(-activity.cost, activity.name);
            }

            // Sonuç belirle
            float roll = Random.value;
            string message;
            bool success;
            bool funny = false;

            if (roll < 0.1f && activity.funnyMessages.Length > 0) // %10 komik olay
            {
                message = activity.funnyMessages[Random.Range(0, activity.funnyMessages.Length)];
                success = false;
                funny = true;
            }
            else if (roll < 0.25f && activity.failMessages.Length > 0) // %15 başarısız
            {
                message = activity.failMessages[Random.Range(0, activity.failMessages.Length)];
                success = false;
            }
            else // %75 başarılı
            {
                message = activity.successMessages[Random.Range(0, activity.successMessages.Length)];
                success = true;

                // Stat efektlerini uygula
                foreach (var effect in activity.statEffects)
                {
                    character.Stats.ModifyStat(effect.Key, effect.Value);
                }
            }

            // Event yayınla
            EventBus.Publish(new ActivityCompletedEvent
            {
                ActivityId = activityId,
                ActivityName = activity.name,
                Success = success,
                Message = message
            });

            return new ActivityResult
            {
                success = success,
                message = message,
                activityName = activity.name,
                isFunny = funny
            };
        }
    }

    #region Data Structures

    [System.Serializable]
    public class Activity
    {
        public string id;
        public string name;
        public string description;
        public ActivityCategory category;
        public int minAge;
        public decimal cost;
        public Dictionary<StatType, int> statEffects;
        public string[] successMessages;
        public string[] failMessages;
        public string[] funnyMessages;
    }

    public enum ActivityCategory
    {
        Sports,      // Spor
        Culture,     // Kültür
        Social,      // Sosyal
        SelfCare,    // Kişisel Bakım
        Education    // Eğitim
    }

    public class ActivityResult
    {
        public bool success;
        public string message;
        public string activityName;
        public bool isFunny;
    }

    #endregion
}
