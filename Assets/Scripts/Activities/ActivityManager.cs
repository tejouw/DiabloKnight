using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Activities
{
    /// <summary>
    /// Aktivite Yöneticisi - Oyuncu aktivitelerini yönetir.
    /// </summary>
    public class ActivityManager : Singleton<ActivityManager>
    {
        private List<ActivityDefinition> _allActivities;
        private Dictionary<ActivityType, ActivityDefinition> _activityLookup;

        // Günlük aktivite limiti
        private int _dailyActivitiesUsed = 0;
        private const int MAX_DAILY_ACTIVITIES = 3;

        #region Properties

        public int DailyActivitiesRemaining => MAX_DAILY_ACTIVITIES - _dailyActivitiesUsed;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            LoadActivities();

            // Yaş ilerlediğinde günlük limiti sıfırla
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);
        }

        private void LoadActivities()
        {
            _allActivities = DefaultActivities.GetAllActivities();
            _activityLookup = new Dictionary<ActivityType, ActivityDefinition>();

            foreach (var activity in _allActivities)
            {
                _activityLookup[activity.type] = activity;
            }

            Debug.Log($"[ActivityManager] Loaded {_allActivities.Count} activities.");
        }

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            // Günlük aktivite limitini sıfırla
            _dailyActivitiesUsed = 0;
        }

        #endregion

        #region Activity Execution

        /// <summary>
        /// Mevcut aktiviteleri al (yaş filtresine göre).
        /// </summary>
        public List<ActivityDefinition> GetAvailableActivities(int characterAge)
        {
            return _allActivities
                .Where(a => a.minAge <= characterAge)
                .OrderBy(a => a.category)
                .ThenBy(a => a.name)
                .ToList();
        }

        /// <summary>
        /// Kategoriye göre aktiviteleri al.
        /// </summary>
        public List<ActivityDefinition> GetActivitiesByCategory(ActivityCategory category, int characterAge)
        {
            return _allActivities
                .Where(a => a.category == category && a.minAge <= characterAge)
                .ToList();
        }

        /// <summary>
        /// Aktivite yap.
        /// </summary>
        public ActivityResult PerformActivity(ActivityType activityType)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null)
            {
                return new ActivityResult
                {
                    activityType = activityType,
                    success = false,
                    resultText = "Karakter bulunamadı!"
                };
            }

            // Aktivite tanımını al
            if (!_activityLookup.TryGetValue(activityType, out var activity))
            {
                return new ActivityResult
                {
                    activityType = activityType,
                    success = false,
                    resultText = "Aktivite bulunamadı!"
                };
            }

            // Yaş kontrolü
            if (character.Age < activity.minAge)
            {
                return new ActivityResult
                {
                    activityType = activityType,
                    success = false,
                    resultText = $"Bu aktivite için en az {activity.minAge} yaşında olmalısın."
                };
            }

            // Günlük limit kontrolü
            if (_dailyActivitiesUsed >= MAX_DAILY_ACTIVITIES)
            {
                return new ActivityResult
                {
                    activityType = activityType,
                    success = false,
                    resultText = "Bugün için aktivite limitine ulaştın."
                };
            }

            // Para kontrolü
            if (character.Finances.CurrentMoney < activity.cost)
            {
                return new ActivityResult
                {
                    activityType = activityType,
                    success = false,
                    resultText = $"Yeterli paran yok. ({activity.cost:N0} TL gerekli)"
                };
            }

            // Aktiviteyi gerçekleştir
            _dailyActivitiesUsed++;

            // Parayı düş
            if (activity.cost > 0)
            {
                character.Finances.ModifyMoney(-activity.cost, activity.name);
            }

            // Başarı kontrolü
            bool success = Random.value <= activity.successChance;
            var result = new ActivityResult
            {
                activityType = activityType,
                success = success,
                statChanges = new Dictionary<StatType, int>(),
                moneyChange = -activity.cost
            };

            if (success)
            {
                // Başarılı - pozitif etkileri uygula
                ApplyActivityEffects(character, activity, result);
                result.resultText = activity.successText;
            }
            else
            {
                // Başarısız - negatif etkileri uygula
                ApplyFailureEffects(character, activity, result);
                result.resultText = activity.failText;
            }

            // Event yayınla
            EventBus.Publish(new ActivityCompletedEvent
            {
                ActivityType = activityType,
                Success = success,
                ResultText = result.resultText
            });

            return result;
        }

        /// <summary>
        /// Kumar oyna.
        /// </summary>
        public GamblingResult PlayGambling(ActivityType gamblingType, int betAmount)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null)
            {
                return new GamblingResult
                {
                    isWin = false,
                    resultText = "Karakter bulunamadı!"
                };
            }

            // Yaş kontrolü
            if (character.Age < 18)
            {
                return new GamblingResult
                {
                    isWin = false,
                    resultText = "Kumar için 18 yaşından büyük olmalısın!"
                };
            }

            // Para kontrolü
            if (character.Finances.CurrentMoney < betAmount)
            {
                return new GamblingResult
                {
                    isWin = false,
                    resultText = "Yeterli paran yok!"
                };
            }

            var result = new GamblingResult
            {
                betAmount = betAmount,
                gameName = GetGamblingGameName(gamblingType)
            };

            // Kazanma şansını ve çarpanı belirle
            float winChance;
            float multiplier;

            switch (gamblingType)
            {
                case ActivityType.Lottery:
                    winChance = 0.001f; // %0.1 şans
                    multiplier = 10000f; // Dev ikramiye
                    break;
                case ActivityType.Blackjack:
                    winChance = 0.45f;
                    multiplier = 2f;
                    break;
                case ActivityType.Casino:
                default:
                    winChance = 0.35f;
                    multiplier = 2.5f;
                    break;
            }

            // Bahsi al
            character.Finances.ModifyMoney(-betAmount, $"{result.gameName} bahsi");

            // Şans kontrolü
            result.isWin = Random.value <= winChance;

            if (result.isWin)
            {
                result.winAmount = (int)(betAmount * multiplier);
                character.Finances.ModifyMoney(result.winAmount, $"{result.gameName} kazancı");
                character.Stats.ModifyStat(StatType.Happiness, 10);

                if (gamblingType == ActivityType.Lottery && result.winAmount > 100000)
                {
                    result.resultText = $"BÜYÜK İKRAMİYE! {result.winAmount:N0} TL kazandın!";
                    character.Stats.ModifyStat(StatType.Fame, 5);
                }
                else
                {
                    result.resultText = $"Kazandın! +{result.winAmount:N0} TL";
                }
            }
            else
            {
                result.winAmount = 0;
                character.Stats.ModifyStat(StatType.Happiness, -3);
                result.resultText = $"Kaybettin! -{betAmount:N0} TL";
            }

            return result;
        }

        #endregion

        #region Private Methods

        private void ApplyActivityEffects(CharacterData character, ActivityDefinition activity, ActivityResult result)
        {
            if (activity.healthEffect != 0)
            {
                character.Stats.ModifyStat(StatType.Health, activity.healthEffect);
                result.statChanges[StatType.Health] = activity.healthEffect;
            }

            if (activity.happinessEffect != 0)
            {
                character.Stats.ModifyStat(StatType.Happiness, activity.happinessEffect);
                result.statChanges[StatType.Happiness] = activity.happinessEffect;
            }

            if (activity.intelligenceEffect != 0)
            {
                character.Stats.ModifyStat(StatType.Intelligence, activity.intelligenceEffect);
                result.statChanges[StatType.Intelligence] = activity.intelligenceEffect;
            }

            if (activity.appearanceEffect != 0)
            {
                character.Stats.ModifyStat(StatType.Appearance, activity.appearanceEffect);
                result.statChanges[StatType.Appearance] = activity.appearanceEffect;
            }

            if (activity.fameEffect != 0)
            {
                character.Stats.ModifyStat(StatType.Fame, activity.fameEffect);
                result.statChanges[StatType.Fame] = activity.fameEffect;
            }
        }

        private void ApplyFailureEffects(CharacterData character, ActivityDefinition activity, ActivityResult result)
        {
            // Başarısızlık durumunda olumsuz etkiler
            int negativeHealth = Mathf.Min(0, -Mathf.Abs(activity.healthEffect) / 2);
            int negativeHappiness = Mathf.Min(0, -Mathf.Abs(activity.happinessEffect) / 2 - 2);

            if (negativeHealth < 0)
            {
                character.Stats.ModifyStat(StatType.Health, negativeHealth);
                result.statChanges[StatType.Health] = negativeHealth;
            }

            character.Stats.ModifyStat(StatType.Happiness, negativeHappiness);
            result.statChanges[StatType.Happiness] = negativeHappiness;
        }

        private string GetGamblingGameName(ActivityType type)
        {
            return type switch
            {
                ActivityType.Lottery => "Milli Piyango",
                ActivityType.Blackjack => "Blackjack",
                ActivityType.Casino => "Slot Makinesi",
                _ => "Kumar"
            };
        }

        #endregion

        #region Utility

        /// <summary>
        /// Aktivite tanımını al.
        /// </summary>
        public ActivityDefinition GetActivityDefinition(ActivityType type)
        {
            return _activityLookup.TryGetValue(type, out var activity) ? activity : null;
        }

        /// <summary>
        /// Günlük aktivite sayısını sıfırla.
        /// </summary>
        public void ResetDailyActivities()
        {
            _dailyActivitiesUsed = 0;
        }

        #endregion
    }

    /// <summary>
    /// Aktivite tamamlandı eventi.
    /// </summary>
    public struct ActivityCompletedEvent : IGameEvent
    {
        public ActivityType ActivityType;
        public bool Success;
        public string ResultText;
    }
}
