using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Varlık Sistemi - Araba, ev, yatırım yönetimi.
    /// </summary>
    public class AssetSystem : Singleton<AssetSystem>
    {
        private List<Vehicle> _availableVehicles = new List<Vehicle>();
        private List<Property> _availableProperties = new List<Property>();
        private List<OwnedAsset> _ownedAssets = new List<OwnedAsset>();

        public List<OwnedAsset> OwnedAssets => _ownedAssets;

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeAssets();
            Debug.Log("[AssetSystem] Initialized successfully.");
        }

        private void InitializeAssets()
        {
            // ARAÇLAR
            _availableVehicles = new List<Vehicle>
            {
                // Ekonomik
                new Vehicle
                {
                    id = "bicycle",
                    name = "Bisiklet",
                    brand = "Bianchi",
                    type = VehicleType.Bicycle,
                    basePrice = 3000,
                    annualMaintenance = 100,
                    happinessBonus = 2,
                    prestigeBonus = 0
                },
                new Vehicle
                {
                    id = "scooter",
                    name = "Scooter",
                    brand = "Vespa",
                    type = VehicleType.Motorcycle,
                    basePrice = 35000,
                    annualMaintenance = 2000,
                    happinessBonus = 5,
                    prestigeBonus = 1
                },
                new Vehicle
                {
                    id = "economy_car",
                    name = "Ekonomik Araba",
                    brand = "Fiat Egea",
                    type = VehicleType.Car,
                    basePrice = 450000,
                    annualMaintenance = 15000,
                    happinessBonus = 8,
                    prestigeBonus = 2
                },
                new Vehicle
                {
                    id = "sedan",
                    name = "Sedan",
                    brand = "Toyota Corolla",
                    type = VehicleType.Car,
                    basePrice = 850000,
                    annualMaintenance = 25000,
                    happinessBonus = 10,
                    prestigeBonus = 5
                },
                new Vehicle
                {
                    id = "suv",
                    name = "SUV",
                    brand = "Volkswagen Tiguan",
                    type = VehicleType.Car,
                    basePrice = 1500000,
                    annualMaintenance = 40000,
                    happinessBonus = 12,
                    prestigeBonus = 8
                },
                new Vehicle
                {
                    id = "luxury_sedan",
                    name = "Lüks Sedan",
                    brand = "Mercedes E-Class",
                    type = VehicleType.Car,
                    basePrice = 3500000,
                    annualMaintenance = 80000,
                    happinessBonus = 15,
                    prestigeBonus = 15
                },
                new Vehicle
                {
                    id = "sports_car",
                    name = "Spor Araba",
                    brand = "Porsche 911",
                    type = VehicleType.Car,
                    basePrice = 8000000,
                    annualMaintenance = 150000,
                    happinessBonus = 20,
                    prestigeBonus = 25
                },
                new Vehicle
                {
                    id = "supercar",
                    name = "Süper Araba",
                    brand = "Ferrari F8",
                    type = VehicleType.Car,
                    basePrice = 25000000,
                    annualMaintenance = 500000,
                    happinessBonus = 30,
                    prestigeBonus = 40
                },
                new Vehicle
                {
                    id = "yacht",
                    name = "Yat",
                    brand = "Azimut",
                    type = VehicleType.Boat,
                    basePrice = 50000000,
                    annualMaintenance = 2000000,
                    happinessBonus = 35,
                    prestigeBonus = 50
                },
                new Vehicle
                {
                    id = "helicopter",
                    name = "Helikopter",
                    brand = "Bell",
                    type = VehicleType.Aircraft,
                    basePrice = 100000000,
                    annualMaintenance = 5000000,
                    happinessBonus = 40,
                    prestigeBonus = 60
                }
            };

            // EMLAK
            _availableProperties = new List<Property>
            {
                new Property
                {
                    id = "studio",
                    name = "Stüdyo Daire",
                    type = PropertyType.Apartment,
                    location = "Varoş",
                    basePrice = 500000,
                    monthlyRent = 5000,
                    annualMaintenance = 10000,
                    happinessBonus = 3,
                    prestigeBonus = 1
                },
                new Property
                {
                    id = "apartment_2_1",
                    name = "2+1 Daire",
                    type = PropertyType.Apartment,
                    location = "Şehir Merkezi",
                    basePrice = 1500000,
                    monthlyRent = 12000,
                    annualMaintenance = 25000,
                    happinessBonus = 8,
                    prestigeBonus = 5
                },
                new Property
                {
                    id = "apartment_3_1",
                    name = "3+1 Daire",
                    type = PropertyType.Apartment,
                    location = "İyi Semt",
                    basePrice = 3000000,
                    monthlyRent = 20000,
                    annualMaintenance = 40000,
                    happinessBonus = 12,
                    prestigeBonus = 10
                },
                new Property
                {
                    id = "penthouse",
                    name = "Penthouse",
                    type = PropertyType.Apartment,
                    location = "Lüks Semt",
                    basePrice = 10000000,
                    monthlyRent = 50000,
                    annualMaintenance = 100000,
                    happinessBonus = 20,
                    prestigeBonus = 25
                },
                new Property
                {
                    id = "small_house",
                    name = "Müstakil Ev",
                    type = PropertyType.House,
                    location = "Banliyö",
                    basePrice = 2500000,
                    monthlyRent = 15000,
                    annualMaintenance = 50000,
                    happinessBonus = 15,
                    prestigeBonus = 12
                },
                new Property
                {
                    id = "villa",
                    name = "Villa",
                    type = PropertyType.Villa,
                    location = "Sahil",
                    basePrice = 15000000,
                    monthlyRent = 80000,
                    annualMaintenance = 200000,
                    happinessBonus = 25,
                    prestigeBonus = 35
                },
                new Property
                {
                    id = "mansion",
                    name = "Konak",
                    type = PropertyType.Mansion,
                    location = "Boğaz Manzaralı",
                    basePrice = 50000000,
                    monthlyRent = 200000,
                    annualMaintenance = 500000,
                    happinessBonus = 35,
                    prestigeBonus = 50
                },
                new Property
                {
                    id = "palace",
                    name = "Saray",
                    type = PropertyType.Mansion,
                    location = "Tarihi Yarımada",
                    basePrice = 200000000,
                    monthlyRent = 0, // Satılık değil kiralanmaz
                    annualMaintenance = 2000000,
                    happinessBonus = 50,
                    prestigeBonus = 80
                }
            };
        }

        /// <summary>
        /// Araç satın al.
        /// </summary>
        public AssetResult BuyVehicle(string vehicleId, CharacterData character)
        {
            var vehicle = _availableVehicles.Find(v => v.id == vehicleId);
            if (vehicle == null)
            {
                return new AssetResult { success = false, message = "Araç bulunamadı." };
            }

            if (character.Finances.CurrentMoney < vehicle.basePrice)
            {
                return new AssetResult { success = false, message = "Yeterli paranız yok!" };
            }

            character.Finances.ModifyMoney(-vehicle.basePrice, $"{vehicle.brand} satın alma");

            var ownedAsset = new OwnedAsset
            {
                id = System.Guid.NewGuid().ToString(),
                assetType = AssetType.Vehicle,
                name = vehicle.name,
                brand = vehicle.brand,
                purchasePrice = vehicle.basePrice,
                currentValue = vehicle.basePrice,
                annualMaintenance = vehicle.annualMaintenance,
                happinessBonus = vehicle.happinessBonus,
                prestigeBonus = vehicle.prestigeBonus,
                purchaseYear = character.Age
            };

            _ownedAssets.Add(ownedAsset);
            character.Stats.ModifyStat(StatType.Happiness, vehicle.happinessBonus);

            return new AssetResult
            {
                success = true,
                message = $"{vehicle.brand} satın aldın! Tebrikler!"
            };
        }

        /// <summary>
        /// Emlak satın al.
        /// </summary>
        public AssetResult BuyProperty(string propertyId, CharacterData character)
        {
            var property = _availableProperties.Find(p => p.id == propertyId);
            if (property == null)
            {
                return new AssetResult { success = false, message = "Emlak bulunamadı." };
            }

            if (character.Finances.CurrentMoney < property.basePrice)
            {
                return new AssetResult { success = false, message = "Yeterli paranız yok!" };
            }

            character.Finances.ModifyMoney(-property.basePrice, $"{property.name} satın alma");

            var ownedAsset = new OwnedAsset
            {
                id = System.Guid.NewGuid().ToString(),
                assetType = AssetType.Property,
                name = property.name,
                location = property.location,
                purchasePrice = property.basePrice,
                currentValue = property.basePrice,
                monthlyIncome = property.monthlyRent,
                annualMaintenance = property.annualMaintenance,
                happinessBonus = property.happinessBonus,
                prestigeBonus = property.prestigeBonus,
                purchaseYear = character.Age
            };

            _ownedAssets.Add(ownedAsset);
            character.Stats.ModifyStat(StatType.Happiness, property.happinessBonus);

            return new AssetResult
            {
                success = true,
                message = $"{property.name} ({property.location}) satın aldın!"
            };
        }

        /// <summary>
        /// Varlık sat.
        /// </summary>
        public AssetResult SellAsset(string assetId, CharacterData character)
        {
            var asset = _ownedAssets.Find(a => a.id == assetId);
            if (asset == null)
            {
                return new AssetResult { success = false, message = "Varlık bulunamadı." };
            }

            // Değer kaybı hesapla (yılda %5)
            int yearsOwned = character.Age - asset.purchaseYear;
            decimal depreciation = asset.purchasePrice * 0.05m * yearsOwned;
            decimal salePrice = asset.purchasePrice - depreciation;
            salePrice = System.Math.Max(salePrice, asset.purchasePrice * 0.3m); // Min %30

            // Emlak değer kazanabilir
            if (asset.assetType == AssetType.Property)
            {
                decimal appreciation = asset.purchasePrice * 0.03m * yearsOwned; // Yılda %3
                salePrice = asset.purchasePrice + appreciation;
            }

            character.Finances.ModifyMoney(salePrice, $"{asset.name} satışı");
            _ownedAssets.Remove(asset);

            return new AssetResult
            {
                success = true,
                message = $"{asset.name} {salePrice:N0} TL'ye sattın!"
            };
        }

        /// <summary>
        /// Yıllık bakım masraflarını öde.
        /// </summary>
        public void PayAnnualMaintenance(CharacterData character)
        {
            decimal totalMaintenance = 0;

            foreach (var asset in _ownedAssets)
            {
                totalMaintenance += asset.annualMaintenance;
            }

            if (totalMaintenance > 0)
            {
                character.Finances.ModifyMoney(-totalMaintenance, "Yıllık bakım masrafları");
            }
        }

        /// <summary>
        /// Kira geliri al.
        /// </summary>
        public decimal CollectRentalIncome(CharacterData character)
        {
            decimal totalRent = 0;

            foreach (var asset in _ownedAssets)
            {
                if (asset.assetType == AssetType.Property && asset.monthlyIncome > 0)
                {
                    totalRent += asset.monthlyIncome * 12; // Yıllık
                }
            }

            if (totalRent > 0)
            {
                character.Finances.ModifyMoney(totalRent, "Kira geliri");
            }

            return totalRent;
        }

        /// <summary>
        /// Piyango oyna.
        /// </summary>
        public LotteryResult PlayLottery(CharacterData character, int ticketCount = 1)
        {
            decimal ticketPrice = 20;
            decimal totalCost = ticketPrice * ticketCount;

            if (character.Finances.CurrentMoney < totalCost)
            {
                return new LotteryResult
                {
                    success = false,
                    message = "Yeterli paranız yok!"
                };
            }

            character.Finances.ModifyMoney(-totalCost, "Piyango bileti");

            // Kazanma şansları
            decimal winnings = 0;
            string message;

            for (int i = 0; i < ticketCount; i++)
            {
                float roll = Random.value;

                if (roll < 0.0001f) // Büyük ikramiye - 1/10000
                {
                    winnings += 10000000;
                    message = "BÜYÜK İKRAMİYE! 10 MİLYON TL KAZANDIN!";
                }
                else if (roll < 0.001f) // 1/1000
                {
                    winnings += 100000;
                }
                else if (roll < 0.01f) // 1/100
                {
                    winnings += 1000;
                }
                else if (roll < 0.1f) // 1/10
                {
                    winnings += 100;
                }
            }

            if (winnings > 0)
            {
                character.Finances.ModifyMoney(winnings, "Piyango kazancı");
                character.Stats.ModifyStat(StatType.Happiness, Mathf.Min((int)(winnings / 1000), 50));

                if (winnings >= 10000000)
                {
                    message = $"BÜYÜK İKRAMİYE! {winnings:N0} TL KAZANDIN!";
                }
                else if (winnings >= 100000)
                {
                    message = $"Şanslı gün! {winnings:N0} TL kazandın!";
                }
                else
                {
                    message = $"Küçük bir kazanç: {winnings:N0} TL";
                }

                return new LotteryResult
                {
                    success = true,
                    won = true,
                    amount = winnings,
                    message = message
                };
            }
            else
            {
                string[] loseMessages = new string[]
                {
                    "Maalesef kazanamadın. Bir dahaki sefere!",
                    "Şans bu sefer yanında değildi.",
                    "Boş çıktı. Yine dene!",
                    "Hiçbir şey kazanmadın. Kumarda şans yok!",
                    "Piyango hayallerini yıktı."
                };

                return new LotteryResult
                {
                    success = true,
                    won = false,
                    amount = 0,
                    message = loseMessages[Random.Range(0, loseMessages.Length)]
                };
            }
        }

        /// <summary>
        /// Borsa yatırımı yap.
        /// </summary>
        public InvestmentResult Invest(CharacterData character, decimal amount, InvestmentType type)
        {
            if (character.Finances.CurrentMoney < amount)
            {
                return new InvestmentResult
                {
                    success = false,
                    message = "Yeterli paranız yok!"
                };
            }

            character.Finances.ModifyMoney(-amount, $"{type} yatırımı");

            // Risk ve getiri hesapla
            float multiplier = 1f;
            string message;

            switch (type)
            {
                case InvestmentType.Bonds: // Düşük risk, düşük getiri
                    multiplier = Random.Range(1.02f, 1.08f);
                    break;

                case InvestmentType.Stocks: // Orta risk
                    multiplier = Random.Range(0.7f, 1.5f);
                    break;

                case InvestmentType.Crypto: // Yüksek risk
                    multiplier = Random.Range(0.1f, 3f);
                    break;

                case InvestmentType.StartupFund: // Çok yüksek risk
                    multiplier = Random.value < 0.2f ? Random.Range(5f, 20f) : Random.Range(0f, 0.5f);
                    break;
            }

            decimal returns = amount * (decimal)multiplier;
            decimal profit = returns - amount;

            character.Finances.ModifyMoney(returns, $"{type} getirisi");

            if (profit > 0)
            {
                character.Stats.ModifyStat(StatType.Happiness, Mathf.Min((int)(profit / 1000), 30));
                message = $"Yatırımın kar etti! {profit:N0} TL kazandın!";
            }
            else if (profit < 0)
            {
                character.Stats.ModifyStat(StatType.Happiness, Mathf.Max((int)(profit / 1000), -20));
                message = $"Yatırımın zarar etti! {-profit:N0} TL kaybettin.";
            }
            else
            {
                message = "Yatırımın ne kar ne zarar etti.";
            }

            return new InvestmentResult
            {
                success = true,
                profit = profit,
                message = message
            };
        }

        /// <summary>
        /// Kumarhanede oyna.
        /// </summary>
        public GamblingResult Gamble(CharacterData character, decimal betAmount, GamblingGame game)
        {
            if (character.Age < 18)
            {
                return new GamblingResult
                {
                    success = false,
                    message = "Kumarhaneye girmek için 18 yaşından büyük olmalısın!"
                };
            }

            if (character.Finances.CurrentMoney < betAmount)
            {
                return new GamblingResult
                {
                    success = false,
                    message = "Yeterli paranız yok!"
                };
            }

            character.Finances.ModifyMoney(-betAmount, $"Kumarhane - {game}");

            float winChance;
            float multiplier;
            string winMessage;
            string loseMessage;

            switch (game)
            {
                case GamblingGame.SlotMachine:
                    winChance = 0.35f;
                    multiplier = Random.Range(1.5f, 10f);
                    winMessage = "Slot makinesi çıldırdı! Jackpot!";
                    loseMessage = "Slot makinesi sessiz kaldı...";
                    break;

                case GamblingGame.Blackjack:
                    winChance = 0.42f;
                    multiplier = 2f;
                    winMessage = "Blackjack! 21!";
                    loseMessage = "Krupiye kazandı.";
                    break;

                case GamblingGame.Roulette:
                    winChance = 0.48f;
                    multiplier = 2f;
                    winMessage = "Sayın tuttu!";
                    loseMessage = "Top başka yere düştü.";
                    break;

                case GamblingGame.Poker:
                    winChance = 0.40f;
                    multiplier = Random.Range(2f, 5f);
                    winMessage = "Blöf tuttu! Pot senin!";
                    loseMessage = "Rakip daha iyi eldi.";
                    break;

                default:
                    winChance = 0.45f;
                    multiplier = 2f;
                    winMessage = "Kazandın!";
                    loseMessage = "Kaybettin.";
                    break;
            }

            bool won = Random.value < winChance;

            if (won)
            {
                decimal winnings = betAmount * (decimal)multiplier;
                character.Finances.ModifyMoney(winnings, $"Kumarhane kazancı");
                character.Stats.ModifyStat(StatType.Happiness, 10);

                return new GamblingResult
                {
                    success = true,
                    won = true,
                    amount = winnings,
                    message = $"{winMessage} {winnings:N0} TL kazandın!"
                };
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -5);

                string[] funnyLoses = new string[]
                {
                    $"{loseMessage} Ev her zaman kazanır!",
                    $"{loseMessage} Bir dahaki sefere!",
                    $"{loseMessage} Kumar bağımlılığı tehlikelidir!",
                    $"{loseMessage} Paranı yaktın!",
                    $"{loseMessage} Kumarda kayıp normaldir."
                };

                return new GamblingResult
                {
                    success = true,
                    won = false,
                    amount = betAmount,
                    message = funnyLoses[Random.Range(0, funnyLoses.Length)]
                };
            }
        }

        public List<Vehicle> GetAvailableVehicles() => _availableVehicles;
        public List<Property> GetAvailableProperties() => _availableProperties;
    }

    #region Data Structures

    [System.Serializable]
    public class Vehicle
    {
        public string id;
        public string name;
        public string brand;
        public VehicleType type;
        public decimal basePrice;
        public decimal annualMaintenance;
        public int happinessBonus;
        public int prestigeBonus;
    }

    public enum VehicleType
    {
        Bicycle,
        Motorcycle,
        Car,
        Boat,
        Aircraft
    }

    [System.Serializable]
    public class Property
    {
        public string id;
        public string name;
        public PropertyType type;
        public string location;
        public decimal basePrice;
        public decimal monthlyRent;
        public decimal annualMaintenance;
        public int happinessBonus;
        public int prestigeBonus;
    }

    public enum PropertyType
    {
        Apartment,
        House,
        Villa,
        Mansion
    }

    [System.Serializable]
    public class OwnedAsset
    {
        public string id;
        public AssetType assetType;
        public string name;
        public string brand;
        public string location;
        public decimal purchasePrice;
        public decimal currentValue;
        public decimal monthlyIncome;
        public decimal annualMaintenance;
        public int happinessBonus;
        public int prestigeBonus;
        public int purchaseYear;
    }

    public enum AssetType
    {
        Vehicle,
        Property
    }

    public class AssetResult
    {
        public bool success;
        public string message;
    }

    public class LotteryResult
    {
        public bool success;
        public bool won;
        public decimal amount;
        public string message;
    }

    public class InvestmentResult
    {
        public bool success;
        public decimal profit;
        public string message;
    }

    public enum InvestmentType
    {
        Bonds,      // Tahvil
        Stocks,     // Hisse senedi
        Crypto,     // Kripto para
        StartupFund // Risk sermayesi
    }

    public class GamblingResult
    {
        public bool success;
        public bool won;
        public decimal amount;
        public string message;
    }

    public enum GamblingGame
    {
        SlotMachine,
        Blackjack,
        Roulette,
        Poker
    }

    #endregion
}
