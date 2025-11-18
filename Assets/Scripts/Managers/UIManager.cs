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
                case ScreenType.Activities:
                    screen = CreateActivitiesScreen();
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

        private GameObject CreateActivitiesScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "ActivitiesScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.Game), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "Aktiviteler", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // Controller ekle
            screen.AddComponent<TurkishLifeSim.UI.ActivitiesScreenController>();

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

            float yPos = 0.82f;
            float spacing = 0.10f;

            // Ses Ayarları başlığı
            var soundTitle = _factory.CreateText(screen.transform, "Ses Ayarları", UIStyles.SubtitleText);
            var soundTitleRect = soundTitle.GetComponent<RectTransform>();
            soundTitleRect.anchorMin = new Vector2(0.05f, yPos - 0.03f);
            soundTitleRect.anchorMax = new Vector2(0.95f, yPos + 0.03f);
            soundTitleRect.offsetMin = Vector2.zero;
            soundTitleRect.offsetMax = Vector2.zero;
            yPos -= spacing;

            // Müzik Toggle
            CreateSettingsToggle(screen.transform, "Müzik", yPos, GameManager.Instance.Settings.MusicEnabled, (val) => {
                GameManager.Instance.Settings.MusicEnabled = val;
                AudioManager.Instance?.SetMusicEnabled(val);
            });
            yPos -= spacing * 0.7f;

            // Ses Efektleri Toggle
            CreateSettingsToggle(screen.transform, "Ses Efektleri", yPos, GameManager.Instance.Settings.SfxEnabled, (val) => {
                GameManager.Instance.Settings.SfxEnabled = val;
                AudioManager.Instance?.SetSfxEnabled(val);
            });
            yPos -= spacing;

            // Oyun Ayarları başlığı
            var gameTitle = _factory.CreateText(screen.transform, "Oyun Ayarları", UIStyles.SubtitleText);
            var gameTitleRect = gameTitle.GetComponent<RectTransform>();
            gameTitleRect.anchorMin = new Vector2(0.05f, yPos - 0.03f);
            gameTitleRect.anchorMax = new Vector2(0.95f, yPos + 0.03f);
            gameTitleRect.offsetMin = Vector2.zero;
            gameTitleRect.offsetMax = Vector2.zero;
            yPos -= spacing;

            // Otomatik Kaydet Toggle
            CreateSettingsToggle(screen.transform, "Otomatik Kaydet", yPos, GameManager.Instance.Settings.AutoSaveEnabled, (val) => {
                GameManager.Instance.Settings.AutoSaveEnabled = val;
            });
            yPos -= spacing * 0.7f;

            // Bildirimler Toggle
            CreateSettingsToggle(screen.transform, "Bildirimler", yPos, GameManager.Instance.Settings.NotificationsEnabled, (val) => {
                GameManager.Instance.Settings.NotificationsEnabled = val;
            });
            yPos -= spacing * 1.5f;

            // Tüm Kayıtları Sil butonu
            var deleteButton = _factory.CreateButton(screen.transform, "Tüm Kayıtları Sil", () => {
                ShowConfirmation("Tüm kayıtlarınız silinecek. Emin misiniz?", () => {
                    SaveManager.Instance.DeleteAllSaves();
                    ShowInfo("Bilgi", "Tüm kayıtlar silindi.");
                });
            }, UIStyles.DangerButton);
            var deleteRect = deleteButton.GetComponent<RectTransform>();
            deleteRect.anchorMin = new Vector2(0.15f, yPos - 0.03f);
            deleteRect.anchorMax = new Vector2(0.85f, yPos + 0.03f);
            deleteRect.offsetMin = Vector2.zero;
            deleteRect.offsetMax = Vector2.zero;

            return screen;
        }

        private void CreateSettingsToggle(Transform parent, string label, float yPos, bool initialValue, Action<bool> onValueChanged)
        {
            // Label
            var labelText = _factory.CreateText(parent, label, UIStyles.BodyText);
            var labelRect = labelText.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.1f, yPos - 0.025f);
            labelRect.anchorMax = new Vector2(0.6f, yPos + 0.025f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Toggle Button
            string buttonText = initialValue ? "AÇIK" : "KAPALI";
            var toggleButton = _factory.CreateButton(parent, buttonText, null, initialValue ? UIStyles.SuccessButton : UIStyles.SecondaryButton);
            var toggleRect = toggleButton.GetComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0.65f, yPos - 0.025f);
            toggleRect.anchorMax = new Vector2(0.9f, yPos + 0.025f);
            toggleRect.offsetMin = Vector2.zero;
            toggleRect.offsetMax = Vector2.zero;

            bool currentValue = initialValue;
            var button = toggleButton.GetComponent<Button>();
            var buttonTextComponent = toggleButton.GetComponentInChildren<Text>();

            button.onClick.AddListener(() => {
                currentValue = !currentValue;
                buttonTextComponent.text = currentValue ? "AÇIK" : "KAPALI";
                var colors = button.colors;
                colors.normalColor = currentValue ? UIStyles.SuccessColor : UIStyles.SecondaryColor;
                button.colors = colors;
                onValueChanged?.Invoke(currentValue);
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

            // Save slotları container
            var slotsContainer = new GameObject("SlotsContainer");
            slotsContainer.transform.SetParent(screen.transform, false);
            var containerRect = slotsContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.05f, 0.1f);
            containerRect.anchorMax = new Vector2(0.95f, 0.88f);
            containerRect.offsetMin = Vector2.zero;
            containerRect.offsetMax = Vector2.zero;

            // Kayıt slotlarını oluştur
            var saveSlots = SaveManager.Instance.GetAllSaveSlots();
            float slotHeight = 0.15f;
            float spacing = 0.02f;
            float yPos = 1f;

            // Auto-save slotu
            if (SaveManager.Instance.HasAutoSave())
            {
                yPos -= slotHeight + spacing;
                CreateSaveSlotUI(slotsContainer.transform, -1, "Otomatik Kayıt", yPos, slotHeight);
            }

            // Manuel save slotları
            foreach (var slot in saveSlots)
            {
                yPos -= slotHeight + spacing;
                if (slot.isEmpty)
                {
                    CreateEmptySaveSlotUI(slotsContainer.transform, slot.slotIndex, yPos, slotHeight);
                }
                else
                {
                    CreateSaveSlotUI(slotsContainer.transform, slot.slotIndex,
                        $"{slot.characterName} - {slot.characterAge} yaş\n{slot.saveDate}",
                        yPos, slotHeight);
                }
            }

            return screen;
        }

        private void CreateSaveSlotUI(Transform parent, int slotIndex, string info, float yPos, float height)
        {
            // Slot panel
            var slotPanel = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var slotRect = slotPanel.GetComponent<RectTransform>();
            slotRect.anchorMin = new Vector2(0f, yPos);
            slotRect.anchorMax = new Vector2(1f, yPos + height);
            slotRect.offsetMin = Vector2.zero;
            slotRect.offsetMax = Vector2.zero;

            // Slot bilgisi
            var infoText = _factory.CreateText(slotPanel.transform, info, UIStyles.BodyText);
            var infoRect = infoText.GetComponent<RectTransform>();
            infoRect.anchorMin = new Vector2(0.05f, 0.1f);
            infoRect.anchorMax = new Vector2(0.55f, 0.9f);
            infoRect.offsetMin = Vector2.zero;
            infoRect.offsetMax = Vector2.zero;
            infoText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Yükle butonu
            var loadButton = _factory.CreateButton(slotPanel.transform, "Yükle", () => {
                var saveData = SaveManager.Instance.LoadGame(slotIndex);
                if (saveData != null)
                {
                    GameManager.Instance.LoadGame(saveData);
                }
            }, UIStyles.PrimaryButton);
            var loadRect = loadButton.GetComponent<RectTransform>();
            loadRect.anchorMin = new Vector2(0.58f, 0.2f);
            loadRect.anchorMax = new Vector2(0.78f, 0.8f);
            loadRect.offsetMin = Vector2.zero;
            loadRect.offsetMax = Vector2.zero;

            // Sil butonu
            var deleteButton = _factory.CreateButton(slotPanel.transform, "Sil", () => {
                ShowConfirmation("Bu kaydı silmek istiyor musunuz?", () => {
                    if (slotIndex == -1)
                        SaveManager.Instance.DeleteAutoSave();
                    else
                        SaveManager.Instance.DeleteSave(slotIndex);
                    // Ekranı yenile
                    if (_screens.ContainsKey(ScreenType.SaveLoad))
                    {
                        Destroy(_screens[ScreenType.SaveLoad]);
                        _screens.Remove(ScreenType.SaveLoad);
                    }
                    ShowScreen(ScreenType.SaveLoad);
                });
            }, UIStyles.DangerButton);
            var deleteRect = deleteButton.GetComponent<RectTransform>();
            deleteRect.anchorMin = new Vector2(0.82f, 0.2f);
            deleteRect.anchorMax = new Vector2(0.98f, 0.8f);
            deleteRect.offsetMin = Vector2.zero;
            deleteRect.offsetMax = Vector2.zero;
        }

        private void CreateEmptySaveSlotUI(Transform parent, int slotIndex, float yPos, float height)
        {
            // Slot panel
            var slotPanel = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var slotRect = slotPanel.GetComponent<RectTransform>();
            slotRect.anchorMin = new Vector2(0f, yPos);
            slotRect.anchorMax = new Vector2(1f, yPos + height);
            slotRect.offsetMin = Vector2.zero;
            slotRect.offsetMax = Vector2.zero;

            // Boş slot metni
            var emptyText = _factory.CreateText(slotPanel.transform, $"Slot {slotIndex + 1} - Boş", UIStyles.BodyText);
            var emptyRect = emptyText.GetComponent<RectTransform>();
            emptyRect.anchorMin = new Vector2(0.05f, 0.1f);
            emptyRect.anchorMax = new Vector2(0.95f, 0.9f);
            emptyRect.offsetMin = Vector2.zero;
            emptyRect.offsetMax = Vector2.zero;
            emptyText.GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
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
