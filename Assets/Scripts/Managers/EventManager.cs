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
                    ApplyRelationshipChange(outcome, character);
                    break;

                case OutcomeType.JobChange:
                    ApplyJobChange(outcome, character);
                    break;

                case OutcomeType.EducationChange:
                    ApplyEducationChange(outcome, character);
                    break;

                case OutcomeType.ItemGain:
                    ApplyItemGain(outcome, character);
                    break;

                case OutcomeType.ItemLoss:
                    ApplyItemLoss(outcome, character);
                    break;

                case OutcomeType.Death:
                    character.Stats.ModifyStat(StatType.Health, -100);
                    break;

                case OutcomeType.Custom:
                    ApplyCustomOutcome(outcome, character);
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
        /// İlişki değişikliği uygula.
        /// </summary>
        private void ApplyRelationshipChange(EventOutcome outcome, CharacterData character)
        {
            if (string.IsNullOrEmpty(outcome.targetStat)) return;

            // targetStat formatı: "npcId:intimacy" veya "npcId:trust"
            var parts = outcome.targetStat.Split(':');
            if (parts.Length < 2) return;

            string npcId = parts[0];
            string field = parts[1].ToLower();

            var relationship = character.Relationships.Find(r => r.npcId == npcId);
            if (relationship == null)
            {
                // Eğer npcId "random" ise rastgele bir ilişki seç
                if (npcId == "random" && character.Relationships.Count > 0)
                {
                    relationship = character.Relationships[Random.Range(0, character.Relationships.Count)];
                }
                else
                {
                    return;
                }
            }

            int changeAmount = Random.Range(outcome.minValue, outcome.maxValue + 1);

            switch (field)
            {
                case "intimacy":
                    relationship.intimacy = Mathf.Clamp(relationship.intimacy + changeAmount, 0, 100);
                    break;
                case "trust":
                    relationship.trust = Mathf.Clamp(relationship.trust + changeAmount, 0, 100);
                    break;
            }

            // Event yayınla
            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = relationship.npcId,
                NpcName = relationship.npcName,
                Field = field,
                OldValue = field == "intimacy" ? relationship.intimacy - changeAmount : relationship.trust - changeAmount,
                NewValue = field == "intimacy" ? relationship.intimacy : relationship.trust
            });
        }

        /// <summary>
        /// İş değişikliği uygula.
        /// </summary>
        private void ApplyJobChange(EventOutcome outcome, CharacterData character)
        {
            if (string.IsNullOrEmpty(outcome.targetStat)) return;

            // targetStat: "jobId" formatında
            var newJob = DataManager.Instance?.GetJobById(outcome.targetStat);
            if (newJob == null)
            {
                // Rastgele uygun bir iş bul
                var eligibleJobs = DataManager.Instance?.GetEligibleJobs(character);
                if (eligibleJobs != null && eligibleJobs.Count > 0)
                {
                    newJob = eligibleJobs[Random.Range(0, eligibleJobs.Count)];
                }
            }

            if (newJob != null)
            {
                // Eski işi geçmişe ekle
                if (character.Career.CurrentJob != null)
                {
                    character.Career.jobHistory.Add(character.Career.CurrentJob);
                }

                // Yeni işi ata
                character.Career.currentJob = new Job
                {
                    id = newJob.id,
                    title = newJob.title,
                    company = newJob.company,
                    category = newJob.category,
                    baseSalary = newJob.baseSalary,
                    yearsWorked = 0
                };
                character.Career.yearsInJob = 0;
                character.isEmployed = true;

                Debug.Log($"[EventManager] Job changed to: {newJob.title}");
            }
        }

        /// <summary>
        /// Eğitim değişikliği uygula.
        /// </summary>
        private void ApplyEducationChange(EventOutcome outcome, CharacterData character)
        {
            if (string.IsNullOrEmpty(outcome.targetStat)) return;

            // targetStat formatı: "level:değer" veya "gpa:değer"
            var parts = outcome.targetStat.Split(':');
            if (parts.Length < 2) return;

            string field = parts[0].ToLower();

            switch (field)
            {
                case "level":
                    if (System.Enum.TryParse<EducationLevel>(parts[1], out var level))
                    {
                        character.Education.currentLevel = level;
                    }
                    break;
                case "gpa":
                    if (float.TryParse(parts[1], out var gpa))
                    {
                        character.Education.gpa = Mathf.Clamp(gpa, 0f, 4f);
                    }
                    break;
                case "school":
                    character.Education.schoolName = parts[1];
                    break;
                case "graduated":
                    character.Education.isGraduated = parts[1].ToLower() == "true";
                    break;
                case "yks":
                    if (int.TryParse(parts[1], out var yks))
                    {
                        character.Education.yksScore = yks;
                    }
                    break;
            }
        }

        /// <summary>
        /// Eşya kazanma uygula.
        /// </summary>
        private void ApplyItemGain(EventOutcome outcome, CharacterData character)
        {
            if (string.IsNullOrEmpty(outcome.targetStat)) return;

            // targetStat: "asset:Araba" veya "debt:Kredi"
            var parts = outcome.targetStat.Split(':');
            if (parts.Length < 2) return;

            string type = parts[0].ToLower();
            string item = parts[1];

            switch (type)
            {
                case "asset":
                    if (!character.Finances.assets.Contains(item))
                    {
                        character.Finances.assets.Add(item);
                        Debug.Log($"[EventManager] Asset gained: {item}");
                    }
                    break;
                case "debt":
                    if (!character.Finances.debts.Contains(item))
                    {
                        character.Finances.debts.Add(item);
                        Debug.Log($"[EventManager] Debt added: {item}");
                    }
                    break;
            }
        }

        /// <summary>
        /// Eşya kaybetme uygula.
        /// </summary>
        private void ApplyItemLoss(EventOutcome outcome, CharacterData character)
        {
            if (string.IsNullOrEmpty(outcome.targetStat)) return;

            // targetStat: "asset:Araba" veya "debt:Kredi"
            var parts = outcome.targetStat.Split(':');
            if (parts.Length < 2) return;

            string type = parts[0].ToLower();
            string item = parts[1];

            switch (type)
            {
                case "asset":
                    character.Finances.assets.Remove(item);
                    Debug.Log($"[EventManager] Asset lost: {item}");
                    break;
                case "debt":
                    character.Finances.debts.Remove(item);
                    Debug.Log($"[EventManager] Debt removed: {item}");
                    break;
            }
        }

        /// <summary>
        /// Özel sonuç uygula.
        /// </summary>
        private void ApplyCustomOutcome(EventOutcome outcome, CharacterData character)
        {
            if (string.IsNullOrEmpty(outcome.targetStat)) return;

            // targetStat formatı: "action:parametre"
            var parts = outcome.targetStat.Split(':');
            if (parts.Length < 1) return;

            string action = parts[0].ToLower();

            switch (action)
            {
                case "marry":
                    character.isMarried = true;
                    break;
                case "divorce":
                    character.isMarried = false;
                    break;
                case "military":
                    character.hasCompletedMilitary = true;
                    break;
                case "unemployed":
                    character.isEmployed = false;
                    character.Career.currentJob = null;
                    break;
            }
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
