using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Character
{
    /// <summary>
    /// Yetenek ve hobi sistemi data modelleri.
    /// </summary>

    #region Talent System

    /// <summary>
    /// Karakter yetenek verisi - Tüm yetenekleri ve hobileri içerir.
    /// </summary>
    [System.Serializable]
    public class TalentData
    {
        public List<Talent> talents = new List<Talent>();
        public List<Hobby> hobbies = new List<Hobby>();
        public int totalPracticeHours = 0;

        /// <summary>
        /// Yetenek ekle.
        /// </summary>
        public void AddTalent(TalentType type, int initialSkill = 0)
        {
            if (HasTalent(type)) return;

            var talent = new Talent
            {
                type = type,
                skillLevel = initialSkill,
                practiceHours = 0,
                isDiscovered = true,
                discoveryAge = 0
            };

            talents.Add(talent);

            EventBus.Publish(new TalentDiscoveredEvent
            {
                TalentType = type,
                InitialSkill = initialSkill
            });
        }

        /// <summary>
        /// Yetenek var mı kontrol et.
        /// </summary>
        public bool HasTalent(TalentType type)
        {
            return talents.Exists(t => t.type == type);
        }

        /// <summary>
        /// Yetenek al.
        /// </summary>
        public Talent GetTalent(TalentType type)
        {
            return talents.Find(t => t.type == type);
        }

        /// <summary>
        /// Hobi ekle.
        /// </summary>
        public void AddHobby(HobbyType type)
        {
            if (HasHobby(type)) return;

            var hobby = new Hobby
            {
                type = type,
                enjoymentLevel = 50,
                experiencePoints = 0,
                isActive = true,
                startAge = 0
            };

            hobbies.Add(hobby);

            EventBus.Publish(new HobbyStartedEvent
            {
                HobbyType = type
            });
        }

        /// <summary>
        /// Hobi var mı kontrol et.
        /// </summary>
        public bool HasHobby(HobbyType type)
        {
            return hobbies.Exists(h => h.type == type);
        }

        /// <summary>
        /// Hobi al.
        /// </summary>
        public Hobby GetHobby(HobbyType type)
        {
            return hobbies.Find(h => h.type == type);
        }

        /// <summary>
        /// Aktif hobileri al.
        /// </summary>
        public List<Hobby> GetActiveHobbies()
        {
            return hobbies.FindAll(h => h.isActive);
        }
    }

    /// <summary>
    /// Yetenek verisi.
    /// </summary>
    [System.Serializable]
    public class Talent
    {
        public TalentType type;
        [Range(0, 100)] public int skillLevel = 0;
        public int practiceHours = 0;
        public bool isDiscovered = false;
        public int discoveryAge = 0;
        public List<string> achievements = new List<string>();
        public TalentRank currentRank = TalentRank.Beginner;

        /// <summary>
        /// Yetenek seviyesi string olarak.
        /// </summary>
        public string SkillLevelText
        {
            get
            {
                if (skillLevel < 20) return "Acemi";
                if (skillLevel < 40) return "Başlangıç";
                if (skillLevel < 60) return "Orta";
                if (skillLevel < 80) return "İleri";
                if (skillLevel < 95) return "Uzman";
                return "Usta";
            }
        }

        /// <summary>
        /// Pratik yap ve skill seviyesini artır.
        /// </summary>
        public int Practice(int hours, int intelligence)
        {
            practiceHours += hours;

            // Intelligence'a göre öğrenme hızı
            float learningMultiplier = 1f + (intelligence - 50) * 0.01f;

            // Seviye yükseldikçe ilerleme yavaşlar
            float difficultyMultiplier = 1f - (skillLevel * 0.005f);

            int skillGain = Mathf.RoundToInt(hours * learningMultiplier * difficultyMultiplier * 0.5f);
            skillGain = Mathf.Max(1, skillGain);

            int oldSkill = skillLevel;
            skillLevel = Mathf.Clamp(skillLevel + skillGain, 0, 100);

            // Rank güncelle
            UpdateRank();

            return skillLevel - oldSkill;
        }

        /// <summary>
        /// Rank güncelle.
        /// </summary>
        private void UpdateRank()
        {
            TalentRank oldRank = currentRank;

            if (skillLevel >= 90) currentRank = TalentRank.Master;
            else if (skillLevel >= 75) currentRank = TalentRank.Expert;
            else if (skillLevel >= 50) currentRank = TalentRank.Advanced;
            else if (skillLevel >= 25) currentRank = TalentRank.Intermediate;
            else currentRank = TalentRank.Beginner;

            if (oldRank != currentRank)
            {
                EventBus.Publish(new TalentRankChangedEvent
                {
                    TalentType = type,
                    OldRank = oldRank,
                    NewRank = currentRank
                });
            }
        }
    }

    /// <summary>
    /// Hobi verisi.
    /// </summary>
    [System.Serializable]
    public class Hobby
    {
        public HobbyType type;
        [Range(0, 100)] public int enjoymentLevel = 50;
        public int experiencePoints = 0;
        public bool isActive = true;
        public int startAge = 0;
        public int totalSessions = 0;

        /// <summary>
        /// Hobi aktivitesi yap.
        /// </summary>
        public HobbyActivityResult DoActivity(int hours)
        {
            totalSessions++;
            experiencePoints += hours * 10;

            var result = new HobbyActivityResult
            {
                HobbyType = type,
                HoursSpent = hours,
                HappinessGain = CalculateHappinessGain(hours),
                HealthGain = CalculateHealthGain(hours),
                SkillGain = hours * 2
            };

            return result;
        }

        private int CalculateHappinessGain(int hours)
        {
            // Keyif seviyesine göre mutluluk kazancı
            float multiplier = enjoymentLevel / 50f;
            return Mathf.RoundToInt(hours * 2 * multiplier);
        }

        private int CalculateHealthGain(int hours)
        {
            // Fiziksel hobiler sağlık kazandırır
            if (IsPhysicalHobby())
            {
                return Mathf.RoundToInt(hours * 1.5f);
            }
            return 0;
        }

        private bool IsPhysicalHobby()
        {
            return type == HobbyType.Football ||
                   type == HobbyType.Basketball ||
                   type == HobbyType.Swimming ||
                   type == HobbyType.Running ||
                   type == HobbyType.Gym ||
                   type == HobbyType.MartialArts ||
                   type == HobbyType.Dancing ||
                   type == HobbyType.Hiking ||
                   type == HobbyType.Cycling ||
                   type == HobbyType.Yoga;
        }
    }

    /// <summary>
    /// Hobi aktivitesi sonucu.
    /// </summary>
    public struct HobbyActivityResult
    {
        public HobbyType HobbyType;
        public int HoursSpent;
        public int HappinessGain;
        public int HealthGain;
        public int SkillGain;
    }

    #endregion

    #region Enums

    /// <summary>
    /// Yetenek türleri.
    /// </summary>
    public enum TalentType
    {
        // Müzik
        Singing,            // Şarkı söyleme
        Guitar,             // Gitar
        Piano,              // Piyano
        Violin,             // Keman
        Drums,              // Bateri
        Saz,                // Saz (Türk müziği)

        // Sanat
        Painting,           // Resim
        Drawing,            // Çizim
        Sculpture,          // Heykel
        Photography,        // Fotoğrafçılık
        Writing,            // Yazarlık
        Acting,             // Oyunculuk

        // Spor
        Football,           // Futbol
        Basketball,         // Basketbol
        Volleyball,         // Voleybol
        Swimming,           // Yüzme
        MartialArts,        // Dövüş sanatları
        Athletics,          // Atletizm

        // Akademik
        Mathematics,        // Matematik
        Science,            // Bilim
        Languages,          // Diller
        Coding,             // Programlama
        Chess,              // Satranç

        // Diğer
        Cooking,            // Yemek yapma
        Crafting,           // El sanatları
        Gaming,             // Video oyunları
        PublicSpeaking      // Topluluk önünde konuşma
    }

    /// <summary>
    /// Hobi türleri.
    /// </summary>
    public enum HobbyType
    {
        // Spor Hobileri
        Football,
        Basketball,
        Swimming,
        Running,
        Gym,
        MartialArts,
        Dancing,
        Hiking,
        Cycling,
        Yoga,

        // Sanat Hobileri
        Painting,
        Drawing,
        Photography,
        Writing,
        Music,
        Sculpting,

        // Eğlence Hobileri
        Gaming,
        Reading,
        Movies,
        Cooking,
        Gardening,
        Fishing,

        // Sosyal Hobiler
        Volunteering,
        Traveling,
        Socializing,
        Blogging,

        // Koleksiyon Hobileri
        Collecting,
        Antiques,

        // Diğer
        Meditation,
        Chess,
        Coding
    }

    /// <summary>
    /// Yetenek seviyesi sıralaması.
    /// </summary>
    public enum TalentRank
    {
        Beginner,       // Başlangıç (0-24)
        Intermediate,   // Orta (25-49)
        Advanced,       // İleri (50-74)
        Expert,         // Uzman (75-89)
        Master          // Usta (90-100)
    }

    #endregion

    #region Events

    /// <summary>
    /// Yetenek keşfedildiğinde tetiklenir.
    /// </summary>
    public struct TalentDiscoveredEvent : IGameEvent
    {
        public TalentType TalentType;
        public int InitialSkill;
    }

    /// <summary>
    /// Yetenek seviyesi değiştiğinde tetiklenir.
    /// </summary>
    public struct TalentLevelChangedEvent : IGameEvent
    {
        public TalentType TalentType;
        public int OldLevel;
        public int NewLevel;
        public int PracticeHours;
    }

    /// <summary>
    /// Yetenek rankı değiştiğinde tetiklenir.
    /// </summary>
    public struct TalentRankChangedEvent : IGameEvent
    {
        public TalentType TalentType;
        public TalentRank OldRank;
        public TalentRank NewRank;
    }

    /// <summary>
    /// Hobi başladığında tetiklenir.
    /// </summary>
    public struct HobbyStartedEvent : IGameEvent
    {
        public HobbyType HobbyType;
    }

    /// <summary>
    /// Hobi aktivitesi yapıldığında tetiklenir.
    /// </summary>
    public struct HobbyActivityEvent : IGameEvent
    {
        public HobbyType HobbyType;
        public int HappinessGain;
        public int HealthGain;
    }

    /// <summary>
    /// Hobi bırakıldığında tetiklenir.
    /// </summary>
    public struct HobbyQuitEvent : IGameEvent
    {
        public HobbyType HobbyType;
        public int TotalSessions;
    }

    #endregion

    #region Talent Definitions

    /// <summary>
    /// Yetenek tanımları ve özellikleri.
    /// </summary>
    public static class TalentDefinitions
    {
        /// <summary>
        /// Yetenek bilgisini al.
        /// </summary>
        public static TalentInfo GetTalentInfo(TalentType type)
        {
            return type switch
            {
                // Müzik
                TalentType.Singing => new TalentInfo("Şarkı Söyleme", "Ses ve vokal yeteneği", TalentCategory.Music, 5),
                TalentType.Guitar => new TalentInfo("Gitar", "Gitar çalma yeteneği", TalentCategory.Music, 7),
                TalentType.Piano => new TalentInfo("Piyano", "Piyano çalma yeteneği", TalentCategory.Music, 8),
                TalentType.Violin => new TalentInfo("Keman", "Keman çalma yeteneği", TalentCategory.Music, 9),
                TalentType.Drums => new TalentInfo("Bateri", "Bateri çalma yeteneği", TalentCategory.Music, 6),
                TalentType.Saz => new TalentInfo("Saz", "Saz çalma yeteneği", TalentCategory.Music, 7),

                // Sanat
                TalentType.Painting => new TalentInfo("Resim", "Resim yapma yeteneği", TalentCategory.Art, 6),
                TalentType.Drawing => new TalentInfo("Çizim", "Çizim yeteneği", TalentCategory.Art, 5),
                TalentType.Sculpture => new TalentInfo("Heykel", "Heykel yapma yeteneği", TalentCategory.Art, 8),
                TalentType.Photography => new TalentInfo("Fotoğrafçılık", "Fotoğraf çekme yeteneği", TalentCategory.Art, 4),
                TalentType.Writing => new TalentInfo("Yazarlık", "Yazı yazma yeteneği", TalentCategory.Art, 6),
                TalentType.Acting => new TalentInfo("Oyunculuk", "Oyunculuk yeteneği", TalentCategory.Art, 7),

                // Spor
                TalentType.Football => new TalentInfo("Futbol", "Futbol oynama yeteneği", TalentCategory.Sports, 5),
                TalentType.Basketball => new TalentInfo("Basketbol", "Basketbol oynama yeteneği", TalentCategory.Sports, 6),
                TalentType.Volleyball => new TalentInfo("Voleybol", "Voleybol oynama yeteneği", TalentCategory.Sports, 5),
                TalentType.Swimming => new TalentInfo("Yüzme", "Yüzme yeteneği", TalentCategory.Sports, 4),
                TalentType.MartialArts => new TalentInfo("Dövüş Sanatları", "Dövüş sanatları yeteneği", TalentCategory.Sports, 7),
                TalentType.Athletics => new TalentInfo("Atletizm", "Atletizm yeteneği", TalentCategory.Sports, 5),

                // Akademik
                TalentType.Mathematics => new TalentInfo("Matematik", "Matematik yeteneği", TalentCategory.Academic, 7),
                TalentType.Science => new TalentInfo("Bilim", "Bilimsel düşünce yeteneği", TalentCategory.Academic, 6),
                TalentType.Languages => new TalentInfo("Diller", "Dil öğrenme yeteneği", TalentCategory.Academic, 5),
                TalentType.Coding => new TalentInfo("Programlama", "Kod yazma yeteneği", TalentCategory.Academic, 7),
                TalentType.Chess => new TalentInfo("Satranç", "Satranç oynama yeteneği", TalentCategory.Academic, 6),

                // Diğer
                TalentType.Cooking => new TalentInfo("Yemek Yapma", "Aşçılık yeteneği", TalentCategory.Other, 4),
                TalentType.Crafting => new TalentInfo("El Sanatları", "El işi yapma yeteneği", TalentCategory.Other, 5),
                TalentType.Gaming => new TalentInfo("Video Oyunları", "Video oyunu oynama yeteneği", TalentCategory.Other, 3),
                TalentType.PublicSpeaking => new TalentInfo("Topluluk Önünde Konuşma", "Sunum ve konuşma yeteneği", TalentCategory.Other, 6),

                _ => new TalentInfo("Bilinmeyen", "Bilinmeyen yetenek", TalentCategory.Other, 5)
            };
        }

        /// <summary>
        /// Hobi bilgisini al.
        /// </summary>
        public static HobbyInfo GetHobbyInfo(HobbyType type)
        {
            return type switch
            {
                // Spor
                HobbyType.Football => new HobbyInfo("Futbol", "Futbol oynama", HobbyCategory.Sports, 50),
                HobbyType.Basketball => new HobbyInfo("Basketbol", "Basketbol oynama", HobbyCategory.Sports, 40),
                HobbyType.Swimming => new HobbyInfo("Yüzme", "Yüzme", HobbyCategory.Sports, 100),
                HobbyType.Running => new HobbyInfo("Koşu", "Koşu yapma", HobbyCategory.Sports, 0),
                HobbyType.Gym => new HobbyInfo("Spor Salonu", "Spor salonu", HobbyCategory.Sports, 200),
                HobbyType.MartialArts => new HobbyInfo("Dövüş Sanatları", "Dövüş sanatları", HobbyCategory.Sports, 300),
                HobbyType.Dancing => new HobbyInfo("Dans", "Dans etme", HobbyCategory.Sports, 150),
                HobbyType.Hiking => new HobbyInfo("Doğa Yürüyüşü", "Doğa yürüyüşü", HobbyCategory.Sports, 50),
                HobbyType.Cycling => new HobbyInfo("Bisiklet", "Bisiklet sürme", HobbyCategory.Sports, 500),
                HobbyType.Yoga => new HobbyInfo("Yoga", "Yoga yapma", HobbyCategory.Sports, 100),

                // Sanat
                HobbyType.Painting => new HobbyInfo("Resim", "Resim yapma", HobbyCategory.Art, 200),
                HobbyType.Drawing => new HobbyInfo("Çizim", "Çizim yapma", HobbyCategory.Art, 50),
                HobbyType.Photography => new HobbyInfo("Fotoğrafçılık", "Fotoğraf çekme", HobbyCategory.Art, 1000),
                HobbyType.Writing => new HobbyInfo("Yazarlık", "Yazı yazma", HobbyCategory.Art, 0),
                HobbyType.Music => new HobbyInfo("Müzik", "Müzik yapma", HobbyCategory.Art, 500),
                HobbyType.Sculpting => new HobbyInfo("Heykel", "Heykel yapma", HobbyCategory.Art, 300),

                // Eğlence
                HobbyType.Gaming => new HobbyInfo("Video Oyunları", "Video oyunu oynama", HobbyCategory.Entertainment, 300),
                HobbyType.Reading => new HobbyInfo("Kitap Okuma", "Kitap okuma", HobbyCategory.Entertainment, 50),
                HobbyType.Movies => new HobbyInfo("Film İzleme", "Film izleme", HobbyCategory.Entertainment, 50),
                HobbyType.Cooking => new HobbyInfo("Yemek Yapma", "Yemek yapma", HobbyCategory.Entertainment, 100),
                HobbyType.Gardening => new HobbyInfo("Bahçecilik", "Bahçe işleri", HobbyCategory.Entertainment, 200),
                HobbyType.Fishing => new HobbyInfo("Balık Tutma", "Balık tutma", HobbyCategory.Entertainment, 300),

                // Sosyal
                HobbyType.Volunteering => new HobbyInfo("Gönüllülük", "Gönüllü çalışma", HobbyCategory.Social, 0),
                HobbyType.Traveling => new HobbyInfo("Seyahat", "Seyahat etme", HobbyCategory.Social, 1000),
                HobbyType.Socializing => new HobbyInfo("Sosyalleşme", "Sosyal aktiviteler", HobbyCategory.Social, 100),
                HobbyType.Blogging => new HobbyInfo("Blog Yazma", "Blog yazma", HobbyCategory.Social, 0),

                // Koleksiyon
                HobbyType.Collecting => new HobbyInfo("Koleksiyon", "Koleksiyon yapma", HobbyCategory.Collection, 500),
                HobbyType.Antiques => new HobbyInfo("Antika", "Antika toplama", HobbyCategory.Collection, 2000),

                // Diğer
                HobbyType.Meditation => new HobbyInfo("Meditasyon", "Meditasyon yapma", HobbyCategory.Other, 0),
                HobbyType.Chess => new HobbyInfo("Satranç", "Satranç oynama", HobbyCategory.Other, 50),
                HobbyType.Coding => new HobbyInfo("Programlama", "Kod yazma", HobbyCategory.Other, 0),

                _ => new HobbyInfo("Bilinmeyen", "Bilinmeyen hobi", HobbyCategory.Other, 0)
            };
        }
    }

    /// <summary>
    /// Yetenek bilgi yapısı.
    /// </summary>
    public struct TalentInfo
    {
        public string Name;
        public string Description;
        public TalentCategory Category;
        public int Difficulty;  // 1-10

        public TalentInfo(string name, string description, TalentCategory category, int difficulty)
        {
            Name = name;
            Description = description;
            Category = category;
            Difficulty = difficulty;
        }
    }

    /// <summary>
    /// Hobi bilgi yapısı.
    /// </summary>
    public struct HobbyInfo
    {
        public string Name;
        public string Description;
        public HobbyCategory Category;
        public decimal MonthlyCost;  // TL

        public HobbyInfo(string name, string description, HobbyCategory category, decimal monthlyCost)
        {
            Name = name;
            Description = description;
            Category = category;
            MonthlyCost = monthlyCost;
        }
    }

    /// <summary>
    /// Yetenek kategorileri.
    /// </summary>
    public enum TalentCategory
    {
        Music,
        Art,
        Sports,
        Academic,
        Other
    }

    /// <summary>
    /// Hobi kategorileri.
    /// </summary>
    public enum HobbyCategory
    {
        Sports,
        Art,
        Entertainment,
        Social,
        Collection,
        Other
    }

    #endregion
}
