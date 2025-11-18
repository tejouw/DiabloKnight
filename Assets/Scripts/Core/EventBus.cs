using System;
using System.Collections.Generic;
using UnityEngine;

namespace TurkishLifeSim.Core
{
    /// <summary>
    /// Event Bus sistemi - Observer pattern implementasyonu.
    /// Oyun içi olayların publish/subscribe modeli ile yönetimi.
    /// </summary>
    public static class EventBus
    {
        // Event tipine göre subscriber listesi
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Belirli bir event tipine abone ol.
        /// </summary>
        public static void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            Type eventType = typeof(T);

            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
            }

            if (!_subscribers[eventType].Contains(handler))
            {
                _subscribers[eventType].Add(handler);
            }
        }

        /// <summary>
        /// Belirli bir event tipinden aboneliği kaldır.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            Type eventType = typeof(T);

            if (_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType].Remove(handler);
            }
        }

        /// <summary>
        /// Event yayınla - tüm abonelere bildir.
        /// </summary>
        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
            Type eventType = typeof(T);

            if (_subscribers.ContainsKey(eventType))
            {
                // Subscriber listesinin kopyasını al (iteration sırasında değişiklik olmasın)
                var handlers = new List<Delegate>(_subscribers[eventType]);

                foreach (var handler in handlers)
                {
                    try
                    {
                        ((Action<T>)handler)?.Invoke(gameEvent);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[EventBus] Error invoking handler for event {eventType.Name}: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Tüm abonelikleri temizle.
        /// </summary>
        public static void Clear()
        {
            _subscribers.Clear();
        }

        /// <summary>
        /// Belirli bir event tipinin tüm aboneliklerini temizle.
        /// </summary>
        public static void Clear<T>() where T : IGameEvent
        {
            Type eventType = typeof(T);
            if (_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType].Clear();
            }
        }

        /// <summary>
        /// Belirli bir event tipinin subscriber sayısını döndür.
        /// </summary>
        public static int GetSubscriberCount<T>() where T : IGameEvent
        {
            Type eventType = typeof(T);
            return _subscribers.ContainsKey(eventType) ? _subscribers[eventType].Count : 0;
        }
    }

    /// <summary>
    /// Tüm game eventler için marker interface.
    /// </summary>
    public interface IGameEvent
    {
    }

    #region Core Game Events

    /// <summary>
    /// Oyun durumu değiştiğinde tetiklenir.
    /// </summary>
    public struct GameStateChangedEvent : IGameEvent
    {
        public GameState OldState;
        public GameState NewState;
    }

    /// <summary>
    /// Karakter statı değiştiğinde tetiklenir.
    /// </summary>
    public struct StatChangedEvent : IGameEvent
    {
        public StatType StatType;
        public int OldValue;
        public int NewValue;
        public int Delta;
    }

    /// <summary>
    /// Yaş ilerlediğinde tetiklenir.
    /// </summary>
    public struct AgeProgressedEvent : IGameEvent
    {
        public int OldAge;
        public int NewAge;
        public LifeStage OldStage;
        public LifeStage NewStage;
    }

    /// <summary>
    /// Yeni olay başladığında tetiklenir.
    /// </summary>
    public struct EventStartedEvent : IGameEvent
    {
        public string EventId;
        public string EventTitle;
    }

    /// <summary>
    /// Olay seçimi yapıldığında tetiklenir.
    /// </summary>
    public struct ChoiceMadeEvent : IGameEvent
    {
        public string EventId;
        public int ChoiceIndex;
        public string ResultText;
    }

    /// <summary>
    /// Para değiştiğinde tetiklenir.
    /// </summary>
    public struct MoneyChangedEvent : IGameEvent
    {
        public float OldAmount;
        public float NewAmount;
        public float Delta;
        public string Reason;
    }

    /// <summary>
    /// İlişki değiştiğinde tetiklenir.
    /// </summary>
    public struct RelationshipChangedEvent : IGameEvent
    {
        public string NpcId;
        public string NpcName;
        public RelationshipChangeType ChangeType;
    }

    /// <summary>
    /// Karakter öldüğünde tetiklenir.
    /// </summary>
    public struct CharacterDiedEvent : IGameEvent
    {
        public int Age;
        public string DeathCause;
        public string Epitaph;
    }

    /// <summary>
    /// UI ekranı değiştiğinde tetiklenir.
    /// </summary>
    public struct ScreenChangedEvent : IGameEvent
    {
        public ScreenType OldScreen;
        public ScreenType NewScreen;
    }

    /// <summary>
    /// Oyun kaydedildiğinde tetiklenir.
    /// </summary>
    public struct GameSavedEvent : IGameEvent
    {
        public int SlotIndex;
        public DateTime SaveTime;
    }

    /// <summary>
    /// Oyun yüklendiğinde tetiklenir.
    /// </summary>
    public struct GameLoadedEvent : IGameEvent
    {
        public int SlotIndex;
    }

    #endregion

    #region Enums

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Loading
    }

    public enum StatType
    {
        Health,
        Happiness,
        Intelligence,
        Appearance,
        Fame
    }

    public enum LifeStage
    {
        Baby,       // 0-4
        Child,      // 5-11
        Teen,       // 12-17
        YoungAdult, // 18-29
        Adult,      // 30-59
        Senior      // 60+
    }

    public enum RelationshipChangeType
    {
        Created,
        Improved,
        Worsened,
        Ended,
        StatusChanged
    }

    public enum ScreenType
    {
        None,
        MainMenu,
        Game,
        Profile,
        Relationships,
        Settings,
        SaveLoad,
        EventResult,
        Death
    }

    #endregion
}
