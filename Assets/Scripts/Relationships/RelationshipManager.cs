using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Relationships
{
    /// <summary>
    /// İlişki Yöneticisi - Flört, evlilik, boşanma ve çocuk sistemini yönetir.
    /// </summary>
    public class RelationshipManager : Singleton<RelationshipManager>
    {
        // Flört havuzu
        private List<PotentialPartner> _datingPool = new List<PotentialPartner>();
        private const int DATING_POOL_SIZE = 10;

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[RelationshipManager] Initialized successfully.");
        }

        #endregion

        #region Dating System

        /// <summary>
        /// Flört havuzunu yenile.
        /// </summary>
        public void RefreshDatingPool(CharacterData character)
        {
            _datingPool.Clear();

            for (int i = 0; i < DATING_POOL_SIZE; i++)
            {
                _datingPool.Add(GeneratePotentialPartner(character));
            }
        }

        /// <summary>
        /// Potansiyel partner oluştur.
        /// </summary>
        private PotentialPartner GeneratePotentialPartner(CharacterData character)
        {
            var dataManager = DataManager.Instance;

            // Cinsiyet belirleme (basit hetero model, genişletilebilir)
            Gender partnerGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;

            string firstName = partnerGender == Gender.Male
                ? dataManager.GetRandomMaleName()
                : dataManager.GetRandomFemaleName();

            // Yaş: karakter yaşına yakın (+/- 10 yaş)
            int age = Mathf.Clamp(character.Age + Random.Range(-10, 11), 18, 80);

            return new PotentialPartner
            {
                id = System.Guid.NewGuid().ToString(),
                name = $"{firstName} {dataManager.GetRandomSurname()}",
                gender = partnerGender,
                age = age,
                appearance = Random.Range(30, 100),
                intelligence = Random.Range(30, 100),
                wealth = Random.Range(0, 100),
                crazy = Random.Range(0, 100),
                occupation = GetRandomOccupation(),
                compatibility = CalculateCompatibility(character, age)
            };
        }

        /// <summary>
        /// Uyumluluk hesapla.
        /// </summary>
        private int CalculateCompatibility(CharacterData character, int partnerAge)
        {
            int baseCompat = Random.Range(30, 90);

            // Yaş farkı etkisi
            int ageDiff = Mathf.Abs(character.Age - partnerAge);
            if (ageDiff > 20) baseCompat -= 20;
            else if (ageDiff > 10) baseCompat -= 10;

            return Mathf.Clamp(baseCompat, 10, 100);
        }

        /// <summary>
        /// Flört havuzunu al.
        /// </summary>
        public List<PotentialPartner> GetDatingPool()
        {
            return _datingPool;
        }

        /// <summary>
        /// Biriyle flört et.
        /// </summary>
        public DatingResult GoOnDate(CharacterData character, string partnerId)
        {
            var partner = _datingPool.FirstOrDefault(p => p.id == partnerId);
            if (partner == null)
            {
                return new DatingResult
                {
                    success = false,
                    resultText = "Partner bulunamadı!"
                };
            }

            // Para kontrolü (flört masrafı)
            int dateCost = 500;
            if (character.Finances.CurrentMoney < dateCost)
            {
                return new DatingResult
                {
                    success = false,
                    resultText = "Flört için yeterli paran yok!"
                };
            }

            character.Finances.ModifyMoney(-dateCost, "Flört");

            // Başarı hesapla
            float successChance = (partner.compatibility + character.Stats.Appearance) / 200f;
            successChance = Mathf.Clamp(successChance, 0.2f, 0.9f);

            bool success = Random.value <= successChance;
            var result = new DatingResult { partnerId = partnerId };

            if (success)
            {
                // İlişki başlat
                var relationship = new Relationship
                {
                    npcId = partner.id,
                    npcName = partner.name,
                    type = partner.gender == Gender.Male ? RelationType.Boyfriend : RelationType.Girlfriend,
                    gender = partner.gender,
                    age = partner.age,
                    intimacy = Random.Range(30, 50),
                    trust = Random.Range(30, 50),
                    status = RelationshipStatus.Active
                };

                character.relationships.Add(relationship);
                _datingPool.Remove(partner);

                result.success = true;
                result.resultText = $"{partner.name} ile çıkmaya başladın!";

                character.Stats.ModifyStat(StatType.Happiness, 10);

                EventBus.Publish(new RelationshipChangedEvent
                {
                    NpcId = partner.id,
                    NpcName = partner.name,
                    ChangeType = RelationshipChangeType.Created
                });
            }
            else
            {
                result.success = false;
                result.resultText = $"{partner.name} seni reddetti.";
                character.Stats.ModifyStat(StatType.Happiness, -5);
            }

            return result;
        }

        #endregion

        #region Relationship Actions

        /// <summary>
        /// İlişkide vakit geçir.
        /// </summary>
        public string SpendTime(CharacterData character, string npcId)
        {
            var relationship = character.relationships.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null) return "İlişki bulunamadı!";

            int intimacyGain = Random.Range(3, 8);
            relationship.intimacy = Mathf.Clamp(relationship.intimacy + intimacyGain, 0, 100);

            character.Stats.ModifyStat(StatType.Happiness, 3);

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = npcId,
                NpcName = relationship.npcName,
                ChangeType = RelationshipChangeType.Improved
            });

            return $"{relationship.npcName} ile güzel vakit geçirdin. (+{intimacyGain} yakınlık)";
        }

        /// <summary>
        /// Hediye ver.
        /// </summary>
        public string GiveGift(CharacterData character, string npcId, int giftValue)
        {
            var relationship = character.relationships.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null) return "İlişki bulunamadı!";

            if (character.Finances.CurrentMoney < giftValue)
            {
                return "Yeterli paran yok!";
            }

            character.Finances.ModifyMoney(-giftValue, "Hediye");

            int intimacyGain = giftValue / 100;
            relationship.intimacy = Mathf.Clamp(relationship.intimacy + intimacyGain, 0, 100);

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = npcId,
                NpcName = relationship.npcName,
                ChangeType = RelationshipChangeType.Improved
            });

            return $"{relationship.npcName} hediyeni çok beğendi! (+{intimacyGain} yakınlık)";
        }

        /// <summary>
        /// Konuş.
        /// </summary>
        public string HaveConversation(CharacterData character, string npcId)
        {
            var relationship = character.relationships.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null) return "İlişki bulunamadı!";

            // Rastgele konuşma sonucu
            string[] goodResults = {
                "Güzel bir sohbet ettiniz.",
                "Çok güldünüz.",
                "Derin bir muhabbet yaptınız.",
                "Birbirinizi daha iyi anladınız."
            };

            string[] badResults = {
                "Tartışmaya dönüştü.",
                "Yanlış bir şey söyledin.",
                "Konuşma gergin geçti."
            };

            bool goodConversation = Random.value > 0.3f;

            if (goodConversation)
            {
                int gain = Random.Range(2, 6);
                relationship.intimacy = Mathf.Clamp(relationship.intimacy + gain, 0, 100);
                return goodResults[Random.Range(0, goodResults.Length)] + $" (+{gain} yakınlık)";
            }
            else
            {
                int loss = Random.Range(2, 8);
                relationship.intimacy = Mathf.Clamp(relationship.intimacy - loss, 0, 100);
                return badResults[Random.Range(0, badResults.Length)] + $" (-{loss} yakınlık)";
            }
        }

        /// <summary>
        /// İlişkiyi bitir.
        /// </summary>
        public string BreakUp(CharacterData character, string npcId)
        {
            var relationship = character.relationships.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null) return "İlişki bulunamadı!";

            if (relationship.type == RelationType.Boyfriend || relationship.type == RelationType.Girlfriend)
            {
                relationship.type = RelationType.Ex;
                relationship.status = RelationshipStatus.Broken;
                relationship.intimacy = Mathf.Max(0, relationship.intimacy - 50);

                character.Stats.ModifyStat(StatType.Happiness, -15);

                EventBus.Publish(new RelationshipChangedEvent
                {
                    NpcId = npcId,
                    NpcName = relationship.npcName,
                    ChangeType = RelationshipChangeType.Ended
                });

                return $"{relationship.npcName} ile ayrıldın.";
            }

            return "Bu ilişki tipi sonlandırılamaz.";
        }

        #endregion

        #region Marriage System

        /// <summary>
        /// Evlenme teklifi et.
        /// </summary>
        public MarriageResult ProposeMarriage(CharacterData character, string npcId)
        {
            var relationship = character.relationships.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null)
            {
                return new MarriageResult { success = false, resultText = "İlişki bulunamadı!" };
            }

            // Yaş kontrolü
            if (character.Age < 18)
            {
                return new MarriageResult { success = false, resultText = "Evlenmek için 18 yaşından büyük olmalısın!" };
            }

            // Zaten evli mi?
            if (character.isMarried)
            {
                return new MarriageResult { success = false, resultText = "Zaten evlisin!" };
            }

            // İlişki türü kontrolü
            if (relationship.type != RelationType.Boyfriend && relationship.type != RelationType.Girlfriend)
            {
                return new MarriageResult { success = false, resultText = "Önce flört etmelisin!" };
            }

            // Kabul şansı (yakınlık + güven ortalaması)
            float acceptChance = (relationship.intimacy + relationship.trust) / 200f;
            acceptChance = Mathf.Clamp(acceptChance, 0.1f, 0.95f);

            bool accepted = Random.value <= acceptChance;

            if (accepted)
            {
                // Evlilik gerçekleşti
                relationship.type = RelationType.Spouse;
                character.isMarried = true;

                // Soyisim değişikliği (opsiyonel)
                if (character.Gender == Gender.Female)
                {
                    string spouseSurname = relationship.npcName.Split(' ').Last();
                    character.lastName = spouseSurname;
                }

                character.Stats.ModifyStat(StatType.Happiness, 25);

                EventBus.Publish(new RelationshipChangedEvent
                {
                    NpcId = npcId,
                    NpcName = relationship.npcName,
                    ChangeType = RelationshipChangeType.StatusChanged
                });

                return new MarriageResult
                {
                    success = true,
                    resultText = $"{relationship.npcName} ile evlendin! Mutluluklar!"
                };
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -20);

                return new MarriageResult
                {
                    success = false,
                    resultText = $"{relationship.npcName} teklifini reddetti."
                };
            }
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public string Divorce(CharacterData character)
        {
            var spouse = character.relationships.FirstOrDefault(r => r.type == RelationType.Spouse);
            if (spouse == null)
            {
                return "Evli değilsin!";
            }

            spouse.type = RelationType.ExSpouse;
            spouse.status = RelationshipStatus.Broken;
            character.isMarried = false;

            // Mal paylaşımı (basit model)
            decimal divorce_cost = character.Finances.CurrentMoney * 0.4m;
            character.Finances.ModifyMoney(-divorce_cost, "Boşanma nafakası");

            character.Stats.ModifyStat(StatType.Happiness, -30);

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = spouse.npcId,
                NpcName = spouse.npcName,
                ChangeType = RelationshipChangeType.Ended
            });

            return $"{spouse.npcName} ile boşandın. Nafaka: {divorce_cost:N0} TL";
        }

        #endregion

        #region Children System

        /// <summary>
        /// Çocuk sahibi olmaya çalış.
        /// </summary>
        public ChildResult TryForBaby(CharacterData character)
        {
            var spouse = character.relationships.FirstOrDefault(r => r.type == RelationType.Spouse);
            if (spouse == null)
            {
                return new ChildResult { success = false, resultText = "Önce evlenmelisin!" };
            }

            // Yaş kontrolü (kadın için)
            int motherAge = character.Gender == Gender.Female ? character.Age : spouse.age;
            if (motherAge > 45)
            {
                return new ChildResult { success = false, resultText = "Çocuk sahibi olmak için çok geç." };
            }

            // Başarı şansı (yaşa göre azalır)
            float successChance = 0.6f - (motherAge - 25) * 0.02f;
            successChance = Mathf.Clamp(successChance, 0.1f, 0.7f);

            bool success = Random.value <= successChance;

            if (success)
            {
                // Çocuk doğdu
                var dataManager = DataManager.Instance;
                Gender childGender = Random.value < 0.5f ? Gender.Male : Gender.Female;
                string childName = childGender == Gender.Male
                    ? dataManager.GetRandomMaleName()
                    : dataManager.GetRandomFemaleName();

                var child = new Relationship
                {
                    npcId = System.Guid.NewGuid().ToString(),
                    npcName = $"{childName} {character.LastName}",
                    type = RelationType.Child,
                    gender = childGender,
                    age = 0,
                    intimacy = 100,
                    trust = 100,
                    status = RelationshipStatus.Active
                };

                character.relationships.Add(child);
                character.Stats.ModifyStat(StatType.Happiness, 20);

                EventBus.Publish(new RelationshipChangedEvent
                {
                    NpcId = child.npcId,
                    NpcName = child.npcName,
                    ChangeType = RelationshipChangeType.Created
                });

                string genderText = childGender == Gender.Male ? "oğlunuz" : "kızınız";
                return new ChildResult
                {
                    success = true,
                    resultText = $"Tebrikler! {childName} adında bir {genderText} oldu!",
                    childName = childName
                };
            }
            else
            {
                return new ChildResult
                {
                    success = false,
                    resultText = "Bu sefer olmadı."
                };
            }
        }

        #endregion

        #region Utility

        private string GetRandomOccupation()
        {
            string[] occupations = {
                "Öğretmen", "Doktor", "Mühendis", "Avukat", "İşçi", "Memur",
                "Esnaf", "Öğrenci", "Hemşire", "Polis", "İşsiz", "Freelancer",
                "Yazılımcı", "Bankacı", "Gazeteci", "Aşçı", "Garson", "Şoför"
            };
            return occupations[Random.Range(0, occupations.Length)];
        }

        #endregion
    }

    #region Data Structures

    /// <summary>
    /// Potansiyel partner (flört havuzu için).
    /// </summary>
    [System.Serializable]
    public class PotentialPartner
    {
        public string id;
        public string name;
        public Gender gender;
        public int age;
        public int appearance;
        public int intelligence;
        public int wealth;
        public int crazy;
        public string occupation;
        public int compatibility;
    }

    /// <summary>
    /// Flört sonucu.
    /// </summary>
    public class DatingResult
    {
        public string partnerId;
        public bool success;
        public string resultText;
    }

    /// <summary>
    /// Evlilik sonucu.
    /// </summary>
    public class MarriageResult
    {
        public bool success;
        public string resultText;
    }

    /// <summary>
    /// Çocuk sonucu.
    /// </summary>
    public class ChildResult
    {
        public bool success;
        public string resultText;
        public string childName;
    }

    #endregion
}
