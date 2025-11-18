using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Data;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Kariyer ve Eğitim Yöneticisi - İş ve eğitim sistemlerini yönetir.
    /// </summary>
    public class CareerManager : Singleton<CareerManager>
    {
        #region Properties

        public bool IsEmployed => GameManager.Instance.CurrentCharacter?.isEmployed ?? false;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);
            Debug.Log("[CareerManager] Initialized successfully.");
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<AgeProgressedEvent>(OnAgeProgressed);
        }

        #endregion

        #region Event Handlers

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Eğitim ilerlemesi
            HandleEducationProgression(character);

            // Maaş ödemesi
            ProcessSalary(character);

            // İş performansı
            UpdateJobPerformance(character);
        }

        #endregion

        #region Education System

        /// <summary>
        /// Eğitim ilerlemesini yönet.
        /// </summary>
        private void HandleEducationProgression(CharacterData character)
        {
            int age = character.age;
            var education = character.education;

            // İlkokula başla (6 yaş)
            if (age == 6 && education.currentLevel == EducationLevel.None)
            {
                StartEducation(character, EducationLevel.PrimarySchool, "İlkokul");
            }
            // Ortaokula geç (10 yaş)
            else if (age == 10 && education.currentLevel == EducationLevel.PrimarySchool)
            {
                StartEducation(character, EducationLevel.MiddleSchool, "Ortaokul");
            }
            // Liseye geç (14 yaş)
            else if (age == 14 && education.currentLevel == EducationLevel.MiddleSchool)
            {
                StartEducation(character, EducationLevel.HighSchool, "Anadolu Lisesi");
            }
            // Lise bitişi (18 yaş)
            else if (age == 18 && education.currentLevel == EducationLevel.HighSchool)
            {
                education.isGraduated = true;
            }
        }

        /// <summary>
        /// Eğitime başla.
        /// </summary>
        private void StartEducation(CharacterData character, EducationLevel level, string schoolName)
        {
            character.education.currentLevel = level;
            character.education.schoolName = schoolName;
            character.education.gpa = 0;
            character.education.isGraduated = false;

            EventBus.Publish(new EducationStartedEvent
            {
                Level = level,
                SchoolName = schoolName
            });
        }

        /// <summary>
        /// YKS sınavına gir.
        /// </summary>
        public int TakeYKSExam(CharacterData character)
        {
            // Puan hesaplama: Zeka + GPA + rastgelelik
            int intelligence = character.stats.Intelligence;
            float gpa = character.education.gpa;

            // Temel puan: 200-500 arası
            float baseScore = 200 + (intelligence * 2f) + (gpa * 50f);

            // Rastgelelik: +-30 puan
            float randomFactor = Random.Range(-30f, 30f);

            int finalScore = Mathf.RoundToInt(baseScore + randomFactor);
            finalScore = Mathf.Clamp(finalScore, 100, 500);

            character.education.yksScore = finalScore;

            EventBus.Publish(new YKSResultEvent
            {
                Score = finalScore,
                AvailableUniversities = DataManager.Instance.GetAvailableUniversities(finalScore)
            });

            return finalScore;
        }

        /// <summary>
        /// Üniversiteye kayıt ol.
        /// </summary>
        public bool EnrollUniversity(CharacterData character, UniversityData university, string department)
        {
            if (character.education.yksScore < university.minScore)
            {
                return false;
            }

            character.education.currentLevel = EducationLevel.University;
            character.education.universityName = university.name;
            character.education.department = department;
            character.education.schoolName = university.name;
            character.education.isGraduated = false;

            EventBus.Publish(new EducationStartedEvent
            {
                Level = EducationLevel.University,
                SchoolName = university.name
            });

            return true;
        }

        /// <summary>
        /// Üniversiteden mezun ol.
        /// </summary>
        public void GraduateUniversity(CharacterData character)
        {
            character.education.isGraduated = true;
            character.stats.ModifyStat(StatType.Intelligence, Random.Range(5, 10));

            EventBus.Publish(new GraduationEvent
            {
                Level = EducationLevel.University,
                SchoolName = character.education.universityName
            });
        }

        #endregion

        #region Career System

        /// <summary>
        /// İşe başvur.
        /// </summary>
        public bool ApplyForJob(CharacterData character, JobData jobData)
        {
            int educationLevel = (int)character.education.currentLevel;
            int intelligence = character.stats.Intelligence;

            // Gereksinimler kontrolü
            if (educationLevel < jobData.minEducation || intelligence < jobData.minIntelligence)
            {
                return false;
            }

            // Başvuru başarı şansı
            float baseChance = 0.5f;
            float educationBonus = (educationLevel - jobData.minEducation) * 0.1f;
            float intelligenceBonus = (intelligence - jobData.minIntelligence) * 0.005f;
            float successChance = Mathf.Clamp01(baseChance + educationBonus + intelligenceBonus);

            if (Random.value <= successChance)
            {
                HireForJob(character, jobData);
                return true;
            }

            EventBus.Publish(new JobApplicationResultEvent
            {
                JobTitle = jobData.title,
                Success = false
            });

            return false;
        }

        /// <summary>
        /// İşe al.
        /// </summary>
        public void HireForJob(CharacterData character, JobData jobData)
        {
            // Önceki işi kaydet
            if (character.career.currentJob != null)
            {
                character.career.jobHistory.Add(character.career.currentJob);
            }

            // Yeni iş ata
            character.career.currentJob = new Job
            {
                id = jobData.id,
                title = jobData.title,
                company = GenerateCompanyName(jobData.category),
                category = jobData.category,
                baseSalary = jobData.baseSalary,
                yearsWorked = 0
            };

            character.isEmployed = true;
            character.career.yearsInJob = 0;
            character.career.performanceRating = 50;

            EventBus.Publish(new JobHiredEvent
            {
                JobTitle = jobData.title,
                Salary = jobData.baseSalary
            });
        }

        /// <summary>
        /// İşten ayrıl.
        /// </summary>
        public void QuitJob(CharacterData character)
        {
            if (character.career.currentJob == null) return;

            character.career.jobHistory.Add(character.career.currentJob);

            EventBus.Publish(new JobQuitEvent
            {
                JobTitle = character.career.currentJob.title
            });

            character.career.currentJob = null;
            character.isEmployed = false;
            character.career.yearsInJob = 0;
        }

        /// <summary>
        /// İşten çıkarıl.
        /// </summary>
        public void GetFired(CharacterData character, string reason)
        {
            if (character.career.currentJob == null) return;

            string jobTitle = character.career.currentJob.title;
            character.career.jobHistory.Add(character.career.currentJob);
            character.career.currentJob = null;
            character.isEmployed = false;
            character.career.yearsInJob = 0;

            character.stats.ModifyStat(StatType.Happiness, -15);

            EventBus.Publish(new JobFiredEvent
            {
                JobTitle = jobTitle,
                Reason = reason
            });
        }

        /// <summary>
        /// Maaş işle.
        /// </summary>
        private void ProcessSalary(CharacterData character)
        {
            if (!character.isEmployed || character.career.currentJob == null) return;

            var job = character.career.currentJob;

            // Yıllık maaş hesapla (kıdem zammı ile)
            decimal baseSalary = job.baseSalary;
            decimal seniorityBonus = baseSalary * (character.career.yearsInJob * 0.05m);
            decimal performanceBonus = baseSalary * (character.career.performanceRating / 1000m);

            decimal annualSalary = (baseSalary + seniorityBonus + performanceBonus) * 12;

            character.finances.ModifyMoney(annualSalary, $"{job.title} maaşı");
            character.career.yearsInJob++;
            job.yearsWorked++;
        }

        /// <summary>
        /// İş performansını güncelle.
        /// </summary>
        private void UpdateJobPerformance(CharacterData character)
        {
            if (!character.isEmployed) return;

            // Rastgele performans değişimi
            int change = Random.Range(-5, 10);
            character.career.performanceRating = Mathf.Clamp(
                character.career.performanceRating + change, 0, 100);
        }

        /// <summary>
        /// Terfi al.
        /// </summary>
        public bool GetPromotion(CharacterData character)
        {
            if (!character.isEmployed || character.career.currentJob == null) return false;

            // Terfi şansı: Performans ve kıdeme bağlı
            float chance = (character.career.performanceRating / 200f) +
                          (character.career.yearsInJob * 0.05f);

            if (Random.value <= chance)
            {
                // Maaş artışı
                character.career.currentJob.baseSalary *= 1.2m;
                character.stats.ModifyStat(StatType.Happiness, 10);

                EventBus.Publish(new PromotionEvent
                {
                    JobTitle = character.career.currentJob.title,
                    NewSalary = character.career.currentJob.baseSalary
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// Şirket ismi oluştur.
        /// </summary>
        private string GenerateCompanyName(string category)
        {
            string[] prefixes = { "Türk", "Anadolu", "İstanbul", "Ankara", "Ege", "Marmara" };
            string[] suffixes = { "A.Ş.", "Ltd.", "Holding", "Grup" };

            string prefix = prefixes[Random.Range(0, prefixes.Length)];
            string suffix = suffixes[Random.Range(0, suffixes.Length)];

            return $"{prefix} {category} {suffix}";
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Uygun işleri al.
        /// </summary>
        public List<JobData> GetAvailableJobs(CharacterData character)
        {
            int educationLevel = (int)character.education.currentLevel;
            int intelligence = character.stats.Intelligence;
            return DataManager.Instance.GetAvailableJobs(educationLevel, intelligence);
        }

        /// <summary>
        /// Uygun üniversiteleri al.
        /// </summary>
        public List<UniversityData> GetAvailableUniversities(CharacterData character)
        {
            return DataManager.Instance.GetAvailableUniversities(character.education.yksScore);
        }

        #endregion
    }

    #region Events

    public class EducationStartedEvent
    {
        public EducationLevel Level;
        public string SchoolName;
    }

    public class YKSResultEvent
    {
        public int Score;
        public List<UniversityData> AvailableUniversities;
    }

    public class GraduationEvent
    {
        public EducationLevel Level;
        public string SchoolName;
    }

    public class JobApplicationResultEvent
    {
        public string JobTitle;
        public bool Success;
    }

    public class JobHiredEvent
    {
        public string JobTitle;
        public decimal Salary;
    }

    public class JobQuitEvent
    {
        public string JobTitle;
    }

    public class JobFiredEvent
    {
        public string JobTitle;
        public string Reason;
    }

    public class PromotionEvent
    {
        public string JobTitle;
        public decimal NewSalary;
    }

    #endregion
}
