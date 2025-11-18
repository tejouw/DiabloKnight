using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;
using TurkishLifeSim.Systems;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Aktivite ekranı kontrolcüsü - Oyuncunun yapabileceği aktiviteleri gösterir.
    /// </summary>
    public class ActivityScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private ActivityCategory _currentCategory = ActivityCategory.Health;
        private GameObject _activityList;
        private List<GameObject> _categoryButtons = new List<GameObject>();

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
        }

        private void BuildUI()
        {
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

            // Kategori butonları
            BuildCategoryButtons();

            // Aktivite listesi
            BuildActivityList();

            // İlk kategoriyi göster
            ShowCategory(ActivityCategory.Health);
        }

        private void BuildCategoryButtons()
        {
            var categories = new (ActivityCategory cat, string name)[]
            {
                (ActivityCategory.Health, "Sağlık"),
                (ActivityCategory.Education, "Eğitim"),
                (ActivityCategory.Social, "Sosyal"),
                (ActivityCategory.Career, "Kariyer"),
                (ActivityCategory.Entertainment, "Eğlence"),
                (ActivityCategory.Financial, "Finans"),
                (ActivityCategory.Crime, "Suç")
            };

            float buttonWidth = 1f / categories.Length;

            for (int i = 0; i < categories.Length; i++)
            {
                var cat = categories[i];
                int index = i;

                var button = _factory.CreateButton(transform, cat.name, () =>
                {
                    ShowCategory(cat.cat);
                }, UIStyles.SecondaryButton);

                var rect = button.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(i * buttonWidth, 0.85f);
                rect.anchorMax = new Vector2((i + 1) * buttonWidth - 0.005f, 0.91f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                // Buton metnini küçült
                var buttonText = button.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.fontSize = 14;
                }

                _categoryButtons.Add(button);
            }
        }

        private void BuildActivityList()
        {
            // ScrollView
            var scrollView = _factory.CreateScrollView(transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 0.84f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            _activityList = scrollView.transform.Find("Viewport/Content").gameObject;
        }

        private void ShowCategory(ActivityCategory category)
        {
            _currentCategory = category;

            // Listeyi temizle
            foreach (Transform child in _activityList.transform)
            {
                Destroy(child.gameObject);
            }

            // Kategori butonlarını güncelle
            UpdateCategoryButtons();

            // Aktiviteleri getir
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            var allActivities = ActivitySystem.GetAvailableActivities(character);
            var categoryActivities = allActivities.FindAll(a => a.category == category);

            if (categoryActivities.Count == 0)
            {
                var noActivity = _factory.CreateText(_activityList.transform, "Bu kategoride şu an yapabileceğin aktivite yok.", UIStyles.BodyText);
                var layout = noActivity.AddComponent<LayoutElement>();
                layout.minHeight = 100;
                return;
            }

            foreach (var activity in categoryActivities)
            {
                CreateActivityCard(activity);
            }
        }

        private void UpdateCategoryButtons()
        {
            var categories = new ActivityCategory[]
            {
                ActivityCategory.Health,
                ActivityCategory.Education,
                ActivityCategory.Social,
                ActivityCategory.Career,
                ActivityCategory.Entertainment,
                ActivityCategory.Financial,
                ActivityCategory.Crime
            };

            for (int i = 0; i < _categoryButtons.Count && i < categories.Length; i++)
            {
                var image = _categoryButtons[i].GetComponent<Image>();
                if (image != null)
                {
                    if (categories[i] == _currentCategory)
                    {
                        image.color = UIStyles.PrimaryColor;
                    }
                    else
                    {
                        image.color = new Color(0.4f, 0.4f, 0.45f, 1f);
                    }
                }
            }
        }

        private void CreateActivityCard(Activity activity)
        {
            // Card container
            var card = _factory.CreatePanel(_activityList.transform, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 120;
            cardLayout.preferredHeight = 120;

            // Aktivite adı
            var nameObj = _factory.CreateText(card.transform, activity.name, UIStyles.SubtitleText);
            var nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0.05f, 0.65f);
            nameRect.anchorMax = new Vector2(0.7f, 0.95f);
            nameRect.offsetMin = Vector2.zero;
            nameRect.offsetMax = Vector2.zero;
            nameObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Maliyet göster
            if (activity.cost > 0)
            {
                var costObj = _factory.CreateText(card.transform, $"{activity.cost:N0} TL", UIStyles.SmallText);
                var costRect = costObj.GetComponent<RectTransform>();
                costRect.anchorMin = new Vector2(0.7f, 0.65f);
                costRect.anchorMax = new Vector2(0.95f, 0.95f);
                costRect.offsetMin = Vector2.zero;
                costRect.offsetMax = Vector2.zero;
                var costText = costObj.GetComponent<Text>();
                costText.alignment = TextAnchor.MiddleRight;
                costText.color = UIStyles.SecondaryColor;
            }

            // Açıklama
            var descObj = _factory.CreateText(card.transform, activity.description, UIStyles.SmallText);
            var descRect = descObj.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0.05f, 0.35f);
            descRect.anchorMax = new Vector2(0.95f, 0.65f);
            descRect.offsetMin = Vector2.zero;
            descRect.offsetMax = Vector2.zero;
            descObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Yap butonu
            var doButton = _factory.CreateButton(card.transform, "Yap", () =>
            {
                ExecuteActivity(activity);
            }, UIStyles.PrimaryButton);
            var doRect = doButton.GetComponent<RectTransform>();
            doRect.anchorMin = new Vector2(0.3f, 0.05f);
            doRect.anchorMax = new Vector2(0.7f, 0.3f);
            doRect.offsetMin = Vector2.zero;
            doRect.offsetMax = Vector2.zero;
        }

        private void ExecuteActivity(Activity activity)
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            // Aktiviteyi çalıştır
            string result = activity.Execute(character);

            // Sonucu göster
            UIManager.Instance.ShowEventResult(result, () =>
            {
                // Aktivite listesini yenile
                ShowCategory(_currentCategory);

                // Ana UI'ı yenile
                UIManager.Instance.RefreshUI();
            });
        }
    }
}
