using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Kariyer Sistemi - İş bulma, terfi, maaş yönetimi.
    /// </summary>
    public class CareerSystem : Singleton<CareerSystem>
    {
        private List<JobDefinition> _allJobs = new List<JobDefinition>();
        private Dictionary<string, List<JobDefinition>> _jobsByCategory = new Dictionary<string, List<JobDefinition>>();

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeJobs();
        }

        private void InitializeJobs()
        {
            // Türkiye'ye özel iş veritabanı
            _allJobs = new List<JobDefinition>
            {
                // Yiyecek/İçecek Sektörü
                new JobDefinition("fast_food_worker", "Fast Food Çalışanı", "Yiyecek/İçecek", 8500, EducationLevel.None, 0, 16),
                new JobDefinition("waiter", "Garson", "Yiyecek/İçecek", 9000, EducationLevel.None, 0, 16),
                new JobDefinition("barista", "Barista", "Yiyecek/İçecek", 9500, EducationLevel.None, 0, 18),
                new JobDefinition("chef_assistant", "Aşçı Yardımcısı", "Yiyecek/İçecek", 11000, EducationLevel.HighSchool, 0, 18),
                new JobDefinition("chef", "Aşçı", "Yiyecek/İçecek", 18000, EducationLevel.HighSchool, 30, 20),
                new JobDefinition("head_chef", "Şef", "Yiyecek/İçecek", 35000, EducationLevel.HighSchool, 50, 25),
                new JobDefinition("restaurant_manager", "Restoran Müdürü", "Yiyecek/İçecek", 45000, EducationLevel.University, 40, 25),

                // Perakende
                new JobDefinition("cashier", "Kasiyer", "Perakende", 8500, EducationLevel.None, 0, 16),
                new JobDefinition("sales_associate", "Satış Danışmanı", "Perakende", 9500, EducationLevel.HighSchool, 0, 18),
                new JobDefinition("store_supervisor", "Mağaza Sorumlusu", "Perakende", 15000, EducationLevel.HighSchool, 20, 20),
                new JobDefinition("store_manager", "Mağaza Müdürü", "Perakende", 25000, EducationLevel.University, 35, 25),
                new JobDefinition("regional_manager", "Bölge Müdürü", "Perakende", 50000, EducationLevel.University, 50, 30),

                // Ofis/Büro
                new JobDefinition("office_assistant", "Büro Elemanı", "Ofis", 10000, EducationLevel.HighSchool, 0, 18),
                new JobDefinition("secretary", "Sekreter", "Ofis", 12000, EducationLevel.HighSchool, 20, 18),
                new JobDefinition("accountant_assistant", "Muhasebe Elemanı", "Ofis", 14000, EducationLevel.University, 0, 22),
                new JobDefinition("accountant", "Muhasebeci", "Ofis", 22000, EducationLevel.University, 35, 24),
                new JobDefinition("financial_analyst", "Finans Analisti", "Ofis", 35000, EducationLevel.University, 50, 24),
                new JobDefinition("cfo", "Finans Direktörü", "Ofis", 85000, EducationLevel.Masters, 70, 35),

                // Teknoloji
                new JobDefinition("it_support", "IT Destek", "Teknoloji", 15000, EducationLevel.HighSchool, 30, 18),
                new JobDefinition("junior_developer", "Junior Developer", "Teknoloji", 25000, EducationLevel.University, 40, 22),
                new JobDefinition("software_developer", "Yazılım Geliştirici", "Teknoloji", 45000, EducationLevel.University, 55, 24),
                new JobDefinition("senior_developer", "Senior Developer", "Teknoloji", 70000, EducationLevel.University, 70, 28),
                new JobDefinition("tech_lead", "Teknik Lider", "Teknoloji", 90000, EducationLevel.University, 75, 30),
                new JobDefinition("cto", "CTO", "Teknoloji", 150000, EducationLevel.Masters, 85, 35),
                new JobDefinition("data_scientist", "Veri Bilimci", "Teknoloji", 60000, EducationLevel.Masters, 65, 26),
                new JobDefinition("cybersecurity", "Siber Güvenlik Uzmanı", "Teknoloji", 55000, EducationLevel.University, 60, 24),

                // Sağlık
                new JobDefinition("nurse_assistant", "Hasta Bakıcı", "Sağlık", 10000, EducationLevel.HighSchool, 0, 18),
                new JobDefinition("nurse", "Hemşire", "Sağlık", 18000, EducationLevel.University, 0, 22),
                new JobDefinition("head_nurse", "Başhemşire", "Sağlık", 28000, EducationLevel.University, 40, 28),
                new JobDefinition("pharmacist", "Eczacı", "Sağlık", 35000, EducationLevel.University, 0, 24),
                new JobDefinition("doctor_resident", "Asistan Doktor", "Sağlık", 25000, EducationLevel.Doctorate, 0, 26),
                new JobDefinition("doctor", "Doktor", "Sağlık", 50000, EducationLevel.Doctorate, 0, 30),
                new JobDefinition("specialist_doctor", "Uzman Doktor", "Sağlık", 80000, EducationLevel.Doctorate, 50, 32),
                new JobDefinition("surgeon", "Cerrah", "Sağlık", 120000, EducationLevel.Doctorate, 70, 35),
                new JobDefinition("chief_doctor", "Başhekim", "Sağlık", 150000, EducationLevel.Doctorate, 80, 40),

                // Hukuk
                new JobDefinition("legal_assistant", "Hukuk Asistanı", "Hukuk", 12000, EducationLevel.University, 0, 22),
                new JobDefinition("paralegal", "Paralegal", "Hukuk", 18000, EducationLevel.University, 25, 24),
                new JobDefinition("junior_lawyer", "Stajyer Avukat", "Hukuk", 20000, EducationLevel.University, 0, 24),
                new JobDefinition("lawyer", "Avukat", "Hukuk", 40000, EducationLevel.University, 40, 26),
                new JobDefinition("senior_lawyer", "Kıdemli Avukat", "Hukuk", 70000, EducationLevel.University, 60, 30),
                new JobDefinition("partner", "Ortak Avukat", "Hukuk", 120000, EducationLevel.Masters, 75, 35),
                new JobDefinition("judge", "Hakim", "Hukuk", 45000, EducationLevel.University, 70, 35),

                // Eğitim
                new JobDefinition("teacher_assistant", "Öğretmen Yardımcısı", "Eğitim", 10000, EducationLevel.University, 0, 22),
                new JobDefinition("teacher", "Öğretmen", "Eğitim", 22000, EducationLevel.University, 0, 24),
                new JobDefinition("senior_teacher", "Kıdemli Öğretmen", "Eğitim", 30000, EducationLevel.University, 40, 30),
                new JobDefinition("vice_principal", "Müdür Yardımcısı", "Eğitim", 35000, EducationLevel.Masters, 50, 35),
                new JobDefinition("principal", "Okul Müdürü", "Eğitim", 45000, EducationLevel.Masters, 65, 40),
                new JobDefinition("university_lecturer", "Öğretim Görevlisi", "Eğitim", 35000, EducationLevel.Masters, 0, 28),
                new JobDefinition("professor", "Profesör", "Eğitim", 60000, EducationLevel.Doctorate, 70, 40),

                // İnşaat
                new JobDefinition("construction_worker", "İnşaat İşçisi", "İnşaat", 12000, EducationLevel.None, 0, 18),
                new JobDefinition("electrician", "Elektrikçi", "İnşaat", 16000, EducationLevel.HighSchool, 20, 18),
                new JobDefinition("plumber", "Tesisatçı", "İnşaat", 15000, EducationLevel.HighSchool, 20, 18),
                new JobDefinition("foreman", "Usta", "İnşaat", 22000, EducationLevel.HighSchool, 40, 25),
                new JobDefinition("site_engineer", "Şantiye Mühendisi", "İnşaat", 30000, EducationLevel.University, 30, 24),
                new JobDefinition("civil_engineer", "İnşaat Mühendisi", "İnşaat", 40000, EducationLevel.University, 45, 26),
                new JobDefinition("architect", "Mimar", "İnşaat", 45000, EducationLevel.University, 50, 26),
                new JobDefinition("project_manager", "Proje Müdürü", "İnşaat", 65000, EducationLevel.University, 60, 30),

                // Güvenlik
                new JobDefinition("security_guard", "Güvenlik Görevlisi", "Güvenlik", 10000, EducationLevel.None, 0, 18),
                new JobDefinition("security_supervisor", "Güvenlik Amiri", "Güvenlik", 15000, EducationLevel.HighSchool, 30, 25),
                new JobDefinition("police_officer", "Polis Memuru", "Güvenlik", 18000, EducationLevel.HighSchool, 40, 20),
                new JobDefinition("detective", "Dedektif", "Güvenlik", 28000, EducationLevel.University, 55, 28),
                new JobDefinition("police_chief", "Emniyet Müdürü", "Güvenlik", 50000, EducationLevel.University, 70, 40),

                // Medya/Sanat
                new JobDefinition("intern_journalist", "Stajyer Gazeteci", "Medya", 8000, EducationLevel.University, 0, 22),
                new JobDefinition("journalist", "Gazeteci", "Medya", 18000, EducationLevel.University, 30, 24),
                new JobDefinition("senior_journalist", "Kıdemli Gazeteci", "Medya", 35000, EducationLevel.University, 50, 30),
                new JobDefinition("editor", "Editör", "Medya", 40000, EducationLevel.University, 55, 30),
                new JobDefinition("editor_in_chief", "Genel Yayın Yönetmeni", "Medya", 70000, EducationLevel.University, 70, 40),
                new JobDefinition("photographer", "Fotoğrafçı", "Medya", 15000, EducationLevel.HighSchool, 30, 18),
                new JobDefinition("graphic_designer", "Grafik Tasarımcı", "Medya", 20000, EducationLevel.University, 40, 22),
                new JobDefinition("art_director", "Sanat Yönetmeni", "Medya", 50000, EducationLevel.University, 60, 30),

                // Bankacılık
                new JobDefinition("bank_teller", "Banka Veznedarı", "Bankacılık", 14000, EducationLevel.University, 0, 22),
                new JobDefinition("customer_rep", "Müşteri Temsilcisi", "Bankacılık", 16000, EducationLevel.University, 20, 22),
                new JobDefinition("loan_officer", "Kredi Uzmanı", "Bankacılık", 22000, EducationLevel.University, 35, 24),
                new JobDefinition("branch_manager", "Şube Müdürü", "Bankacılık", 40000, EducationLevel.University, 50, 30),
                new JobDefinition("investment_banker", "Yatırım Bankacısı", "Bankacılık", 80000, EducationLevel.Masters, 65, 28),
                new JobDefinition("bank_director", "Banka Genel Müdürü", "Bankacılık", 200000, EducationLevel.Masters, 80, 45),

                // Ulaşım
                new JobDefinition("taxi_driver", "Taksi Şoförü", "Ulaşım", 12000, EducationLevel.None, 0, 18),
                new JobDefinition("bus_driver", "Otobüs Şoförü", "Ulaşım", 14000, EducationLevel.HighSchool, 0, 21),
                new JobDefinition("truck_driver", "Tır Şoförü", "Ulaşım", 16000, EducationLevel.None, 0, 21),
                new JobDefinition("train_conductor", "Tren Makinisti", "Ulaşım", 20000, EducationLevel.HighSchool, 30, 25),
                new JobDefinition("pilot_trainee", "Pilot Adayı", "Ulaşım", 25000, EducationLevel.University, 50, 22),
                new JobDefinition("pilot", "Pilot", "Ulaşım", 80000, EducationLevel.University, 70, 28),
                new JobDefinition("captain", "Kaptan Pilot", "Ulaşım", 150000, EducationLevel.University, 80, 35),

                // Freelance/Girişimcilik
                new JobDefinition("freelancer", "Serbest Çalışan", "Freelance", 15000, EducationLevel.None, 30, 18),
                new JobDefinition("content_creator", "İçerik Üreticisi", "Freelance", 20000, EducationLevel.None, 40, 16),
                new JobDefinition("influencer", "Influencer", "Freelance", 50000, EducationLevel.None, 60, 16),
                new JobDefinition("entrepreneur", "Girişimci", "Freelance", 30000, EducationLevel.None, 50, 18),
                new JobDefinition("startup_founder", "Startup Kurucusu", "Freelance", 0, EducationLevel.None, 60, 20), // Değişken gelir
                new JobDefinition("ceo", "CEO", "Freelance", 250000, EducationLevel.Masters, 85, 35),
            };

            // Kategorilere ayır
            foreach (var job in _allJobs)
            {
                if (!_jobsByCategory.ContainsKey(job.category))
                {
                    _jobsByCategory[job.category] = new List<JobDefinition>();
                }
                _jobsByCategory[job.category].Add(job);
            }

            Debug.Log($"[CareerSystem] Loaded {_allJobs.Count} jobs in {_jobsByCategory.Count} categories.");
        }

        #endregion

        #region Job Search

        /// <summary>
        /// Karaktere uygun işleri listele.
        /// </summary>
        public List<JobDefinition> GetAvailableJobs(CharacterData character)
        {
            return _allJobs.Where(job => IsEligibleForJob(character, job)).ToList();
        }

        /// <summary>
        /// Belirli kategoriden uygun işleri listele.
        /// </summary>
        public List<JobDefinition> GetAvailableJobsByCategory(CharacterData character, string category)
        {
            if (!_jobsByCategory.ContainsKey(category))
                return new List<JobDefinition>();

            return _jobsByCategory[category].Where(job => IsEligibleForJob(character, job)).ToList();
        }

        /// <summary>
        /// Karakter bu iş için uygun mu?
        /// </summary>
        public bool IsEligibleForJob(CharacterData character, JobDefinition job)
        {
            // Yaş kontrolü
            if (character.Age < job.minAge)
                return false;

            // Eğitim kontrolü
            if ((int)character.Education.CurrentLevel < (int)job.requiredEducation)
                return false;

            // Zeka kontrolü
            if (character.Stats.Intelligence < job.requiredIntelligence)
                return false;

            return true;
        }

        /// <summary>
        /// İş başvurusu yap.
        /// </summary>
        public JobApplicationResult ApplyForJob(CharacterData character, JobDefinition job)
        {
            if (!IsEligibleForJob(character, job))
            {
                return new JobApplicationResult
                {
                    success = false,
                    message = "Bu iş için gerekli şartları karşılamıyorsunuz."
                };
            }

            // Başarı şansı hesapla
            float successChance = CalculateApplicationSuccess(character, job);
            bool success = Random.value < successChance;

            if (success)
            {
                // İşe al
                HireForJob(character, job);
                return new JobApplicationResult
                {
                    success = true,
                    message = $"Tebrikler! {job.title} olarak işe alındınız. Maaşınız: {job.baseSalary:N0} TL"
                };
            }
            else
            {
                return new JobApplicationResult
                {
                    success = false,
                    message = "Maalesef başvurunuz kabul edilmedi. Belki başka bir iş deneyebilirsiniz."
                };
            }
        }

        /// <summary>
        /// Başvuru başarı şansını hesapla.
        /// </summary>
        private float CalculateApplicationSuccess(CharacterData character, JobDefinition job)
        {
            float baseChance = 0.5f;

            // Eğitim bonusu
            int educationDiff = (int)character.Education.CurrentLevel - (int)job.requiredEducation;
            baseChance += educationDiff * 0.1f;

            // Zeka bonusu
            int intelligenceDiff = character.Stats.Intelligence - job.requiredIntelligence;
            baseChance += intelligenceDiff * 0.005f;

            // Görünüş bonusu (bazı işler için)
            if (job.category == "Perakende" || job.category == "Medya" || job.category == "Bankacılık")
            {
                baseChance += (character.Stats.Appearance - 50) * 0.003f;
            }

            // Deneyim bonusu
            if (character.Career.jobHistory.Count > 0)
            {
                baseChance += 0.1f;

                // Aynı kategoride deneyim
                if (character.Career.jobHistory.Any(j => j.category == job.category))
                {
                    baseChance += 0.15f;
                }
            }

            return Mathf.Clamp(baseChance, 0.1f, 0.95f);
        }

        /// <summary>
        /// Karakteri işe al.
        /// </summary>
        private void HireForJob(CharacterData character, JobDefinition jobDef)
        {
            // Eski işi history'ye ekle
            if (character.Career.CurrentJob != null)
            {
                character.Career.jobHistory.Add(character.Career.CurrentJob);
            }

            // Yeni iş ata
            character.Career.currentJob = new Job
            {
                id = jobDef.id,
                title = jobDef.title,
                company = GenerateCompanyName(jobDef.category),
                category = jobDef.category,
                baseSalary = jobDef.baseSalary,
                yearsWorked = 0
            };

            character.Career.yearsInJob = 0;
            character.Career.performanceRating = 50;
            character.isEmployed = true;

            // Event yayınla
            EventBus.Publish(new JobChangedEvent
            {
                NewJobTitle = jobDef.title,
                Salary = jobDef.baseSalary,
                ChangeType = JobChangeType.Hired
            });
        }

        #endregion

        #region Job Management

        /// <summary>
        /// İşten istifa et.
        /// </summary>
        public void QuitJob(CharacterData character)
        {
            if (character.Career.CurrentJob == null)
                return;

            var oldJob = character.Career.CurrentJob;
            character.Career.jobHistory.Add(oldJob);
            character.Career.currentJob = null;
            character.Career.yearsInJob = 0;
            character.isEmployed = false;

            EventBus.Publish(new JobChangedEvent
            {
                NewJobTitle = oldJob.title,
                Salary = 0,
                ChangeType = JobChangeType.Quit
            });
        }

        /// <summary>
        /// İşten kovul.
        /// </summary>
        public void FireFromJob(CharacterData character, string reason)
        {
            if (character.Career.CurrentJob == null)
                return;

            var oldJob = character.Career.CurrentJob;
            character.Career.jobHistory.Add(oldJob);
            character.Career.currentJob = null;
            character.Career.yearsInJob = 0;
            character.isEmployed = false;

            // Mutluluğu düşür
            character.Stats.ModifyStat(StatType.Happiness, -20);

            EventBus.Publish(new JobChangedEvent
            {
                NewJobTitle = oldJob.title,
                Salary = 0,
                ChangeType = JobChangeType.Fired
            });
        }

        /// <summary>
        /// Terfi al.
        /// </summary>
        public bool TryGetPromotion(CharacterData character)
        {
            if (character.Career.CurrentJob == null)
                return false;

            // Performans ve deneyime göre terfi şansı
            float promotionChance = (character.Career.performanceRating - 50) * 0.01f;
            promotionChance += character.Career.yearsInJob * 0.05f;
            promotionChance = Mathf.Clamp(promotionChance, 0.05f, 0.5f);

            if (Random.value < promotionChance)
            {
                // Aynı kategoriden daha yüksek maaşlı iş bul
                var currentJob = character.Career.CurrentJob;
                var betterJob = _allJobs
                    .Where(j => j.category == currentJob.category &&
                           j.baseSalary > currentJob.baseSalary &&
                           IsEligibleForJob(character, j))
                    .OrderBy(j => j.baseSalary)
                    .FirstOrDefault();

                if (betterJob != null)
                {
                    HireForJob(character, betterJob);
                    character.Stats.ModifyStat(StatType.Happiness, 15);
                    return true;
                }
                else
                {
                    // Maaş artışı
                    decimal raise = currentJob.baseSalary * 0.1m;
                    character.Career.currentJob.baseSalary += raise;
                    character.Stats.ModifyStat(StatType.Happiness, 10);

                    EventBus.Publish(new JobChangedEvent
                    {
                        NewJobTitle = currentJob.title,
                        Salary = character.Career.CurrentJob.baseSalary,
                        ChangeType = JobChangeType.Promoted
                    });
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sert çalış (performansı artır).
        /// </summary>
        public void WorkHard(CharacterData character)
        {
            if (character.Career.CurrentJob == null)
                return;

            int performanceGain = Random.Range(3, 8);
            character.Career.performanceRating = Mathf.Min(100, character.Career.performanceRating + performanceGain);

            // Sağlık ve mutluluğu biraz düşür
            character.Stats.ModifyStat(StatType.Health, -Random.Range(1, 4));
            character.Stats.ModifyStat(StatType.Happiness, -Random.Range(2, 6));
        }

        /// <summary>
        /// Yıllık maaş ödemesi.
        /// </summary>
        public void ProcessAnnualSalary(CharacterData character)
        {
            if (character.Career.CurrentJob == null)
                return;

            decimal annualSalary = character.Career.CurrentJob.baseSalary * 12;

            // Performans bonusu
            if (character.Career.performanceRating > 75)
            {
                annualSalary *= 1.1m;
            }

            character.Finances.ModifyMoney(annualSalary, $"Yıllık maaş ({character.Career.CurrentJob.title})");
            character.Career.yearsInJob++;

            // Deneyim arttıkça maaş artışı
            if (character.Career.yearsInJob % 3 == 0)
            {
                character.Career.currentJob.baseSalary *= 1.05m;
            }
        }

        #endregion

        #region Utilities

        private string GenerateCompanyName(string category)
        {
            string[] prefixes = { "Türk", "Anadolu", "İstanbul", "Ege", "Marmara", "Akdeniz", "Global", "Mega", "Ultra" };
            string[] suffixes = category switch
            {
                "Yiyecek/İçecek" => new[] { "Restoran", "Cafe", "Mutfak", "Lezzet" },
                "Perakende" => new[] { "Market", "Mağaza", "AVM", "Ticaret" },
                "Teknoloji" => new[] { "Yazılım", "Tech", "Dijital", "Sistem" },
                "Sağlık" => new[] { "Hastanesi", "Sağlık", "Tıp Merkezi", "Klinik" },
                "Bankacılık" => new[] { "Bank", "Finans", "Yatırım", "Kredi" },
                "İnşaat" => new[] { "İnşaat", "Yapı", "Müteahhitlik", "Proje" },
                _ => new[] { "Şirket", "Holding", "Grup", "A.Ş." }
            };

            return $"{prefixes[Random.Range(0, prefixes.Length)]} {suffixes[Random.Range(0, suffixes.Length)]}";
        }

        public List<string> GetAllCategories()
        {
            return _jobsByCategory.Keys.ToList();
        }

        public JobDefinition GetJobById(string jobId)
        {
            return _allJobs.FirstOrDefault(j => j.id == jobId);
        }

        #endregion
    }

    #region Data Classes

    [System.Serializable]
    public class JobDefinition
    {
        public string id;
        public string title;
        public string category;
        public decimal baseSalary;
        public EducationLevel requiredEducation;
        public int requiredIntelligence;
        public int minAge;

        public JobDefinition(string id, string title, string category, decimal baseSalary,
            EducationLevel requiredEducation, int requiredIntelligence, int minAge)
        {
            this.id = id;
            this.title = title;
            this.category = category;
            this.baseSalary = baseSalary;
            this.requiredEducation = requiredEducation;
            this.requiredIntelligence = requiredIntelligence;
            this.minAge = minAge;
        }
    }

    public class JobApplicationResult
    {
        public bool success;
        public string message;
    }

    public enum JobChangeType
    {
        Hired,
        Quit,
        Fired,
        Promoted
    }

    public struct JobChangedEvent : IGameEvent
    {
        public string NewJobTitle;
        public decimal Salary;
        public JobChangeType ChangeType;
    }

    #endregion
}
