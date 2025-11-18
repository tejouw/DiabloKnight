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

        // Death screen data
        private string _lastDeathCause;
        private string _lastEpitaph;
        private int _lastDeathAge;

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

            // Subscribe to events
            EventBus.Subscribe<CharacterDiedEvent>(OnCharacterDied);
            EventBus.Subscribe<GameSavedEvent>(OnGameSaved);
            EventBus.Subscribe<GameLoadedEvent>(OnGameLoaded);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<CharacterDiedEvent>(OnCharacterDied);
            EventBus.Unsubscribe<GameSavedEvent>(OnGameSaved);
            EventBus.Unsubscribe<GameLoadedEvent>(OnGameLoaded);
        }

        private void OnCharacterDied(CharacterDiedEvent evt)
        {
            _lastDeathAge = evt.Age;
            _lastDeathCause = evt.DeathCause;
            _lastEpitaph = evt.Epitaph;
        }

        private void OnGameSaved(GameSavedEvent evt)
        {
            ShowInfo("Kayıt Başarılı", $"Oyun Slot {evt.SlotIndex + 1}'e kaydedildi.");
        }

        private void OnGameLoaded(GameLoadedEvent evt)
        {
            // Close any open popups when game is loaded
            CloseAllPopups();
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

            return screen;
        }

        private GameObject CreateSettingsScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "SettingsScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () =>
            {
                AudioManager.Instance?.SaveSettings();
                ShowScreen(ScreenType.MainMenu);
            }, UIStyles.SecondaryButton);
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

            // Ses Ayarları başlığı
            var audioTitle = _factory.CreateText(screen.transform, "Ses Ayarları", UIStyles.SubtitleText);
            var audioTitleRect = audioTitle.GetComponent<RectTransform>();
            audioTitleRect.anchorMin = new Vector2(0.1f, 0.78f);
            audioTitleRect.anchorMax = new Vector2(0.9f, 0.85f);
            audioTitleRect.offsetMin = Vector2.zero;
            audioTitleRect.offsetMax = Vector2.zero;

            float sliderY = 0.68f;
            float sliderSpacing = 0.12f;

            // Master Volume
            CreateVolumeSlider(screen.transform, "Ana Ses", sliderY,
                AudioManager.Instance?.MasterVolume ?? 1f,
                (value) => { if (AudioManager.Instance != null) AudioManager.Instance.MasterVolume = value; });
            sliderY -= sliderSpacing;

            // Music Volume
            CreateVolumeSlider(screen.transform, "Müzik", sliderY,
                AudioManager.Instance?.MusicVolume ?? 0.8f,
                (value) => { if (AudioManager.Instance != null) AudioManager.Instance.MusicVolume = value; });
            sliderY -= sliderSpacing;

            // SFX Volume
            CreateVolumeSlider(screen.transform, "Efektler", sliderY,
                AudioManager.Instance?.SFXVolume ?? 1f,
                (value) => { if (AudioManager.Instance != null) AudioManager.Instance.SFXVolume = value; });

            return screen;
        }

        private void CreateVolumeSlider(Transform parent, string label, float yPosition, float initialValue, Action<float> onValueChanged)
        {
            // Label
            var labelObj = _factory.CreateText(parent, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.1f, yPosition - 0.02f);
            labelRect.anchorMax = new Vector2(0.3f, yPosition + 0.04f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Slider
            var slider = _factory.CreateSlider(parent, 0f, 1f, initialValue, onValueChanged);
            var sliderRect = slider.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.32f, yPosition - 0.01f);
            sliderRect.anchorMax = new Vector2(0.78f, yPosition + 0.03f);
            sliderRect.offsetMin = Vector2.zero;
            sliderRect.offsetMax = Vector2.zero;

            // Value text
            var valueObj = _factory.CreateText(parent, $"{(int)(initialValue * 100)}%", UIStyles.BodyText);
            var valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.8f, yPosition - 0.02f);
            valueRect.anchorMax = new Vector2(0.95f, yPosition + 0.04f);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            var valueText = valueObj.GetComponent<Text>();
            valueText.alignment = TextAnchor.MiddleRight;

            // Update value text when slider changes
            var sliderComponent = slider.GetComponent<Slider>();
            sliderComponent.onValueChanged.AddListener((value) =>
            {
                valueText.text = $"{(int)(value * 100)}%";
            });
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

            // ScrollView for save slots
            var scrollView = _factory.CreateScrollView(screen.transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0.05f);
            scrollRect.anchorMax = new Vector2(1, 0.9f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var content = scrollView.transform.Find("Viewport/Content");

            // Get all save slots
            var saveSlots = SaveManager.Instance?.GetAllSaveSlots();
            if (saveSlots != null)
            {
                foreach (var slot in saveSlots)
                {
                    CreateSaveSlotCard(content, slot);
                }
            }

            // Auto-save slot
            if (SaveManager.Instance?.HasAutoSave() == true)
            {
                var autoSaveCard = _factory.CreatePanel(content, UIStyles.CardPanel);
                var autoSaveLayout = autoSaveCard.AddComponent<LayoutElement>();
                autoSaveLayout.minHeight = 100;
                autoSaveLayout.preferredHeight = 100;

                // Auto-save label
                var autoLabel = _factory.CreateText(autoSaveCard.transform, "Otomatik Kayıt", UIStyles.SubtitleText);
                var autoLabelRect = autoLabel.GetComponent<RectTransform>();
                autoLabelRect.anchorMin = new Vector2(0.05f, 0.5f);
                autoLabelRect.anchorMax = new Vector2(0.6f, 0.9f);
                autoLabelRect.offsetMin = Vector2.zero;
                autoLabelRect.offsetMax = Vector2.zero;
                autoLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Load auto-save button
                var loadAutoBtn = _factory.CreateButton(autoSaveCard.transform, "Yükle", () =>
                {
                    GameManager.Instance?.LoadGame(-1);
                }, UIStyles.PrimaryButton);
                var loadAutoRect = loadAutoBtn.GetComponent<RectTransform>();
                loadAutoRect.anchorMin = new Vector2(0.65f, 0.2f);
                loadAutoRect.anchorMax = new Vector2(0.95f, 0.8f);
                loadAutoRect.offsetMin = Vector2.zero;
                loadAutoRect.offsetMax = Vector2.zero;
            }

            return screen;
        }

        private void CreateSaveSlotCard(Transform parent, SaveSlotInfo slot)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 120;
            cardLayout.preferredHeight = 120;

            if (slot.isEmpty)
            {
                // Empty slot
                var emptyLabel = _factory.CreateText(card.transform, $"Slot {slot.slotIndex + 1} - Boş", UIStyles.BodyText);
                var emptyRect = emptyLabel.GetComponent<RectTransform>();
                emptyRect.anchorMin = new Vector2(0.05f, 0.3f);
                emptyRect.anchorMax = new Vector2(0.95f, 0.7f);
                emptyRect.offsetMin = Vector2.zero;
                emptyRect.offsetMax = Vector2.zero;
                emptyLabel.GetComponent<Text>().color = UIStyles.SubtextColor;
            }
            else
            {
                // Slot header
                var slotLabel = _factory.CreateText(card.transform, $"Slot {slot.slotIndex + 1}", UIStyles.SmallText);
                var slotLabelRect = slotLabel.GetComponent<RectTransform>();
                slotLabelRect.anchorMin = new Vector2(0.05f, 0.75f);
                slotLabelRect.anchorMax = new Vector2(0.3f, 0.95f);
                slotLabelRect.offsetMin = Vector2.zero;
                slotLabelRect.offsetMax = Vector2.zero;
                slotLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Character name
                var nameLabel = _factory.CreateText(card.transform, slot.characterName, UIStyles.SubtitleText);
                var nameRect = nameLabel.GetComponent<RectTransform>();
                nameRect.anchorMin = new Vector2(0.05f, 0.45f);
                nameRect.anchorMax = new Vector2(0.6f, 0.75f);
                nameRect.offsetMin = Vector2.zero;
                nameRect.offsetMax = Vector2.zero;
                nameLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Age and date
                var infoLabel = _factory.CreateText(card.transform, $"Yaş: {slot.characterAge} | {slot.saveDate}", UIStyles.SmallText);
                var infoRect = infoLabel.GetComponent<RectTransform>();
                infoRect.anchorMin = new Vector2(0.05f, 0.15f);
                infoRect.anchorMax = new Vector2(0.6f, 0.45f);
                infoRect.offsetMin = Vector2.zero;
                infoRect.offsetMax = Vector2.zero;
                infoLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Load button
                int slotIndex = slot.slotIndex;
                var loadBtn = _factory.CreateButton(card.transform, "Yükle", () =>
                {
                    GameManager.Instance?.LoadGame(slotIndex);
                }, UIStyles.PrimaryButton);
                var loadRect = loadBtn.GetComponent<RectTransform>();
                loadRect.anchorMin = new Vector2(0.62f, 0.5f);
                loadRect.anchorMax = new Vector2(0.82f, 0.9f);
                loadRect.offsetMin = Vector2.zero;
                loadRect.offsetMax = Vector2.zero;

                // Delete button
                var deleteBtn = _factory.CreateButton(card.transform, "Sil", () =>
                {
                    ShowConfirmation($"Slot {slotIndex + 1} silinsin mi?", () =>
                    {
                        SaveManager.Instance?.DeleteSave(slotIndex);
                        // Refresh screen
                        if (_screens.ContainsKey(ScreenType.SaveLoad))
                        {
                            Destroy(_screens[ScreenType.SaveLoad]);
                            _screens.Remove(ScreenType.SaveLoad);
                        }
                        ShowScreen(ScreenType.SaveLoad);
                    });
                }, UIStyles.DangerButton);
                var deleteRect = deleteBtn.GetComponent<RectTransform>();
                deleteRect.anchorMin = new Vector2(0.62f, 0.1f);
                deleteRect.anchorMax = new Vector2(0.82f, 0.45f);
                deleteRect.offsetMin = Vector2.zero;
                deleteRect.offsetMax = Vector2.zero;
            }
        }

        private GameObject CreateDeathScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "DeathScreen";

            // R.I.P başlık
            var title = _factory.CreateText(screen.transform, "Huzur İçinde Yat", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.1f, 0.75f);
            titleRect.anchorMax = new Vector2(0.9f, 0.9f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // Epitaph (karakter ismi ve yaşı)
            string epitaphText = !string.IsNullOrEmpty(_lastEpitaph) ? _lastEpitaph : "Hayatını kaybetti.";
            var epitaph = _factory.CreateText(screen.transform, epitaphText, UIStyles.SubtitleText);
            var epitaphRect = epitaph.GetComponent<RectTransform>();
            epitaphRect.anchorMin = new Vector2(0.1f, 0.55f);
            epitaphRect.anchorMax = new Vector2(0.9f, 0.72f);
            epitaphRect.offsetMin = Vector2.zero;
            epitaphRect.offsetMax = Vector2.zero;

            // Ölüm nedeni
            string deathCauseText = !string.IsNullOrEmpty(_lastDeathCause) ? _lastDeathCause : "Bilinmeyen nedenlerden.";
            var deathCause = _factory.CreateText(screen.transform, deathCauseText, UIStyles.BodyText);
            var deathCauseRect = deathCause.GetComponent<RectTransform>();
            deathCauseRect.anchorMin = new Vector2(0.1f, 0.4f);
            deathCauseRect.anchorMax = new Vector2(0.9f, 0.52f);
            deathCauseRect.offsetMin = Vector2.zero;
            deathCauseRect.offsetMax = Vector2.zero;
            deathCause.GetComponent<Text>().color = UIStyles.SubtextColor;

            // Yaş bilgisi
            if (_lastDeathAge > 0)
            {
                var ageText = _factory.CreateText(screen.transform, $"Yaşam Süresi: {_lastDeathAge} yıl", UIStyles.BodyText);
                var ageRect = ageText.GetComponent<RectTransform>();
                ageRect.anchorMin = new Vector2(0.1f, 0.3f);
                ageRect.anchorMax = new Vector2(0.9f, 0.38f);
                ageRect.offsetMin = Vector2.zero;
                ageRect.offsetMax = Vector2.zero;
                ageText.GetComponent<Text>().color = UIStyles.AccentColor;
            }

            // Ana Menü butonu
            var menuButton = _factory.CreateButton(screen.transform, "Ana Menü", () => GameManager.Instance.ReturnToMainMenu(), UIStyles.PrimaryButton);
            var menuRect = menuButton.GetComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(0.2f, 0.1f);
            menuRect.anchorMax = new Vector2(0.8f, 0.18f);
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
