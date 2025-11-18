using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Aktiviteler Sistemi - Spor, sosyal, eğlence aktiviteleri.
    /// </summary>
    public class ActivitiesSystem : Singleton<ActivitiesSystem>
    {
        private List<ActivityDefinition> _activities = new List<ActivityDefinition>();
        private Dictionary<string, List<ActivityDefinition>> _activitiesByCategory = new Dictionary<string, List<ActivityDefinition>>();

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeActivities();
        }

        private void InitializeActivities()
        {
            _activities = new List<ActivityDefinition>
            {
                // Sağlık & Fitness
                new ActivityDefinition("gym", "Spor Salonu", "Sağlık", 100, 16,
                    new StatEffect[] {
                        new StatEffect(StatType.Health, 3, 8),
                        new StatEffect(StatType.Appearance, 1, 3),
                        new StatEffect(StatType.Happiness, 2, 5)
                    }),
                new ActivityDefinition("yoga", "Yoga", "Sağlık", 150, 14,
                    new StatEffect[] {
                        new StatEffect(StatType.Health, 2, 5),
                        new StatEffect(StatType.Happiness, 3, 7)
                    }),
                new ActivityDefinition("swimming", "Yüzme", "Sağlık", 80, 6,
                    new StatEffect[] {
                        new StatEffect(StatType.Health, 3, 6),
                        new StatEffect(StatType.Happiness, 2, 5)
                    }),
                new ActivityDefinition("martial_arts", "Dövüş Sanatları", "Sağlık", 200, 10,
                    new StatEffect[] {
                        new StatEffect(StatType.Health, 4, 8),
                        new StatEffect(StatType.Happiness, 2, 4)
                    }),
                new ActivityDefinition("jogging", "Koşu", "Sağlık", 0, 10,
                    new StatEffect[] {
                        new StatEffect(StatType.Health, 2, 5),
                        new StatEffect(StatType.Happiness, 1, 3)
                    }),
                new ActivityDefinition("doctor", "Doktora Git", "Sağlık", 500, 0,
                    new StatEffect[] {
                        new StatEffect(StatType.Health, 5, 15)
                    }),
                new ActivityDefinition("meditation", "Meditasyon", "Sağlık", 0, 12,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 3, 8),
                        new StatEffect(StatType.Health, 1, 3)
                    }),
                new ActivityDefinition("diet", "Diyet Yap", "Sağlık", 200, 14,
                    new StatEffect[] {
                        new StatEffect(StatType.Health, 2, 6),
                        new StatEffect(StatType.Appearance, 1, 4),
                        new StatEffect(StatType.Happiness, -3, -1)
                    }),

                // Eğitim & Gelişim
                new ActivityDefinition("library", "Kütüphane", "Eğitim", 0, 6,
                    new StatEffect[] {
                        new StatEffect(StatType.Intelligence, 2, 5),
                        new StatEffect(StatType.Happiness, 0, 2)
                    }),
                new ActivityDefinition("online_course", "Online Kurs", "Eğitim", 300, 14,
                    new StatEffect[] {
                        new StatEffect(StatType.Intelligence, 3, 7)
                    }),
                new ActivityDefinition("language_course", "Dil Kursu", "Eğitim", 500, 10,
                    new StatEffect[] {
                        new StatEffect(StatType.Intelligence, 2, 5)
                    }),
                new ActivityDefinition("music_lesson", "Müzik Dersi", "Eğitim", 400, 6,
                    new StatEffect[] {
                        new StatEffect(StatType.Intelligence, 1, 3),
                        new StatEffect(StatType.Happiness, 2, 5)
                    }),
                new ActivityDefinition("art_class", "Resim Kursu", "Eğitim", 350, 8,
                    new StatEffect[] {
                        new StatEffect(StatType.Intelligence, 1, 3),
                        new StatEffect(StatType.Happiness, 2, 4)
                    }),

                // Sosyal
                new ActivityDefinition("cafe", "Kafeye Git", "Sosyal", 50, 14,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 2, 5)
                    }),
                new ActivityDefinition("restaurant", "Restorana Git", "Sosyal", 200, 0,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 3, 7),
                        new StatEffect(StatType.Health, -1, 1)
                    }),
                new ActivityDefinition("park", "Parka Git", "Sosyal", 0, 0,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 2, 5),
                        new StatEffect(StatType.Health, 1, 3)
                    }),
                new ActivityDefinition("shopping", "Alışveriş", "Sosyal", 500, 12,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 3, 8),
                        new StatEffect(StatType.Appearance, 1, 3)
                    }),
                new ActivityDefinition("volunteer", "Gönüllü Çalışma", "Sosyal", 0, 14,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 4, 8),
                        new StatEffect(StatType.Fame, 1, 3)
                    }),

                // Eğlence
                new ActivityDefinition("cinema", "Sinemaya Git", "Eğlence", 100, 8,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 3, 7)
                    }),
                new ActivityDefinition("concert", "Konsere Git", "Eğlence", 300, 14,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 5, 10)
                    }),
                new ActivityDefinition("theater", "Tiyatroya Git", "Eğlence", 150, 12,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 3, 6),
                        new StatEffect(StatType.Intelligence, 1, 2)
                    }),
                new ActivityDefinition("museum", "Müzeye Git", "Eğlence", 50, 6,
                    new StatEffect[] {
                        new StatEffect(StatType.Intelligence, 2, 4),
                        new StatEffect(StatType.Happiness, 1, 3)
                    }),
                new ActivityDefinition("amusement_park", "Lunaparka Git", "Eğlence", 200, 6,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 5, 10)
                    }),
                new ActivityDefinition("video_games", "Video Oyunu Oyna", "Eğlence", 0, 8,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 2, 5),
                        new StatEffect(StatType.Health, -1, 0)
                    }),

                // Gece Hayatı
                new ActivityDefinition("bar", "Bara Git", "Gece Hayatı", 150, 18,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 3, 8),
                        new StatEffect(StatType.Health, -2, -1)
                    }),
                new ActivityDefinition("nightclub", "Gece Kulübü", "Gece Hayatı", 300, 18,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 5, 12),
                        new StatEffect(StatType.Health, -3, -1)
                    }),
                new ActivityDefinition("party", "Partiye Git", "Gece Hayatı", 100, 16,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 4, 10),
                        new StatEffect(StatType.Health, -2, 0)
                    }),

                // Tatil & Seyahat
                new ActivityDefinition("vacation_local", "Yurt İçi Tatil", "Seyahat", 5000, 0,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 10, 20),
                        new StatEffect(StatType.Health, 3, 8)
                    }),
                new ActivityDefinition("vacation_abroad", "Yurt Dışı Tatil", "Seyahat", 15000, 18,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 15, 25),
                        new StatEffect(StatType.Health, 5, 10),
                        new StatEffect(StatType.Intelligence, 2, 5)
                    }),
                new ActivityDefinition("spa", "Spa & Hamam", "Seyahat", 500, 16,
                    new StatEffect[] {
                        new StatEffect(StatType.Happiness, 5, 10),
                        new StatEffect(StatType.Health, 3, 7),
                        new StatEffect(StatType.Appearance, 1, 3)
                    }),

                // Güzellik & Bakım
                new ActivityDefinition("haircut", "Kuaför", "Güzellik", 100, 12,
                    new StatEffect[] {
                        new StatEffect(StatType.Appearance, 2, 5),
                        new StatEffect(StatType.Happiness, 1, 3)
                    }),
                new ActivityDefinition("salon", "Güzellik Salonu", "Güzellik", 300, 16,
                    new StatEffect[] {
                        new StatEffect(StatType.Appearance, 3, 7),
                        new StatEffect(StatType.Happiness, 2, 5)
                    }),
                new ActivityDefinition("plastic_surgery", "Estetik Ameliyat", "Güzellik", 20000, 18,
                    new StatEffect[] {
                        new StatEffect(StatType.Appearance, 10, 20),
                        new StatEffect(StatType.Health, -5, -2)
                    }),

                // Kumar & Risk
                new ActivityDefinition("lottery", "Piyango", "Kumar", 50, 18,
                    new StatEffect[] { }, true),
                new ActivityDefinition("casino", "Kumarhane", "Kumar", 500, 21,
                    new StatEffect[] { }, true),
                new ActivityDefinition("bet", "Bahis", "Kumar", 100, 18,
                    new StatEffect[] { }, true),
            };

            // Kategorilere ayır
            foreach (var activity in _activities)
            {
                if (!_activitiesByCategory.ContainsKey(activity.category))
                {
                    _activitiesByCategory[activity.category] = new List<ActivityDefinition>();
                }
                _activitiesByCategory[activity.category].Add(activity);
            }

            Debug.Log($"[ActivitiesSystem] Loaded {_activities.Count} activities in {_activitiesByCategory.Count} categories.");
        }

        #endregion

        #region Activity Execution

        /// <summary>
        /// Aktivite yap.
        /// </summary>
        public ActivityResult DoActivity(CharacterData character, string activityId)
        {
            var activity = _activities.Find(a => a.id == activityId);
            if (activity == null)
            {
                return new ActivityResult
                {
                    success = false,
                    message = "Aktivite bulunamadı."
                };
            }

            // Yaş kontrolü
            if (character.Age < activity.minAge)
            {
                return new ActivityResult
                {
                    success = false,
                    message = $"Bu aktivite için en az {activity.minAge} yaşında olmalısın."
                };
            }

            // Para kontrolü
            if (character.Finances.CurrentMoney < activity.cost)
            {
                return new ActivityResult
                {
                    success = false,
                    message = $"Yeterli paran yok. Gerekli: {activity.cost:N0} TL"
                };
            }

            // Parayı düş
            if (activity.cost > 0)
            {
                character.Finances.ModifyMoney(-activity.cost, activity.name);
            }

            // Kumar aktiviteleri için özel işlem
            if (activity.isGambling)
            {
                return ProcessGambling(character, activity);
            }

            // Stat efektlerini uygula
            string effectMessage = "";
            foreach (var effect in activity.effects)
            {
                int change = Random.Range(effect.minChange, effect.maxChange + 1);
                character.Stats.ModifyStat(effect.statType, change);

                string sign = change >= 0 ? "+" : "";
                effectMessage += $"\n{effect.statType}: {sign}{change}";
            }

            // Rastgele bonus olaylar
            string bonusMessage = CheckForBonusEvent(character, activity);

            return new ActivityResult
            {
                success = true,
                message = $"{activity.name} yaptın!{effectMessage}{bonusMessage}"
            };
        }

        /// <summary>
        /// Kumar aktivitelerini işle.
        /// </summary>
        private ActivityResult ProcessGambling(CharacterData character, ActivityDefinition activity)
        {
            float winChance;
            decimal potentialWin;

            switch (activity.id)
            {
                case "lottery":
                    winChance = 0.001f; // Çok düşük
                    potentialWin = 1000000m; // Büyük ikramiye
                    break;
                case "casino":
                    winChance = 0.4f;
                    potentialWin = activity.cost * 3;
                    break;
                case "bet":
                    winChance = 0.35f;
                    potentialWin = activity.cost * 2.5m;
                    break;
                default:
                    winChance = 0.3f;
                    potentialWin = activity.cost * 2;
                    break;
            }

            if (Random.value < winChance)
            {
                character.Finances.ModifyMoney(potentialWin, $"{activity.name} kazancı");
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 15));

                return new ActivityResult
                {
                    success = true,
                    message = $"Tebrikler! {potentialWin:N0} TL kazandın!"
                };
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(-5, -1));

                return new ActivityResult
                {
                    success = true,
                    message = "Kaybettin. Belki bir dahaki sefere şansın döner."
                };
            }
        }

        /// <summary>
        /// Bonus olay kontrolü.
        /// </summary>
        private string CheckForBonusEvent(CharacterData character, ActivityDefinition activity)
        {
            // %10 şansla bonus olay
            if (Random.value > 0.1f)
                return "";

            switch (activity.category)
            {
                case "Sosyal":
                    if (Random.value < 0.5f)
                    {
                        character.Stats.ModifyStat(StatType.Fame, 1);
                        return "\nBirisi seni tanıdı! (+1 Şöhret)";
                    }
                    break;

                case "Sağlık":
                    if (Random.value < 0.3f)
                    {
                        character.Stats.ModifyStat(StatType.Appearance, 2);
                        return "\nKendinle gurur duyuyorsun! (+2 Görünüş)";
                    }
                    break;

                case "Eğlence":
                    if (Random.value < 0.5f)
                    {
                        decimal foundMoney = Random.Range(10, 100);
                        character.Finances.ModifyMoney(foundMoney, "Yerde bulduğun para");
                        return $"\nYerde {foundMoney} TL buldun!";
                    }
                    break;
            }

            return "";
        }

        #endregion

        #region Utilities

        public List<ActivityDefinition> GetActivitiesByCategory(string category)
        {
            if (_activitiesByCategory.ContainsKey(category))
                return _activitiesByCategory[category];
            return new List<ActivityDefinition>();
        }

        public List<string> GetAllCategories()
        {
            return new List<string>(_activitiesByCategory.Keys);
        }

        public List<ActivityDefinition> GetAvailableActivities(CharacterData character)
        {
            return _activities.FindAll(a => character.Age >= a.minAge && character.Finances.CurrentMoney >= a.cost);
        }

        #endregion
    }

    #region Data Classes

    [System.Serializable]
    public class ActivityDefinition
    {
        public string id;
        public string name;
        public string category;
        public decimal cost;
        public int minAge;
        public StatEffect[] effects;
        public bool isGambling;

        public ActivityDefinition(string id, string name, string category, decimal cost, int minAge, StatEffect[] effects, bool isGambling = false)
        {
            this.id = id;
            this.name = name;
            this.category = category;
            this.cost = cost;
            this.minAge = minAge;
            this.effects = effects;
            this.isGambling = isGambling;
        }
    }

    [System.Serializable]
    public class StatEffect
    {
        public StatType statType;
        public int minChange;
        public int maxChange;

        public StatEffect(StatType statType, int minChange, int maxChange)
        {
            this.statType = statType;
            this.minChange = minChange;
            this.maxChange = maxChange;
        }
    }

    public class ActivityResult
    {
        public bool success;
        public string message;
    }

    #endregion
}
