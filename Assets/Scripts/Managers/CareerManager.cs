using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Data;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Kariyer Yöneticisi - İş bulma, maaş, terfi ve kariyer gelişimini yönetir.
    /// </summary>
    public class CareerManager : Singleton<CareerManager>
    {
        #region Properties

        /// <summary>
        /// Minimum çalışma yaşı.
        /// </summary>
        public const int MIN_WORK_AGE = 16;

        /// <summary>
        /// Emeklilik yaşı.
        /// </summary>
        public const int RETIREMENT_AGE = 60;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[CareerManager] Initialized successfully.");
        }

        #endregion

        #region Job Search

        /// <summary>
        /// Karakterin başvurabileceği işleri getir.
        /// </summary>
        public List<JobData> GetAvailableJobs(CharacterData character)
        {
            if (character == null) return new List<JobData>();

            int educationLevel = (int)character.Education.CurrentLevel;
            int intelligence = character.Stats.Intelligence;

            return DataManager.Instance.GetAvailableJobs(educationLevel, intelligence);
        }

        /// <summary>
        /// İşe başvur.
        /// </summary>
        public JobApplicationResult ApplyForJob(CharacterData character, JobData jobData)
        {
            if (character == null || jobData == null)
            {
                return new JobApplicationResult { Success = false, Message = "Geçersiz başvuru." };
            }

            // Yaş kontrolü
            if (character.Age < MIN_WORK_AGE)
            {
                return new JobApplicationResult { Success = false, Message = "Çalışmak için çok gençsin." };
            }

            // Eğitim ve zeka kontrolü
            int educationLevel = (int)character.Education.CurrentLevel;
            if (educationLevel < jobData.minEducation)
            {
                return new JobApplicationResult { Success = false, Message = "Yeterli eğitim seviyesine sahip değilsin." };
            }

            if (character.Stats.Intelligence < jobData.minIntelligence)
            {
                return new JobApplicationResult { Success = false, Message = "Bu iş için yeterli zekaya sahip değilsin." };
            }

            // Başvuru başarı olasılığı
            float successChance = CalculateApplicationSuccessChance(character, jobData);
            bool success = Random.value < successChance;

            if (success)
            {
                // İşe kabul edildi
                HireForJob(character, jobData);
                return new JobApplicationResult
                {
                    Success = true,
                    Message = $"{jobData.title} pozisyonuna kabul edildin! Tebrikler!",
                    Job = character.Career.CurrentJob
                };
            }
            else
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = $"{jobData.title} başvurun reddedildi. Belki başka zaman."
                };
            }
        }

        /// <summary>
        /// Başvuru başarı şansını hesapla.
        /// </summary>
        private float CalculateApplicationSuccessChance(CharacterData character, JobData jobData)
        {
            float baseChance = 0.4f;

            // Zeka bonusu
            int intelligenceExcess = character.Stats.Intelligence - jobData.minIntelligence;
            baseChance += intelligenceExcess * 0.01f;

            // Eğitim bonusu
            int educationExcess = (int)character.Education.CurrentLevel - jobData.minEducation;
            baseChance += educationExcess * 0.1f;

            // Görünüş bonusu
            baseChance += character.Stats.Appearance * 0.002f;

            // Şöhret bonusu
            baseChance += character.Stats.Fame * 0.003f;

            // Deneyim bonusu (daha önce iş yaptıysa)
            if (character.Career.jobHistory.Count > 0)
            {
                baseChance += 0.1f;
            }

            return Mathf.Clamp01(baseChance);
        }

        /// <summary>
        /// Karakteri işe al.
        /// </summary>
        private void HireForJob(CharacterData character, JobData jobData)
        {
            // Mevcut işten ayrıl
            if (character.Career.CurrentJob != null)
            {
                QuitJob(character, false);
            }

            // Yeni iş oluştur
            var newJob = new Job
            {
                id = jobData.id,
                title = jobData.title,
                company = GenerateCompanyName(jobData.category),
                category = jobData.category,
                baseSalary = jobData.baseSalary,
                yearsWorked = 0,
                performanceBonus = 0,
                startDate = System.DateTime.Now.ToString("dd/MM/yyyy")
            };

            character.Career.currentJob = newJob;
            character.Career.performanceRating = 50;
            character.isEmployed = true;

            Debug.Log($"[CareerManager] {character.FullName} started working as {jobData.title}");
        }

        /// <summary>
        /// Şirket ismi oluştur.
        /// </summary>
        private string GenerateCompanyName(string category)
        {
            string[] prefixes = { "Türk", "Anadolu", "İstanbul", "Ankara", "Ege", "Marmara", "Karadeniz" };
            string[] suffixes = category switch
            {
                "Sağlık" => new[] { "Hastanesi", "Tıp Merkezi", "Sağlık Grubu" },
                "Mühendislik" => new[] { "Teknoloji", "Yazılım", "Mühendislik" },
                "Kamu" => new[] { "Bakanlığı", "Müdürlüğü", "Kurumu" },
                "Ticaret" => new[] { "Ticaret", "Holding", "Şirketi" },
                "Hukuk" => new[] { "Hukuk Bürosu", "Avukatlık", "Danışmanlık" },
                _ => new[] { "Şirketi", "A.Ş.", "Ltd." }
            };

            string prefix = prefixes[Random.Range(0, prefixes.Length)];
            string suffix = suffixes[Random.Range(0, suffixes.Length)];

            return $"{prefix} {suffix}";
        }

        #endregion

        #region Salary & Performance

        /// <summary>
        /// Aylık maaş al.
        /// </summary>
        public long ReceiveSalary(CharacterData character)
        {
            if (character?.Career?.CurrentJob == null) return 0;

            var job = character.Career.CurrentJob;

            // Temel maaş
            long salary = job.baseSalary;

            // Performans bonusu
            float performanceMultiplier = 1f + (character.Career.performanceRating - 50) * 0.005f;
            salary = (long)(salary * performanceMultiplier);

            // Kıdem bonusu (yılda %5)
            float seniorityBonus = 1f + job.yearsWorked * 0.05f;
            salary = (long)(salary * seniorityBonus);

            // Parayı ekle
            character.Finances.ModifyMoney(salary, $"Maaş: {job.title}");

            return salary;
        }

        /// <summary>
        /// İş performansını güncelle.
        /// </summary>
        public void UpdatePerformance(CharacterData character, int change)
        {
            if (character?.Career == null) return;

            int oldRating = character.Career.performanceRating;
            character.Career.performanceRating = Mathf.Clamp(character.Career.performanceRating + change, 0, 100);

            // Çok düşük performans = kovulma riski
            if (character.Career.performanceRating < 20)
            {
                if (Random.value < 0.3f)
                {
                    FireFromJob(character, "Düşük performans nedeniyle işten çıkarıldın.");
                }
            }
        }

        /// <summary>
        /// Yıllık iş güncellemesi (yaş ilerlemesinde çağrılır).
        /// </summary>
        public void ProcessYearlyCareerUpdate(CharacterData character)
        {
            if (character?.Career?.CurrentJob == null) return;

            // Çalışma süresini artır
            character.Career.CurrentJob.yearsWorked++;

            // Rastgele performans değişikliği
            int performanceChange = Random.Range(-5, 10);
            UpdatePerformance(character, performanceChange);

            // Terfi kontrolü
            if (character.Career.CurrentJob.yearsWorked >= 3 && character.Career.performanceRating >= 70)
            {
                if (Random.value < 0.3f)
                {
                    PromoteEmployee(character);
                }
            }
        }

        #endregion

        #region Promotion & Quit

        /// <summary>
        /// Çalışanı terfi ettir.
        /// </summary>
        public void PromoteEmployee(CharacterData character)
        {
            if (character?.Career?.CurrentJob == null) return;

            var job = character.Career.CurrentJob;

            // Maaş artışı (%15-25)
            float raisePercent = Random.Range(0.15f, 0.25f);
            job.baseSalary = (long)(job.baseSalary * (1 + raisePercent));

            // Unvan güncelle
            job.title = GetPromotedTitle(job.title);

            // Mutluluk artışı
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(10, 20));

            // Şöhret artışı
            character.Stats.ModifyStat(StatType.Fame, Random.Range(1, 5));

            Debug.Log($"[CareerManager] {character.FullName} promoted to {job.title}!");
        }

        /// <summary>
        /// Terfi unvanı getir.
        /// </summary>
        private string GetPromotedTitle(string currentTitle)
        {
            // Basit unvan yükseltme
            if (currentTitle.Contains("Stajyer"))
                return currentTitle.Replace("Stajyer", "Junior");
            if (currentTitle.Contains("Junior"))
                return currentTitle.Replace("Junior", "");
            if (!currentTitle.Contains("Kıdemli") && !currentTitle.Contains("Müdür"))
                return "Kıdemli " + currentTitle;
            if (currentTitle.Contains("Kıdemli") && !currentTitle.Contains("Müdür"))
                return currentTitle.Replace("Kıdemli ", "") + " Müdürü";

            return "Genel " + currentTitle;
        }

        /// <summary>
        /// İşten ayrıl.
        /// </summary>
        public void QuitJob(CharacterData character, bool voluntary = true)
        {
            if (character?.Career?.CurrentJob == null) return;

            var oldJob = character.Career.CurrentJob;

            // İş geçmişine ekle
            character.Career.jobHistory.Add(oldJob);

            // Mevcut işi temizle
            character.Career.currentJob = null;
            character.Career.yearsInJob = 0;
            character.isEmployed = false;

            if (voluntary)
            {
                Debug.Log($"[CareerManager] {character.FullName} quit from {oldJob.title}");
            }
        }

        /// <summary>
        /// İşten kovul.
        /// </summary>
        public void FireFromJob(CharacterData character, string reason)
        {
            if (character?.Career?.CurrentJob == null) return;

            var oldJob = character.Career.CurrentJob;
            QuitJob(character, false);

            // Mutluluk kaybı
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(-20, -10));

            Debug.Log($"[CareerManager] {character.FullName} fired from {oldJob.title}: {reason}");
        }

        /// <summary>
        /// Emekli ol.
        /// </summary>
        public long Retire(CharacterData character)
        {
            if (character?.Career?.CurrentJob == null) return 0;

            var job = character.Career.CurrentJob;

            // Kıdem tazminatı hesapla
            long severancePay = job.baseSalary * job.yearsWorked;

            // İşten ayrıl
            QuitJob(character, true);

            // Tazminatı öde
            character.Finances.ModifyMoney(severancePay, "Emekli ikramiyesi");

            // Mutluluk artışı
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 15));

            Debug.Log($"[CareerManager] {character.FullName} retired with {severancePay} TL severance.");

            return severancePay;
        }

        #endregion

        #region Freelance & Side Jobs

        /// <summary>
        /// Part-time/yan iş yap.
        /// </summary>
        public long DoSideJob(CharacterData character, string jobType)
        {
            if (character == null) return 0;

            long earnings = 0;
            int happinessChange = 0;
            int healthChange = 0;

            switch (jobType.ToLower())
            {
                case "freelance":
                    earnings = Random.Range(500, 3000);
                    happinessChange = Random.Range(-5, 5);
                    break;
                case "uber":
                    earnings = Random.Range(200, 800);
                    healthChange = Random.Range(-3, 0);
                    break;
                case "tutoring":
                    earnings = Random.Range(300, 1000);
                    happinessChange = Random.Range(0, 5);
                    break;
                default:
                    earnings = Random.Range(100, 500);
                    break;
            }

            character.Finances.ModifyMoney(earnings, $"Yan iş: {jobType}");

            if (happinessChange != 0)
                character.Stats.ModifyStat(StatType.Happiness, happinessChange);
            if (healthChange != 0)
                character.Stats.ModifyStat(StatType.Health, healthChange);

            return earnings;
        }

        #endregion
    }

    /// <summary>
    /// İş başvuru sonucu.
    /// </summary>
    public class JobApplicationResult
    {
        public bool Success;
        public string Message;
        public Job Job;
    }
}
