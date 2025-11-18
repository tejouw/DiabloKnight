using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Activities
{
    /// <summary>
    /// Aktivite türü enum.
    /// </summary>
    public enum ActivityType
    {
        // Fitness
        Gym,
        MartialArts,
        Yoga,
        Running,
        Swimming,

        // Zihinsel
        Library,
        Meditation,
        OnlineCourse,

        // Eğlence
        Cinema,
        Concert,
        Nightclub,
        Restaurant,

        // Kumar
        Casino,
        Lottery,
        Blackjack,

        // Güzellik
        Spa,
        Hairdresser,
        PlasticSurgery,

        // Sosyal
        Vacation,
        Shopping,

        // Spor
        Football,
        Basketball,
        Tennis
    }

    /// <summary>
    /// Aktivite kategorisi.
    /// </summary>
    public enum ActivityCategory
    {
        Fitness,
        Mind,
        Entertainment,
        Gambling,
        Beauty,
        Social,
        Sports
    }

    /// <summary>
    /// Aktivite tanımı.
    /// </summary>
    [System.Serializable]
    public class ActivityDefinition
    {
        public ActivityType type;
        public string name;
        public string description;
        public ActivityCategory category;
        public int minAge;
        public int cost;
        public bool isRepeatable;

        // Stat etkileri
        public int healthEffect;
        public int happinessEffect;
        public int intelligenceEffect;
        public int appearanceEffect;
        public int fameEffect;

        // Ek efektler
        public float successChance; // 0-1 arası
        public string successText;
        public string failText;
    }

    /// <summary>
    /// Aktivite sonucu.
    /// </summary>
    [System.Serializable]
    public class ActivityResult
    {
        public ActivityType activityType;
        public bool success;
        public string resultText;
        public int moneyChange;
        public Dictionary<StatType, int> statChanges;
        public string specialEvent; // Özel olay tetiklenirse
    }

    /// <summary>
    /// Kumar sonucu.
    /// </summary>
    [System.Serializable]
    public class GamblingResult
    {
        public int betAmount;
        public int winAmount;
        public bool isWin;
        public string gameName;
        public string resultText;
    }

    /// <summary>
    /// Varsayılan aktivite tanımları.
    /// </summary>
    public static class DefaultActivities
    {
        public static List<ActivityDefinition> GetAllActivities()
        {
            return new List<ActivityDefinition>
            {
                // FITNESS
                new ActivityDefinition
                {
                    type = ActivityType.Gym,
                    name = "Spor Salonu",
                    description = "Kas yap, sağlıklı kal!",
                    category = ActivityCategory.Fitness,
                    minAge = 10,
                    cost = 200,
                    isRepeatable = true,
                    healthEffect = 5,
                    happinessEffect = 2,
                    appearanceEffect = 3,
                    successChance = 0.9f,
                    successText = "Harika bir antrenman yaptın!",
                    failText = "Sakatlandın!"
                },
                new ActivityDefinition
                {
                    type = ActivityType.MartialArts,
                    name = "Dövüş Sanatları",
                    description = "Taekwondo, karate veya boks öğren.",
                    category = ActivityCategory.Fitness,
                    minAge = 6,
                    cost = 500,
                    isRepeatable = true,
                    healthEffect = 4,
                    happinessEffect = 3,
                    intelligenceEffect = 1,
                    successChance = 0.85f,
                    successText = "Yeni bir kuşak kazandın!",
                    failText = "Bu spor sana göre değil."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Yoga,
                    name = "Yoga",
                    description = "Zihin ve beden uyumu.",
                    category = ActivityCategory.Fitness,
                    minAge = 12,
                    cost = 150,
                    isRepeatable = true,
                    healthEffect = 3,
                    happinessEffect = 5,
                    appearanceEffect = 1,
                    successChance = 0.95f,
                    successText = "Huzurlu bir seans geçirdin.",
                    failText = "Konsantrasyonun dağıldı."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Running,
                    name = "Koşu",
                    description = "Parkta koşu yap.",
                    category = ActivityCategory.Fitness,
                    minAge = 5,
                    cost = 0,
                    isRepeatable = true,
                    healthEffect = 4,
                    happinessEffect = 2,
                    successChance = 0.9f,
                    successText = "Güzel bir koşu yaptın!",
                    failText = "Ayağın burkuldu."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Swimming,
                    name = "Yüzme",
                    description = "Havuzda yüz.",
                    category = ActivityCategory.Fitness,
                    minAge = 5,
                    cost = 100,
                    isRepeatable = true,
                    healthEffect = 5,
                    happinessEffect = 3,
                    appearanceEffect = 2,
                    successChance = 0.85f,
                    successText = "Harika bir yüzme seansı!",
                    failText = "Su soğuktu, hasta oldun."
                },

                // ZİHİNSEL
                new ActivityDefinition
                {
                    type = ActivityType.Library,
                    name = "Kütüphane",
                    description = "Kitap oku, bilgini artır.",
                    category = ActivityCategory.Mind,
                    minAge = 6,
                    cost = 0,
                    isRepeatable = true,
                    intelligenceEffect = 5,
                    happinessEffect = 1,
                    successChance = 0.95f,
                    successText = "Çok şey öğrendin!",
                    failText = "Konsantre olamadın."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Meditation,
                    name = "Meditasyon",
                    description = "Zihnini temizle.",
                    category = ActivityCategory.Mind,
                    minAge = 10,
                    cost = 0,
                    isRepeatable = true,
                    healthEffect = 2,
                    happinessEffect = 5,
                    successChance = 0.9f,
                    successText = "Huzur buldun.",
                    failText = "Zihnin çok karışık."
                },
                new ActivityDefinition
                {
                    type = ActivityType.OnlineCourse,
                    name = "Online Kurs",
                    description = "İnternetten yeni beceriler öğren.",
                    category = ActivityCategory.Mind,
                    minAge = 12,
                    cost = 500,
                    isRepeatable = true,
                    intelligenceEffect = 8,
                    successChance = 0.8f,
                    successText = "Sertifika kazandın!",
                    failText = "Kursu tamamlayamadın."
                },

                // EĞLENCE
                new ActivityDefinition
                {
                    type = ActivityType.Cinema,
                    name = "Sinema",
                    description = "Film izle.",
                    category = ActivityCategory.Entertainment,
                    minAge = 5,
                    cost = 100,
                    isRepeatable = true,
                    happinessEffect = 5,
                    successChance = 0.95f,
                    successText = "Harika bir filmdi!",
                    failText = "Film berbattı."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Concert,
                    name = "Konser",
                    description = "Canlı müzik dinle.",
                    category = ActivityCategory.Entertainment,
                    minAge = 12,
                    cost = 300,
                    isRepeatable = true,
                    happinessEffect = 8,
                    successChance = 0.9f,
                    successText = "Muhteşem bir gece!",
                    failText = "Konser iptal edildi."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Nightclub,
                    name = "Gece Kulübü",
                    description = "Dans et, eğlen.",
                    category = ActivityCategory.Entertainment,
                    minAge = 18,
                    cost = 500,
                    isRepeatable = true,
                    happinessEffect = 6,
                    healthEffect = -1,
                    successChance = 0.85f,
                    successText = "Çılgın bir gece!",
                    failText = "Kapıda geri çevrildın."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Restaurant,
                    name = "Restoran",
                    description = "Güzel bir yemek ye.",
                    category = ActivityCategory.Entertainment,
                    minAge = 0,
                    cost = 200,
                    isRepeatable = true,
                    happinessEffect = 4,
                    healthEffect = 1,
                    successChance = 0.95f,
                    successText = "Lezzetli bir yemekti!",
                    failText = "Miden bozuldu."
                },

                // KUMAR
                new ActivityDefinition
                {
                    type = ActivityType.Casino,
                    name = "Kumarhane",
                    description = "Şansını dene!",
                    category = ActivityCategory.Gambling,
                    minAge = 18,
                    cost = 0, // Bahis miktarı ayrı
                    isRepeatable = true,
                    successChance = 0.35f,
                    successText = "Kazandın!",
                    failText = "Kaybettin!"
                },
                new ActivityDefinition
                {
                    type = ActivityType.Lottery,
                    name = "Piyango",
                    description = "Milli piyango bileti al.",
                    category = ActivityCategory.Gambling,
                    minAge = 18,
                    cost = 50,
                    isRepeatable = true,
                    successChance = 0.01f,
                    successText = "BÜYÜK İKRAMİYE!",
                    failText = "Bu sefer olmadı."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Blackjack,
                    name = "Blackjack",
                    description = "21 oyna.",
                    category = ActivityCategory.Gambling,
                    minAge = 18,
                    cost = 0,
                    isRepeatable = true,
                    successChance = 0.45f,
                    successText = "Blackjack!",
                    failText = "Bust!"
                },

                // GÜZELLİK
                new ActivityDefinition
                {
                    type = ActivityType.Spa,
                    name = "Spa & Hamam",
                    description = "Kendini şımart.",
                    category = ActivityCategory.Beauty,
                    minAge = 16,
                    cost = 500,
                    isRepeatable = true,
                    happinessEffect = 6,
                    appearanceEffect = 3,
                    healthEffect = 2,
                    successChance = 0.95f,
                    successText = "Harika hissediyorsun!",
                    failText = "Beklentini karşılamadı."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Hairdresser,
                    name = "Kuaför",
                    description = "Saç kesimi ve bakım.",
                    category = ActivityCategory.Beauty,
                    minAge = 5,
                    cost = 150,
                    isRepeatable = true,
                    happinessEffect = 2,
                    appearanceEffect = 5,
                    successChance = 0.85f,
                    successText = "Yeni saçını herkes beğendi!",
                    failText = "Berbat bir kesim oldu."
                },
                new ActivityDefinition
                {
                    type = ActivityType.PlasticSurgery,
                    name = "Estetik Ameliyat",
                    description = "Görünüşünü değiştir.",
                    category = ActivityCategory.Beauty,
                    minAge = 18,
                    cost = 50000,
                    isRepeatable = true,
                    appearanceEffect = 20,
                    healthEffect = -5,
                    successChance = 0.7f,
                    successText = "Ameliyat başarılı!",
                    failText = "Komplikasyon gelişti."
                },

                // SOSYAL
                new ActivityDefinition
                {
                    type = ActivityType.Vacation,
                    name = "Tatil",
                    description = "Antalya, Bodrum veya yurt dışı.",
                    category = ActivityCategory.Social,
                    minAge = 0,
                    cost = 5000,
                    isRepeatable = true,
                    happinessEffect = 15,
                    healthEffect = 3,
                    successChance = 0.95f,
                    successText = "Muhteşem bir tatildi!",
                    failText = "Tatil berbat geçti."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Shopping,
                    name = "Alışveriş",
                    description = "AVM'de gezin.",
                    category = ActivityCategory.Social,
                    minAge = 5,
                    cost = 500,
                    isRepeatable = true,
                    happinessEffect = 5,
                    appearanceEffect = 2,
                    successChance = 0.9f,
                    successText = "Güzel alışveriş yaptın!",
                    failText = "Aradığını bulamadın."
                },

                // SPOR
                new ActivityDefinition
                {
                    type = ActivityType.Football,
                    name = "Futbol",
                    description = "Mahalle maçı yap.",
                    category = ActivityCategory.Sports,
                    minAge = 6,
                    cost = 0,
                    isRepeatable = true,
                    healthEffect = 4,
                    happinessEffect = 5,
                    successChance = 0.85f,
                    successText = "Gol attın!",
                    failText = "Sakatlandın."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Basketball,
                    name = "Basketbol",
                    description = "Basket at.",
                    category = ActivityCategory.Sports,
                    minAge = 8,
                    cost = 0,
                    isRepeatable = true,
                    healthEffect = 4,
                    happinessEffect = 4,
                    successChance = 0.85f,
                    successText = "Üç sayı!",
                    failText = "Top yüzüne geldi."
                },
                new ActivityDefinition
                {
                    type = ActivityType.Tennis,
                    name = "Tenis",
                    description = "Kort kirala ve oyna.",
                    category = ActivityCategory.Sports,
                    minAge = 8,
                    cost = 200,
                    isRepeatable = true,
                    healthEffect = 3,
                    happinessEffect = 3,
                    appearanceEffect = 1,
                    successChance = 0.8f,
                    successText = "Ace!",
                    failText = "Rakibin çok iyiydi."
                }
            };
        }
    }
}
