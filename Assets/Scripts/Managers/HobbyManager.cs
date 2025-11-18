using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Yetenek ve hobi sistemi yöneticisi.
    /// Yeteneklerin keşfi, gelişimi ve hobilerin yönetimi.
    /// </summary>
    public class HobbyManager : Singleton<HobbyManager>
    {
        // Yetenek keşif olasılıkları (yaşa göre)
        private readonly Dictionary<LifeStage, float> _discoveryChances = new Dictionary<LifeStage, float>
        {
            { LifeStage.Baby, 0.05f },
            { LifeStage.Child, 0.15f },
            { LifeStage.Teen, 0.10f },
            { LifeStage.YoungAdult, 0.05f },
            { LifeStage.Adult, 0.02f },
            { LifeStage.Senior, 0.01f }
        };

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[HobbyManager] Initialized successfully.");
        }

        #region Talent Management

        /// <summary>
        /// Yıllık yetenek kontrolü - Yeni yetenekler keşfedilebilir.
        /// </summary>
        public void ProcessYearlyTalentCheck(CharacterData character)
        {
            if (character?.talentData == null) return;

            float discoveryChance = _discoveryChances.GetValueOrDefault(character.CurrentLifeStage, 0.05f);

            // Intelligence yetenek keşfini artırır
            discoveryChance += character.Stats.Intelligence * 0.001f;

            if (Random.value < discoveryChance)
            {
                DiscoverRandomTalent(character);
            }
        }

        /// <summary>
        /// Rastgele yetenek keşfet.
        /// </summary>
        public void DiscoverRandomTalent(CharacterData character)
        {
            var availableTalents = GetAvailableTalentsForAge(character.Age);

            // Zaten sahip olunan yetenekleri çıkar
            availableTalents.RemoveAll(t => character.talentData.HasTalent(t));

            if (availableTalents.Count == 0) return;

            // Rastgele seç
            var selectedTalent = availableTalents[Random.Range(0, availableTalents.Count)];

            // Başlangıç beceri seviyesi (intelligence'a göre)
            int initialSkill = Random.Range(5, 15) + (character.Stats.Intelligence / 10);

            character.talentData.AddTalent(selectedTalent, initialSkill);

            var info = TalentDefinitions.GetTalentInfo(selectedTalent);
            Debug.Log($"[HobbyManager] {character.FullName} yeni yetenek keşfetti: {info.Name}");
        }

        /// <summary>
        /// Yaşa uygun yetenekleri al.
        /// </summary>
        private List<TalentType> GetAvailableTalentsForAge(int age)
        {
            var talents = new List<TalentType>();

            // Çocukluk yetenekleri
            if (age >= 5)
            {
                talents.Add(TalentType.Drawing);
                talents.Add(TalentType.Swimming);
                talents.Add(TalentType.Football);
            }

            // Okul çağı
            if (age >= 7)
            {
                talents.Add(TalentType.Piano);
                talents.Add(TalentType.Guitar);
                talents.Add(TalentType.Mathematics);
                talents.Add(TalentType.Chess);
                talents.Add(TalentType.Basketball);
            }

            // Ergenlik
            if (age >= 12)
            {
                talents.Add(TalentType.Singing);
                talents.Add(TalentType.Writing);
                talents.Add(TalentType.Painting);
                talents.Add(TalentType.Coding);
                talents.Add(TalentType.Acting);
                talents.Add(TalentType.MartialArts);
                talents.Add(TalentType.Saz);
            }

            // Yetişkinlik
            if (age >= 18)
            {
                talents.Add(TalentType.Photography);
                talents.Add(TalentType.PublicSpeaking);
                talents.Add(TalentType.Cooking);
                talents.Add(TalentType.Crafting);
            }

            return talents;
        }

        /// <summary>
        /// Yetenek pratiği yap.
        /// </summary>
        public int PracticeTalent(CharacterData character, TalentType talentType, int hours)
        {
            if (character?.talentData == null) return 0;

            var talent = character.talentData.GetTalent(talentType);
            if (talent == null)
            {
                Debug.LogWarning($"[HobbyManager] Character doesn't have talent: {talentType}");
                return 0;
            }

            int oldLevel = talent.skillLevel;
            int gained = talent.Practice(hours, character.Stats.Intelligence);

            if (gained > 0)
            {
                EventBus.Publish(new TalentLevelChangedEvent
                {
                    TalentType = talentType,
                    OldLevel = oldLevel,
                    NewLevel = talent.skillLevel,
                    PracticeHours = hours
                });

                // Pratik yapmak mutluluk verir
                int happinessGain = Mathf.Min(gained, 3);
                character.Stats.ModifyStat(StatType.Happiness, happinessGain);
            }

            character.talentData.totalPracticeHours += hours;

            return gained;
        }

        /// <summary>
        /// Yetenek ile para kazan.
        /// </summary>
        public decimal EarnFromTalent(CharacterData character, TalentType talentType)
        {
            if (character?.talentData == null) return 0;

            var talent = character.talentData.GetTalent(talentType);
            if (talent == null) return 0;

            // Seviye 50'den düşükse para kazanılamaz
            if (talent.skillLevel < 50)
            {
                return 0;
            }

            // Temel kazanç
            decimal baseEarning = talent.skillLevel * 10;

            // Rank bonusu
            float rankMultiplier = talent.currentRank switch
            {
                TalentRank.Advanced => 1.5f,
                TalentRank.Expert => 2.5f,
                TalentRank.Master => 5f,
                _ => 1f
            };

            // Fame bonusu
            float fameMultiplier = 1f + (character.Stats.Fame * 0.02f);

            decimal totalEarning = baseEarning * (decimal)rankMultiplier * (decimal)fameMultiplier;

            character.Finances.ModifyMoney(totalEarning, $"{TalentDefinitions.GetTalentInfo(talentType).Name} yeteneği ile kazanç");

            // Fame artışı
            if (Random.value < 0.1f && talent.skillLevel >= 80)
            {
                character.Stats.ModifyStat(StatType.Fame, Random.Range(1, 3));
            }

            return totalEarning;
        }

        #endregion

        #region Hobby Management

        /// <summary>
        /// Yeni hobi başlat.
        /// </summary>
        public bool StartHobby(CharacterData character, HobbyType hobbyType)
        {
            if (character?.talentData == null) return false;

            if (character.talentData.HasHobby(hobbyType))
            {
                Debug.LogWarning($"[HobbyManager] Character already has hobby: {hobbyType}");
                return false;
            }

            // Maliyet kontrolü
            var info = TalentDefinitions.GetHobbyInfo(hobbyType);
            if (info.MonthlyCost > 0 && character.Finances.CurrentMoney < info.MonthlyCost)
            {
                Debug.LogWarning($"[HobbyManager] Not enough money to start hobby: {hobbyType}");
                return false;
            }

            character.talentData.AddHobby(hobbyType);

            var hobby = character.talentData.GetHobby(hobbyType);
            if (hobby != null)
            {
                hobby.startAge = character.Age;
            }

            Debug.Log($"[HobbyManager] {character.FullName} yeni hobi başlattı: {info.Name}");
            return true;
        }

        /// <summary>
        /// Hobi aktivitesi yap.
        /// </summary>
        public HobbyActivityResult DoHobbyActivity(CharacterData character, HobbyType hobbyType, int hours)
        {
            if (character?.talentData == null)
            {
                return new HobbyActivityResult();
            }

            var hobby = character.talentData.GetHobby(hobbyType);
            if (hobby == null || !hobby.isActive)
            {
                return new HobbyActivityResult();
            }

            var result = hobby.DoActivity(hours);

            // Stat değişikliklerini uygula
            if (result.HappinessGain > 0)
            {
                character.Stats.ModifyStat(StatType.Happiness, result.HappinessGain);
            }

            if (result.HealthGain > 0)
            {
                character.Stats.ModifyStat(StatType.Health, result.HealthGain);
            }

            // Event yayınla
            EventBus.Publish(new HobbyActivityEvent
            {
                HobbyType = hobbyType,
                HappinessGain = result.HappinessGain,
                HealthGain = result.HealthGain
            });

            return result;
        }

        /// <summary>
        /// Hobiyi bırak.
        /// </summary>
        public void QuitHobby(CharacterData character, HobbyType hobbyType)
        {
            if (character?.talentData == null) return;

            var hobby = character.talentData.GetHobby(hobbyType);
            if (hobby == null) return;

            hobby.isActive = false;

            EventBus.Publish(new HobbyQuitEvent
            {
                HobbyType = hobbyType,
                TotalSessions = hobby.totalSessions
            });

            Debug.Log($"[HobbyManager] {character.FullName} hobiyi bıraktı: {hobbyType}");
        }

        /// <summary>
        /// Yıllık hobi maliyetlerini işle.
        /// </summary>
        public decimal ProcessYearlyHobbyCosts(CharacterData character)
        {
            if (character?.talentData == null) return 0;

            decimal totalCost = 0;

            foreach (var hobby in character.talentData.GetActiveHobbies())
            {
                var info = TalentDefinitions.GetHobbyInfo(hobby.type);
                decimal yearlyCost = info.MonthlyCost * 12;

                if (yearlyCost > 0)
                {
                    // Para yetmiyorsa hobiyi pasif yap
                    if (character.Finances.CurrentMoney < yearlyCost)
                    {
                        hobby.isActive = false;
                        Debug.Log($"[HobbyManager] {info.Name} hobisi para yetersizliğinden bırakıldı.");
                        continue;
                    }

                    character.Finances.ModifyMoney(-yearlyCost, $"{info.Name} hobi masrafları");
                    totalCost += yearlyCost;
                }
            }

            return totalCost;
        }

        /// <summary>
        /// Yıllık hobi mutluluk bonusu.
        /// </summary>
        public void ProcessYearlyHobbyBonuses(CharacterData character)
        {
            if (character?.talentData == null) return;

            var activeHobbies = character.talentData.GetActiveHobbies();

            if (activeHobbies.Count > 0)
            {
                // Her aktif hobi için mutluluk bonusu
                int happinessBonus = Mathf.Min(activeHobbies.Count * 2, 10);
                character.Stats.ModifyStat(StatType.Happiness, happinessBonus);

                // Fiziksel hobiler için sağlık bonusu
                int physicalHobbies = activeHobbies.FindAll(h => IsPhysicalHobby(h.type)).Count;
                if (physicalHobbies > 0)
                {
                    int healthBonus = Mathf.Min(physicalHobbies * 3, 10);
                    character.Stats.ModifyStat(StatType.Health, healthBonus);
                }
            }
        }

        private bool IsPhysicalHobby(HobbyType type)
        {
            return type == HobbyType.Football ||
                   type == HobbyType.Basketball ||
                   type == HobbyType.Swimming ||
                   type == HobbyType.Running ||
                   type == HobbyType.Gym ||
                   type == HobbyType.MartialArts ||
                   type == HobbyType.Dancing ||
                   type == HobbyType.Hiking ||
                   type == HobbyType.Cycling ||
                   type == HobbyType.Yoga;
        }

        #endregion

        #region Career Integration

        /// <summary>
        /// Yetenek bazlı kariyer fırsatlarını kontrol et.
        /// </summary>
        public List<string> CheckCareerOpportunities(CharacterData character)
        {
            var opportunities = new List<string>();

            if (character?.talentData == null) return opportunities;

            foreach (var talent in character.talentData.talents)
            {
                if (talent.skillLevel >= 70)
                {
                    var info = TalentDefinitions.GetTalentInfo(talent.type);
                    string opportunity = GetCareerOpportunity(talent.type, talent.currentRank);
                    if (!string.IsNullOrEmpty(opportunity))
                    {
                        opportunities.Add(opportunity);
                    }
                }
            }

            return opportunities;
        }

        private string GetCareerOpportunity(TalentType type, TalentRank rank)
        {
            if (rank < TalentRank.Advanced) return null;

            return type switch
            {
                TalentType.Singing => rank >= TalentRank.Expert ? "Profesyonel şarkıcı olabilirsiniz" : "Düğünlerde şarkı söyleyebilirsiniz",
                TalentType.Guitar => rank >= TalentRank.Expert ? "Müzik grubuna katılabilirsiniz" : "Müzik dersleri verebilirsiniz",
                TalentType.Piano => rank >= TalentRank.Expert ? "Konser piyanisti olabilirsiniz" : "Piyano dersleri verebilirsiniz",
                TalentType.Football => rank >= TalentRank.Expert ? "Profesyonel futbolcu olabilirsiniz" : "Amatör ligde oynayabilirsiniz",
                TalentType.Basketball => rank >= TalentRank.Expert ? "Profesyonel basketbolcu olabilirsiniz" : "Antrenörlük yapabilirsiniz",
                TalentType.Painting => rank >= TalentRank.Expert ? "Sanat galerisi açabilirsiniz" : "Tablolarınızı satabilirsiniz",
                TalentType.Writing => rank >= TalentRank.Expert ? "Kitap yayınlayabilirsiniz" : "Freelance yazarlık yapabilirsiniz",
                TalentType.Coding => rank >= TalentRank.Expert ? "Yazılım şirketi kurabilirsiniz" : "Freelance yazılım geliştirebilirsiniz",
                TalentType.Acting => rank >= TalentRank.Expert ? "Film/dizi teklifleri alabilirsiniz" : "Tiyatroda rol alabilirsiniz",
                TalentType.Cooking => rank >= TalentRank.Expert ? "Restoran açabilirsiniz" : "Yemek kursları verebilirsiniz",
                TalentType.Photography => rank >= TalentRank.Expert ? "Profesyonel fotoğrafçı olabilirsiniz" : "Freelance çalışabilirsiniz",
                _ => null
            };
        }

        #endregion

        #region Special Events

        /// <summary>
        /// Yetenek yarışmasına katıl.
        /// </summary>
        public TalentCompetitionResult EnterCompetition(CharacterData character, TalentType talentType)
        {
            if (character?.talentData == null)
            {
                return new TalentCompetitionResult { Placement = 0 };
            }

            var talent = character.talentData.GetTalent(talentType);
            if (talent == null)
            {
                return new TalentCompetitionResult { Placement = 0 };
            }

            // Yarışma puanı hesapla
            float score = talent.skillLevel;
            score += Random.Range(-10f, 10f); // Şans faktörü
            score += character.Stats.Fame * 0.1f; // Fame avantajı

            // Sıralama belirle
            int placement;
            decimal prize = 0;
            int fameGain = 0;

            if (score >= 85)
            {
                placement = 1;
                prize = 10000;
                fameGain = 5;
                talent.achievements.Add($"1. - {TalentDefinitions.GetTalentInfo(talentType).Name} Yarışması");
            }
            else if (score >= 70)
            {
                placement = 2;
                prize = 5000;
                fameGain = 3;
            }
            else if (score >= 55)
            {
                placement = 3;
                prize = 2000;
                fameGain = 1;
            }
            else
            {
                placement = Random.Range(4, 20);
            }

            // Ödülleri ver
            if (prize > 0)
            {
                character.Finances.ModifyMoney(prize, $"{TalentDefinitions.GetTalentInfo(talentType).Name} yarışması ödülü");
            }

            if (fameGain > 0)
            {
                character.Stats.ModifyStat(StatType.Fame, fameGain);
            }

            // Mutluluk
            int happiness = placement <= 3 ? 10 : (placement <= 10 ? 2 : -2);
            character.Stats.ModifyStat(StatType.Happiness, happiness);

            return new TalentCompetitionResult
            {
                TalentType = talentType,
                Placement = placement,
                Prize = prize,
                FameGain = fameGain
            };
        }

        #endregion
    }

    /// <summary>
    /// Yetenek yarışması sonucu.
    /// </summary>
    public struct TalentCompetitionResult
    {
        public TalentType TalentType;
        public int Placement;
        public decimal Prize;
        public int FameGain;
    }
}
