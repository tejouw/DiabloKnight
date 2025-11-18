using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Suç Sistemi - Suç işleme, yakalanma ve hapis yatma.
    /// </summary>
    public class CrimeSystem : Singleton<CrimeSystem>
    {
        private List<Crime> _allCrimes = new List<Crime>();

        // Hapis durumu
        private bool _isInPrison = false;
        private int _prisonYearsRemaining = 0;
        private string _currentPrisonReason = "";

        public bool IsInPrison => _isInPrison;
        public int PrisonYearsRemaining => _prisonYearsRemaining;

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeCrimes();
            Debug.Log("[CrimeSystem] Initialized successfully.");
        }

        private void InitializeCrimes()
        {
            _allCrimes = new List<Crime>
            {
                // KÜÇÜK SUÇLAR
                new Crime
                {
                    id = "shoplifting",
                    name = "Hırsızlık (Mağaza)",
                    description = "Mağazadan ürün çal",
                    category = CrimeCategory.Theft,
                    minAge = 8,
                    baseCatchChance = 0.35f,
                    minSentence = 0,
                    maxSentence = 1,
                    minReward = 50,
                    maxReward = 500,
                    successMessages = new string[]
                    {
                        "Başarıyla çaldın! Kimse fark etmedi.",
                        "Hızlı hareket ettin, cebine attın.",
                        "Güvenlik seni görmedi, şanslısın!"
                    },
                    caughtMessages = new string[]
                    {
                        "Güvenlik kamerası seni yakaladı!",
                        "Mağaza görevlisi seni gördü!",
                        "Alarm çaldı, yakalandın!"
                    },
                    funnyMessages = new string[]
                    {
                        "Çaldığın ürün bozuktu, boşuna risk aldın!",
                        "Annen seni mağazada gördü!",
                        "Yanlışlıkla kendi cüzdanını çaldın!"
                    }
                },
                new Crime
                {
                    id = "pickpocket",
                    name = "Yankesicilik",
                    description = "Birinin cebinden para çal",
                    category = CrimeCategory.Theft,
                    minAge = 12,
                    baseCatchChance = 0.40f,
                    minSentence = 0,
                    maxSentence = 2,
                    minReward = 100,
                    maxReward = 1000,
                    successMessages = new string[]
                    {
                        "Ustalıkla cüzdanı aldın!",
                        "Kurban hiçbir şey hissetmedi.",
                        "Metro'da harika bir iş çıkardın."
                    },
                    caughtMessages = new string[]
                    {
                        "Kurban elini yakaladı!",
                        "Bir tanık polisi çağırdı!",
                        "Polis sivil seni takip ediyormuş!"
                    },
                    funnyMessages = new string[]
                    {
                        "Cüzdanda sadece market fişleri vardı!",
                        "Yanlışlıkla polisin cüzdanını çaldın!",
                        "Kurban senin tanıdığın çıktı!"
                    }
                },
                new Crime
                {
                    id = "vandalism",
                    name = "Vandalizm",
                    description = "Kamu malına zarar ver",
                    category = CrimeCategory.Vandalism,
                    minAge = 10,
                    baseCatchChance = 0.30f,
                    minSentence = 0,
                    maxSentence = 1,
                    minReward = 0,
                    maxReward = 0,
                    happinessGain = 5,
                    successMessages = new string[]
                    {
                        "Grafiti yaptın, sanat eserin orada duruyor!",
                        "Cam kırdın, stres attın.",
                        "Duvarı boyadin, içini döktün!"
                    },
                    caughtMessages = new string[]
                    {
                        "Güvenlik kamerası seni görüntüledi!",
                        "Bir vatandaş ihbar etti!",
                        "Bekçi seni yakaladı!"
                    },
                    funnyMessages = new string[]
                    {
                        "Grafitide yazım hatası yaptın!",
                        "Kendi arabanı çizdin yanlışlıkla!",
                        "Sprey boya üstüne sıçradı!"
                    }
                },

                // ORTA SEVİYE SUÇLAR
                new Crime
                {
                    id = "car_theft",
                    name = "Araba Hırsızlığı",
                    description = "Park halindeki arabayı çal",
                    category = CrimeCategory.Theft,
                    minAge = 16,
                    baseCatchChance = 0.50f,
                    minSentence = 1,
                    maxSentence = 5,
                    minReward = 5000,
                    maxReward = 50000,
                    successMessages = new string[]
                    {
                        "Arabayı çalıştırdın ve kaçtın!",
                        "Hızlıca kontağı açtın.",
                        "Kimse görmeden sürdün gitti!"
                    },
                    caughtMessages = new string[]
                    {
                        "Arabanın alarmı çaldı!",
                        "Polis devriyesi seni gördü!",
                        "Arabanın sahibi geldi!"
                    },
                    funnyMessages = new string[]
                    {
                        "Araba benzinsiz kaldı 100 metre sonra!",
                        "Çaldığın araba babanınkiydı!",
                        "El freni çekiliydi, 2 saat uğraştın!"
                    }
                },
                new Crime
                {
                    id = "burglary",
                    name = "Ev Soygunu",
                    description = "Boş bir eve gir ve değerli eşyaları çal",
                    category = CrimeCategory.Burglary,
                    minAge = 18,
                    baseCatchChance = 0.45f,
                    minSentence = 2,
                    maxSentence = 8,
                    minReward = 2000,
                    maxReward = 30000,
                    successMessages = new string[]
                    {
                        "Evi soydun, değerli eşyaları aldın!",
                        "Alarm sistemi yoktu, şanslısın.",
                        "Hızlı girip çıktın, profesyonel iş!"
                    },
                    caughtMessages = new string[]
                    {
                        "Ev sahibi erken geldi!",
                        "Alarm şirketini aradı!",
                        "Komşu seni gördü ve polisi aradı!"
                    },
                    funnyMessages = new string[]
                    {
                        "Evde sadece IKEA mobilya vardı!",
                        "Ev köpeği bacağını ısırdı!",
                        "Yanlış eve girdin, polis eviydi!"
                    }
                },
                new Crime
                {
                    id = "drug_dealing",
                    name = "Uyuşturucu Satışı",
                    description = "Yasadışı madde sat",
                    category = CrimeCategory.DrugCrime,
                    minAge = 16,
                    baseCatchChance = 0.55f,
                    minSentence = 3,
                    maxSentence = 15,
                    minReward = 1000,
                    maxReward = 20000,
                    successMessages = new string[]
                    {
                        "Satış başarılı, nakit aldın.",
                        "Müşteri memnun kaldı.",
                        "İşler yolunda, para kazandın."
                    },
                    caughtMessages = new string[]
                    {
                        "Sivil polis seni satın aldı!",
                        "Müşteri ihbar etti!",
                        "Narkotik operasyonuna denk geldin!"
                    },
                    funnyMessages = new string[]
                    {
                        "Yanlışlıkla un sattın!",
                        "Müşteri paranın sahte olduğunu anladı!",
                        "Malı kaybettin, cebinde delik varmış!"
                    }
                },
                new Crime
                {
                    id = "fraud",
                    name = "Dolandırıcılık",
                    description = "İnsanları kandırarak para al",
                    category = CrimeCategory.Fraud,
                    minAge = 18,
                    baseCatchChance = 0.35f,
                    minSentence = 1,
                    maxSentence = 7,
                    minReward = 5000,
                    maxReward = 100000,
                    successMessages = new string[]
                    {
                        "Kurban parayı gönderdi!",
                        "Sahte yatırım planı işe yaradı.",
                        "E-posta dolandırıcılığı başarılı!"
                    },
                    caughtMessages = new string[]
                    {
                        "Kurban polise gitti!",
                        "Banka şüpheli işlem bildirdi!",
                        "Siber suçlar seni izliyordu!"
                    },
                    funnyMessages = new string[]
                    {
                        "Kurban senden daha iyi dolandırıcıydı!",
                        "Sahte çek geri döndü!",
                        "Yanlışlıkla kendi annenizi dolandırdın!"
                    }
                },

                // AĞIR SUÇLAR
                new Crime
                {
                    id = "bank_robbery",
                    name = "Banka Soygunu",
                    description = "Banka soy",
                    category = CrimeCategory.Robbery,
                    minAge = 18,
                    baseCatchChance = 0.70f,
                    minSentence = 10,
                    maxSentence = 25,
                    minReward = 50000,
                    maxReward = 500000,
                    successMessages = new string[]
                    {
                        "Banka soygununu başarıyla tamamladın!",
                        "Kasayı boşalttın, kaçış başarılı!",
                        "Profesyonel ekip çalışması!"
                    },
                    caughtMessages = new string[]
                    {
                        "Polis bankayı sardı!",
                        "Güvenlik seni etkisiz hale getirdi!",
                        "Kaçış aracı arızalandı!"
                    },
                    funnyMessages = new string[]
                    {
                        "Soygun notu yazım hatasıyla doluydu!",
                        "Kasa boştu, yeni açılmıştı!",
                        "Maskeni ters giymiştin!"
                    }
                },
                new Crime
                {
                    id = "assault",
                    name = "Saldırı",
                    description = "Birine fiziksel saldır",
                    category = CrimeCategory.Violence,
                    minAge = 14,
                    baseCatchChance = 0.50f,
                    minSentence = 1,
                    maxSentence = 10,
                    minReward = 0,
                    maxReward = 0,
                    happinessGain = -10,
                    successMessages = new string[]
                    {
                        "Kavgayı kazandın, adam yerde kaldı.",
                        "Bir yumrukta işi bitirdin.",
                        "Rakibin seni küçümsemişti, pişman oldu."
                    },
                    caughtMessages = new string[]
                    {
                        "Tanıklar polisi aradı!",
                        "Güvenlik kamerası her şeyi kaydetti!",
                        "Kurban şikayetçi oldu!"
                    },
                    funnyMessages = new string[]
                    {
                        "Yanlış kişiye saldırdın, boksörçıktı!",
                        "İlk yumruğu sen yedin!",
                        "Adamın eli alçıdaydı, sen kötü adam oldun!"
                    }
                },
                new Crime
                {
                    id = "murder",
                    name = "Cinayet",
                    description = "Birini öldür",
                    category = CrimeCategory.Violence,
                    minAge = 18,
                    baseCatchChance = 0.75f,
                    minSentence = 20,
                    maxSentence = 99, // Ömür boyu
                    minReward = 0,
                    maxReward = 0,
                    happinessGain = -50,
                    successMessages = new string[]
                    {
                        "Cinayet işledin... Kanın ellerinde.",
                        "Delilleri yok ettin, kimse bilmiyor.",
                        "Mükemmel cinayet... şimdilik."
                    },
                    caughtMessages = new string[]
                    {
                        "DNA delili seni ele verdi!",
                        "Bir tanık her şeyi gördü!",
                        "Polis cinayeti çözdü!"
                    },
                    funnyMessages = new string[]
                    {
                        "Kurban aslında ölü taklidi yapıyordu!",
                        "Yanlış kişiyi öldürdün!",
                        "Kurban vampir çıktı, geri döndü!"
                    }
                },

                // HAPİSTEN KAÇIŞ
                new Crime
                {
                    id = "prison_escape",
                    name = "Hapishaneden Kaçış",
                    description = "Hapishaneden kaçmayı dene",
                    category = CrimeCategory.Escape,
                    minAge = 18,
                    baseCatchChance = 0.80f,
                    minSentence = 5, // Ek ceza
                    maxSentence = 15,
                    minReward = 0,
                    maxReward = 0,
                    happinessGain = 30,
                    requiresPrison = true,
                    successMessages = new string[]
                    {
                        "Özgürsün! Hapishaneden kaçtın!",
                        "Tünel kazarak kaçtın!",
                        "Gardiyanı atlatın, dışardasın!"
                    },
                    caughtMessages = new string[]
                    {
                        "Gardiyanlar seni yakaladı!",
                        "Tünel çöktü, yakalandın!",
                        "Kaçış planın deşifre oldu!"
                    },
                    funnyMessages = new string[]
                    {
                        "Yanlış yöne kazdın, müdürün odasına çıktın!",
                        "Kaçarken gardıyanla çarpıştın!",
                        "Dışarı çıktın ama geri döndün, yemek güzeldi!"
                    }
                }
            };
        }

        /// <summary>
        /// Suç işle.
        /// </summary>
        public CrimeResult CommitCrime(string crimeId, CharacterData character)
        {
            var crime = _allCrimes.Find(c => c.id == crimeId);
            if (crime == null)
            {
                return new CrimeResult
                {
                    success = false,
                    message = "Suç bulunamadı."
                };
            }

            // Yaş kontrolü
            if (character.Age < crime.minAge)
            {
                return new CrimeResult
                {
                    success = false,
                    message = $"Bu suç için en az {crime.minAge} yaşında olmalısın."
                };
            }

            // Hapis kontrolü
            if (crime.requiresPrison && !_isInPrison)
            {
                return new CrimeResult
                {
                    success = false,
                    message = "Bu suç sadece hapisteyken işlenebilir."
                };
            }

            if (!crime.requiresPrison && _isInPrison)
            {
                return new CrimeResult
                {
                    success = false,
                    message = "Hapisteyken bu suçu işleyemezsin."
                };
            }

            // Yakalanma kontrolü
            float catchChance = crime.baseCatchChance;

            // Zeka yakalanma şansını azaltır
            catchChance -= character.Stats.Intelligence * 0.002f;
            catchChance = Mathf.Clamp(catchChance, 0.1f, 0.95f);

            float roll = Random.value;
            bool caught = roll < catchChance;
            bool funny = false;
            string message;

            if (caught)
            {
                // Yakalandı
                if (Random.value < 0.15f && crime.funnyMessages.Length > 0)
                {
                    message = crime.funnyMessages[Random.Range(0, crime.funnyMessages.Length)];
                    funny = true;
                }
                else
                {
                    message = crime.caughtMessages[Random.Range(0, crime.caughtMessages.Length)];
                }

                // Hapis cezası
                int sentence = Random.Range(crime.minSentence, crime.maxSentence + 1);

                if (sentence > 0)
                {
                    SendToPrison(sentence, crime.name);
                    message += $"\n\n{sentence} yıl hapis cezası aldın!";
                }
                else
                {
                    // Para cezası veya uyarı
                    int fine = Random.Range(100, 1000);
                    character.Finances.ModifyMoney(-fine, "Para cezası");
                    message += $"\n\n{fine} TL para cezası aldın.";
                }

                // Mutluluk düşüşü
                character.Stats.ModifyStat(StatType.Happiness, -10);

                return new CrimeResult
                {
                    success = false,
                    caught = true,
                    message = message,
                    crimeName = crime.name,
                    sentenceYears = sentence,
                    isFunny = funny
                };
            }
            else
            {
                // Başarılı
                if (Random.value < 0.1f && crime.funnyMessages.Length > 0)
                {
                    message = crime.funnyMessages[Random.Range(0, crime.funnyMessages.Length)];
                    funny = true;
                }
                else
                {
                    message = crime.successMessages[Random.Range(0, crime.successMessages.Length)];
                }

                // Ödül
                if (crime.maxReward > 0)
                {
                    int reward = Random.Range(crime.minReward, crime.maxReward + 1);
                    character.Finances.ModifyMoney(reward, crime.name);
                    message += $"\n\n{reward:N0} TL kazandın!";
                }

                // Mutluluk değişimi
                if (crime.happinessGain != 0)
                {
                    character.Stats.ModifyStat(StatType.Happiness, crime.happinessGain);
                }
                else
                {
                    character.Stats.ModifyStat(StatType.Happiness, 5);
                }

                // Hapishaneden kaçış başarılıysa
                if (crime.id == "prison_escape")
                {
                    ReleaseFromPrison();
                }

                return new CrimeResult
                {
                    success = true,
                    caught = false,
                    message = message,
                    crimeName = crime.name,
                    moneyGained = crime.maxReward > 0 ? Random.Range(crime.minReward, crime.maxReward + 1) : 0,
                    isFunny = funny
                };
            }
        }

        /// <summary>
        /// Hapise gönder.
        /// </summary>
        public void SendToPrison(int years, string reason)
        {
            _isInPrison = true;
            _prisonYearsRemaining = years;
            _currentPrisonReason = reason;

            EventBus.Publish(new PrisonEvent
            {
                EventType = PrisonEventType.Sentenced,
                Years = years,
                Reason = reason
            });

            Debug.Log($"[CrimeSystem] Sent to prison for {years} years. Reason: {reason}");
        }

        /// <summary>
        /// Hapisten çıkar.
        /// </summary>
        public void ReleaseFromPrison()
        {
            _isInPrison = false;
            _prisonYearsRemaining = 0;
            _currentPrisonReason = "";

            EventBus.Publish(new PrisonEvent
            {
                EventType = PrisonEventType.Released,
                Years = 0,
                Reason = "Cezası bitti"
            });

            Debug.Log("[CrimeSystem] Released from prison.");
        }

        /// <summary>
        /// Yıl ilerletme - hapis süresini azalt.
        /// </summary>
        public void ProgressYear()
        {
            if (_isInPrison && _prisonYearsRemaining > 0)
            {
                _prisonYearsRemaining--;

                if (_prisonYearsRemaining <= 0)
                {
                    ReleaseFromPrison();
                }
            }
        }

        /// <summary>
        /// Tüm suçları getir.
        /// </summary>
        public List<Crime> GetAllCrimes()
        {
            return _allCrimes;
        }

        /// <summary>
        /// Yaşa ve duruma göre işlenebilir suçları getir.
        /// </summary>
        public List<Crime> GetAvailableCrimes(int age)
        {
            return _allCrimes.FindAll(c => c.minAge <= age && c.requiresPrison == _isInPrison);
        }
    }

    #region Data Structures

    [System.Serializable]
    public class Crime
    {
        public string id;
        public string name;
        public string description;
        public CrimeCategory category;
        public int minAge;
        public float baseCatchChance;
        public int minSentence;
        public int maxSentence;
        public int minReward;
        public int maxReward;
        public int happinessGain;
        public bool requiresPrison;
        public string[] successMessages;
        public string[] caughtMessages;
        public string[] funnyMessages;
    }

    public enum CrimeCategory
    {
        Theft,      // Hırsızlık
        Burglary,   // Ev soygunu
        Robbery,    // Soygun
        Fraud,      // Dolandırıcılık
        DrugCrime,  // Uyuşturucu
        Violence,   // Şiddet
        Vandalism,  // Vandalizm
        Escape      // Kaçış
    }

    public class CrimeResult
    {
        public bool success;
        public bool caught;
        public string message;
        public string crimeName;
        public int moneyGained;
        public int sentenceYears;
        public bool isFunny;
    }

    public enum PrisonEventType
    {
        Sentenced,
        Released,
        Escaped
    }

    #endregion
}
