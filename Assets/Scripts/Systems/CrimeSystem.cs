using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Suç ve Hukuk Sistemi - Suç işleme, tutuklanma, mahkeme, hapishane.
    /// </summary>
    public class CrimeSystem : Singleton<CrimeSystem>
    {
        private List<CrimeDefinition> _crimes = new List<CrimeDefinition>();

        // Karakter suç geçmişi (CharacterData'ya eklenebilir)
        public int PrisonYearsRemaining { get; private set; }
        public bool IsInPrison => PrisonYearsRemaining > 0;
        public List<string> CriminalRecord { get; private set; } = new List<string>();

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeCrimes();
        }

        private void InitializeCrimes()
        {
            _crimes = new List<CrimeDefinition>
            {
                // Hafif Suçlar
                new CrimeDefinition("shoplifting", "Hırsızlık (Dükkan)", CrimeSeverity.Minor, 0.6f, 0, 1, 500, 5000),
                new CrimeDefinition("vandalism", "Vandalizm", CrimeSeverity.Minor, 0.5f, 0, 1, 1000, 10000),
                new CrimeDefinition("pickpocket", "Yankesicilik", CrimeSeverity.Minor, 0.55f, 50, 500, 500, 8000),
                new CrimeDefinition("trespassing", "İzinsiz Giriş", CrimeSeverity.Minor, 0.4f, 0, 0, 200, 3000),
                new CrimeDefinition("public_intoxication", "Sarhoşluk", CrimeSeverity.Minor, 0.3f, 0, 0, 500, 2000),

                // Orta Suçlar
                new CrimeDefinition("burglary", "Ev Soygunu", CrimeSeverity.Moderate, 0.5f, 1000, 10000, 5000, 50000),
                new CrimeDefinition("car_theft", "Araba Hırsızlığı", CrimeSeverity.Moderate, 0.55f, 5000, 30000, 10000, 100000),
                new CrimeDefinition("assault", "Darp", CrimeSeverity.Moderate, 0.45f, 0, 0, 5000, 50000),
                new CrimeDefinition("drug_dealing", "Uyuşturucu Satışı", CrimeSeverity.Moderate, 0.5f, 2000, 20000, 10000, 80000),
                new CrimeDefinition("fraud", "Dolandırıcılık", CrimeSeverity.Moderate, 0.6f, 5000, 50000, 20000, 200000),
                new CrimeDefinition("blackmail", "Şantaj", CrimeSeverity.Moderate, 0.55f, 3000, 30000, 10000, 100000),

                // Ağır Suçlar
                new CrimeDefinition("bank_robbery", "Banka Soygunu", CrimeSeverity.Severe, 0.4f, 50000, 500000, 100000, 1000000),
                new CrimeDefinition("kidnapping", "Adam Kaçırma", CrimeSeverity.Severe, 0.35f, 10000, 100000, 50000, 500000),
                new CrimeDefinition("arson", "Kundakçılık", CrimeSeverity.Severe, 0.4f, 0, 0, 50000, 300000),
                new CrimeDefinition("murder", "Cinayet", CrimeSeverity.Severe, 0.3f, 0, 0, 0, 0), // Ömür boyu hapis riski
            };

            Debug.Log($"[CrimeSystem] Loaded {_crimes.Count} crime types.");
        }

        #endregion

        #region Crime Execution

        /// <summary>
        /// Suç işle.
        /// </summary>
        public CrimeResult CommitCrime(CharacterData character, string crimeId)
        {
            if (IsInPrison)
            {
                return new CrimeResult
                {
                    success = false,
                    message = "Hapishaneden çıkmadan suç işleyemezsin!"
                };
            }

            var crime = _crimes.Find(c => c.id == crimeId);
            if (crime == null)
            {
                return new CrimeResult { success = false, message = "Suç bulunamadı." };
            }

            if (character.Age < 16)
            {
                return new CrimeResult { success = false, message = "Çok küçüksün!" };
            }

            // Başarı şansı
            float successChance = crime.successRate;
            successChance += (character.Stats.Intelligence - 50) * 0.003f;

            // Yakalanma şansı
            bool caught = Random.value > successChance;

            if (caught)
            {
                return ProcessArrest(character, crime);
            }
            else
            {
                // Başarılı suç
                decimal loot = Random.Range((float)crime.minLoot, (float)crime.maxLoot);
                if (loot > 0)
                {
                    character.Finances.ModifyMoney(loot, $"{crime.name} geliri");
                }

                // Suç geçmişine ekle
                CriminalRecord.Add(crime.name);

                return new CrimeResult
                {
                    success = true,
                    message = loot > 0
                        ? $"{crime.name} başarılı! {loot:N0} TL kazandın."
                        : $"{crime.name} başarılı!"
                };
            }
        }

        /// <summary>
        /// Tutuklanma işlemi.
        /// </summary>
        private CrimeResult ProcessArrest(CharacterData character, CrimeDefinition crime)
        {
            character.Stats.ModifyStat(StatType.Happiness, -20);

            // Mahkeme süreci
            bool convicted = Random.value < 0.7f; // %70 mahkumiyet şansı

            if (convicted)
            {
                // Ceza hesapla
                int sentence = crime.severity switch
                {
                    CrimeSeverity.Minor => Random.Range(0, 2),
                    CrimeSeverity.Moderate => Random.Range(1, 5),
                    CrimeSeverity.Severe => Random.Range(5, 25),
                    _ => 1
                };

                // Cinayet için özel
                if (crime.id == "murder")
                {
                    sentence = Random.Range(15, 40);
                }

                // Para cezası
                decimal fine = Random.Range((float)crime.minFine, (float)crime.maxFine);
                if (fine > 0)
                {
                    character.Finances.ModifyMoney(-fine, $"{crime.name} para cezası");
                }

                if (sentence > 0)
                {
                    PrisonYearsRemaining = sentence;
                    character.isEmployed = false;
                    character.Career.currentJob = null;

                    CriminalRecord.Add($"{crime.name} - {sentence} yıl hapis");

                    EventBus.Publish(new CrimeEvent
                    {
                        CrimeName = crime.name,
                        EventType = CrimeEventType.Imprisoned,
                        Sentence = sentence
                    });

                    return new CrimeResult
                    {
                        success = false,
                        caught = true,
                        message = $"Yakalandın! {crime.name} suçundan {sentence} yıl hapis cezası aldın."
                    };
                }
                else
                {
                    return new CrimeResult
                    {
                        success = false,
                        caught = true,
                        message = $"Yakalandın! {fine:N0} TL para cezası ödedin."
                    };
                }
            }
            else
            {
                // Beraat
                return new CrimeResult
                {
                    success = false,
                    caught = true,
                    message = "Yakalandın ama mahkemede beraat ettin!"
                };
            }
        }

        #endregion

        #region Prison

        /// <summary>
        /// Hapishanede yıl geçir.
        /// </summary>
        public void ProcessPrisonYear(CharacterData character)
        {
            if (!IsInPrison) return;

            PrisonYearsRemaining--;

            // Hapishane etkileri
            character.Stats.ModifyStat(StatType.Happiness, -10);
            character.Stats.ModifyStat(StatType.Health, -5);

            // Olaylar
            float eventChance = Random.value;
            if (eventChance < 0.1f)
            {
                // Kavga
                character.Stats.ModifyStat(StatType.Health, -15);
                UIManager.Instance?.ShowInfo("Hapishane", "Bir kavgaya karıştın ve yaralandın.");
            }
            else if (eventChance < 0.2f)
            {
                // Fitness
                character.Stats.ModifyStat(StatType.Health, 5);
                UIManager.Instance?.ShowInfo("Hapishane", "Spor yaparak formda kaldın.");
            }

            if (PrisonYearsRemaining == 0)
            {
                EventBus.Publish(new CrimeEvent
                {
                    CrimeName = "",
                    EventType = CrimeEventType.Released,
                    Sentence = 0
                });

                UIManager.Instance?.ShowInfo("Tahliye", "Cezanı tamamladın ve serbest bırakıldın!");
            }
        }

        /// <summary>
        /// Kaçış girişimi.
        /// </summary>
        public EscapeResult AttemptEscape(CharacterData character)
        {
            if (!IsInPrison)
            {
                return new EscapeResult { success = false, message = "Hapishanede değilsin." };
            }

            float escapeChance = 0.1f + (character.Stats.Intelligence * 0.002f);

            if (Random.value < escapeChance)
            {
                PrisonYearsRemaining = 0;
                character.Stats.ModifyStat(StatType.Happiness, 20);

                EventBus.Publish(new CrimeEvent
                {
                    CrimeName = "",
                    EventType = CrimeEventType.Escaped,
                    Sentence = 0
                });

                return new EscapeResult
                {
                    success = true,
                    message = "Başarılı bir şekilde kaçtın! Ama dikkatli ol, aranan birisin."
                };
            }
            else
            {
                // Ceza artışı
                int additionalYears = Random.Range(2, 5);
                PrisonYearsRemaining += additionalYears;
                character.Stats.ModifyStat(StatType.Health, -10);

                return new EscapeResult
                {
                    success = false,
                    message = $"Kaçış girişimin başarısız oldu! Cezana {additionalYears} yıl eklendi."
                };
            }
        }

        /// <summary>
        /// İyi hal indirimi için başvur.
        /// </summary>
        public bool ApplyForParole(CharacterData character)
        {
            if (!IsInPrison || PrisonYearsRemaining < 2)
                return false;

            float paroleChance = 0.3f;
            if (Random.value < paroleChance)
            {
                PrisonYearsRemaining = Mathf.Max(1, PrisonYearsRemaining / 2);
                return true;
            }

            return false;
        }

        #endregion

        #region Utilities

        public List<CrimeDefinition> GetAllCrimes()
        {
            return new List<CrimeDefinition>(_crimes);
        }

        public List<CrimeDefinition> GetCrimesBySeverity(CrimeSeverity severity)
        {
            return _crimes.FindAll(c => c.severity == severity);
        }

        public bool HasCriminalRecord()
        {
            return CriminalRecord.Count > 0;
        }

        public void ClearPrisonStatus()
        {
            PrisonYearsRemaining = 0;
        }

        #endregion
    }

    #region Data Classes

    [System.Serializable]
    public class CrimeDefinition
    {
        public string id;
        public string name;
        public CrimeSeverity severity;
        public float successRate;
        public decimal minLoot;
        public decimal maxLoot;
        public decimal minFine;
        public decimal maxFine;

        public CrimeDefinition(string id, string name, CrimeSeverity severity, float successRate,
            decimal minLoot, decimal maxLoot, decimal minFine, decimal maxFine)
        {
            this.id = id;
            this.name = name;
            this.severity = severity;
            this.successRate = successRate;
            this.minLoot = minLoot;
            this.maxLoot = maxLoot;
            this.minFine = minFine;
            this.maxFine = maxFine;
        }
    }

    public enum CrimeSeverity
    {
        Minor,
        Moderate,
        Severe
    }

    public class CrimeResult
    {
        public bool success;
        public bool caught;
        public string message;
    }

    public class EscapeResult
    {
        public bool success;
        public string message;
    }

    public enum CrimeEventType
    {
        Arrested,
        Imprisoned,
        Released,
        Escaped
    }

    public struct CrimeEvent : IGameEvent
    {
        public string CrimeName;
        public CrimeEventType EventType;
        public int Sentence;
    }

    #endregion
}
