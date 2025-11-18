using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Activities
{
    /// <summary>
    /// Aktivite Yöneticisi - Oyuncunun seçebileceği tüm aktiviteleri yönetir.
    /// </summary>
    public class ActivityManager : Singleton<ActivityManager>
    {
        #region Mind & Body Activities

        /// <summary>
        /// Spor salonu.
        /// </summary>
        public void GoToGym(CharacterData character)
        {
            if (character.Age < 14) return;

            decimal cost = 100;
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Spor salonu");
            character.Stats.ModifyStat(StatType.Health, UnityEngine.Random.Range(3, 8));
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(1, 5));

            if (UnityEngine.Random.value < 0.3f)
            {
                character.Stats.ModifyStat(StatType.Appearance, 1);
            }

            // Sakatlanma riski
            if (UnityEngine.Random.value < 0.05f)
            {
                character.Stats.ModifyStat(StatType.Health, -15);
                ShowResult("Sporda sakatlandın!");
            }
            else
            {
                ShowResult("Güzel bir antrenman yaptın.");
            }
        }

        /// <summary>
        /// Yürüyüş yap.
        /// </summary>
        public void GoForWalk(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Health, UnityEngine.Random.Range(1, 4));
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(1, 3));
            ShowResult("Güzel bir yürüyüş yaptın.");
        }

        /// <summary>
        /// Koşu yap.
        /// </summary>
        public void GoJogging(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Health, UnityEngine.Random.Range(3, 7));
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(1, 4));

            if (character.Age > 50 && UnityEngine.Random.value < 0.1f)
            {
                character.Stats.ModifyStat(StatType.Health, -10);
                ShowResult("Koşu sırasında dizlerin ağrıdı.");
            }
            else
            {
                ShowResult("Koşu yaptın ve enerjik hissediyorsun.");
            }
        }

        /// <summary>
        /// Yoga yap.
        /// </summary>
        public void DoYoga(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Health, UnityEngine.Random.Range(2, 5));
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(3, 7));
            character.Health.mentalHealth = Mathf.Clamp(character.Health.mentalHealth + 5, 0, 100);
            ShowResult("Yoga seansı bitirdin. Huzurlu hissediyorsun.");
        }

        /// <summary>
        /// Meditasyon yap.
        /// </summary>
        public void Meditate(CharacterData character)
        {
            character.Health.mentalHealth = Mathf.Clamp(character.Health.mentalHealth + 10, 0, 100);
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(5, 10));
            ShowResult("Zihinsel olarak rahatladın.");
        }

        /// <summary>
        /// Kütüphaneye git.
        /// </summary>
        public void GoToLibrary(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Intelligence, UnityEngine.Random.Range(1, 4));
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(1, 3));
            ShowResult("Kütüphanede faydalı vakit geçirdin.");
        }

        /// <summary>
        /// Kitap oku.
        /// </summary>
        public void ReadBook(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Intelligence, UnityEngine.Random.Range(1, 3));
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(1, 4));
            ShowResult("Güzel bir kitap okudun.");
        }

        /// <summary>
        /// Film izle.
        /// </summary>
        public void WatchMovie(CharacterData character)
        {
            decimal cost = UnityEngine.Random.Range(30, 80);
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Sinema");
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(3, 8));
            ShowResult("Güzel bir film izledin.");
        }

        /// <summary>
        /// Dövüş sanatları dersi al.
        /// </summary>
        public void TakeMartialArtsClass(CharacterData character)
        {
            decimal cost = 200;
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Dövüş sanatları dersi");
            character.Stats.ModifyStat(StatType.Health, UnityEngine.Random.Range(3, 6));

            // Hobi ilerlemesi
            var martialArts = character.Hobbies.Find(h => h.category == HobbyCategory.MartialArts);
            if (martialArts == null)
            {
                LifeSystemManager.Instance?.StartHobby(character, "Dövüş Sanatları", HobbyCategory.MartialArts);
            }

            ShowResult("Dövüş sanatları dersine katıldın.");
        }

        #endregion

        #region Social Activities

        /// <summary>
        /// Arkadaşlarla buluş.
        /// </summary>
        public void HangOutWithFriends(CharacterData character)
        {
            var friends = character.Relationships.FindAll(r =>
                r.type == RelationType.Friend || r.type == RelationType.BestFriend);

            if (friends.Count == 0)
            {
                ShowResult("Buluşacak arkadaşın yok.");
                return;
            }

            decimal cost = UnityEngine.Random.Range(50, 200);
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Arkadaşlarla buluşma");
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(5, 15));

            // Rastgele bir arkadaşla ilişki gelişir
            var randomFriend = friends[UnityEngine.Random.Range(0, friends.Count)];
            LifeSystemManager.Instance?.ImproveRelationship(character, randomFriend.npcId, 5);

            ShowResult($"{randomFriend.npcName} ile güzel vakit geçirdin.");
        }

        /// <summary>
        /// Partiye git.
        /// </summary>
        public void GoToParty(CharacterData character)
        {
            if (character.Age < 16)
            {
                ShowResult("Partiye gitmek için çok küçüksün.");
                return;
            }

            decimal cost = UnityEngine.Random.Range(100, 500);
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Parti");
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(5, 20));

            // Yeni tanışıklık şansı
            if (UnityEngine.Random.value < 0.4f)
            {
                LifeSystemManager.Instance?.MeetNewPerson(character, RelationType.Acquaintance);
                ShowResult("Partide biriyle tanıştın.");
            }
            else if (UnityEngine.Random.value < 0.1f)
            {
                // Polis baskını
                character.Stats.ModifyStat(StatType.Happiness, -10);
                ShowResult("Polisler partiyi bastı!");
            }
            else
            {
                ShowResult("Eğlenceli bir parti geçirdin.");
            }
        }

        /// <summary>
        /// Bara git.
        /// </summary>
        public void GoToBar(CharacterData character)
        {
            if (character.Age < 18)
            {
                ShowResult("Bara girmek için 18 yaşından büyük olmalısın.");
                return;
            }

            decimal cost = UnityEngine.Random.Range(100, 400);
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Bar");
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(3, 10));

            // Alkol etkisi
            character.Health.isDrinking = true;
            character.Stats.ModifyStat(StatType.Health, -2);

            // Kavga riski
            if (UnityEngine.Random.value < 0.1f)
            {
                character.Stats.ModifyStat(StatType.Health, -10);
                ShowResult("Barda kavgaya karıştın!");
            }
            else
            {
                ShowResult("Barda içki içtin.");
            }
        }

        /// <summary>
        /// Klübe git.
        /// </summary>
        public void GoToClub(CharacterData character)
        {
            if (character.Age < 18)
            {
                ShowResult("Gece kulübüne girmek için 18 yaşından büyük olmalısın.");
                return;
            }

            decimal cost = UnityEngine.Random.Range(200, 1000);
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Gece kulübü");
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(5, 15));

            // Romantik tanışma şansı
            if (UnityEngine.Random.value < 0.3f && !character.isMarried)
            {
                Gender preferredGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;
                var interest = CharacterFactory.CreateLoveInterest(character.Age, preferredGender);
                character.Relationships.Add(interest);
                ShowResult($"Klüpte {interest.npcName} ile tanıştın!");
            }
            else
            {
                ShowResult("Klüpte dans ettik.");
            }
        }

        /// <summary>
        /// Aileyi ziyaret et.
        /// </summary>
        public void VisitFamily(CharacterData character)
        {
            var family = character.Relationships.FindAll(r =>
                r.type == RelationType.Parent || r.type == RelationType.Sibling);

            if (family.Count == 0)
            {
                ShowResult("Ziyaret edecek aile üyen yok.");
                return;
            }

            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(3, 10));

            foreach (var member in family)
            {
                if (member.status == RelationshipStatus.Active)
                {
                    LifeSystemManager.Instance?.ImproveRelationship(character, member.npcId, 3);
                }
            }

            ShowResult("Aileyle güzel vakit geçirdin.");
        }

        /// <summary>
        /// Sevgili ile buluş.
        /// </summary>
        public void GoOnDate(CharacterData character)
        {
            var partner = character.Relationships.Find(r =>
                r.type == RelationType.Boyfriend ||
                r.type == RelationType.Girlfriend ||
                r.type == RelationType.Spouse);

            if (partner == null)
            {
                ShowResult("Çıkacak birin yok.");
                return;
            }

            decimal cost = UnityEngine.Random.Range(100, 500);
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Romantik buluşma");
            character.Stats.ModifyStat(StatType.Happiness, UnityEngine.Random.Range(8, 20));

            LifeSystemManager.Instance?.ImproveRelationship(character, partner.npcId, 10);

            ShowResult($"{partner.npcName} ile romantik bir akşam geçirdin.");
        }

        #endregion

        #region Education Activities

        /// <summary>
        /// Ders çalış.
        /// </summary>
        public void StudyHarder(CharacterData character)
        {
            if (character.Education.currentLevel == EducationLevel.None)
            {
                ShowResult("Okula gitmiyorsun.");
                return;
            }

            LifeSystemManager.Instance?.Study(character, 3);
            ShowResult("Sıkı ders çalıştın. Not ortalamaların yükseldi.");
        }

        /// <summary>
        /// Özel ders al.
        /// </summary>
        public void TakePrivateLessons(CharacterData character)
        {
            decimal cost = 500;
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Özel ders");
            character.Stats.ModifyStat(StatType.Intelligence, UnityEngine.Random.Range(2, 5));

            if (character.Education.currentLevel != EducationLevel.None)
            {
                character.Education.gpa = Mathf.Clamp(character.Education.gpa + 0.3f, 0, 4);
            }

            ShowResult("Özel ders aldın. Bilgin arttı.");
        }

        /// <summary>
        /// Dil kursu.
        /// </summary>
        public void TakeLanguageCourse(CharacterData character)
        {
            decimal cost = 1000;
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Dil kursu");
            character.Stats.ModifyStat(StatType.Intelligence, UnityEngine.Random.Range(1, 4));
            ShowResult("Dil kursuna katıldın. Yabancı dil becerin gelişti.");
        }

        /// <summary>
        /// Okulu bırak.
        /// </summary>
        public void DropOut(CharacterData character)
        {
            if (character.Education.currentLevel == EducationLevel.None)
            {
                ShowResult("Zaten okula gitmiyorsun.");
                return;
            }

            LifeSystemManager.Instance?.DropOutOfSchool(character, false);
            ShowResult("Okulu bıraktın.");
        }

        #endregion

        #region Career Activities

        /// <summary>
        /// İş ara.
        /// </summary>
        public void LookForJob(CharacterData character)
        {
            if (character.Age < 16)
            {
                ShowResult("İş bulmak için çok küçüksün.");
                return;
            }

            if (character.isEmployed)
            {
                ShowResult("Zaten bir işin var.");
                return;
            }

            // Mevcut işleri listele
            var jobs = GetAvailableJobs(character);
            if (jobs.Count == 0)
            {
                ShowResult("Uygun iş bulunamadı.");
                return;
            }

            var job = jobs[UnityEngine.Random.Range(0, jobs.Count)];
            if (LifeSystemManager.Instance.ApplyForJob(character, job))
            {
                ShowResult($"{job.company}'da {job.title} olarak işe alındın!");
            }
            else
            {
                ShowResult($"{job.title} başvurusu reddedildi.");
            }
        }

        /// <summary>
        /// Terfi iste.
        /// </summary>
        public void AskForPromotion(CharacterData character)
        {
            if (!character.isEmployed)
            {
                ShowResult("Bir işin yok.");
                return;
            }

            if (LifeSystemManager.Instance.GetPromotion(character))
            {
                ShowResult("Terfi aldın! Maaşın arttı.");
            }
            else
            {
                ShowResult("Terfi talebiniz reddedildi.");
                character.Stats.ModifyStat(StatType.Happiness, -5);
            }
        }

        /// <summary>
        /// Zam iste.
        /// </summary>
        public void AskForRaise(CharacterData character)
        {
            if (!character.isEmployed)
            {
                ShowResult("Bir işin yok.");
                return;
            }

            float raiseChance = character.Career.performanceRating / 150f;

            if (UnityEngine.Random.value < raiseChance)
            {
                decimal raise = character.Career.currentJob.baseSalary * 0.1m;
                character.Career.currentJob.baseSalary += raise;
                character.Stats.ModifyStat(StatType.Happiness, 10);
                ShowResult($"Zam aldın! Yıllık maaşın {raise:N0} TL arttı.");
            }
            else
            {
                ShowResult("Zam talebiniz reddedildi.");
                character.Stats.ModifyStat(StatType.Happiness, -5);
            }
        }

        /// <summary>
        /// İşten ayrıl.
        /// </summary>
        public void QuitJob(CharacterData character)
        {
            if (!character.isEmployed)
            {
                ShowResult("Bir işin yok.");
                return;
            }

            LifeSystemManager.Instance?.QuitJob(character);
            ShowResult("İşinden ayrıldın.");
        }

        /// <summary>
        /// Emekli ol.
        /// </summary>
        public void Retire(CharacterData character)
        {
            if (character.Age < 55)
            {
                ShowResult("Emekli olmak için çok gençsin.");
                return;
            }

            LifeSystemManager.Instance?.Retire(character);
            ShowResult("Emekli oldun! Huzurlu bir emeklilik dileriz.");
        }

        private List<Job> GetAvailableJobs(CharacterData character)
        {
            var jobs = new List<Job>();

            // Eğitim seviyesine göre işler
            if (character.Education.currentLevel >= EducationLevel.University)
            {
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Mühendis", company = "TechCorp", category = "Mühendislik", baseSalary = 120000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Avukat", company = "Hukuk Bürosu", category = "Hukuk", baseSalary = 150000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Doktor", company = "Devlet Hastanesi", category = "Sağlık", baseSalary = 180000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Öğretmen", company = "Devlet Okulu", category = "Eğitim", baseSalary = 80000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Bankacı", company = "Ulusal Banka", category = "Finans", baseSalary = 100000 });
            }
            else if (character.Education.currentLevel >= EducationLevel.HighSchool)
            {
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Satış Danışmanı", company = "Perakende Mağaza", category = "Satış", baseSalary = 50000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Resepsiyon", company = "Otel", category = "Hizmet", baseSalary = 45000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Teknisyen", company = "Servis", category = "Teknik", baseSalary = 55000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Sekreter", company = "Şirket", category = "Ofis", baseSalary = 48000 });
            }
            else
            {
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Garson", company = "Restoran", category = "Hizmet", baseSalary = 35000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Kurye", company = "Kargo", category = "Lojistik", baseSalary = 38000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "Temizlik", company = "Bina Yönetimi", category = "Hizmet", baseSalary = 32000 });
                jobs.Add(new Job { id = Guid.NewGuid().ToString(), title = "İnşaat İşçisi", company = "İnşaat Şirketi", category = "İnşaat", baseSalary = 40000 });
            }

            return jobs;
        }

        #endregion

        #region Crime Activities

        /// <summary>
        /// Hırsızlık yap.
        /// </summary>
        public void CommitTheft(CharacterData character)
        {
            if (character.Age < 14) return;

            if (LifeSystemManager.Instance.CommitCrime(character, CrimeType.Theft))
            {
                ShowResult("Hırsızlık başarılı! Yakalanmadın.");
            }
            else
            {
                ShowResult("Yakalandın ve tutuklandın!");
            }
        }

        /// <summary>
        /// Mağaza hırsızlığı.
        /// </summary>
        public void Shoplift(CharacterData character)
        {
            if (LifeSystemManager.Instance.CommitCrime(character, CrimeType.Shoplifting))
            {
                ShowResult("Mağazadan bir şey çaldın!");
            }
            else
            {
                ShowResult("Güvenlik tarafından yakalandın!");
            }
        }

        /// <summary>
        /// Gasp yap.
        /// </summary>
        public void CommitRobbery(CharacterData character)
        {
            if (character.Age < 16) return;

            if (LifeSystemManager.Instance.CommitCrime(character, CrimeType.Robbery))
            {
                ShowResult("Gasp başarılı! İyi para kazandın.");
            }
            else
            {
                ShowResult("Yakalandın! Ciddi ceza alacaksın.");
            }
        }

        /// <summary>
        /// Uyuşturucu sat.
        /// </summary>
        public void DealDrugs(CharacterData character)
        {
            if (character.Age < 16) return;

            if (LifeSystemManager.Instance.CommitCrime(character, CrimeType.DrugTrafficking))
            {
                ShowResult("Uyuşturucu sattın ve iyi para kazandın.");
            }
            else
            {
                ShowResult("Narkotik tarafından yakalandın!");
            }
        }

        /// <summary>
        /// Hapisten kaç.
        /// </summary>
        public void EscapeFromPrison(CharacterData character)
        {
            if (!character.isInPrison)
            {
                ShowResult("Hapiste değilsin.");
                return;
            }

            if (LifeSystemManager.Instance.AttemptPrisonEscape(character))
            {
                ShowResult("Hapisten kaçmayı başardın!");
            }
            else
            {
                ShowResult("Kaçış girişimi başarısız! Ceza süren uzatıldı.");
            }
        }

        #endregion

        #region Financial Activities

        /// <summary>
        /// Piyango al.
        /// </summary>
        public void BuyLotteryTicket(CharacterData character)
        {
            decimal winnings = LifeSystemManager.Instance.PlayLottery(character, 50);

            if (winnings > 0)
            {
                ShowResult($"Piyango kazandın! {winnings:N0} TL");
            }
            else
            {
                ShowResult("Piyangodan bir şey çıkmadı.");
            }
        }

        /// <summary>
        /// Kumarhaneye git.
        /// </summary>
        public void GoToCasino(CharacterData character)
        {
            if (character.Age < 18)
            {
                ShowResult("Kumarhaneye girmek için 18 yaşından büyük olmalısın.");
                return;
            }

            decimal betAmount = UnityEngine.Random.Range(100, 1000);
            if (character.Finances.CurrentMoney < betAmount)
            {
                ShowNotEnoughMoney();
                return;
            }

            decimal winnings = LifeSystemManager.Instance.Gamble(character, betAmount);

            if (winnings > 0)
            {
                ShowResult($"Kumar kazandın! {winnings:N0} TL");
            }
            else
            {
                ShowResult($"Kumar kaybettin. {betAmount:N0} TL gittin.");
            }
        }

        /// <summary>
        /// Bankaya para yatır.
        /// </summary>
        public void DepositMoney(CharacterData character, decimal amount)
        {
            // Basit simülasyon - para zaten finanslarda
            character.Stats.ModifyStat(StatType.Happiness, 1);
            ShowResult($"{amount:N0} TL bankaya yatırıldı.");
        }

        /// <summary>
        /// Kredi çek.
        /// </summary>
        public void TakeLoan(CharacterData character, decimal amount)
        {
            character.Finances.ModifyMoney(amount, "Banka kredisi");
            character.Finances.debts.Add($"Banka Kredisi: {amount:N0} TL");
            ShowResult($"{amount:N0} TL kredi çektin. Geri ödemeyi unutma!");
        }

        #endregion

        #region Health Activities

        /// <summary>
        /// Doktora git.
        /// </summary>
        public void VisitDoctor(CharacterData character)
        {
            LifeSystemManager.Instance?.VisitDoctor(character);
            ShowResult("Doktor muayenesi tamamlandı.");
        }

        /// <summary>
        /// Psikiyatriste git.
        /// </summary>
        public void VisitPsychiatrist(CharacterData character)
        {
            decimal cost = character.Health.hasInsurance ? 100 : 800;
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Psikiyatrist");
            character.Health.mentalHealth = Mathf.Clamp(character.Health.mentalHealth + 20, 0, 100);
            character.Stats.ModifyStat(StatType.Happiness, 10);
            ShowResult("Psikiyatrist seansı tamamlandı. Daha iyi hissediyorsun.");
        }

        /// <summary>
        /// Estetik ameliyat.
        /// </summary>
        public void GetPlasticSurgery(CharacterData character)
        {
            decimal cost = 50000;
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Estetik ameliyat");

            if (UnityEngine.Random.value < 0.9f)
            {
                character.Stats.ModifyStat(StatType.Appearance, UnityEngine.Random.Range(10, 25));
                ShowResult("Estetik ameliyat başarılı! Görünümün iyileşti.");
            }
            else
            {
                character.Stats.ModifyStat(StatType.Appearance, -10);
                character.Stats.ModifyStat(StatType.Health, -20);
                ShowResult("Estetik ameliyat başarısız oldu!");
            }
        }

        /// <summary>
        /// Diyete başla.
        /// </summary>
        public void StartDiet(CharacterData character)
        {
            character.Health.isOnDiet = true;
            ShowResult("Diyete başladın.");
        }

        /// <summary>
        /// Sigara bırak.
        /// </summary>
        public void QuitSmoking(CharacterData character)
        {
            if (!character.Health.isSmoking)
            {
                ShowResult("Zaten sigara içmiyorsun.");
                return;
            }

            if (UnityEngine.Random.value < 0.3f)
            {
                character.Health.isSmoking = false;
                character.Stats.ModifyStat(StatType.Health, 10);
                ShowResult("Sigarayı bırakmayı başardın!");
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, -10);
                ShowResult("Sigarayı bırakamadın.");
            }
        }

        #endregion

        #region Love & Dating

        /// <summary>
        /// Dating uygulaması kullan.
        /// </summary>
        public void UseDatingApp(CharacterData character)
        {
            if (character.Age < 18)
            {
                ShowResult("Dating uygulaması için 18 yaşından büyük olmalısın.");
                return;
            }

            if (character.isMarried)
            {
                ShowResult("Evlisin!");
                return;
            }

            if (UnityEngine.Random.value < 0.5f)
            {
                Gender preferredGender = character.Gender == Gender.Male ? Gender.Female : Gender.Male;
                var interest = CharacterFactory.CreateLoveInterest(character.Age, preferredGender);
                character.Relationships.Add(interest);
                ShowResult($"Dating uygulamasında {interest.npcName} ile eşleştin!");
            }
            else
            {
                ShowResult("Bugün kimseyle eşleşemedin.");
            }
        }

        /// <summary>
        /// Evlilik teklifi et.
        /// </summary>
        public void Propose(CharacterData character, string partnerId)
        {
            if (LifeSystemManager.Instance.ProposeMarriage(character, partnerId))
            {
                ShowResult("Evlilik teklifin kabul edildi!");
            }
            else
            {
                ShowResult("Evlilik teklifin reddedildi.");
            }
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public void FileDivorce(CharacterData character)
        {
            var spouse = character.Relationships.Find(r => r.type == RelationType.Spouse);
            if (spouse == null)
            {
                ShowResult("Evli değilsin.");
                return;
            }

            LifeSystemManager.Instance?.Divorce(character, spouse.npcId);
            ShowResult("Boşanma işlemleri tamamlandı.");
        }

        /// <summary>
        /// Çocuk yap.
        /// </summary>
        public void TryForBaby(CharacterData character)
        {
            if (!character.isMarried)
            {
                ShowResult("Evli değilsin.");
                return;
            }

            var child = LifeSystemManager.Instance.HaveChild(character);
            if (child != null)
            {
                ShowResult($"{child.npcName} adında bir bebeğin oldu!");
            }
            else
            {
                ShowResult("Hamilelik gerçekleşmedi.");
            }
        }

        #endregion

        #region Property

        /// <summary>
        /// Ev al.
        /// </summary>
        public void BuyHouse(CharacterData character)
        {
            if (character.Age < 18)
            {
                ShowResult("Ev almak için 18 yaşından büyük olmalısın.");
                return;
            }

            // Basit ev seçenekleri
            var property = new Property
            {
                id = Guid.NewGuid().ToString(),
                name = "Daire",
                type = PropertyType.Apartment,
                location = character.birthCity,
                purchasePrice = UnityEngine.Random.Range(500000, 2000000),
                currentValue = 0,
                condition = 100
            };
            property.currentValue = property.purchasePrice;

            if (LifeSystemManager.Instance.BuyProperty(character, property))
            {
                ShowResult($"Ev satın aldın! {property.purchasePrice:N0} TL");
            }
            else
            {
                ShowResult("Yeterli paran yok.");
            }
        }

        /// <summary>
        /// Araba al.
        /// </summary>
        public void BuyCar(CharacterData character)
        {
            if (!character.hasDrivingLicense)
            {
                ShowResult("Ehliyet olmadan araba alamazsın.");
                return;
            }

            var vehicle = new Vehicle
            {
                id = Guid.NewGuid().ToString(),
                brand = GetRandomCarBrand(),
                model = "Standart",
                year = DateTime.Now.Year,
                type = VehicleType.Car,
                purchasePrice = UnityEngine.Random.Range(200000, 1000000),
                condition = 100,
                mileage = 0
            };
            vehicle.currentValue = vehicle.purchasePrice;

            if (LifeSystemManager.Instance.BuyVehicle(character, vehicle))
            {
                ShowResult($"{vehicle.brand} {vehicle.model} satın aldın!");
            }
            else
            {
                ShowResult("Yeterli paran yok.");
            }
        }

        /// <summary>
        /// Ehliyet al.
        /// </summary>
        public void GetDrivingLicense(CharacterData character)
        {
            if (character.Age < 18)
            {
                ShowResult("Ehliyet almak için 18 yaşından büyük olmalısın.");
                return;
            }

            if (character.hasDrivingLicense)
            {
                ShowResult("Zaten ehliyetin var.");
                return;
            }

            decimal cost = 5000;
            if (character.Finances.CurrentMoney < cost)
            {
                ShowNotEnoughMoney();
                return;
            }

            character.Finances.ModifyMoney(-cost, "Sürücü kursu");

            if (UnityEngine.Random.value < 0.7f + character.Stats.Intelligence / 300f)
            {
                character.hasDrivingLicense = true;
                ShowResult("Ehliyet sınavını geçtin!");
            }
            else
            {
                ShowResult("Ehliyet sınavında kaldın.");
            }
        }

        private string GetRandomCarBrand()
        {
            string[] brands = { "Toyota", "Honda", "BMW", "Mercedes", "Audi", "Volkswagen", "Ford", "Renault", "Fiat", "Hyundai" };
            return brands[UnityEngine.Random.Range(0, brands.Length)];
        }

        #endregion

        #region Social Media

        /// <summary>
        /// Sosyal medya hesabı aç.
        /// </summary>
        public void OpenSocialMediaAccount(CharacterData character, string platform)
        {
            LifeSystemManager.Instance?.CreateSocialMediaAccount(character, platform);
            ShowResult($"{platform} hesabı açıldı!");
        }

        /// <summary>
        /// Paylaşım yap.
        /// </summary>
        public void PostOnSocialMedia(CharacterData character, string platform)
        {
            LifeSystemManager.Instance?.PostContent(character, platform);
            ShowResult("İçerik paylaştın. Takipçilerin artıyor!");
        }

        #endregion

        #region Military

        /// <summary>
        /// Askere git.
        /// </summary>
        public void JoinMilitary(CharacterData character)
        {
            LifeSystemManager.Instance?.StartMilitaryService(character);
            ShowResult("Askerlik hizmetine başladın.");
        }

        /// <summary>
        /// Bedelli askerlik yap.
        /// </summary>
        public void PayForMilitaryExemption(CharacterData character)
        {
            decimal cost = 100000; // Bedelli ücreti

            if (LifeSystemManager.Instance.PayForMilitaryExemption(character, cost))
            {
                ShowResult("Bedelli askerlik tamamlandı.");
            }
            else
            {
                ShowResult("Bedelli için yeterli paran yok.");
            }
        }

        #endregion

        #region Utility

        private void ShowResult(string message)
        {
            UIManager.Instance?.ShowEventResult(message, null);
        }

        private void ShowNotEnoughMoney()
        {
            UIManager.Instance?.ShowEventResult("Yeterli paran yok!", null);
        }

        #endregion
    }
}
