using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Profil ekranı kontrolcüsü - Karakter detaylarını gösterir.
    /// </summary>
    public class ProfileScreenController : MonoBehaviour
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
            var title = _factory.CreateText(transform, "Profil", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // ScrollView oluştur
            var scrollView = _factory.CreateScrollView(transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 0.9f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var content = scrollView.transform.Find("Viewport/Content");

            // Kişisel bilgiler
            AddSectionTitle(content, "Kişisel Bilgiler");
            AddInfoRow(content, "Ad Soyad", character.FullName);
            AddInfoRow(content, "Yaş", character.Age.ToString());
            AddInfoRow(content, "Cinsiyet", character.Gender == Gender.Male ? "Erkek" : "Kadın");
            AddInfoRow(content, "Doğum Yeri", character.birthCity);
            AddInfoRow(content, "Hayat Dönemi", GetLifeStageText(character.CurrentLifeStage));

            // İstatistikler
            AddSectionTitle(content, "İstatistikler");
            AddStatRow(content, "Sağlık", character.Stats.Health, UIStyles.HealthColor);
            AddStatRow(content, "Mutluluk", character.Stats.Happiness, UIStyles.HappinessColor);
            AddStatRow(content, "Zeka", character.Stats.Intelligence, UIStyles.IntelligenceColor);
            AddStatRow(content, "Görünüş", character.Stats.Appearance, UIStyles.AppearanceColor);
            AddStatRow(content, "Şöhret", character.Stats.Fame, UIStyles.FameColor);

            // Finansal durum
            AddSectionTitle(content, "Finansal Durum");
            AddInfoRow(content, "Mevcut Para", $"{character.Finances.CurrentMoney:N0} TL");
            AddInfoRow(content, "Toplam Kazanç", $"{character.Finances.totalEarned:N0} TL");
            AddInfoRow(content, "Toplam Harcama", $"{character.Finances.totalSpent:N0} TL");

            // Eğitim
            AddSectionTitle(content, "Eğitim");
            AddInfoRow(content, "Seviye", GetEducationText(character.Education.CurrentLevel));
            if (!string.IsNullOrEmpty(character.Education.schoolName))
            {
                AddInfoRow(content, "Okul", character.Education.schoolName);
            }
            if (character.Education.yksScore > 0)
            {
                AddInfoRow(content, "YKS Puanı", character.Education.yksScore.ToString());
            }
            if (!string.IsNullOrEmpty(character.Education.universityName))
            {
                AddInfoRow(content, "Üniversite", character.Education.universityName);
            }

            // Kariyer
            AddSectionTitle(content, "Kariyer");
            if (character.Career.CurrentJob != null)
            {
                AddInfoRow(content, "Meslek", character.Career.CurrentJob.title);
                AddInfoRow(content, "Maaş", $"{character.Career.CurrentJob.baseSalary:N0} TL");
                AddInfoRow(content, "Deneyim", $"{character.Career.yearsInJob} yıl");
            }
            else
            {
                AddInfoRow(content, "Durum", "İşsiz");
            }

            // Aile durumu
            AddSectionTitle(content, "Aile");
            AddInfoRow(content, "Medeni Hal", character.IsMarried ? "Evli" : "Bekar");

            // Aile üyeleri
            var parents = character.Relationships.FindAll(r => r.type == RelationType.Parent);
            var siblings = character.Relationships.FindAll(r => r.type == RelationType.Sibling);
            var children = character.Relationships.FindAll(r => r.type == RelationType.Child);

            AddInfoRow(content, "Ebeveyn Sayısı", parents.Count.ToString());
            AddInfoRow(content, "Kardeş Sayısı", siblings.Count.ToString());
            AddInfoRow(content, "Çocuk Sayısı", children.Count.ToString());
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

        private void AddInfoRow(Transform parent, string label, string value)
        {
            // Row container
            var row = new GameObject("InfoRow");
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

        private void AddStatRow(Transform parent, string label, int value, Color color)
        {
            // Row container
            var row = new GameObject("StatRow");
            row.transform.SetParent(parent, false);

            var rect = row.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 45);

            var layoutElement = row.AddComponent<LayoutElement>();
            layoutElement.minHeight = 45;
            layoutElement.preferredHeight = 45;

            // Label
            var labelObj = _factory.CreateText(row.transform, label, UIStyles.SmallText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0);
            labelRect.anchorMax = new Vector2(0.25f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Progress bar
            var bar = _factory.CreateProgressBar(row.transform, color, value / 100f);
            var barRect = bar.GetComponent<RectTransform>();
            barRect.anchorMin = new Vector2(0.27f, 0.25f);
            barRect.anchorMax = new Vector2(0.85f, 0.75f);
            barRect.offsetMin = Vector2.zero;
            barRect.offsetMax = Vector2.zero;

            // Value
            var valueObj = _factory.CreateText(row.transform, value.ToString(), UIStyles.SmallText);
            var valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.87f, 0);
            valueRect.anchorMax = new Vector2(0.98f, 1);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueObj.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
        }

        private string GetLifeStageText(LifeStage stage)
        {
            return stage switch
            {
                LifeStage.Baby => "Bebek",
                LifeStage.Child => "Çocuk",
                LifeStage.Teen => "Ergen",
                LifeStage.YoungAdult => "Genç Yetişkin",
                LifeStage.Adult => "Yetişkin",
                LifeStage.Senior => "Yaşlı",
                _ => "Bilinmiyor"
            };
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
