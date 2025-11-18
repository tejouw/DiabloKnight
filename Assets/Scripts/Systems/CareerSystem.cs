using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Kariyer Sistemi - İş bulma, terfi ve kariyer yönetimi.
    /// </summary>
    public static class CareerSystem
    {
        #region İş Listeleri

        /// <summary>
        /// Yarı zamanlı işler (öğrenciler için).
        /// </summary>
        public static List<JobListing> GetPartTimeJobs()
        {
            return new List<JobListing>
            {
                new JobListing
                {
                    Id = "cashier",
                    Title = "Kasiyer",
                    Company = "Market",
                    Category = "Perakende",
                    BaseSalary = 8000f,
                    MinAge = 15,
                    MinEducation = EducationLevel.None,
                    MinIntelligence = 20
                },
                new JobListing
                {
                    Id = "waiter",
                    Title = "Garson",
                    Company = "Restoran",
                    Category = "Hizmet",
                    BaseSalary = 9000f,
                    MinAge = 16,
                    MinEducation = EducationLevel.None,
                    MinIntelligence = 25
                },
                new JobListing
                {
                    Id = "delivery",
                    Title = "Kurye",
                    Company = "Kargo Şirketi",
                    Category = "Lojistik",
                    BaseSalary = 10000f,
                    MinAge = 18,
                    MinEducation = EducationLevel.None,
                    MinIntelligence = 20
                },
                new JobListing
                {
                    Id = "tutor",
                    Title = "Özel Ders Öğretmeni",
                    Company = "Serbest",
                    Category = "Eğitim",
                    BaseSalary = 12000f,
                    MinAge = 16,
                    MinEducation = EducationLevel.HighSchool,
                    MinIntelligence = 60
                }
            };
        }

        /// <summary>
        /// Tam zamanlı işler.
        /// </summary>
        public static List<JobListing> GetFullTimeJobs()
        {
            return new List<JobListing>
            {
                // Giriş Seviyesi
                new JobListing
                {
                    Id = "office_clerk",
                    Title = "Ofis Elemanı",
                    Company = "Çeşitli",
                    Category = "Ofis",
                    BaseSalary = 15000f,
                    MinAge = 18,
                    MinEducation = EducationLevel.HighSchool,
                    MinIntelligence = 40
                },
                new JobListing
                {
                    Id = "sales_rep",
                    Title = "Satış Temsilcisi",
                    Company = "Çeşitli",
                    Category = "Satış",
                    BaseSalary = 18000f,
                    MinAge = 18,
                    MinEducation = EducationLevel.HighSchool,
                    MinIntelligence = 45
                },
                new JobListing
                {
                    Id = "factory_worker",
                    Title = "Fabrika İşçisi",
                    Company = "Fabrika",
                    Category = "Üretim",
                    BaseSalary = 14000f,
                    MinAge = 18,
                    MinEducation = EducationLevel.None,
                    MinIntelligence = 30
                },
                new JobListing
                {
                    Id = "security",
                    Title = "Güvenlik Görevlisi",
                    Company = "Güvenlik Şirketi",
                    Category = "Güvenlik",
                    BaseSalary = 16000f,
                    MinAge = 18,
                    MinEducation = EducationLevel.HighSchool,
                    MinIntelligence = 35
                },

                // Orta Seviye
                new JobListing
                {
                    Id = "accountant",
                    Title = "Muhasebeci",
                    Company = "Çeşitli",
                    Category = "Finans",
                    BaseSalary = 25000f,
                    MinAge = 22,
                    MinEducation = EducationLevel.University,
                    MinIntelligence = 60
                },
                new JobListing
                {
                    Id = "teacher",
                    Title = "Öğretmen",
                    Company = "Okul",
                    Category = "Eğitim",
                    BaseSalary = 22000f,
                    MinAge = 22,
                    MinEducation = EducationLevel.University,
                    MinIntelligence = 65
                },
                new JobListing
                {
                    Id = "nurse",
                    Title = "Hemşire",
                    Company = "Hastane",
                    Category = "Sağlık",
                    BaseSalary = 24000f,
                    MinAge = 22,
                    MinEducation = EducationLevel.University,
                    MinIntelligence = 60
                },
                new JobListing
                {
                    Id = "programmer",
                    Title = "Yazılımcı",
                    Company = "Teknoloji Şirketi",
                    Category = "Teknoloji",
                    BaseSalary = 35000f,
                    MinAge = 22,
                    MinEducation = EducationLevel.University,
                    MinIntelligence = 70
                },

                // Üst Seviye
                new JobListing
                {
                    Id = "engineer",
                    Title = "Mühendis",
                    Company = "Mühendislik Firması",
                    Category = "Mühendislik",
                    BaseSalary = 40000f,
                    MinAge = 24,
                    MinEducation = EducationLevel.University,
                    MinIntelligence = 75
                },
                new JobListing
                {
                    Id = "doctor",
                    Title = "Doktor",
                    Company = "Hastane",
                    Category = "Sağlık",
                    BaseSalary = 60000f,
                    MinAge = 26,
                    MinEducation = EducationLevel.Masters,
                    MinIntelligence = 80
                },
                new JobListing
                {
                    Id = "lawyer",
                    Title = "Avukat",
                    Company = "Hukuk Bürosu",
                    Category = "Hukuk",
                    BaseSalary = 50000f,
                    MinAge = 24,
                    MinEducation = EducationLevel.University,
                    MinIntelligence = 75
                },
                new JobListing
                {
                    Id = "manager",
                    Title = "Müdür",
                    Company = "Çeşitli",
                    Category = "Yönetim",
                    BaseSalary = 45000f,
                    MinAge = 30,
                    MinEducation = EducationLevel.University,
                    MinIntelligence = 70
                },

                // Üst Düzey
                new JobListing
                {
                    Id = "ceo",
                    Title = "CEO",
                    Company = "Şirket",
                    Category = "Yönetim",
                    BaseSalary = 150000f,
                    MinAge = 35,
                    MinEducation = EducationLevel.Masters,
                    MinIntelligence = 85
                },
                new JobListing
                {
                    Id = "professor",
                    Title = "Profesör",
                    Company = "Üniversite",
                    Category = "Akademi",
                    BaseSalary = 70000f,
                    MinAge = 35,
                    MinEducation = EducationLevel.Doctorate,
                    MinIntelligence = 90
                }
            };
        }

        #endregion

        #region İş Başvurusu

        /// <summary>
        /// İşe başvur.
        /// </summary>
        public static JobApplicationResult ApplyForJob(CharacterData character, JobListing job)
        {
            // Yaş kontrolü
            if (character.Age < job.MinAge)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = $"Bu iş için en az {job.MinAge} yaşında olmalısın."
                };
            }

            // Eğitim kontrolü
            if (character.Education.CurrentLevel < job.MinEducation)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = $"Bu iş için {GetEducationLevelName(job.MinEducation)} eğitimi gerekli."
                };
            }

            // Zeka kontrolü
            if (character.Stats.Intelligence < job.MinIntelligence)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Bu iş için yeterli zekan yok."
                };
            }

            // Başarı şansı hesapla
            float baseChance = 0.5f;
            float intelligenceBonus = (character.Stats.Intelligence - job.MinIntelligence) / 100f;
            float appearanceBonus = character.Stats.Appearance / 200f;
            float educationBonus = ((int)character.Education.CurrentLevel - (int)job.MinEducation) * 0.1f;

            float successChance = Mathf.Clamp01(baseChance + intelligenceBonus + appearanceBonus + educationBonus);

            if (Random.value < successChance)
            {
                // İşe alındı
                character.Career.currentJob = new Job
                {
                    id = job.Id,
                    title = job.Title,
                    company = job.Company,
                    category = job.Category,
                    baseSalary = job.BaseSalary,
                    yearsWorked = 0
                };
                character.isEmployed = true;

                return new JobApplicationResult
                {
                    Success = true,
                    Message = $"Tebrikler! {job.Company}'de {job.Title} olarak işe alındın. Maaş: {job.BaseSalary:N0} TL"
                };
            }
            else
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Maalesef işe alınmadın. Başka bir iş dene."
                };
            }
        }

        /// <summary>
        /// İşten istifa et.
        /// </summary>
        public static JobApplicationResult QuitJob(CharacterData character)
        {
            if (character.Career.currentJob == null)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Şu anda bir işin yok."
                };
            }

            string oldJobTitle = character.Career.currentJob.title;

            // İş geçmişine ekle
            character.Career.jobHistory.Add(character.Career.currentJob);
            character.Career.currentJob = null;
            character.isEmployed = false;

            return new JobApplicationResult
            {
                Success = true,
                Message = $"{oldJobTitle} işinden istifa ettin."
            };
        }

        /// <summary>
        /// Terfi iste.
        /// </summary>
        public static JobApplicationResult AskForPromotion(CharacterData character)
        {
            if (character.Career.currentJob == null)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Şu anda bir işin yok."
                };
            }

            var job = character.Career.currentJob;

            // En az 1 yıl çalışmış olmalı
            if (job.yearsWorked < 1)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Terfi için en az 1 yıl çalışmış olmalısın."
                };
            }

            // Başarı şansı
            float baseChance = 0.3f;
            float yearsBonus = Mathf.Min(job.yearsWorked * 0.05f, 0.3f);
            float performanceBonus = character.Career.performanceRating / 200f;
            float intelligenceBonus = character.Stats.Intelligence / 200f;

            float successChance = Mathf.Clamp01(baseChance + yearsBonus + performanceBonus + intelligenceBonus);

            if (Random.value < successChance)
            {
                // Terfi
                float salaryIncrease = job.baseSalary * Random.Range(0.15f, 0.30f);
                job.baseSalary += salaryIncrease;
                character.Career.performanceRating = Mathf.Min(100, character.Career.performanceRating + 10);

                return new JobApplicationResult
                {
                    Success = true,
                    Message = $"Tebrikler! Terfi aldın. Yeni maaş: {job.baseSalary:N0} TL (+{salaryIncrease:N0} TL)"
                };
            }
            else
            {
                character.Career.performanceRating = Mathf.Max(0, character.Career.performanceRating - 5);
                character.Stats.ModifyStat(StatType.Happiness, -5);

                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Terfi isteğin reddedildi."
                };
            }
        }

        /// <summary>
        /// Zam iste.
        /// </summary>
        public static JobApplicationResult AskForRaise(CharacterData character)
        {
            if (character.Career.currentJob == null)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Şu anda bir işin yok."
                };
            }

            var job = character.Career.currentJob;

            // Başarı şansı
            float baseChance = 0.4f;
            float yearsBonus = Mathf.Min(job.yearsWorked * 0.03f, 0.2f);
            float performanceBonus = character.Career.performanceRating / 250f;

            float successChance = Mathf.Clamp01(baseChance + yearsBonus + performanceBonus);

            if (Random.value < successChance)
            {
                float raiseAmount = job.baseSalary * Random.Range(0.05f, 0.15f);
                job.baseSalary += raiseAmount;

                return new JobApplicationResult
                {
                    Success = true,
                    Message = $"Zam aldın! Yeni maaş: {job.baseSalary:N0} TL (+{raiseAmount:N0} TL)"
                };
            }
            else
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Zam isteğin reddedildi."
                };
            }
        }

        /// <summary>
        /// Daha çok çalış (performans artır).
        /// </summary>
        public static JobApplicationResult WorkHarder(CharacterData character)
        {
            if (character.Career.currentJob == null)
            {
                return new JobApplicationResult
                {
                    Success = false,
                    Message = "Şu anda bir işin yok."
                };
            }

            int performanceGain = Random.Range(5, 15);
            character.Career.performanceRating = Mathf.Min(100, character.Career.performanceRating + performanceGain);

            // Sağlık kaybı olabilir
            if (Random.value < 0.3f)
            {
                int healthLoss = Random.Range(2, 5);
                character.Stats.ModifyStat(StatType.Health, -healthLoss);
            }

            return new JobApplicationResult
            {
                Success = true,
                Message = $"Çok çalıştın. Performans +{performanceGain}"
            };
        }

        /// <summary>
        /// Maaş ödemesi yap (yıllık).
        /// </summary>
        public static void ProcessYearlySalary(CharacterData character)
        {
            if (character.Career.currentJob == null) return;

            var job = character.Career.currentJob;
            float yearlySalary = job.baseSalary * 12;

            // Vergi düşümü (%20)
            float tax = yearlySalary * 0.20f;
            float netSalary = yearlySalary - tax;

            character.Finances.ModifyMoney(netSalary, $"Yıllık maaş ({job.title})");

            // İş deneyimi artır
            job.yearsWorked++;

            // Performans düşüşü (sabitleme için)
            character.Career.performanceRating = Mathf.Max(50, character.Career.performanceRating - 5);
        }

        #endregion

        #region Yardımcı Metodlar

        private static string GetEducationLevelName(EducationLevel level)
        {
            return level switch
            {
                EducationLevel.None => "Eğitim yok",
                EducationLevel.Elementary => "İlkokul",
                EducationLevel.MiddleSchool => "Ortaokul",
                EducationLevel.HighSchool => "Lise",
                EducationLevel.University => "Üniversite",
                EducationLevel.Masters => "Yüksek Lisans",
                EducationLevel.Doctorate => "Doktora",
                _ => "Bilinmiyor"
            };
        }

        /// <summary>
        /// Karakter için uygun işleri getir.
        /// </summary>
        public static List<JobListing> GetAvailableJobs(CharacterData character)
        {
            var allJobs = new List<JobListing>();

            // Yarı zamanlı işler (öğrenciler için)
            if (character.Age >= 15 && character.Age < 22)
            {
                allJobs.AddRange(GetPartTimeJobs());
            }

            // Tam zamanlı işler
            if (character.Age >= 18)
            {
                foreach (var job in GetFullTimeJobs())
                {
                    // Şartları karşılıyor mu kontrol et
                    if (character.Age >= job.MinAge &&
                        character.Education.CurrentLevel >= job.MinEducation &&
                        character.Stats.Intelligence >= job.MinIntelligence * 0.8f) // %80 karşılaması yeterli
                    {
                        allJobs.Add(job);
                    }
                }
            }

            return allJobs;
        }

        #endregion
    }

    /// <summary>
    /// İş listesi.
    /// </summary>
    public class JobListing
    {
        public string Id;
        public string Title;
        public string Company;
        public string Category;
        public float BaseSalary;
        public int MinAge;
        public EducationLevel MinEducation;
        public int MinIntelligence;
    }

    /// <summary>
    /// İş başvurusu sonucu.
    /// </summary>
    public class JobApplicationResult
    {
        public bool Success;
        public string Message;
    }
}
