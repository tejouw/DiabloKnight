using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// İlişki Yöneticisi - Aile ve sosyal ilişkileri yönetir.
    /// </summary>
    public class RelationshipManager : Singleton<RelationshipManager>
    {
        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);
            Debug.Log("[RelationshipManager] Initialized successfully.");
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<AgeProgressedEvent>(OnAgeProgressed);
        }

        #endregion

        #region Event Handlers

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // NPC'leri yaşlandır
            AgeAllNPCs(character);

            // İlişkileri güncelle
            UpdateRelationships(character);

            // Ölüm kontrolü
            CheckNPCDeaths(character);
        }

        #endregion

        #region NPC Aging

        /// <summary>
        /// Tüm NPC'leri yaşlandır.
        /// </summary>
        private void AgeAllNPCs(CharacterData character)
        {
            foreach (var relationship in character.relationships)
            {
                if (relationship.status != RelationshipStatus.Deceased)
                {
                    relationship.age++;
                }
            }
        }

        /// <summary>
        /// NPC ölümlerini kontrol et.
        /// </summary>
        private void CheckNPCDeaths(CharacterData character)
        {
            foreach (var relationship in character.relationships)
            {
                if (relationship.status == RelationshipStatus.Deceased) continue;

                // Yaşlılık ölümü
                if (relationship.age >= 70)
                {
                    float deathChance = (relationship.age - 70) * 0.03f;
                    if (UnityEngine.Random.value < deathChance)
                    {
                        HandleNPCDeath(character, relationship);
                    }
                }

                // Maksimum yaş
                if (relationship.age >= 100)
                {
                    HandleNPCDeath(character, relationship);
                }
            }
        }

        /// <summary>
        /// NPC ölümünü işle.
        /// </summary>
        private void HandleNPCDeath(CharacterData character, Relationship relationship)
        {
            relationship.status = RelationshipStatus.Deceased;

            // Yakınlık düzeyine göre üzüntü
            int sadnessAmount = -(relationship.intimacy / 5);
            character.stats.ModifyStat(StatType.Happiness, sadnessAmount);

            EventBus.Publish(new NPCDeathEvent
            {
                NPCName = relationship.npcName,
                RelationType = relationship.type
            });
        }

        #endregion

        #region Relationship Updates

        /// <summary>
        /// İlişkileri güncelle.
        /// </summary>
        private void UpdateRelationships(CharacterData character)
        {
            foreach (var relationship in character.relationships)
            {
                if (relationship.status != RelationshipStatus.Active) continue;

                // Doğal azalma
                int decay = UnityEngine.Random.Range(1, 3);
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - decay);
                relationship.trust = Mathf.Max(0, relationship.trust - 1);

                // Uzak ilişki kontrolü
                if (relationship.intimacy < 20 && relationship.type != RelationType.Parent)
                {
                    relationship.status = RelationshipStatus.Distant;
                }
            }
        }

        #endregion

        #region Social Interactions

        /// <summary>
        /// Sohbet et.
        /// </summary>
        public void Chat(CharacterData character, Relationship relationship)
        {
            if (relationship.status != RelationshipStatus.Active) return;

            int intimacyGain = UnityEngine.Random.Range(2, 5);
            relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);

            character.stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(1, 3));

            relationship.memories.Add($"{character.age} yaşında sohbet ettiniz.");

            EventBus.Publish(new SocialInteractionEvent
            {
                InteractionType = "Sohbet",
                NPCName = relationship.npcName,
                IntimacyChange = intimacyGain
            });
        }

        /// <summary>
        /// Hediye ver.
        /// </summary>
        public void GiveGift(CharacterData character, Relationship relationship, decimal giftValue)
        {
            if (character.finances.CurrentMoney < giftValue) return;

            character.finances.ModifyMoney(-giftValue, $"{relationship.npcName}'e hediye");

            int intimacyGain = (int)(giftValue / 100);
            intimacyGain = Mathf.Clamp(intimacyGain, 5, 20);

            relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);
            relationship.trust = Mathf.Min(100, relationship.trust + (intimacyGain / 2));

            relationship.memories.Add($"{character.age} yaşında hediye aldınız.");

            EventBus.Publish(new SocialInteractionEvent
            {
                InteractionType = "Hediye",
                NPCName = relationship.npcName,
                IntimacyChange = intimacyGain
            });
        }

        /// <summary>
        /// Birlikte vakit geçir.
        /// </summary>
        public void SpendTime(CharacterData character, Relationship relationship)
        {
            if (relationship.status != RelationshipStatus.Active) return;

            int intimacyGain = UnityEngine.Random.Range(5, 10);
            int trustGain = UnityEngine.Random.Range(2, 5);

            relationship.intimacy = Mathf.Min(100, relationship.intimacy + intimacyGain);
            relationship.trust = Mathf.Min(100, relationship.trust + trustGain);

            character.stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(3, 7));

            relationship.memories.Add($"{character.age} yaşında birlikte vakit geçirdiniz.");

            EventBus.Publish(new SocialInteractionEvent
            {
                InteractionType = "Vakit Geçirme",
                NPCName = relationship.npcName,
                IntimacyChange = intimacyGain
            });
        }

        /// <summary>
        /// Tartış.
        /// </summary>
        public void Argue(CharacterData character, Relationship relationship)
        {
            int intimacyLoss = UnityEngine.Random.Range(5, 15);
            int trustLoss = UnityEngine.Random.Range(3, 10);

            relationship.intimacy = Mathf.Max(0, relationship.intimacy - intimacyLoss);
            relationship.trust = Mathf.Max(0, relationship.trust - trustLoss);

            character.stats.ModifyStat(StatType.Happiness, -UnityEngine.Random.Range(3, 8));

            relationship.memories.Add($"{character.age} yaşında tartıştınız.");

            // İlişki kopması
            if (relationship.intimacy <= 0 || relationship.trust <= 0)
            {
                if (relationship.type == RelationType.Friend ||
                    relationship.type == RelationType.Boyfriend ||
                    relationship.type == RelationType.Girlfriend)
                {
                    EndRelationship(character, relationship);
                }
            }

            EventBus.Publish(new SocialInteractionEvent
            {
                InteractionType = "Tartışma",
                NPCName = relationship.npcName,
                IntimacyChange = -intimacyLoss
            });
        }

        /// <summary>
        /// Barış.
        /// </summary>
        public void Reconcile(CharacterData character, Relationship relationship)
        {
            if (relationship.status != RelationshipStatus.Broken &&
                relationship.status != RelationshipStatus.Distant) return;

            float successChance = 0.3f + (character.stats.Intelligence / 200f);

            if (UnityEngine.Random.value <= successChance)
            {
                relationship.status = RelationshipStatus.Active;
                relationship.intimacy = Mathf.Max(30, relationship.intimacy);
                relationship.trust = Mathf.Max(20, relationship.trust);

                character.stats.ModifyStat(StatType.Happiness, 10);

                EventBus.Publish(new ReconciliationEvent
                {
                    NPCName = relationship.npcName,
                    Success = true
                });
            }
            else
            {
                character.stats.ModifyStat(StatType.Happiness, -5);

                EventBus.Publish(new ReconciliationEvent
                {
                    NPCName = relationship.npcName,
                    Success = false
                });
            }
        }

        /// <summary>
        /// İlişkiyi bitir.
        /// </summary>
        public void EndRelationship(CharacterData character, Relationship relationship)
        {
            relationship.status = RelationshipStatus.Broken;

            // Ex türüne çevir
            if (relationship.type == RelationType.Boyfriend ||
                relationship.type == RelationType.Girlfriend)
            {
                relationship.type = RelationType.Ex;
            }
            else if (relationship.type == RelationType.Spouse)
            {
                relationship.type = RelationType.ExSpouse;
                character.isMarried = false;
            }

            character.stats.ModifyStat(StatType.Happiness, -15);

            EventBus.Publish(new RelationshipEndedEvent
            {
                NPCName = relationship.npcName
            });
        }

        #endregion

        #region Marriage System

        /// <summary>
        /// Evlenme teklifi et.
        /// </summary>
        public bool ProposeMarriage(CharacterData character, Relationship relationship)
        {
            if (character.isMarried) return false;
            if (relationship.type != RelationType.Boyfriend &&
                relationship.type != RelationType.Girlfriend) return false;

            // Kabul şansı
            float acceptChance = (relationship.intimacy + relationship.trust) / 250f;

            if (UnityEngine.Random.value <= acceptChance)
            {
                EventBus.Publish(new ProposalResultEvent
                {
                    NPCName = relationship.npcName,
                    Accepted = true
                });
                return true;
            }

            // Reddedilme
            relationship.intimacy = Mathf.Max(0, relationship.intimacy - 20);
            character.stats.ModifyStat(StatType.Happiness, -25);

            EventBus.Publish(new ProposalResultEvent
            {
                NPCName = relationship.npcName,
                Accepted = false
            });

            return false;
        }

        /// <summary>
        /// Evlen.
        /// </summary>
        public void GetMarried(CharacterData character, Relationship relationship)
        {
            relationship.type = RelationType.Spouse;
            character.isMarried = true;

            // Düğün maliyeti
            decimal weddingCost = UnityEngine.Random.Range(20000, 100000);
            character.finances.ModifyMoney(-weddingCost, "Düğün masrafları");

            character.stats.ModifyStat(StatType.Happiness, 30);

            relationship.memories.Add($"{character.age} yaşında evlendiniz.");

            EventBus.Publish(new MarriageEvent
            {
                SpouseName = relationship.npcName,
                WeddingCost = weddingCost
            });
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public void GetDivorced(CharacterData character, Relationship relationship)
        {
            if (relationship.type != RelationType.Spouse) return;

            relationship.type = RelationType.ExSpouse;
            relationship.status = RelationshipStatus.Broken;
            character.isMarried = false;

            // Mal paylaşımı
            decimal divorceCost = character.finances.CurrentMoney / 2;
            character.finances.ModifyMoney(-divorceCost, "Boşanma mal paylaşımı");

            character.stats.ModifyStat(StatType.Happiness, -30);

            EventBus.Publish(new DivorceEvent
            {
                ExSpouseName = relationship.npcName,
                DivorceCost = divorceCost
            });
        }

        #endregion

        #region Children System

        /// <summary>
        /// Çocuk sahibi ol.
        /// </summary>
        public Relationship HaveChild(CharacterData character)
        {
            if (!character.isMarried) return null;

            var spouse = character.relationships.FirstOrDefault(
                r => r.type == RelationType.Spouse && r.status == RelationshipStatus.Active);

            if (spouse == null) return null;

            // Çocuğun cinsiyeti
            Gender childGender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;

            var dataManager = DataManager.Instance;
            string childName = childGender == Gender.Male
                ? dataManager.GetRandomMaleName()
                : dataManager.GetRandomFemaleName();

            var child = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{childName} {character.lastName}",
                type = RelationType.Child,
                gender = childGender,
                age = 0,
                intimacy = 100,
                trust = 100,
                status = RelationshipStatus.Active
            };

            character.relationships.Add(child);
            character.stats.ModifyStat(StatType.Happiness, 25);

            EventBus.Publish(new ChildBornEvent
            {
                ChildName = child.npcName,
                Gender = childGender
            });

            return child;
        }

        #endregion

        #region Dating System

        /// <summary>
        /// Yeni sevgili bul.
        /// </summary>
        public Relationship StartDating(CharacterData character, Gender preferredGender)
        {
            if (character.isMarried) return null;
            if (character.age < 16) return null;

            // Mevcut sevgili var mı?
            var currentPartner = character.relationships.FirstOrDefault(
                r => (r.type == RelationType.Boyfriend || r.type == RelationType.Girlfriend) &&
                     r.status == RelationshipStatus.Active);

            if (currentPartner != null) return null;

            // Yeni sevgili oluştur
            var partner = CharacterFactory.CreateLoveInterest(character.age, preferredGender);
            character.relationships.Add(partner);

            character.stats.ModifyStat(StatType.Happiness, 15);

            EventBus.Publish(new DatingStartedEvent
            {
                PartnerName = partner.npcName
            });

            return partner;
        }

        /// <summary>
        /// Yeni arkadaş edin.
        /// </summary>
        public Relationship MakeFriend(CharacterData character)
        {
            var friend = CharacterFactory.CreateRandomFriend(character.age);
            character.relationships.Add(friend);

            character.stats.ModifyStat(StatType.Happiness, 5);

            EventBus.Publish(new FriendMadeEvent
            {
                FriendName = friend.npcName
            });

            return friend;
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// İlişkileri türe göre al.
        /// </summary>
        public List<Relationship> GetRelationshipsByType(CharacterData character, RelationType type)
        {
            return character.relationships.Where(r => r.type == type).ToList();
        }

        /// <summary>
        /// Aktif ilişkileri al.
        /// </summary>
        public List<Relationship> GetActiveRelationships(CharacterData character)
        {
            return character.relationships
                .Where(r => r.status == RelationshipStatus.Active)
                .ToList();
        }

        /// <summary>
        /// Eşi al.
        /// </summary>
        public Relationship GetSpouse(CharacterData character)
        {
            return character.relationships.FirstOrDefault(
                r => r.type == RelationType.Spouse && r.status == RelationshipStatus.Active);
        }

        /// <summary>
        /// Çocukları al.
        /// </summary>
        public List<Relationship> GetChildren(CharacterData character)
        {
            return character.relationships
                .Where(r => r.type == RelationType.Child)
                .ToList();
        }

        #endregion
    }

    #region Events

    public class NPCDeathEvent
    {
        public string NPCName;
        public RelationType RelationType;
    }

    public class SocialInteractionEvent
    {
        public string InteractionType;
        public string NPCName;
        public int IntimacyChange;
    }

    public class ReconciliationEvent
    {
        public string NPCName;
        public bool Success;
    }

    public class RelationshipEndedEvent
    {
        public string NPCName;
    }

    public class ProposalResultEvent
    {
        public string NPCName;
        public bool Accepted;
    }

    public class MarriageEvent
    {
        public string SpouseName;
        public decimal WeddingCost;
    }

    public class DivorceEvent
    {
        public string ExSpouseName;
        public decimal DivorceCost;
    }

    public class ChildBornEvent
    {
        public string ChildName;
        public Gender Gender;
    }

    public class DatingStartedEvent
    {
        public string PartnerName;
    }

    public class FriendMadeEvent
    {
        public string FriendName;
    }

    #endregion
}
