using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Events
{
    /// <summary>
    /// Oyun olayı - Karar ağacı sistemi.
    /// </summary>
    [System.Serializable]
    public class GameEvent
    {
        public string id;
        public string title;
        public string description;
        public AgeRange ageRange;
        public List<EventChoice> choices;
        public List<EventCondition> conditions;
        public EventCategory category;
        public float probability = 1f;
    }

    /// <summary>
    /// Yaş aralığı.
    /// </summary>
    [System.Serializable]
    public class AgeRange
    {
        public int min;
        public int max;
    }

    /// <summary>
    /// Olay seçeneği.
    /// </summary>
    [System.Serializable]
    public class EventChoice
    {
        public string text;
        public List<EventOutcome> outcomes;
        public List<ChoiceCondition> requirements;
    }

    /// <summary>
    /// Seçim için koşul.
    /// </summary>
    [System.Serializable]
    public class ChoiceCondition
    {
        public ConditionType type;
        public StatType statType;
        public int targetValue;
    }

    /// <summary>
    /// Olay sonucu.
    /// </summary>
    [System.Serializable]
    public class EventOutcome
    {
        public OutcomeType type;
        public string targetStat;
        public string targetRelationship;
        public int minValue;
        public int maxValue;
        public float probability = 1f;
        public string resultText;
    }

    /// <summary>
    /// Olay koşulu.
    /// </summary>
    [System.Serializable]
    public class EventCondition
    {
        public ConditionType type;
        public StatType statType;
        public ComparisonType comparison;
        public int targetValue;
    }

    /// <summary>
    /// Olay kategorisi enum.
    /// </summary>
    public enum EventCategory
    {
        Family,         // Aile olayları
        School,         // Okul olayları
        Health,         // Sağlık olayları
        Social,         // Sosyal olaylar
        Career,         // Kariyer olayları
        Financial,      // Finansal olaylar
        Random,         // Rastgele olaylar
        Crime,          // Suç olayları
        Military,       // Askerlik olayları
        Romance,        // Romantik olaylar
        Milestone       // Dönüm noktası olayları
    }

    /// <summary>
    /// Sonuç tipi enum.
    /// </summary>
    public enum OutcomeType
    {
        None,
        StatChange,
        MoneyChange,
        RelationshipChange,
        ItemGain,
        ItemLoss,
        JobChange,
        EducationChange,
        Death,
        Custom
    }

    /// <summary>
    /// Koşul tipi enum.
    /// </summary>
    public enum ConditionType
    {
        Stat,
        Money,
        Education,
        HasJob,
        IsMarried,
        Gender,
        HasChild,
        HasSibling
    }

    /// <summary>
    /// Karşılaştırma tipi enum.
    /// </summary>
    public enum ComparisonType
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterOrEqual,
        LessOrEqual
    }
}
