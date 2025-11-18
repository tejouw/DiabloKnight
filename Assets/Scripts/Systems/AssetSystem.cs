using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Mülk Sistemi - Ev, araba, eşya yönetimi.
    /// </summary>
    public class AssetSystem : Singleton<AssetSystem>
    {
        private List<PropertyDefinition> _properties = new List<PropertyDefinition>();
        private List<VehicleDefinition> _vehicles = new List<VehicleDefinition>();

        // Karakterin sahip olduğu varlıklar
        public List<OwnedProperty> OwnedProperties { get; private set; } = new List<OwnedProperty>();
        public List<OwnedVehicle> OwnedVehicles { get; private set; } = new List<OwnedVehicle>();

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeAssets();
        }

        private void InitializeAssets()
        {
            // Mülkler
            _properties = new List<PropertyDefinition>
            {
                // Evler
                new PropertyDefinition("studio", "Stüdyo Daire", PropertyType.Apartment, 500000, 2000, 10),
                new PropertyDefinition("1plus1", "1+1 Daire", PropertyType.Apartment, 800000, 3000, 15),
                new PropertyDefinition("2plus1", "2+1 Daire", PropertyType.Apartment, 1200000, 4500, 20),
                new PropertyDefinition("3plus1", "3+1 Daire", PropertyType.Apartment, 1800000, 6000, 25),
                new PropertyDefinition("4plus1", "4+1 Daire", PropertyType.Apartment, 2500000, 8000, 30),
                new PropertyDefinition("penthouse", "Penthouse", PropertyType.Apartment, 5000000, 15000, 40),

                new PropertyDefinition("small_house", "Küçük Müstakil Ev", PropertyType.House, 2000000, 5000, 35),
                new PropertyDefinition("medium_house", "Orta Müstakil Ev", PropertyType.House, 3500000, 8000, 40),
                new PropertyDefinition("large_house", "Büyük Müstakil Ev", PropertyType.House, 6000000, 12000, 50),
                new PropertyDefinition("villa", "Villa", PropertyType.Villa, 10000000, 20000, 60),
                new PropertyDefinition("luxury_villa", "Lüks Villa", PropertyType.Villa, 25000000, 50000, 80),
                new PropertyDefinition("mansion", "Malikane", PropertyType.Mansion, 50000000, 100000, 100),

                // Yatırım mülkleri
                new PropertyDefinition("small_shop", "Küçük Dükkan", PropertyType.Commercial, 800000, 5000, 10),
                new PropertyDefinition("office", "Ofis", PropertyType.Commercial, 1500000, 8000, 15),
                new PropertyDefinition("large_shop", "Büyük Mağaza", PropertyType.Commercial, 3000000, 15000, 25),
                new PropertyDefinition("warehouse", "Depo", PropertyType.Commercial, 2000000, 10000, 5),
            };

            // Araçlar
            _vehicles = new List<VehicleDefinition>
            {
                // Arabalar
                new VehicleDefinition("bicycle", "Bisiklet", VehicleType.Bicycle, 3000, 100, 0),
                new VehicleDefinition("scooter", "Elektrikli Scooter", VehicleType.Scooter, 15000, 500, 5),
                new VehicleDefinition("motorcycle", "Motosiklet", VehicleType.Motorcycle, 80000, 2000, 15),
                new VehicleDefinition("sport_motorcycle", "Spor Motosiklet", VehicleType.Motorcycle, 200000, 5000, 25),

                new VehicleDefinition("economy_car", "Ekonomik Araba", VehicleType.Car, 250000, 3000, 10),
                new VehicleDefinition("sedan", "Sedan", VehicleType.Car, 500000, 5000, 20),
                new VehicleDefinition("suv", "SUV", VehicleType.Car, 800000, 8000, 30),
                new VehicleDefinition("luxury_sedan", "Lüks Sedan", VehicleType.Car, 1500000, 15000, 40),
                new VehicleDefinition("sports_car", "Spor Araba", VehicleType.Car, 3000000, 25000, 50),
                new VehicleDefinition("supercar", "Süper Araba", VehicleType.Car, 10000000, 50000, 70),
                new VehicleDefinition("hypercar", "Hiper Araba", VehicleType.Car, 30000000, 100000, 90),

                new VehicleDefinition("small_boat", "Küçük Tekne", VehicleType.Boat, 500000, 10000, 20),
                new VehicleDefinition("yacht", "Yat", VehicleType.Boat, 5000000, 50000, 50),
                new VehicleDefinition("luxury_yacht", "Lüks Yat", VehicleType.Boat, 20000000, 150000, 70),

                new VehicleDefinition("small_plane", "Küçük Uçak", VehicleType.Aircraft, 10000000, 100000, 60),
                new VehicleDefinition("private_jet", "Özel Jet", VehicleType.Aircraft, 50000000, 300000, 90),
                new VehicleDefinition("helicopter", "Helikopter", VehicleType.Aircraft, 15000000, 150000, 70),
            };

            Debug.Log($"[AssetSystem] Loaded {_properties.Count} properties and {_vehicles.Count} vehicles.");
        }

        #endregion

        #region Property Management

        /// <summary>
        /// Mülk satın al.
        /// </summary>
        public PurchaseResult BuyProperty(CharacterData character, string propertyId)
        {
            var property = _properties.Find(p => p.id == propertyId);
            if (property == null)
            {
                return new PurchaseResult { success = false, message = "Mülk bulunamadı." };
            }

            if (character.Age < 18)
            {
                return new PurchaseResult { success = false, message = "Mülk almak için 18 yaşında olmalısın." };
            }

            if (character.Finances.CurrentMoney < property.price)
            {
                return new PurchaseResult
                {
                    success = false,
                    message = $"Yeterli paran yok. Fiyat: {property.price:N0} TL"
                };
            }

            // Satın al
            character.Finances.ModifyMoney(-property.price, $"{property.name} satın alma");

            var owned = new OwnedProperty
            {
                id = System.Guid.NewGuid().ToString(),
                definitionId = propertyId,
                name = property.name,
                purchasePrice = property.price,
                currentValue = property.price,
                yearPurchased = character.Age,
                type = property.type
            };

            OwnedProperties.Add(owned);
            character.Stats.ModifyStat(StatType.Happiness, property.happinessBonus);

            EventBus.Publish(new AssetPurchasedEvent
            {
                AssetName = property.name,
                Price = property.price,
                AssetType = "Mülk"
            });

            return new PurchaseResult
            {
                success = true,
                message = $"{property.name} satın aldın!"
            };
        }

        /// <summary>
        /// Mülk sat.
        /// </summary>
        public SellResult SellProperty(CharacterData character, string ownedPropertyId)
        {
            var owned = OwnedProperties.Find(p => p.id == ownedPropertyId);
            if (owned == null)
            {
                return new SellResult { success = false, message = "Bu mülke sahip değilsin." };
            }

            // Değer artışı/azalışı
            float marketChange = Random.Range(-0.1f, 0.3f);
            owned.currentValue = owned.purchasePrice * (1 + (decimal)marketChange);

            character.Finances.ModifyMoney(owned.currentValue, $"{owned.name} satış");
            OwnedProperties.Remove(owned);

            return new SellResult
            {
                success = true,
                message = $"{owned.name} sattın! {owned.currentValue:N0} TL kazandın.",
                salePrice = owned.currentValue
            };
        }

        /// <summary>
        /// Mülkü kiraya ver (yıllık gelir).
        /// </summary>
        public void ProcessRentalIncome(CharacterData character)
        {
            foreach (var property in OwnedProperties)
            {
                var definition = _properties.Find(p => p.id == property.definitionId);
                if (definition != null && definition.type == PropertyType.Commercial)
                {
                    decimal rentalIncome = definition.maintenanceCost * 2; // Kira geliri
                    character.Finances.ModifyMoney(rentalIncome, $"{property.name} kira geliri");
                }
            }
        }

        /// <summary>
        /// Bakım masraflarını işle.
        /// </summary>
        public void ProcessMaintenanceCosts(CharacterData character)
        {
            decimal totalMaintenance = 0;

            foreach (var property in OwnedProperties)
            {
                var definition = _properties.Find(p => p.id == property.definitionId);
                if (definition != null)
                {
                    totalMaintenance += definition.maintenanceCost * 12; // Yıllık
                }
            }

            foreach (var vehicle in OwnedVehicles)
            {
                var definition = _vehicles.Find(v => v.id == vehicle.definitionId);
                if (definition != null)
                {
                    totalMaintenance += definition.maintenanceCost * 12;
                }
            }

            if (totalMaintenance > 0)
            {
                character.Finances.ModifyMoney(-totalMaintenance, "Yıllık bakım masrafları");
            }
        }

        #endregion

        #region Vehicle Management

        /// <summary>
        /// Araç satın al.
        /// </summary>
        public PurchaseResult BuyVehicle(CharacterData character, string vehicleId)
        {
            var vehicle = _vehicles.Find(v => v.id == vehicleId);
            if (vehicle == null)
            {
                return new PurchaseResult { success = false, message = "Araç bulunamadı." };
            }

            int minAge = vehicle.type switch
            {
                VehicleType.Bicycle => 6,
                VehicleType.Scooter => 15,
                VehicleType.Motorcycle => 18,
                VehicleType.Car => 18,
                _ => 18
            };

            if (character.Age < minAge)
            {
                return new PurchaseResult { success = false, message = $"Bu araç için en az {minAge} yaşında olmalısın." };
            }

            if (character.Finances.CurrentMoney < vehicle.price)
            {
                return new PurchaseResult
                {
                    success = false,
                    message = $"Yeterli paran yok. Fiyat: {vehicle.price:N0} TL"
                };
            }

            character.Finances.ModifyMoney(-vehicle.price, $"{vehicle.name} satın alma");

            var owned = new OwnedVehicle
            {
                id = System.Guid.NewGuid().ToString(),
                definitionId = vehicleId,
                name = vehicle.name,
                purchasePrice = vehicle.price,
                currentValue = vehicle.price,
                yearPurchased = character.Age,
                type = vehicle.type,
                condition = 100
            };

            OwnedVehicles.Add(owned);
            character.Stats.ModifyStat(StatType.Happiness, vehicle.happinessBonus);

            EventBus.Publish(new AssetPurchasedEvent
            {
                AssetName = vehicle.name,
                Price = vehicle.price,
                AssetType = "Araç"
            });

            return new PurchaseResult
            {
                success = true,
                message = $"{vehicle.name} satın aldın!"
            };
        }

        /// <summary>
        /// Araç sat.
        /// </summary>
        public SellResult SellVehicle(CharacterData character, string ownedVehicleId)
        {
            var owned = OwnedVehicles.Find(v => v.id == ownedVehicleId);
            if (owned == null)
            {
                return new SellResult { success = false, message = "Bu araca sahip değilsin." };
            }

            // Değer kaybı (araçlar genelde değer kaybeder)
            float depreciation = 1f - ((character.Age - owned.yearPurchased) * 0.05f);
            depreciation = Mathf.Clamp(depreciation, 0.3f, 1f);
            depreciation *= owned.condition / 100f;

            owned.currentValue = owned.purchasePrice * (decimal)depreciation;

            character.Finances.ModifyMoney(owned.currentValue, $"{owned.name} satış");
            OwnedVehicles.Remove(owned);

            return new SellResult
            {
                success = true,
                message = $"{owned.name} sattın! {owned.currentValue:N0} TL kazandın.",
                salePrice = owned.currentValue
            };
        }

        /// <summary>
        /// Araç durumunu yıprat (yıllık).
        /// </summary>
        public void ProcessVehicleDepreciation()
        {
            foreach (var vehicle in OwnedVehicles)
            {
                vehicle.condition = Mathf.Max(20, vehicle.condition - Random.Range(3, 8));
            }
        }

        #endregion

        #region Utilities

        public List<PropertyDefinition> GetAvailableProperties(CharacterData character)
        {
            return _properties.Where(p => character.Finances.CurrentMoney >= p.price).ToList();
        }

        public List<VehicleDefinition> GetAvailableVehicles(CharacterData character)
        {
            return _vehicles.Where(v => character.Finances.CurrentMoney >= v.price).ToList();
        }

        public decimal GetTotalAssetValue()
        {
            decimal total = 0;
            total += OwnedProperties.Sum(p => p.currentValue);
            total += OwnedVehicles.Sum(v => v.currentValue);
            return total;
        }

        public int GetTotalAssetCount()
        {
            return OwnedProperties.Count + OwnedVehicles.Count;
        }

        #endregion
    }

    #region Data Classes

    [System.Serializable]
    public class PropertyDefinition
    {
        public string id;
        public string name;
        public PropertyType type;
        public decimal price;
        public decimal maintenanceCost;
        public int happinessBonus;

        public PropertyDefinition(string id, string name, PropertyType type, decimal price, decimal maintenanceCost, int happinessBonus)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.price = price;
            this.maintenanceCost = maintenanceCost;
            this.happinessBonus = happinessBonus;
        }
    }

    [System.Serializable]
    public class VehicleDefinition
    {
        public string id;
        public string name;
        public VehicleType type;
        public decimal price;
        public decimal maintenanceCost;
        public int happinessBonus;

        public VehicleDefinition(string id, string name, VehicleType type, decimal price, decimal maintenanceCost, int happinessBonus)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.price = price;
            this.maintenanceCost = maintenanceCost;
            this.happinessBonus = happinessBonus;
        }
    }

    public class OwnedProperty
    {
        public string id;
        public string definitionId;
        public string name;
        public decimal purchasePrice;
        public decimal currentValue;
        public int yearPurchased;
        public PropertyType type;
    }

    public class OwnedVehicle
    {
        public string id;
        public string definitionId;
        public string name;
        public decimal purchasePrice;
        public decimal currentValue;
        public int yearPurchased;
        public VehicleType type;
        public int condition;
    }

    public enum PropertyType
    {
        Apartment,
        House,
        Villa,
        Mansion,
        Commercial
    }

    public enum VehicleType
    {
        Bicycle,
        Scooter,
        Motorcycle,
        Car,
        Boat,
        Aircraft
    }

    public class PurchaseResult
    {
        public bool success;
        public string message;
    }

    public class SellResult
    {
        public bool success;
        public string message;
        public decimal salePrice;
    }

    public struct AssetPurchasedEvent : IGameEvent
    {
        public string AssetName;
        public decimal Price;
        public string AssetType;
    }

    #endregion
}
