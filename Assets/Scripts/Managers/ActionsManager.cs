using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Aktivite Yöneticisi - Oyuncunun yapabileceği tüm aksiyonları yönetir.
    /// BitLife benzeri aktivite sistemi.
    /// </summary>
    public class ActionsManager : Singleton<ActionsManager>
    {
        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[ActionsManager] Initialized successfully.");
        }

        #endregion

        #region Education Actions

        /// <summary>
        /// Ders çalış - Zeka artırır.
        /// </summary>
        public ActionResult Study()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            int intelligenceGain = Random.Range(1, 4);
            int happinessChange = Random.Range(-3, 2);

            character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);
            character.Stats.ModifyStat(StatType.Happiness, happinessChange);

            string[] messages = {
                $"Ders çalıştın ve zeka {intelligenceGain} arttı!",
                $"Kitaplarına gömüldün. Zeka +{intelligenceGain}",
                $"Sıkı bir çalışma seansı geçirdin. Zeka +{intelligenceGain}"
            };

            return ActionResult.Success(messages[Random.Range(0, messages.Length)]);
        }

        /// <summary>
        /// Kütüphaneye git - Zeka artırır.
        /// </summary>
        public ActionResult GoToLibrary()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            int intelligenceGain = Random.Range(2, 5);
            character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);

            return ActionResult.Success($"Kütüphanede vakit geçirdin. Zeka +{intelligenceGain}");
        }

        /// <summary>
        /// Okula başla.
        /// </summary>
        public ActionResult StartSchool()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Age < 6)
                return ActionResult.Failed("Okula başlamak için çok küçüksün!");

            if (character.Education.currentLevel >= EducationLevel.PrimarySchool)
                return ActionResult.Failed("Zaten okula gidiyorsun!");

            character.Education.currentLevel = EducationLevel.PrimarySchool;
            character.Education.schoolName = "İlkokul";
            character.Education.gpa = 0;

            return ActionResult.Success("İlkokula başladın! Eğitim hayatın başlıyor.");
        }

        /// <summary>
        /// Okulu terk et.
        /// </summary>
        public ActionResult DropOut()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Education.currentLevel == EducationLevel.None)
                return ActionResult.Failed("Zaten okula gitmiyorsun!");

            string schoolName = character.Education.schoolName;
            character.Education.currentLevel = EducationLevel.None;
            character.Education.schoolName = "";

            return ActionResult.Success($"{schoolName} okulunu bıraktın.");
        }

        /// <summary>
        /// Üniversite sınavına gir (YKS).
        /// </summary>
        public ActionResult TakeYKS()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Age < 17)
                return ActionResult.Failed("YKS'ye girmek için çok gençsin!");

            if (character.Education.currentLevel < EducationLevel.HighSchool)
                return ActionResult.Failed("Önce liseyi bitirmelisin!");

            // Puan hesapla - zekaya ve şansa bağlı
            int baseScore = character.Stats.Intelligence * 4;
            int luck = Random.Range(-50, 51);
            int finalScore = Mathf.Clamp(baseScore + luck, 100, 500);

            character.Education.yksScore = finalScore;

            string result;
            if (finalScore >= 400)
                result = $"Mükemmel! YKS puanın: {finalScore}. En iyi üniversitelere başvurabilirsin!";
            else if (finalScore >= 300)
                result = $"İyi bir puan aldın: {finalScore}. Birçok üniversiteye başvurabilirsin.";
            else if (finalScore >= 200)
                result = $"Ortalama bir puan: {finalScore}. Bazı üniversiteler seni kabul edebilir.";
            else
                result = $"Puan düşük: {finalScore}. Gelecek yıl tekrar deneyebilirsin.";

            return ActionResult.Success(result);
        }

        /// <summary>
        /// Üniversiteye başvur.
        /// </summary>
        public ActionResult ApplyToUniversity(string universityId)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            var university = DataManager.Instance?.GetUniversityById(universityId);
            if (university == null)
                return ActionResult.Failed("Üniversite bulunamadı!");

            if (character.Education.yksScore < university.minScore)
                return ActionResult.Failed($"Puanın yetersiz! Gereken: {university.minScore}, Senin puanın: {character.Education.yksScore}");

            // Kabul şansı
            float acceptanceChance = Mathf.Clamp01((character.Education.yksScore - university.minScore) / 100f + 0.5f);

            if (Random.value < acceptanceChance)
            {
                character.Education.currentLevel = EducationLevel.University;
                character.Education.universityName = university.name;
                character.Education.department = university.departments[Random.Range(0, university.departments.Length)];

                return ActionResult.Success($"Tebrikler! {university.name} üniversitesine {character.Education.department} bölümüne kabul edildin!");
            }
            else
            {
                return ActionResult.Failed($"{university.name} başvurun reddedildi. Başka üniversitelere dene.");
            }
        }

        /// <summary>
        /// Üniversiteden mezun ol.
        /// </summary>
        public ActionResult GraduateUniversity()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Education.currentLevel != EducationLevel.University)
                return ActionResult.Failed("Üniversitede okumuyorsun!");

            character.Education.isGraduated = true;
            character.Stats.ModifyStat(StatType.Intelligence, 10);

            return ActionResult.Success($"{character.Education.universityName} - {character.Education.department} bölümünden mezun oldun!");
        }

        #endregion

        #region Career Actions

        /// <summary>
        /// İş ara.
        /// </summary>
        public ActionResult SearchForJob()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Age < 16)
                return ActionResult.Failed("Çalışmak için çok gençsin!");

            // Rastgele iş teklifleri oluştur
            var availableJobs = DataManager.Instance?.GetAllJobs();
            if (availableJobs == null || availableJobs.Count == 0)
                return ActionResult.Failed("Şu an iş ilanı bulunamadı.");

            // 1-3 iş teklifi
            int jobCount = Random.Range(1, 4);
            var offers = availableJobs.OrderBy(x => Random.value).Take(jobCount).ToList();

            string jobList = string.Join("\n", offers.Select(j => $"- {j.title} ({j.baseSalary:N0} TL)"));

            return ActionResult.Success($"Bulunan iş ilanları:\n{jobList}\n\nBaşvurmak için 'İşe Başvur' seçeneğini kullan.");
        }

        /// <summary>
        /// İşe başvur.
        /// </summary>
        public ActionResult ApplyForJob(string jobId)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            var job = DataManager.Instance?.GetJobById(jobId);
            if (job == null)
                return ActionResult.Failed("İş bulunamadı!");

            // Kabul şansı - zeka, görünüş ve eğitime bağlı
            float baseChance = 0.3f;
            baseChance += character.Stats.Intelligence * 0.003f;
            baseChance += character.Stats.Appearance * 0.002f;
            baseChance += (int)character.Education.currentLevel * 0.1f;

            if (Random.value < baseChance)
            {
                // Eski işi kaydet
                if (character.Career.currentJob != null)
                {
                    character.Career.jobHistory.Add(character.Career.currentJob);
                }

                // Yeni iş ata
                character.Career.currentJob = new Job
                {
                    id = job.id,
                    title = job.title,
                    company = $"{job.category} Şirketi",
                    category = job.category,
                    baseSalary = job.baseSalary,
                    yearsWorked = 0
                };
                character.isEmployed = true;
                character.Career.yearsInJob = 0;
                character.Career.performanceRating = 50;

                return ActionResult.Success($"Tebrikler! {job.title} olarak işe alındın. Maaşın: {job.baseSalary:N0} TL");
            }
            else
            {
                return ActionResult.Failed($"{job.title} başvurun reddedildi. Deneyim veya eğitim yetersiz olabilir.");
            }
        }

        /// <summary>
        /// İşten ayrıl.
        /// </summary>
        public ActionResult QuitJob()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Career.currentJob == null)
                return ActionResult.Failed("Zaten çalışmıyorsun!");

            string jobTitle = character.Career.currentJob.title;
            character.Career.jobHistory.Add(character.Career.currentJob);
            character.Career.currentJob = null;
            character.isEmployed = false;

            return ActionResult.Success($"{jobTitle} işinden ayrıldın.");
        }

        /// <summary>
        /// Terfi iste.
        /// </summary>
        public ActionResult AskForPromotion()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Career.currentJob == null)
                return ActionResult.Failed("Çalışmıyorsun!");

            if (character.Career.yearsInJob < 1)
                return ActionResult.Failed("Terfi istemek için en az 1 yıl çalışmalısın!");

            // Terfi şansı
            float chance = 0.2f + (character.Career.performanceRating * 0.005f) + (character.Career.yearsInJob * 0.05f);

            if (Random.value < chance)
            {
                decimal raise = character.Career.currentJob.baseSalary * 0.15m;
                character.Career.currentJob.baseSalary += raise;
                character.Career.performanceRating = Mathf.Min(character.Career.performanceRating + 10, 100);
                character.Stats.ModifyStat(StatType.Happiness, 10);

                return ActionResult.Success($"Terfi aldın! Yeni maaşın: {character.Career.currentJob.baseSalary:N0} TL");
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -5);
                return ActionResult.Failed("Terfi isteğin reddedildi. Daha çok çalışmalısın.");
            }
        }

        /// <summary>
        /// Sıkı çalış - Performansı artırır.
        /// </summary>
        public ActionResult WorkHard()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Career.currentJob == null)
                return ActionResult.Failed("Çalışmıyorsun!");

            int performanceGain = Random.Range(3, 8);
            character.Career.performanceRating = Mathf.Min(character.Career.performanceRating + performanceGain, 100);

            int happinessLoss = Random.Range(1, 4);
            character.Stats.ModifyStat(StatType.Happiness, -happinessLoss);

            return ActionResult.Success($"Fazla mesai yaptın. Performans +{performanceGain}, Mutluluk -{happinessLoss}");
        }

        #endregion

        #region Social Actions

        /// <summary>
        /// Arkadaş edin.
        /// </summary>
        public ActionResult MakeFriend()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            // Yeni arkadaş oluştur
            string friendName = CharacterFactory.GenerateRandomName(Random.value > 0.5f ? Gender.Male : Gender.Female);
            int friendAge = character.Age + Random.Range(-5, 6);
            friendAge = Mathf.Max(friendAge, 5);

            var friend = new Relationship
            {
                npcId = System.Guid.NewGuid().ToString(),
                npcName = friendName,
                type = RelationType.Friend,
                intimacy = Random.Range(40, 70),
                trust = Random.Range(40, 70),
                status = RelationshipStatus.Active,
                age = friendAge,
                gender = Random.value > 0.5f ? Gender.Male : Gender.Female
            };

            character.Relationships.Add(friend);
            character.Stats.ModifyStat(StatType.Happiness, 5);

            return ActionResult.Success($"{friendName} ile arkadaş oldun!");
        }

        /// <summary>
        /// Aile ile vakit geçir.
        /// </summary>
        public ActionResult SpendTimeWithFamily()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            var family = character.Relationships.Where(r =>
                r.type == RelationType.Parent ||
                r.type == RelationType.Sibling ||
                r.type == RelationType.Child ||
                r.type == RelationType.Spouse).ToList();

            if (family.Count == 0)
                return ActionResult.Failed("Ailen yok!");

            foreach (var member in family)
            {
                member.intimacy = Mathf.Min(member.intimacy + Random.Range(2, 6), 100);
            }

            character.Stats.ModifyStat(StatType.Happiness, Random.Range(3, 8));

            return ActionResult.Success("Ailenle güzel vakit geçirdin. İlişkiler güçlendi!");
        }

        /// <summary>
        /// Sevgili bul.
        /// </summary>
        public ActionResult FindLove()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Age < 14)
                return ActionResult.Failed("Bunun için çok gençsin!");

            if (character.IsMarried)
                return ActionResult.Failed("Zaten evlisin!");

            // Mevcut sevgili var mı kontrol et
            var currentLove = character.Relationships.FirstOrDefault(r =>
                r.type == RelationType.Boyfriend || r.type == RelationType.Girlfriend);

            if (currentLove != null)
                return ActionResult.Failed($"Zaten {currentLove.npcName} ile birliktesin!");

            // Başarı şansı - görünüşe bağlı
            float chance = 0.3f + (character.Stats.Appearance * 0.005f);

            if (Random.value < chance)
            {
                Gender partnerGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;
                string partnerName = CharacterFactory.GenerateRandomName(partnerGender);
                int partnerAge = character.Age + Random.Range(-3, 4);

                var partner = new Relationship
                {
                    npcId = System.Guid.NewGuid().ToString(),
                    npcName = partnerName,
                    type = partnerGender == Gender.Male ? RelationType.Boyfriend : RelationType.Girlfriend,
                    intimacy = Random.Range(50, 80),
                    trust = Random.Range(50, 80),
                    status = RelationshipStatus.Active,
                    age = partnerAge,
                    gender = partnerGender
                };

                character.Relationships.Add(partner);
                character.Stats.ModifyStat(StatType.Happiness, 15);

                return ActionResult.Success($"{partnerName} ile tanıştın ve birlikte olmaya başladınız!");
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -3);
                return ActionResult.Failed("Kimseyle tanışamadın. Belki başka zaman!");
            }
        }

        /// <summary>
        /// Evlenme teklifi yap.
        /// </summary>
        public ActionResult Propose()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Age < 18)
                return ActionResult.Failed("Evlenmek için 18 yaşını doldurmalısın!");

            if (character.IsMarried)
                return ActionResult.Failed("Zaten evlisin!");

            var partner = character.Relationships.FirstOrDefault(r =>
                r.type == RelationType.Boyfriend || r.type == RelationType.Girlfriend);

            if (partner == null)
                return ActionResult.Failed("Önce bir sevgili bulmalısın!");

            // Kabul şansı - ilişki yakınlığına bağlı
            float chance = partner.intimacy / 100f;

            if (Random.value < chance)
            {
                partner.type = RelationType.Spouse;
                character.isMarried = true;
                character.Stats.ModifyStat(StatType.Happiness, 25);

                // Düğün masrafı
                decimal weddingCost = Random.Range(5000, 50000);
                character.Finances.ModifyMoney(-weddingCost, "Düğün masrafları");

                return ActionResult.Success($"{partner.npcName} evlilik teklifini kabul etti! Tebrikler!");
            }
            else
            {
                partner.intimacy = Mathf.Max(partner.intimacy - 20, 0);
                character.Stats.ModifyStat(StatType.Happiness, -15);
                return ActionResult.Failed($"{partner.npcName} evlilik teklifini reddetti!");
            }
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public ActionResult Divorce()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (!character.IsMarried)
                return ActionResult.Failed("Evli değilsin!");

            var spouse = character.Relationships.FirstOrDefault(r => r.type == RelationType.Spouse);
            if (spouse == null)
                return ActionResult.Failed("Eş bulunamadı!");

            spouse.type = RelationType.ExSpouse;
            spouse.status = RelationshipStatus.Broken;
            character.isMarried = false;

            // Boşanma maliyeti
            decimal divorceCost = character.Finances.CurrentMoney * 0.3m;
            character.Finances.ModifyMoney(-divorceCost, "Boşanma masrafları");

            character.Stats.ModifyStat(StatType.Happiness, -20);

            return ActionResult.Success($"{spouse.npcName} ile boşandın. Mal paylaşımı: -{divorceCost:N0} TL");
        }

        /// <summary>
        /// Çocuk yap.
        /// </summary>
        public ActionResult HaveChild()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Age < 18)
                return ActionResult.Failed("Çocuk sahibi olmak için 18 yaşını doldurmalısın!");

            var partner = character.Relationships.FirstOrDefault(r =>
                r.type == RelationType.Spouse ||
                r.type == RelationType.Boyfriend ||
                r.type == RelationType.Girlfriend);

            if (partner == null)
                return ActionResult.Failed("Bir partneriniz olmalı!");

            // Şans hesapla
            float chance = 0.5f;
            if (character.Age > 35) chance -= 0.1f;
            if (character.Age > 40) chance -= 0.2f;

            if (Random.value < chance)
            {
                Gender childGender = Random.value > 0.5f ? Gender.Male : Gender.Female;
                string childName = CharacterFactory.GenerateRandomName(childGender);

                var child = new Relationship
                {
                    npcId = System.Guid.NewGuid().ToString(),
                    npcName = childName,
                    type = RelationType.Child,
                    intimacy = 100,
                    trust = 100,
                    status = RelationshipStatus.Active,
                    age = 0,
                    gender = childGender
                };

                character.Relationships.Add(child);
                character.Stats.ModifyStat(StatType.Happiness, 20);

                string genderText = childGender == Gender.Male ? "oğlunuz" : "kızınız";
                return ActionResult.Success($"Tebrikler! Bir {genderText} oldu: {childName}!");
            }
            else
            {
                return ActionResult.Failed("Şu an çocuk sahibi olamadınız. Tekrar deneyebilirsiniz.");
            }
        }

        #endregion

        #region Health Actions

        /// <summary>
        /// Doktora git.
        /// </summary>
        public ActionResult VisitDoctor()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            decimal cost = Random.Range(100, 500);
            if (character.Finances.CurrentMoney < cost)
                return ActionResult.Failed($"Yeterli paran yok! Gereken: {cost:N0} TL");

            character.Finances.ModifyMoney(-cost, "Doktor ücreti");

            int healthGain = Random.Range(5, 15);
            character.Stats.ModifyStat(StatType.Health, healthGain);

            return ActionResult.Success($"Doktora gittin. Sağlık +{healthGain}. Ücret: {cost:N0} TL");
        }

        /// <summary>
        /// Spor salonu - Sağlık ve görünüş artırır.
        /// </summary>
        public ActionResult GoToGym()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            decimal cost = 50;
            if (character.Finances.CurrentMoney < cost)
                return ActionResult.Failed($"Yeterli paran yok! Gereken: {cost:N0} TL");

            character.Finances.ModifyMoney(-cost, "Spor salonu");

            int healthGain = Random.Range(1, 4);
            int appearanceGain = Random.Range(1, 3);

            character.Stats.ModifyStat(StatType.Health, healthGain);
            character.Stats.ModifyStat(StatType.Appearance, appearanceGain);

            return ActionResult.Success($"Spor yaptın! Sağlık +{healthGain}, Görünüş +{appearanceGain}");
        }

        /// <summary>
        /// Meditasyon yap.
        /// </summary>
        public ActionResult Meditate()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            int happinessGain = Random.Range(3, 8);
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            return ActionResult.Success($"Meditasyon yaptın ve huzur buldun. Mutluluk +{happinessGain}");
        }

        /// <summary>
        /// Yürüyüş yap.
        /// </summary>
        public ActionResult TakeWalk()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            int healthGain = Random.Range(1, 3);
            int happinessGain = Random.Range(1, 4);

            character.Stats.ModifyStat(StatType.Health, healthGain);
            character.Stats.ModifyStat(StatType.Happiness, happinessGain);

            return ActionResult.Success($"Güzel bir yürüyüş yaptın. Sağlık +{healthGain}, Mutluluk +{happinessGain}");
        }

        #endregion

        #region Crime Actions

        /// <summary>
        /// Hırsızlık yap.
        /// </summary>
        public ActionResult CommitTheft()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            // Yakalanma şansı
            float catchChance = 0.4f;

            if (Random.value < catchChance)
            {
                character.Stats.ModifyStat(StatType.Happiness, -15);
                // TODO: Hapis sistemi eklendiğinde hapse at
                return ActionResult.Failed("Yakalandın! Polisler seni tutukladı.");
            }
            else
            {
                decimal stolen = Random.Range(100, 2000);
                character.Finances.ModifyMoney(stolen, "Hırsızlık");
                character.Stats.ModifyStat(StatType.Happiness, 5);

                return ActionResult.Success($"Başarılı bir hırsızlık! Kazanç: {stolen:N0} TL");
            }
        }

        /// <summary>
        /// Kumar oyna.
        /// </summary>
        public ActionResult Gamble(decimal amount)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Finances.CurrentMoney < amount)
                return ActionResult.Failed("Yeterli paran yok!");

            // %45 kazanma şansı
            if (Random.value < 0.45f)
            {
                decimal winnings = amount * 2;
                character.Finances.ModifyMoney(winnings, "Kumar kazancı");
                character.Stats.ModifyStat(StatType.Happiness, 10);
                return ActionResult.Success($"Kazandın! Toplam kazanç: {winnings:N0} TL");
            }
            else
            {
                character.Finances.ModifyMoney(-amount, "Kumar kaybı");
                character.Stats.ModifyStat(StatType.Happiness, -5);
                return ActionResult.Failed($"Kaybettin! Kayıp: {amount:N0} TL");
            }
        }

        #endregion

        #region Asset Actions

        /// <summary>
        /// Ev satın al.
        /// </summary>
        public ActionResult BuyHouse()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            decimal[] housePrices = { 500000, 750000, 1000000, 2000000, 5000000 };
            string[] houseTypes = { "Stüdyo Daire", "2+1 Daire", "3+1 Daire", "Villa", "Lüks Villa" };

            // En uygun evi bul
            for (int i = 0; i < housePrices.Length; i++)
            {
                if (character.Finances.CurrentMoney >= housePrices[i])
                {
                    character.Finances.ModifyMoney(-housePrices[i], $"{houseTypes[i]} satın alma");
                    character.Finances.assets.Add(houseTypes[i]);
                    character.Stats.ModifyStat(StatType.Happiness, 15);

                    return ActionResult.Success($"{houseTypes[i]} satın aldın! Fiyat: {housePrices[i]:N0} TL");
                }
            }

            return ActionResult.Failed("Ev almak için yeterli paran yok!");
        }

        /// <summary>
        /// Araba satın al.
        /// </summary>
        public ActionResult BuyCar()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            decimal[] carPrices = { 100000, 250000, 500000, 1000000, 3000000 };
            string[] carTypes = { "Ekonomik Araba", "Sedan", "SUV", "Spor Araba", "Lüks Araba" };

            for (int i = 0; i < carPrices.Length; i++)
            {
                if (character.Finances.CurrentMoney >= carPrices[i])
                {
                    character.Finances.ModifyMoney(-carPrices[i], $"{carTypes[i]} satın alma");
                    character.Finances.assets.Add(carTypes[i]);
                    character.Stats.ModifyStat(StatType.Happiness, 10);
                    character.Stats.ModifyStat(StatType.Fame, 2);

                    return ActionResult.Success($"{carTypes[i]} satın aldın! Fiyat: {carPrices[i]:N0} TL");
                }
            }

            return ActionResult.Failed("Araba almak için yeterli paran yok!");
        }

        #endregion

        #region Migration Actions

        /// <summary>
        /// Şehir değiştir.
        /// </summary>
        public ActionResult MoveToCity(string cityId)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            var city = DataManager.Instance?.GetCityById(cityId);
            if (city == null)
                return ActionResult.Failed("Şehir bulunamadı!");

            decimal movingCost = Random.Range(5000, 20000);
            if (character.Finances.CurrentMoney < movingCost)
                return ActionResult.Failed($"Taşınmak için yeterli paran yok! Gereken: {movingCost:N0} TL");

            character.Finances.ModifyMoney(-movingCost, "Taşınma masrafları");
            character.birthCity = city.name;

            // İşi kaybedebilir
            if (character.Career.currentJob != null && Random.value < 0.5f)
            {
                character.Career.jobHistory.Add(character.Career.currentJob);
                character.Career.currentJob = null;
                character.isEmployed = false;
            }

            return ActionResult.Success($"{city.name} şehrine taşındın! Maliyet: {movingCost:N0} TL");
        }

        #endregion

        #region Military Actions

        /// <summary>
        /// Askerlik yap.
        /// </summary>
        public ActionResult DoMilitaryService()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Gender == Gender.Female)
                return ActionResult.Failed("Kadınlar için askerlik zorunlu değil!");

            if (character.Age < 20)
                return ActionResult.Failed("Askerlik yaşı gelmedi!");

            if (character.hasCompletedMilitary)
                return ActionResult.Failed("Askerliğini zaten yaptın!");

            character.hasCompletedMilitary = true;
            character.Stats.ModifyStat(StatType.Health, 5);
            character.Stats.ModifyStat(StatType.Happiness, -10);

            // İş kaybı
            if (character.Career.currentJob != null)
            {
                character.Career.jobHistory.Add(character.Career.currentJob);
                character.Career.currentJob = null;
                character.isEmployed = false;
            }

            return ActionResult.Success("Askerlik görevini tamamladın! Artık askerliğini yapmış bir vatandaşsın.");
        }

        #endregion

        #region Fame Actions

        /// <summary>
        /// Sosyal medyada paylaşım yap.
        /// </summary>
        public ActionResult PostOnSocialMedia()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return ActionResult.Failed("Karakter bulunamadı!");

            if (character.Age < 13)
                return ActionResult.Failed("Sosyal medya için çok küçüksün!");

            // Viral olma şansı
            if (Random.value < 0.1f)
            {
                int fameGain = Random.Range(5, 15);
                character.Stats.ModifyStat(StatType.Fame, fameGain);
                character.Stats.ModifyStat(StatType.Happiness, 10);
                return ActionResult.Success($"Paylaşımın viral oldu! Şöhret +{fameGain}");
            }
            else
            {
                int fameGain = Random.Range(0, 2);
                character.Stats.ModifyStat(StatType.Fame, fameGain);
                return ActionResult.Success($"Sosyal medyada paylaşım yaptın. Şöhret +{fameGain}");
            }
        }

        #endregion
    }

    /// <summary>
    /// Aksiyon sonucu.
    /// </summary>
    public class ActionResult
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        private ActionResult(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
        }

        public static ActionResult Success(string message)
        {
            return new ActionResult(true, message);
        }

        public static ActionResult Failed(string message)
        {
            return new ActionResult(false, message);
        }
    }
}
