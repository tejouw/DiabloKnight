using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Crime
{
    /// <summary>
    /// Suç tipi enum.
    /// </summary>
    public enum CrimeType
    {
        Pickpocket,         // Yankesicilik
        Shoplifting,        // Hırsızlık (mağaza)
        CarTheft,           // Araba hırsızlığı
        HomeInvasion,       // Ev soygunu
        BankRobbery,        // Banka soygunu
        GrandTheftAuto,     // Büyük ölçekli araba hırsızlığı
        DrugDealing,        // Uyuşturucu satıcılığı
        Murder,             // Cinayet
        Fraud,              // Dolandırıcılık
        TaxEvasion,         // Vergi kaçırma
        Assault,            // Saldırı
        Vandalism           // Vandalizm
    }

    /// <summary>
    /// Suç tanımı.
    /// </summary>
    [System.Serializable]
    public class CrimeDefinition
    {
        public CrimeType type;
        public string name;
        public string description;
        public int minAge;
        public float successChance;      // 0-1
        public int minReward;
        public int maxReward;
        public int minPrisonYears;
        public int maxPrisonYears;
        public int notorietyGain;        // Kötü şöhret kazancı
    }

    /// <summary>
    /// Suç sonucu.
    /// </summary>
    public class CrimeResult
    {
        public CrimeType crimeType;
        public bool success;
        public bool caught;
        public int reward;
        public int prisonSentence;
        public string resultText;
    }

    /// <summary>
    /// Suç Yöneticisi.
    /// </summary>
    public class CrimeManager : Singleton<CrimeManager>
    {
        private List<CrimeDefinition> _allCrimes;
        private Dictionary<CrimeType, CrimeDefinition> _crimeLookup;

        // Hapis durumu
        private int _remainingPrisonYears = 0;
        private int _totalPrisonTime = 0;

        #region Properties

        public bool IsInPrison => _remainingPrisonYears > 0;
        public int RemainingPrisonYears => _remainingPrisonYears;
        public int TotalPrisonTime => _totalPrisonTime;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            LoadCrimes();

            // Yaş ilerlediğinde hapis süresini azalt
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);
        }

        private void LoadCrimes()
        {
            _allCrimes = GetDefaultCrimes();
            _crimeLookup = new Dictionary<CrimeType, CrimeDefinition>();

            foreach (var crime in _allCrimes)
            {
                _crimeLookup[crime.type] = crime;
            }

            Debug.Log($"[CrimeManager] Loaded {_allCrimes.Count} crimes.");
        }

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            if (_remainingPrisonYears > 0)
            {
                _remainingPrisonYears--;

                if (_remainingPrisonYears == 0)
                {
                    // Cezası bitti
                    EventBus.Publish(new PrisonReleasedEvent());
                    Debug.Log("[CrimeManager] Released from prison!");
                }
            }
        }

        #endregion

        #region Crime Execution

        /// <summary>
        /// Mevcut suçları al.
        /// </summary>
        public List<CrimeDefinition> GetAvailableCrimes(int characterAge)
        {
            var available = new List<CrimeDefinition>();
            foreach (var crime in _allCrimes)
            {
                if (crime.minAge <= characterAge)
                {
                    available.Add(crime);
                }
            }
            return available;
        }

        /// <summary>
        /// Suç işle.
        /// </summary>
        public CrimeResult CommitCrime(CrimeType crimeType)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null)
            {
                return new CrimeResult
                {
                    success = false,
                    resultText = "Karakter bulunamadı!"
                };
            }

            // Hapiste mi?
            if (IsInPrison)
            {
                return new CrimeResult
                {
                    success = false,
                    resultText = $"Hapistesin! Kalan süre: {_remainingPrisonYears} yıl"
                };
            }

            // Suç tanımını al
            if (!_crimeLookup.TryGetValue(crimeType, out var crime))
            {
                return new CrimeResult
                {
                    success = false,
                    resultText = "Suç tanımı bulunamadı!"
                };
            }

            // Yaş kontrolü
            if (character.Age < crime.minAge)
            {
                return new CrimeResult
                {
                    success = false,
                    resultText = $"Bu suç için en az {crime.minAge} yaşında olmalısın."
                };
            }

            var result = new CrimeResult { crimeType = crimeType };

            // Başarı kontrolü
            bool success = Random.value <= crime.successChance;

            if (success)
            {
                result.success = true;
                result.reward = Random.Range(crime.minReward, crime.maxReward + 1);

                character.Finances.ModifyMoney(result.reward, crime.name);

                // Yakalanma şansı (başarılı olsa bile)
                float catchChance = 0.2f; // %20 yakalanma şansı
                result.caught = Random.value <= catchChance;

                if (result.caught)
                {
                    // Yakalandı
                    result.prisonSentence = Random.Range(crime.minPrisonYears, crime.maxPrisonYears + 1);
                    _remainingPrisonYears = result.prisonSentence;
                    _totalPrisonTime += result.prisonSentence;

                    character.Stats.ModifyStat(StatType.Happiness, -20);

                    result.resultText = $"{crime.name} başarılı! {result.reward:N0} TL kazandın ama yakalandın! Ceza: {result.prisonSentence} yıl hapis.";

                    EventBus.Publish(new ArrestedEvent
                    {
                        CrimeType = crimeType,
                        Sentence = result.prisonSentence
                    });
                }
                else
                {
                    // Yakalanmadı
                    character.Stats.ModifyStat(StatType.Happiness, 5);
                    result.resultText = $"{crime.name} başarılı! {result.reward:N0} TL kazandın ve yakalanmadın!";
                }
            }
            else
            {
                result.success = false;

                // Yakalanma şansı (başarısız olursa daha yüksek)
                float catchChance = 0.6f;
                result.caught = Random.value <= catchChance;

                if (result.caught)
                {
                    result.prisonSentence = Random.Range(crime.minPrisonYears, crime.maxPrisonYears + 1);
                    _remainingPrisonYears = result.prisonSentence;
                    _totalPrisonTime += result.prisonSentence;

                    character.Stats.ModifyStat(StatType.Happiness, -25);

                    result.resultText = $"{crime.name} başarısız ve yakalandın! Ceza: {result.prisonSentence} yıl hapis.";

                    EventBus.Publish(new ArrestedEvent
                    {
                        CrimeType = crimeType,
                        Sentence = result.prisonSentence
                    });
                }
                else
                {
                    character.Stats.ModifyStat(StatType.Happiness, -5);
                    result.resultText = $"{crime.name} başarısız ama yakalanmadın.";
                }
            }

            return result;
        }

        /// <summary>
        /// Hapisten kaç (düşük şans).
        /// </summary>
        public bool TryEscape()
        {
            if (!IsInPrison) return false;

            float escapeChance = 0.05f; // %5 şans
            bool success = Random.value <= escapeChance;

            if (success)
            {
                _remainingPrisonYears = 0;
                EventBus.Publish(new PrisonEscapedEvent());
                return true;
            }
            else
            {
                // Başarısız kaçış, ceza artar
                _remainingPrisonYears += 3;
                return false;
            }
        }

        #endregion

        #region Default Crimes

        private List<CrimeDefinition> GetDefaultCrimes()
        {
            return new List<CrimeDefinition>
            {
                new CrimeDefinition
                {
                    type = CrimeType.Pickpocket,
                    name = "Yankesicilik",
                    description = "Birinin cüzdanını çal.",
                    minAge = 10,
                    successChance = 0.7f,
                    minReward = 50,
                    maxReward = 500,
                    minPrisonYears = 0,
                    maxPrisonYears = 1,
                    notorietyGain = 1
                },
                new CrimeDefinition
                {
                    type = CrimeType.Shoplifting,
                    name = "Mağaza Hırsızlığı",
                    description = "Mağazadan bir şey çal.",
                    minAge = 8,
                    successChance = 0.6f,
                    minReward = 100,
                    maxReward = 1000,
                    minPrisonYears = 0,
                    maxPrisonYears = 1,
                    notorietyGain = 2
                },
                new CrimeDefinition
                {
                    type = CrimeType.Vandalism,
                    name = "Vandalizm",
                    description = "Bir şeyleri tahrip et.",
                    minAge = 8,
                    successChance = 0.8f,
                    minReward = 0,
                    maxReward = 0,
                    minPrisonYears = 0,
                    maxPrisonYears = 1,
                    notorietyGain = 3
                },
                new CrimeDefinition
                {
                    type = CrimeType.CarTheft,
                    name = "Araba Hırsızlığı",
                    description = "Bir araba çal.",
                    minAge = 16,
                    successChance = 0.4f,
                    minReward = 5000,
                    maxReward = 50000,
                    minPrisonYears = 1,
                    maxPrisonYears = 5,
                    notorietyGain = 10
                },
                new CrimeDefinition
                {
                    type = CrimeType.HomeInvasion,
                    name = "Ev Soygunu",
                    description = "Bir eve gir ve soy.",
                    minAge = 18,
                    successChance = 0.35f,
                    minReward = 10000,
                    maxReward = 100000,
                    minPrisonYears = 2,
                    maxPrisonYears = 10,
                    notorietyGain = 15
                },
                new CrimeDefinition
                {
                    type = CrimeType.Assault,
                    name = "Saldırı",
                    description = "Birini dövdü.",
                    minAge = 14,
                    successChance = 0.5f,
                    minReward = 0,
                    maxReward = 200,
                    minPrisonYears = 1,
                    maxPrisonYears = 5,
                    notorietyGain = 8
                },
                new CrimeDefinition
                {
                    type = CrimeType.Fraud,
                    name = "Dolandırıcılık",
                    description = "Birini dolandır.",
                    minAge = 18,
                    successChance = 0.45f,
                    minReward = 5000,
                    maxReward = 200000,
                    minPrisonYears = 1,
                    maxPrisonYears = 7,
                    notorietyGain = 12
                },
                new CrimeDefinition
                {
                    type = CrimeType.DrugDealing,
                    name = "Uyuşturucu Satışı",
                    description = "Uyuşturucu sat.",
                    minAge = 16,
                    successChance = 0.5f,
                    minReward = 2000,
                    maxReward = 50000,
                    minPrisonYears = 3,
                    maxPrisonYears = 15,
                    notorietyGain = 20
                },
                new CrimeDefinition
                {
                    type = CrimeType.BankRobbery,
                    name = "Banka Soygunu",
                    description = "Bir banka soy.",
                    minAge = 18,
                    successChance = 0.2f,
                    minReward = 100000,
                    maxReward = 1000000,
                    minPrisonYears = 5,
                    maxPrisonYears = 25,
                    notorietyGain = 50
                },
                new CrimeDefinition
                {
                    type = CrimeType.TaxEvasion,
                    name = "Vergi Kaçırma",
                    description = "Vergi ödeme.",
                    minAge = 18,
                    successChance = 0.6f,
                    minReward = 10000,
                    maxReward = 100000,
                    minPrisonYears = 1,
                    maxPrisonYears = 5,
                    notorietyGain = 5
                },
                new CrimeDefinition
                {
                    type = CrimeType.Murder,
                    name = "Cinayet",
                    description = "Birini öldür.",
                    minAge = 18,
                    successChance = 0.3f,
                    minReward = 0,
                    maxReward = 0,
                    minPrisonYears = 15,
                    maxPrisonYears = 100, // Ağırlaştırılmış müebbet
                    notorietyGain = 100
                }
            };
        }

        #endregion
    }

    #region Events

    public struct ArrestedEvent : IGameEvent
    {
        public CrimeType CrimeType;
        public int Sentence;
    }

    public struct PrisonReleasedEvent : IGameEvent { }

    public struct PrisonEscapedEvent : IGameEvent { }

    #endregion
}
