using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Aktiviteler Sistemi - Oyuncunun yapabileceği aktiviteleri yönetir.
    /// </summary>
    public static class ActivitiesSystem
    {
        #region Sağlık Aktiviteleri

        /// <summary>
        /// Spor salonuna git.
        /// </summary>
        public static ActivityResult GoToGym(CharacterData character)
        {
            if (character.Age < 12)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Spor salonuna gitmek için çok küçüksün."
                };
            }

            // Para kontrolü (aylık üyelik)
            float cost = 500f;
            if (character.Finances.CurrentMoney < cost)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Spor salonu üyeliği için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-cost, "Spor salonu üyeliği");

            // Sonuç hesaplama
            float successChance = 0.7f + (character.Stats.Health / 200f);
            bool success = Random.value < successChance;

            if (success)
            {
                int healthGain = Random.Range(3, 8);
                int happinessGain = Random.Range(2, 5);
                character.Stats.ModifyStat(StatType.Health, healthGain);
                character.Stats.ModifyStat(StatType.Happiness, happinessGain);

                // Görünüş de iyileşebilir
                if (Random.value < 0.3f)
                {
                    character.Stats.ModifyStat(StatType.Appearance, Random.Range(1, 3));
                }

                return new ActivityResult
                {
                    Success = true,
                    Message = $"Harika bir antrenman yaptın! Sağlık +{healthGain}, Mutluluk +{happinessGain}"
                };
            }
            else
            {
                int healthLoss = Random.Range(2, 5);
                character.Stats.ModifyStat(StatType.Health, -healthLoss);

                return new ActivityResult
                {
                    Success = false,
                    Message = $"Aşırı zorladın ve sakatlandın. Sağlık -{healthLoss}"
                };
            }
        }

        /// <summary>
        /// Meditasyon yap.
        /// </summary>
        public static ActivityResult Meditate(CharacterData character)
        {
            if (character.Age < 8)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Meditasyon için çok küçüksün."
                };
            }

            int happinessGain = Random.Range(3, 8);
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            // Zeka da artabilir
            if (Random.value < 0.4f)
            {
                int intelligenceGain = Random.Range(1, 3);
                character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);
            }

            return new ActivityResult
            {
                Success = true,
                Message = $"Huzurlu bir meditasyon seansı geçirdin. Mutluluk +{happinessGain}"
            };
        }

        /// <summary>
        /// Doktora git.
        /// </summary>
        public static ActivityResult VisitDoctor(CharacterData character)
        {
            float cost = character.Age >= 65 ? 100f : 500f; // Yaşlılara indirim

            if (character.Finances.CurrentMoney < cost)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Doktor ziyareti için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-cost, "Doktor ziyareti");

            int healthGain = Random.Range(5, 15);
            character.Stats.ModifyStat(StatType.Health, healthGain);

            return new ActivityResult
            {
                Success = true,
                Message = $"Doktor seni muayene etti ve tedavi uyguladı. Sağlık +{healthGain}"
            };
        }

        /// <summary>
        /// Yürüyüşe çık.
        /// </summary>
        public static ActivityResult GoForWalk(CharacterData character)
        {
            int healthGain = Random.Range(1, 4);
            int happinessGain = Random.Range(1, 4);

            character.Stats.ModifyStat(StatType.Health, healthGain);
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            return new ActivityResult
            {
                Success = true,
                Message = $"Güzel bir yürüyüş yaptın. Sağlık +{healthGain}, Mutluluk +{happinessGain}"
            };
        }

        #endregion

        #region Eğitim Aktiviteleri

        /// <summary>
        /// Kütüphaneye git.
        /// </summary>
        public static ActivityResult GoToLibrary(CharacterData character)
        {
            if (character.Age < 6)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Kütüphaneye gitmek için çok küçüksün."
                };
            }

            int intelligenceGain = Random.Range(2, 6);
            character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);

            // Bazen mutluluk da artar
            if (Random.value < 0.5f)
            {
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(1, 3));
            }

            return new ActivityResult
            {
                Success = true,
                Message = $"Kütüphanede kitap okudun. Zeka +{intelligenceGain}"
            };
        }

        /// <summary>
        /// Ders çalış.
        /// </summary>
        public static ActivityResult Study(CharacterData character)
        {
            if (character.Age < 6)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Ders çalışmak için çok küçüksün."
                };
            }

            int intelligenceGain = Random.Range(1, 5);
            character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);

            // Çalışma yorucu olabilir
            if (Random.value < 0.3f)
            {
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(1, 3));
            }

            return new ActivityResult
            {
                Success = true,
                Message = $"Ders çalıştın. Zeka +{intelligenceGain}"
            };
        }

        /// <summary>
        /// Online kurs al.
        /// </summary>
        public static ActivityResult TakeOnlineCourse(CharacterData character)
        {
            if (character.Age < 12)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Online kurs için çok küçüksün."
                };
            }

            float cost = 1000f;
            if (character.Finances.CurrentMoney < cost)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Online kurs için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-cost, "Online kurs");

            int intelligenceGain = Random.Range(5, 10);
            character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);

            return new ActivityResult
            {
                Success = true,
                Message = $"Online kurs tamamladın. Zeka +{intelligenceGain}"
            };
        }

        #endregion

        #region Eğlence Aktiviteleri

        /// <summary>
        /// Sinemaya git.
        /// </summary>
        public static ActivityResult GoToMovies(CharacterData character)
        {
            if (character.Age < 5)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Sinema için çok küçüksün."
                };
            }

            float cost = 150f;
            if (character.Finances.CurrentMoney < cost)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Sinema bileti için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-cost, "Sinema bileti");

            int happinessGain = Random.Range(5, 10);
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            return new ActivityResult
            {
                Success = true,
                Message = $"Harika bir film izledin. Mutluluk +{happinessGain}"
            };
        }

        /// <summary>
        /// Partiye git.
        /// </summary>
        public static ActivityResult GoToParty(CharacterData character)
        {
            if (character.Age < 14)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Parti için çok küçüksün."
                };
            }

            float successChance = 0.6f + (character.Stats.Appearance / 200f);
            bool success = Random.value < successChance;

            if (success)
            {
                int happinessGain = Random.Range(8, 15);
                character.Stats.ModifyStat(StatType.Happiness, happinessGain);

                // Şöhret artabilir
                if (Random.value < 0.2f)
                {
                    character.Stats.ModifyStat(StatType.Fame, Random.Range(1, 3));
                }

                return new ActivityResult
                {
                    Success = true,
                    Message = $"Harika bir parti! Mutluluk +{happinessGain}"
                };
            }
            else
            {
                int happinessLoss = Random.Range(3, 8);
                character.Stats.ModifyStat(StatType.Happiness, -happinessLoss);

                return new ActivityResult
                {
                    Success = false,
                    Message = $"Parti berbat geçti. Mutluluk -{happinessLoss}"
                };
            }
        }

        /// <summary>
        /// Tatile git.
        /// </summary>
        public static ActivityResult GoOnVacation(CharacterData character)
        {
            float cost = 5000f;
            if (character.Finances.CurrentMoney < cost)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Tatil için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-cost, "Tatil");

            int happinessGain = Random.Range(15, 25);
            int healthGain = Random.Range(5, 10);

            character.Stats.ModifyStat(StatType.Happiness, happinessGain);
            character.Stats.ModifyStat(StatType.Health, healthGain);

            return new ActivityResult
            {
                Success = true,
                Message = $"Harika bir tatil geçirdin! Mutluluk +{happinessGain}, Sağlık +{healthGain}"
            };
        }

        #endregion

        #region Sosyal Medya

        /// <summary>
        /// Sosyal medyada paylaşım yap.
        /// </summary>
        public static ActivityResult PostOnSocialMedia(CharacterData character)
        {
            if (character.Age < 13)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Sosyal medya için çok küçüksün."
                };
            }

            float viralChance = 0.05f + (character.Stats.Fame / 500f) + (character.Stats.Appearance / 300f);
            bool viral = Random.value < viralChance;

            if (viral)
            {
                int fameGain = Random.Range(10, 25);
                character.Stats.ModifyStat(StatType.Fame, fameGain);
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 10));

                return new ActivityResult
                {
                    Success = true,
                    Message = $"Paylaşımın viral oldu! Şöhret +{fameGain}"
                };
            }
            else
            {
                // Normal paylaşım
                if (Random.value < 0.7f)
                {
                    character.Stats.ModifyStat(StatType.Fame, Random.Range(1, 3));
                    return new ActivityResult
                    {
                        Success = true,
                        Message = "Paylaşımın biraz ilgi gördü."
                    };
                }
                else
                {
                    character.Stats.ModifyStat(StatType.Happiness, -Random.Range(2, 5));
                    return new ActivityResult
                    {
                        Success = false,
                        Message = "Paylaşımın kimsenin ilgisini çekmedi."
                    };
                }
            }
        }

        #endregion

        #region Güzellik/Bakım

        /// <summary>
        /// Kuaföre git.
        /// </summary>
        public static ActivityResult GoToHairdresser(CharacterData character)
        {
            if (character.Age < 5)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Kuaföre gitmek için çok küçüksün."
                };
            }

            float cost = 200f;
            if (character.Finances.CurrentMoney < cost)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Kuaför için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-cost, "Kuaför");

            int appearanceGain = Random.Range(2, 5);
            int happinessGain = Random.Range(2, 5);

            character.Stats.ModifyStat(StatType.Appearance, appearanceGain);
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            return new ActivityResult
            {
                Success = true,
                Message = $"Yeni bir saç modeli! Görünüş +{appearanceGain}, Mutluluk +{happinessGain}"
            };
        }

        /// <summary>
        /// Estetik ameliyat ol.
        /// </summary>
        public static ActivityResult GetPlasticSurgery(CharacterData character)
        {
            if (character.Age < 18)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Estetik ameliyat için 18 yaşından büyük olmalısın."
                };
            }

            float cost = 50000f;
            if (character.Finances.CurrentMoney < cost)
            {
                return new ActivityResult
                {
                    Success = false,
                    Message = "Estetik ameliyat için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-cost, "Estetik ameliyat");

            // Başarı şansı
            if (Random.value < 0.85f)
            {
                int appearanceGain = Random.Range(10, 20);
                character.Stats.ModifyStat(StatType.Appearance, appearanceGain);
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 10));

                return new ActivityResult
                {
                    Success = true,
                    Message = $"Ameliyat başarılı! Görünüş +{appearanceGain}"
                };
            }
            else
            {
                int appearanceLoss = Random.Range(5, 15);
                character.Stats.ModifyStat(StatType.Appearance, -appearanceLoss);
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(10, 20));
                character.Stats.ModifyStat(StatType.Health, -Random.Range(5, 10));

                return new ActivityResult
                {
                    Success = false,
                    Message = $"Ameliyat başarısız oldu! Görünüş -{appearanceLoss}"
                };
            }
        }

        #endregion

        /// <summary>
        /// Tüm aktivitelerin listesini al.
        /// </summary>
        public static List<Activity> GetAvailableActivities(CharacterData character)
        {
            var activities = new List<Activity>();

            // Sağlık
            activities.Add(new Activity
            {
                Id = "gym",
                Name = "Spor Salonu",
                Description = "Egzersiz yap ve formda kal",
                Category = ActivityCategory.Health,
                MinAge = 12,
                Cost = 500f
            });

            activities.Add(new Activity
            {
                Id = "meditate",
                Name = "Meditasyon",
                Description = "Zihnini dinlendir",
                Category = ActivityCategory.Health,
                MinAge = 8,
                Cost = 0f
            });

            activities.Add(new Activity
            {
                Id = "doctor",
                Name = "Doktora Git",
                Description = "Sağlık kontrolü yaptır",
                Category = ActivityCategory.Health,
                MinAge = 0,
                Cost = 500f
            });

            activities.Add(new Activity
            {
                Id = "walk",
                Name = "Yürüyüş",
                Description = "Temiz hava al",
                Category = ActivityCategory.Health,
                MinAge = 0,
                Cost = 0f
            });

            // Eğitim
            activities.Add(new Activity
            {
                Id = "library",
                Name = "Kütüphane",
                Description = "Kitap oku ve öğren",
                Category = ActivityCategory.Education,
                MinAge = 6,
                Cost = 0f
            });

            activities.Add(new Activity
            {
                Id = "study",
                Name = "Ders Çalış",
                Description = "Bilgini artır",
                Category = ActivityCategory.Education,
                MinAge = 6,
                Cost = 0f
            });

            activities.Add(new Activity
            {
                Id = "online_course",
                Name = "Online Kurs",
                Description = "Yeni beceriler kazan",
                Category = ActivityCategory.Education,
                MinAge = 12,
                Cost = 1000f
            });

            // Eğlence
            activities.Add(new Activity
            {
                Id = "movies",
                Name = "Sinema",
                Description = "Film izle",
                Category = ActivityCategory.Entertainment,
                MinAge = 5,
                Cost = 150f
            });

            activities.Add(new Activity
            {
                Id = "party",
                Name = "Parti",
                Description = "Eğlen ve sosyalleş",
                Category = ActivityCategory.Entertainment,
                MinAge = 14,
                Cost = 0f
            });

            activities.Add(new Activity
            {
                Id = "vacation",
                Name = "Tatil",
                Description = "Dinlen ve keyfini çıkar",
                Category = ActivityCategory.Entertainment,
                MinAge = 0,
                Cost = 5000f
            });

            // Sosyal Medya
            activities.Add(new Activity
            {
                Id = "social_media",
                Name = "Sosyal Medya",
                Description = "Paylaşım yap",
                Category = ActivityCategory.Social,
                MinAge = 13,
                Cost = 0f
            });

            // Güzellik
            activities.Add(new Activity
            {
                Id = "hairdresser",
                Name = "Kuaför",
                Description = "Saçını yaptır",
                Category = ActivityCategory.Beauty,
                MinAge = 5,
                Cost = 200f
            });

            activities.Add(new Activity
            {
                Id = "plastic_surgery",
                Name = "Estetik Ameliyat",
                Description = "Görünüşünü değiştir",
                Category = ActivityCategory.Beauty,
                MinAge = 18,
                Cost = 50000f
            });

            return activities;
        }

        /// <summary>
        /// Aktiviteyi ID'ye göre çalıştır.
        /// </summary>
        public static ActivityResult ExecuteActivity(string activityId, CharacterData character)
        {
            return activityId switch
            {
                "gym" => GoToGym(character),
                "meditate" => Meditate(character),
                "doctor" => VisitDoctor(character),
                "walk" => GoForWalk(character),
                "library" => GoToLibrary(character),
                "study" => Study(character),
                "online_course" => TakeOnlineCourse(character),
                "movies" => GoToMovies(character),
                "party" => GoToParty(character),
                "vacation" => GoOnVacation(character),
                "social_media" => PostOnSocialMedia(character),
                "hairdresser" => GoToHairdresser(character),
                "plastic_surgery" => GetPlasticSurgery(character),
                _ => new ActivityResult { Success = false, Message = "Bilinmeyen aktivite." }
            };
        }
    }

    /// <summary>
    /// Aktivite sonucu.
    /// </summary>
    public class ActivityResult
    {
        public bool Success;
        public string Message;
    }

    /// <summary>
    /// Aktivite tanımı.
    /// </summary>
    public class Activity
    {
        public string Id;
        public string Name;
        public string Description;
        public ActivityCategory Category;
        public int MinAge;
        public float Cost;
    }

    /// <summary>
    /// Aktivite kategorisi.
    /// </summary>
    public enum ActivityCategory
    {
        Health,
        Education,
        Entertainment,
        Social,
        Beauty,
        Career
    }
}
