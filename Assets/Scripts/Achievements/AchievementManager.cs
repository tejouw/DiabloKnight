using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Achievements
{
    /// <summary>
    /// Başarı Yöneticisi - Oyun başarılarını izler ve ödüllendirir.
    /// </summary>
    public class AchievementManager : Singleton<AchievementManager>
    {
        private List<Achievement> _achievements = new List<Achievement>();
        private HashSet<string> _unlockedAchievements = new HashSet<string>();

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeAchievements();
        }

        private void InitializeAchievements()
        {
            _achievements = new List<Achievement>
            {
                // Yaşam Başarıları
                new Achievement("centenarian", "Yüzyılın İnsanı", "100 yaşına ulaş", AchievementCategory.Lifetime),
                new Achievement("first_steps", "İlk Adımlar", "İlk oyunu tamamla", AchievementCategory.Lifetime),
                new Achievement("successful_life", "Başarılı Hayat", "Tüm statları 80'in üzerinde bitir", AchievementCategory.Lifetime),
                new Achievement("perfect_life", "Mükemmel Hayat", "Tüm statları 100'de bitir", AchievementCategory.Lifetime),
                new Achievement("long_marriage", "Altın Yıldönümü", "50 yıl evli kal", AchievementCategory.Lifetime),

                // Kariyer Başarıları
                new Achievement("ceo", "CEO", "Şirket CEO'su ol", AchievementCategory.Career),
                new Achievement("retired_rich", "Zengin Emekli", "1 milyonun üzerinde parayla emekli ol", AchievementCategory.Career),
                new Achievement("job_hopper", "İş Cambazı", "10 farklı iş değiştir", AchievementCategory.Career),
                new Achievement("entrepreneur", "Girişimci", "Kendi şirketini kur", AchievementCategory.Career),
                new Achievement("doctor", "Doktor", "Tıp fakültesini bitir", AchievementCategory.Career),
                new Achievement("lawyer", "Avukat", "Hukuk fakültesini bitir", AchievementCategory.Career),

                // Eğitim Başarıları
                new Achievement("phd", "Akademisyen", "Doktora yap", AchievementCategory.Education),
                new Achievement("honor_student", "Onur Öğrencisi", "4.0 GPA ile mezun ol", AchievementCategory.Education),
                new Achievement("dropout", "Okul Bırakan", "Okulu bırak", AchievementCategory.Education),
                new Achievement("valedictorian", "Birinci", "Sınıf birincisi ol", AchievementCategory.Education),

                // Sosyal Başarıları
                new Achievement("social_butterfly", "Sosyal Kelebek", "50 arkadaş edin", AchievementCategory.Social),
                new Achievement("popular", "Popüler", "Fame 100'e ulaş", AchievementCategory.Social),
                new Achievement("influencer", "Fenomen", "1 milyon takipçiye ulaş", AchievementCategory.Social),
                new Achievement("lone_wolf", "Yalnız Kurt", "Hiç arkadaş edinmeden 50'ye ulaş", AchievementCategory.Social),

                // Aile Başarıları
                new Achievement("big_family", "Kalabalık Aile", "5 çocuk sahibi ol", AchievementCategory.Family),
                new Achievement("grandparent", "Dede/Nine", "Torun sahibi ol", AchievementCategory.Family),
                new Achievement("family_person", "Aile İnsanı", "Tüm aile ilişkilerini 90'ın üzerinde tut", AchievementCategory.Family),
                new Achievement("adopted", "Evlat Edinen", "Bir çocuk evlat edin", AchievementCategory.Family),

                // Para Başarıları
                new Achievement("millionaire", "Milyoner", "1 milyon TL biriktir", AchievementCategory.Financial),
                new Achievement("billionaire", "Milyarder", "1 milyar TL biriktir", AchievementCategory.Financial),
                new Achievement("generous", "Cömert", "100.000 TL bağışla", AchievementCategory.Financial),
                new Achievement("bankrupt", "İflas", "Tüm paranı kaybet", AchievementCategory.Financial),
                new Achievement("lottery_winner", "Şanslı", "Piyango kazan", AchievementCategory.Financial),

                // Suç Başarıları
                new Achievement("crime_lord", "Suç Baronu", "10 suç işle ve yakalanma", AchievementCategory.Crime),
                new Achievement("escaped", "Kaçak", "Hapisten kaç", AchievementCategory.Crime),
                new Achievement("clean_record", "Temiz Sicil", "Hiç suç işlemeden 60'a ulaş", AchievementCategory.Crime),
                new Achievement("reformed", "Islah Olmuş", "Hapisten çık ve bir daha suç işleme", AchievementCategory.Crime),

                // Sağlık Başarıları
                new Achievement("healthy", "Sağlıklı", "Hiç hastalanmadan 50'ye ulaş", AchievementCategory.Health),
                new Achievement("fitness_freak", "Spor Hastası", "100 kez spor yap", AchievementCategory.Health),
                new Achievement("survivor", "Hayatta Kalan", "Terminal hastalıktan kurtul", AchievementCategory.Health),

                // Askerlik Başarıları
                new Achievement("veteran", "Gazi", "Askerliği şerefle tamamla", AchievementCategory.Military),
                new Achievement("general", "General", "General rütbesine ulaş", AchievementCategory.Military),
                new Achievement("decorated", "Madalyalı", "3 madalya kazan", AchievementCategory.Military),

                // Özel Türk Başarıları
                new Achievement("true_turk", "Gerçek Türk", "YKS'de ilk 1000'e gir", AchievementCategory.Special),
                new Achievement("kurban", "Kurban", "Her yıl kurban kes", AchievementCategory.Special),
                new Achievement("hac", "Hacı", "Hacca git", AchievementCategory.Special),

                // Mülk Başarıları
                new Achievement("homeowner", "Ev Sahibi", "İlk evini al", AchievementCategory.Property),
                new Achievement("real_estate_mogul", "Emlak Kralı", "5 mülk sahibi ol", AchievementCategory.Property),
                new Achievement("car_collector", "Koleksiyoncu", "10 araba sahibi ol", AchievementCategory.Property),
                new Achievement("yacht_owner", "Yat Sahibi", "Yat al", AchievementCategory.Property),

                // Romantik Başarıları
                new Achievement("true_love", "Gerçek Aşk", "İlk sevgilinle evlen", AchievementCategory.Romance),
                new Achievement("heartbreaker", "Kalp Kıran", "10 ilişki bitir", AchievementCategory.Romance),
                new Achievement("divorced", "Boşanmış", "Boşan", AchievementCategory.Romance),
                new Achievement("golden_anniversary", "50. Yıl", "50 yıllık evliliği kutla", AchievementCategory.Romance)
            };
        }

        /// <summary>
        /// Başarı kilidini aç.
        /// </summary>
        public bool UnlockAchievement(string achievementId)
        {
            if (_unlockedAchievements.Contains(achievementId))
            {
                return false; // Zaten açık
            }

            var achievement = _achievements.FirstOrDefault(a => a.Id == achievementId);
            if (achievement == null)
            {
                return false;
            }

            _unlockedAchievements.Add(achievementId);

            EventBus.Publish(new AchievementUnlockedEvent
            {
                AchievementId = achievementId,
                AchievementName = achievement.Name,
                Description = achievement.Description
            });

            Debug.Log($"[AchievementManager] Achievement unlocked: {achievement.Name}");
            return true;
        }

        /// <summary>
        /// Karakter durumuna göre başarıları kontrol et.
        /// </summary>
        public void CheckAchievements(CharacterData character)
        {
            // Yaş başarıları
            if (character.Age >= 100)
            {
                UnlockAchievement("centenarian");
            }

            // Para başarıları
            if (character.Finances.CurrentMoney >= 1000000)
            {
                UnlockAchievement("millionaire");
            }
            if (character.Finances.CurrentMoney >= 1000000000)
            {
                UnlockAchievement("billionaire");
            }

            // Stat başarıları
            if (character.Stats.Fame >= 100)
            {
                UnlockAchievement("popular");
            }

            // Kariyer başarıları
            if (character.Career.jobHistory.Count >= 10)
            {
                UnlockAchievement("job_hopper");
            }

            // Sosyal başarıları
            int friendCount = character.Relationships.Count(r =>
                r.type == RelationType.Friend || r.type == RelationType.BestFriend);
            if (friendCount >= 50)
            {
                UnlockAchievement("social_butterfly");
            }
            if (friendCount == 0 && character.Age >= 50)
            {
                UnlockAchievement("lone_wolf");
            }

            // Aile başarıları
            int childCount = character.Relationships.Count(r => r.type == RelationType.Child);
            if (childCount >= 5)
            {
                UnlockAchievement("big_family");
            }

            // Mülk başarıları
            if (character.Properties.ownedProperties.Count >= 1)
            {
                UnlockAchievement("homeowner");
            }
            if (character.Properties.ownedProperties.Count >= 5)
            {
                UnlockAchievement("real_estate_mogul");
            }
            if (character.Properties.ownedVehicles.Count >= 10)
            {
                UnlockAchievement("car_collector");
            }

            // Suç başarıları
            int undetectedCrimes = character.Crime.crimeHistory.Count(c => !c.wasCaught);
            if (undetectedCrimes >= 10)
            {
                UnlockAchievement("crime_lord");
            }
            if (character.Age >= 60 && character.Crime.crimeHistory.Count == 0)
            {
                UnlockAchievement("clean_record");
            }

            // Askerlik başarıları
            if (character.Military.status == MilitaryStatus.Completed)
            {
                UnlockAchievement("veteran");
            }
            if (character.Military.rank == MilitaryRank.General)
            {
                UnlockAchievement("general");
            }
            if (character.Military.medals.Count >= 3)
            {
                UnlockAchievement("decorated");
            }

            // Eğitim başarıları
            if (character.Education.currentLevel == EducationLevel.Doctorate)
            {
                UnlockAchievement("phd");
            }
            if (character.Education.gpa >= 4.0f && character.Education.isGraduated)
            {
                UnlockAchievement("honor_student");
            }

            // Sosyal medya başarıları
            if (character.SocialMedia.totalFollowers >= 1000000)
            {
                UnlockAchievement("influencer");
            }
        }

        /// <summary>
        /// Tüm başarıları al.
        /// </summary>
        public List<Achievement> GetAllAchievements()
        {
            return _achievements;
        }

        /// <summary>
        /// Açılmış başarıları al.
        /// </summary>
        public List<Achievement> GetUnlockedAchievements()
        {
            return _achievements.Where(a => _unlockedAchievements.Contains(a.Id)).ToList();
        }

        /// <summary>
        /// Başarı yüzdesini al.
        /// </summary>
        public float GetCompletionPercentage()
        {
            if (_achievements.Count == 0) return 0;
            return (float)_unlockedAchievements.Count / _achievements.Count * 100f;
        }

        /// <summary>
        /// Başarıları sıfırla.
        /// </summary>
        public void ResetAchievements()
        {
            _unlockedAchievements.Clear();
        }
    }

    /// <summary>
    /// Başarı tanımı.
    /// </summary>
    [System.Serializable]
    public class Achievement
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public AchievementCategory Category { get; private set; }

        public Achievement(string id, string name, string description, AchievementCategory category)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
        }
    }

    /// <summary>
    /// Başarı kategorisi.
    /// </summary>
    public enum AchievementCategory
    {
        Lifetime,
        Career,
        Education,
        Social,
        Family,
        Financial,
        Crime,
        Health,
        Military,
        Property,
        Romance,
        Special
    }

    /// <summary>
    /// Başarı açıldı eventi.
    /// </summary>
    public class AchievementUnlockedEvent
    {
        public string AchievementId;
        public string AchievementName;
        public string Description;
    }
}
