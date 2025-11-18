using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Events;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Olay Yöneticisi - Oyun olaylarını yükler, seçer ve işler.
    /// </summary>
    public class EventManager : Singleton<EventManager>
    {
        // Yüklenen tüm olaylar
        private List<GameEvent> _allEvents = new List<GameEvent>();

        // Kategori bazlı olay cache'i
        private Dictionary<EventCategory, List<GameEvent>> _eventsByCategory = new Dictionary<EventCategory, List<GameEvent>>();

        // Mevcut aktif olay
        private GameEvent _currentEvent;

        // Olay geçmişi (tekrarları önlemek için)
        private HashSet<string> _recentEventIds = new HashSet<string>();
        private const int MAX_RECENT_EVENTS = 20;

        #region Properties

        public GameEvent CurrentEvent => _currentEvent;
        public int TotalEventCount => _allEvents.Count;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            LoadAllEvents();
        }

        /// <summary>
        /// Tüm olayları Resources'tan yükle.
        /// </summary>
        private void LoadAllEvents()
        {
            // JSON dosyalarından yükle
            var eventFiles = Resources.LoadAll<TextAsset>("Events");

            foreach (var file in eventFiles)
            {
                try
                {
                    var eventData = JsonUtility.FromJson<GameEventCollection>(file.text);
                    if (eventData != null && eventData.events != null)
                    {
                        _allEvents.AddRange(eventData.events);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[EventManager] Error loading event file {file.name}: {e.Message}");
                }
            }

            // Eğer dosya yoksa varsayılan olayları ekle
            if (_allEvents.Count == 0)
            {
                LoadDefaultEvents();
            }

            // Kategorilere ayır
            CategorizeEvents();

            Debug.Log($"[EventManager] Loaded {_allEvents.Count} events.");
        }

        /// <summary>
        /// Varsayılan olayları yükle (JSON dosyası yoksa).
        /// </summary>
        private void LoadDefaultEvents()
        {
            _allEvents = DefaultEvents.GetAllEvents();
        }

        /// <summary>
        /// Olayları kategorilere ayır.
        /// </summary>
        private void CategorizeEvents()
        {
            _eventsByCategory.Clear();

            foreach (EventCategory category in System.Enum.GetValues(typeof(EventCategory)))
            {
                _eventsByCategory[category] = new List<GameEvent>();
            }

            foreach (var evt in _allEvents)
            {
                if (_eventsByCategory.ContainsKey(evt.category))
                {
                    _eventsByCategory[evt.category].Add(evt);
                }
            }
        }

        #endregion

        #region Event Selection

        /// <summary>
        /// Sonraki olayı tetikle.
        /// </summary>
        public void TriggerNextEvent()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null)
            {
                Debug.LogError("[EventManager] No active character!");
                return;
            }

            // Uygun bir olay seç
            _currentEvent = SelectRandomEvent(character);

            if (_currentEvent != null)
            {
                // Event bus'a yayınla
                EventBus.Publish(new EventStartedEvent
                {
                    EventId = _currentEvent.id,
                    EventTitle = _currentEvent.title
                });

                Debug.Log($"[EventManager] Triggered event: {_currentEvent.title}");
            }
            else
            {
                // Uygun olay bulunamadı, jenerik olay oluştur
                _currentEvent = GenerateGenericEvent(character);
            }

            // UI'ı güncelle
            UIManager.Instance?.RefreshUI();
        }

        /// <summary>
        /// Karaktere uygun rastgele bir olay seç.
        /// </summary>
        private GameEvent SelectRandomEvent(CharacterData character)
        {
            int age = character.Age;

            // Yaşa uygun olayları filtrele
            var eligibleEvents = _allEvents.Where(e =>
                e.ageRange.min <= age &&
                e.ageRange.max >= age &&
                !_recentEventIds.Contains(e.id) &&
                CheckEventConditions(e, character)
            ).ToList();

            if (eligibleEvents.Count == 0)
            {
                return null;
            }

            // Olasılıklara göre ağırlıklı seçim
            float totalWeight = eligibleEvents.Sum(e => e.probability);
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var evt in eligibleEvents)
            {
                currentWeight += evt.probability;
                if (randomValue <= currentWeight)
                {
                    AddToRecentEvents(evt.id);
                    return evt;
                }
            }

            // Fallback
            var selectedEvent = eligibleEvents[Random.Range(0, eligibleEvents.Count)];
            AddToRecentEvents(selectedEvent.id);
            return selectedEvent;
        }

        /// <summary>
        /// Olay koşullarını kontrol et.
        /// </summary>
        private bool CheckEventConditions(GameEvent evt, CharacterData character)
        {
            if (evt.conditions == null || evt.conditions.Count == 0)
            {
                return true;
            }

            foreach (var condition in evt.conditions)
            {
                if (!EvaluateCondition(condition, character))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Tek bir koşulu değerlendir.
        /// </summary>
        private bool EvaluateCondition(EventCondition condition, CharacterData character)
        {
            int value = 0;

            switch (condition.type)
            {
                case ConditionType.Stat:
                    value = character.Stats.GetStat(condition.statType);
                    break;
                case ConditionType.Money:
                    value = (int)character.Finances.CurrentMoney;
                    break;
                case ConditionType.Education:
                    value = (int)character.Education.CurrentLevel;
                    break;
                case ConditionType.HasJob:
                    return (character.Career.CurrentJob != null) == (condition.targetValue > 0);
                case ConditionType.IsMarried:
                    return character.IsMarried == (condition.targetValue > 0);
                case ConditionType.Gender:
                    return (int)character.Gender == condition.targetValue;
                case ConditionType.InPrison:
                    return character.IsInPrison == (condition.targetValue > 0);
                case ConditionType.HasCriminalRecord:
                    return (character.Prison.criminalRecord.Count > 0) == (condition.targetValue > 0);
            }

            switch (condition.comparison)
            {
                case ComparisonType.Equal:
                    return value == condition.targetValue;
                case ComparisonType.NotEqual:
                    return value != condition.targetValue;
                case ComparisonType.GreaterThan:
                    return value > condition.targetValue;
                case ComparisonType.LessThan:
                    return value < condition.targetValue;
                case ComparisonType.GreaterOrEqual:
                    return value >= condition.targetValue;
                case ComparisonType.LessOrEqual:
                    return value <= condition.targetValue;
            }

            return true;
        }

        /// <summary>
        /// Son olaylar listesine ekle.
        /// </summary>
        private void AddToRecentEvents(string eventId)
        {
            _recentEventIds.Add(eventId);

            // Listeyi sınırla
            if (_recentEventIds.Count > MAX_RECENT_EVENTS)
            {
                _recentEventIds.Remove(_recentEventIds.First());
            }
        }

        /// <summary>
        /// Jenerik olay oluştur (uygun olay bulunamazsa).
        /// </summary>
        private GameEvent GenerateGenericEvent(CharacterData character)
        {
            var lifeStage = character.CurrentLifeStage;

            string[] genericDescriptions = lifeStage switch
            {
                LifeStage.Baby => new[] { "Sakin bir gün geçirdin.", "Ailecek güzel vakit geçirdiniz." },
                LifeStage.Child => new[] { "Okulda sıradan bir gün geçti.", "Arkadaşlarınla oynadın." },
                LifeStage.Teen => new[] { "Sosyal medyada vakit geçirdin.", "Eve geç kaldın." },
                LifeStage.YoungAdult => new[] { "İş aramaya devam ettin.", "Arkadaşlarla buluştun." },
                LifeStage.Adult => new[] { "Normal bir iş günü geçti.", "Aile ile vakit geçirdin." },
                LifeStage.Senior => new[] { "Huzurlu bir gün geçirdin.", "Torunlarını özledin." },
                _ => new[] { "Sıradan bir gün geçti." }
            };

            return new GameEvent
            {
                id = "generic_" + System.Guid.NewGuid().ToString(),
                title = "Günlük Hayat",
                description = genericDescriptions[Random.Range(0, genericDescriptions.Length)],
                category = EventCategory.Random,
                probability = 1f,
                ageRange = new AgeRange { min = 0, max = 120 },
                choices = new List<EventChoice>
                {
                    new EventChoice
                    {
                        text = "Devam et",
                        outcomes = new List<EventOutcome>
                        {
                            new EventOutcome
                            {
                                type = OutcomeType.None,
                                probability = 1f,
                                resultText = "Hayat devam ediyor."
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Event Processing

        /// <summary>
        /// Seçim yap ve sonucu işle.
        /// </summary>
        public void MakeChoice(int choiceIndex)
        {
            if (_currentEvent == null || choiceIndex < 0 || choiceIndex >= _currentEvent.choices.Count)
            {
                Debug.LogError($"[EventManager] Invalid choice index: {choiceIndex}");
                return;
            }

            var choice = _currentEvent.choices[choiceIndex];
            var outcome = SelectOutcome(choice.outcomes);

            // Sonucu uygula
            ApplyOutcome(outcome);

            // Event bus'a yayınla
            EventBus.Publish(new ChoiceMadeEvent
            {
                EventId = _currentEvent.id,
                ChoiceIndex = choiceIndex,
                ResultText = outcome.resultText
            });

            // Sonucu göster
            UIManager.Instance?.ShowEventResult(outcome.resultText, () =>
            {
                // Sonuç kapatıldıktan sonra devam et
                // (Yaşla butonu ile kontrol edilecek)
            });

            Debug.Log($"[EventManager] Choice made: {choice.text} -> {outcome.resultText}");
        }

        /// <summary>
        /// Olasılıklara göre sonuç seç.
        /// </summary>
        private EventOutcome SelectOutcome(List<EventOutcome> outcomes)
        {
            if (outcomes == null || outcomes.Count == 0)
            {
                return new EventOutcome
                {
                    type = OutcomeType.None,
                    probability = 1f,
                    resultText = "Bir şey olmadı."
                };
            }

            // Tek sonuç varsa direkt döndür
            if (outcomes.Count == 1)
            {
                return outcomes[0];
            }

            // Olasılıklara göre seç
            float totalProbability = outcomes.Sum(o => o.probability);
            float randomValue = Random.Range(0f, totalProbability);
            float currentProbability = 0f;

            foreach (var outcome in outcomes)
            {
                currentProbability += outcome.probability;
                if (randomValue <= currentProbability)
                {
                    return outcome;
                }
            }

            return outcomes[outcomes.Count - 1];
        }

        /// <summary>
        /// Sonucu karaktere uygula.
        /// </summary>
        private void ApplyOutcome(EventOutcome outcome)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            switch (outcome.type)
            {
                case OutcomeType.StatChange:
                    ApplyStatChange(outcome, character);
                    break;

                case OutcomeType.MoneyChange:
                    ApplyMoneyChange(outcome, character);
                    break;

                case OutcomeType.RelationshipChange:
                    // Faz 6'da implement edilecek
                    break;

                case OutcomeType.Death:
                    character.Stats.ModifyStat(StatType.Health, -100);
                    break;

                case OutcomeType.PrisonSentence:
                    ApplyPrisonSentence(outcome, character);
                    break;

                case OutcomeType.PrisonRelease:
                    ApplyPrisonRelease(outcome, character);
                    break;

                case OutcomeType.UniversityEnroll:
                    ApplyUniversityEnroll(outcome, character);
                    break;

                case OutcomeType.None:
                default:
                    break;
            }

            // UI'ı güncelle
            UIManager.Instance?.RefreshUI();
        }

        /// <summary>
        /// Stat değişikliği uygula.
        /// </summary>
        private void ApplyStatChange(EventOutcome outcome, CharacterData character)
        {
            if (!System.Enum.TryParse<StatType>(outcome.targetStat, out var statType))
            {
                Debug.LogWarning($"[EventManager] Unknown stat type: {outcome.targetStat}");
                return;
            }

            int changeAmount = Random.Range(outcome.minValue, outcome.maxValue + 1);
            character.Stats.ModifyStat(statType, changeAmount);
        }

        /// <summary>
        /// Para değişikliği uygula.
        /// </summary>
        private void ApplyMoneyChange(EventOutcome outcome, CharacterData character)
        {
            decimal changeAmount = Random.Range(outcome.minValue, outcome.maxValue + 1);
            character.Finances.ModifyMoney(changeAmount, outcome.resultText);
        }

        /// <summary>
        /// Hapis cezası uygula.
        /// </summary>
        private void ApplyPrisonSentence(EventOutcome outcome, CharacterData character)
        {
            int years = Random.Range(outcome.minValue, outcome.maxValue + 1);
            string crime = outcome.targetStat ?? "Suç"; // targetStat'ı suç adı için kullanıyoruz
            character.Prison.ServeSentence(years, crime);

            // Hapiste iş kaybı
            if (character.Career.CurrentJob != null)
            {
                character.Career.jobHistory.Add(character.Career.CurrentJob);
                character.Career.currentJob = null;
                character.isEmployed = false;
            }

            // Mutluluk düşüşü
            character.Stats.ModifyStat(StatType.Happiness, -20);
        }

        /// <summary>
        /// Hapisten çıkış uygula.
        /// </summary>
        private void ApplyPrisonRelease(EventOutcome outcome, CharacterData character)
        {
            int yearsReduced = Random.Range(outcome.minValue, outcome.maxValue + 1);
            character.Prison.EarlyRelease(yearsReduced);

            // Mutluluk artışı
            character.Stats.ModifyStat(StatType.Happiness, 15);
        }

        /// <summary>
        /// Üniversiteye kayıt uygula.
        /// </summary>
        private void ApplyUniversityEnroll(EventOutcome outcome, CharacterData character)
        {
            // targetStat formatı: "ÜniversiteAdı|BölümAdı"
            if (!string.IsNullOrEmpty(outcome.targetStat))
            {
                var parts = outcome.targetStat.Split('|');
                if (parts.Length >= 2)
                {
                    character.Education.universityName = parts[0];
                    character.Education.department = parts[1];
                    character.Education.currentLevel = EducationLevel.University;
                }
            }

            // Zeka artışı
            int intelligenceGain = Random.Range(outcome.minValue, outcome.maxValue + 1);
            character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);
        }

        #endregion

        #region Utility

        /// <summary>
        /// Belirli bir kategoriden olay al.
        /// </summary>
        public GameEvent GetEventByCategory(EventCategory category, CharacterData character)
        {
            if (!_eventsByCategory.ContainsKey(category))
            {
                return null;
            }

            var events = _eventsByCategory[category]
                .Where(e => e.ageRange.min <= character.Age && e.ageRange.max >= character.Age)
                .ToList();

            if (events.Count == 0)
            {
                return null;
            }

            return events[Random.Range(0, events.Count)];
        }

        /// <summary>
        /// ID ile olay bul.
        /// </summary>
        public GameEvent GetEventById(string eventId)
        {
            return _allEvents.FirstOrDefault(e => e.id == eventId);
        }

        /// <summary>
        /// Olay geçmişini temizle.
        /// </summary>
        public void ClearRecentEvents()
        {
            _recentEventIds.Clear();
        }

        #endregion
    }

    /// <summary>
    /// JSON'dan event collection yükleme için wrapper.
    /// </summary>
    [System.Serializable]
    public class GameEventCollection
    {
        public List<GameEvent> events;
    }
}
