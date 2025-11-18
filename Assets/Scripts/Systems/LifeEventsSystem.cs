using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Hayat Olayları Sistemi - Askerlik, evlilik, mülk edinme gibi büyük hayat olaylarını yönetir.
    /// </summary>
    public static class LifeEventsSystem
    {
        #region Military Service (Askerlik)

        /// <summary>
        /// Askerlik kontrolü - Türkiye'de erkekler için zorunlu.
        /// </summary>
        public static bool ShouldTriggerMilitary(CharacterData character)
        {
            if (character.gender != Gender.Male) return false;
            if (character.hasCompletedMilitary) return false;
            if (character.age < 20 || character.age > 40) return false;

            // Üniversite öğrencisi ise erteleme
            if (character.Education.CurrentLevel == EducationLevel.University && character.age < 28)
                return false;

            return true;
        }

        /// <summary>
        /// Askerlik seçenekleri.
        /// </summary>
        public static List<MilitaryOption> GetMilitaryOptions(CharacterData character)
        {
            var options = new List<MilitaryOption>();

            // Bedelli askerlik (para karşılığı kısa süre)
            if (character.Finances.CurrentMoney >= 150000)
            {
                options.Add(new MilitaryOption
                {
                    id = "bedelli",
                    name = "Bedelli Askerlik",
                    description = "150.000 TL ödeyerek 1 ay askerlik yap",
                    cost = 150000,
                    duration = 1,
                    healthEffect = -5,
                    happinessEffect = 5
                });
            }

            // Uzun dönem
            options.Add(new MilitaryOption
            {
                id = "uzun_donem",
                name = "Uzun Dönem",
                description = "6 ay zorunlu askerlik",
                cost = 0,
                duration = 6,
                healthEffect = 10,
                happinessEffect = -10
            });

            // Kısa dönem (üniversite mezunları için)
            if (character.Education.CurrentLevel >= EducationLevel.University)
            {
                options.Add(new MilitaryOption
                {
                    id = "kisa_donem",
                    name = "Kısa Dönem",
                    description = "Üniversite mezunları için 2 ay askerlik",
                    cost = 0,
                    duration = 2,
                    healthEffect = 5,
                    happinessEffect = -5
                });
            }

            return options;
        }

        /// <summary>
        /// Askerlik tamamla.
        /// </summary>
        public static string CompleteMilitaryService(CharacterData character, MilitaryOption option)
        {
            // Para öde
            if (option.cost > 0)
            {
                character.Finances.ModifyMoney(-option.cost, "Bedelli askerlik ücreti");
            }

            // Stat değişiklikleri
            character.Stats.ModifyStat(StatType.Health, option.healthEffect);
            character.Stats.ModifyStat(StatType.Happiness, option.happinessEffect);

            // Askerlik tamamlandı
            character.hasCompletedMilitary = true;

            string[] cities = { "Ankara", "Konya", "Kayseri", "Erzurum", "Van", "Hakkari" };
            string city = cities[Random.Range(0, cities.Length)];

            return $"{option.name} tamamlandı! {option.duration} ay {city}'de görev yaptın.";
        }

        #endregion

        #region Marriage System

        /// <summary>
        /// Evlilik kontrolü.
        /// </summary>
        public static bool CanGetMarried(CharacterData character)
        {
            if (character.isMarried) return false;
            if (character.age < 18) return false;

            // Romantik partner var mı?
            var partner = character.Relationships.Find(r =>
                r.type == RelationType.Boyfriend ||
                r.type == RelationType.Girlfriend);

            return partner != null && partner.intimacy >= 70;
        }

        /// <summary>
        /// Evlen.
        /// </summary>
        public static string GetMarried(CharacterData character, bool expensiveWedding)
        {
            var partner = character.Relationships.Find(r =>
                r.type == RelationType.Boyfriend ||
                r.type == RelationType.Girlfriend);

            if (partner == null) return "Evlenecek kimse yok!";

            // Düğün masrafı
            decimal weddingCost = expensiveWedding ? 50000 : 5000;
            if (character.Finances.CurrentMoney < weddingCost)
            {
                return "Düğün için yeterli paran yok!";
            }

            character.Finances.ModifyMoney(-weddingCost, "Düğün masrafları");

            // Partner'ı eşe dönüştür
            partner.type = RelationType.Spouse;
            character.isMarried = true;

            // Mutluluk artışı
            int happinessGain = expensiveWedding ? 30 : 20;
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            string weddingType = expensiveWedding ? "görkemli" : "sade";
            return $"{partner.npcName} ile {weddingType} bir düğünle evlendin! Mutluluk +{happinessGain}";
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public static string GetDivorced(CharacterData character)
        {
            var spouse = character.Relationships.Find(r => r.type == RelationType.Spouse);

            if (spouse == null) return "Evli değilsin!";

            // Boşanma maliyeti
            decimal divorceCost = Random.Range(5000, 30000);
            character.Finances.ModifyMoney(-divorceCost, "Boşanma masrafları");

            // Eşi ex'e dönüştür
            spouse.type = RelationType.ExSpouse;
            spouse.intimacy = Random.Range(0, 30);
            character.isMarried = false;

            // Mutluluk kaybı
            int happinessLoss = Random.Range(15, 30);
            character.Stats.ModifyStat(StatType.Happiness, -happinessLoss);

            return $"{spouse.npcName} ile boşandın. Mutluluk -{happinessLoss}";
        }

        /// <summary>
        /// Çocuk sahibi ol.
        /// </summary>
        public static string HaveChild(CharacterData character)
        {
            if (!character.isMarried)
            {
                return "Önce evlenmelisin!";
            }

            if (character.age > 50)
            {
                return "Çocuk sahibi olmak için çok geç.";
            }

            // Çocuk oluştur
            Gender childGender = Random.value > 0.5f ? Gender.Male : Gender.Female;
            string childName = childGender == Gender.Male
                ? DataManager.Instance.GetRandomMaleName()
                : DataManager.Instance.GetRandomFemaleName();

            var child = new Relationship
            {
                npcName = childName,
                type = RelationType.Child,
                gender = childGender,
                age = 0,
                intimacy = 100,
                status = RelationshipStatus.Active
            };

            character.Relationships.Add(child);

            // Masraflar
            character.Finances.ModifyMoney(-5000, "Doğum masrafları");

            // Büyük mutluluk
            character.Stats.ModifyStat(StatType.Happiness, 25);

            string genderText = childGender == Gender.Male ? "bir oğlun" : "bir kızın";
            return $"{genderText} oldu! Adı: {childName}. Mutluluk +25";
        }

        #endregion

        #region Property System

        /// <summary>
        /// Mülk satın al.
        /// </summary>
        public static string BuyProperty(CharacterData character, PropertyType propertyType)
        {
            var property = GetPropertyData(propertyType);

            if (character.Finances.CurrentMoney < property.price)
            {
                return $"{property.name} almak için yeterli paran yok! ({property.price:N0} TL gerekli)";
            }

            character.Finances.ModifyMoney(-property.price, $"{property.name} satın alma");
            character.Finances.assets.Add(property.name);

            int happinessGain = property.happinessBonus;
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            if (property.fameBonus > 0)
            {
                character.Stats.ModifyStat(StatType.Fame, property.fameBonus);
            }

            return $"{property.name} satın aldın! Mutluluk +{happinessGain}";
        }

        /// <summary>
        /// Mülk verilerini al.
        /// </summary>
        public static PropertyData GetPropertyData(PropertyType type)
        {
            return type switch
            {
                PropertyType.SmallApartment => new PropertyData
                {
                    name = "Küçük Daire",
                    price = 150000,
                    happinessBonus = 10,
                    fameBonus = 0
                },
                PropertyType.MediumApartment => new PropertyData
                {
                    name = "Orta Boy Daire",
                    price = 300000,
                    happinessBonus = 15,
                    fameBonus = 1
                },
                PropertyType.LargeApartment => new PropertyData
                {
                    name = "Büyük Daire",
                    price = 500000,
                    happinessBonus = 20,
                    fameBonus = 2
                },
                PropertyType.Villa => new PropertyData
                {
                    name = "Villa",
                    price = 1500000,
                    happinessBonus = 30,
                    fameBonus = 5
                },
                PropertyType.Mansion => new PropertyData
                {
                    name = "Malikane",
                    price = 5000000,
                    happinessBonus = 40,
                    fameBonus = 10
                },
                PropertyType.EconomyCar => new PropertyData
                {
                    name = "Ekonomik Araba",
                    price = 50000,
                    happinessBonus = 8,
                    fameBonus = 0
                },
                PropertyType.MidRangeCar => new PropertyData
                {
                    name = "Orta Segment Araba",
                    price = 150000,
                    happinessBonus = 12,
                    fameBonus = 1
                },
                PropertyType.LuxuryCar => new PropertyData
                {
                    name = "Lüks Araba",
                    price = 500000,
                    happinessBonus = 20,
                    fameBonus = 3
                },
                PropertyType.SportsCar => new PropertyData
                {
                    name = "Spor Araba",
                    price = 1000000,
                    happinessBonus = 25,
                    fameBonus = 5
                },
                PropertyType.Yacht => new PropertyData
                {
                    name = "Yat",
                    price = 3000000,
                    happinessBonus = 30,
                    fameBonus = 8
                },
                _ => new PropertyData
                {
                    name = "Bilinmeyen",
                    price = 0,
                    happinessBonus = 0,
                    fameBonus = 0
                }
            };
        }

        /// <summary>
        /// Mülk sat.
        /// </summary>
        public static string SellProperty(CharacterData character, string propertyName)
        {
            if (!character.Finances.assets.Contains(propertyName))
            {
                return "Bu mülke sahip değilsin!";
            }

            // Satış fiyatı (orijinalin %70-90'ı)
            decimal sellPrice = GetPropertyPriceByName(propertyName) * (decimal)Random.Range(0.7f, 0.9f);

            character.Finances.ModifyMoney(sellPrice, $"{propertyName} satışı");
            character.Finances.assets.Remove(propertyName);

            return $"{propertyName} satıldı! {sellPrice:N0} TL kazandın.";
        }

        private static decimal GetPropertyPriceByName(string name)
        {
            return name switch
            {
                "Küçük Daire" => 150000,
                "Orta Boy Daire" => 300000,
                "Büyük Daire" => 500000,
                "Villa" => 1500000,
                "Malikane" => 5000000,
                "Ekonomik Araba" => 50000,
                "Orta Segment Araba" => 150000,
                "Lüks Araba" => 500000,
                "Spor Araba" => 1000000,
                "Yat" => 3000000,
                _ => 10000
            };
        }

        #endregion

        #region Emigration System

        /// <summary>
        /// Göç et.
        /// </summary>
        public static string Emigrate(CharacterData character, string country)
        {
            if (character.age < 18)
            {
                return "Göç etmek için 18 yaşından büyük olmalısın!";
            }

            // Göç masrafları
            decimal emigrationCost = GetEmigrationCost(country);
            if (character.Finances.CurrentMoney < emigrationCost)
            {
                return $"{country}'a göç etmek için yeterli paran yok! ({emigrationCost:N0} TL gerekli)";
            }

            character.Finances.ModifyMoney(-emigrationCost, $"{country} göç masrafları");

            // Başarı şansı (zekaya ve eğitime bağlı)
            float successChance = 0.3f + (character.Stats.Intelligence * 0.003f) + ((int)character.Education.CurrentLevel * 0.05f);

            if (Random.value < successChance)
            {
                // Başarılı göç
                character.birthCity = country;

                int happinessGain = Random.Range(10, 25);
                character.Stats.ModifyStat(StatType.Happiness, happinessGain);

                // İş ve aile bağlantıları kopar
                character.isEmployed = false;
                character.Career.currentJob = null;

                return $"{country}'a başarıyla göç ettin! Yeni bir hayat başlıyor. Mutluluk +{happinessGain}";
            }
            else
            {
                // Başarısız
                int happinessLoss = Random.Range(10, 20);
                character.Stats.ModifyStat(StatType.Happiness, -happinessLoss);

                return $"{country}'a göç başvurun reddedildi. Mutluluk -{happinessLoss}";
            }
        }

        private static decimal GetEmigrationCost(string country)
        {
            return country switch
            {
                "Almanya" => 30000,
                "İngiltere" => 40000,
                "Amerika" => 50000,
                "Kanada" => 35000,
                "Avustralya" => 45000,
                "Hollanda" => 30000,
                "İsviçre" => 60000,
                "Norveç" => 40000,
                _ => 25000
            };
        }

        /// <summary>
        /// Göç seçenekleri.
        /// </summary>
        public static List<string> GetEmigrationOptions()
        {
            return new List<string>
            {
                "Almanya",
                "İngiltere",
                "Amerika",
                "Kanada",
                "Avustralya",
                "Hollanda",
                "İsviçre",
                "Norveç"
            };
        }

        #endregion

        #region Fame System

        /// <summary>
        /// Sosyal medya influencer ol.
        /// </summary>
        public static string BecomeSocialMediaStar(CharacterData character)
        {
            if (character.age < 13)
            {
                return "Sosyal medya için çok gençsin!";
            }

            // Başarı şansı (görünüş ve zekaya bağlı)
            float successChance = 0.05f + (character.Stats.Appearance * 0.002f) + (character.Stats.Intelligence * 0.001f);

            if (Random.value < successChance)
            {
                int fameGain = Random.Range(10, 30);
                int moneyGain = Random.Range(1000, 10000);

                character.Stats.ModifyStat(StatType.Fame, fameGain);
                character.Finances.ModifyMoney(moneyGain, "Sosyal medya kazancı");

                return $"Viral oldun! Şöhret +{fameGain}, {moneyGain:N0} TL kazandın!";
            }
            else
            {
                return "Kimse içeriğini beğenmedi. Belki başka zaman.";
            }
        }

        /// <summary>
        /// Ünlü ol (yetenek bazlı).
        /// </summary>
        public static string TryToBecomeFamous(CharacterData character, FameCategory category)
        {
            string categoryName = category switch
            {
                FameCategory.Music => "Müzisyen",
                FameCategory.Acting => "Oyuncu",
                FameCategory.Sports => "Sporcu",
                FameCategory.Writing => "Yazar",
                FameCategory.Business => "İş İnsanı",
                _ => "Ünlü"
            };

            // Gerekli stat kontrolü
            int requiredStat = category switch
            {
                FameCategory.Music => character.Stats.Happiness,
                FameCategory.Acting => character.Stats.Appearance,
                FameCategory.Sports => character.Stats.Health,
                FameCategory.Writing => character.Stats.Intelligence,
                FameCategory.Business => character.Stats.Intelligence,
                _ => 50
            };

            float successChance = 0.02f + (requiredStat * 0.003f);

            if (Random.value < successChance)
            {
                int fameGain = Random.Range(20, 50);
                int moneyGain = Random.Range(50000, 200000);

                character.Stats.ModifyStat(StatType.Fame, fameGain);
                character.Finances.ModifyMoney(moneyGain, $"{categoryName} kariyeri");

                return $"{categoryName} olarak ünlendin! Şöhret +{fameGain}, {moneyGain:N0} TL kazandın!";
            }
            else
            {
                int happinessLoss = Random.Range(5, 15);
                character.Stats.ModifyStat(StatType.Happiness, -happinessLoss);

                return $"{categoryName} olmak için yeterli değildin. Mutluluk -{happinessLoss}";
            }
        }

        #endregion
    }

    #region Data Classes

    public class MilitaryOption
    {
        public string id;
        public string name;
        public string description;
        public decimal cost;
        public int duration;
        public int healthEffect;
        public int happinessEffect;
    }

    public enum PropertyType
    {
        SmallApartment,
        MediumApartment,
        LargeApartment,
        Villa,
        Mansion,
        EconomyCar,
        MidRangeCar,
        LuxuryCar,
        SportsCar,
        Yacht
    }

    public class PropertyData
    {
        public string name;
        public decimal price;
        public int happinessBonus;
        public int fameBonus;
    }

    public enum FameCategory
    {
        Music,
        Acting,
        Sports,
        Writing,
        Business
    }

    #endregion
}
