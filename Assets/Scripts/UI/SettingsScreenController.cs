using UnityEngine;
using UnityEngine.UI;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Ayarlar ekranı kontrolcüsü - Ses ve oyun ayarlarını yönetir.
    /// </summary>
    public class SettingsScreenController : MonoBehaviour
    {
        private UIFactory _factory;

        // Slider referansları
        private Slider _masterVolumeSlider;
        private Slider _musicVolumeSlider;
        private Slider _sfxVolumeSlider;

        // Toggle referansları
        private Toggle _autoSaveToggle;
        private Toggle _notificationsToggle;

        // Value label referansları
        private Text _masterValueText;
        private Text _musicValueText;
        private Text _sfxValueText;

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
                UIManager.Instance.ShowScreen(ScreenType.MainMenu);
            }, UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(transform, "Ayarlar", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // ScrollView oluştur
            var scrollView = _factory.CreateScrollView(transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0.08f);
            scrollRect.anchorMax = new Vector2(1, 0.9f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var content = scrollView.transform.Find("Viewport/Content");

            // Ses Ayarları Bölümü
            AddSectionTitle(content, "Ses Ayarları");

            // Master Volume
            CreateVolumeSlider(content, "Ana Ses", AudioManager.Instance?.MasterVolume ?? 1f,
                (value) => {
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.MasterVolume = value;
                    UpdateValueText(_masterValueText, value);
                }, out _masterVolumeSlider, out _masterValueText);

            // Music Volume
            CreateVolumeSlider(content, "Müzik", AudioManager.Instance?.MusicVolume ?? 0.8f,
                (value) => {
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.MusicVolume = value;
                    UpdateValueText(_musicValueText, value);
                }, out _musicVolumeSlider, out _musicValueText);

            // SFX Volume
            CreateVolumeSlider(content, "Ses Efektleri", AudioManager.Instance?.SFXVolume ?? 1f,
                (value) => {
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.SFXVolume = value;
                    UpdateValueText(_sfxValueText, value);
                }, out _sfxVolumeSlider, out _sfxValueText);

            // Oyun Ayarları Bölümü
            AddSectionTitle(content, "Oyun Ayarları");

            // Auto-save Toggle
            CreateToggle(content, "Otomatik Kaydetme",
                GameManager.Instance?.Settings.AutoSaveEnabled ?? true,
                (value) => {
                    if (GameManager.Instance != null)
                        GameManager.Instance.Settings.AutoSaveEnabled = value;
                }, out _autoSaveToggle);

            // Notifications Toggle
            CreateToggle(content, "Bildirimler",
                GameManager.Instance?.Settings.NotificationsEnabled ?? true,
                (value) => {
                    if (GameManager.Instance != null)
                        GameManager.Instance.Settings.NotificationsEnabled = value;
                }, out _notificationsToggle);

            // Kaydet Butonu
            var saveButton = _factory.CreateButton(transform, "Kaydet", SaveSettings, UIStyles.PrimaryButton);
            var saveRect = saveButton.GetComponent<RectTransform>();
            saveRect.anchorMin = new Vector2(0.25f, 0.01f);
            saveRect.anchorMax = new Vector2(0.75f, 0.07f);
            saveRect.offsetMin = Vector2.zero;
            saveRect.offsetMax = Vector2.zero;
        }

        private void AddSectionTitle(Transform parent, string title)
        {
            var titleObj = _factory.CreateText(parent, title, UIStyles.SubtitleText);
            var layoutElement = titleObj.AddComponent<LayoutElement>();
            layoutElement.minHeight = 60;
            layoutElement.preferredHeight = 60;

            var text = titleObj.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleLeft;
            text.color = UIStyles.PrimaryColor;
        }

        private void CreateVolumeSlider(Transform parent, string label, float initialValue,
            System.Action<float> onValueChanged, out Slider slider, out Text valueText)
        {
            // Container
            var container = new GameObject("SliderContainer");
            container.transform.SetParent(parent, false);

            var containerRect = container.AddComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(0, 80);

            var layoutElement = container.AddComponent<LayoutElement>();
            layoutElement.minHeight = 80;
            layoutElement.preferredHeight = 80;

            // Label
            var labelObj = _factory.CreateText(container.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0.55f);
            labelRect.anchorMax = new Vector2(0.95f, 0.95f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Slider
            var sliderObj = _factory.CreateSlider(container.transform, 0f, 1f, initialValue, onValueChanged);
            var sliderRect = sliderObj.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.05f, 0.1f);
            sliderRect.anchorMax = new Vector2(0.75f, 0.5f);
            sliderRect.offsetMin = Vector2.zero;
            sliderRect.offsetMax = Vector2.zero;

            slider = sliderObj.GetComponent<Slider>();

            // Value text
            var valueObj = _factory.CreateText(container.transform, $"{Mathf.RoundToInt(initialValue * 100)}%", UIStyles.BodyText);
            var valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.78f, 0.1f);
            valueRect.anchorMax = new Vector2(0.95f, 0.5f);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueText = valueObj.GetComponent<Text>();
            valueText.alignment = TextAnchor.MiddleRight;
        }

        private void CreateToggle(Transform parent, string label, bool initialValue,
            System.Action<bool> onValueChanged, out Toggle toggle)
        {
            // Container
            var container = new GameObject("ToggleContainer");
            container.transform.SetParent(parent, false);

            var containerRect = container.AddComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(0, 60);

            var layoutElement = container.AddComponent<LayoutElement>();
            layoutElement.minHeight = 60;
            layoutElement.preferredHeight = 60;

            // Background for toggle
            var toggleObj = new GameObject("Toggle");
            toggleObj.transform.SetParent(container.transform, false);

            var toggleRect = toggleObj.AddComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0.05f, 0.15f);
            toggleRect.anchorMax = new Vector2(0.15f, 0.85f);
            toggleRect.offsetMin = Vector2.zero;
            toggleRect.offsetMax = Vector2.zero;

            // Toggle background
            var bgImage = toggleObj.AddComponent<Image>();
            bgImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            // Checkmark
            var checkmark = new GameObject("Checkmark");
            checkmark.transform.SetParent(toggleObj.transform, false);

            var checkRect = checkmark.AddComponent<RectTransform>();
            checkRect.anchorMin = new Vector2(0.15f, 0.15f);
            checkRect.anchorMax = new Vector2(0.85f, 0.85f);
            checkRect.offsetMin = Vector2.zero;
            checkRect.offsetMax = Vector2.zero;

            var checkImage = checkmark.AddComponent<Image>();
            checkImage.color = UIStyles.AccentColor;

            // Toggle component
            toggle = toggleObj.AddComponent<Toggle>();
            toggle.targetGraphic = bgImage;
            toggle.graphic = checkImage;
            toggle.isOn = initialValue;

            if (onValueChanged != null)
            {
                toggle.onValueChanged.AddListener((value) => onValueChanged(value));
            }

            // Label
            var labelObj = _factory.CreateText(container.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.18f, 0);
            labelRect.anchorMax = new Vector2(0.95f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        }

        private void UpdateValueText(Text text, float value)
        {
            if (text != null)
            {
                text.text = $"{Mathf.RoundToInt(value * 100)}%";
            }
        }

        private void SaveSettings()
        {
            // Ses ayarlarını kaydet
            AudioManager.Instance?.SaveSettings();

            // Bilgi popup göster
            UIManager.Instance?.ShowInfo("Kaydedildi", "Ayarlarınız başarıyla kaydedildi.");
        }
    }
}
