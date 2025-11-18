using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Suç Sistemi - Yasadışı aktiviteler ve sonuçları.
    /// </summary>
    public static class CrimeSystem
    {
        #region Suç Eylemleri

        /// <summary>
        /// Mağazadan hırsızlık.
        /// </summary>
        public static CrimeResult Shoplift(CharacterData character)
        {
            if (character.Age < 10)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Bu eylem için çok küçüksün."
                };
            }

            // Yakalanma şansı
            float catchChance = 0.3f;
            bool caught = Random.value < catchChance;

            if (caught)
            {
                if (character.Age < 18)
                {
                    // Reşit değil - polise teslim
                    character.Stats.ModifyStat(StatType.Happiness, -Random.Range(15, 25));
                    return new CrimeResult
                    {
                        Success = false,
                        Caught = true,
                        Message = "Yakalandın! Ailen polisten seni aldı. Büyük kavga çıktı."
                    };
                }
                else
                {
                    // Reşit - ceza/hapis
                    int jailTime = Random.Range(1, 7); // gün
                    character.Stats.ModifyStat(StatType.Happiness, -Random.Range(20, 35));

                    return new CrimeResult
                    {
                        Success = false,
                        Caught = true,
                        JailTime = jailTime,
                        Message = $"Yakalandın ve tutuklandın! {jailTime} gün hapis cezası aldın."
                    };
                }
            }
            else
            {
                // Başarılı hırsızlık
                float stolenValue = Random.Range(50f, 500f);
                character.Finances.ModifyMoney(stolenValue, "Hırsızlık");

                return new CrimeResult
                {
                    Success = true,
                    MoneyGained = stolenValue,
                    Message = $"Hırsızlık başarılı! {stolenValue:N0} TL değerinde mal çaldın."
                };
            }
        }

        /// <summary>
        /// Araba çal.
        /// </summary>
        public static CrimeResult StealCar(CharacterData character)
        {
            if (character.Age < 16)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Araba çalmak için çok küçüksün."
                };
            }

            float catchChance = 0.4f - (character.Stats.Intelligence / 300f);
            bool caught = Random.value < catchChance;

            if (caught)
            {
                int jailTime = Random.Range(30, 180);
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(30, 50));

                return new CrimeResult
                {
                    Success = false,
                    Caught = true,
                    JailTime = jailTime,
                    Message = $"Araba çalarken yakalandın! {jailTime} gün hapis cezası."
                };
            }
            else
            {
                float carValue = Random.Range(10000f, 50000f);
                character.Finances.ModifyMoney(carValue, "Araba hırsızlığı");

                return new CrimeResult
                {
                    Success = true,
                    MoneyGained = carValue,
                    Message = $"Arabayı çaldın ve sattın! {carValue:N0} TL kazandın."
                };
            }
        }

        /// <summary>
        /// Banka soy.
        /// </summary>
        public static CrimeResult RobBank(CharacterData character)
        {
            if (character.Age < 18)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Banka soymak için çok küçüksün."
                };
            }

            // Çok riskli
            float catchChance = 0.6f - (character.Stats.Intelligence / 400f);
            bool caught = Random.value < catchChance;

            if (caught)
            {
                // Polisle çatışma olabilir
                if (Random.value < 0.3f)
                {
                    int healthLoss = Random.Range(30, 60);
                    character.Stats.ModifyStat(StatType.Health, -healthLoss);

                    if (character.Stats.Health <= 0)
                    {
                        return new CrimeResult
                        {
                            Success = false,
                            Caught = true,
                            Killed = true,
                            Message = "Polisle çatışmada hayatını kaybettin!"
                        };
                    }
                }

                int jailTime = Random.Range(365 * 5, 365 * 20); // 5-20 yıl
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(40, 60));

                return new CrimeResult
                {
                    Success = false,
                    Caught = true,
                    JailTime = jailTime,
                    Message = $"Banka soygunu başarısız! {jailTime / 365} yıl hapis cezası."
                };
            }
            else
            {
                float stolenMoney = Random.Range(100000f, 1000000f);
                character.Finances.ModifyMoney(stolenMoney, "Banka soygunu");
                character.Stats.ModifyStat(StatType.Fame, Random.Range(5, 15));

                return new CrimeResult
                {
                    Success = true,
                    MoneyGained = stolenMoney,
                    Message = $"Banka soygunu başarılı! {stolenMoney:N0} TL çaldın!"
                };
            }
        }

        /// <summary>
        /// Dolandırıcılık yap.
        /// </summary>
        public static CrimeResult Scam(CharacterData character)
        {
            if (character.Age < 18)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Dolandırıcılık için çok küçüksün."
                };
            }

            float catchChance = 0.35f - (character.Stats.Intelligence / 400f);
            bool caught = Random.value < catchChance;

            if (caught)
            {
                int jailTime = Random.Range(90, 365);
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(25, 40));

                return new CrimeResult
                {
                    Success = false,
                    Caught = true,
                    JailTime = jailTime,
                    Message = $"Dolandırıcılıktan yakalandın! {jailTime} gün hapis."
                };
            }
            else
            {
                float scamMoney = Random.Range(5000f, 50000f);
                character.Finances.ModifyMoney(scamMoney, "Dolandırıcılık");

                return new CrimeResult
                {
                    Success = true,
                    MoneyGained = scamMoney,
                    Message = $"Dolandırıcılık başarılı! {scamMoney:N0} TL kazandın."
                };
            }
        }

        /// <summary>
        /// Uyuşturucu sat.
        /// </summary>
        public static CrimeResult DealDrugs(CharacterData character)
        {
            if (character.Age < 16)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Bu eylem için çok küçüksün."
                };
            }

            float catchChance = 0.25f;
            bool caught = Random.value < catchChance;

            if (caught)
            {
                int jailTime = Random.Range(365, 365 * 10); // 1-10 yıl
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(35, 55));

                return new CrimeResult
                {
                    Success = false,
                    Caught = true,
                    JailTime = jailTime,
                    Message = $"Uyuşturucu satarken yakalandın! {jailTime / 365} yıl hapis."
                };
            }
            else
            {
                float drugMoney = Random.Range(10000f, 100000f);
                character.Finances.ModifyMoney(drugMoney, "Uyuşturucu satışı");

                return new CrimeResult
                {
                    Success = true,
                    MoneyGained = drugMoney,
                    Message = $"Satış başarılı! {drugMoney:N0} TL kazandın."
                };
            }
        }

        /// <summary>
        /// Birine saldır.
        /// </summary>
        public static CrimeResult Assault(CharacterData character, Relationship target = null)
        {
            if (character.Age < 12)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Bu eylem için çok küçüksün."
                };
            }

            float successChance = 0.5f + (character.Stats.Health / 200f);
            bool success = Random.value < successChance;

            if (success)
            {
                // Saldırı başarılı ama yakalanma riski
                float catchChance = 0.4f;
                bool caught = Random.value < catchChance;

                if (caught)
                {
                    int jailTime = Random.Range(30, 365);

                    return new CrimeResult
                    {
                        Success = true,
                        Caught = true,
                        JailTime = jailTime,
                        Message = $"Saldırı yaptın ama yakalandın! {jailTime} gün hapis."
                    };
                }
                else
                {
                    character.Stats.ModifyStat(StatType.Happiness, -Random.Range(5, 10));

                    return new CrimeResult
                    {
                        Success = true,
                        Message = "Saldırı yaptın ve kaçtın."
                    };
                }
            }
            else
            {
                // Saldırı başarısız, dayak yedin
                int healthLoss = Random.Range(10, 30);
                character.Stats.ModifyStat(StatType.Health, -healthLoss);

                return new CrimeResult
                {
                    Success = false,
                    Message = $"Saldırın başarısız! Dayak yedin. Sağlık -{healthLoss}"
                };
            }
        }

        /// <summary>
        /// Cinayet işle.
        /// </summary>
        public static CrimeResult Murder(CharacterData character, Relationship target)
        {
            if (character.Age < 14)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Bu eylem için çok küçüksün."
                };
            }

            if (target == null)
            {
                return new CrimeResult
                {
                    Success = false,
                    Message = "Hedef seçmelisin."
                };
            }

            // Çok yüksek yakalanma riski
            float catchChance = 0.7f - (character.Stats.Intelligence / 300f);
            bool caught = Random.value < catchChance;

            target.status = RelationshipStatus.Deceased;
            string memory = $"Cinayet işledin: {target.npcName}. (Yaş {character.Age})";

            if (caught)
            {
                int jailTime = 365 * 25; // Ömür boyu hapis
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(50, 70));

                return new CrimeResult
                {
                    Success = true,
                    Caught = true,
                    JailTime = jailTime,
                    Message = $"Cinayet işledin ve yakalandın! Ömür boyu hapis cezası."
                };
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(20, 40));

                return new CrimeResult
                {
                    Success = true,
                    Message = $"{target.npcName} artık hayatta değil."
                };
            }
        }

        #endregion

        #region Hapis Sistemi

        /// <summary>
        /// Hapishanede bir yıl geçir.
        /// </summary>
        public static void ServeJailTime(CharacterData character, int days)
        {
            // Hapiste stat kayıpları
            character.Stats.ModifyStat(StatType.Happiness, -Random.Range(10, 20));
            character.Stats.ModifyStat(StatType.Health, -Random.Range(5, 15));

            // İşten kovulma
            if (character.Career.currentJob != null && days > 30)
            {
                character.Career.jobHistory.Add(character.Career.currentJob);
                character.Career.currentJob = null;
                character.isEmployed = false;
            }

            // İlişkiler zarar görür
            foreach (var rel in character.Relationships)
            {
                if (rel.status == RelationshipStatus.Active)
                {
                    rel.intimacy = Mathf.Max(0, rel.intimacy - Random.Range(5, 15));
                    rel.trust = Mathf.Max(0, rel.trust - Random.Range(5, 15));
                }
            }
        }

        /// <summary>
        /// Hapisten kaçmaya çalış.
        /// </summary>
        public static CrimeResult AttemptEscape(CharacterData character)
        {
            float escapeChance = 0.15f + (character.Stats.Intelligence / 300f);
            bool success = Random.value < escapeChance;

            if (success)
            {
                return new CrimeResult
                {
                    Success = true,
                    Message = "Hapisten kaçmayı başardın! Ama artık aranan birisin."
                };
            }
            else
            {
                character.Stats.ModifyStat(StatType.Health, -Random.Range(10, 25));

                return new CrimeResult
                {
                    Success = false,
                    JailTime = 365, // Ekstra 1 yıl
                    Message = "Kaçış girişimin başarısız! Cezana 1 yıl eklendi."
                };
            }
        }

        #endregion

        /// <summary>
        /// Suç eylemini ID'ye göre çalıştır.
        /// </summary>
        public static CrimeResult ExecuteCrime(string crimeId, CharacterData character, Relationship target = null)
        {
            return crimeId switch
            {
                "shoplift" => Shoplift(character),
                "steal_car" => StealCar(character),
                "rob_bank" => RobBank(character),
                "scam" => Scam(character),
                "deal_drugs" => DealDrugs(character),
                "assault" => Assault(character, target),
                "murder" => Murder(character, target),
                "escape" => AttemptEscape(character),
                _ => new CrimeResult { Success = false, Message = "Bilinmeyen suç eylemi." }
            };
        }

        /// <summary>
        /// Mevcut suçların listesi.
        /// </summary>
        public static List<CrimeOption> GetAvailableCrimes(CharacterData character)
        {
            var crimes = new List<CrimeOption>();

            if (character.Age >= 10)
            {
                crimes.Add(new CrimeOption
                {
                    Id = "shoplift",
                    Name = "Mağaza Hırsızlığı",
                    Description = "Marketten bir şeyler çal",
                    Risk = "Düşük",
                    MinAge = 10
                });
            }

            if (character.Age >= 16)
            {
                crimes.Add(new CrimeOption
                {
                    Id = "steal_car",
                    Name = "Araba Hırsızlığı",
                    Description = "Bir araba çal ve sat",
                    Risk = "Orta",
                    MinAge = 16
                });

                crimes.Add(new CrimeOption
                {
                    Id = "deal_drugs",
                    Name = "Uyuşturucu Satışı",
                    Description = "Yasadışı madde sat",
                    Risk = "Yüksek",
                    MinAge = 16
                });
            }

            if (character.Age >= 18)
            {
                crimes.Add(new CrimeOption
                {
                    Id = "scam",
                    Name = "Dolandırıcılık",
                    Description = "İnsanları dolandır",
                    Risk = "Orta",
                    MinAge = 18
                });

                crimes.Add(new CrimeOption
                {
                    Id = "rob_bank",
                    Name = "Banka Soygunu",
                    Description = "Bankayı soy",
                    Risk = "Çok Yüksek",
                    MinAge = 18
                });
            }

            if (character.Age >= 12)
            {
                crimes.Add(new CrimeOption
                {
                    Id = "assault",
                    Name = "Saldırı",
                    Description = "Birine saldır",
                    Risk = "Orta",
                    MinAge = 12
                });
            }

            return crimes;
        }
    }

    /// <summary>
    /// Suç sonucu.
    /// </summary>
    public class CrimeResult
    {
        public bool Success;
        public bool Caught;
        public bool Killed;
        public int JailTime;
        public float MoneyGained;
        public string Message;
    }

    /// <summary>
    /// Suç seçeneği.
    /// </summary>
    public class CrimeOption
    {
        public string Id;
        public string Name;
        public string Description;
        public string Risk;
        public int MinAge;
    }
}
