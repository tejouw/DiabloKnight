using UnityEngine;
using UnityEngine.UI;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Ayarlar ekranı kontrolcüsü - Ses, bildirim ve oyun ayarlarını yönetir.
    /// </summary>
    public class SettingsScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private GameSettings _settings;

        // UI referansları
        private GameObject _masterVolumeSlider;
        private GameObject _musicVolumeSlider;
        private GameObject _sfxVolumeSlider;
        private Text _masterVolumeText;
        private Text _musicVolumeText;
        private Text _sfxVolumeText;

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            _settings = GameManager.Instance.Settings;
            BuildUI();
        }

        private void BuildUI()
        {
            // ScrollView
            var scrollView = _factory.CreateScrollView(transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 0.9f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var content = scrollView.transform.Find("Viewport/Content");

            // Ses Ayarları Bölümü
            AddSectionTitle(content, "Ses Ayarları");

            // Master Volume
            CreateVolumeSlider(content, "Ana Ses", _settings.MasterVolume, (value) =>
            {
                _settings.MasterVolume = value;
                if (_masterVolumeText != null)
                    _masterVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
                SaveSettings();
            }, out _masterVolumeSlider, out _masterVolumeText);

            // Music Volume
            CreateVolumeSlider(content, "Müzik", _settings.MusicVolume, (value) =>
            {
                _settings.MusicVolume = value;
                if (_musicVolumeText != null)
                    _musicVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
                SaveSettings();
            }, out _musicVolumeSlider, out _musicVolumeText);

            // SFX Volume
            CreateVolumeSlider(content, "Efektler", _settings.SFXVolume, (value) =>
            {
                _settings.SFXVolume = value;
                if (_sfxVolumeText != null)
                    _sfxVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
                SaveSettings();
            }, out _sfxVolumeSlider, out _sfxVolumeText);

            // Oyun Ayarları Bölümü
            AddSectionTitle(content, "Oyun Ayarları");

            // Auto-Save Toggle
            CreateToggleOption(content, "Otomatik Kayıt", _settings.AutoSaveEnabled, (enabled) =>
            {
                _settings.AutoSaveEnabled = enabled;
                SaveSettings();
            });

            // Notifications Toggle
            CreateToggleOption(content, "Bildirimler", _settings.NotificationsEnabled, (enabled) =>
            {
                _settings.NotificationsEnabled = enabled;
                SaveSettings();
            });

            // Tehlikeli İşlemler Bölümü
            AddSectionTitle(content, "Veri Yönetimi");

            // Tüm Kayıtları Sil
            var deleteAllButton = _factory.CreateButton(content, "Tüm Kayıtları Sil", () =>
            {
                UIManager.Instance.ShowConfirmation(
                    "Tüm kayıtlı oyunlar silinecek. Bu işlem geri alınamaz!\n\nEmin misiniz?",
                    () =>
                    {
                        SaveManager.Instance?.DeleteAllSaves();
                        UIManager.Instance.ShowInfo("Başarılı", "Tüm kayıtlar silindi.");
                    }
                );
            }, UIStyles.DangerButton);
            var deleteLayout = deleteAllButton.AddComponent<LayoutElement>();
            deleteLayout.minHeight = 60;
            deleteLayout.preferredHeight = 60;

            // Ayarları Sıfırla
            var resetButton = _factory.CreateButton(content, "Ayarları Sıfırla", () =>
            {
                UIManager.Instance.ShowConfirmation(
                    "Tüm ayarlar varsayılan değerlere döndürülecek.\n\nEmin misiniz?",
                    () =>
                    {
                        ResetToDefaults();
                        UIManager.Instance.ShowInfo("Başarılı", "Ayarlar sıfırlandı.");
                        // UI'ı yeniden oluştur
                        RefreshUI();
                    }
                );
            }, UIStyles.SecondaryButton);
            var resetLayout = resetButton.AddComponent<LayoutElement>();
            resetLayout.minHeight = 60;
            resetLayout.preferredHeight = 60;

            // Boşluk ekle
            AddSpacer(content, 50);
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
            System.Action<float> onValueChanged, out GameObject sliderObj, out Text valueText)
        {
            // Container
            var container = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var containerLayout = container.AddComponent<LayoutElement>();
            containerLayout.minHeight = 80;
            containerLayout.preferredHeight = 80;

            // Label
            var labelObj = _factory.CreateText(container.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0.55f);
            labelRect.anchorMax = new Vector2(0.5f, 0.95f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Value text
            var valueObj = _factory.CreateText(container.transform, Mathf.RoundToInt(initialValue * 100) + "%", UIStyles.BodyText);
            var valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.85f, 0.55f);
            valueRect.anchorMax = new Vector2(0.95f, 0.95f);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueText = valueObj.GetComponent<Text>();
            valueText.alignment = TextAnchor.MiddleRight;

            // Slider
            sliderObj = _factory.CreateSlider(container.transform, 0f, 1f, initialValue, onValueChanged);
            var sliderRect = sliderObj.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.05f, 0.1f);
            sliderRect.anchorMax = new Vector2(0.95f, 0.5f);
            sliderRect.offsetMin = Vector2.zero;
            sliderRect.offsetMax = Vector2.zero;
        }

        private void CreateToggleOption(Transform parent, string label, bool initialValue, System.Action<bool> onValueChanged)
        {
            // Container
            var container = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var containerLayout = container.AddComponent<LayoutElement>();
            containerLayout.minHeight = 70;
            containerLayout.preferredHeight = 70;

            // Label
            var labelObj = _factory.CreateText(container.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0.1f);
            labelRect.anchorMax = new Vector2(0.6f, 0.9f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Toggle butonları
            var currentValue = initialValue;

            var toggleButton = _factory.CreateButton(container.transform, initialValue ? "Açık" : "Kapalı", () =>
            {
                currentValue = !currentValue;
                onValueChanged(currentValue);
            }, initialValue ? UIStyles.PrimaryButton : UIStyles.SecondaryButton);

            var toggleRect = toggleButton.GetComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0.65f, 0.15f);
            toggleRect.anchorMax = new Vector2(0.95f, 0.85f);
            toggleRect.offsetMin = Vector2.zero;
            toggleRect.offsetMax = Vector2.zero;

            // Toggle değiştiğinde buton metnini ve rengini güncelle
            var button = toggleButton.GetComponent<Button>();
            var buttonImage = toggleButton.GetComponent<Image>();
            var buttonText = toggleButton.GetComponentInChildren<Text>();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                currentValue = !currentValue;
                buttonText.text = currentValue ? "Açık" : "Kapalı";
                buttonImage.color = currentValue ? UIStyles.PrimaryColor : new Color(0.4f, 0.4f, 0.45f, 1f);
                onValueChanged(currentValue);
            });
        }

        private void AddSpacer(Transform parent, float height)
        {
            var spacer = new GameObject("Spacer");
            spacer.transform.SetParent(parent, false);
            var rect = spacer.AddComponent<RectTransform>();
            var layout = spacer.AddComponent<LayoutElement>();
            layout.minHeight = height;
            layout.preferredHeight = height;
        }

        private void SaveSettings()
        {
            // Ayarları PlayerPrefs'e kaydet
            PlayerPrefs.SetFloat("Settings_MasterVolume", _settings.MasterVolume);
            PlayerPrefs.SetFloat("Settings_MusicVolume", _settings.MusicVolume);
            PlayerPrefs.SetFloat("Settings_SFXVolume", _settings.SFXVolume);
            PlayerPrefs.SetInt("Settings_AutoSave", _settings.AutoSaveEnabled ? 1 : 0);
            PlayerPrefs.SetInt("Settings_Notifications", _settings.NotificationsEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void ResetToDefaults()
        {
            _settings.MasterVolume = 1f;
            _settings.MusicVolume = 0.8f;
            _settings.SFXVolume = 1f;
            _settings.AutoSaveEnabled = true;
            _settings.NotificationsEnabled = true;
            SaveSettings();
        }

        private void RefreshUI()
        {
            // Mevcut UI'ı temizle
            foreach (Transform child in transform)
            {
                // Geri butonu ve başlığı koruyalım (ilk iki child)
                if (child.GetSiblingIndex() >= 2)
                {
                    Destroy(child.gameObject);
                }
            }
            // UI'ı yeniden oluştur
            BuildUI();
        }
    }
}
