using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Assets
{
    /// <summary>
    /// Varlık türü.
    /// </summary>
    public enum AssetType
    {
        House,
        Car,
        Jewelry,
        Electronics,
        Furniture
    }

    /// <summary>
    /// Ev türü.
    /// </summary>
    public enum HouseType
    {
        Gecekondu,          // Gecekondu
        Studio,             // Stüdyo daire
        Apartment,          // Daire
        Villa,              // Villa
        Mansion,            // Konak
        Penthouse           // Çatı katı
    }

    /// <summary>
    /// Araba markası.
    /// </summary>
    public enum CarBrand
    {
        Tofas,
        Renault,
        Fiat,
        Volkswagen,
        Toyota,
        Honda,
        BMW,
        MercedesBenz,
        Audi,
        Porsche,
        Ferrari,
        Lamborghini
    }

    /// <summary>
    /// Ev varlığı.
    /// </summary>
    [System.Serializable]
    public class HouseAsset
    {
        public string id;
        public string name;
        public HouseType type;
        public string location;
        public decimal purchasePrice;
        public decimal currentValue;
        public int yearPurchased;
        public int bedrooms;
        public int bathrooms;
        public bool hasMortgage;
        public decimal mortgageRemaining;
    }

    /// <summary>
    /// Araba varlığı.
    /// </summary>
    [System.Serializable]
    public class CarAsset
    {
        public string id;
        public string name;
        public CarBrand brand;
        public int year;
        public decimal purchasePrice;
        public decimal currentValue;
        public int yearPurchased;
        public int condition; // 0-100
    }

    /// <summary>
    /// Genel varlık.
    /// </summary>
    [System.Serializable]
    public class GeneralAsset
    {
        public string id;
        public string name;
        public AssetType type;
        public decimal value;
        public int yearPurchased;
    }

    /// <summary>
    /// Varlık Yöneticisi - Ev, araba ve diğer varlıkları yönetir.
    /// </summary>
    public class AssetManager : Singleton<AssetManager>
    {
        // Sahip olunan varlıklar
        private List<HouseAsset> _houses = new List<HouseAsset>();
        private List<CarAsset> _cars = new List<CarAsset>();
        private List<GeneralAsset> _generalAssets = new List<GeneralAsset>();

        // Satılık evler
        private List<HouseAsset> _housesForSale = new List<HouseAsset>();
        private List<CarAsset> _carsForSale = new List<CarAsset>();

        #region Properties

        public List<HouseAsset> OwnedHouses => _houses;
        public List<CarAsset> OwnedCars => _cars;
        public List<GeneralAsset> OwnedAssets => _generalAssets;
        public decimal TotalAssetValue => CalculateTotalAssetValue();

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            RefreshMarket();

            // Her yıl varlık değerlerini güncelle
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);

            Debug.Log("[AssetManager] Initialized successfully.");
        }

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            // Ev değerleri artar
            foreach (var house in _houses)
            {
                float appreciation = Random.Range(0.02f, 0.08f);
                house.currentValue *= (decimal)(1 + appreciation);

                // Mortgage ödemesi
                if (house.hasMortgage && house.mortgageRemaining > 0)
                {
                    decimal payment = house.purchasePrice * 0.05m;
                    house.mortgageRemaining -= payment;

                    var character = GameManager.Instance?.CurrentCharacter;
                    if (character != null)
                    {
                        character.Finances.ModifyMoney(-payment, $"{house.name} mortgage ödemesi");
                    }

                    if (house.mortgageRemaining <= 0)
                    {
                        house.mortgageRemaining = 0;
                        house.hasMortgage = false;
                    }
                }
            }

            // Araba değerleri düşer
            foreach (var car in _cars)
            {
                float depreciation = Random.Range(0.05f, 0.15f);
                car.currentValue *= (decimal)(1 - depreciation);
                car.condition = Mathf.Max(0, car.condition - Random.Range(2, 8));
            }

            // Pazarı yenile
            RefreshMarket();
        }

        #endregion

        #region Market

        /// <summary>
        /// Satılık ev ve araba listesini yenile.
        /// </summary>
        public void RefreshMarket()
        {
            _housesForSale.Clear();
            _carsForSale.Clear();

            // Satılık evler oluştur
            _housesForSale.AddRange(GenerateHousesForSale(8));

            // Satılık arabalar oluştur
            _carsForSale.AddRange(GenerateCarsForSale(10));
        }

        /// <summary>
        /// Satılık evleri al.
        /// </summary>
        public List<HouseAsset> GetHousesForSale()
        {
            return _housesForSale;
        }

        /// <summary>
        /// Satılık arabaları al.
        /// </summary>
        public List<CarAsset> GetCarsForSale()
        {
            return _carsForSale;
        }

        #endregion

        #region House Operations

        /// <summary>
        /// Ev satın al.
        /// </summary>
        public string BuyHouse(string houseId, bool withMortgage = false)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return "Karakter bulunamadı!";

            var house = _housesForSale.FirstOrDefault(h => h.id == houseId);
            if (house == null) return "Ev bulunamadı!";

            if (withMortgage)
            {
                // Mortgage ile satın al (peşinat %20)
                decimal downPayment = house.purchasePrice * 0.2m;

                if (character.Finances.CurrentMoney < downPayment)
                {
                    return $"Peşinat için yeterli paran yok! ({downPayment:N0} TL gerekli)";
                }

                character.Finances.ModifyMoney(-downPayment, $"{house.name} peşinatı");
                house.hasMortgage = true;
                house.mortgageRemaining = house.purchasePrice * 0.8m;
            }
            else
            {
                // Nakit satın al
                if (character.Finances.CurrentMoney < house.purchasePrice)
                {
                    return $"Yeterli paran yok! ({house.purchasePrice:N0} TL gerekli)";
                }

                character.Finances.ModifyMoney(-house.purchasePrice, $"{house.name} satın alma");
            }

            house.yearPurchased = character.Age;
            _houses.Add(house);
            _housesForSale.Remove(house);

            character.Stats.ModifyStat(StatType.Happiness, 15);

            EventBus.Publish(new AssetPurchasedEvent
            {
                AssetName = house.name,
                AssetType = AssetType.House,
                Price = house.purchasePrice
            });

            return $"{house.name} satın aldın!";
        }

        /// <summary>
        /// Ev sat.
        /// </summary>
        public string SellHouse(string houseId)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return "Karakter bulunamadı!";

            var house = _houses.FirstOrDefault(h => h.id == houseId);
            if (house == null) return "Bu evin sahibi değilsin!";

            decimal salePrice = house.currentValue;

            // Mortgage varsa düş
            if (house.hasMortgage)
            {
                salePrice -= house.mortgageRemaining;
            }

            character.Finances.ModifyMoney(salePrice, $"{house.name} satışı");
            _houses.Remove(house);

            EventBus.Publish(new AssetSoldEvent
            {
                AssetName = house.name,
                AssetType = AssetType.House,
                SalePrice = salePrice
            });

            return $"{house.name} sattın! +{salePrice:N0} TL";
        }

        #endregion

        #region Car Operations

        /// <summary>
        /// Araba satın al.
        /// </summary>
        public string BuyCar(string carId)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return "Karakter bulunamadı!";

            // Yaş kontrolü
            if (character.Age < 18)
            {
                return "Araba satın almak için 18 yaşından büyük olmalısın!";
            }

            var car = _carsForSale.FirstOrDefault(c => c.id == carId);
            if (car == null) return "Araba bulunamadı!";

            if (character.Finances.CurrentMoney < car.purchasePrice)
            {
                return $"Yeterli paran yok! ({car.purchasePrice:N0} TL gerekli)";
            }

            character.Finances.ModifyMoney(-car.purchasePrice, $"{car.name} satın alma");
            car.yearPurchased = character.Age;
            _cars.Add(car);
            _carsForSale.Remove(car);

            character.Stats.ModifyStat(StatType.Happiness, 10);

            EventBus.Publish(new AssetPurchasedEvent
            {
                AssetName = car.name,
                AssetType = AssetType.Car,
                Price = car.purchasePrice
            });

            return $"{car.name} satın aldın!";
        }

        /// <summary>
        /// Araba sat.
        /// </summary>
        public string SellCar(string carId)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return "Karakter bulunamadı!";

            var car = _cars.FirstOrDefault(c => c.id == carId);
            if (car == null) return "Bu arabanın sahibi değilsin!";

            decimal salePrice = car.currentValue;
            character.Finances.ModifyMoney(salePrice, $"{car.name} satışı");
            _cars.Remove(car);

            EventBus.Publish(new AssetSoldEvent
            {
                AssetName = car.name,
                AssetType = AssetType.Car,
                SalePrice = salePrice
            });

            return $"{car.name} sattın! +{salePrice:N0} TL";
        }

        /// <summary>
        /// Araba tamir et.
        /// </summary>
        public string RepairCar(string carId)
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return "Karakter bulunamadı!";

            var car = _cars.FirstOrDefault(c => c.id == carId);
            if (car == null) return "Bu arabanın sahibi değilsin!";

            int repairCost = (100 - car.condition) * 100;

            if (character.Finances.CurrentMoney < repairCost)
            {
                return $"Tamir için yeterli paran yok! ({repairCost:N0} TL gerekli)";
            }

            character.Finances.ModifyMoney(-repairCost, $"{car.name} tamiri");
            car.condition = 100;
            car.currentValue = car.purchasePrice * 0.8m; // Değeri biraz artar

            return $"{car.name} tamir edildi! Durum: %100";
        }

        #endregion

        #region Generators

        private List<HouseAsset> GenerateHousesForSale(int count)
        {
            var houses = new List<HouseAsset>();
            string[] locations = { "Kadıköy", "Beşiktaş", "Üsküdar", "Bakırköy", "Şişli", "Ataşehir", "Maltepe", "Kartal", "Pendik", "Beylikdüzü" };

            for (int i = 0; i < count; i++)
            {
                HouseType type = (HouseType)Random.Range(0, 6);
                string location = locations[Random.Range(0, locations.Length)];

                decimal basePrice = type switch
                {
                    HouseType.Gecekondu => Random.Range(100000, 300000),
                    HouseType.Studio => Random.Range(500000, 1000000),
                    HouseType.Apartment => Random.Range(1000000, 3000000),
                    HouseType.Villa => Random.Range(5000000, 15000000),
                    HouseType.Mansion => Random.Range(20000000, 50000000),
                    HouseType.Penthouse => Random.Range(10000000, 30000000),
                    _ => 1000000
                };

                houses.Add(new HouseAsset
                {
                    id = System.Guid.NewGuid().ToString(),
                    name = $"{location} {GetHouseTypeName(type)}",
                    type = type,
                    location = location,
                    purchasePrice = basePrice,
                    currentValue = basePrice,
                    bedrooms = GetBedroomCount(type),
                    bathrooms = GetBathroomCount(type)
                });
            }

            return houses;
        }

        private List<CarAsset> GenerateCarsForSale(int count)
        {
            var cars = new List<CarAsset>();

            for (int i = 0; i < count; i++)
            {
                CarBrand brand = (CarBrand)Random.Range(0, 12);
                int year = Random.Range(2010, 2025);

                decimal basePrice = brand switch
                {
                    CarBrand.Tofas => Random.Range(50000, 150000),
                    CarBrand.Renault => Random.Range(200000, 500000),
                    CarBrand.Fiat => Random.Range(200000, 600000),
                    CarBrand.Volkswagen => Random.Range(400000, 1000000),
                    CarBrand.Toyota => Random.Range(500000, 1200000),
                    CarBrand.Honda => Random.Range(500000, 1100000),
                    CarBrand.BMW => Random.Range(1000000, 3000000),
                    CarBrand.MercedesBenz => Random.Range(1200000, 4000000),
                    CarBrand.Audi => Random.Range(1000000, 3500000),
                    CarBrand.Porsche => Random.Range(3000000, 10000000),
                    CarBrand.Ferrari => Random.Range(10000000, 30000000),
                    CarBrand.Lamborghini => Random.Range(15000000, 40000000),
                    _ => 500000
                };

                cars.Add(new CarAsset
                {
                    id = System.Guid.NewGuid().ToString(),
                    name = $"{year} {brand}",
                    brand = brand,
                    year = year,
                    purchasePrice = basePrice,
                    currentValue = basePrice,
                    condition = Random.Range(70, 100)
                });
            }

            return cars;
        }

        #endregion

        #region Utility

        private decimal CalculateTotalAssetValue()
        {
            decimal total = 0;

            foreach (var house in _houses)
            {
                total += house.currentValue - house.mortgageRemaining;
            }

            foreach (var car in _cars)
            {
                total += car.currentValue;
            }

            foreach (var asset in _generalAssets)
            {
                total += asset.value;
            }

            return total;
        }

        private string GetHouseTypeName(HouseType type)
        {
            return type switch
            {
                HouseType.Gecekondu => "Gecekondu",
                HouseType.Studio => "Stüdyo Daire",
                HouseType.Apartment => "Daire",
                HouseType.Villa => "Villa",
                HouseType.Mansion => "Konak",
                HouseType.Penthouse => "Çatı Katı",
                _ => "Ev"
            };
        }

        private int GetBedroomCount(HouseType type)
        {
            return type switch
            {
                HouseType.Gecekondu => 1,
                HouseType.Studio => 0,
                HouseType.Apartment => Random.Range(2, 4),
                HouseType.Villa => Random.Range(4, 6),
                HouseType.Mansion => Random.Range(6, 10),
                HouseType.Penthouse => Random.Range(3, 5),
                _ => 2
            };
        }

        private int GetBathroomCount(HouseType type)
        {
            return type switch
            {
                HouseType.Gecekondu => 1,
                HouseType.Studio => 1,
                HouseType.Apartment => Random.Range(1, 3),
                HouseType.Villa => Random.Range(3, 5),
                HouseType.Mansion => Random.Range(5, 8),
                HouseType.Penthouse => Random.Range(2, 4),
                _ => 1
            };
        }

        #endregion
    }

    #region Events

    public struct AssetPurchasedEvent : IGameEvent
    {
        public string AssetName;
        public AssetType AssetType;
        public decimal Price;
    }

    public struct AssetSoldEvent : IGameEvent
    {
        public string AssetName;
        public AssetType AssetType;
        public decimal SalePrice;
    }

    #endregion
}
