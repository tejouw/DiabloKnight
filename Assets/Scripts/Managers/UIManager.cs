using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.UI;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// UI Yöneticisi - Tüm UI elemanlarını kod ile oluşturur ve yönetir.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        // Ana canvas referansları
        private Canvas _mainCanvas;
        private Canvas _popupCanvas;
        private CanvasScaler _canvasScaler;

        // Aktif ekranlar
        private Dictionary<ScreenType, GameObject> _screens = new Dictionary<ScreenType, GameObject>();
        private ScreenType _currentScreen = ScreenType.None;

        // Popup stack
        private Stack<GameObject> _popupStack = new Stack<GameObject>();

        // UI Factory instance
        private UIFactory _factory;

        #region Properties

        public Canvas MainCanvas => _mainCanvas;
        public Canvas PopupCanvas => _popupCanvas;
        public UIFactory Factory => _factory;
        public ScreenType CurrentScreen => _currentScreen;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Ana canvas oluştur
            CreateMainCanvas();

            // Popup canvas oluştur
            CreatePopupCanvas();

            // UI Factory başlat
            _factory = new UIFactory(_mainCanvas.transform);

            Debug.Log("[UIManager] Initialized successfully.");
        }

        private void CreateMainCanvas()
        {
            // Canvas GameObject
            GameObject canvasObject = new GameObject("MainCanvas");
            canvasObject.transform.SetParent(transform);

            // Canvas component
            _mainCanvas = canvasObject.AddComponent<Canvas>();
            _mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _mainCanvas.sortingOrder = 0;

            // Canvas Scaler
            _canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1080, 1920);
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            _canvasScaler.matchWidthOrHeight = 0.5f;

            // Graphic Raycaster
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        private void CreatePopupCanvas()
        {
            // Popup Canvas GameObject
            GameObject popupCanvasObject = new GameObject("PopupCanvas");
            popupCanvasObject.transform.SetParent(transform);

            // Canvas component
            _popupCanvas = popupCanvasObject.AddComponent<Canvas>();
            _popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _popupCanvas.sortingOrder = 100;

            // Canvas Scaler
            var popupScaler = popupCanvasObject.AddComponent<CanvasScaler>();
            popupScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            popupScaler.referenceResolution = new Vector2(1080, 1920);
            popupScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            popupScaler.matchWidthOrHeight = 0.5f;

            // Graphic Raycaster
            popupCanvasObject.AddComponent<GraphicRaycaster>();
        }

        #endregion

        #region Screen Management

        /// <summary>
        /// Belirtilen ekranı göster.
        /// </summary>
        public void ShowScreen(ScreenType screenType)
        {
            if (_currentScreen == screenType) return;

            // Önceki ekranı gizle
            HideCurrentScreen();

            // Yeni ekranı göster veya oluştur
            if (!_screens.ContainsKey(screenType))
            {
                CreateScreen(screenType);
            }

            if (_screens.ContainsKey(screenType))
            {
                _screens[screenType].SetActive(true);
            }

            var oldScreen = _currentScreen;
            _currentScreen = screenType;

            // Event yayınla
            EventBus.Publish(new ScreenChangedEvent
            {
                OldScreen = oldScreen,
                NewScreen = screenType
            });

            Debug.Log($"[UIManager] Screen changed to {screenType}");
        }

        /// <summary>
        /// Mevcut ekranı gizle.
        /// </summary>
        public void HideCurrentScreen()
        {
            if (_currentScreen != ScreenType.None && _screens.ContainsKey(_currentScreen))
            {
                _screens[_currentScreen].SetActive(false);
            }
        }

        /// <summary>
        /// Ekran oluştur.
        /// </summary>
        private void CreateScreen(ScreenType screenType)
        {
            GameObject screen = null;

            switch (screenType)
            {
                case ScreenType.MainMenu:
                    screen = CreateMainMenuScreen();
                    break;
                case ScreenType.Game:
                    screen = CreateGameScreen();
                    break;
                case ScreenType.Profile:
                    screen = CreateProfileScreen();
                    break;
                case ScreenType.Relationships:
                    screen = CreateRelationshipsScreen();
                    break;
                case ScreenType.Settings:
                    screen = CreateSettingsScreen();
                    break;
                case ScreenType.SaveLoad:
                    screen = CreateSaveLoadScreen();
                    break;
                case ScreenType.Death:
                    screen = CreateDeathScreen();
                    break;
            }

            if (screen != null)
            {
                _screens[screenType] = screen;
            }
        }

        #endregion

        #region Screen Creators

        private GameObject CreateMainMenuScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "MainMenuScreen";

            // Logo/Başlık
            var title = _factory.CreateText(screen.transform, "TÜRK HAYATI", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.1f, 0.7f);
            titleRect.anchorMax = new Vector2(0.9f, 0.85f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // Alt başlık
            var subtitle = _factory.CreateText(screen.transform, "Hayat Simülasyonu", UIStyles.SubtitleText);
            var subtitleRect = subtitle.GetComponent<RectTransform>();
            subtitleRect.anchorMin = new Vector2(0.1f, 0.62f);
            subtitleRect.anchorMax = new Vector2(0.9f, 0.7f);
            subtitleRect.offsetMin = Vector2.zero;
            subtitleRect.offsetMax = Vector2.zero;

            // Butonlar container
            float buttonY = 0.5f;
            float buttonSpacing = 0.08f;

            // Yeni Oyun butonu
            CreateMenuButton(screen.transform, "Yeni Oyun", buttonY, () => GameManager.Instance.StartNewGame());
            buttonY -= buttonSpacing;

            // Devam Et butonu
            CreateMenuButton(screen.transform, "Devam Et", buttonY, () => ShowScreen(ScreenType.SaveLoad));
            buttonY -= buttonSpacing;

            // Ayarlar butonu
            CreateMenuButton(screen.transform, "Ayarlar", buttonY, () => ShowScreen(ScreenType.Settings));
            buttonY -= buttonSpacing;

            // Çıkış butonu
            CreateMenuButton(screen.transform, "Çıkış", buttonY, () => GameManager.Instance.QuitGame());

            return screen;
        }

        private void CreateMenuButton(Transform parent, string text, float yPosition, Action onClick)
        {
            var button = _factory.CreateButton(parent, text, onClick, UIStyles.PrimaryButton);
            var rect = button.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.2f, yPosition - 0.03f);
            rect.anchorMax = new Vector2(0.8f, yPosition + 0.03f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private GameObject CreateGameScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "GameScreen";

            // Bu ekran GameScreenController tarafından yönetilecek
            screen.AddComponent<GameScreenController>();

            return screen;
        }

        private GameObject CreateProfileScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "ProfileScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.Game), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "Profil", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // ProfileScreenController ekle
            screen.AddComponent<ProfileScreenController>();

            return screen;
        }

        private GameObject CreateRelationshipsScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "RelationshipsScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.Game), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "İlişkiler", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // RelationshipsScreenController ekle
            screen.AddComponent<RelationshipsScreenController>();

            return screen;
        }

        private GameObject CreateSettingsScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "SettingsScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.MainMenu), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "Ayarlar", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // ScrollView oluştur
            var scrollView = _factory.CreateScrollView(screen.transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.05f, 0.08f);
            scrollRect.anchorMax = new Vector2(0.95f, 0.9f);
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            var content = scrollView.transform.Find("Viewport/Content");

            // Ses Ayarları Başlığı
            CreateSettingsSectionTitle(content, "Ses Ayarları");

            // Master Volume
            CreateVolumeSlider(content, "Ana Ses", AudioManager.Instance.MasterVolume, (value) => {
                AudioManager.Instance.MasterVolume = value;
            });

            // Music Volume
            CreateVolumeSlider(content, "Müzik", AudioManager.Instance.MusicVolume, (value) => {
                AudioManager.Instance.MusicVolume = value;
            });

            // SFX Volume
            CreateVolumeSlider(content, "Efektler", AudioManager.Instance.SFXVolume, (value) => {
                AudioManager.Instance.SFXVolume = value;
            });

            // Oyun Ayarları Başlığı
            CreateSettingsSectionTitle(content, "Oyun Ayarları");

            // Auto-Save Toggle
            CreateToggleSetting(content, "Otomatik Kayıt", GameManager.Instance.Settings.AutoSaveEnabled, (value) => {
                GameManager.Instance.Settings.AutoSaveEnabled = value;
            });

            // Notifications Toggle
            CreateToggleSetting(content, "Bildirimler", GameManager.Instance.Settings.NotificationsEnabled, (value) => {
                GameManager.Instance.Settings.NotificationsEnabled = value;
            });

            // Boşluk
            CreateSettingsSpacer(content);

            // Kaydet butonu
            var saveButtonContainer = new GameObject("SaveButtonContainer");
            saveButtonContainer.transform.SetParent(content, false);
            var saveContainerRect = saveButtonContainer.AddComponent<RectTransform>();
            saveContainerRect.sizeDelta = new Vector2(0, 80);
            var saveContainerLayout = saveButtonContainer.AddComponent<LayoutElement>();
            saveContainerLayout.preferredHeight = 80;

            var saveButton = _factory.CreateButton(saveButtonContainer.transform, "Kaydet", () => {
                AudioManager.Instance.SaveSettings();
                ShowInfo("Ayarlar", "Ayarlar başarıyla kaydedildi.");
            }, UIStyles.PrimaryButton);
            var saveButtonRect = saveButton.GetComponent<RectTransform>();
            saveButtonRect.anchorMin = new Vector2(0.2f, 0.1f);
            saveButtonRect.anchorMax = new Vector2(0.8f, 0.9f);
            saveButtonRect.offsetMin = Vector2.zero;
            saveButtonRect.offsetMax = Vector2.zero;

            // Varsayılana Dön butonu
            var resetButtonContainer = new GameObject("ResetButtonContainer");
            resetButtonContainer.transform.SetParent(content, false);
            var resetContainerRect = resetButtonContainer.AddComponent<RectTransform>();
            resetContainerRect.sizeDelta = new Vector2(0, 70);
            var resetContainerLayout = resetButtonContainer.AddComponent<LayoutElement>();
            resetContainerLayout.preferredHeight = 70;

            var resetButton = _factory.CreateButton(resetButtonContainer.transform, "Varsayılana Dön", () => {
                ShowConfirmation("Tüm ayarlar varsayılan değerlere döndürülsün mü?", () => {
                    AudioManager.Instance.MasterVolume = 1f;
                    AudioManager.Instance.MusicVolume = 0.8f;
                    AudioManager.Instance.SFXVolume = 1f;
                    GameManager.Instance.Settings.AutoSaveEnabled = true;
                    GameManager.Instance.Settings.NotificationsEnabled = true;
                    AudioManager.Instance.SaveSettings();
                    // Ekranı yeniden oluştur
                    if (_screens.ContainsKey(ScreenType.Settings))
                    {
                        Destroy(_screens[ScreenType.Settings]);
                        _screens.Remove(ScreenType.Settings);
                    }
                    ShowScreen(ScreenType.Settings);
                });
            }, UIStyles.SecondaryButton);
            var resetButtonRect = resetButton.GetComponent<RectTransform>();
            resetButtonRect.anchorMin = new Vector2(0.25f, 0.1f);
            resetButtonRect.anchorMax = new Vector2(0.75f, 0.9f);
            resetButtonRect.offsetMin = Vector2.zero;
            resetButtonRect.offsetMax = Vector2.zero;

            return screen;
        }

        private void CreateSettingsSectionTitle(Transform parent, string titleText)
        {
            var container = new GameObject("SectionTitle");
            container.transform.SetParent(parent, false);
            var containerRect = container.AddComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(0, 50);
            var layout = container.AddComponent<LayoutElement>();
            layout.preferredHeight = 50;

            var title = _factory.CreateText(container.transform, titleText, UIStyles.SubtitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = Vector2.zero;
            titleRect.anchorMax = Vector2.one;
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;
            title.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            title.GetComponent<Text>().color = UIStyles.PrimaryColor;
        }

        private void CreateVolumeSlider(Transform parent, string label, float initialValue, Action<float> onValueChanged)
        {
            var container = new GameObject("VolumeSlider_" + label);
            container.transform.SetParent(parent, false);
            var containerRect = container.AddComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(0, 70);
            var layout = container.AddComponent<LayoutElement>();
            layout.preferredHeight = 70;

            // Label
            var labelObj = _factory.CreateText(container.transform, label, UIStyles.StatLabelText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0.5f);
            labelRect.anchorMax = new Vector2(0.3f, 1f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            // Value Text
            var valueText = _factory.CreateText(container.transform, Mathf.RoundToInt(initialValue * 100) + "%", UIStyles.StatLabelText);
            var valueRect = valueText.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.85f, 0.5f);
            valueRect.anchorMax = new Vector2(1f, 1f);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueText.GetComponent<Text>().alignment = TextAnchor.MiddleRight;

            // Slider
            var slider = _factory.CreateSlider(container.transform, 0f, 1f, initialValue, (value) => {
                onValueChanged?.Invoke(value);
                valueText.GetComponent<Text>().text = Mathf.RoundToInt(value * 100) + "%";
            });
            var sliderRect = slider.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.05f, 0f);
            sliderRect.anchorMax = new Vector2(0.95f, 0.5f);
            sliderRect.offsetMin = Vector2.zero;
            sliderRect.offsetMax = Vector2.zero;
        }

        private void CreateToggleSetting(Transform parent, string label, bool initialValue, Action<bool> onValueChanged)
        {
            var container = new GameObject("Toggle_" + label);
            container.transform.SetParent(parent, false);
            var containerRect = container.AddComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(0, 60);
            var layout = container.AddComponent<LayoutElement>();
            layout.preferredHeight = 60;

            // Label
            var labelObj = _factory.CreateText(container.transform, label, UIStyles.StatLabelText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(0.6f, 1f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            // Toggle Buttons Container
            var toggleContainer = new GameObject("ToggleButtons");
            toggleContainer.transform.SetParent(container.transform, false);
            var toggleRect = toggleContainer.AddComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0.6f, 0.1f);
            toggleRect.anchorMax = new Vector2(1f, 0.9f);
            toggleRect.offsetMin = Vector2.zero;
            toggleRect.offsetMax = Vector2.zero;

            bool currentValue = initialValue;
            GameObject onButton = null;
            GameObject offButton = null;

            Action updateButtonColors = () => {
                if (onButton != null && offButton != null)
                {
                    onButton.GetComponent<Image>().color = currentValue ? UIStyles.AccentColor : new Color(0.3f, 0.3f, 0.35f, 1f);
                    offButton.GetComponent<Image>().color = currentValue ? new Color(0.3f, 0.3f, 0.35f, 1f) : UIStyles.SecondaryColor;
                }
            };

            // On Button
            onButton = _factory.CreateButton(toggleContainer.transform, "Açık", () => {
                currentValue = true;
                onValueChanged?.Invoke(true);
                updateButtonColors();
            }, UIStyles.SecondaryButton);
            var onRect = onButton.GetComponent<RectTransform>();
            onRect.anchorMin = new Vector2(0, 0);
            onRect.anchorMax = new Vector2(0.48f, 1f);
            onRect.offsetMin = Vector2.zero;
            onRect.offsetMax = Vector2.zero;

            // Off Button
            offButton = _factory.CreateButton(toggleContainer.transform, "Kapalı", () => {
                currentValue = false;
                onValueChanged?.Invoke(false);
                updateButtonColors();
            }, UIStyles.SecondaryButton);
            var offRect = offButton.GetComponent<RectTransform>();
            offRect.anchorMin = new Vector2(0.52f, 0);
            offRect.anchorMax = new Vector2(1f, 1f);
            offRect.offsetMin = Vector2.zero;
            offRect.offsetMax = Vector2.zero;

            updateButtonColors();
        }

        private void CreateSettingsSpacer(Transform parent)
        {
            var spacer = new GameObject("Spacer");
            spacer.transform.SetParent(parent, false);
            var spacerRect = spacer.AddComponent<RectTransform>();
            spacerRect.sizeDelta = new Vector2(0, 30);
            var layout = spacer.AddComponent<LayoutElement>();
            layout.preferredHeight = 30;
        }

        private GameObject CreateSaveLoadScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "SaveLoadScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.MainMenu), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "Kayıtlı Oyunlar", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // ScrollView oluştur
            var scrollView = _factory.CreateScrollView(screen.transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.05f, 0.08f);
            scrollRect.anchorMax = new Vector2(0.95f, 0.9f);
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            var content = scrollView.transform.Find("Viewport/Content");

            // Auto-save slot (varsa)
            if (SaveManager.Instance.HasAutoSave())
            {
                var autoSaveData = SaveManager.Instance.LoadAutoSave();
                if (autoSaveData != null)
                {
                    CreateSaveSlotCard(content, -1, autoSaveData.characterName, autoSaveData.characterAge, autoSaveData.saveDate, true);
                }
            }

            // Normal save slotları
            var saveSlots = SaveManager.Instance.GetAllSaveSlots();
            foreach (var slot in saveSlots)
            {
                if (slot.isEmpty)
                {
                    CreateEmptySaveSlotCard(content, slot.slotIndex);
                }
                else
                {
                    CreateSaveSlotCard(content, slot.slotIndex, slot.characterName, slot.characterAge, slot.saveDate, false);
                }
            }

            return screen;
        }

        private void CreateSaveSlotCard(Transform parent, int slotIndex, string characterName, int characterAge, string saveDate, bool isAutoSave)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            card.name = isAutoSave ? "AutoSaveSlot" : $"SaveSlot_{slotIndex}";
            var cardRect = card.GetComponent<RectTransform>();
            cardRect.sizeDelta = new Vector2(0, 140);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.preferredHeight = 140;

            // Slot başlığı (Auto-save veya Slot numarası)
            string slotTitle = isAutoSave ? "Otomatik Kayıt" : $"Kayıt {slotIndex + 1}";
            var slotTitleObj = _factory.CreateText(card.transform, slotTitle, UIStyles.StatLabelText);
            var slotTitleRect = slotTitleObj.GetComponent<RectTransform>();
            slotTitleRect.anchorMin = new Vector2(0.05f, 0.7f);
            slotTitleRect.anchorMax = new Vector2(0.6f, 0.95f);
            slotTitleRect.offsetMin = Vector2.zero;
            slotTitleRect.offsetMax = Vector2.zero;
            slotTitleObj.GetComponent<Text>().color = isAutoSave ? UIStyles.AccentColor : UIStyles.PrimaryColor;

            // Karakter adı
            var nameObj = _factory.CreateText(card.transform, characterName, UIStyles.BodyText);
            var nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0.05f, 0.4f);
            nameRect.anchorMax = new Vector2(0.6f, 0.7f);
            nameRect.offsetMin = Vector2.zero;
            nameRect.offsetMax = Vector2.zero;
            nameObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Yaş ve tarih
            string infoText = $"Yaş: {characterAge} | {saveDate}";
            var infoObj = _factory.CreateText(card.transform, infoText, UIStyles.SmallText);
            var infoRect = infoObj.GetComponent<RectTransform>();
            infoRect.anchorMin = new Vector2(0.05f, 0.1f);
            infoRect.anchorMax = new Vector2(0.6f, 0.4f);
            infoRect.offsetMin = Vector2.zero;
            infoRect.offsetMax = Vector2.zero;

            // Yükle butonu
            var loadButton = _factory.CreateButton(card.transform, "Yükle", () => {
                ShowConfirmation($"{characterName} karakterini yüklemek istiyor musunuz?", () => {
                    GameManager.Instance.LoadGame(slotIndex);
                });
            }, UIStyles.PrimaryButton);
            var loadRect = loadButton.GetComponent<RectTransform>();
            loadRect.anchorMin = new Vector2(0.62f, 0.5f);
            loadRect.anchorMax = new Vector2(0.95f, 0.9f);
            loadRect.offsetMin = Vector2.zero;
            loadRect.offsetMax = Vector2.zero;

            // Sil butonu (auto-save için de silme imkanı)
            var deleteButton = _factory.CreateButton(card.transform, "Sil", () => {
                ShowConfirmation($"Bu kaydı silmek istediğinizden emin misiniz?", () => {
                    if (isAutoSave)
                    {
                        SaveManager.Instance.DeleteAutoSave();
                    }
                    else
                    {
                        SaveManager.Instance.DeleteSave(slotIndex);
                    }
                    // Ekranı yenile
                    RefreshSaveLoadScreen();
                });
            }, UIStyles.DangerButton);
            var deleteRect = deleteButton.GetComponent<RectTransform>();
            deleteRect.anchorMin = new Vector2(0.62f, 0.1f);
            deleteRect.anchorMax = new Vector2(0.95f, 0.45f);
            deleteRect.offsetMin = Vector2.zero;
            deleteRect.offsetMax = Vector2.zero;
        }

        private void CreateEmptySaveSlotCard(Transform parent, int slotIndex)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            card.name = $"EmptySaveSlot_{slotIndex}";
            var cardRect = card.GetComponent<RectTransform>();
            cardRect.sizeDelta = new Vector2(0, 100);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.preferredHeight = 100;

            // Slot başlığı
            string slotTitle = $"Kayıt {slotIndex + 1}";
            var slotTitleObj = _factory.CreateText(card.transform, slotTitle, UIStyles.StatLabelText);
            var slotTitleRect = slotTitleObj.GetComponent<RectTransform>();
            slotTitleRect.anchorMin = new Vector2(0.05f, 0.5f);
            slotTitleRect.anchorMax = new Vector2(0.5f, 0.9f);
            slotTitleRect.offsetMin = Vector2.zero;
            slotTitleRect.offsetMax = Vector2.zero;
            slotTitleObj.GetComponent<Text>().color = UIStyles.SubtextColor;

            // Boş slot mesajı
            var emptyText = _factory.CreateText(card.transform, "Boş Slot", UIStyles.SmallText);
            var emptyRect = emptyText.GetComponent<RectTransform>();
            emptyRect.anchorMin = new Vector2(0.05f, 0.1f);
            emptyRect.anchorMax = new Vector2(0.5f, 0.5f);
            emptyRect.offsetMin = Vector2.zero;
            emptyRect.offsetMax = Vector2.zero;

            // Kaydet butonu (sadece oyun oynanıyorsa aktif)
            if (GameManager.Instance != null && GameManager.Instance.IsPlaying)
            {
                var saveButton = _factory.CreateButton(card.transform, "Kaydet", () => {
                    ShowConfirmation($"Oyunu Kayıt {slotIndex + 1} slotuna kaydetmek istiyor musunuz?", () => {
                        GameManager.Instance.SaveGame(slotIndex);
                        ShowInfo("Kayıt", "Oyun başarıyla kaydedildi.");
                        // Ekranı yenile
                        RefreshSaveLoadScreen();
                    });
                }, UIStyles.AccentButton);
                var saveRect = saveButton.GetComponent<RectTransform>();
                saveRect.anchorMin = new Vector2(0.55f, 0.15f);
                saveRect.anchorMax = new Vector2(0.95f, 0.85f);
                saveRect.offsetMin = Vector2.zero;
                saveRect.offsetMax = Vector2.zero;
            }
        }

        private void RefreshSaveLoadScreen()
        {
            if (_screens.ContainsKey(ScreenType.SaveLoad))
            {
                Destroy(_screens[ScreenType.SaveLoad]);
                _screens.Remove(ScreenType.SaveLoad);
            }
            ShowScreen(ScreenType.SaveLoad);
        }

        private GameObject CreateDeathScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "DeathScreen";

            // R.I.P başlık
            var title = _factory.CreateText(screen.transform, "Huzur İçinde Yat", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.1f, 0.7f);
            titleRect.anchorMax = new Vector2(0.9f, 0.85f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // Ana Menü butonu
            var menuButton = _factory.CreateButton(screen.transform, "Ana Menü", () => GameManager.Instance.ReturnToMainMenu(), UIStyles.PrimaryButton);
            var menuRect = menuButton.GetComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(0.2f, 0.15f);
            menuRect.anchorMax = new Vector2(0.8f, 0.22f);
            menuRect.offsetMin = Vector2.zero;
            menuRect.offsetMax = Vector2.zero;

            return screen;
        }

        #endregion

        #region Popup Management

        /// <summary>
        /// Onay popup'ı göster.
        /// </summary>
        public void ShowConfirmation(string message, Action onConfirm, Action onCancel = null)
        {
            var popup = CreatePopupBase("Onay");

            // Mesaj
            var messageText = _factory.CreateText(popup.transform, message, UIStyles.BodyText);
            var messageRect = messageText.GetComponent<RectTransform>();
            messageRect.anchorMin = new Vector2(0.1f, 0.4f);
            messageRect.anchorMax = new Vector2(0.9f, 0.7f);
            messageRect.offsetMin = Vector2.zero;
            messageRect.offsetMax = Vector2.zero;

            // Evet butonu
            var yesButton = _factory.CreateButton(popup.transform, "Evet", () =>
            {
                onConfirm?.Invoke();
                ClosePopup();
            }, UIStyles.PrimaryButton);
            var yesRect = yesButton.GetComponent<RectTransform>();
            yesRect.anchorMin = new Vector2(0.1f, 0.15f);
            yesRect.anchorMax = new Vector2(0.45f, 0.28f);
            yesRect.offsetMin = Vector2.zero;
            yesRect.offsetMax = Vector2.zero;

            // Hayır butonu
            var noButton = _factory.CreateButton(popup.transform, "Hayır", () =>
            {
                onCancel?.Invoke();
                ClosePopup();
            }, UIStyles.SecondaryButton);
            var noRect = noButton.GetComponent<RectTransform>();
            noRect.anchorMin = new Vector2(0.55f, 0.15f);
            noRect.anchorMax = new Vector2(0.9f, 0.28f);
            noRect.offsetMin = Vector2.zero;
            noRect.offsetMax = Vector2.zero;

            _popupStack.Push(popup);
        }

        /// <summary>
        /// Bilgi popup'ı göster.
        /// </summary>
        public void ShowInfo(string title, string message)
        {
            var popup = CreatePopupBase(title);

            // Mesaj
            var messageText = _factory.CreateText(popup.transform, message, UIStyles.BodyText);
            var messageRect = messageText.GetComponent<RectTransform>();
            messageRect.anchorMin = new Vector2(0.1f, 0.35f);
            messageRect.anchorMax = new Vector2(0.9f, 0.75f);
            messageRect.offsetMin = Vector2.zero;
            messageRect.offsetMax = Vector2.zero;

            // Tamam butonu
            var okButton = _factory.CreateButton(popup.transform, "Tamam", ClosePopup, UIStyles.PrimaryButton);
            var okRect = okButton.GetComponent<RectTransform>();
            okRect.anchorMin = new Vector2(0.25f, 0.12f);
            okRect.anchorMax = new Vector2(0.75f, 0.25f);
            okRect.offsetMin = Vector2.zero;
            okRect.offsetMax = Vector2.zero;

            _popupStack.Push(popup);
        }

        /// <summary>
        /// Olay sonucu popup'ı göster.
        /// </summary>
        public void ShowEventResult(string resultText, Action onClose = null)
        {
            var popup = CreatePopupBase("Sonuç");

            // Sonuç metni
            var text = _factory.CreateText(popup.transform, resultText, UIStyles.BodyText);
            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.1f, 0.35f);
            textRect.anchorMax = new Vector2(0.9f, 0.75f);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // Devam butonu
            var continueButton = _factory.CreateButton(popup.transform, "Devam", () =>
            {
                ClosePopup();
                onClose?.Invoke();
            }, UIStyles.PrimaryButton);
            var continueRect = continueButton.GetComponent<RectTransform>();
            continueRect.anchorMin = new Vector2(0.25f, 0.12f);
            continueRect.anchorMax = new Vector2(0.75f, 0.25f);
            continueRect.offsetMin = Vector2.zero;
            continueRect.offsetMax = Vector2.zero;

            _popupStack.Push(popup);
        }

        private GameObject CreatePopupBase(string title)
        {
            // Arkaplan overlay
            var overlay = _factory.CreatePanel(_popupCanvas.transform, UIStyles.PopupOverlay);
            overlay.name = "PopupOverlay";

            // Popup panel
            var popup = _factory.CreatePanel(overlay.transform, UIStyles.PopupPanel);
            var popupRect = popup.GetComponent<RectTransform>();
            popupRect.anchorMin = new Vector2(0.1f, 0.3f);
            popupRect.anchorMax = new Vector2(0.9f, 0.7f);
            popupRect.offsetMin = Vector2.zero;
            popupRect.offsetMax = Vector2.zero;

            // Başlık
            var titleText = _factory.CreateText(popup.transform, title, UIStyles.SubtitleText);
            var titleRect = titleText.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.1f, 0.8f);
            titleRect.anchorMax = new Vector2(0.9f, 0.95f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            return overlay;
        }

        /// <summary>
        /// En üstteki popup'ı kapat.
        /// </summary>
        public void ClosePopup()
        {
            if (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                Destroy(popup);
            }
        }

        /// <summary>
        /// Tüm popup'ları kapat.
        /// </summary>
        public void CloseAllPopups()
        {
            while (_popupStack.Count > 0)
            {
                ClosePopup();
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// UI'ı yenile (karakter verisi değiştiğinde çağırılır).
        /// </summary>
        public void RefreshUI()
        {
            // Mevcut ekran varsa yenile
            if (_currentScreen == ScreenType.Game)
            {
                var gameScreen = _screens[ScreenType.Game];
                var controller = gameScreen.GetComponent<GameScreenController>();
                controller?.RefreshUI();
            }
        }

        #endregion
    }
}
