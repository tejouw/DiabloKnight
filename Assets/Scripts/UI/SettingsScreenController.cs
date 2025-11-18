using UnityEngine;
using UnityEngine.UI;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Ayarlar ekranı kontrolcüsü.
    /// </summary>
    public class SettingsScreenController : MonoBehaviour
    {
        private UIFactory _factory;

        // Slider referansları
        private GameObject _masterVolumeSlider;
        private GameObject _musicVolumeSlider;
        private GameObject _sfxVolumeSlider;

        // Toggle referansları
        private Toggle _autoSaveToggle;
        private Toggle _notificationsToggle;

        // Labels
        private Text _masterValueText;
        private Text _musicValueText;
        private Text _sfxValueText;

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
            LoadSettings();
        }

        private void BuildUI()
        {
            // Başlık
            var titleObj = _factory.CreateText(transform, "AYARLAR", UIStyles.TitleText);
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.88f);
            titleRect.anchorMax = new Vector2(1, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;
            titleObj.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            // Ana panel
            var mainPanel = _factory.CreatePanel(transform, UIStyles.CardPanel);
            var mainRect = mainPanel.GetComponent<RectTransform>();
            mainRect.anchorMin = new Vector2(0.05f, 0.15f);
            mainRect.anchorMax = new Vector2(0.95f, 0.85f);
            mainRect.offsetMin = Vector2.zero;
            mainRect.offsetMax = Vector2.zero;

            float yPos = 0.9f;
            float yStep = 0.12f;

            // Ses Ayarları Başlığı
            CreateSectionHeader(mainPanel.transform, "Ses Ayarları", yPos);
            yPos -= yStep;

            // Ana Ses
            CreateSliderRow(mainPanel.transform, "Ana Ses", yPos, out _masterVolumeSlider, out _masterValueText, value =>
            {
                GameManager.Instance.Settings.MasterVolume = value;
                AudioManager.Instance?.SetMasterVolume(value);
                _masterValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
            });
            yPos -= yStep;

            // Müzik
            CreateSliderRow(mainPanel.transform, "Müzik", yPos, out _musicVolumeSlider, out _musicValueText, value =>
            {
                GameManager.Instance.Settings.MusicVolume = value;
                AudioManager.Instance?.SetMusicVolume(value);
                _musicValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
            });
            yPos -= yStep;

            // Ses Efektleri
            CreateSliderRow(mainPanel.transform, "Efektler", yPos, out _sfxVolumeSlider, out _sfxValueText, value =>
            {
                GameManager.Instance.Settings.SFXVolume = value;
                AudioManager.Instance?.SetSFXVolume(value);
                _sfxValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
            });
            yPos -= yStep * 1.5f;

            // Oyun Ayarları Başlığı
            CreateSectionHeader(mainPanel.transform, "Oyun Ayarları", yPos);
            yPos -= yStep;

            // Otomatik Kayıt
            CreateToggleRow(mainPanel.transform, "Otomatik Kayıt", yPos, out _autoSaveToggle, isOn =>
            {
                GameManager.Instance.Settings.AutoSaveEnabled = isOn;
            });
            yPos -= yStep;

            // Bildirimler
            CreateToggleRow(mainPanel.transform, "Bildirimler", yPos, out _notificationsToggle, isOn =>
            {
                GameManager.Instance.Settings.NotificationsEnabled = isOn;
            });

            // Geri butonu
            var backBtn = _factory.CreateButton(transform, "Geri", () =>
            {
                SaveSettings();
                UIManager.Instance.ShowScreen(ScreenType.MainMenu);
            }, UIStyles.SecondaryButton);
            var backRect = backBtn.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.3f, 0.03f);
            backRect.anchorMax = new Vector2(0.7f, 0.12f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;
        }

        private void CreateSectionHeader(Transform parent, string text, float y)
        {
            var headerObj = _factory.CreateText(parent, text, UIStyles.SubtitleText);
            var headerRect = headerObj.GetComponent<RectTransform>();
            headerRect.anchorMin = new Vector2(0.05f, y - 0.04f);
            headerRect.anchorMax = new Vector2(0.95f, y + 0.04f);
            headerRect.offsetMin = Vector2.zero;
            headerRect.offsetMax = Vector2.zero;
            headerObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        }

        private void CreateSliderRow(Transform parent, string label, float y, out GameObject slider, out Text valueText, System.Action<float> onValueChanged)
        {
            // Row container
            var row = _factory.CreatePanel(parent, new PanelStyle { backgroundColor = Color.clear });
            var rowRect = row.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.05f, y - 0.04f);
            rowRect.anchorMax = new Vector2(0.95f, y + 0.04f);
            rowRect.offsetMin = Vector2.zero;
            rowRect.offsetMax = Vector2.zero;

            // Label
            var labelObj = _factory.CreateText(row.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(0.25f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Slider
            slider = _factory.CreateSlider(row.transform, 0.8f, onValueChanged);
            var sliderRect = slider.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.27f, 0.2f);
            sliderRect.anchorMax = new Vector2(0.85f, 0.8f);
            sliderRect.offsetMin = Vector2.zero;
            sliderRect.offsetMax = Vector2.zero;

            // Value text
            var valueObj = _factory.CreateText(row.transform, "80%", UIStyles.SmallText);
            valueText = valueObj.GetComponent<Text>();
            var valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.87f, 0);
            valueRect.anchorMax = new Vector2(1, 1);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueText.alignment = TextAnchor.MiddleRight;
        }

        private void CreateToggleRow(Transform parent, string label, float y, out Toggle toggle, System.Action<bool> onValueChanged)
        {
            // Row container
            var row = _factory.CreatePanel(parent, new PanelStyle { backgroundColor = Color.clear });
            var rowRect = row.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.05f, y - 0.04f);
            rowRect.anchorMax = new Vector2(0.95f, y + 0.04f);
            rowRect.offsetMin = Vector2.zero;
            rowRect.offsetMax = Vector2.zero;

            // Label
            var labelObj = _factory.CreateText(row.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(0.7f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Toggle
            var toggleObj = new GameObject("Toggle");
            toggleObj.transform.SetParent(row.transform, false);
            toggle = toggleObj.AddComponent<Toggle>();

            var toggleRect = toggleObj.AddComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0.8f, 0.1f);
            toggleRect.anchorMax = new Vector2(0.95f, 0.9f);
            toggleRect.offsetMin = Vector2.zero;
            toggleRect.offsetMax = Vector2.zero;

            // Background
            var bg = new GameObject("Background");
            bg.transform.SetParent(toggleObj.transform, false);
            var bgImage = bg.AddComponent<Image>();
            bgImage.color = UIStyles.CardBackgroundColor;
            var bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Checkmark
            var checkmark = new GameObject("Checkmark");
            checkmark.transform.SetParent(bg.transform, false);
            var checkImage = checkmark.AddComponent<Image>();
            checkImage.color = UIStyles.PrimaryColor;
            var checkRect = checkmark.GetComponent<RectTransform>();
            checkRect.anchorMin = new Vector2(0.1f, 0.1f);
            checkRect.anchorMax = new Vector2(0.9f, 0.9f);
            checkRect.offsetMin = Vector2.zero;
            checkRect.offsetMax = Vector2.zero;

            toggle.targetGraphic = bgImage;
            toggle.graphic = checkImage;
            toggle.isOn = true;

            toggle.onValueChanged.AddListener(value => onValueChanged?.Invoke(value));
        }

        private void LoadSettings()
        {
            var settings = GameManager.Instance.Settings;

            // Slider değerlerini ayarla
            if (_masterVolumeSlider != null)
            {
                var slider = _masterVolumeSlider.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.value = settings.MasterVolume;
                    _masterValueText.text = $"{Mathf.RoundToInt(settings.MasterVolume * 100)}%";
                }
            }

            if (_musicVolumeSlider != null)
            {
                var slider = _musicVolumeSlider.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.value = settings.MusicVolume;
                    _musicValueText.text = $"{Mathf.RoundToInt(settings.MusicVolume * 100)}%";
                }
            }

            if (_sfxVolumeSlider != null)
            {
                var slider = _sfxVolumeSlider.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.value = settings.SFXVolume;
                    _sfxValueText.text = $"{Mathf.RoundToInt(settings.SFXVolume * 100)}%";
                }
            }

            // Toggle değerlerini ayarla
            if (_autoSaveToggle != null)
            {
                _autoSaveToggle.isOn = settings.AutoSaveEnabled;
            }

            if (_notificationsToggle != null)
            {
                _notificationsToggle.isOn = settings.NotificationsEnabled;
            }
        }

        private void SaveSettings()
        {
            // Ayarlar otomatik olarak kaydediliyor (callback'lerde)
            PlayerPrefs.SetFloat("MasterVolume", GameManager.Instance.Settings.MasterVolume);
            PlayerPrefs.SetFloat("MusicVolume", GameManager.Instance.Settings.MusicVolume);
            PlayerPrefs.SetFloat("SFXVolume", GameManager.Instance.Settings.SFXVolume);
            PlayerPrefs.SetInt("AutoSave", GameManager.Instance.Settings.AutoSaveEnabled ? 1 : 0);
            PlayerPrefs.SetInt("Notifications", GameManager.Instance.Settings.NotificationsEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
