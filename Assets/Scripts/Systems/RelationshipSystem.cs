using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// İlişki Sistemi - Flört, evlilik, boşanma, çocuk.
    /// </summary>
    public class RelationshipSystem : Singleton<RelationshipSystem>
    {
        #region Dating & Flirting

        /// <summary>
        /// Yeni biriyle tanış.
        /// </summary>
        public MeetResult MeetSomeone(CharacterData character)
        {
            if (character.Age < 14)
            {
                return new MeetResult
                {
                    success = false,
                    message = "Henüz çok küçüksün."
                };
            }

            // Yeni NPC oluştur
            Gender preferredGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;
            var npc = CharacterFactory.CreateNPC(preferredGender, character.Age + Random.Range(-5, 6));

            // Kimya hesapla
            float chemistry = CalculateChemistry(character, npc);

            if (chemistry > 0.5f)
            {
                // Tanışma başarılı
                var relationship = new Relationship
                {
                    npcId = System.Guid.NewGuid().ToString(),
                    npcName = npc.FullName,
                    type = RelationType.Acquaintance,
                    intimacy = Random.Range(20, 40),
                    trust = Random.Range(30, 50),
                    status = RelationshipStatus.Active,
                    age = npc.Age,
                    gender = npc.Gender
                };

                character.Relationships.Add(relationship);

                return new MeetResult
                {
                    success = true,
                    message = $"{npc.FullName} ({npc.Age}) ile tanıştın! İyi anlaşıyor gibisiniz.",
                    npcName = npc.FullName
                };
            }
            else
            {
                return new MeetResult
                {
                    success = false,
                    message = "Biriyle tanıştın ama pek anlaşamadınız."
                };
            }
        }

        /// <summary>
        /// Flört et.
        /// </summary>
        public FlirtResult Flirt(CharacterData character, string npcId)
        {
            var relationship = character.Relationships.Find(r => r.npcId == npcId);
            if (relationship == null)
            {
                return new FlirtResult { success = false, message = "Bu kişi bulunamadı." };
            }

            if (character.Age < 14)
            {
                return new FlirtResult { success = false, message = "Henüz çok küçüksün." };
            }

            // Başarı şansı
            float successChance = 0.3f;
            successChance += (character.Stats.Appearance - 50) * 0.005f;
            successChance += (character.Stats.Happiness - 50) * 0.003f;
            successChance += relationship.intimacy * 0.003f;

            if (Random.value < successChance)
            {
                relationship.intimacy = Mathf.Min(100, relationship.intimacy + Random.Range(5, 15));

                // İlişki türünü güncelle
                if (relationship.type == RelationType.Acquaintance && relationship.intimacy > 50)
                {
                    relationship.type = character.Gender == Gender.Male ? RelationType.Girlfriend : RelationType.Boyfriend;

                    return new FlirtResult
                    {
                        success = true,
                        message = $"{relationship.npcName} ile çıkmaya başladınız!"
                    };
                }

                return new FlirtResult
                {
                    success = true,
                    message = $"{relationship.npcName} ile flörtün iyi gitti! (+Yakınlık)"
                };
            }
            else
            {
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - Random.Range(3, 8));
                character.Stats.ModifyStat(StatType.Happiness, -3);

                return new FlirtResult
                {
                    success = false,
                    message = $"{relationship.npcName} sana pek ilgi göstermedi."
                };
            }
        }

        /// <summary>
        /// Randevuya çık.
        /// </summary>
        public DateResult GoOnDate(CharacterData character, string npcId)
        {
            var relationship = character.Relationships.Find(r => r.npcId == npcId);
            if (relationship == null)
            {
                return new DateResult { success = false, message = "Bu kişi bulunamadı." };
            }

            if (relationship.type != RelationType.Boyfriend && relationship.type != RelationType.Girlfriend)
            {
                return new DateResult { success = false, message = "Bu kişiyle çıkmıyorsun." };
            }

            // Randevu maliyeti
            decimal dateCost = Random.Range(100, 500);
            if (character.Finances.CurrentMoney < dateCost)
            {
                return new DateResult { success = false, message = $"Randevu için yeterli paran yok ({dateCost:N0} TL)." };
            }

            character.Finances.ModifyMoney(-dateCost, "Randevu");

            // Randevu kalitesi
            float quality = Random.value;
            if (quality > 0.7f)
            {
                // Harika randevu
                relationship.intimacy = Mathf.Min(100, relationship.intimacy + Random.Range(10, 20));
                relationship.trust = Mathf.Min(100, relationship.trust + Random.Range(5, 10));
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 10));

                return new DateResult
                {
                    success = true,
                    message = $"{relationship.npcName} ile harika vakit geçirdiniz!"
                };
            }
            else if (quality > 0.3f)
            {
                // Normal randevu
                relationship.intimacy = Mathf.Min(100, relationship.intimacy + Random.Range(3, 8));
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(2, 5));

                return new DateResult
                {
                    success = true,
                    message = $"{relationship.npcName} ile güzel vakit geçirdiniz."
                };
            }
            else
            {
                // Kötü randevu
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - Random.Range(5, 15));
                character.Stats.ModifyStat(StatType.Happiness, -3);

                return new DateResult
                {
                    success = false,
                    message = "Randevu pek iyi gitmedi..."
                };
            }
        }

        #endregion

        #region Marriage

        /// <summary>
        /// Evlenme teklifi yap.
        /// </summary>
        public ProposeResult Propose(CharacterData character, string npcId)
        {
            var relationship = character.Relationships.Find(r => r.npcId == npcId);
            if (relationship == null)
            {
                return new ProposeResult { success = false, message = "Bu kişi bulunamadı." };
            }

            if (character.Age < 18)
            {
                return new ProposeResult { success = false, message = "Evlenmek için 18 yaşında olmalısın." };
            }

            if (character.IsMarried)
            {
                return new ProposeResult { success = false, message = "Zaten evlisin!" };
            }

            if (relationship.type != RelationType.Boyfriend && relationship.type != RelationType.Girlfriend)
            {
                return new ProposeResult { success = false, message = "Bu kişiyle çıkmıyorsun." };
            }

            // Yüzük maliyeti
            decimal ringCost = 5000;
            if (character.Finances.CurrentMoney < ringCost)
            {
                return new ProposeResult { success = false, message = $"Yüzük almak için yeterli paran yok ({ringCost:N0} TL)." };
            }

            // Kabul şansı
            float acceptChance = relationship.intimacy * 0.007f + relationship.trust * 0.003f;
            if (Random.value < acceptChance)
            {
                character.Finances.ModifyMoney(-ringCost, "Nişan yüzüğü");
                relationship.type = RelationType.Spouse;
                character.isMarried = true;

                // Düğün
                PerformWedding(character, relationship);

                return new ProposeResult
                {
                    success = true,
                    message = $"{relationship.npcName} teklifini kabul etti! Evlendiniz!"
                };
            }
            else
            {
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - 20);
                character.Stats.ModifyStat(StatType.Happiness, -15);

                return new ProposeResult
                {
                    success = false,
                    message = $"{relationship.npcName} henüz evlenmeye hazır değil."
                };
            }
        }

        private void PerformWedding(CharacterData character, Relationship spouse)
        {
            // Düğün maliyeti
            decimal weddingCost = Random.Range(10000, 50000);
            character.Finances.ModifyMoney(-weddingCost, "Düğün masrafları");

            // Mutluluk bonusu
            character.Stats.ModifyStat(StatType.Happiness, 20);
            character.Stats.ModifyStat(StatType.Fame, 2);

            // Düğün hediyesi (akrabalardan)
            decimal gifts = Random.Range(5000, 20000);
            character.Finances.ModifyMoney(gifts, "Düğün hediyeleri");

            spouse.memories.Add("Evlilik");

            EventBus.Publish(new MarriageEvent
            {
                SpouseName = spouse.npcName,
                EventType = MarriageEventType.Married
            });
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public DivorceResult Divorce(CharacterData character, string npcId)
        {
            var spouse = character.Relationships.Find(r => r.npcId == npcId && r.type == RelationType.Spouse);
            if (spouse == null)
            {
                return new DivorceResult { success = false, message = "Bu kişiyle evli değilsin." };
            }

            // Boşanma maliyeti
            decimal divorceCost = 10000;
            character.Finances.ModifyMoney(-divorceCost, "Boşanma masrafları");

            // Mal paylaşımı (paranın yarısını kaybedebilirsin)
            if (Random.value < 0.5f)
            {
                decimal loss = character.Finances.CurrentMoney * 0.3m;
                character.Finances.ModifyMoney(-loss, "Boşanma anlaşması");
            }

            spouse.type = RelationType.ExSpouse;
            spouse.status = RelationshipStatus.Broken;
            character.isMarried = false;

            character.Stats.ModifyStat(StatType.Happiness, -25);

            EventBus.Publish(new MarriageEvent
            {
                SpouseName = spouse.npcName,
                EventType = MarriageEventType.Divorced
            });

            return new DivorceResult
            {
                success = true,
                message = $"{spouse.npcName} ile boşandın."
            };
        }

        #endregion

        #region Children

        /// <summary>
        /// Çocuk sahibi ol.
        /// </summary>
        public ChildResult HaveChild(CharacterData character)
        {
            if (!character.IsMarried)
            {
                return new ChildResult { success = false, message = "Çocuk sahibi olmak için evli olmalısın." };
            }

            var spouse = character.Relationships.Find(r => r.type == RelationType.Spouse);
            if (spouse == null)
            {
                return new ChildResult { success = false, message = "Eşin bulunamadı." };
            }

            // Bebek cinsiyeti
            Gender babyGender = Random.value < 0.5f ? Gender.Male : Gender.Female;
            string[] boyNames = { "Ahmet", "Mehmet", "Ali", "Mustafa", "Can", "Ege", "Berk", "Kaan", "Yusuf", "Emre" };
            string[] girlNames = { "Ayşe", "Fatma", "Zeynep", "Elif", "Defne", "Ela", "Su", "Deniz", "Yağmur", "İrem" };

            string babyName = babyGender == Gender.Male
                ? boyNames[Random.Range(0, boyNames.Length)]
                : girlNames[Random.Range(0, girlNames.Length)];

            // Çocuk ilişkisi oluştur
            var child = new Relationship
            {
                npcId = System.Guid.NewGuid().ToString(),
                npcName = $"{babyName} {character.LastName}",
                type = RelationType.Child,
                intimacy = 80,
                trust = 90,
                status = RelationshipStatus.Active,
                age = 0,
                gender = babyGender
            };

            character.Relationships.Add(child);

            // Etkiler
            character.Stats.ModifyStat(StatType.Happiness, 15);
            character.Finances.ModifyMoney(-5000, "Doğum masrafları");

            EventBus.Publish(new ChildBornEvent
            {
                ChildName = child.npcName,
                Gender = babyGender
            });

            return new ChildResult
            {
                success = true,
                message = $"Tebrikler! {child.npcName} adında bir {(babyGender == Gender.Male ? "oğlunuz" : "kızınız")} oldu!",
                childName = child.npcName
            };
        }

        #endregion

        #region Relationship Management

        /// <summary>
        /// İlişkiyi bitir.
        /// </summary>
        public void BreakUp(CharacterData character, string npcId)
        {
            var relationship = character.Relationships.Find(r => r.npcId == npcId);
            if (relationship == null) return;

            if (relationship.type == RelationType.Boyfriend || relationship.type == RelationType.Girlfriend)
            {
                relationship.type = RelationType.Ex;
                relationship.status = RelationshipStatus.Broken;
                character.Stats.ModifyStat(StatType.Happiness, -10);

                EventBus.Publish(new RelationshipChangedEvent
                {
                    NpcId = npcId,
                    NpcName = relationship.npcName,
                    ChangeType = RelationshipChangeType.Ended
                });
            }
        }

        /// <summary>
        /// İlişki yakınlığını artır.
        /// </summary>
        public void SpendTimeWith(CharacterData character, string npcId)
        {
            var relationship = character.Relationships.Find(r => r.npcId == npcId);
            if (relationship == null) return;

            relationship.intimacy = Mathf.Min(100, relationship.intimacy + Random.Range(3, 8));
            relationship.trust = Mathf.Min(100, relationship.trust + Random.Range(1, 4));
        }

        /// <summary>
        /// Tartış.
        /// </summary>
        public void Argue(CharacterData character, string npcId)
        {
            var relationship = character.Relationships.Find(r => r.npcId == npcId);
            if (relationship == null) return;

            relationship.intimacy = Mathf.Max(0, relationship.intimacy - Random.Range(5, 15));
            relationship.trust = Mathf.Max(0, relationship.trust - Random.Range(3, 8));
            character.Stats.ModifyStat(StatType.Happiness, -5);

            // İlişki çok kötüleştiyse
            if (relationship.intimacy < 10 && (relationship.type == RelationType.Boyfriend || relationship.type == RelationType.Girlfriend))
            {
                BreakUp(character, npcId);
            }
        }

        private float CalculateChemistry(CharacterData character, CharacterData npc)
        {
            float chemistry = 0.5f;
            chemistry += (character.Stats.Appearance - 50) * 0.005f;
            chemistry += Random.Range(-0.2f, 0.3f);
            return Mathf.Clamp01(chemistry);
        }

        /// <summary>
        /// Yıllık ilişki güncellemesi.
        /// </summary>
        public void ProcessAnnualRelationships(CharacterData character)
        {
            foreach (var relationship in character.Relationships)
            {
                // Çocukları yaşlandır
                if (relationship.type == RelationType.Child)
                {
                    relationship.age++;
                }

                // Rastgele ilişki değişimleri
                if (relationship.status == RelationshipStatus.Active)
                {
                    int change = Random.Range(-5, 5);
                    relationship.intimacy = Mathf.Clamp(relationship.intimacy + change, 0, 100);
                }
            }
        }

        #endregion
    }

    #region Data Classes & Events

    public class MeetResult
    {
        public bool success;
        public string message;
        public string npcName;
    }

    public class FlirtResult
    {
        public bool success;
        public string message;
    }

    public class DateResult
    {
        public bool success;
        public string message;
    }

    public class ProposeResult
    {
        public bool success;
        public string message;
    }

    public class DivorceResult
    {
        public bool success;
        public string message;
    }

    public class ChildResult
    {
        public bool success;
        public string message;
        public string childName;
    }

    public enum MarriageEventType
    {
        Married,
        Divorced
    }

    public struct MarriageEvent : IGameEvent
    {
        public string SpouseName;
        public MarriageEventType EventType;
    }

    public struct ChildBornEvent : IGameEvent
    {
        public string ChildName;
        public Gender Gender;
    }

    #endregion
}
