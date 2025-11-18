using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Aktivite Sistemi - Oyuncunun yapabileceği aktiviteleri yönetir.
    /// BitLife benzeri aktivite menüsü sistemi.
    /// </summary>
    public class ActivitySystem : Singleton<ActivitySystem>
    {
        #region Activity Definitions

        public static List<Activity> GetAvailableActivities(CharacterData character)
        {
            var activities = new List<Activity>();
            int age = character.age;

            // Sağlık Aktiviteleri
            if (age >= 5)
            {
                activities.Add(new Activity
                {
                    id = "exercise",
                    name = "Egzersiz Yap",
                    description = "Spor yaparak sağlığını geliştir",
                    category = ActivityCategory.Health,
                    minAge = 5,
                    Execute = (c) => ExecuteExercise(c)
                });
            }

            if (age >= 12)
            {
                activities.Add(new Activity
                {
                    id = "gym",
                    name = "Spor Salonuna Git",
                    description = "Düzenli spor yap",
                    category = ActivityCategory.Health,
                    minAge = 12,
                    Execute = (c) => ExecuteGym(c)
                });

                activities.Add(new Activity
                {
                    id = "meditation",
                    name = "Meditasyon Yap",
                    description = "Zihinsel sağlığını geliştir",
                    category = ActivityCategory.Health,
                    minAge = 12,
                    Execute = (c) => ExecuteMeditation(c)
                });

                activities.Add(new Activity
                {
                    id = "doctor",
                    name = "Doktora Git",
                    description = "Sağlık kontrolü yaptır",
                    category = ActivityCategory.Health,
                    minAge = 0,
                    cost = 500,
                    Execute = (c) => ExecuteDoctor(c)
                });
            }

            // Eğitim Aktiviteleri
            if (age >= 6 && age <= 18)
            {
                activities.Add(new Activity
                {
                    id = "study",
                    name = "Ders Çalış",
                    description = "Zekayı ve notlarını geliştir",
                    category = ActivityCategory.Education,
                    minAge = 6,
                    Execute = (c) => ExecuteStudy(c)
                });
            }

            if (age >= 10)
            {
                activities.Add(new Activity
                {
                    id = "library",
                    name = "Kütüphaneye Git",
                    description = "Kitap okuyarak zekayı geliştir",
                    category = ActivityCategory.Education,
                    minAge = 10,
                    Execute = (c) => ExecuteLibrary(c)
                });
            }

            if (age >= 18 && character.Education.currentLevel < EducationLevel.University)
            {
                activities.Add(new Activity
                {
                    id = "university_apply",
                    name = "Üniversiteye Başvur",
                    description = "YKS puanınla üniversiteye başvur",
                    category = ActivityCategory.Education,
                    minAge = 18,
                    Execute = (c) => ExecuteUniversityApply(c)
                });
            }

            // Sosyal Aktiviteler
            if (age >= 5)
            {
                activities.Add(new Activity
                {
                    id = "make_friend",
                    name = "Arkadaş Edin",
                    description = "Yeni insanlarla tanış",
                    category = ActivityCategory.Social,
                    minAge = 5,
                    Execute = (c) => ExecuteMakeFriend(c)
                });
            }

            if (age >= 13)
            {
                activities.Add(new Activity
                {
                    id = "party",
                    name = "Partiye Git",
                    description = "Sosyalleş ve eğlen",
                    category = ActivityCategory.Social,
                    minAge = 13,
                    Execute = (c) => ExecuteParty(c)
                });

                activities.Add(new Activity
                {
                    id = "date",
                    name = "Flört Et",
                    description = "Romantik ilişki ara",
                    category = ActivityCategory.Social,
                    minAge = 13,
                    Execute = (c) => ExecuteDate(c)
                });
            }

            // Kariyer Aktiviteleri
            if (age >= 16)
            {
                activities.Add(new Activity
                {
                    id = "job_search",
                    name = "İş Ara",
                    description = "Mevcut işlere başvur",
                    category = ActivityCategory.Career,
                    minAge = 16,
                    Execute = (c) => ExecuteJobSearch(c)
                });

                if (character.isEmployed && character.Career.currentJob != null)
                {
                    activities.Add(new Activity
                    {
                        id = "work_hard",
                        name = "Fazla Mesai Yap",
                        description = "Terfi şansını artır",
                        category = ActivityCategory.Career,
                        minAge = 16,
                        Execute = (c) => ExecuteWorkHard(c)
                    });

                    activities.Add(new Activity
                    {
                        id = "quit_job",
                        name = "İşten Ayrıl",
                        description = "Mevcut işinden istifa et",
                        category = ActivityCategory.Career,
                        minAge = 16,
                        Execute = (c) => ExecuteQuitJob(c)
                    });
                }
            }

            // Eğlence Aktiviteleri
            if (age >= 5)
            {
                activities.Add(new Activity
                {
                    id = "play_games",
                    name = "Oyun Oyna",
                    description = "Eğlen ve mutluluğunu artır",
                    category = ActivityCategory.Entertainment,
                    minAge = 5,
                    Execute = (c) => ExecutePlayGames(c)
                });
            }

            if (age >= 10)
            {
                activities.Add(new Activity
                {
                    id = "cinema",
                    name = "Sinemaya Git",
                    description = "Film izleyerek eğlen",
                    category = ActivityCategory.Entertainment,
                    minAge = 10,
                    cost = 100,
                    Execute = (c) => ExecuteCinema(c)
                });
            }

            if (age >= 18)
            {
                activities.Add(new Activity
                {
                    id = "vacation",
                    name = "Tatile Git",
                    description = "Dinlen ve mutluluğunu artır",
                    category = ActivityCategory.Entertainment,
                    minAge = 18,
                    cost = 5000,
                    Execute = (c) => ExecuteVacation(c)
                });
            }

            // Finansal Aktiviteler
            if (age >= 18)
            {
                activities.Add(new Activity
                {
                    id = "lottery",
                    name = "Piyango Al",
                    description = "Şansını dene",
                    category = ActivityCategory.Financial,
                    minAge = 18,
                    cost = 50,
                    Execute = (c) => ExecuteLottery(c)
                });

                if (character.Finances.currentMoney >= 50000)
                {
                    activities.Add(new Activity
                    {
                        id = "buy_car",
                        name = "Araba Al",
                        description = "Yeni bir araç satın al",
                        category = ActivityCategory.Financial,
                        minAge = 18,
                        cost = 50000,
                        Execute = (c) => ExecuteBuyCar(c)
                    });
                }

                if (character.Finances.currentMoney >= 200000)
                {
                    activities.Add(new Activity
                    {
                        id = "buy_house",
                        name = "Ev Al",
                        description = "Ev sahibi ol",
                        category = ActivityCategory.Financial,
                        minAge = 18,
                        cost = 200000,
                        Execute = (c) => ExecuteBuyHouse(c)
                    });
                }

                // Göç etme
                activities.Add(new Activity
                {
                    id = "emigrate",
                    name = "Göç Et",
                    description = "Yurt dışına göç et",
                    category = ActivityCategory.Financial,
                    minAge = 18,
                    Execute = (c) => ExecuteEmigrate(c)
                });

                // Sosyal medya şöhreti
                activities.Add(new Activity
                {
                    id = "social_media",
                    name = "Sosyal Medyada Yayınla",
                    description = "Viral olmaya çalış",
                    category = ActivityCategory.Entertainment,
                    minAge = 13,
                    Execute = (c) => ExecuteSocialMedia(c)
                });
            }

            // Evlilik aktiviteleri
            if (age >= 18 && LifeEventsSystem.CanGetMarried(character))
            {
                activities.Add(new Activity
                {
                    id = "marry",
                    name = "Evlen",
                    description = "Partnerinle evlen",
                    category = ActivityCategory.Social,
                    minAge = 18,
                    Execute = (c) => ExecuteMarriage(c)
                });
            }

            if (character.isMarried)
            {
                activities.Add(new Activity
                {
                    id = "have_child",
                    name = "Çocuk Yap",
                    description = "Aileyi büyüt",
                    category = ActivityCategory.Social,
                    minAge = 18,
                    Execute = (c) => ExecuteHaveChild(c)
                });

                activities.Add(new Activity
                {
                    id = "divorce",
                    name = "Boşan",
                    description = "Evliliğini bitir",
                    category = ActivityCategory.Social,
                    minAge = 18,
                    Execute = (c) => ExecuteDivorce(c)
                });
            }

            // Askerlik (erkekler için)
            if (LifeEventsSystem.ShouldTriggerMilitary(character))
            {
                activities.Add(new Activity
                {
                    id = "military",
                    name = "Askere Git",
                    description = "Zorunlu askerlik hizmeti",
                    category = ActivityCategory.Career,
                    minAge = 20,
                    Execute = (c) => ExecuteMilitary(c)
                });
            }

            // Suç Aktiviteleri (opsiyonel, riskli)
            if (age >= 12)
            {
                activities.Add(new Activity
                {
                    id = "steal",
                    name = "Hırsızlık Yap",
                    description = "Riskli ama kazançlı",
                    category = ActivityCategory.Crime,
                    minAge = 12,
                    Execute = (c) => ExecuteSteal(c)
                });
            }

            return activities;
        }

        #endregion

        #region Activity Executions

        private static string ExecuteExercise(CharacterData character)
        {
            int healthGain = UnityEngine.Random.Range(2, 6);
            int happinessGain = UnityEngine.Random.Range(1, 4);

            character.stats.ModifyStat(StatType.Health, healthGain);
            character.stats.ModifyStat(StatType.Happiness, happinessGain);

            return $"Egzersiz yaptın! Sağlık +{healthGain}, Mutluluk +{happinessGain}";
        }

        private static string ExecuteGym(CharacterData character)
        {
            if (character.Finances.currentMoney < 200)
            {
                return "Spor salonu ücreti için yeterli paran yok! (200 TL gerekli)";
            }

            character.Finances.ModifyMoney(-200, "Spor salonu üyeliği");

            int healthGain = UnityEngine.Random.Range(3, 8);
            int appearanceGain = UnityEngine.Random.Range(1, 4);

            character.stats.ModifyStat(StatType.Health, healthGain);
            character.stats.ModifyStat(StatType.Appearance, appearanceGain);

            return $"Spor salonunda çalıştın! Sağlık +{healthGain}, Görünüş +{appearanceGain}";
        }

        private static string ExecuteMeditation(CharacterData character)
        {
            int happinessGain = UnityEngine.Random.Range(3, 8);
            int healthGain = UnityEngine.Random.Range(1, 3);

            character.stats.ModifyStat(StatType.Happiness, happinessGain);
            character.stats.ModifyStat(StatType.Health, healthGain);

            return $"Meditasyon yaptın! Mutluluk +{happinessGain}, Sağlık +{healthGain}";
        }

        private static string ExecuteDoctor(CharacterData character)
        {
            if (character.Finances.currentMoney < 500)
            {
                return "Doktor ücreti için yeterli paran yok! (500 TL gerekli)";
            }

            character.Finances.ModifyMoney(-500, "Doktor muayenesi");

            if (character.stats.health < 50)
            {
                int healthGain = UnityEngine.Random.Range(10, 20);
                character.stats.ModifyStat(StatType.Health, healthGain);
                return $"Tedavi oldun! Sağlık +{healthGain}";
            }
            else
            {
                int healthGain = UnityEngine.Random.Range(2, 5);
                character.stats.ModifyStat(StatType.Health, healthGain);
                return $"Sağlık kontrolü yaptırdın. Her şey yolunda! Sağlık +{healthGain}";
            }
        }

        private static string ExecuteStudy(CharacterData character)
        {
            int intelligenceGain = UnityEngine.Random.Range(1, 4);
            int happinessLoss = UnityEngine.Random.Range(1, 3);

            character.stats.ModifyStat(StatType.Intelligence, intelligenceGain);
            character.stats.ModifyStat(StatType.Happiness, -happinessLoss);
            character.Education.gpa += UnityEngine.Random.Range(0.05f, 0.15f);
            character.Education.gpa = Mathf.Min(character.Education.gpa, 4.0f);

            return $"Ders çalıştın! Zeka +{intelligenceGain}, Mutluluk -{happinessLoss}, Not ortalaması arttı.";
        }

        private static string ExecuteLibrary(CharacterData character)
        {
            int intelligenceGain = UnityEngine.Random.Range(2, 5);

            character.stats.ModifyStat(StatType.Intelligence, intelligenceGain);

            return $"Kütüphanede kitap okudun! Zeka +{intelligenceGain}";
        }

        private static string ExecuteUniversityApply(CharacterData character)
        {
            if (character.Education.currentLevel < EducationLevel.HighSchool)
            {
                return "Önce liseyi bitirmelisin!";
            }

            // YKS puanını hesapla (zekaya bağlı)
            int baseScore = character.stats.intelligence * 4;
            int randomBonus = UnityEngine.Random.Range(-50, 51);
            int yksScore = Mathf.Clamp(baseScore + randomBonus, 100, 500);

            character.Education.yksScore = yksScore;

            // Uygun üniversiteleri bul
            var universities = DataManager.Instance.GetAvailableUniversities(yksScore);

            if (universities.Count > 0)
            {
                var selectedUni = universities[UnityEngine.Random.Range(0, universities.Count)];
                character.Education.universityName = selectedUni.name;
                character.Education.currentLevel = EducationLevel.University;
                character.Education.schoolName = selectedUni.name;

                int happinessGain = UnityEngine.Random.Range(15, 25);
                character.stats.ModifyStat(StatType.Happiness, happinessGain);

                return $"YKS Puanın: {yksScore}. {selectedUni.name} kazandın! Mutluluk +{happinessGain}";
            }
            else
            {
                int happinessLoss = UnityEngine.Random.Range(10, 20);
                character.stats.ModifyStat(StatType.Happiness, -happinessLoss);

                return $"YKS Puanın: {yksScore}. Maalesef hiçbir üniversiteyi kazanamadın. Mutluluk -{happinessLoss}";
            }
        }

        private static string ExecuteMakeFriend(CharacterData character)
        {
            if (UnityEngine.Random.value < 0.7f)
            {
                var newFriend = CharacterFactory.CreateRandomFriend(character.age);
                character.relationships.Add(newFriend);

                int happinessGain = UnityEngine.Random.Range(5, 10);
                character.stats.ModifyStat(StatType.Happiness, happinessGain);

                return $"{newFriend.npcName} ile arkadaş oldun! Mutluluk +{happinessGain}";
            }
            else
            {
                return "Kimseyle tanışamadın. Belki başka zaman.";
            }
        }

        private static string ExecuteParty(CharacterData character)
        {
            int happinessGain = UnityEngine.Random.Range(5, 15);
            character.stats.ModifyStat(StatType.Happiness, happinessGain);

            if (UnityEngine.Random.value < 0.3f)
            {
                var newFriend = CharacterFactory.CreateRandomFriend(character.age);
                character.relationships.Add(newFriend);
                return $"Partide eğlendin ve {newFriend.npcName} ile tanıştın! Mutluluk +{happinessGain}";
            }

            return $"Partide eğlendin! Mutluluk +{happinessGain}";
        }

        private static string ExecuteDate(CharacterData character)
        {
            Gender preferredGender = character.gender == Gender.Male ? Gender.Female : Gender.Male;

            if (UnityEngine.Random.value < 0.5f)
            {
                var loveInterest = CharacterFactory.CreateLoveInterest(character.age, preferredGender);
                character.relationships.Add(loveInterest);

                int happinessGain = UnityEngine.Random.Range(10, 20);
                character.stats.ModifyStat(StatType.Happiness, happinessGain);

                string partnerType = preferredGender == Gender.Male ? "erkek arkadaş" : "kız arkadaş";
                return $"{loveInterest.npcName} ile {partnerType} oldun! Mutluluk +{happinessGain}";
            }
            else
            {
                int happinessLoss = UnityEngine.Random.Range(3, 8);
                character.stats.ModifyStat(StatType.Happiness, -happinessLoss);
                return $"Kimseyle bağlantı kuramadın. Mutluluk -{happinessLoss}";
            }
        }

        private static string ExecuteJobSearch(CharacterData character)
        {
            int education = (int)character.Education.currentLevel;
            int intelligence = character.stats.intelligence;

            var availableJobs = DataManager.Instance.GetAvailableJobs(education, intelligence);

            if (availableJobs.Count == 0)
            {
                return "Niteliklerine uygun iş bulunamadı. Eğitimini veya zekayı geliştirmeyi dene.";
            }

            // Rastgele bir işe başvur
            var selectedJob = availableJobs[UnityEngine.Random.Range(0, availableJobs.Count)];

            if (UnityEngine.Random.value < 0.6f + (intelligence * 0.003f))
            {
                // İşe alındı
                character.Career.currentJob = new Job
                {
                    id = selectedJob.id,
                    title = selectedJob.title,
                    category = selectedJob.category,
                    baseSalary = selectedJob.baseSalary,
                    company = GetRandomCompany(),
                    yearsWorked = 0
                };
                character.isEmployed = true;
                character.Career.yearsInJob = 0;

                int happinessGain = UnityEngine.Random.Range(10, 20);
                character.stats.ModifyStat(StatType.Happiness, happinessGain);

                return $"{selectedJob.title} olarak işe alındın! Maaş: {selectedJob.baseSalary:N0} TL. Mutluluk +{happinessGain}";
            }
            else
            {
                int happinessLoss = UnityEngine.Random.Range(3, 8);
                character.stats.ModifyStat(StatType.Happiness, -happinessLoss);
                return $"{selectedJob.title} başvurun reddedildi. Mutluluk -{happinessLoss}";
            }
        }

        private static string ExecuteWorkHard(CharacterData character)
        {
            character.Career.performanceRating += UnityEngine.Random.Range(5, 15);
            character.Career.performanceRating = Mathf.Min(character.Career.performanceRating, 100);

            int happinessLoss = UnityEngine.Random.Range(2, 5);
            character.stats.ModifyStat(StatType.Happiness, -happinessLoss);

            if (character.Career.performanceRating >= 80 && UnityEngine.Random.value < 0.3f)
            {
                // Terfi
                decimal raise = character.Career.currentJob.baseSalary * 0.2m;
                character.Career.currentJob.baseSalary += raise;

                int happinessGain = UnityEngine.Random.Range(15, 25);
                character.stats.ModifyStat(StatType.Happiness, happinessGain);

                return $"Terfiye hak kazandın! Maaşın {raise:N0} TL arttı. Mutluluk +{happinessGain}";
            }

            return $"Fazla mesai yaptın. Performansın arttı. Mutluluk -{happinessLoss}";
        }

        private static string ExecuteQuitJob(CharacterData character)
        {
            string oldJob = character.Career.currentJob.title;

            character.Career.jobHistory.Add(character.Career.currentJob);
            character.Career.currentJob = null;
            character.isEmployed = false;
            character.Career.yearsInJob = 0;
            character.Career.performanceRating = 50;

            return $"{oldJob} işinden ayrıldın.";
        }

        private static string ExecutePlayGames(CharacterData character)
        {
            int happinessGain = UnityEngine.Random.Range(3, 8);
            character.stats.ModifyStat(StatType.Happiness, happinessGain);

            return $"Oyun oynadın ve eğlendin! Mutluluk +{happinessGain}";
        }

        private static string ExecuteCinema(CharacterData character)
        {
            if (character.Finances.currentMoney < 100)
            {
                return "Sinema bileti için yeterli paran yok! (100 TL gerekli)";
            }

            character.Finances.ModifyMoney(-100, "Sinema bileti");

            int happinessGain = UnityEngine.Random.Range(5, 12);
            character.stats.ModifyStat(StatType.Happiness, happinessGain);

            return $"Sinemada güzel bir film izledin! Mutluluk +{happinessGain}";
        }

        private static string ExecuteVacation(CharacterData character)
        {
            if (character.Finances.currentMoney < 5000)
            {
                return "Tatil için yeterli paran yok! (5000 TL gerekli)";
            }

            character.Finances.ModifyMoney(-5000, "Tatil");

            int happinessGain = UnityEngine.Random.Range(15, 30);
            int healthGain = UnityEngine.Random.Range(5, 10);

            character.stats.ModifyStat(StatType.Happiness, happinessGain);
            character.stats.ModifyStat(StatType.Health, healthGain);

            string[] destinations = { "Antalya", "Bodrum", "Fethiye", "Kapadokya", "İstanbul" };
            string destination = destinations[UnityEngine.Random.Range(0, destinations.Length)];

            return $"{destination}'da harika bir tatil geçirdin! Mutluluk +{happinessGain}, Sağlık +{healthGain}";
        }

        private static string ExecuteLottery(CharacterData character)
        {
            if (character.Finances.currentMoney < 50)
            {
                return "Piyango bileti için yeterli paran yok! (50 TL gerekli)";
            }

            character.Finances.ModifyMoney(-50, "Piyango bileti");

            float random = UnityEngine.Random.value;

            if (random < 0.01f)
            {
                // Büyük ikramiye
                int prize = UnityEngine.Random.Range(100000, 500000);
                character.Finances.ModifyMoney(prize, "Piyango büyük ikramiye");
                character.stats.ModifyStat(StatType.Happiness, 50);
                character.stats.ModifyStat(StatType.Fame, 10);
                return $"BÜYÜK İKRAMİYE! {prize:N0} TL kazandın!";
            }
            else if (random < 0.1f)
            {
                // Küçük ödül
                int prize = UnityEngine.Random.Range(100, 1000);
                character.Finances.ModifyMoney(prize, "Piyango küçük ödül");
                character.stats.ModifyStat(StatType.Happiness, 5);
                return $"Küçük bir şey kazandın! {prize:N0} TL";
            }
            else
            {
                return "Maalesef bu sefer kazanamadın.";
            }
        }

        private static string ExecuteBuyCar(CharacterData character)
        {
            if (character.Finances.currentMoney < 50000)
            {
                return "Araba almak için yeterli paran yok! (50000 TL gerekli)";
            }

            character.Finances.ModifyMoney(-50000, "Araba satın alma");
            character.Finances.assets.Add("Otomobil");

            int happinessGain = UnityEngine.Random.Range(10, 20);
            character.stats.ModifyStat(StatType.Happiness, happinessGain);

            return $"Yeni bir araba aldın! Mutluluk +{happinessGain}";
        }

        private static string ExecuteBuyHouse(CharacterData character)
        {
            if (character.Finances.currentMoney < 200000)
            {
                return "Ev almak için yeterli paran yok! (200000 TL gerekli)";
            }

            character.Finances.ModifyMoney(-200000, "Ev satın alma");
            character.Finances.assets.Add("Ev");

            int happinessGain = UnityEngine.Random.Range(20, 35);
            character.stats.ModifyStat(StatType.Happiness, happinessGain);

            return $"Ev sahibi oldun! Mutluluk +{happinessGain}";
        }

        private static string ExecuteSteal(CharacterData character)
        {
            if (UnityEngine.Random.value < 0.4f)
            {
                // Yakalandı
                int happinessLoss = UnityEngine.Random.Range(20, 40);
                character.stats.ModifyStat(StatType.Happiness, -happinessLoss);
                character.stats.ModifyStat(StatType.Fame, -5);

                return $"Yakalandın! Polise götürüldün. Mutluluk -{happinessLoss}";
            }
            else
            {
                // Başarılı
                int stolen = UnityEngine.Random.Range(100, 2000);
                character.Finances.ModifyMoney(stolen, "Hırsızlık");

                return $"Başarıyla {stolen:N0} TL çaldın!";
            }
        }

        private static string GetRandomCompany()
        {
            string[] companies = {
                "Turkcell", "Koç Holding", "Sabancı Holding", "Arçelik", "Ford Otosan",
                "THY", "Garanti BBVA", "İş Bankası", "Akbank", "Yapı Kredi",
                "Migros", "BİM", "A101", "Petrol Ofisi", "Tüpraş"
            };
            return companies[UnityEngine.Random.Range(0, companies.Length)];
        }

        // Yeni hayat olayları aktiviteleri
        private static string ExecuteEmigrate(CharacterData character)
        {
            var options = LifeEventsSystem.GetEmigrationOptions();
            string country = options[UnityEngine.Random.Range(0, options.Count)];
            return LifeEventsSystem.Emigrate(character, country);
        }

        private static string ExecuteSocialMedia(CharacterData character)
        {
            return LifeEventsSystem.BecomeSocialMediaStar(character);
        }

        private static string ExecuteMarriage(CharacterData character)
        {
            bool expensiveWedding = character.Finances.CurrentMoney >= 50000 && UnityEngine.Random.value > 0.5f;
            return LifeEventsSystem.GetMarried(character, expensiveWedding);
        }

        private static string ExecuteHaveChild(CharacterData character)
        {
            return LifeEventsSystem.HaveChild(character);
        }

        private static string ExecuteDivorce(CharacterData character)
        {
            return LifeEventsSystem.GetDivorced(character);
        }

        private static string ExecuteMilitary(CharacterData character)
        {
            var options = LifeEventsSystem.GetMilitaryOptions(character);
            if (options.Count == 0)
            {
                return "Askerlik seçeneği bulunamadı.";
            }

            // En uygun seçeneği seç
            MilitaryOption selected = options[0];

            // Bedelli varsa ve parası yeterliyse tercih et
            var bedelli = options.Find(o => o.id == "bedelli");
            if (bedelli != null && character.Finances.CurrentMoney >= bedelli.cost)
            {
                selected = bedelli;
            }

            return LifeEventsSystem.CompleteMilitaryService(character, selected);
        }

        #endregion

        #region Life Progression

        /// <summary>
        /// Yaşa göre otomatik hayat ilerlemelerini uygula.
        /// Her yaş artışında çağrılır.
        /// </summary>
        public static void ApplyLifeProgression(CharacterData character)
        {
            int age = character.age;

            // Eğitim ilerlemesi
            ApplyEducationProgression(character, age);

            // Maaş alma
            ApplySalary(character);

            // İlişki yaşlanması
            AgeRelationships(character);

            // Rastgele olaylar
            ApplyRandomLifeEvents(character, age);
        }

        private static void ApplyEducationProgression(CharacterData character, int age)
        {
            switch (age)
            {
                case 6:
                    character.Education.currentLevel = EducationLevel.PrimarySchool;
                    character.Education.schoolName = $"{character.birthCity} İlkokulu";
                    character.Education.gpa = UnityEngine.Random.Range(2.5f, 4.0f);
                    break;
                case 10:
                    character.Education.currentLevel = EducationLevel.MiddleSchool;
                    character.Education.schoolName = $"{character.birthCity} Ortaokulu";
                    break;
                case 14:
                    character.Education.currentLevel = EducationLevel.HighSchool;
                    string[] liceTipleri = { "Anadolu Lisesi", "Fen Lisesi", "Meslek Lisesi" };
                    string lise = character.stats.intelligence > 60 ? liceTipleri[1] : (character.stats.intelligence > 40 ? liceTipleri[0] : liceTipleri[2]);
                    character.Education.schoolName = $"{character.birthCity} {lise}";
                    break;
            }
        }

        private static void ApplySalary(CharacterData character)
        {
            if (character.isEmployed && character.Career.currentJob != null)
            {
                decimal salary = character.Career.currentJob.baseSalary;

                // Deneyime göre bonus
                if (character.Career.yearsInJob > 0)
                {
                    salary += salary * (character.Career.yearsInJob * 0.02m);
                }

                character.Finances.ModifyMoney(salary, $"{character.Career.currentJob.title} maaşı");
                character.Career.yearsInJob++;
                character.Career.currentJob.yearsWorked++;
            }
        }

        private static void AgeRelationships(CharacterData character)
        {
            foreach (var rel in character.relationships)
            {
                rel.age++;

                // Yaşlı ebeveynler için ölüm şansı
                if (rel.type == RelationType.Parent && rel.age >= 70)
                {
                    float deathChance = (rel.age - 70) * 0.03f;
                    if (UnityEngine.Random.value < deathChance)
                    {
                        rel.status = RelationshipStatus.Deceased;
                        character.stats.ModifyStat(StatType.Happiness, -30);
                    }
                }
            }
        }

        private static void ApplyRandomLifeEvents(CharacterData character, int age)
        {
            // Rastgele arkadaş kazanma (düşük şans)
            if (UnityEngine.Random.value < 0.1f && age >= 5)
            {
                var friend = CharacterFactory.CreateRandomFriend(age);
                character.relationships.Add(friend);
            }

            // İş performansı değişimi
            if (character.isEmployed)
            {
                int performanceChange = UnityEngine.Random.Range(-5, 10);
                character.Career.performanceRating = Mathf.Clamp(character.Career.performanceRating + performanceChange, 0, 100);
            }
        }

        #endregion
    }

    #region Data Classes

    public class Activity
    {
        public string id;
        public string name;
        public string description;
        public ActivityCategory category;
        public int minAge;
        public decimal cost;
        public Func<CharacterData, string> Execute;
    }

    public enum ActivityCategory
    {
        Health,
        Education,
        Social,
        Career,
        Entertainment,
        Financial,
        Crime
    }

    #endregion
}
