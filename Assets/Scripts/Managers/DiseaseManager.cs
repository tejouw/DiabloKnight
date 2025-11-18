using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Hastalık ve tedavi sistemi yöneticisi.
    /// Hastalık kontrolü, tedavi süreçleri ve sağlık yönetimi.
    /// </summary>
    public class DiseaseManager : Singleton<DiseaseManager>
    {
        // Yaşa göre hastalık risk çarpanları
        private readonly Dictionary<LifeStage, float> _diseaseRiskMultipliers = new Dictionary<LifeStage, float>
        {
            { LifeStage.Baby, 1.2f },
            { LifeStage.Child, 0.8f },
            { LifeStage.Teen, 0.7f },
            { LifeStage.YoungAdult, 0.6f },
            { LifeStage.Adult, 1.0f },
            { LifeStage.Senior, 1.8f }
        };

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[DiseaseManager] Initialized successfully.");
        }

        #region Disease Checks

        /// <summary>
        /// Yıllık hastalık kontrolü - Rastgele hastalık kapılabilir.
        /// </summary>
        public void ProcessYearlyDiseaseCheck(CharacterData character)
        {
            if (character?.healthData == null) return;

            float riskMultiplier = _diseaseRiskMultipliers.GetValueOrDefault(character.CurrentLifeStage, 1f);

            // Sağlık düşükse risk artar
            if (character.Stats.Health < 50)
            {
                riskMultiplier *= 1.5f;
            }

            // Her hastalık kategorisi için kontrol
            CheckForDisease(character, DiseaseCategory.Infection, 0.15f * riskMultiplier);
            CheckForDisease(character, DiseaseCategory.Mental, 0.05f * riskMultiplier);

            // Yaşa bağlı hastalıklar
            if (character.Age >= 40)
            {
                CheckForDisease(character, DiseaseCategory.Cardiovascular, 0.03f * riskMultiplier);
                CheckForDisease(character, DiseaseCategory.Chronic, 0.02f * riskMultiplier);
            }

            if (character.Age >= 50)
            {
                CheckForDisease(character, DiseaseCategory.Cancer, 0.01f * riskMultiplier);
            }

            // Mevcut hastalıkları ilerlet
            ProcessExistingDiseases(character);

            // Tedavileri ilerlet
            ProcessOngoingTreatments(character);
        }

        /// <summary>
        /// Belirli kategoride hastalık kontrolü.
        /// </summary>
        private void CheckForDisease(CharacterData character, DiseaseCategory category, float baseChance)
        {
            if (Random.value > baseChance) return;

            var availableDiseases = GetDiseasesForCategory(category, character.Age);

            // Zaten sahip olunan hastalıkları çıkar
            availableDiseases.RemoveAll(d => character.healthData.HasDisease(d));

            if (availableDiseases.Count == 0) return;

            // Rastgele hastalık seç
            var selectedDisease = availableDiseases[Random.Range(0, availableDiseases.Count)];
            character.healthData.AddDisease(selectedDisease, character.Age);

            // Sağlık düşüşü
            var info = DiseaseDefinitions.GetDiseaseInfo(selectedDisease);
            character.Stats.ModifyStat(StatType.Health, -info.HealthImpactPerYear);
            character.Stats.ModifyStat(StatType.Happiness, -5);

            Debug.Log($"[DiseaseManager] {character.FullName} hastalığa yakalandı: {info.Name}");
        }

        /// <summary>
        /// Kategoriye göre hastalıkları al.
        /// </summary>
        private List<DiseaseType> GetDiseasesForCategory(DiseaseCategory category, int age)
        {
            var diseases = new List<DiseaseType>();

            foreach (DiseaseType type in System.Enum.GetValues(typeof(DiseaseType)))
            {
                var info = DiseaseDefinitions.GetDiseaseInfo(type);
                if (info.Category == category)
                {
                    // Yaş kontrolü
                    if (age >= info.AgeRange[0] && age <= info.AgeRange[1])
                    {
                        diseases.Add(type);
                    }
                }
            }

            return diseases;
        }

        /// <summary>
        /// Mevcut hastalıkları işle (ilerleme, etki).
        /// </summary>
        private void ProcessExistingDiseases(CharacterData character)
        {
            if (character?.healthData == null) return;

            int totalHealthLoss = character.healthData.CalculateTotalHealthImpact();

            if (totalHealthLoss > 0)
            {
                character.Stats.ModifyStat(StatType.Health, -totalHealthLoss);
            }

            // Her hastalığı ilerlet
            foreach (var disease in character.healthData.currentDiseases)
            {
                disease.Progress();

                // Terminal evreye ulaştıysa ölüm riski
                if (disease.stage == DiseaseStage.Terminal)
                {
                    var info = DiseaseDefinitions.GetDiseaseInfo(disease.type);
                    if (info.CanBeFatal && Random.value < 0.3f)
                    {
                        character.Stats.SetStat(StatType.Health, 0);
                        Debug.Log($"[DiseaseManager] {character.FullName} {info.Name} hastalığından hayatını kaybetti.");
                    }
                }
            }
        }

        /// <summary>
        /// Devam eden tedavileri işle.
        /// </summary>
        private void ProcessOngoingTreatments(CharacterData character)
        {
            if (character?.healthData == null) return;

            var completedTreatments = new List<Treatment>();

            foreach (var treatment in character.healthData.ongoingTreatments)
            {
                if (treatment.isCompleted) continue;

                // Haftalık ilerleme (yıllık kontrolde 52 hafta)
                for (int week = 0; week < 52 && !treatment.isCompleted; week++)
                {
                    var result = treatment.AdvanceWeek();

                    if (result.IsCompleted)
                    {
                        completedTreatments.Add(treatment);

                        if (result.WasSuccessful)
                        {
                            // Hastalığı iyileştir veya kontrol altına al
                            HandleSuccessfulTreatment(character, treatment);
                        }
                        else
                        {
                            // Tedavi başarısız
                            HandleFailedTreatment(character, treatment);
                        }

                        EventBus.Publish(new TreatmentCompletedEvent
                        {
                            DiseaseType = treatment.targetDisease,
                            TreatmentType = treatment.treatmentType,
                            WasSuccessful = result.WasSuccessful
                        });
                    }
                }
            }

            // Tamamlanan tedavileri listeden çıkar
            foreach (var treatment in completedTreatments)
            {
                character.healthData.ongoingTreatments.Remove(treatment);
            }
        }

        private void HandleSuccessfulTreatment(CharacterData character, Treatment treatment)
        {
            var disease = character.healthData.GetDisease(treatment.targetDisease);
            if (disease == null) return;

            var diseaseInfo = DiseaseDefinitions.GetDiseaseInfo(treatment.targetDisease);

            if (diseaseInfo.IsChronic)
            {
                // Kronik hastalıklar kontrol altına alınır
                disease.isChronicManaged = true;
                character.Stats.ModifyStat(StatType.Health, 10);
                Debug.Log($"[DiseaseManager] {diseaseInfo.Name} kontrol altına alındı.");
            }
            else
            {
                // Akut hastalıklar iyileştirilir
                character.healthData.CureDisease(treatment.targetDisease, character.Age);
                character.Stats.ModifyStat(StatType.Health, 20);
                Debug.Log($"[DiseaseManager] {diseaseInfo.Name} hastalığı iyileştirildi.");
            }

            character.Stats.ModifyStat(StatType.Happiness, 15);
        }

        private void HandleFailedTreatment(CharacterData character, Treatment treatment)
        {
            var disease = character.healthData.GetDisease(treatment.targetDisease);
            if (disease == null) return;

            var diseaseInfo = DiseaseDefinitions.GetDiseaseInfo(treatment.targetDisease);

            // Hastalık kötüleşebilir
            if (Random.value < 0.3f && disease.stage < DiseaseStage.Terminal)
            {
                disease.stage++;
            }

            character.Stats.ModifyStat(StatType.Happiness, -10);
            Debug.Log($"[DiseaseManager] {diseaseInfo.Name} tedavisi başarısız oldu.");
        }

        #endregion

        #region Treatment Management

        /// <summary>
        /// Tedavi başlat.
        /// </summary>
        public bool StartTreatment(CharacterData character, DiseaseType diseaseType, TreatmentType treatmentType)
        {
            if (character?.healthData == null) return false;

            if (!character.healthData.HasDisease(diseaseType))
            {
                Debug.LogWarning($"[DiseaseManager] Character doesn't have disease: {diseaseType}");
                return false;
            }

            // Zaten aynı hastalık için tedavi var mı?
            if (character.healthData.ongoingTreatments.Exists(t => t.targetDisease == diseaseType))
            {
                Debug.LogWarning($"[DiseaseManager] Already has treatment for: {diseaseType}");
                return false;
            }

            var treatmentInfo = TreatmentDefinitions.GetTreatmentInfo(treatmentType);

            // Maliyet kontrolü
            decimal cost = treatmentInfo.BaseCost;

            // Sağlık sigortası varsa indirim
            if (character.healthData.hasHealthInsurance)
            {
                cost *= 0.2m; // %80 indirim
            }

            if (character.Finances.CurrentMoney < cost)
            {
                Debug.LogWarning($"[DiseaseManager] Not enough money for treatment: {treatmentType}");
                return false;
            }

            // Parayı düş
            character.Finances.ModifyMoney(-cost, $"{treatmentInfo.Name} tedavi masrafı");

            // Tedaviyi başlat
            character.healthData.StartTreatment(diseaseType, treatmentType, character.Age);

            Debug.Log($"[DiseaseManager] {character.FullName} tedaviye başladı: {treatmentInfo.Name}");
            return true;
        }

        /// <summary>
        /// Önerilen tedavileri al.
        /// </summary>
        public List<TreatmentOption> GetTreatmentOptions(CharacterData character, DiseaseType diseaseType)
        {
            var options = new List<TreatmentOption>();

            var recommendedTreatments = TreatmentDefinitions.GetRecommendedTreatments(diseaseType);

            foreach (var treatmentType in recommendedTreatments)
            {
                var info = TreatmentDefinitions.GetTreatmentInfo(treatmentType);
                decimal cost = info.BaseCost;

                if (character.healthData.hasHealthInsurance)
                {
                    cost *= 0.2m;
                }

                options.Add(new TreatmentOption
                {
                    TreatmentType = treatmentType,
                    Name = info.Name,
                    Description = info.Description,
                    Cost = cost,
                    SuccessRate = info.BaseSuccessRate,
                    DurationWeeks = info.DurationWeeks,
                    CanAfford = character.Finances.CurrentMoney >= cost
                });
            }

            return options;
        }

        #endregion

        #region Health Services

        /// <summary>
        /// Sağlık kontrolü yap (check-up).
        /// </summary>
        public HealthCheckupResult DoHealthCheckup(CharacterData character)
        {
            if (character?.healthData == null)
            {
                return new HealthCheckupResult();
            }

            decimal cost = 500;

            if (character.healthData.hasHealthInsurance)
            {
                cost *= 0.1m;
            }

            if (character.Finances.CurrentMoney < cost)
            {
                return new HealthCheckupResult { Success = false, Message = "Yeterli paranız yok." };
            }

            character.Finances.ModifyMoney(-cost, "Sağlık kontrolü");

            var result = new HealthCheckupResult
            {
                Success = true,
                Cost = cost,
                DiseasesFound = new List<DiseaseType>()
            };

            // Gizli hastalıkları keşfet
            int diseasesFound = 0;

            // Rastgele hastalık kontrolü
            foreach (DiseaseCategory category in System.Enum.GetValues(typeof(DiseaseCategory)))
            {
                if (category == DiseaseCategory.Injury) continue;

                float checkChance = 0.05f;
                if (character.Age >= 40) checkChance = 0.08f;
                if (character.Age >= 60) checkChance = 0.12f;

                if (Random.value < checkChance)
                {
                    var diseases = GetDiseasesForCategory(category, character.Age);
                    diseases.RemoveAll(d => character.healthData.HasDisease(d));

                    if (diseases.Count > 0)
                    {
                        var disease = diseases[Random.Range(0, diseases.Count)];
                        character.healthData.AddDisease(disease, character.Age);
                        result.DiseasesFound.Add(disease);
                        diseasesFound++;
                    }
                }
            }

            // Sağlık artışı (erken teşhis)
            if (diseasesFound > 0)
            {
                result.Message = $"{diseasesFound} hastalık erken teşhis edildi.";
            }
            else
            {
                character.Stats.ModifyStat(StatType.Health, 5);
                result.Message = "Sağlık durumunuz iyi görünüyor.";
            }

            EventBus.Publish(new HealthCheckupEvent
            {
                DiseasesFound = diseasesFound,
                Cost = cost
            });

            character.healthData.totalMedicalExpenses += cost;

            return result;
        }

        /// <summary>
        /// Sağlık sigortası satın al.
        /// </summary>
        public bool PurchaseHealthInsurance(CharacterData character)
        {
            if (character?.healthData == null) return false;

            if (character.healthData.hasHealthInsurance)
            {
                Debug.LogWarning("[DiseaseManager] Already has health insurance.");
                return false;
            }

            decimal yearlyPremium = CalculateInsurancePremium(character);

            if (character.Finances.CurrentMoney < yearlyPremium)
            {
                Debug.LogWarning("[DiseaseManager] Not enough money for insurance.");
                return false;
            }

            character.Finances.ModifyMoney(-yearlyPremium, "Sağlık sigortası primi");
            character.healthData.hasHealthInsurance = true;

            Debug.Log($"[DiseaseManager] {character.FullName} sağlık sigortası satın aldı.");
            return true;
        }

        /// <summary>
        /// Sigorta primini hesapla.
        /// </summary>
        public decimal CalculateInsurancePremium(CharacterData character)
        {
            decimal basePremium = 2000;

            // Yaşa göre prim
            if (character.Age >= 50) basePremium *= 1.5m;
            if (character.Age >= 65) basePremium *= 2m;

            // Mevcut hastalıklara göre prim
            basePremium += character.healthData.currentDiseases.Count * 500;

            return basePremium;
        }

        /// <summary>
        /// Yıllık sigorta primi öde.
        /// </summary>
        public void ProcessYearlyInsurance(CharacterData character)
        {
            if (character?.healthData == null) return;

            if (!character.healthData.hasHealthInsurance) return;

            decimal premium = CalculateInsurancePremium(character);

            if (character.Finances.CurrentMoney < premium)
            {
                character.healthData.hasHealthInsurance = false;
                Debug.Log("[DiseaseManager] Sağlık sigortası iptal edildi (ödeme yapılamadı).");
                return;
            }

            character.Finances.ModifyMoney(-premium, "Yıllık sigorta primi");
        }

        #endregion

        #region Emergency & Accidents

        /// <summary>
        /// Kaza yaralanması.
        /// </summary>
        public void ProcessAccident(CharacterData character, AccidentType accidentType)
        {
            if (character?.healthData == null) return;

            DiseaseType injury;
            int healthLoss;
            decimal cost;

            switch (accidentType)
            {
                case AccidentType.MinorFall:
                    healthLoss = Random.Range(5, 15);
                    cost = 200;
                    injury = DiseaseType.BrokenBone;
                    if (Random.value > 0.3f) injury = default; // Çoğu zaman kırık olmaz
                    break;

                case AccidentType.CarAccident:
                    healthLoss = Random.Range(20, 50);
                    cost = 5000;
                    injury = Random.value > 0.5f ? DiseaseType.BrokenBone : DiseaseType.Concussion;
                    break;

                case AccidentType.SportInjury:
                    healthLoss = Random.Range(10, 25);
                    cost = 1000;
                    injury = DiseaseType.BrokenBone;
                    break;

                case AccidentType.BurnAccident:
                    healthLoss = Random.Range(15, 35);
                    cost = 3000;
                    injury = DiseaseType.BurnInjury;
                    break;

                default:
                    healthLoss = Random.Range(5, 20);
                    cost = 500;
                    injury = default;
                    break;
            }

            character.Stats.ModifyStat(StatType.Health, -healthLoss);
            character.Stats.ModifyStat(StatType.Happiness, -10);

            if (injury != default && !character.healthData.HasDisease(injury))
            {
                character.healthData.AddDisease(injury, character.Age);
            }

            // Hastane masrafı
            if (character.healthData.hasHealthInsurance)
            {
                cost *= 0.2m;
            }

            character.Finances.ModifyMoney(-cost, "Acil sağlık hizmeti");
            character.healthData.totalMedicalExpenses += cost;

            Debug.Log($"[DiseaseManager] {character.FullName} kaza geçirdi. Sağlık: -{healthLoss}");
        }

        /// <summary>
        /// Acil durum - ölümcül hastalık kontrolü.
        /// </summary>
        public bool CheckForFatalOutcome(CharacterData character)
        {
            if (character?.healthData == null) return false;

            // Tedavi edilmeyen terminal hastalıklar
            foreach (var disease in character.healthData.currentDiseases)
            {
                if (disease.stage == DiseaseStage.Terminal)
                {
                    var info = DiseaseDefinitions.GetDiseaseInfo(disease.type);
                    if (info.CanBeFatal)
                    {
                        // Tedavi var mı kontrol et
                        bool hasTreatment = character.healthData.ongoingTreatments.Exists(
                            t => t.targetDisease == disease.type);

                        if (!hasTreatment)
                        {
                            float deathChance = 0.5f + (disease.yearsWithDisease * 0.1f);
                            if (Random.value < deathChance)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Sağlık istatistiklerini al.
        /// </summary>
        public HealthStatistics GetHealthStatistics(CharacterData character)
        {
            if (character?.healthData == null)
            {
                return new HealthStatistics();
            }

            return new HealthStatistics
            {
                CurrentDiseaseCount = character.healthData.currentDiseases.Count,
                CuredDiseaseCount = character.healthData.curedDiseases.Count,
                OngoingTreatmentCount = character.healthData.ongoingTreatments.Count,
                HasChronicConditions = character.healthData.HasChronicDiseases(),
                HasFatalConditions = character.healthData.HasFatalDiseases(),
                TotalMedicalExpenses = character.healthData.totalMedicalExpenses,
                HasInsurance = character.healthData.hasHealthInsurance,
                YearlyHealthImpact = character.healthData.CalculateTotalHealthImpact()
            };
        }

        #endregion
    }

    #region Supporting Types

    /// <summary>
    /// Tedavi seçeneği.
    /// </summary>
    public struct TreatmentOption
    {
        public TreatmentType TreatmentType;
        public string Name;
        public string Description;
        public decimal Cost;
        public float SuccessRate;
        public int DurationWeeks;
        public bool CanAfford;
    }

    /// <summary>
    /// Sağlık kontrolü sonucu.
    /// </summary>
    public struct HealthCheckupResult
    {
        public bool Success;
        public decimal Cost;
        public List<DiseaseType> DiseasesFound;
        public string Message;
    }

    /// <summary>
    /// Sağlık istatistikleri.
    /// </summary>
    public struct HealthStatistics
    {
        public int CurrentDiseaseCount;
        public int CuredDiseaseCount;
        public int OngoingTreatmentCount;
        public bool HasChronicConditions;
        public bool HasFatalConditions;
        public decimal TotalMedicalExpenses;
        public bool HasInsurance;
        public int YearlyHealthImpact;
    }

    /// <summary>
    /// Kaza türleri.
    /// </summary>
    public enum AccidentType
    {
        MinorFall,
        CarAccident,
        SportInjury,
        BurnAccident,
        WorkAccident
    }

    #endregion
}
