using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// İlişki Yöneticisi - Tüm sosyal ilişkileri yönetir.
    /// </summary>
    public class RelationshipManager : Singleton<RelationshipManager>
    {
        #region Constants

        public const int MIN_DATING_AGE = 16;
        public const int MIN_MARRIAGE_AGE = 18;
        public const int INTIMACY_FOR_DATING = 60;
        public const int INTIMACY_FOR_MARRIAGE = 80;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[RelationshipManager] Initialized successfully.");
        }

        #endregion

        #region Relationship Access

        /// <summary>
        /// Belirli türdeki ilişkileri getir.
        /// </summary>
        public List<Relationship> GetRelationshipsByType(CharacterData character, RelationType type)
        {
            return character?.Relationships?.Where(r => r.type == type && r.status == RelationshipStatus.Active).ToList()
                   ?? new List<Relationship>();
        }

        /// <summary>
        /// Tüm aktif ilişkileri getir.
        /// </summary>
        public List<Relationship> GetActiveRelationships(CharacterData character)
        {
            return character?.Relationships?.Where(r => r.status == RelationshipStatus.Active).ToList()
                   ?? new List<Relationship>();
        }

        /// <summary>
        /// Ebeveynleri getir.
        /// </summary>
        public List<Relationship> GetParents(CharacterData character)
        {
            return GetRelationshipsByType(character, RelationType.Parent);
        }

        /// <summary>
        /// Kardeşleri getir.
        /// </summary>
        public List<Relationship> GetSiblings(CharacterData character)
        {
            return GetRelationshipsByType(character, RelationType.Sibling);
        }

        /// <summary>
        /// Eşi getir.
        /// </summary>
        public Relationship GetSpouse(CharacterData character)
        {
            return character?.Relationships?.FirstOrDefault(r => r.type == RelationType.Spouse && r.status == RelationshipStatus.Active);
        }

        /// <summary>
        /// Mevcut sevgiliyi getir.
        /// </summary>
        public Relationship GetCurrentPartner(CharacterData character)
        {
            return character?.Relationships?.FirstOrDefault(r =>
                (r.type == RelationType.Boyfriend || r.type == RelationType.Girlfriend) &&
                r.status == RelationshipStatus.Active);
        }

        /// <summary>
        /// Çocukları getir.
        /// </summary>
        public List<Relationship> GetChildren(CharacterData character)
        {
            return GetRelationshipsByType(character, RelationType.Child);
        }

        #endregion

        #region Meeting People

        /// <summary>
        /// Yeni biriyle tanış.
        /// </summary>
        public Relationship MeetNewPerson(CharacterData character, RelationType type)
        {
            if (character == null) return null;

            Gender gender;
            int age;

            switch (type)
            {
                case RelationType.Friend:
                    gender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
                    age = character.Age + UnityEngine.Random.Range(-5, 6);
                    break;
                case RelationType.Boyfriend:
                    gender = Gender.Male;
                    age = character.Age + UnityEngine.Random.Range(-5, 6);
                    break;
                case RelationType.Girlfriend:
                    gender = Gender.Female;
                    age = character.Age + UnityEngine.Random.Range(-5, 6);
                    break;
                case RelationType.Colleague:
                    gender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
                    age = character.Age + UnityEngine.Random.Range(-10, 10);
                    break;
                default:
                    gender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
                    age = character.Age + UnityEngine.Random.Range(-10, 10);
                    break;
            }

            age = Mathf.Max(1, age);

            var newRelationship = CharacterFactory.CreateNPC(type, gender, age);
            character.Relationships.Add(newRelationship);

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = newRelationship.npcId,
                NpcName = newRelationship.npcName,
                ChangeType = RelationshipChangeType.Created
            });

            Debug.Log($"[RelationshipManager] {character.FullName} met {newRelationship.npcName}");

            return newRelationship;
        }

        /// <summary>
        /// Flört etmek için biriyle tanış.
        /// </summary>
        public Relationship MeetPotentialPartner(CharacterData character)
        {
            if (character == null || character.Age < MIN_DATING_AGE) return null;

            // Tercih edilen cinsiyet (basit hetero model, genişletilebilir)
            Gender preferredGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;
            RelationType type = preferredGender == Gender.Male ? RelationType.Boyfriend : RelationType.Girlfriend;

            return MeetNewPerson(character, type);
        }

        #endregion

        #region Interaction

        /// <summary>
        /// İlişkiyle etkileşime geç.
        /// </summary>
        public InteractionResult Interact(CharacterData character, string npcId, InteractionType interaction)
        {
            var relationship = character?.Relationships?.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null)
            {
                return new InteractionResult { Success = false, Message = "Kişi bulunamadı." };
            }

            int intimacyChange = 0;
            int trustChange = 0;
            string message = "";

            switch (interaction)
            {
                case InteractionType.Chat:
                    intimacyChange = UnityEngine.Random.Range(1, 5);
                    trustChange = UnityEngine.Random.Range(0, 3);
                    message = $"{relationship.npcName} ile sohbet ettin.";
                    break;

                case InteractionType.GiveGift:
                    intimacyChange = UnityEngine.Random.Range(5, 15);
                    trustChange = UnityEngine.Random.Range(2, 8);
                    message = $"{relationship.npcName}'a hediye verdin.";
                    character.Finances.ModifyMoney(-UnityEngine.Random.Range(100, 500), "Hediye");
                    break;

                case InteractionType.SpendTime:
                    intimacyChange = UnityEngine.Random.Range(3, 10);
                    trustChange = UnityEngine.Random.Range(1, 5);
                    message = $"{relationship.npcName} ile vakit geçirdin.";
                    break;

                case InteractionType.Compliment:
                    intimacyChange = UnityEngine.Random.Range(2, 8);
                    if (UnityEngine.Random.value < 0.8f)
                        message = $"{relationship.npcName}'a iltifat ettin, memnun oldu.";
                    else
                    {
                        intimacyChange = -intimacyChange;
                        message = $"{relationship.npcName} iltifatını samimi bulmadı.";
                    }
                    break;

                case InteractionType.Insult:
                    intimacyChange = UnityEngine.Random.Range(-15, -5);
                    trustChange = UnityEngine.Random.Range(-10, -3);
                    message = $"{relationship.npcName}'a hakaret ettin.";
                    break;

                case InteractionType.Argue:
                    intimacyChange = UnityEngine.Random.Range(-10, 5);
                    trustChange = UnityEngine.Random.Range(-8, 2);
                    message = intimacyChange < 0
                        ? $"{relationship.npcName} ile tartıştın, ilişkiniz gerildi."
                        : $"{relationship.npcName} ile tartıştın ama sonra barıştınız.";
                    break;

                case InteractionType.Flirt:
                    if (relationship.type == RelationType.Boyfriend ||
                        relationship.type == RelationType.Girlfriend ||
                        relationship.type == RelationType.Spouse)
                    {
                        intimacyChange = UnityEngine.Random.Range(5, 15);
                        message = $"{relationship.npcName} ile flört ettin.";
                    }
                    else
                    {
                        if (UnityEngine.Random.value < 0.5f)
                        {
                            intimacyChange = UnityEngine.Random.Range(3, 10);
                            message = $"{relationship.npcName} flörtünü beğendi.";
                        }
                        else
                        {
                            intimacyChange = UnityEngine.Random.Range(-5, 0);
                            message = $"{relationship.npcName} flörtünü yadırgadı.";
                        }
                    }
                    break;

                case InteractionType.Kiss:
                    if (IsRomantic(relationship))
                    {
                        intimacyChange = UnityEngine.Random.Range(8, 20);
                        message = $"{relationship.npcName}'ı öptün.";
                    }
                    else
                    {
                        intimacyChange = UnityEngine.Random.Range(-10, -3);
                        message = $"{relationship.npcName} öpücüğü beklenmiyordu, garip oldu.";
                    }
                    break;
            }

            // İlişki değerlerini güncelle
            ModifyRelationship(relationship, intimacyChange, trustChange);

            // İlişki durumunu kontrol et
            CheckRelationshipStatus(character, relationship);

            return new InteractionResult
            {
                Success = intimacyChange >= 0,
                Message = message,
                IntimacyChange = intimacyChange,
                TrustChange = trustChange
            };
        }

        /// <summary>
        /// İlişki değerlerini değiştir.
        /// </summary>
        private void ModifyRelationship(Relationship relationship, int intimacyChange, int trustChange)
        {
            relationship.intimacy = Mathf.Clamp(relationship.intimacy + intimacyChange, 0, 100);
            relationship.trust = Mathf.Clamp(relationship.trust + trustChange, 0, 100);

            RelationshipChangeType changeType = intimacyChange >= 0
                ? RelationshipChangeType.Improved
                : RelationshipChangeType.Worsened;

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = relationship.npcId,
                NpcName = relationship.npcName,
                ChangeType = changeType
            });
        }

        /// <summary>
        /// Romantik ilişki mi kontrol et.
        /// </summary>
        private bool IsRomantic(Relationship relationship)
        {
            return relationship.type == RelationType.Boyfriend ||
                   relationship.type == RelationType.Girlfriend ||
                   relationship.type == RelationType.Spouse;
        }

        /// <summary>
        /// İlişki durumunu kontrol et.
        /// </summary>
        private void CheckRelationshipStatus(CharacterData character, Relationship relationship)
        {
            // Çok düşük intimacy = ilişki biter
            if (relationship.intimacy <= 0 || relationship.trust <= 0)
            {
                if (IsRomantic(relationship))
                {
                    BreakUp(character, relationship.npcId);
                }
                else
                {
                    relationship.status = RelationshipStatus.Distant;
                }
            }
        }

        #endregion

        #region Dating & Romance

        /// <summary>
        /// Çıkma teklif et.
        /// </summary>
        public bool AskOut(CharacterData character, string npcId)
        {
            var relationship = character?.Relationships?.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null || character.Age < MIN_DATING_AGE) return false;

            // Zaten sevgiliyse
            if (IsRomantic(relationship)) return false;

            // Başarı şansı
            float successChance = relationship.intimacy / 100f * 0.8f + character.Stats.Appearance / 100f * 0.2f;

            if (UnityEngine.Random.value < successChance)
            {
                // Kabul edildi
                relationship.type = character.Gender == Gender.Male
                    ? RelationType.Girlfriend
                    : RelationType.Boyfriend;
                relationship.intimacy = Mathf.Min(relationship.intimacy + 20, 100);

                EventBus.Publish(new RelationshipChangedEvent
                {
                    NpcId = relationship.npcId,
                    NpcName = relationship.npcName,
                    ChangeType = RelationshipChangeType.StatusChanged
                });

                return true;
            }

            // Reddedildi
            relationship.intimacy = Mathf.Max(relationship.intimacy - 10, 0);
            return false;
        }

        /// <summary>
        /// Ayrıl.
        /// </summary>
        public void BreakUp(CharacterData character, string npcId)
        {
            var relationship = character?.Relationships?.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null) return;

            relationship.type = RelationType.Ex;
            relationship.status = RelationshipStatus.Broken;
            relationship.intimacy = Mathf.Max(relationship.intimacy - 30, 0);

            // Mutluluk kaybı
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(-20, -10));

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = relationship.npcId,
                NpcName = relationship.npcName,
                ChangeType = RelationshipChangeType.Ended
            });

            Debug.Log($"[RelationshipManager] {character.FullName} broke up with {relationship.npcName}");
        }

        #endregion

        #region Marriage

        /// <summary>
        /// Evlenme teklifi yap.
        /// </summary>
        public bool Propose(CharacterData character, string npcId)
        {
            var partner = character?.Relationships?.FirstOrDefault(r => r.npcId == npcId);
            if (partner == null || character.Age < MIN_MARRIAGE_AGE) return false;

            // Sevgili olmalı
            if (!IsRomantic(partner) || partner.type == RelationType.Spouse) return false;

            // Yeterli intimacy gerekli
            if (partner.intimacy < INTIMACY_FOR_MARRIAGE) return false;

            // Başarı şansı
            float successChance = (partner.intimacy - 50) / 50f * 0.7f + partner.trust / 100f * 0.3f;

            if (UnityEngine.Random.value < successChance)
            {
                // Kabul edildi - evlen
                Marry(character, partner);
                return true;
            }

            // Reddedildi
            partner.intimacy = Mathf.Max(partner.intimacy - 20, 0);
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(-25, -15));
            return false;
        }

        /// <summary>
        /// Evlen.
        /// </summary>
        private void Marry(CharacterData character, Relationship partner)
        {
            partner.type = RelationType.Spouse;
            partner.intimacy = 100;
            partner.trust = Mathf.Min(partner.trust + 20, 100);

            character.isMarried = true;

            // Büyük mutluluk
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(25, 40));

            // Düğün masrafı
            character.Finances.ModifyMoney(-UnityEngine.Random.Range(20000, 100000), "Düğün masrafları");

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = partner.npcId,
                NpcName = partner.npcName,
                ChangeType = RelationshipChangeType.StatusChanged
            });

            Debug.Log($"[RelationshipManager] {character.FullName} married {partner.npcName}!");
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public void Divorce(CharacterData character)
        {
            var spouse = GetSpouse(character);
            if (spouse == null) return;

            spouse.type = RelationType.ExSpouse;
            spouse.status = RelationshipStatus.Broken;
            spouse.intimacy = 0;
            spouse.trust = 0;

            character.isMarried = false;

            // Mal paylaşımı
            long assets = character.Finances.CurrentMoney;
            character.Finances.ModifyMoney(-assets / 2, "Boşanma - mal paylaşımı");

            // Büyük mutluluk kaybı
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(-30, -20));

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = spouse.npcId,
                NpcName = spouse.npcName,
                ChangeType = RelationshipChangeType.Ended
            });

            Debug.Log($"[RelationshipManager] {character.FullName} divorced from {spouse.npcName}");
        }

        #endregion

        #region Children

        /// <summary>
        /// Çocuk sahibi ol.
        /// </summary>
        public Relationship HaveChild(CharacterData character)
        {
            if (!character.IsMarried) return null;

            Gender childGender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
            string childName = childGender == Gender.Male
                ? DataManager.Instance.GetRandomMaleName()
                : DataManager.Instance.GetRandomFemaleName();

            var child = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{childName} {character.LastName}",
                type = RelationType.Child,
                gender = childGender,
                age = 0,
                intimacy = 100,
                trust = 100,
                status = RelationshipStatus.Active
            };

            character.Relationships.Add(child);

            // Mutluluk artışı
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(20, 35));

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = child.npcId,
                NpcName = child.npcName,
                ChangeType = RelationshipChangeType.Created
            });

            Debug.Log($"[RelationshipManager] {character.FullName} had a baby: {child.npcName}");

            return child;
        }

        #endregion

        #region Yearly Updates

        /// <summary>
        /// Yıllık ilişki güncellemesi.
        /// </summary>
        public void ProcessYearlyRelationshipUpdates(CharacterData character)
        {
            if (character?.Relationships == null) return;

            foreach (var relationship in character.Relationships.ToList())
            {
                if (relationship.status != RelationshipStatus.Active) continue;

                // NPC yaşını artır
                relationship.age++;

                // Ebeveynler için ölüm kontrolü
                if (relationship.type == RelationType.Parent && relationship.age >= 70)
                {
                    float deathChance = (relationship.age - 70) * 0.03f;
                    if (UnityEngine.Random.value < deathChance)
                    {
                        relationship.status = RelationshipStatus.Deceased;
                        character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(-30, -15));
                        Debug.Log($"[RelationshipManager] {relationship.npcName} passed away.");
                    }
                }

                // İlişki bozulması (etkileşim yoksa)
                if (relationship.type != RelationType.Parent &&
                    relationship.type != RelationType.Spouse &&
                    relationship.type != RelationType.Child)
                {
                    int decay = UnityEngine.Random.Range(1, 5);
                    relationship.intimacy = Mathf.Max(relationship.intimacy - decay, 0);

                    if (relationship.intimacy <= 10)
                    {
                        relationship.status = RelationshipStatus.Distant;
                    }
                }

                // Evlilik stresi (düşük trust)
                if (relationship.type == RelationType.Spouse && relationship.trust < 30)
                {
                    if (UnityEngine.Random.value < 0.1f)
                    {
                        Divorce(character);
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Etkileşim türleri.
    /// </summary>
    public enum InteractionType
    {
        Chat,
        GiveGift,
        SpendTime,
        Compliment,
        Insult,
        Argue,
        Flirt,
        Kiss
    }

    /// <summary>
    /// Etkileşim sonucu.
    /// </summary>
    public class InteractionResult
    {
        public bool Success;
        public string Message;
        public int IntimacyChange;
        public int TrustChange;
    }
}
