using UnityEngine;
using UnityEngine.UI;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Ayarlar ekrani kontrolcusu.
    /// </summary>
    public class SettingsScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private Slider _musicSlider;
        private Slider _sfxSlider;
        private Toggle _autoSaveToggle;
        private Toggle _notificationToggle;

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
                SaveSettings();
                if (GameManager.Instance.IsPlaying)
                    UIManager.Instance.ShowScreen(ScreenType.Game);
                else
                    UIManager.Instance.ShowScreen(ScreenType.MainMenu);
            }, UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Baslik
            var title = _factory.CreateText(transform, "Ayarlar", UIStyles.TitleText);
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

            // Ses Ayarlari Bolumu
            CreateSectionHeader(content, "Ses Ayarlari");

            // Muzik Sesi
            CreateSliderSetting(content, "Muzik Sesi", (value) =>
            {
                AudioManager.Instance?.SetMusicVolume(value);
            }, out _musicSlider);
            _musicSlider.value = AudioManager.Instance?.MusicVolume ?? 0.7f;

            // Efekt Sesi
            CreateSliderSetting(content, "Efekt Sesi", (value) =>
            {
                AudioManager.Instance?.SetSFXVolume(value);
            }, out _sfxSlider);
            _sfxSlider.value = AudioManager.Instance?.SFXVolume ?? 1f;

            // Oyun Ayarlari Bolumu
            CreateSectionHeader(content, "Oyun Ayarlari");

            // Otomatik Kayit
            CreateToggleSetting(content, "Otomatik Kayit", (isOn) =>
            {
                // Otomatik kayit ayari
                PlayerPrefs.SetInt("AutoSave", isOn ? 1 : 0);
            }, out _autoSaveToggle);
            _autoSaveToggle.isOn = PlayerPrefs.GetInt("AutoSave", 1) == 1;

            // Bildirimler
            CreateToggleSetting(content, "Bildirimler", (isOn) =>
            {
                PlayerPrefs.SetInt("Notifications", isOn ? 1 : 0);
            }, out _notificationToggle);
            _notificationToggle.isOn = PlayerPrefs.GetInt("Notifications", 1) == 1;

            // Veri Yonetimi Bolumu
            CreateSectionHeader(content, "Veri Yonetimi");

            // Tum Kayitlari Sil
            CreateDangerButton(content, "Tum Kayitlari Sil", () =>
            {
                UIManager.Instance.ShowConfirmation(
                    "Tum oyun kayitlarinizi silmek istediginize emin misiniz? Bu islem geri alinamaz!",
                    () =>
                    {
                        DeleteAllSaves();
                        UIManager.Instance.ShowInfo("Basarili", "Tum kayitlar silindi.");
                    }
                );
            });

            // Ayarlari Sifirla
            CreateDangerButton(content, "Ayarlari Sifirla", () =>
            {
                UIManager.Instance.ShowConfirmation(
                    "Tum ayarlari varsayilana dondurmek istediginize emin misiniz?",
                    () =>
                    {
                        ResetSettings();
                        UIManager.Instance.ShowInfo("Basarili", "Ayarlar sifirlandi.");
                        RefreshUI();
                    }
                );
            });

            // Hakkinda Bolumu
            CreateSectionHeader(content, "Hakkinda");
            CreateInfoText(content, "Turk Hayati v1.0");
            CreateInfoText(content, "Bir hayat simulasyonu oyunu");
            CreateInfoText(content, "2024 - Tum haklari saklidir");
        }

        private void CreateSectionHeader(Transform parent, string text)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 50;
            cardLayout.preferredHeight = 50;

            var headerText = _factory.CreateText(card.transform, text, UIStyles.SubtitleText);
            var headerRect = headerText.GetComponent<RectTransform>();
            headerRect.anchorMin = new Vector2(0.05f, 0);
            headerRect.anchorMax = new Vector2(0.95f, 1);
            headerRect.offsetMin = Vector2.zero;
            headerRect.offsetMax = Vector2.zero;
            headerText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            var image = card.GetComponent<Image>();
            if (image != null)
            {
                image.color = UIStyles.PrimaryColor;
            }
        }

        private void CreateSliderSetting(Transform parent, string label, System.Action<float> onValueChanged, out Slider slider)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 80;
            cardLayout.preferredHeight = 80;

            // Label
            var labelObj = _factory.CreateText(card.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0.5f);
            labelRect.anchorMax = new Vector2(0.4f, 0.95f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Slider
            var sliderObj = new GameObject("Slider");
            sliderObj.transform.SetParent(card.transform, false);

            var sliderRect = sliderObj.AddComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.05f, 0.15f);
            sliderRect.anchorMax = new Vector2(0.95f, 0.45f);
            sliderRect.offsetMin = Vector2.zero;
            sliderRect.offsetMax = Vector2.zero;

            // Slider Background
            var bgObj = new GameObject("Background");
            bgObj.transform.SetParent(sliderObj.transform, false);
            var bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.25f, 1f);
            var bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Fill Area
            var fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObj.transform, false);
            var fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0, 0.25f);
            fillAreaRect.anchorMax = new Vector2(1, 0.75f);
            fillAreaRect.offsetMin = new Vector2(5, 0);
            fillAreaRect.offsetMax = new Vector2(-5, 0);

            // Fill
            var fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            var fillImage = fill.AddComponent<Image>();
            fillImage.color = UIStyles.PrimaryColor;
            var fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            // Handle Slide Area
            var handleArea = new GameObject("Handle Slide Area");
            handleArea.transform.SetParent(sliderObj.transform, false);
            var handleAreaRect = handleArea.AddComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.offsetMin = new Vector2(10, 0);
            handleAreaRect.offsetMax = new Vector2(-10, 0);

            // Handle
            var handle = new GameObject("Handle");
            handle.transform.SetParent(handleArea.transform, false);
            var handleImage = handle.AddComponent<Image>();
            handleImage.color = Color.white;
            var handleRect = handle.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 0);
            handleRect.anchorMin = new Vector2(0, 0);
            handleRect.anchorMax = new Vector2(0, 1);

            // Slider Component
            slider = sliderObj.AddComponent<Slider>();
            slider.fillRect = fillRect;
            slider.handleRect = handleRect;
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.onValueChanged.AddListener((value) => onValueChanged?.Invoke(value));
        }

        private void CreateToggleSetting(Transform parent, string label, System.Action<bool> onValueChanged, out Toggle toggle)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 70;
            cardLayout.preferredHeight = 70;

            // Label
            var labelObj = _factory.CreateText(card.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0);
            labelRect.anchorMax = new Vector2(0.7f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Toggle
            var toggleObj = new GameObject("Toggle");
            toggleObj.transform.SetParent(card.transform, false);

            var toggleRect = toggleObj.AddComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0.75f, 0.2f);
            toggleRect.anchorMax = new Vector2(0.95f, 0.8f);
            toggleRect.offsetMin = Vector2.zero;
            toggleRect.offsetMax = Vector2.zero;

            // Background
            var bgObj = new GameObject("Background");
            bgObj.transform.SetParent(toggleObj.transform, false);
            var bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0.3f, 0.3f, 0.35f, 1f);
            var bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Checkmark
            var checkmark = new GameObject("Checkmark");
            checkmark.transform.SetParent(bgObj.transform, false);
            var checkImage = checkmark.AddComponent<Image>();
            checkImage.color = UIStyles.PrimaryColor;
            var checkRect = checkmark.GetComponent<RectTransform>();
            checkRect.anchorMin = new Vector2(0.1f, 0.1f);
            checkRect.anchorMax = new Vector2(0.9f, 0.9f);
            checkRect.offsetMin = Vector2.zero;
            checkRect.offsetMax = Vector2.zero;

            // Toggle Component
            toggle = toggleObj.AddComponent<Toggle>();
            toggle.targetGraphic = bgImage;
            toggle.graphic = checkImage;
            toggle.onValueChanged.AddListener((isOn) => onValueChanged?.Invoke(isOn));
        }

        private void CreateDangerButton(Transform parent, string text, System.Action onClick)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 70;
            cardLayout.preferredHeight = 70;

            var button = _factory.CreateButton(card.transform, text, onClick, UIStyles.DangerButton);
            var buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.1f, 0.15f);
            buttonRect.anchorMax = new Vector2(0.9f, 0.85f);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;
        }

        private void CreateInfoText(Transform parent, string text)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 40;
            cardLayout.preferredHeight = 40;

            var textObj = _factory.CreateText(card.transform, text, UIStyles.SmallText);
            var textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.05f, 0);
            textRect.anchorMax = new Vector2(0.95f, 1);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            var textComp = textObj.GetComponent<Text>();
            textComp.alignment = TextAnchor.MiddleCenter;
            textComp.color = UIStyles.SubtextColor;
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetFloat("MusicVolume", _musicSlider?.value ?? 0.7f);
            PlayerPrefs.SetFloat("SFXVolume", _sfxSlider?.value ?? 1f);
            PlayerPrefs.Save();
        }

        private void ResetSettings()
        {
            // Ses ayarlarini sifirla
            AudioManager.Instance?.SetMusicVolume(0.7f);
            AudioManager.Instance?.SetSFXVolume(1f);

            // Toggle'lari sifirla
            PlayerPrefs.SetInt("AutoSave", 1);
            PlayerPrefs.SetInt("Notifications", 1);
            PlayerPrefs.Save();
        }

        private void DeleteAllSaves()
        {
            for (int i = 0; i < 5; i++)
            {
                SaveManager.Instance?.DeleteSave(i);
            }
            SaveManager.Instance?.DeleteAutoSave();
        }

        private void RefreshUI()
        {
            // Mevcut UI'i temizle ve yeniden olustur
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            BuildUI();
        }
    }
}
