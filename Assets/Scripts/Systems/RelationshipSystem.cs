using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// İlişki Sistemi - NPC etkileşimleri ve ilişki yönetimi.
    /// </summary>
    public static class RelationshipSystem
    {
        #region Temel Etkileşimler

        /// <summary>
        /// NPC ile sohbet et.
        /// </summary>
        public static InteractionResult TalkTo(CharacterData character, Relationship relationship)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            float successChance = 0.6f + (relationship.intimacy / 200f);
            bool success = Random.value < successChance;

            if (success)
            {
                int intimacyGain = Random.Range(3, 8);
                int trustGain = Random.Range(1, 4);

                relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);
                relationship.trust = Mathf.Min(100, relationship.trust + trustGain);

                character.Stats.ModifyStat(StatType.Happiness, Random.Range(1, 4));

                string memory = $"Güzel bir sohbet yaptınız. (Yaş {character.Age})";
                relationship.memories.Add(memory);

                return new InteractionResult
                {
                    Success = true,
                    Message = $"{relationship.npcName} ile güzel bir sohbet ettin. Yakınlık +{intimacyGain}"
                };
            }
            else
            {
                int intimacyLoss = Random.Range(1, 4);
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - intimacyLoss);

                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} ile konuşma pek iyi gitmedi. Yakınlık -{intimacyLoss}"
                };
            }
        }

        /// <summary>
        /// Hediye ver.
        /// </summary>
        public static InteractionResult GiveGift(CharacterData character, Relationship relationship, float giftCost = 500f)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            if (character.Finances.CurrentMoney < giftCost)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Hediye almak için yeterli paran yok."
                };
            }

            character.Finances.ModifyMoney(-giftCost, $"Hediye ({relationship.npcName})");

            // Pahalı hediyeler daha etkili
            float giftQuality = Mathf.Clamp01(giftCost / 5000f);
            int baseGain = (int)(5 + giftQuality * 15);

            int intimacyGain = Random.Range(baseGain - 2, baseGain + 3);
            int trustGain = Random.Range(2, 6);

            relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);
            relationship.trust = Mathf.Min(100, relationship.trust + trustGain);

            character.Stats.ModifyStat(StatType.Happiness, Random.Range(2, 5));

            string memory = $"Bir hediye aldın. (Yaş {character.Age})";
            relationship.memories.Add(memory);

            return new InteractionResult
            {
                Success = true,
                Message = $"{relationship.npcName} hediyeye çok sevindi! Yakınlık +{intimacyGain}, Güven +{trustGain}"
            };
        }

        /// <summary>
        /// Birlikte vakit geçir.
        /// </summary>
        public static InteractionResult SpendTime(CharacterData character, Relationship relationship)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            float successChance = 0.7f + (relationship.intimacy / 300f);
            bool success = Random.value < successChance;

            if (success)
            {
                int intimacyGain = Random.Range(5, 12);
                int trustGain = Random.Range(2, 6);

                relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);
                relationship.trust = Mathf.Min(100, relationship.trust + trustGain);

                character.Stats.ModifyStat(StatType.Happiness, Random.Range(3, 7));

                return new InteractionResult
                {
                    Success = true,
                    Message = $"{relationship.npcName} ile harika vakit geçirdin! Yakınlık +{intimacyGain}"
                };
            }
            else
            {
                int intimacyLoss = Random.Range(2, 5);
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - intimacyLoss);

                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} ile geçirdiğin zaman pek iyi gitmedi."
                };
            }
        }

        /// <summary>
        /// İltifat et.
        /// </summary>
        public static InteractionResult Compliment(CharacterData character, Relationship relationship)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            float successChance = 0.65f;
            bool success = Random.value < successChance;

            if (success)
            {
                int intimacyGain = Random.Range(2, 6);
                relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);

                return new InteractionResult
                {
                    Success = true,
                    Message = $"{relationship.npcName} iltifatına memnun oldu. Yakınlık +{intimacyGain}"
                };
            }
            else
            {
                int intimacyLoss = Random.Range(1, 3);
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - intimacyLoss);

                return new InteractionResult
                {
                    Success = false,
                    Message = $"İltifatın pek samimi gelmedi. Yakınlık -{intimacyLoss}"
                };
            }
        }

        #endregion

        #region Olumsuz Etkileşimler

        /// <summary>
        /// Kavga et.
        /// </summary>
        public static InteractionResult Argue(CharacterData character, Relationship relationship)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            int intimacyLoss = Random.Range(10, 20);
            int trustLoss = Random.Range(5, 15);

            relationship.intimacy = Mathf.Max(0, relationship.intimacy - intimacyLoss);
            relationship.trust = Mathf.Max(0, relationship.trust - trustLoss);

            character.Stats.ModifyStat(StatType.Happiness, -Random.Range(5, 10));

            // İlişki durumu kötüleşebilir
            if (relationship.intimacy < 20 && relationship.trust < 20)
            {
                relationship.status = RelationshipStatus.Broken;
            }
            else if (relationship.intimacy < 40)
            {
                relationship.status = RelationshipStatus.Distant;
            }

            string memory = $"Kavga ettiniz. (Yaş {character.Age})";
            relationship.memories.Add(memory);

            return new InteractionResult
            {
                Success = true,
                Message = $"{relationship.npcName} ile kavga ettin. Yakınlık -{intimacyLoss}, Güven -{trustLoss}"
            };
        }

        /// <summary>
        /// Hakaret et.
        /// </summary>
        public static InteractionResult Insult(CharacterData character, Relationship relationship)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            int intimacyLoss = Random.Range(15, 30);
            int trustLoss = Random.Range(10, 25);

            relationship.intimacy = Mathf.Max(0, relationship.intimacy - intimacyLoss);
            relationship.trust = Mathf.Max(0, relationship.trust - trustLoss);

            // İlişki kopabilir
            if (relationship.intimacy < 10)
            {
                relationship.status = RelationshipStatus.Broken;

                return new InteractionResult
                {
                    Success = true,
                    Message = $"{relationship.npcName} ile ilişkin koptu!"
                };
            }

            return new InteractionResult
            {
                Success = true,
                Message = $"{relationship.npcName}'e hakaret ettin. Yakınlık -{intimacyLoss}"
            };
        }

        /// <summary>
        /// Barış.
        /// </summary>
        public static InteractionResult Reconcile(CharacterData character, Relationship relationship)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            float successChance = 0.4f + (relationship.trust / 200f);
            bool success = Random.value < successChance;

            if (success)
            {
                int intimacyGain = Random.Range(10, 20);
                int trustGain = Random.Range(5, 10);

                relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);
                relationship.trust = Mathf.Min(100, relationship.trust + trustGain);

                if (relationship.status == RelationshipStatus.Broken || relationship.status == RelationshipStatus.Distant)
                {
                    relationship.status = RelationshipStatus.Active;
                }

                character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 10));

                string memory = $"Barıştınız. (Yaş {character.Age})";
                relationship.memories.Add(memory);

                return new InteractionResult
                {
                    Success = true,
                    Message = $"{relationship.npcName} ile barıştın! Yakınlık +{intimacyGain}"
                };
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(3, 7));

                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} barışmayı reddetti."
                };
            }
        }

        #endregion

        #region Romantik İlişkiler

        /// <summary>
        /// Flört et.
        /// </summary>
        public static InteractionResult Flirt(CharacterData character, Relationship relationship)
        {
            if (relationship.status == RelationshipStatus.Deceased)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} artık hayatta değil."
                };
            }

            if (character.Age < 14)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Flört için çok küçüksün."
                };
            }

            // Aile ile flört edilemez
            if (relationship.type == RelationType.Parent || relationship.type == RelationType.Sibling ||
                relationship.type == RelationType.Child)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Aile üyesi ile flört edemezsin!"
                };
            }

            float successChance = 0.3f + (character.Stats.Appearance / 150f) + (relationship.intimacy / 200f);
            bool success = Random.value < successChance;

            if (success)
            {
                int intimacyGain = Random.Range(8, 15);
                relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);

                // İlişki türü değişebilir
                if (relationship.type == RelationType.Friend && relationship.intimacy >= 60)
                {
                    relationship.type = RelationType.Boyfriend; // veya Girlfriend
                }

                return new InteractionResult
                {
                    Success = true,
                    Message = $"{relationship.npcName} flörtüne karşılık verdi! Yakınlık +{intimacyGain}"
                };
            }
            else
            {
                int intimacyLoss = Random.Range(3, 8);
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - intimacyLoss);

                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} flörtüne olumsuz tepki verdi."
                };
            }
        }

        /// <summary>
        /// Evlenme teklifi et.
        /// </summary>
        public static InteractionResult Propose(CharacterData character, Relationship relationship)
        {
            if (character.Age < 18)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Evlenmek için 18 yaşından büyük olmalısın."
                };
            }

            if (character.isMarried)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Zaten evlisin!"
                };
            }

            if (relationship.type != RelationType.Boyfriend && relationship.type != RelationType.Girlfriend)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Sadece sevgiline evlenme teklifi edebilirsin."
                };
            }

            float successChance = 0.3f + (relationship.intimacy / 150f) + (relationship.trust / 200f);
            bool success = Random.value < successChance;

            if (success)
            {
                relationship.type = RelationType.Spouse;
                character.isMarried = true;

                character.Stats.ModifyStat(StatType.Happiness, Random.Range(20, 30));

                string memory = $"Evlendiniz! (Yaş {character.Age})";
                relationship.memories.Add(memory);

                return new InteractionResult
                {
                    Success = true,
                    Message = $"{relationship.npcName} EVET dedi! Evleniyorsunuz!"
                };
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -Random.Range(15, 25));

                return new InteractionResult
                {
                    Success = false,
                    Message = $"{relationship.npcName} teklifini reddetti."
                };
            }
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public static InteractionResult Divorce(CharacterData character, Relationship relationship)
        {
            if (relationship.type != RelationType.Spouse)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Bu kişiyle evli değilsin."
                };
            }

            relationship.type = RelationType.ExSpouse;
            character.isMarried = false;

            // Mal paylaşımı
            float settlement = character.Finances.CurrentMoney * 0.3f;
            character.Finances.ModifyMoney(-settlement, "Boşanma nafakası");

            character.Stats.ModifyStat(StatType.Happiness, -Random.Range(20, 35));

            string memory = $"Boşandınız. (Yaş {character.Age})";
            relationship.memories.Add(memory);

            return new InteractionResult
            {
                Success = true,
                Message = $"{relationship.npcName} ile boşandın. Nafaka: {settlement:N0} TL"
            };
        }

        #endregion

        #region Arkadaşlık

        /// <summary>
        /// Yeni arkadaş edin.
        /// </summary>
        public static InteractionResult MakeNewFriend(CharacterData character)
        {
            if (character.Age < 5)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Arkadaş edinmek için çok küçüksün."
                };
            }

            float successChance = 0.5f + (character.Stats.Appearance / 200f) + (character.Stats.Happiness / 300f);

            if (Random.value < successChance)
            {
                // Yeni NPC oluştur
                var newFriend = CharacterFactory.CreateRandomFriend(character.Age);
                character.Relationships.Add(newFriend);

                character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 10));

                return new InteractionResult
                {
                    Success = true,
                    Message = $"Yeni bir arkadaş edindin: {newFriend.npcName}!"
                };
            }
            else
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Yeni biriyle tanışamadın."
                };
            }
        }

        /// <summary>
        /// Sevgili bul.
        /// </summary>
        public static InteractionResult FindLoveInterest(CharacterData character)
        {
            if (character.Age < 16)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Romantik ilişki için çok küçüksün."
                };
            }

            if (character.isMarried)
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Zaten evlisin!"
                };
            }

            float successChance = 0.3f + (character.Stats.Appearance / 150f) + (character.Stats.Happiness / 300f);

            if (Random.value < successChance)
            {
                // Tercih edilen cinsiyet (basit implementasyon)
                Gender preferredGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;
                var loveInterest = CharacterFactory.CreateLoveInterest(character.Age, preferredGender);
                character.Relationships.Add(loveInterest);

                character.Stats.ModifyStat(StatType.Happiness, Random.Range(10, 20));

                return new InteractionResult
                {
                    Success = true,
                    Message = $"Yeni biriyle tanıştın: {loveInterest.npcName}!"
                };
            }
            else
            {
                return new InteractionResult
                {
                    Success = false,
                    Message = "Kimseyle tanışamadın."
                };
            }
        }

        #endregion

        /// <summary>
        /// Etkileşim tipine göre çalıştır.
        /// </summary>
        public static InteractionResult ExecuteInteraction(string interactionType, CharacterData character, Relationship relationship)
        {
            return interactionType switch
            {
                "talk" => TalkTo(character, relationship),
                "gift" => GiveGift(character, relationship),
                "spend_time" => SpendTime(character, relationship),
                "compliment" => Compliment(character, relationship),
                "argue" => Argue(character, relationship),
                "insult" => Insult(character, relationship),
                "reconcile" => Reconcile(character, relationship),
                "flirt" => Flirt(character, relationship),
                "propose" => Propose(character, relationship),
                "divorce" => Divorce(character, relationship),
                _ => new InteractionResult { Success = false, Message = "Bilinmeyen etkileşim." }
            };
        }
    }

    /// <summary>
    /// Etkileşim sonucu.
    /// </summary>
    public class InteractionResult
    {
        public bool Success;
        public string Message;
    }
}
