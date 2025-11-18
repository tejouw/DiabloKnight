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
                case ScreenType.Education:
                    screen = CreateEducationScreen();
                    break;
                case ScreenType.Career:
                    screen = CreateCareerScreen();
                    break;
                case ScreenType.Financial:
                    screen = CreateFinancialScreen();
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

            var settings = GameManager.Instance.Settings;
            float yPos = 0.82f;
            float yStep = 0.12f;

            // Ana Ses slider
            CreateSettingsSlider(screen.transform, "Ana Ses", yPos, settings.MasterVolume, (value) => {
                settings.MasterVolume = value;
                AudioListener.volume = value;
            });
            yPos -= yStep;

            // Müzik Ses slider
            CreateSettingsSlider(screen.transform, "Müzik", yPos, settings.MusicVolume, (value) => {
                settings.MusicVolume = value;
            });
            yPos -= yStep;

            // SFX Ses slider
            CreateSettingsSlider(screen.transform, "Efektler", yPos, settings.SFXVolume, (value) => {
                settings.SFXVolume = value;
            });
            yPos -= yStep;

            // Otomatik Kaydetme toggle
            CreateSettingsToggle(screen.transform, "Otomatik Kaydet", yPos, settings.AutoSaveEnabled, (value) => {
                settings.AutoSaveEnabled = value;
            });
            yPos -= yStep;

            // Bildirimler toggle
            CreateSettingsToggle(screen.transform, "Bildirimler", yPos, settings.NotificationsEnabled, (value) => {
                settings.NotificationsEnabled = value;
            });
            yPos -= yStep;

            // Dil seçimi
            var langRow = _factory.CreatePanel(screen.transform, new PanelStyle { backgroundColor = Color.clear });
            var langRowRect = langRow.GetComponent<RectTransform>();
            langRowRect.anchorMin = new Vector2(0.05f, yPos - 0.04f);
            langRowRect.anchorMax = new Vector2(0.95f, yPos + 0.04f);
            langRowRect.offsetMin = Vector2.zero;
            langRowRect.offsetMax = Vector2.zero;

            var langLabel = _factory.CreateText(langRow.transform, "Dil: Türkçe", UIStyles.BodyText);
            var langLabelRect = langLabel.GetComponent<RectTransform>();
            langLabelRect.anchorMin = new Vector2(0, 0);
            langLabelRect.anchorMax = new Vector2(1, 1);
            langLabelRect.offsetMin = Vector2.zero;
            langLabelRect.offsetMax = Vector2.zero;
            langLabel.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            return screen;
        }

        private void CreateSettingsSlider(Transform parent, string label, float yPos, float initialValue, System.Action<float> onValueChanged)
        {
            var row = _factory.CreatePanel(parent, new PanelStyle { backgroundColor = Color.clear });
            var rowRect = row.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.05f, yPos - 0.04f);
            rowRect.anchorMax = new Vector2(0.95f, yPos + 0.04f);
            rowRect.offsetMin = Vector2.zero;
            rowRect.offsetMax = Vector2.zero;

            var labelObj = _factory.CreateText(row.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(0.3f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            var slider = _factory.CreateSlider(row.transform, 0f, 1f, initialValue, onValueChanged);
            var sliderRect = slider.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.35f, 0.2f);
            sliderRect.anchorMax = new Vector2(0.95f, 0.8f);
            sliderRect.offsetMin = Vector2.zero;
            sliderRect.offsetMax = Vector2.zero;
        }

        private void CreateSettingsToggle(Transform parent, string label, float yPos, bool initialValue, System.Action<bool> onValueChanged)
        {
            var row = _factory.CreatePanel(parent, new PanelStyle { backgroundColor = Color.clear });
            var rowRect = row.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.05f, yPos - 0.04f);
            rowRect.anchorMax = new Vector2(0.95f, yPos + 0.04f);
            rowRect.offsetMin = Vector2.zero;
            rowRect.offsetMax = Vector2.zero;

            var labelObj = _factory.CreateText(row.transform, label, UIStyles.BodyText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(0.6f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            string btnText = initialValue ? "Açık" : "Kapalı";
            var toggleBtn = _factory.CreateButton(row.transform, btnText, null, UIStyles.SecondaryButton);
            var toggleRect = toggleBtn.GetComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0.65f, 0.1f);
            toggleRect.anchorMax = new Vector2(0.95f, 0.9f);
            toggleRect.offsetMin = Vector2.zero;
            toggleRect.offsetMax = Vector2.zero;

            bool currentValue = initialValue;
            var btnComponent = toggleBtn.GetComponent<Button>();
            var btnTextComponent = toggleBtn.GetComponentInChildren<Text>();

            btnComponent.onClick.AddListener(() => {
                currentValue = !currentValue;
                btnTextComponent.text = currentValue ? "Açık" : "Kapalı";
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

            // Scroll view oluştur
            var scrollView = _factory.CreateScrollView(screen.transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.05f, 0.1f);
            scrollRect.anchorMax = new Vector2(0.95f, 0.88f);
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            var content = scrollView.transform.Find("Viewport/Content");

            // Kayıt slotlarını oluştur
            var saveSlots = SaveManager.Instance?.GetAllSaveSlots();
            if (saveSlots != null)
            {
                foreach (var slot in saveSlots)
                {
                    CreateSaveSlotUI(content, slot);
                }
            }

            // Auto-save kontrolü
            if (SaveManager.Instance != null && SaveManager.Instance.HasAutoSave())
            {
                var autoSaveSlot = new SaveSlotInfo
                {
                    slotIndex = -1,
                    characterName = "Otomatik Kayıt",
                    isEmpty = false
                };
                CreateSaveSlotUI(content, autoSaveSlot, true);
            }

            return screen;
        }

        private void CreateSaveSlotUI(Transform parent, SaveSlotInfo slot, bool isAutoSave = false)
        {
            var slotPanel = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var slotRect = slotPanel.GetComponent<RectTransform>();
            slotRect.sizeDelta = new Vector2(0, 120);

            var layout = slotPanel.AddComponent<LayoutElement>();
            layout.minHeight = 120;
            layout.preferredHeight = 120;

            if (slot.isEmpty && !isAutoSave)
            {
                // Boş slot
                var emptyLabel = _factory.CreateText(slotPanel.transform, $"Slot {slot.slotIndex + 1} - Boş", UIStyles.SubtitleText);
                var emptyRect = emptyLabel.GetComponent<RectTransform>();
                emptyRect.anchorMin = new Vector2(0.05f, 0.5f);
                emptyRect.anchorMax = new Vector2(0.6f, 0.9f);
                emptyRect.offsetMin = Vector2.zero;
                emptyRect.offsetMax = Vector2.zero;
                emptyLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Kaydet butonu (sadece oyundayken)
                if (GameManager.Instance?.IsPlaying == true)
                {
                    int slotIndex = slot.slotIndex;
                    var saveBtn = _factory.CreateButton(slotPanel.transform, "Kaydet", () => {
                        GameManager.Instance.SaveGame(slotIndex);
                        ShowInfo("Kayıt", $"Oyun Slot {slotIndex + 1}'e kaydedildi.");
                        // Ekranı yenile
                        _screens.Remove(ScreenType.SaveLoad);
                        ShowScreen(ScreenType.SaveLoad);
                    }, UIStyles.PrimaryButton);
                    var saveBtnRect = saveBtn.GetComponent<RectTransform>();
                    saveBtnRect.anchorMin = new Vector2(0.65f, 0.2f);
                    saveBtnRect.anchorMax = new Vector2(0.95f, 0.8f);
                    saveBtnRect.offsetMin = Vector2.zero;
                    saveBtnRect.offsetMax = Vector2.zero;
                }
            }
            else
            {
                // Dolu slot
                string slotName = isAutoSave ? "Otomatik Kayıt" : $"Slot {slot.slotIndex + 1}";
                var nameLabel = _factory.CreateText(slotPanel.transform, slotName, UIStyles.SubtitleText);
                var nameRect = nameLabel.GetComponent<RectTransform>();
                nameRect.anchorMin = new Vector2(0.05f, 0.6f);
                nameRect.anchorMax = new Vector2(0.6f, 0.95f);
                nameRect.offsetMin = Vector2.zero;
                nameRect.offsetMax = Vector2.zero;
                nameLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Karakter bilgisi
                string charInfo = $"{slot.characterName} - Yaş: {slot.characterAge}";
                var infoLabel = _factory.CreateText(slotPanel.transform, charInfo, UIStyles.SmallText);
                var infoRect = infoLabel.GetComponent<RectTransform>();
                infoRect.anchorMin = new Vector2(0.05f, 0.3f);
                infoRect.anchorMax = new Vector2(0.6f, 0.6f);
                infoRect.offsetMin = Vector2.zero;
                infoRect.offsetMax = Vector2.zero;
                infoLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Tarih
                if (!string.IsNullOrEmpty(slot.saveDate))
                {
                    var dateLabel = _factory.CreateText(slotPanel.transform, slot.saveDate, UIStyles.SmallText);
                    var dateRect = dateLabel.GetComponent<RectTransform>();
                    dateRect.anchorMin = new Vector2(0.05f, 0.05f);
                    dateRect.anchorMax = new Vector2(0.6f, 0.3f);
                    dateRect.offsetMin = Vector2.zero;
                    dateRect.offsetMax = Vector2.zero;
                    dateLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                }

                // Yükle butonu
                int loadIndex = slot.slotIndex;
                var loadBtn = _factory.CreateButton(slotPanel.transform, "Yükle", () => {
                    GameManager.Instance.LoadGame(loadIndex);
                }, UIStyles.PrimaryButton);
                var loadBtnRect = loadBtn.GetComponent<RectTransform>();
                loadBtnRect.anchorMin = new Vector2(0.65f, 0.5f);
                loadBtnRect.anchorMax = new Vector2(0.95f, 0.9f);
                loadBtnRect.offsetMin = Vector2.zero;
                loadBtnRect.offsetMax = Vector2.zero;

                // Sil butonu (otomatik kayıt değilse)
                if (!isAutoSave)
                {
                    int deleteIndex = slot.slotIndex;
                    var deleteBtn = _factory.CreateButton(slotPanel.transform, "Sil", () => {
                        ShowConfirmation("Bu kaydı silmek istediğinize emin misiniz?", () => {
                            SaveManager.Instance.DeleteSave(deleteIndex);
                            // Ekranı yenile
                            _screens.Remove(ScreenType.SaveLoad);
                            ShowScreen(ScreenType.SaveLoad);
                        });
                    }, UIStyles.DangerButton);
                    var deleteBtnRect = deleteBtn.GetComponent<RectTransform>();
                    deleteBtnRect.anchorMin = new Vector2(0.65f, 0.1f);
                    deleteBtnRect.anchorMax = new Vector2(0.95f, 0.45f);
                    deleteBtnRect.offsetMin = Vector2.zero;
                    deleteBtnRect.offsetMax = Vector2.zero;
                }
            }
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

        private GameObject CreateEducationScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "EducationScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.Profile), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "Eğitim", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return screen;

            var education = character.Education;

            // Scroll view
            var scrollView = _factory.CreateScrollView(screen.transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.05f, 0.1f);
            scrollRect.anchorMax = new Vector2(0.95f, 0.88f);
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            var content = scrollView.transform.Find("Viewport/Content");

            // Mevcut Eğitim Seviyesi
            CreateInfoCard(content, "Mevcut Seviye", GetEducationLevelText(education.CurrentLevel));

            // Okul
            if (!string.IsNullOrEmpty(education.schoolName))
            {
                CreateInfoCard(content, "Okul", education.schoolName);
            }

            // GPA
            if (education.gpa > 0)
            {
                CreateInfoCard(content, "Not Ortalaması", $"{education.gpa:F2} / 4.00");
            }

            // YKS Puanı
            if (education.yksScore > 0)
            {
                CreateInfoCard(content, "YKS Puanı", education.yksScore.ToString());
            }

            // Üniversite
            if (!string.IsNullOrEmpty(education.universityName))
            {
                CreateInfoCard(content, "Üniversite", education.universityName);
                if (!string.IsNullOrEmpty(education.department))
                {
                    CreateInfoCard(content, "Bölüm", education.department);
                }
            }

            // Mezuniyet durumu
            CreateInfoCard(content, "Mezuniyet", education.isGraduated ? "Mezun" : "Öğrenci");

            // Başarılar
            if (education.achievements != null && education.achievements.Count > 0)
            {
                var achievementPanel = _factory.CreatePanel(content, UIStyles.CardPanel);
                var achLayout = achievementPanel.AddComponent<LayoutElement>();
                achLayout.minHeight = 50 + education.achievements.Count * 30;

                var achTitle = _factory.CreateText(achievementPanel.transform, "Başarılar", UIStyles.SubtitleText);
                var achTitleRect = achTitle.GetComponent<RectTransform>();
                achTitleRect.anchorMin = new Vector2(0.05f, 0.7f);
                achTitleRect.anchorMax = new Vector2(0.95f, 0.95f);
                achTitleRect.offsetMin = Vector2.zero;
                achTitleRect.offsetMax = Vector2.zero;
                achTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                string achList = string.Join("\n", education.achievements);
                var achText = _factory.CreateText(achievementPanel.transform, achList, UIStyles.SmallText);
                var achTextRect = achText.GetComponent<RectTransform>();
                achTextRect.anchorMin = new Vector2(0.05f, 0.05f);
                achTextRect.anchorMax = new Vector2(0.95f, 0.7f);
                achTextRect.offsetMin = Vector2.zero;
                achTextRect.offsetMax = Vector2.zero;
                achText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
            }

            return screen;
        }

        private GameObject CreateCareerScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "CareerScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.Profile), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "Kariyer", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return screen;

            var career = character.Career;

            // Scroll view
            var scrollView = _factory.CreateScrollView(screen.transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.05f, 0.1f);
            scrollRect.anchorMax = new Vector2(0.95f, 0.88f);
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            var content = scrollView.transform.Find("Viewport/Content");

            // Mevcut İş
            if (career.CurrentJob != null)
            {
                var jobPanel = _factory.CreatePanel(content, UIStyles.CardPanel);
                var jobLayout = jobPanel.AddComponent<LayoutElement>();
                jobLayout.minHeight = 150;

                var jobTitle = _factory.CreateText(jobPanel.transform, "Mevcut İş", UIStyles.SubtitleText);
                var jobTitleRect = jobTitle.GetComponent<RectTransform>();
                jobTitleRect.anchorMin = new Vector2(0.05f, 0.75f);
                jobTitleRect.anchorMax = new Vector2(0.95f, 0.95f);
                jobTitleRect.offsetMin = Vector2.zero;
                jobTitleRect.offsetMax = Vector2.zero;
                jobTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                var positionText = _factory.CreateText(jobPanel.transform, career.CurrentJob.title, UIStyles.BodyText);
                var posRect = positionText.GetComponent<RectTransform>();
                posRect.anchorMin = new Vector2(0.05f, 0.5f);
                posRect.anchorMax = new Vector2(0.95f, 0.75f);
                posRect.offsetMin = Vector2.zero;
                posRect.offsetMax = Vector2.zero;
                positionText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                positionText.GetComponent<Text>().color = UIStyles.AccentColor;

                var companyText = _factory.CreateText(jobPanel.transform, career.CurrentJob.company, UIStyles.SmallText);
                var compRect = companyText.GetComponent<RectTransform>();
                compRect.anchorMin = new Vector2(0.05f, 0.3f);
                compRect.anchorMax = new Vector2(0.95f, 0.5f);
                compRect.offsetMin = Vector2.zero;
                compRect.offsetMax = Vector2.zero;
                companyText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                var salaryText = _factory.CreateText(jobPanel.transform, $"Maaş: {career.CurrentJob.baseSalary:N0} TL", UIStyles.SmallText);
                var salRect = salaryText.GetComponent<RectTransform>();
                salRect.anchorMin = new Vector2(0.05f, 0.05f);
                salRect.anchorMax = new Vector2(0.5f, 0.3f);
                salRect.offsetMin = Vector2.zero;
                salRect.offsetMax = Vector2.zero;
                salaryText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                var yearsText = _factory.CreateText(jobPanel.transform, $"{career.yearsInJob} yıl", UIStyles.SmallText);
                var yearsRect = yearsText.GetComponent<RectTransform>();
                yearsRect.anchorMin = new Vector2(0.5f, 0.05f);
                yearsRect.anchorMax = new Vector2(0.95f, 0.3f);
                yearsRect.offsetMin = Vector2.zero;
                yearsRect.offsetMax = Vector2.zero;
                yearsText.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            }
            else
            {
                CreateInfoCard(content, "İş Durumu", "İşsiz");
            }

            // Performans
            CreateInfoCard(content, "Performans", $"{career.performanceRating}/100");

            // İş Geçmişi
            if (career.jobHistory != null && career.jobHistory.Count > 0)
            {
                var historyTitle = _factory.CreateText(content, "İş Geçmişi", UIStyles.SubtitleText);
                var histLayout = historyTitle.AddComponent<LayoutElement>();
                histLayout.minHeight = 40;
                historyTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                foreach (var job in career.jobHistory)
                {
                    var histPanel = _factory.CreatePanel(content, UIStyles.CardPanel);
                    var histPanelLayout = histPanel.AddComponent<LayoutElement>();
                    histPanelLayout.minHeight = 80;

                    var histJobTitle = _factory.CreateText(histPanel.transform, job.title, UIStyles.BodyText);
                    var histJobRect = histJobTitle.GetComponent<RectTransform>();
                    histJobRect.anchorMin = new Vector2(0.05f, 0.5f);
                    histJobRect.anchorMax = new Vector2(0.95f, 0.95f);
                    histJobRect.offsetMin = Vector2.zero;
                    histJobRect.offsetMax = Vector2.zero;
                    histJobTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                    var histInfo = _factory.CreateText(histPanel.transform, $"{job.company} - {job.yearsWorked} yıl", UIStyles.SmallText);
                    var histInfoRect = histInfo.GetComponent<RectTransform>();
                    histInfoRect.anchorMin = new Vector2(0.05f, 0.05f);
                    histInfoRect.anchorMax = new Vector2(0.95f, 0.5f);
                    histInfoRect.offsetMin = Vector2.zero;
                    histInfoRect.offsetMax = Vector2.zero;
                    histInfo.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                }
            }

            return screen;
        }

        private GameObject CreateFinancialScreen()
        {
            var screen = _factory.CreatePanel(_mainCanvas.transform, UIStyles.FullScreenPanel);
            screen.name = "FinancialScreen";

            // Geri butonu
            var backButton = _factory.CreateButton(screen.transform, "< Geri", () => ShowScreen(ScreenType.Profile), UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(screen.transform, "Finans", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return screen;

            var finances = character.Finances;

            // Scroll view
            var scrollView = _factory.CreateScrollView(screen.transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.05f, 0.1f);
            scrollRect.anchorMax = new Vector2(0.95f, 0.88f);
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            var content = scrollView.transform.Find("Viewport/Content");

            // Mevcut Para
            CreateInfoCard(content, "Mevcut Bakiye", $"{finances.CurrentMoney:N0} TL", UIStyles.AccentColor);

            // Toplam Kazanç
            CreateInfoCard(content, "Toplam Kazanç", $"{finances.totalEarned:N0} TL");

            // Toplam Harcama
            CreateInfoCard(content, "Toplam Harcama", $"{finances.totalSpent:N0} TL");

            // Net Değer
            decimal netWorth = finances.CurrentMoney;
            CreateInfoCard(content, "Net Değer", $"{netWorth:N0} TL");

            // Varlıklar
            if (finances.assets != null && finances.assets.Count > 0)
            {
                var assetPanel = _factory.CreatePanel(content, UIStyles.CardPanel);
                var assetLayout = assetPanel.AddComponent<LayoutElement>();
                assetLayout.minHeight = 50 + finances.assets.Count * 25;

                var assetTitle = _factory.CreateText(assetPanel.transform, "Varlıklar", UIStyles.SubtitleText);
                var assetTitleRect = assetTitle.GetComponent<RectTransform>();
                assetTitleRect.anchorMin = new Vector2(0.05f, 0.7f);
                assetTitleRect.anchorMax = new Vector2(0.95f, 0.95f);
                assetTitleRect.offsetMin = Vector2.zero;
                assetTitleRect.offsetMax = Vector2.zero;
                assetTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                string assetList = string.Join("\n", finances.assets);
                var assetText = _factory.CreateText(assetPanel.transform, assetList, UIStyles.SmallText);
                var assetTextRect = assetText.GetComponent<RectTransform>();
                assetTextRect.anchorMin = new Vector2(0.05f, 0.05f);
                assetTextRect.anchorMax = new Vector2(0.95f, 0.7f);
                assetTextRect.offsetMin = Vector2.zero;
                assetTextRect.offsetMax = Vector2.zero;
                assetText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
                assetText.GetComponent<Text>().color = UIStyles.AccentColor;
            }
            else
            {
                CreateInfoCard(content, "Varlıklar", "Henüz varlık yok");
            }

            // Borçlar
            if (finances.debts != null && finances.debts.Count > 0)
            {
                var debtPanel = _factory.CreatePanel(content, UIStyles.CardPanel);
                var debtLayout = debtPanel.AddComponent<LayoutElement>();
                debtLayout.minHeight = 50 + finances.debts.Count * 25;

                var debtTitle = _factory.CreateText(debtPanel.transform, "Borçlar", UIStyles.SubtitleText);
                var debtTitleRect = debtTitle.GetComponent<RectTransform>();
                debtTitleRect.anchorMin = new Vector2(0.05f, 0.7f);
                debtTitleRect.anchorMax = new Vector2(0.95f, 0.95f);
                debtTitleRect.offsetMin = Vector2.zero;
                debtTitleRect.offsetMax = Vector2.zero;
                debtTitle.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                string debtList = string.Join("\n", finances.debts);
                var debtText = _factory.CreateText(debtPanel.transform, debtList, UIStyles.SmallText);
                var debtTextRect = debtText.GetComponent<RectTransform>();
                debtTextRect.anchorMin = new Vector2(0.05f, 0.05f);
                debtTextRect.anchorMax = new Vector2(0.95f, 0.7f);
                debtTextRect.offsetMin = Vector2.zero;
                debtTextRect.offsetMax = Vector2.zero;
                debtText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
                debtText.GetComponent<Text>().color = UIStyles.SecondaryColor;
            }
            else
            {
                CreateInfoCard(content, "Borçlar", "Borç yok");
            }

            return screen;
        }

        private void CreateInfoCard(Transform parent, string label, string value, Color? valueColor = null)
        {
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var layout = card.AddComponent<LayoutElement>();
            layout.minHeight = 70;

            var labelText = _factory.CreateText(card.transform, label, UIStyles.SmallText);
            var labelRect = labelText.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0.55f);
            labelRect.anchorMax = new Vector2(0.95f, 0.9f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            var valueText = _factory.CreateText(card.transform, value, UIStyles.BodyText);
            var valueRect = valueText.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.05f, 0.1f);
            valueRect.anchorMax = new Vector2(0.95f, 0.55f);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            if (valueColor.HasValue)
            {
                valueText.GetComponent<Text>().color = valueColor.Value;
            }
        }

        private string GetEducationLevelText(Character.EducationLevel level)
        {
            return level switch
            {
                Character.EducationLevel.None => "Eğitim Yok",
                Character.EducationLevel.PrimarySchool => "İlkokul",
                Character.EducationLevel.MiddleSchool => "Ortaokul",
                Character.EducationLevel.HighSchool => "Lise",
                Character.EducationLevel.University => "Üniversite",
                Character.EducationLevel.Masters => "Yüksek Lisans",
                Character.EducationLevel.Doctorate => "Doktora",
                _ => "Bilinmiyor"
            };
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
