using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Character
{
    #region Asset System

    /// <summary>
    /// Varlık verisi - Ev, araba vb.
    /// </summary>
    [Serializable]
    public class AssetData
    {
        public List<Property> properties = new List<Property>();
        public List<Vehicle> vehicles = new List<Vehicle>();
        public List<Business> businesses = new List<Business>();
        public List<Investment> investments = new List<Investment>();

        public decimal TotalAssetValue
        {
            get
            {
                decimal total = 0;
                foreach (var p in properties) total += p.currentValue;
                foreach (var v in vehicles) total += v.currentValue;
                foreach (var b in businesses) total += b.value;
                foreach (var i in investments) total += i.currentValue;
                return total;
            }
        }
    }

    /// <summary>
    /// Gayrimenkul.
    /// </summary>
    [Serializable]
    public class Property
    {
        public string id;
        public string name;
        public PropertyType type;
        public string city;
        public int squareMeters;
        public decimal purchasePrice;
        public decimal currentValue;
        public int purchaseYear;
        public bool isRented;
        public decimal monthlyRent;
        public int condition; // 0-100
    }

    public enum PropertyType
    {
        Apartment,      // Daire
        House,          // Müstakil ev
        Villa,          // Villa
        Land,           // Arsa
        Commercial,     // İşyeri
        Farm            // Çiftlik
    }

    /// <summary>
    /// Araç.
    /// </summary>
    [Serializable]
    public class Vehicle
    {
        public string id;
        public string brand;
        public string model;
        public int year;
        public VehicleType type;
        public decimal purchasePrice;
        public decimal currentValue;
        public int condition; // 0-100
        public int mileage;
        public bool isInsured;
    }

    public enum VehicleType
    {
        Car,            // Otomobil
        Motorcycle,     // Motosiklet
        Bicycle,        // Bisiklet
        Boat,           // Tekne
        Yacht,          // Yat
        Helicopter,     // Helikopter
        Plane           // Uçak
    }

    /// <summary>
    /// İş/Şirket.
    /// </summary>
    [Serializable]
    public class Business
    {
        public string id;
        public string name;
        public BusinessType type;
        public decimal value;
        public decimal monthlyRevenue;
        public decimal monthlyExpenses;
        public int employees;
        public int reputation; // 0-100
        public int yearsOwned;
    }

    public enum BusinessType
    {
        Restaurant,     // Restoran
        Cafe,           // Kafe
        Shop,           // Dükkan
        Gym,            // Spor salonu
        Salon,          // Güzellik salonu
        Nightclub,      // Gece kulübü
        Hotel,          // Otel
        Farm,           // Çiftlik
        Factory,        // Fabrika
        TechStartup,    // Teknoloji girişimi
        RealEstate      // Emlak
    }

    /// <summary>
    /// Yatırım.
    /// </summary>
    [Serializable]
    public class Investment
    {
        public string id;
        public InvestmentType type;
        public string name;
        public decimal purchasePrice;
        public decimal currentValue;
        public int shares;
        public int purchaseYear;
    }

    public enum InvestmentType
    {
        Stock,          // Hisse senedi
        Crypto,         // Kripto para
        Gold,           // Altın
        Bond,           // Tahvil
        MutualFund,     // Yatırım fonu
        RealEstate      // Gayrimenkul fonu
    }

    #endregion

    #region Crime System

    /// <summary>
    /// Suç kaydı verisi.
    /// </summary>
    [Serializable]
    public class CrimeData
    {
        public List<CrimeRecord> crimeHistory = new List<CrimeRecord>();
        public List<PrisonSentence> prisonHistory = new List<PrisonSentence>();
        public int totalCrimesCommitted;
        public int timesCaught;
        public int timesEscaped;
        public bool isInPrison;
        public int prisonYearsRemaining;
        public int notoriety; // 0-100 Şöhret/Kötü ün
    }

    /// <summary>
    /// Suç kaydı.
    /// </summary>
    [Serializable]
    public class CrimeRecord
    {
        public string id;
        public CrimeType type;
        public int ageCommitted;
        public bool wasCaught;
        public decimal moneyGained;
        public string victimName;
    }

    public enum CrimeType
    {
        Pickpocket,         // Yankesicilik
        Shoplifting,        // Hırsızlık (mağaza)
        BurglaryCar,        // Araba hırsızlığı
        BurglaryHome,       // Ev soygunu
        BankRobbery,        // Banka soygunu
        GrandTheftAuto,     // Büyük araba hırsızlığı
        Assault,            // Saldırı
        Murder,             // Cinayet
        DrugDealing,        // Uyuşturucu satıcılığı
        Fraud,              // Dolandırıcılık
        Extortion,          // Şantaj
        Kidnapping,         // Adam kaçırma
        Arson,              // Kundaklama
        Hacking,            // Hackleme
        TaxEvasion,         // Vergi kaçırma
        MoneyLaundering     // Kara para aklama
    }

    /// <summary>
    /// Hapis cezası.
    /// </summary>
    [Serializable]
    public class PrisonSentence
    {
        public string id;
        public CrimeType crimeType;
        public int sentenceYears;
        public int yearsServed;
        public int ageEntered;
        public bool escaped;
        public bool paroled;
        public string prisonName;
    }

    #endregion

    #region Health System

    /// <summary>
    /// Sağlık verisi.
    /// </summary>
    [Serializable]
    public class HealthData
    {
        public List<Disease> currentDiseases = new List<Disease>();
        public List<Disease> diseaseHistory = new List<Disease>();
        public List<string> allergies = new List<string>();
        public BloodType bloodType;
        public int fitness; // 0-100
        public int mentalHealth; // 0-100
        public bool hasInsurance;
        public bool isAddicted;
        public List<Addiction> addictions = new List<Addiction>();
        public int doctorVisits;
        public int hospitalizations;
    }

    /// <summary>
    /// Hastalık.
    /// </summary>
    [Serializable]
    public class Disease
    {
        public string id;
        public string name;
        public DiseaseType type;
        public DiseaseSeverity severity;
        public bool isChronic;
        public bool isCured;
        public int yearDiagnosed;
        public int healthImpact; // Yıllık sağlık etkisi
    }

    public enum DiseaseType
    {
        Cold,               // Soğuk algınlığı
        Flu,                // Grip
        Allergy,            // Alerji
        Asthma,             // Astım
        Diabetes,           // Şeker hastalığı
        Hypertension,       // Hipertansiyon
        HeartDisease,       // Kalp hastalığı
        Cancer,             // Kanser
        Depression,         // Depresyon
        Anxiety,            // Anksiyete
        ADHD,               // Dikkat eksikliği
        Alzheimer,          // Alzheimer
        Arthritis,          // Eklem iltihabı
        HIV,                // HIV
        COVID,              // COVID
        Hepatitis,          // Hepatit
        Tuberculosis        // Verem
    }

    public enum DiseaseSeverity
    {
        Mild,       // Hafif
        Moderate,   // Orta
        Severe,     // Ciddi
        Critical    // Kritik
    }

    public enum BloodType
    {
        APositive,
        ANegative,
        BPositive,
        BNegative,
        ABPositive,
        ABNegative,
        OPositive,
        ONegative
    }

    /// <summary>
    /// Bağımlılık.
    /// </summary>
    [Serializable]
    public class Addiction
    {
        public string name;
        public AddictionType type;
        public int severity; // 0-100
        public int yearsAddicted;
        public bool inRecovery;
    }

    public enum AddictionType
    {
        Alcohol,        // Alkol
        Tobacco,        // Sigara
        Gambling,       // Kumar
        Drugs,          // Uyuşturucu
        Gaming,         // Oyun
        SocialMedia,    // Sosyal medya
        Shopping        // Alışveriş
    }

    #endregion

    #region Social Media / Fame System

    /// <summary>
    /// Sosyal medya verisi.
    /// </summary>
    [Serializable]
    public class SocialMediaData
    {
        public List<SocialAccount> accounts = new List<SocialAccount>();
        public int totalFollowers;
        public int postsPublished;
        public bool isVerified;
        public bool isInfluencer;
        public decimal monthlyEarnings;
    }

    /// <summary>
    /// Sosyal medya hesabı.
    /// </summary>
    [Serializable]
    public class SocialAccount
    {
        public string id;
        public SocialPlatform platform;
        public string username;
        public int followers;
        public int following;
        public int posts;
        public bool isVerified;
        public int engagement; // 0-100
    }

    public enum SocialPlatform
    {
        Instagram,
        Twitter,
        TikTok,
        YouTube,
        Twitch,
        LinkedIn,
        Facebook
    }

    /// <summary>
    /// Şöhret verisi.
    /// </summary>
    [Serializable]
    public class FameData
    {
        public FameType fameType;
        public int fameLevel; // 0-100
        public List<string> achievements = new List<string>();
        public List<string> awards = new List<string>();
        public int mediaAppearances;
        public int scandals;
        public bool hasAgent;
        public bool hasPublicist;
    }

    public enum FameType
    {
        None,
        LocalCelebrity,     // Yerel ünlü
        Influencer,         // Influencer
        Singer,             // Şarkıcı
        Actor,              // Oyuncu
        Athlete,            // Sporcu
        Politician,         // Politikacı
        Author,             // Yazar
        Chef,               // Şef
        Model,              // Model
        Entrepreneur        // Girişimci
    }

    #endregion

    #region Military System

    /// <summary>
    /// Askerlik verisi.
    /// </summary>
    [Serializable]
    public class MilitaryData
    {
        public bool hasServed;
        public bool isCurrentlyServing;
        public MilitaryBranch branch;
        public MilitaryRank rank;
        public int yearsServed;
        public int deploymentsCompleted;
        public List<string> medals = new List<string>();
        public bool wasWounded;
        public bool isPOW; // Prisoner of War
    }

    public enum MilitaryBranch
    {
        None,
        Army,           // Kara Kuvvetleri
        Navy,           // Deniz Kuvvetleri
        AirForce,       // Hava Kuvvetleri
        Gendarmerie,    // Jandarma
        CoastGuard      // Sahil Güvenlik
    }

    public enum MilitaryRank
    {
        Private,        // Er
        Corporal,       // Onbaşı
        Sergeant,       // Çavuş
        Lieutenant,     // Teğmen
        Captain,        // Yüzbaşı
        Major,          // Binbaşı
        Colonel,        // Albay
        General         // General
    }

    #endregion

    #region Gambling System

    /// <summary>
    /// Kumar verisi.
    /// </summary>
    [Serializable]
    public class GamblingData
    {
        public decimal totalWon;
        public decimal totalLost;
        public int timesGambled;
        public int biggestWin;
        public int biggestLoss;
        public bool isBanned; // Casinolardan yasaklı
        public List<GamblingRecord> history = new List<GamblingRecord>();
    }

    /// <summary>
    /// Kumar kaydı.
    /// </summary>
    [Serializable]
    public class GamblingRecord
    {
        public GamblingType type;
        public decimal betAmount;
        public decimal winAmount;
        public int year;
    }

    public enum GamblingType
    {
        Lottery,        // Piyango
        Casino,         // Casino
        SportsBetting,  // Spor bahis
        Poker,          // Poker
        Blackjack,      // Blackjack
        Roulette,       // Rulet
        SlotMachine,    // Slot makinesi
        HorseRacing     // At yarışı
    }

    #endregion

    #region Pet System

    /// <summary>
    /// Evcil hayvan.
    /// </summary>
    [Serializable]
    public class Pet
    {
        public string id;
        public string name;
        public PetType type;
        public string breed;
        public int age;
        public int health; // 0-100
        public int happiness; // 0-100
        public int bond; // 0-100 Sahiple bağ
        public bool isAlive;
        public int purchaseYear;
    }

    public enum PetType
    {
        Dog,        // Köpek
        Cat,        // Kedi
        Bird,       // Kuş
        Fish,       // Balık
        Hamster,    // Hamster
        Rabbit,     // Tavşan
        Turtle,     // Kaplumbağa
        Snake,      // Yılan
        Horse       // At
    }

    #endregion

    #region Travel System

    /// <summary>
    /// Seyahat kaydı.
    /// </summary>
    [Serializable]
    public class TravelRecord
    {
        public string id;
        public string destination;
        public string country;
        public TravelType type;
        public int year;
        public int duration; // Gün
        public decimal cost;
        public List<string> activities = new List<string>();
    }

    public enum TravelType
    {
        Vacation,       // Tatil
        Business,       // İş seyahati
        Honeymoon,      // Balayı
        Adventure,      // Macera
        Cultural,       // Kültür turu
        Medical,        // Sağlık turizmi
        Religious,      // Dini ziyaret
        Educational     // Eğitim
    }

    #endregion

    #region Lottery & Gambling Results

    /// <summary>
    /// Piyango bileti.
    /// </summary>
    [Serializable]
    public class LotteryTicket
    {
        public string id;
        public LotteryType type;
        public decimal cost;
        public decimal prize;
        public bool won;
        public int year;
    }

    public enum LotteryType
    {
        Sayisal,        // Sayısal Loto
        SuperLoto,      // Süper Loto
        SansTopu,       // Şans Topu
        OnNumara,       // On Numara
        MilliPiyango,   // Milli Piyango
        Kazıkazan       // Kazı kazan
    }

    #endregion

    #region Will & Inheritance

    /// <summary>
    /// Vasiyet.
    /// </summary>
    [Serializable]
    public class Will
    {
        public bool hasWill;
        public List<WillBeneficiary> beneficiaries = new List<WillBeneficiary>();
        public string notes;
        public int lastUpdated;
    }

    /// <summary>
    /// Vasiyet lehdarı.
    /// </summary>
    [Serializable]
    public class WillBeneficiary
    {
        public string npcId;
        public string npcName;
        public float percentage; // 0-100
        public List<string> specificItems = new List<string>();
    }

    /// <summary>
    /// Miras kaydı.
    /// </summary>
    [Serializable]
    public class InheritanceRecord
    {
        public string fromNpcId;
        public string fromNpcName;
        public decimal amount;
        public List<string> items = new List<string>();
        public int yearReceived;
    }

    #endregion
}
