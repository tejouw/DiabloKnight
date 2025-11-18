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
        public HealthData health;
        public CrimeData crime;
        public PropertyData properties;
        public SocialMediaData socialMedia;
        public MilitaryData military;
        public List<HobbyData> hobbies;
        public LegalData legal;

        // Bayraklar
        public bool isMarried;
        public bool hasCompletedMilitary;
        public bool isEmployed;
        public bool isInPrison;
        public bool isPregnant;
        public bool hasDrivingLicense;
        public bool isRetired;
        public bool isHomeless;
        public bool isAddicted;

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
            health = new HealthData();
            crime = new CrimeData();
            properties = new PropertyData();
            socialMedia = new SocialMediaData();
            military = new MilitaryData();
            hobbies = new List<HobbyData>();
            legal = new LegalData();
        }

        // Ek property'ler
        public HealthData Health => health;
        public CrimeData Crime => crime;
        public PropertyData Properties => properties;
        public SocialMediaData SocialMedia => socialMedia;
        public MilitaryData Military => military;
        public List<HobbyData> Hobbies => hobbies;
        public LegalData Legal => legal;
        public bool IsInPrison => isInPrison;
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

    /// <summary>
    /// Sağlık verisi.
    /// </summary>
    [System.Serializable]
    public class HealthData
    {
        public List<Disease> diseases = new List<Disease>();
        public List<string> allergies = new List<string>();
        public List<string> medications = new List<string>();
        public bool hasInsurance = false;
        public InsuranceType insuranceType = InsuranceType.None;
        public int doctorVisitsThisYear = 0;
        public List<Surgery> surgeries = new List<Surgery>();
        public float mentalHealth = 100f;
        public bool isOnDiet = false;
        public bool isSmoking = false;
        public bool isDrinking = false;
        public int fertilityStat = 100;
    }

    [System.Serializable]
    public class Disease
    {
        public string id;
        public string name;
        public DiseaseSeverity severity;
        public bool isChronic;
        public bool isTreatable;
        public float treatmentCost;
        public int yearDiagnosed;
        public bool isBeingTreated;
    }

    public enum DiseaseSeverity
    {
        Mild,
        Moderate,
        Severe,
        Critical,
        Terminal
    }

    public enum InsuranceType
    {
        None,
        SGK,
        Private,
        Premium
    }

    [System.Serializable]
    public class Surgery
    {
        public string name;
        public int year;
        public bool wasSuccessful;
        public float cost;
    }

    /// <summary>
    /// Suç verisi.
    /// </summary>
    [System.Serializable]
    public class CrimeData
    {
        public List<Crime> crimeHistory = new List<Crime>();
        public int prisonYearsRemaining = 0;
        public int totalPrisonTime = 0;
        public bool hasEscaped = false;
        public int escapeAttempts = 0;
        public float prisonReputation = 50f;
        public bool isOnProbation = false;
        public int probationYearsRemaining = 0;
        public bool hasCriminalRecord = false;
    }

    [System.Serializable]
    public class Crime
    {
        public string id;
        public string name;
        public CrimeType type;
        public int yearCommitted;
        public bool wasCaught;
        public int sentenceYears;
        public float fine;
        public bool wasConvicted;
    }

    public enum CrimeType
    {
        Theft,
        Robbery,
        Burglary,
        Assault,
        Murder,
        Fraud,
        DrugPossession,
        DrugTrafficking,
        DUI,
        Vandalism,
        Shoplifting,
        TaxEvasion,
        Bribery,
        Extortion,
        Kidnapping,
        Arson
    }

    /// <summary>
    /// Mülk verisi.
    /// </summary>
    [System.Serializable]
    public class PropertyData
    {
        public List<Property> ownedProperties = new List<Property>();
        public List<Vehicle> ownedVehicles = new List<Vehicle>();
        public Property currentResidence;
        public bool isRenting = false;
        public decimal monthlyRent = 0;
    }

    [System.Serializable]
    public class Property
    {
        public string id;
        public string name;
        public PropertyType type;
        public string location;
        public decimal purchasePrice;
        public decimal currentValue;
        public int yearPurchased;
        public float condition = 100f;
        public bool isMortgaged;
        public decimal mortgageRemaining;
        public decimal monthlyMortgagePayment;
    }

    public enum PropertyType
    {
        Apartment,
        House,
        Villa,
        Mansion,
        Land,
        CommercialBuilding,
        Office
    }

    [System.Serializable]
    public class Vehicle
    {
        public string id;
        public string brand;
        public string model;
        public int year;
        public VehicleType type;
        public decimal purchasePrice;
        public decimal currentValue;
        public float condition = 100f;
        public int mileage;
        public bool isFinanced;
        public decimal loanRemaining;
    }

    public enum VehicleType
    {
        Car,
        Motorcycle,
        Bicycle,
        Boat,
        Yacht,
        Helicopter,
        Jet,
        SportsCar,
        SUV,
        Truck
    }

    /// <summary>
    /// Sosyal medya verisi.
    /// </summary>
    [System.Serializable]
    public class SocialMediaData
    {
        public List<SocialMediaAccount> accounts = new List<SocialMediaAccount>();
        public int totalFollowers = 0;
        public bool isVerified = false;
        public bool isInfluencer = false;
        public decimal monthlyEarnings = 0;
    }

    [System.Serializable]
    public class SocialMediaAccount
    {
        public string platform;
        public string username;
        public int followers;
        public int posts;
        public float engagementRate;
        public bool isVerified;
    }

    /// <summary>
    /// Askerlik verisi.
    /// </summary>
    [System.Serializable]
    public class MilitaryData
    {
        public MilitaryStatus status = MilitaryStatus.NotServed;
        public MilitaryBranch branch;
        public MilitaryRank rank;
        public int serviceMonthsRemaining = 0;
        public int totalServiceMonths = 0;
        public bool wasDischarged = false;
        public string dischargeReason = "";
        public List<string> medals = new List<string>();
        public bool paidExemption = false;
    }

    public enum MilitaryStatus
    {
        NotServed,
        Serving,
        Completed,
        Exempted,
        Deferred,
        Deserted
    }

    public enum MilitaryBranch
    {
        None,
        Army,
        Navy,
        AirForce,
        Gendarmerie,
        CoastGuard
    }

    public enum MilitaryRank
    {
        None,
        Private,
        Corporal,
        Sergeant,
        Lieutenant,
        Captain,
        Major,
        Colonel,
        General
    }

    /// <summary>
    /// Hobi verisi.
    /// </summary>
    [System.Serializable]
    public class HobbyData
    {
        public string id;
        public string name;
        public HobbyCategory category;
        public int skillLevel = 0; // 0-100
        public int yearsActive = 0;
        public decimal totalInvestment = 0;
        public List<string> achievements = new List<string>();
    }

    public enum HobbyCategory
    {
        Sports,
        Music,
        Art,
        Gaming,
        Reading,
        Writing,
        Cooking,
        Photography,
        Travel,
        Fitness,
        Dance,
        MartialArts,
        Gardening,
        Collecting,
        Technology
    }

    /// <summary>
    /// Hukuki veri.
    /// </summary>
    [System.Serializable]
    public class LegalData
    {
        public List<Lawsuit> activeLawsuits = new List<Lawsuit>();
        public List<Lawsuit> completedLawsuits = new List<Lawsuit>();
        public bool hasLawyer = false;
        public decimal lawyerRetainerFee = 0;
        public string lawyerName = "";
        public List<string> restrainingOrders = new List<string>();
    }

    [System.Serializable]
    public class Lawsuit
    {
        public string id;
        public string description;
        public LawsuitType type;
        public bool isPlaintiff;
        public decimal claimAmount;
        public int yearFiled;
        public LawsuitStatus status;
        public decimal settlementAmount;
    }

    public enum LawsuitType
    {
        Personal,
        Civil,
        Criminal,
        Divorce,
        Custody,
        Workplace,
        MedicalMalpractice,
        PropertyDispute
    }

    public enum LawsuitStatus
    {
        Pending,
        InProgress,
        Won,
        Lost,
        Settled,
        Dismissed
    }

    /// <summary>
    /// Stat tipi enum.
    /// </summary>
    public enum StatType
    {
        Health,
        Happiness,
        Intelligence,
        Appearance,
        Fame
    }

    /// <summary>
    /// Yaşam evresi enum.
    /// </summary>
    public enum LifeStage
    {
        Baby,
        Child,
        Teen,
        YoungAdult,
        Adult,
        Senior
    }
}
