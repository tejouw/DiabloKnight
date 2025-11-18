using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Health
{
    /// <summary>
    /// Hastalık türü.
    /// </summary>
    public enum DiseaseType
    {
        // Hafif
        Cold,               // Soğuk algınlığı
        Flu,                // Grip
        Headache,           // Baş ağrısı
        Stomachache,        // Mide ağrısı
        Allergy,            // Alerji

        // Orta
        FoodPoisoning,      // Gıda zehirlenmesi
        Bronchitis,         // Bronşit
        Infection,          // Enfeksiyon
        Anxiety,            // Anksiyete
        Depression,         // Depresyon

        // Ciddi
        Diabetes,           // Şeker hastalığı
        Hypertension,       // Hipertansiyon
        HeartDisease,       // Kalp hastalığı
        Cancer,             // Kanser
        Stroke              // İnme
    }

    /// <summary>
    /// Hastalık şiddeti.
    /// </summary>
    public enum DiseaseSeverity
    {
        Mild,       // Hafif
        Moderate,   // Orta
        Severe,     // Ciddi
        Critical    // Kritik
    }

    /// <summary>
    /// Hastalık tanımı.
    /// </summary>
    [System.Serializable]
    public class DiseaseDefinition
    {
        public DiseaseType type;
        public string name;
        public string description;
        public DiseaseSeverity severity;
        public int healthDamagePerYear;
        public int happinessDamage;
        public int treatmentCost;
        public float cureChance;          // Tedavi ile iyileşme şansı
        public float naturalCureChance;   // Doğal iyileşme şansı
        public bool isChronic;            // Kronik mi?
        public int minAge;                // Minimum yaş
    }

    /// <summary>
    /// Aktif hastalık.
    /// </summary>
    [System.Serializable]
    public class ActiveDisease
    {
        public DiseaseType type;
        public string name;
        public int yearDiagnosed;
        public bool isTreated;
    }

    /// <summary>
    /// Sağlık Yöneticisi.
    /// </summary>
    public class HealthManager : Singleton<HealthManager>
    {
        private List<DiseaseDefinition> _allDiseases;
        private Dictionary<DiseaseType, DiseaseDefinition> _diseaseLookup;

        // Aktif hastalıklar
        private List<ActiveDisease> _activeDiseases = new List<ActiveDisease>();

        #region Properties

        public List<ActiveDisease> ActiveDiseases => _activeDiseases;
        public int DiseaseCount => _activeDiseases.Count;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            LoadDiseases();

            // Her yıl hastalık etkilerini uygula
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);
        }

        private void LoadDiseases()
        {
            _allDiseases = GetDefaultDiseases();
            _diseaseLookup = new Dictionary<DiseaseType, DiseaseDefinition>();

            foreach (var disease in _allDiseases)
            {
                _diseaseLookup[disease.type] = disease;
            }

            Debug.Log($"[HealthManager] Loaded {_allDiseases.Count} diseases.");
        }

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return;

            // Rastgele hastalık şansı
            CheckForRandomDisease(character);

            // Mevcut hastalık etkilerini uygula
            ApplyDiseaseEffects(character);

            // Doğal iyileşme kontrolü
            CheckNaturalHealing();
        }

        #endregion

        #region Disease System

        /// <summary>
        /// Rastgele hastalık kontrolü.
        /// </summary>
        private void CheckForRandomDisease(CharacterData character)
        {
            // Yaşa göre hastalık şansı artar
            float diseaseChance = 0.05f + (character.Age / 500f);

            // Sağlık düşükse şans artar
            if (character.Stats.Health < 50)
            {
                diseaseChance += 0.1f;
            }

            if (Random.value <= diseaseChance)
            {
                // Rastgele bir hastalık seç
                var eligibleDiseases = _allDiseases
                    .Where(d => d.minAge <= character.Age && !HasDisease(d.type))
                    .ToList();

                if (eligibleDiseases.Count > 0)
                {
                    var disease = eligibleDiseases[Random.Range(0, eligibleDiseases.Count)];
                    ContractDisease(disease.type, character.Age);
                }
            }
        }

        /// <summary>
        /// Hastalık kap.
        /// </summary>
        public void ContractDisease(DiseaseType type, int year)
        {
            if (HasDisease(type)) return;

            if (!_diseaseLookup.TryGetValue(type, out var disease)) return;

            var activeDisease = new ActiveDisease
            {
                type = type,
                name = disease.name,
                yearDiagnosed = year,
                isTreated = false
            };

            _activeDiseases.Add(activeDisease);

            var character = GameManager.Instance?.CurrentCharacter;
            if (character != null)
            {
                character.Stats.ModifyStat(StatType.Happiness, -disease.happinessDamage);
            }

            EventBus.Publish(new DiseaseContractedEvent
            {
                DiseaseType = type,
                DiseaseName = disease.name
            });

            Debug.Log($"[HealthManager] Contracted disease: {disease.name}");
        }

        /// <summary>
        /// Hastalık etkilerini uygula.
        /// </summary>
        private void ApplyDiseaseEffects(CharacterData character)
        {
            foreach (var activeDisease in _activeDiseases)
            {
                if (!_diseaseLookup.TryGetValue(activeDisease.type, out var disease)) continue;

                // Tedavi edilmediyse hasar uygula
                if (!activeDisease.isTreated)
                {
                    character.Stats.ModifyStat(StatType.Health, -disease.healthDamagePerYear);
                }
                else
                {
                    // Tedavi edilse bile kronik hastalıklar küçük hasar verir
                    if (disease.isChronic)
                    {
                        character.Stats.ModifyStat(StatType.Health, -disease.healthDamagePerYear / 3);
                    }
                }
            }
        }

        /// <summary>
        /// Doğal iyileşme kontrolü.
        /// </summary>
        private void CheckNaturalHealing()
        {
            var toRemove = new List<ActiveDisease>();

            foreach (var activeDisease in _activeDiseases)
            {
                if (!_diseaseLookup.TryGetValue(activeDisease.type, out var disease)) continue;

                // Kronik hastalıklar doğal iyileşmez
                if (disease.isChronic) continue;

                if (Random.value <= disease.naturalCureChance)
                {
                    toRemove.Add(activeDisease);

                    EventBus.Publish(new DiseaseCuredEvent
                    {
                        DiseaseType = activeDisease.type,
                        DiseaseName = disease.name
                    });
                }
            }

            foreach (var disease in toRemove)
            {
                _activeDiseases.Remove(disease);
            }
        }

        /// <summary>
        /// Hastalık var mı?
        /// </summary>
        public bool HasDisease(DiseaseType type)
        {
            return _activeDiseases.Any(d => d.type == type);
        }

        #endregion

        #region Medical Actions

        /// <summary>
        /// Doktora git.
        /// </summary>
        public string VisitDoctor(CharacterData character)
        {
            int cost = 500;

            if (character.Finances.CurrentMoney < cost)
            {
                return "Doktor için yeterli paran yok!";
            }

            character.Finances.ModifyMoney(-cost, "Doktor muayenesi");

            if (_activeDiseases.Count == 0)
            {
                character.Stats.ModifyStat(StatType.Health, 5);
                return "Sağlık kontrolünden geçtin. Her şey yolunda!";
            }

            // Hastalıkları tespit et
            string result = "Teşhis edilen hastalıklar:\n";
            foreach (var disease in _activeDiseases)
            {
                result += $"- {disease.name}\n";
            }

            return result;
        }

        /// <summary>
        /// Hastalığı tedavi et.
        /// </summary>
        public string TreatDisease(CharacterData character, DiseaseType type)
        {
            var activeDisease = _activeDiseases.FirstOrDefault(d => d.type == type);
            if (activeDisease == null)
            {
                return "Bu hastalığın yok!";
            }

            if (!_diseaseLookup.TryGetValue(type, out var disease))
            {
                return "Hastalık tanımı bulunamadı!";
            }

            if (character.Finances.CurrentMoney < disease.treatmentCost)
            {
                return $"Tedavi için yeterli paran yok! ({disease.treatmentCost:N0} TL gerekli)";
            }

            character.Finances.ModifyMoney(-disease.treatmentCost, $"{disease.name} tedavisi");

            // Tedavi başarı kontrolü
            if (Random.value <= disease.cureChance)
            {
                if (disease.isChronic)
                {
                    // Kronik hastalıklar kontrol altına alınır
                    activeDisease.isTreated = true;
                    character.Stats.ModifyStat(StatType.Happiness, 5);
                    return $"{disease.name} kontrol altına alındı.";
                }
                else
                {
                    // Hastalık tamamen iyileşti
                    _activeDiseases.Remove(activeDisease);
                    character.Stats.ModifyStat(StatType.Health, 10);
                    character.Stats.ModifyStat(StatType.Happiness, 10);

                    EventBus.Publish(new DiseaseCuredEvent
                    {
                        DiseaseType = type,
                        DiseaseName = disease.name
                    });

                    return $"{disease.name} tamamen iyileşti!";
                }
            }
            else
            {
                return $"Tedavi başarısız oldu. {disease.name} devam ediyor.";
            }
        }

        /// <summary>
        /// Terapiye git.
        /// </summary>
        public string VisitTherapist(CharacterData character)
        {
            int cost = 1000;

            if (character.Finances.CurrentMoney < cost)
            {
                return "Terapi için yeterli paran yok!";
            }

            character.Finances.ModifyMoney(-cost, "Terapi seansı");

            character.Stats.ModifyStat(StatType.Happiness, 10);

            // Mental hastalıkları iyileştirme şansı
            var mentalDiseases = _activeDiseases
                .Where(d => d.type == DiseaseType.Anxiety || d.type == DiseaseType.Depression)
                .ToList();

            foreach (var disease in mentalDiseases)
            {
                if (Random.value < 0.3f)
                {
                    _activeDiseases.Remove(disease);
                    return $"Terapi işe yaradı! {disease.name} iyileşti.";
                }
            }

            return "Terapi seansı tamamlandı. Kendini daha iyi hissediyorsun.";
        }

        /// <summary>
        /// Diyet yap.
        /// </summary>
        public string GoDiet(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Health, 3);
            character.Stats.ModifyStat(StatType.Appearance, 2);
            character.Stats.ModifyStat(StatType.Happiness, -2);

            return "Diyet yapıyorsun. Sağlık ve görünüşün iyileşti ama biraz mutsuz oldun.";
        }

        #endregion

        #region Default Diseases

        private List<DiseaseDefinition> GetDefaultDiseases()
        {
            return new List<DiseaseDefinition>
            {
                // Hafif
                new DiseaseDefinition
                {
                    type = DiseaseType.Cold,
                    name = "Soğuk Algınlığı",
                    description = "Hafif üst solunum yolu enfeksiyonu.",
                    severity = DiseaseSeverity.Mild,
                    healthDamagePerYear = 2,
                    happinessDamage = 3,
                    treatmentCost = 100,
                    cureChance = 0.95f,
                    naturalCureChance = 0.8f,
                    isChronic = false,
                    minAge = 0
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Flu,
                    name = "Grip",
                    description = "Viral enfeksiyon.",
                    severity = DiseaseSeverity.Mild,
                    healthDamagePerYear = 5,
                    happinessDamage = 5,
                    treatmentCost = 200,
                    cureChance = 0.9f,
                    naturalCureChance = 0.6f,
                    isChronic = false,
                    minAge = 0
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Headache,
                    name = "Kronik Baş Ağrısı",
                    description = "Sürekli baş ağrısı.",
                    severity = DiseaseSeverity.Mild,
                    healthDamagePerYear = 1,
                    happinessDamage = 5,
                    treatmentCost = 500,
                    cureChance = 0.7f,
                    naturalCureChance = 0.4f,
                    isChronic = false,
                    minAge = 10
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Allergy,
                    name = "Alerji",
                    description = "Alerjik reaksiyon.",
                    severity = DiseaseSeverity.Mild,
                    healthDamagePerYear = 2,
                    happinessDamage = 3,
                    treatmentCost = 300,
                    cureChance = 0.5f,
                    naturalCureChance = 0.1f,
                    isChronic = true,
                    minAge = 5
                },

                // Orta
                new DiseaseDefinition
                {
                    type = DiseaseType.FoodPoisoning,
                    name = "Gıda Zehirlenmesi",
                    description = "Bozuk yiyecekten zehirlenme.",
                    severity = DiseaseSeverity.Moderate,
                    healthDamagePerYear = 10,
                    happinessDamage = 10,
                    treatmentCost = 1000,
                    cureChance = 0.95f,
                    naturalCureChance = 0.5f,
                    isChronic = false,
                    minAge = 0
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Anxiety,
                    name = "Anksiyete",
                    description = "Sürekli kaygı hali.",
                    severity = DiseaseSeverity.Moderate,
                    healthDamagePerYear = 3,
                    happinessDamage = 15,
                    treatmentCost = 3000,
                    cureChance = 0.6f,
                    naturalCureChance = 0.1f,
                    isChronic = true,
                    minAge = 12
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Depression,
                    name = "Depresyon",
                    description = "Klinik depresyon.",
                    severity = DiseaseSeverity.Moderate,
                    healthDamagePerYear = 5,
                    happinessDamage = 25,
                    treatmentCost = 5000,
                    cureChance = 0.5f,
                    naturalCureChance = 0.05f,
                    isChronic = true,
                    minAge = 12
                },

                // Ciddi
                new DiseaseDefinition
                {
                    type = DiseaseType.Diabetes,
                    name = "Diyabet",
                    description = "Şeker hastalığı.",
                    severity = DiseaseSeverity.Severe,
                    healthDamagePerYear = 8,
                    happinessDamage = 10,
                    treatmentCost = 10000,
                    cureChance = 0.3f,
                    naturalCureChance = 0f,
                    isChronic = true,
                    minAge = 30
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Hypertension,
                    name = "Hipertansiyon",
                    description = "Yüksek tansiyon.",
                    severity = DiseaseSeverity.Severe,
                    healthDamagePerYear = 7,
                    happinessDamage = 8,
                    treatmentCost = 8000,
                    cureChance = 0.4f,
                    naturalCureChance = 0f,
                    isChronic = true,
                    minAge = 35
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.HeartDisease,
                    name = "Kalp Hastalığı",
                    description = "Kardiyovasküler hastalık.",
                    severity = DiseaseSeverity.Severe,
                    healthDamagePerYear = 15,
                    happinessDamage = 20,
                    treatmentCost = 50000,
                    cureChance = 0.3f,
                    naturalCureChance = 0f,
                    isChronic = true,
                    minAge = 40
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Cancer,
                    name = "Kanser",
                    description = "Kötü huylu tümör.",
                    severity = DiseaseSeverity.Critical,
                    healthDamagePerYear = 25,
                    happinessDamage = 30,
                    treatmentCost = 200000,
                    cureChance = 0.4f,
                    naturalCureChance = 0.01f,
                    isChronic = false,
                    minAge = 20
                },
                new DiseaseDefinition
                {
                    type = DiseaseType.Stroke,
                    name = "İnme",
                    description = "Beyin felci.",
                    severity = DiseaseSeverity.Critical,
                    healthDamagePerYear = 30,
                    happinessDamage = 25,
                    treatmentCost = 100000,
                    cureChance = 0.2f,
                    naturalCureChance = 0f,
                    isChronic = true,
                    minAge = 50
                }
            };
        }

        #endregion
    }

    #region Events

    public struct DiseaseContractedEvent : IGameEvent
    {
        public DiseaseType DiseaseType;
        public string DiseaseName;
    }

    public struct DiseaseCuredEvent : IGameEvent
    {
        public DiseaseType DiseaseType;
        public string DiseaseName;
    }

    #endregion
}
