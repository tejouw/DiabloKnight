using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Aktiviteler ekranı - Oyuncunun yapabileceği aktiviteleri listeler.
    /// </summary>
    public class ActivitiesScreenController : MonoBehaviour
    {
        private UIFactory _factory;

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
        }

        private void BuildUI()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Geri butonu
            var backButton = _factory.CreateButton(transform, "< Geri", () =>
            {
                UIManager.Instance.ShowScreen(ScreenType.Game);
            }, UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(transform, "Aktiviteler", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // ScrollView
            var scrollView = _factory.CreateScrollView(transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 0.9f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var content = scrollView.transform.Find("Viewport/Content");

            // Sağlık Aktiviteleri
            AddSectionTitle(content, "Sağlık");
            AddActivityButton(content, "Doktora Git", "Sağlık kontrolü (500 TL)", () => GoToDoctor(character));
            AddActivityButton(content, "Spor Salonu", "Egzersiz yap", () => GoToGym(character));
            AddActivityButton(content, "Meditasyon", "İç huzur bul", () => Meditate(character));
            AddActivityButton(content, "Diyet Yap", "Sağlıklı beslen", () => StartDiet(character));

            // Eğitim Aktiviteleri
            AddSectionTitle(content, "Eğitim");
            AddActivityButton(content, "Kütüphane", "Kitap oku", () => GoToLibrary(character));
            AddActivityButton(content, "Online Kurs", "Yeni beceri öğren (1000 TL)", () => TakeOnlineCourse(character));

            // Sosyal Aktiviteler
            AddSectionTitle(content, "Sosyal");
            AddActivityButton(content, "Sinemaya Git", "Film izle (100 TL)", () => GoToCinema(character));
            AddActivityButton(content, "Kafede Takıl", "Rahatla (50 TL)", () => GoToCafe(character));
            AddActivityButton(content, "Gönüllü Çalış", "İnsanlara yardım et", () => Volunteer(character));

            // Finansal Aktiviteler
            if (character.Age >= 18)
            {
                AddSectionTitle(content, "Finans");
                AddActivityButton(content, "İş Ara", "İş başvurusu yap", () => SearchForJob(character));
                AddActivityButton(content, "Piyango Al", "Şansını dene (100 TL)", () => BuyLottery(character));
            }

            // Suç (gizli)
            if (character.Age >= 16)
            {
                AddSectionTitle(content, "Diğer");
                AddActivityButton(content, "Şüpheli İşler", "Risk al...", () => DoShadyStuff(character));
            }
        }

        private void AddSectionTitle(Transform parent, string title)
        {
            var titleObj = _factory.CreateText(parent, title, UIStyles.SubtitleText);
            var layoutElement = titleObj.AddComponent<LayoutElement>();
            layoutElement.minHeight = 50;
            layoutElement.preferredHeight = 50;

            var text = titleObj.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleLeft;
            text.color = UIStyles.PrimaryColor;
        }

        private void AddActivityButton(Transform parent, string buttonText, string description, System.Action onClick)
        {
            // Activity card
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 80;
            cardLayout.preferredHeight = 80;

            // Button
            var button = _factory.CreateButton(card.transform, buttonText, onClick, UIStyles.ChoiceButton);
            var buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.05f, 0.15f);
            buttonRect.anchorMax = new Vector2(0.6f, 0.85f);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;

            // Description
            var descObj = _factory.CreateText(card.transform, description, UIStyles.SmallText);
            var descRect = descObj.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0.62f, 0.15f);
            descRect.anchorMax = new Vector2(0.95f, 0.85f);
            descRect.offsetMin = Vector2.zero;
            descRect.offsetMax = Vector2.zero;
            descObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        }

        #region Activity Actions

        private void GoToDoctor(CharacterData character)
        {
            if (character.Finances.CurrentMoney < 500)
            {
                UIManager.Instance.ShowInfo("Yeterli paran yok!", "Doktor ücreti 500 TL.");
                return;
            }

            character.Finances.ModifyMoney(-500, "Doktor ücreti");

            if (Random.value < 0.7f)
            {
                character.Stats.ModifyStat(StatType.Health, Random.Range(5, 15));
                UIManager.Instance.ShowInfo("Sağlık Kontrolü", "Her şey yolunda! Sağlığın iyileşti.");
            }
            else
            {
                UIManager.Instance.ShowInfo("Sağlık Kontrolü", "Küçük bir sorun tespit edildi, ilaç aldın.");
            }

            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void GoToGym(CharacterData character)
        {
            if (Random.value < 0.7f)
            {
                character.Stats.ModifyStat(StatType.Health, Random.Range(3, 8));
                character.Stats.ModifyStat(StatType.Appearance, Random.Range(1, 3));
                UIManager.Instance.ShowInfo("Spor Salonu", "Harika antrenman! Kendini güçlü hissediyorsun.");
            }
            else
            {
                character.Stats.ModifyStat(StatType.Health, Random.Range(-3, -1));
                UIManager.Instance.ShowInfo("Spor Salonu", "Biraz fazla zorladın, kasların ağrıyor.");
            }

            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void Meditate(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 12));
            UIManager.Instance.ShowInfo("Meditasyon", "İç huzur buldun, kendini çok iyi hissediyorsun!");
            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void StartDiet(CharacterData character)
        {
            if (Random.value < 0.6f)
            {
                character.Stats.ModifyStat(StatType.Health, Random.Range(2, 6));
                character.Stats.ModifyStat(StatType.Appearance, Random.Range(1, 4));
                UIManager.Instance.ShowInfo("Diyet", "Sağlıklı beslenme işe yaradı!");
            }
            else
            {
                UIManager.Instance.ShowInfo("Diyet", "Bu sefer tutmadı, tekrar dene!");
            }

            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void GoToLibrary(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Intelligence, Random.Range(2, 5));
            UIManager.Instance.ShowInfo("Kütüphane", "Güzel kitaplar okudun, zekan gelişti!");
            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void TakeOnlineCourse(CharacterData character)
        {
            if (character.Finances.CurrentMoney < 1000)
            {
                UIManager.Instance.ShowInfo("Yetersiz Bakiye", "Online kurs ücreti 1000 TL.");
                return;
            }

            character.Finances.ModifyMoney(-1000, "Online kurs ücreti");
            character.Stats.ModifyStat(StatType.Intelligence, Random.Range(5, 10));
            UIManager.Instance.ShowInfo("Online Kurs", "Yeni beceriler öğrendin, zekan gelişti!");
            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void GoToCinema(CharacterData character)
        {
            if (character.Finances.CurrentMoney < 100)
            {
                UIManager.Instance.ShowInfo("Yetersiz Bakiye", "Sinema bileti 100 TL.");
                return;
            }

            character.Finances.ModifyMoney(-100, "Sinema bileti");
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(5, 10));
            UIManager.Instance.ShowInfo("Sinema", "Güzel bir film izledin!");
            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void GoToCafe(CharacterData character)
        {
            if (character.Finances.CurrentMoney < 50)
            {
                UIManager.Instance.ShowInfo("Yetersiz Bakiye", "Kafe masrafı 50 TL.");
                return;
            }

            character.Finances.ModifyMoney(-50, "Kafe");
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(3, 7));
            UIManager.Instance.ShowInfo("Kafe", "Rahatladın, güzel vakit geçirdin.");
            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void Volunteer(CharacterData character)
        {
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(8, 15));
            UIManager.Instance.ShowInfo("Gönüllü Çalışma", "İnsanlara yardım etmek harika hissettirdi!");
            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void SearchForJob(CharacterData character)
        {
            // İş bulma şansı zeka ve görünüşe bağlı
            float chance = (character.Stats.Intelligence + character.Stats.Appearance) / 200f;

            if (Random.value < chance + 0.3f)
            {
                // İş bul
                var availableJobs = DataManager.Instance.GetAvailableJobs(
                    character.Education.currentLevel,
                    character.Stats.Intelligence
                );

                if (availableJobs.Count > 0)
                {
                    var randomJob = availableJobs[Random.Range(0, availableJobs.Count)];

                    var job = new Job
                    {
                        id = randomJob.id,
                        title = randomJob.title,
                        company = GetRandomCompany(),
                        category = randomJob.category,
                        baseSalary = randomJob.baseSalary
                    };

                    character.Career.SetJob(job);
                    character.Stats.ModifyStat(StatType.Happiness, Random.Range(10, 20));

                    UIManager.Instance.ShowInfo("Tebrikler!",
                        $"İşe alındın!\n\n{job.title}\nMaaş: {job.baseSalary:N0} TL/ay");
                }
                else
                {
                    UIManager.Instance.ShowInfo("İş Arama", "Eğitim seviyene uygun iş bulunamadı.");
                }
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(-5, -2));
                UIManager.Instance.ShowInfo("İş Arama", "Maalesef bu sefer olmadı. Tekrar dene!");
            }

            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private string GetRandomCompany()
        {
            string[] companies = {
                "ABC Holding", "TechTürk A.Ş.", "Anadolu Grup", "İstanbul Finans",
                "Mega Market", "Süper Elektronik", "Altın Yazılım", "Deniz İnşaat",
                "Güneş Enerji", "Türk Sanayi"
            };
            return companies[Random.Range(0, companies.Length)];
        }

        private void BuyLottery(CharacterData character)
        {
            if (character.Finances.CurrentMoney < 100)
            {
                UIManager.Instance.ShowInfo("Yetersiz Bakiye", "Piyango bileti 100 TL.");
                return;
            }

            character.Finances.ModifyMoney(-100, "Piyango bileti");

            float roll = Random.value;

            if (roll < 0.05f)
            {
                // Büyük ikramiye!
                int prize = Random.Range(50000, 500000);
                character.Finances.ModifyMoney(prize, "Piyango büyük ikramiye!");
                character.Stats.ModifyStat(StatType.Happiness, 30);
                UIManager.Instance.ShowInfo("BÜYÜK İKRAMİYE!", $"Tebrikler! {prize:N0} TL kazandın!");
            }
            else if (roll < 0.2f)
            {
                // Küçük ödül
                int prize = Random.Range(100, 1000);
                character.Finances.ModifyMoney(prize, "Piyango ödülü");
                UIManager.Instance.ShowInfo("Kazandın!", $"{prize:N0} TL kazandın.");
            }
            else
            {
                UIManager.Instance.ShowInfo("Piyango", "Bu sefer şanssızdın. Belki bir dahaki sefere!");
            }

            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        private void DoShadyStuff(CharacterData character)
        {
            // Suç aktiviteleri
            float successChance = 0.4f;

            if (Random.value < successChance)
            {
                int gain = Random.Range(500, 3000);
                character.Finances.ModifyMoney(gain, "Şüpheli kazanç");
                UIManager.Instance.ShowInfo("Başarılı", $"Kimse görmedi... {gain:N0} TL kazandın.");
            }
            else
            {
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(-25, -15));
                UIManager.Instance.ShowInfo("Yakalandın!", "Polise ihbar edildin. Çok kötü bir gün.");
            }

            UIManager.Instance.ShowScreen(ScreenType.Game);
        }

        #endregion
    }
}
