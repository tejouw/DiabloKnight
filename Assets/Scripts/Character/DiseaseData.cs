using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Character
{
    /// <summary>
    /// Hastalık ve tedavi sistemi data modelleri.
    /// </summary>

    #region Disease System

    /// <summary>
    /// Karakter sağlık verisi - Hastalıklar ve tedavileri içerir.
    /// </summary>
    [System.Serializable]
    public class HealthData
    {
        public List<Disease> currentDiseases = new List<Disease>();
        public List<Disease> curedDiseases = new List<Disease>();
        public List<Treatment> ongoingTreatments = new List<Treatment>();
        public List<string> medicalHistory = new List<string>();
        public bool hasHealthInsurance = false;
        public decimal totalMedicalExpenses = 0;

        /// <summary>
        /// Hastalık ekle.
        /// </summary>
        public void AddDisease(DiseaseType type, int diagnosisAge)
        {
            if (HasDisease(type)) return;

            var diseaseInfo = DiseaseDefinitions.GetDiseaseInfo(type);
            var disease = new Disease
            {
                type = type,
                stage = DiseaseStage.Early,
                diagnosisAge = diagnosisAge,
                severity = diseaseInfo.BaseSeverity,
                isChronicManaged = false,
                treatmentProgress = 0
            };

            currentDiseases.Add(disease);
            medicalHistory.Add($"{diagnosisAge} yaşında {diseaseInfo.Name} teşhisi konuldu.");

            EventBus.Publish(new DiseaseContractedEvent
            {
                DiseaseType = type,
                Severity = disease.severity
            });
        }

        /// <summary>
        /// Hastalık var mı kontrol et.
        /// </summary>
        public bool HasDisease(DiseaseType type)
        {
            return currentDiseases.Exists(d => d.type == type);
        }

        /// <summary>
        /// Hastalık al.
        /// </summary>
        public Disease GetDisease(DiseaseType type)
        {
            return currentDiseases.Find(d => d.type == type);
        }

        /// <summary>
        /// Hastalığı iyileştir.
        /// </summary>
        public void CureDisease(DiseaseType type, int cureAge)
        {
            var disease = GetDisease(type);
            if (disease == null) return;

            currentDiseases.Remove(disease);
            curedDiseases.Add(disease);

            var diseaseInfo = DiseaseDefinitions.GetDiseaseInfo(type);
            medicalHistory.Add($"{cureAge} yaşında {diseaseInfo.Name} hastalığından kurtuldu.");

            EventBus.Publish(new DiseaseCuredEvent
            {
                DiseaseType = type
            });
        }

        /// <summary>
        /// Aktif tedavi başlat.
        /// </summary>
        public void StartTreatment(DiseaseType diseaseType, TreatmentType treatmentType, int startAge)
        {
            var treatmentInfo = TreatmentDefinitions.GetTreatmentInfo(treatmentType);
            var treatment = new Treatment
            {
                treatmentType = treatmentType,
                targetDisease = diseaseType,
                startAge = startAge,
                duration = treatmentInfo.DurationWeeks,
                weeksCompleted = 0,
                isCompleted = false,
                successRate = treatmentInfo.BaseSuccessRate,
                totalCost = treatmentInfo.BaseCost
            };

            ongoingTreatments.Add(treatment);
            totalMedicalExpenses += treatment.totalCost;

            EventBus.Publish(new TreatmentStartedEvent
            {
                DiseaseType = diseaseType,
                TreatmentType = treatmentType,
                Cost = treatment.totalCost
            });
        }

        /// <summary>
        /// Kronik hastalıklar var mı?
        /// </summary>
        public bool HasChronicDiseases()
        {
            foreach (var disease in currentDiseases)
            {
                var info = DiseaseDefinitions.GetDiseaseInfo(disease.type);
                if (info.IsChronic) return true;
            }
            return false;
        }

        /// <summary>
        /// Ölümcül hastalıklar var mı?
        /// </summary>
        public bool HasFatalDiseases()
        {
            foreach (var disease in currentDiseases)
            {
                var info = DiseaseDefinitions.GetDiseaseInfo(disease.type);
                if (info.CanBeFatal && disease.stage == DiseaseStage.Terminal) return true;
            }
            return false;
        }

        /// <summary>
        /// Toplam sağlık etkisini hesapla.
        /// </summary>
        public int CalculateTotalHealthImpact()
        {
            int totalImpact = 0;
            foreach (var disease in currentDiseases)
            {
                var info = DiseaseDefinitions.GetDiseaseInfo(disease.type);
                int impact = info.HealthImpactPerYear;

                // Hastalık evresine göre çarpan
                switch (disease.stage)
                {
                    case DiseaseStage.Early:
                        impact = Mathf.RoundToInt(impact * 0.5f);
                        break;
                    case DiseaseStage.Middle:
                        impact = Mathf.RoundToInt(impact * 1f);
                        break;
                    case DiseaseStage.Advanced:
                        impact = Mathf.RoundToInt(impact * 1.5f);
                        break;
                    case DiseaseStage.Terminal:
                        impact = Mathf.RoundToInt(impact * 2.5f);
                        break;
                }

                // Tedavi altındaysa azalt
                if (disease.isChronicManaged)
                {
                    impact = Mathf.RoundToInt(impact * 0.3f);
                }

                totalImpact += impact;
            }
            return totalImpact;
        }
    }

    /// <summary>
    /// Hastalık verisi.
    /// </summary>
    [System.Serializable]
    public class Disease
    {
        public DiseaseType type;
        public DiseaseStage stage = DiseaseStage.Early;
        public int diagnosisAge;
        [Range(1, 10)] public int severity = 5;
        public bool isChronicManaged = false;
        public int treatmentProgress = 0;  // 0-100
        public int yearsWithDisease = 0;

        /// <summary>
        /// Hastalığı ilerlet (yıllık).
        /// </summary>
        public void Progress()
        {
            yearsWithDisease++;
            var info = DiseaseDefinitions.GetDiseaseInfo(type);

            // Tedavi altında değilse hastalık ilerleyebilir
            if (!isChronicManaged && !info.IsChronic)
            {
                float progressChance = severity * 0.1f;
                if (UnityEngine.Random.value < progressChance)
                {
                    AdvanceStage();
                }
            }
        }

        /// <summary>
        /// Hastalık evresini ilerlet.
        /// </summary>
        private void AdvanceStage()
        {
            if (stage < DiseaseStage.Terminal)
            {
                var oldStage = stage;
                stage++;

                EventBus.Publish(new DiseaseProgressedEvent
                {
                    DiseaseType = type,
                    OldStage = oldStage,
                    NewStage = stage
                });
            }
        }
    }

    /// <summary>
    /// Tedavi verisi.
    /// </summary>
    [System.Serializable]
    public class Treatment
    {
        public TreatmentType treatmentType;
        public DiseaseType targetDisease;
        public int startAge;
        public int duration;  // Hafta
        public int weeksCompleted = 0;
        public bool isCompleted = false;
        [Range(0, 100)] public float successRate = 50f;
        public decimal totalCost;
        public List<string> sideEffects = new List<string>();

        /// <summary>
        /// Tedavi ilerleme yüzdesi.
        /// </summary>
        public float Progress => duration > 0 ? (float)weeksCompleted / duration * 100f : 0;

        /// <summary>
        /// Tedaviyi bir hafta ilerlet.
        /// </summary>
        public TreatmentResult AdvanceWeek()
        {
            weeksCompleted++;

            if (weeksCompleted >= duration)
            {
                isCompleted = true;
                bool success = UnityEngine.Random.value < (successRate / 100f);

                return new TreatmentResult
                {
                    TreatmentType = treatmentType,
                    TargetDisease = targetDisease,
                    IsCompleted = true,
                    WasSuccessful = success
                };
            }

            return new TreatmentResult
            {
                TreatmentType = treatmentType,
                TargetDisease = targetDisease,
                IsCompleted = false,
                WasSuccessful = false
            };
        }
    }

    /// <summary>
    /// Tedavi sonucu.
    /// </summary>
    public struct TreatmentResult
    {
        public TreatmentType TreatmentType;
        public DiseaseType TargetDisease;
        public bool IsCompleted;
        public bool WasSuccessful;
    }

    #endregion

    #region Enums

    /// <summary>
    /// Hastalık türleri.
    /// </summary>
    public enum DiseaseType
    {
        // Kanser Türleri
        LungCancer,         // Akciğer kanseri
        BreastCancer,       // Meme kanseri
        ColonCancer,        // Kolon kanseri
        ProstateCancer,     // Prostat kanseri
        SkinCancer,         // Cilt kanseri
        Leukemia,           // Lösemi
        BrainTumor,         // Beyin tümörü
        StomachCancer,      // Mide kanseri

        // Kalp Hastalıkları
        HeartDisease,       // Kalp hastalığı
        Hypertension,       // Yüksek tansiyon
        Arrhythmia,         // Aritmi
        HeartFailure,       // Kalp yetmezliği
        CoronaryArtery,     // Koroner arter hastalığı

        // Kronik Hastalıklar
        Diabetes,           // Diyabet
        Asthma,             // Astım
        Epilepsy,           // Epilepsi
        Arthritis,          // Artrit
        Alzheimer,          // Alzheimer
        Parkinson,          // Parkinson
        MultipleSclerosis,  // MS

        // Enfeksiyonlar
        Flu,                // Grip
        Pneumonia,          // Zatürre
        Tuberculosis,       // Verem
        Hepatitis,          // Hepatit
        HIV,                // HIV

        // Akıl Sağlığı
        Depression,         // Depresyon
        Anxiety,            // Anksiyete
        Bipolar,            // Bipolar bozukluk
        PTSD,               // TSSB

        // Diğer
        Kidney,             // Böbrek hastalığı
        Liver,              // Karaciğer hastalığı
        Thyroid,            // Tiroid hastalığı
        Anemia,             // Anemi
        Osteoporosis,       // Osteoporoz

        // Yaralanmalar
        BrokenBone,         // Kırık kemik
        Concussion,         // Beyin sarsıntısı
        BurnInjury          // Yanık
    }

    /// <summary>
    /// Hastalık evresi.
    /// </summary>
    public enum DiseaseStage
    {
        Early,      // Erken evre
        Middle,     // Orta evre
        Advanced,   // İleri evre
        Terminal    // Son evre
    }

    /// <summary>
    /// Tedavi türleri.
    /// </summary>
    public enum TreatmentType
    {
        // Genel
        Medication,         // İlaç tedavisi
        Surgery,            // Ameliyat
        PhysicalTherapy,    // Fizik tedavi
        Hospitalization,    // Hastanede yatış

        // Kanser Tedavileri
        Chemotherapy,       // Kemoterapi
        Radiation,          // Radyoterapi
        Immunotherapy,      // İmmünoterapi
        BoneMarrow,         // Kemik iliği nakli

        // Kalp Tedavileri
        Bypass,             // Bypass ameliyatı
        Angioplasty,        // Anjiyoplasti
        Pacemaker,          // Kalp pili
        HeartTransplant,    // Kalp nakli

        // Psikolojik
        Psychotherapy,      // Psikoterapi
        Counseling,         // Danışmanlık

        // Diğer
        Dialysis,           // Diyaliz
        Insulin,            // İnsülin tedavisi
        Rehabilitation,     // Rehabilitasyon
        AlternativeMedicine // Alternatif tıp
    }

    /// <summary>
    /// Hastalık kategorisi.
    /// </summary>
    public enum DiseaseCategory
    {
        Cancer,
        Cardiovascular,
        Chronic,
        Infection,
        Mental,
        Injury,
        Other
    }

    #endregion

    #region Events

    /// <summary>
    /// Hastalık kapıldığında tetiklenir.
    /// </summary>
    public struct DiseaseContractedEvent : IGameEvent
    {
        public DiseaseType DiseaseType;
        public int Severity;
    }

    /// <summary>
    /// Hastalık ilerlediğinde tetiklenir.
    /// </summary>
    public struct DiseaseProgressedEvent : IGameEvent
    {
        public DiseaseType DiseaseType;
        public DiseaseStage OldStage;
        public DiseaseStage NewStage;
    }

    /// <summary>
    /// Hastalık iyileştiğinde tetiklenir.
    /// </summary>
    public struct DiseaseCuredEvent : IGameEvent
    {
        public DiseaseType DiseaseType;
    }

    /// <summary>
    /// Tedavi başladığında tetiklenir.
    /// </summary>
    public struct TreatmentStartedEvent : IGameEvent
    {
        public DiseaseType DiseaseType;
        public TreatmentType TreatmentType;
        public decimal Cost;
    }

    /// <summary>
    /// Tedavi tamamlandığında tetiklenir.
    /// </summary>
    public struct TreatmentCompletedEvent : IGameEvent
    {
        public DiseaseType DiseaseType;
        public TreatmentType TreatmentType;
        public bool WasSuccessful;
    }

    /// <summary>
    /// Sağlık kontrolü yapıldığında tetiklenir.
    /// </summary>
    public struct HealthCheckupEvent : IGameEvent
    {
        public int DiseasesFound;
        public decimal Cost;
    }

    #endregion

    #region Disease Definitions

    /// <summary>
    /// Hastalık tanımları ve özellikleri.
    /// </summary>
    public static class DiseaseDefinitions
    {
        /// <summary>
        /// Hastalık bilgisini al.
        /// </summary>
        public static DiseaseInfo GetDiseaseInfo(DiseaseType type)
        {
            return type switch
            {
                // Kanserler
                DiseaseType.LungCancer => new DiseaseInfo("Akciğer Kanseri", "Akciğer hücrelerinde oluşan kanser", DiseaseCategory.Cancer, 8, 15, true, true, new int[] { 40, 90 }),
                DiseaseType.BreastCancer => new DiseaseInfo("Meme Kanseri", "Meme dokusunda oluşan kanser", DiseaseCategory.Cancer, 7, 12, true, true, new int[] { 30, 80 }),
                DiseaseType.ColonCancer => new DiseaseInfo("Kolon Kanseri", "Kalın bağırsakta oluşan kanser", DiseaseCategory.Cancer, 7, 10, true, true, new int[] { 45, 85 }),
                DiseaseType.ProstateCancer => new DiseaseInfo("Prostat Kanseri", "Prostat bezinde oluşan kanser", DiseaseCategory.Cancer, 6, 8, true, true, new int[] { 50, 90 }),
                DiseaseType.SkinCancer => new DiseaseInfo("Cilt Kanseri", "Ciltte oluşan kanser", DiseaseCategory.Cancer, 5, 6, true, false, new int[] { 20, 80 }),
                DiseaseType.Leukemia => new DiseaseInfo("Lösemi", "Kan kanseri", DiseaseCategory.Cancer, 9, 20, true, true, new int[] { 0, 90 }),
                DiseaseType.BrainTumor => new DiseaseInfo("Beyin Tümörü", "Beyinde oluşan tümör", DiseaseCategory.Cancer, 9, 25, true, true, new int[] { 20, 80 }),
                DiseaseType.StomachCancer => new DiseaseInfo("Mide Kanseri", "Midede oluşan kanser", DiseaseCategory.Cancer, 8, 15, true, true, new int[] { 40, 85 }),

                // Kalp Hastalıkları
                DiseaseType.HeartDisease => new DiseaseInfo("Kalp Hastalığı", "Genel kalp rahatsızlıkları", DiseaseCategory.Cardiovascular, 7, 10, true, true, new int[] { 40, 90 }),
                DiseaseType.Hypertension => new DiseaseInfo("Yüksek Tansiyon", "Hipertansiyon", DiseaseCategory.Cardiovascular, 4, 3, false, true, new int[] { 30, 90 }),
                DiseaseType.Arrhythmia => new DiseaseInfo("Aritmi", "Kalp ritim bozukluğu", DiseaseCategory.Cardiovascular, 5, 5, true, true, new int[] { 20, 90 }),
                DiseaseType.HeartFailure => new DiseaseInfo("Kalp Yetmezliği", "Kalbin pompalama gücü kaybı", DiseaseCategory.Cardiovascular, 9, 20, true, true, new int[] { 50, 90 }),
                DiseaseType.CoronaryArtery => new DiseaseInfo("Koroner Arter", "Koroner arter hastalığı", DiseaseCategory.Cardiovascular, 8, 15, true, true, new int[] { 40, 90 }),

                // Kronik Hastalıklar
                DiseaseType.Diabetes => new DiseaseInfo("Diyabet", "Şeker hastalığı", DiseaseCategory.Chronic, 5, 5, false, true, new int[] { 10, 90 }),
                DiseaseType.Asthma => new DiseaseInfo("Astım", "Solunum yolu hastalığı", DiseaseCategory.Chronic, 4, 2, false, true, new int[] { 5, 80 }),
                DiseaseType.Epilepsy => new DiseaseInfo("Epilepsi", "Sara hastalığı", DiseaseCategory.Chronic, 5, 3, false, true, new int[] { 0, 70 }),
                DiseaseType.Arthritis => new DiseaseInfo("Artrit", "Eklem iltihabı", DiseaseCategory.Chronic, 4, 3, false, true, new int[] { 40, 90 }),
                DiseaseType.Alzheimer => new DiseaseInfo("Alzheimer", "Bunama hastalığı", DiseaseCategory.Chronic, 8, 15, true, true, new int[] { 60, 100 }),
                DiseaseType.Parkinson => new DiseaseInfo("Parkinson", "Sinir sistemi hastalığı", DiseaseCategory.Chronic, 7, 10, true, true, new int[] { 55, 90 }),
                DiseaseType.MultipleSclerosis => new DiseaseInfo("MS", "Multipl skleroz", DiseaseCategory.Chronic, 7, 8, true, true, new int[] { 20, 60 }),

                // Enfeksiyonlar
                DiseaseType.Flu => new DiseaseInfo("Grip", "Mevsimsel grip", DiseaseCategory.Infection, 2, 1, false, false, new int[] { 0, 100 }),
                DiseaseType.Pneumonia => new DiseaseInfo("Zatürre", "Akciğer iltihabı", DiseaseCategory.Infection, 6, 8, true, false, new int[] { 0, 100 }),
                DiseaseType.Tuberculosis => new DiseaseInfo("Verem", "Tüberküloz", DiseaseCategory.Infection, 7, 10, true, false, new int[] { 10, 80 }),
                DiseaseType.Hepatitis => new DiseaseInfo("Hepatit", "Karaciğer iltihabı", DiseaseCategory.Infection, 6, 7, true, false, new int[] { 15, 70 }),
                DiseaseType.HIV => new DiseaseInfo("HIV", "İnsan bağışıklık yetmezliği virüsü", DiseaseCategory.Infection, 8, 10, false, true, new int[] { 15, 60 }),

                // Akıl Sağlığı
                DiseaseType.Depression => new DiseaseInfo("Depresyon", "Klinik depresyon", DiseaseCategory.Mental, 5, 3, false, true, new int[] { 10, 90 }),
                DiseaseType.Anxiety => new DiseaseInfo("Anksiyete", "Kaygı bozukluğu", DiseaseCategory.Mental, 4, 2, false, true, new int[] { 10, 80 }),
                DiseaseType.Bipolar => new DiseaseInfo("Bipolar", "Bipolar bozukluk", DiseaseCategory.Mental, 6, 4, false, true, new int[] { 15, 50 }),
                DiseaseType.PTSD => new DiseaseInfo("TSSB", "Travma sonrası stres bozukluğu", DiseaseCategory.Mental, 5, 3, false, true, new int[] { 10, 80 }),

                // Diğer
                DiseaseType.Kidney => new DiseaseInfo("Böbrek Hastalığı", "Böbrek fonksiyon bozukluğu", DiseaseCategory.Other, 7, 12, true, true, new int[] { 30, 90 }),
                DiseaseType.Liver => new DiseaseInfo("Karaciğer Hastalığı", "Karaciğer fonksiyon bozukluğu", DiseaseCategory.Other, 7, 10, true, true, new int[] { 25, 85 }),
                DiseaseType.Thyroid => new DiseaseInfo("Tiroid Hastalığı", "Tiroid bezi bozukluğu", DiseaseCategory.Other, 4, 2, false, true, new int[] { 20, 70 }),
                DiseaseType.Anemia => new DiseaseInfo("Anemi", "Kansızlık", DiseaseCategory.Other, 3, 2, false, false, new int[] { 10, 80 }),
                DiseaseType.Osteoporosis => new DiseaseInfo("Osteoporoz", "Kemik erimesi", DiseaseCategory.Other, 5, 4, false, true, new int[] { 50, 90 }),

                // Yaralanmalar
                DiseaseType.BrokenBone => new DiseaseInfo("Kırık Kemik", "Kemik kırığı", DiseaseCategory.Injury, 4, 5, false, false, new int[] { 0, 100 }),
                DiseaseType.Concussion => new DiseaseInfo("Beyin Sarsıntısı", "Kafa travması", DiseaseCategory.Injury, 5, 3, false, false, new int[] { 5, 80 }),
                DiseaseType.BurnInjury => new DiseaseInfo("Yanık", "Yanık yaralanması", DiseaseCategory.Injury, 5, 5, false, false, new int[] { 0, 100 }),

                _ => new DiseaseInfo("Bilinmeyen", "Bilinmeyen hastalık", DiseaseCategory.Other, 5, 5, false, false, new int[] { 0, 100 })
            };
        }
    }

    /// <summary>
    /// Hastalık bilgi yapısı.
    /// </summary>
    public struct DiseaseInfo
    {
        public string Name;
        public string Description;
        public DiseaseCategory Category;
        public int BaseSeverity;        // 1-10
        public int HealthImpactPerYear; // Yıllık sağlık kaybı
        public bool CanBeFatal;         // Ölümcül olabilir mi
        public bool IsChronic;          // Kronik mi
        public int[] AgeRange;          // Min-max yaş aralığı

        public DiseaseInfo(string name, string description, DiseaseCategory category,
                         int baseSeverity, int healthImpact, bool canBeFatal, bool isChronic, int[] ageRange)
        {
            Name = name;
            Description = description;
            Category = category;
            BaseSeverity = baseSeverity;
            HealthImpactPerYear = healthImpact;
            CanBeFatal = canBeFatal;
            IsChronic = isChronic;
            AgeRange = ageRange;
        }
    }

    /// <summary>
    /// Tedavi tanımları.
    /// </summary>
    public static class TreatmentDefinitions
    {
        /// <summary>
        /// Tedavi bilgisini al.
        /// </summary>
        public static TreatmentInfo GetTreatmentInfo(TreatmentType type)
        {
            return type switch
            {
                // Genel
                TreatmentType.Medication => new TreatmentInfo("İlaç Tedavisi", "Reçeteli ilaç tedavisi", 4, 70, 500),
                TreatmentType.Surgery => new TreatmentInfo("Ameliyat", "Cerrahi müdahale", 1, 85, 15000),
                TreatmentType.PhysicalTherapy => new TreatmentInfo("Fizik Tedavi", "Fiziksel rehabilitasyon", 12, 75, 3000),
                TreatmentType.Hospitalization => new TreatmentInfo("Hastanede Yatış", "Hastane tedavisi", 2, 80, 8000),

                // Kanser
                TreatmentType.Chemotherapy => new TreatmentInfo("Kemoterapi", "Kansere karşı ilaç tedavisi", 24, 60, 50000),
                TreatmentType.Radiation => new TreatmentInfo("Radyoterapi", "Radyasyon tedavisi", 8, 65, 30000),
                TreatmentType.Immunotherapy => new TreatmentInfo("İmmünoterapi", "Bağışıklık sistemi tedavisi", 16, 55, 80000),
                TreatmentType.BoneMarrow => new TreatmentInfo("Kemik İliği Nakli", "Kemik iliği transplantasyonu", 4, 50, 200000),

                // Kalp
                TreatmentType.Bypass => new TreatmentInfo("Bypass Ameliyatı", "Koroner bypass", 1, 85, 80000),
                TreatmentType.Angioplasty => new TreatmentInfo("Anjiyoplasti", "Damar genişletme", 1, 90, 25000),
                TreatmentType.Pacemaker => new TreatmentInfo("Kalp Pili", "Kalp pili yerleştirme", 1, 95, 40000),
                TreatmentType.HeartTransplant => new TreatmentInfo("Kalp Nakli", "Kalp transplantasyonu", 2, 75, 500000),

                // Psikolojik
                TreatmentType.Psychotherapy => new TreatmentInfo("Psikoterapi", "Psikolojik terapi", 24, 70, 5000),
                TreatmentType.Counseling => new TreatmentInfo("Danışmanlık", "Psikolojik danışmanlık", 12, 65, 2000),

                // Diğer
                TreatmentType.Dialysis => new TreatmentInfo("Diyaliz", "Böbrek diyalizi", 52, 60, 50000),
                TreatmentType.Insulin => new TreatmentInfo("İnsülin", "İnsülin tedavisi", 52, 90, 3000),
                TreatmentType.Rehabilitation => new TreatmentInfo("Rehabilitasyon", "Genel rehabilitasyon", 12, 80, 10000),
                TreatmentType.AlternativeMedicine => new TreatmentInfo("Alternatif Tıp", "Alternatif tedavi", 8, 30, 2000),

                _ => new TreatmentInfo("Bilinmeyen", "Bilinmeyen tedavi", 4, 50, 1000)
            };
        }

        /// <summary>
        /// Hastalık için önerilen tedavileri al.
        /// </summary>
        public static List<TreatmentType> GetRecommendedTreatments(DiseaseType disease)
        {
            var treatments = new List<TreatmentType>();

            var info = DiseaseDefinitions.GetDiseaseInfo(disease);

            switch (info.Category)
            {
                case DiseaseCategory.Cancer:
                    treatments.Add(TreatmentType.Surgery);
                    treatments.Add(TreatmentType.Chemotherapy);
                    treatments.Add(TreatmentType.Radiation);
                    if (disease == DiseaseType.Leukemia)
                        treatments.Add(TreatmentType.BoneMarrow);
                    break;

                case DiseaseCategory.Cardiovascular:
                    treatments.Add(TreatmentType.Medication);
                    if (disease == DiseaseType.HeartDisease || disease == DiseaseType.CoronaryArtery)
                    {
                        treatments.Add(TreatmentType.Bypass);
                        treatments.Add(TreatmentType.Angioplasty);
                    }
                    if (disease == DiseaseType.Arrhythmia)
                        treatments.Add(TreatmentType.Pacemaker);
                    if (disease == DiseaseType.HeartFailure)
                        treatments.Add(TreatmentType.HeartTransplant);
                    break;

                case DiseaseCategory.Chronic:
                    treatments.Add(TreatmentType.Medication);
                    if (disease == DiseaseType.Diabetes)
                        treatments.Add(TreatmentType.Insulin);
                    treatments.Add(TreatmentType.PhysicalTherapy);
                    break;

                case DiseaseCategory.Infection:
                    treatments.Add(TreatmentType.Medication);
                    treatments.Add(TreatmentType.Hospitalization);
                    break;

                case DiseaseCategory.Mental:
                    treatments.Add(TreatmentType.Psychotherapy);
                    treatments.Add(TreatmentType.Medication);
                    treatments.Add(TreatmentType.Counseling);
                    break;

                case DiseaseCategory.Injury:
                    treatments.Add(TreatmentType.Surgery);
                    treatments.Add(TreatmentType.PhysicalTherapy);
                    treatments.Add(TreatmentType.Rehabilitation);
                    break;

                default:
                    treatments.Add(TreatmentType.Medication);
                    treatments.Add(TreatmentType.Hospitalization);
                    break;
            }

            return treatments;
        }
    }

    /// <summary>
    /// Tedavi bilgi yapısı.
    /// </summary>
    public struct TreatmentInfo
    {
        public string Name;
        public string Description;
        public int DurationWeeks;       // Hafta cinsinden süre
        public float BaseSuccessRate;   // 0-100 başarı oranı
        public decimal BaseCost;        // TL maliyeti

        public TreatmentInfo(string name, string description, int durationWeeks, float successRate, decimal baseCost)
        {
            Name = name;
            Description = description;
            DurationWeeks = durationWeeks;
            BaseSuccessRate = successRate;
            BaseCost = baseCost;
        }
    }

    #endregion
}
