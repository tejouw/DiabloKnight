using UnityEngine;
using UnityEngine.UI;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Ölüm ekranı kontrolcüsü.
    /// </summary>
    public class DeathScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private CharacterDiedEvent _deathEvent;

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            EventBus.Subscribe<CharacterDiedEvent>(OnCharacterDied);
            BuildUI();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<CharacterDiedEvent>(OnCharacterDied);
        }

        private void OnCharacterDied(CharacterDiedEvent evt)
        {
            _deathEvent = evt;
            RefreshUI();
        }

        private void BuildUI()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Karanlık arkaplan
            var bg = gameObject.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.9f);

            // R.I.P. Başlık
            var ripObj = _factory.CreateText(transform, "R.I.P.", UIStyles.TitleText);
            var ripText = ripObj.GetComponent<Text>();
            ripText.fontSize = 72;
            ripText.color = Color.white;
            var ripRect = ripObj.GetComponent<RectTransform>();
            ripRect.anchorMin = new Vector2(0, 0.8f);
            ripRect.anchorMax = new Vector2(1, 0.95f);
            ripRect.offsetMin = Vector2.zero;
            ripRect.offsetMax = Vector2.zero;
            ripText.alignment = TextAnchor.MiddleCenter;

            // İsim ve yaş
            var nameObj = _factory.CreateText(transform, character.FullName, UIStyles.SubtitleText);
            var nameText = nameObj.GetComponent<Text>();
            nameText.color = Color.white;
            var nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0, 0.7f);
            nameRect.anchorMax = new Vector2(1, 0.78f);
            nameRect.offsetMin = Vector2.zero;
            nameRect.offsetMax = Vector2.zero;
            nameText.alignment = TextAnchor.MiddleCenter;

            // Yaşam süresi
            var ageText = _factory.CreateText(transform, $"0 - {character.age} yaş", UIStyles.BodyText);
            var ageComp = ageText.GetComponent<Text>();
            ageComp.color = new Color(0.7f, 0.7f, 0.7f);
            var ageRect = ageText.GetComponent<RectTransform>();
            ageRect.anchorMin = new Vector2(0, 0.63f);
            ageRect.anchorMax = new Vector2(1, 0.7f);
            ageRect.offsetMin = Vector2.zero;
            ageRect.offsetMax = Vector2.zero;
            ageComp.alignment = TextAnchor.MiddleCenter;

            // İstatistikler paneli
            var statsPanel = _factory.CreatePanel(transform, new PanelStyle
            {
                backgroundColor = new Color(0.15f, 0.15f, 0.15f, 0.9f)
            });
            var statsRect = statsPanel.GetComponent<RectTransform>();
            statsRect.anchorMin = new Vector2(0.1f, 0.25f);
            statsRect.anchorMax = new Vector2(0.9f, 0.6f);
            statsRect.offsetMin = Vector2.zero;
            statsRect.offsetMax = Vector2.zero;

            // İstatistik başlığı
            var statsTitle = _factory.CreateText(statsPanel.transform, "Hayat İstatistikleri", UIStyles.SubtitleText);
            var statsTitleText = statsTitle.GetComponent<Text>();
            statsTitleText.color = UIStyles.AccentColor;
            var statsTitleRect = statsTitle.GetComponent<RectTransform>();
            statsTitleRect.anchorMin = new Vector2(0, 0.85f);
            statsTitleRect.anchorMax = new Vector2(1, 0.98f);
            statsTitleRect.offsetMin = Vector2.zero;
            statsTitleRect.offsetMax = Vector2.zero;
            statsTitleText.alignment = TextAnchor.MiddleCenter;

            // İstatistikler
            float yPos = 0.75f;
            float yStep = 0.13f;

            CreateStatLine(statsPanel.transform, "Toplam Kazanç", $"{character.finances.totalEarned:N0} TL", yPos);
            yPos -= yStep;
            CreateStatLine(statsPanel.transform, "Toplam Harcama", $"{character.finances.totalSpent:N0} TL", yPos);
            yPos -= yStep;
            CreateStatLine(statsPanel.transform, "Final Para", $"{character.finances.CurrentMoney:N0} TL", yPos);
            yPos -= yStep;
            CreateStatLine(statsPanel.transform, "İlişki Sayısı", $"{character.relationships.Count}", yPos);
            yPos -= yStep;
            CreateStatLine(statsPanel.transform, "Eğitim Seviyesi", GetEducationName(character.education.currentLevel), yPos);
            yPos -= yStep;

            if (character.career.currentJob != null)
            {
                CreateStatLine(statsPanel.transform, "Son Meslek", character.career.currentJob.title, yPos);
            }

            // Ana menü butonu
            var menuBtn = _factory.CreateButton(transform, "Ana Menü", () =>
            {
                GameManager.Instance.ReturnToMainMenu();
            }, UIStyles.PrimaryButton);
            var menuRect = menuBtn.GetComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(0.15f, 0.08f);
            menuRect.anchorMax = new Vector2(0.48f, 0.18f);
            menuRect.offsetMin = Vector2.zero;
            menuRect.offsetMax = Vector2.zero;

            // Yeni oyun butonu
            var newGameBtn = _factory.CreateButton(transform, "Yeni Hayat", () =>
            {
                GameManager.Instance.StartNewGame();
            }, UIStyles.SecondaryButton);
            var newGameRect = newGameBtn.GetComponent<RectTransform>();
            newGameRect.anchorMin = new Vector2(0.52f, 0.08f);
            newGameRect.anchorMax = new Vector2(0.85f, 0.18f);
            newGameRect.offsetMin = Vector2.zero;
            newGameRect.offsetMax = Vector2.zero;
        }

        private void CreateStatLine(Transform parent, string label, string value, float y)
        {
            // Label
            var labelObj = _factory.CreateText(parent, label, UIStyles.BodyText);
            var labelText = labelObj.GetComponent<Text>();
            labelText.color = new Color(0.7f, 0.7f, 0.7f);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, y - 0.05f);
            labelRect.anchorMax = new Vector2(0.5f, y + 0.05f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelText.alignment = TextAnchor.MiddleLeft;

            // Value
            var valueObj = _factory.CreateText(parent, value, UIStyles.BodyText);
            var valueText = valueObj.GetComponent<Text>();
            valueText.color = Color.white;
            var valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.5f, y - 0.05f);
            valueRect.anchorMax = new Vector2(0.95f, y + 0.05f);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueText.alignment = TextAnchor.MiddleRight;
        }

        private string GetEducationName(EducationLevel level)
        {
            return level switch
            {
                EducationLevel.None => "Eğitimsiz",
                EducationLevel.PrimarySchool => "İlkokul",
                EducationLevel.MiddleSchool => "Ortaokul",
                EducationLevel.HighSchool => "Lise",
                EducationLevel.University => "Üniversite",
                EducationLevel.Masters => "Yüksek Lisans",
                EducationLevel.Doctorate => "Doktora",
                _ => "Bilinmiyor"
            };
        }

        private void RefreshUI()
        {
            // UI'ı yeniden oluştur
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var img = GetComponent<Image>();
            if (img != null) Destroy(img);

            BuildUI();
        }
    }
}
