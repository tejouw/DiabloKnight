using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Character
{
    /// <summary>
    /// Karakter verisi - Oyuncunun karakterini temsil eder.
    /// </summary>
    [System.Serializable]
    public class CharacterData
    {
        // Temel bilgiler
        public string firstName;
        public string lastName;
        public Gender gender;
        public int age;
        public string birthDate;
        public string birthCity;

        // Stat sistemi
        public CharacterStats stats;

        // Alt sistemler
        public EducationData education;
        public CareerData career;
        public FinancialData finances;
        public List<Relationship> relationships;

        // Bayraklar
        public bool isMarried;
        public bool hasCompletedMilitary;
        public bool isEmployed;

        #region Properties

        public string FullName => $"{firstName} {lastName}";

        public LifeStage CurrentLifeStage
        {
            get
            {
                if (age < 5) return LifeStage.Baby;
                if (age < 12) return LifeStage.Child;
                if (age < 18) return LifeStage.Teen;
                if (age < 30) return LifeStage.YoungAdult;
                if (age < 60) return LifeStage.Adult;
                return LifeStage.Senior;
            }
        }

        public string FirstName => firstName;
        public string LastName => lastName;
        public int Age => age;
        public Gender Gender => gender;
        public CharacterStats Stats => stats;
        public EducationData Education => education;
        public CareerData Career => career;
        public FinancialData Finances => finances;
        public List<Relationship> Relationships => relationships;
        public bool IsMarried => isMarried;

        #endregion

        /// <summary>
        /// Yeni karakter oluştur.
        /// </summary>
        public CharacterData()
        {
            stats = new CharacterStats();
            education = new EducationData();
            career = new CareerData();
            finances = new FinancialData();
            relationships = new List<Relationship>();
        }
    }

    /// <summary>
    /// Cinsiyet enum.
    /// </summary>
    public enum Gender
    {
        Male,
        Female
    }

    /// <summary>
    /// Karakter statları.
    /// </summary>
    [System.Serializable]
    public class CharacterStats
    {
        [Range(0, 100)] public int health = 100;
        [Range(0, 100)] public int happiness = 50;
        [Range(0, 100)] public int intelligence = 50;
        [Range(0, 100)] public int appearance = 50;
        [Range(0, 100)] public int fame = 0;

        public int Health => health;
        public int Happiness => happiness;
        public int Intelligence => intelligence;
        public int Appearance => appearance;
        public int Fame => fame;

        /// <summary>
        /// Stat değerini değiştir.
        /// </summary>
        public void ModifyStat(StatType type, int amount)
        {
            int oldValue = GetStat(type);
            int newValue;

            switch (type)
            {
                case StatType.Health:
                    health = Mathf.Clamp(health + amount, 0, 100);
                    newValue = health;
                    break;
                case StatType.Happiness:
                    happiness = Mathf.Clamp(happiness + amount, 0, 100);
                    newValue = happiness;
                    break;
                case StatType.Intelligence:
                    intelligence = Mathf.Clamp(intelligence + amount, 0, 100);
                    newValue = intelligence;
                    break;
                case StatType.Appearance:
                    appearance = Mathf.Clamp(appearance + amount, 0, 100);
                    newValue = appearance;
                    break;
                case StatType.Fame:
                    fame = Mathf.Clamp(fame + amount, 0, 100);
                    newValue = fame;
                    break;
                default:
                    return;
            }

            // Event yayınla
            EventBus.Publish(new StatChangedEvent
            {
                StatType = type,
                OldValue = oldValue,
                NewValue = newValue,
                Delta = amount
            });
        }

        /// <summary>
        /// Stat değerini al.
        /// </summary>
        public int GetStat(StatType type)
        {
            return type switch
            {
                StatType.Health => health,
                StatType.Happiness => happiness,
                StatType.Intelligence => intelligence,
                StatType.Appearance => appearance,
                StatType.Fame => fame,
                _ => 0
            };
        }

        /// <summary>
        /// Stat değerini direkt ayarla.
        /// </summary>
        public void SetStat(StatType type, int value)
        {
            int oldValue = GetStat(type);
            value = Mathf.Clamp(value, 0, 100);

            switch (type)
            {
                case StatType.Health:
                    health = value;
                    break;
                case StatType.Happiness:
                    happiness = value;
                    break;
                case StatType.Intelligence:
                    intelligence = value;
                    break;
                case StatType.Appearance:
                    appearance = value;
                    break;
                case StatType.Fame:
                    fame = value;
                    break;
            }

            // Event yayınla
            EventBus.Publish(new StatChangedEvent
            {
                StatType = type,
                OldValue = oldValue,
                NewValue = value,
                Delta = value - oldValue
            });
        }
    }

    /// <summary>
    /// Eğitim verisi.
    /// </summary>
    [System.Serializable]
    public class EducationData
    {
        public EducationLevel currentLevel = EducationLevel.None;
        public string schoolName = "";
        public float gpa = 0f;
        public List<string> achievements = new List<string>();
        public bool isGraduated = false;
        public int yksScore = 0;
        public string universityName = "";
        public string department = "";

        public EducationLevel CurrentLevel => currentLevel;
    }

    /// <summary>
    /// Eğitim seviyesi enum.
    /// </summary>
    public enum EducationLevel
    {
        None = 0,
        PrimarySchool = 1,      // İlkokul
        MiddleSchool = 2,       // Ortaokul
        HighSchool = 3,         // Lise
        University = 4,         // Üniversite
        Masters = 5,            // Yüksek Lisans
        Doctorate = 6           // Doktora
    }

    /// <summary>
    /// Kariyer verisi.
    /// </summary>
    [System.Serializable]
    public class CareerData
    {
        public Job currentJob;
        public int yearsInJob = 0;
        public int performanceRating = 50;
        public List<Job> jobHistory = new List<Job>();

        public Job CurrentJob => currentJob;
    }

    /// <summary>
    /// İş tanımı.
    /// </summary>
    [System.Serializable]
    public class Job
    {
        public string id;
        public string title;
        public string company;
        public string category;
        public decimal baseSalary;
        public int yearsWorked;
    }

    /// <summary>
    /// Finansal veri.
    /// </summary>
    [System.Serializable]
    public class FinancialData
    {
        public decimal currentMoney = 0;
        public decimal totalEarned = 0;
        public decimal totalSpent = 0;
        public List<string> assets = new List<string>();
        public List<string> debts = new List<string>();

        public decimal CurrentMoney => currentMoney;

        /// <summary>
        /// Para değiştir.
        /// </summary>
        public void ModifyMoney(decimal amount, string reason)
        {
            decimal oldAmount = currentMoney;
            currentMoney += amount;

            if (amount > 0)
            {
                totalEarned += amount;
            }
            else
            {
                totalSpent += Math.Abs(amount);
            }

            // Negatife düşmesin
            if (currentMoney < 0)
            {
                currentMoney = 0;
            }

            // Event yayınla
            EventBus.Publish(new MoneyChangedEvent
            {
                OldAmount = oldAmount,
                NewAmount = currentMoney,
                Delta = amount,
                Reason = reason
            });
        }
    }

    /// <summary>
    /// İlişki verisi.
    /// </summary>
    [System.Serializable]
    public class Relationship
    {
        public string npcId;
        public string npcName;
        public RelationType type;
        public int intimacy = 50;       // 0-100
        public int trust = 50;          // 0-100
        public RelationshipStatus status;
        public List<string> memories = new List<string>();
        public int age;
        public Gender gender;
    }

    /// <summary>
    /// İlişki türü enum.
    /// </summary>
    public enum RelationType
    {
        Parent,
        Sibling,
        Child,
        Spouse,
        ExSpouse,
        Friend,
        BestFriend,
        Enemy,
        Colleague,
        Boyfriend,
        Girlfriend,
        Ex,
        Acquaintance
    }

    /// <summary>
    /// İlişki durumu enum.
    /// </summary>
    public enum RelationshipStatus
    {
        Active,
        Distant,
        Broken,
        Deceased
    }
}
