using UnityEngine;
using UnityEngine.UI;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Ölüm ekranı kontrolcüsü - Karakter ölümü sonrasını gösterir.
    /// </summary>
    public class DeathScreenController : MonoBehaviour
    {
        private UIFactory _factory;

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
        }

        private void BuildUI()
        {
            var character = GameManager.Instance?.CurrentCharacter;

            // ScrollView oluştur
            var scrollView = _factory.CreateScrollView(transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0.15f);
            scrollRect.anchorMax = new Vector2(1, 1f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var content = scrollView.transform.Find("Viewport/Content");

            // R.I.P başlık
            var ripTitle = _factory.CreateText(content, "Huzur İçinde Yat", UIStyles.TitleText);
            var ripLayout = ripTitle.AddComponent<LayoutElement>();
            ripLayout.minHeight = 80;
            ripLayout.preferredHeight = 80;

            if (character != null)
            {
                // Karakter adı
                var nameText = _factory.CreateText(content, character.FullName, UIStyles.SubtitleText);
                var nameLayout = nameText.AddComponent<LayoutElement>();
                nameLayout.minHeight = 50;
                nameLayout.preferredHeight = 50;

                // Yaş bilgisi
                var ageText = _factory.CreateText(content, $"{character.Age} yaşında hayata veda etti", UIStyles.BodyText);
                var ageLayout = ageText.AddComponent<LayoutElement>();
                ageLayout.minHeight = 40;
                ageLayout.preferredHeight = 40;

                // Ölüm nedeni
                string deathCause = DetermineDeathCause(character);
                var causeText = _factory.CreateText(content, deathCause, UIStyles.BodyText);
                var causeLayout = causeText.AddComponent<LayoutElement>();
                causeLayout.minHeight = 50;
                causeLayout.preferredHeight = 50;
                causeText.GetComponent<Text>().color = UIStyles.SubtextColor;

                // Epitaph (mezar yazısı)
                AddEpitaph(content, character);

                // Hayat İstatistikleri
                AddLifeStats(content, character);
            }
            else
            {
                // Karakter yoksa basit mesaj
                var noDataText = _factory.CreateText(content, "Karakter verisi bulunamadı.", UIStyles.BodyText);
                var noDataLayout = noDataText.AddComponent<LayoutElement>();
                noDataLayout.minHeight = 100;
                noDataLayout.preferredHeight = 100;
            }

            // Alt buton container
            var buttonContainer = _factory.CreatePanel(transform, new PanelStyle { backgroundColor = Color.clear });
            var buttonRect = buttonContainer.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0, 0);
            buttonRect.anchorMax = new Vector2(1, 0.14f);
            buttonRect.offsetMin = new Vector2(10, 10);
            buttonRect.offsetMax = new Vector2(-10, -5);

            // Ana Menü butonu
            var menuButton = _factory.CreateButton(buttonContainer.transform, "Ana Menü", () =>
            {
                GameManager.Instance?.ReturnToMainMenu();
            }, UIStyles.SecondaryButton);
            var menuRect = menuButton.GetComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(0.05f, 0.15f);
            menuRect.anchorMax = new Vector2(0.48f, 0.85f);
            menuRect.offsetMin = Vector2.zero;
            menuRect.offsetMax = Vector2.zero;

            // Yeni Oyun butonu
            var newGameButton = _factory.CreateButton(buttonContainer.transform, "Yeni Oyun", () =>
            {
                GameManager.Instance?.StartNewGame();
            }, UIStyles.PrimaryButton);
            var newGameRect = newGameButton.GetComponent<RectTransform>();
            newGameRect.anchorMin = new Vector2(0.52f, 0.15f);
            newGameRect.anchorMax = new Vector2(0.95f, 0.85f);
            newGameRect.offsetMin = Vector2.zero;
            newGameRect.offsetMax = Vector2.zero;
        }

        private string DetermineDeathCause(CharacterData character)
        {
            if (character.Stats.Health <= 0)
            {
                return "Sağlık sorunları nedeniyle hayatını kaybetti.";
            }

            if (character.Age >= 100)
            {
                return "Uzun ve dolu bir hayatın ardından doğal sebeplerden vefat etti.";
            }

            if (character.Age >= 90)
            {
                return "Doğal sebeplerden hayatını kaybetti.";
            }

            if (character.Age >= 70)
            {
                return "Yaşlılık nedeniyle hayata gözlerini yumdu.";
            }

            return "Beklenmedik bir şekilde hayatını kaybetti.";
        }

        private void AddEpitaph(Transform content, CharacterData character)
        {
            // Boşluk
            var spacer = new GameObject("Spacer");
            spacer.transform.SetParent(content, false);
            var spacerLayout = spacer.AddComponent<LayoutElement>();
            spacerLayout.minHeight = 30;

            // Epitaph panel
            var epitaphPanel = _factory.CreatePanel(content, UIStyles.CardPanel);
            var panelLayout = epitaphPanel.AddComponent<LayoutElement>();
            panelLayout.minHeight = 150;
            panelLayout.preferredHeight = 150;

            // Epitaph metni
            string epitaph = GenerateEpitaph(character);
            var epitaphText = _factory.CreateText(epitaphPanel.transform, epitaph, UIStyles.BodyText);
            var epitaphRect = epitaphText.GetComponent<RectTransform>();
            epitaphRect.anchorMin = new Vector2(0.1f, 0.1f);
            epitaphRect.anchorMax = new Vector2(0.9f, 0.9f);
            epitaphRect.offsetMin = Vector2.zero;
            epitaphRect.offsetMax = Vector2.zero;
            var text = epitaphText.GetComponent<Text>();
            text.fontStyle = FontStyle.Italic;
            text.alignment = TextAnchor.MiddleCenter;
        }

        private string GenerateEpitaph(CharacterData character)
        {
            // Karakter özelliklerine göre epitaph oluştur
            int topStat = GetTopStat(character);
            string epitaph = "";

            if (character.Stats.Intelligence >= topStat)
            {
                epitaph = "Bilgeliği ile anılacak...";
            }
            else if (character.Stats.Happiness >= topStat)
            {
                epitaph = "Neşesi ve mutluluğu ile hatırlanacak...";
            }
            else if (character.Stats.Fame >= 50)
            {
                epitaph = "Şöhreti sonsuza dek yaşayacak...";
            }
            else if (character.Finances.totalEarned > 1000000)
            {
                epitaph = "Başarılı bir hayatın ardında iz bıraktı...";
            }
            else if (character.Relationships.Count > 5)
            {
                epitaph = "Sevdikleri tarafından her zaman anılacak...";
            }
            else
            {
                epitaph = "Sakin bir hayatın huzuru içinde yatıyor...";
            }

            return $"\"{epitaph}\"";
        }

        private int GetTopStat(CharacterData character)
        {
            int max = character.Stats.Health;
            if (character.Stats.Happiness > max) max = character.Stats.Happiness;
            if (character.Stats.Intelligence > max) max = character.Stats.Intelligence;
            if (character.Stats.Appearance > max) max = character.Stats.Appearance;
            return max;
        }

        private void AddLifeStats(Transform content, CharacterData character)
        {
            // Boşluk
            var spacer = new GameObject("Spacer2");
            spacer.transform.SetParent(content, false);
            var spacerLayout = spacer.AddComponent<LayoutElement>();
            spacerLayout.minHeight = 30;

            // İstatistik başlığı
            var statsTitle = _factory.CreateText(content, "Hayat İstatistikleri", UIStyles.SubtitleText);
            var titleLayout = statsTitle.AddComponent<LayoutElement>();
            titleLayout.minHeight = 50;
            titleLayout.preferredHeight = 50;
            statsTitle.GetComponent<Text>().color = UIStyles.PrimaryColor;
            statsTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // İstatistik satırları
            AddStatRow(content, "Yaşanan Yıl", $"{character.Age} yıl");
            AddStatRow(content, "Toplam Kazanç", $"{character.Finances.totalEarned:N0} TL");
            AddStatRow(content, "Toplam Harcama", $"{character.Finances.totalSpent:N0} TL");
            AddStatRow(content, "İlişki Sayısı", character.Relationships.Count.ToString());

            // Eğitim durumu
            string educationText = GetEducationText(character.Education.CurrentLevel);
            AddStatRow(content, "Eğitim Seviyesi", educationText);

            // Kariyer
            if (character.Career.CurrentJob != null)
            {
                AddStatRow(content, "Son Meslek", character.Career.CurrentJob.title);
            }

            // Son statlar
            AddStatRow(content, "Son Sağlık", $"{character.Stats.Health}/100");
            AddStatRow(content, "Son Mutluluk", $"{character.Stats.Happiness}/100");
            AddStatRow(content, "Son Zeka", $"{character.Stats.Intelligence}/100");
        }

        private void AddStatRow(Transform parent, string label, string value)
        {
            var row = new GameObject("StatRow");
            row.transform.SetParent(parent, false);

            var rect = row.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 40);

            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;
            layout.spacing = 10;
            layout.padding = new RectOffset(20, 20, 0, 0);

            var layoutElement = row.AddComponent<LayoutElement>();
            layoutElement.minHeight = 40;
            layoutElement.preferredHeight = 40;

            // Label
            var labelObj = _factory.CreateText(row.transform, label + ":", UIStyles.SmallText);
            var labelLayout = labelObj.AddComponent<LayoutElement>();
            labelLayout.flexibleWidth = 1;

            // Value
            var valueObj = _factory.CreateText(row.transform, value, UIStyles.SmallText);
            var valueText = valueObj.GetComponent<Text>();
            valueText.alignment = TextAnchor.MiddleRight;
            valueText.color = UIStyles.TextColor;
            var valueLayout = valueObj.AddComponent<LayoutElement>();
            valueLayout.flexibleWidth = 1;
        }

        private string GetEducationText(EducationLevel level)
        {
            return level switch
            {
                EducationLevel.None => "Eğitim yok",
                EducationLevel.PrimarySchool => "İlkokul",
                EducationLevel.MiddleSchool => "Ortaokul",
                EducationLevel.HighSchool => "Lise",
                EducationLevel.University => "Üniversite",
                EducationLevel.Masters => "Yüksek Lisans",
                EducationLevel.Doctorate => "Doktora",
                _ => "Bilinmiyor"
            };
        }
    }
}
