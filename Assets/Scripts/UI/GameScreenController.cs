using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;
using TurkishLifeSim.Events;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Oyun ekranı kontrolcüsü - Ana oyun arayüzünü yönetir.
    /// </summary>
    public class GameScreenController : MonoBehaviour
    {
        // UI Referansları
        private Text _nameText;
        private Text _ageText;
        private Text _moneyText;

        // Stat bar referansları
        private GameObject _healthBar;
        private GameObject _happinessBar;
        private GameObject _intelligenceBar;
        private GameObject _appearanceBar;
        private GameObject _fameBar;

        // Stat label referansları
        private Text _healthLabel;
        private Text _happinessLabel;
        private Text _intelligenceLabel;
        private Text _appearanceLabel;
        private Text _fameLabel;

        // Olay paneli
        private GameObject _eventPanel;
        private Text _eventTitle;
        private Text _eventDescription;
        private GameObject _choicesContainer;
        private List<GameObject> _choiceButtons = new List<GameObject>();

        // Alt butonlar
        private GameObject _bottomBar;

        // UI Factory
        private UIFactory _factory;

        #region Lifecycle

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
            SubscribeToEvents();
            RefreshUI();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        #endregion

        #region UI Building

        private void BuildUI()
        {
            // Üst panel - Karakter bilgileri
            BuildTopPanel();

            // Stat paneli
            BuildStatPanel();

            // Olay paneli
            BuildEventPanel();

            // Alt butonlar
            BuildBottomBar();
        }

        private void BuildTopPanel()
        {
            // Top panel container
            var topPanel = _factory.CreatePanel(transform, UIStyles.CardPanel);
            var topRect = topPanel.GetComponent<RectTransform>();
            topRect.anchorMin = new Vector2(0, 0.9f);
            topRect.anchorMax = new Vector2(1, 1f);
            topRect.offsetMin = new Vector2(10, 5);
            topRect.offsetMax = new Vector2(-10, -5);

            // İsim
            var nameObj = _factory.CreateText(topPanel.transform, "İsim", UIStyles.SubtitleText);
            _nameText = nameObj.GetComponent<Text>();
            var nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0, 0);
            nameRect.anchorMax = new Vector2(0.6f, 1);
            nameRect.offsetMin = new Vector2(15, 0);
            nameRect.offsetMax = Vector2.zero;
            _nameText.alignment = TextAnchor.MiddleLeft;

            // Yaş
            var ageObj = _factory.CreateText(topPanel.transform, "Yaş: 0", UIStyles.SubtitleText);
            _ageText = ageObj.GetComponent<Text>();
            var ageRect = ageObj.GetComponent<RectTransform>();
            ageRect.anchorMin = new Vector2(0.6f, 0);
            ageRect.anchorMax = new Vector2(1, 1);
            ageRect.offsetMin = Vector2.zero;
            ageRect.offsetMax = new Vector2(-15, 0);
            _ageText.alignment = TextAnchor.MiddleRight;
        }

        private void BuildStatPanel()
        {
            // Stat panel container
            var statPanel = _factory.CreatePanel(transform, UIStyles.CardPanel);
            var statRect = statPanel.GetComponent<RectTransform>();
            statRect.anchorMin = new Vector2(0, 0.62f);
            statRect.anchorMax = new Vector2(1, 0.89f);
            statRect.offsetMin = new Vector2(10, 5);
            statRect.offsetMax = new Vector2(-10, -5);

            float yStart = 0.88f;
            float yStep = 0.18f;

            // Sağlık
            CreateStatRow(statPanel.transform, "Sağlık", UIStyles.HealthColor, yStart, out _healthBar, out _healthLabel);
            yStart -= yStep;

            // Mutluluk
            CreateStatRow(statPanel.transform, "Mutluluk", UIStyles.HappinessColor, yStart, out _happinessBar, out _happinessLabel);
            yStart -= yStep;

            // Zeka
            CreateStatRow(statPanel.transform, "Zeka", UIStyles.IntelligenceColor, yStart, out _intelligenceBar, out _intelligenceLabel);
            yStart -= yStep;

            // Görünüş
            CreateStatRow(statPanel.transform, "Görünüş", UIStyles.AppearanceColor, yStart, out _appearanceBar, out _appearanceLabel);
            yStart -= yStep;

            // Para
            var moneyRow = _factory.CreatePanel(statPanel.transform, new PanelStyle { backgroundColor = Color.clear });
            var moneyRowRect = moneyRow.GetComponent<RectTransform>();
            moneyRowRect.anchorMin = new Vector2(0.05f, yStart - 0.08f);
            moneyRowRect.anchorMax = new Vector2(0.95f, yStart + 0.08f);
            moneyRowRect.offsetMin = Vector2.zero;
            moneyRowRect.offsetMax = Vector2.zero;

            var moneyLabelObj = _factory.CreateText(moneyRow.transform, "Para:", UIStyles.StatLabelText);
            var moneyLabelRect = moneyLabelObj.GetComponent<RectTransform>();
            moneyLabelRect.anchorMin = new Vector2(0, 0);
            moneyLabelRect.anchorMax = new Vector2(0.25f, 1);
            moneyLabelRect.offsetMin = Vector2.zero;
            moneyLabelRect.offsetMax = Vector2.zero;
            moneyLabelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            var moneyValueObj = _factory.CreateText(moneyRow.transform, "0 TL", UIStyles.StatLabelText);
            _moneyText = moneyValueObj.GetComponent<Text>();
            var moneyValueRect = moneyValueObj.GetComponent<RectTransform>();
            moneyValueRect.anchorMin = new Vector2(0.25f, 0);
            moneyValueRect.anchorMax = new Vector2(1, 1);
            moneyValueRect.offsetMin = Vector2.zero;
            moneyValueRect.offsetMax = Vector2.zero;
            _moneyText.alignment = TextAnchor.MiddleLeft;
            _moneyText.color = UIStyles.AccentColor;
        }

        private void CreateStatRow(Transform parent, string label, Color color, float y, out GameObject bar, out Text valueLabel)
        {
            // Row container
            var row = _factory.CreatePanel(parent, new PanelStyle { backgroundColor = Color.clear });
            var rowRect = row.GetComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0.05f, y - 0.08f);
            rowRect.anchorMax = new Vector2(0.95f, y + 0.08f);
            rowRect.offsetMin = Vector2.zero;
            rowRect.offsetMax = Vector2.zero;

            // Label
            var labelObj = _factory.CreateText(row.transform, label, UIStyles.StatLabelText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0, 0);
            labelRect.anchorMax = new Vector2(0.25f, 1);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Bar
            bar = _factory.CreateProgressBar(row.transform, color, 0.5f);
            var barRect = bar.GetComponent<RectTransform>();
            barRect.anchorMin = new Vector2(0.27f, 0.2f);
            barRect.anchorMax = new Vector2(0.85f, 0.8f);
            barRect.offsetMin = Vector2.zero;
            barRect.offsetMax = Vector2.zero;

            // Value label
            var valueObj = _factory.CreateText(row.transform, "50", UIStyles.SmallText);
            valueLabel = valueObj.GetComponent<Text>();
            var valueRect = valueObj.GetComponent<RectTransform>();
            valueRect.anchorMin = new Vector2(0.87f, 0);
            valueRect.anchorMax = new Vector2(1, 1);
            valueRect.offsetMin = Vector2.zero;
            valueRect.offsetMax = Vector2.zero;
            valueLabel.alignment = TextAnchor.MiddleRight;
            valueLabel.color = UIStyles.TextColor;
        }

        private void BuildEventPanel()
        {
            // Event panel container
            _eventPanel = _factory.CreatePanel(transform, UIStyles.CardPanel);
            var eventRect = _eventPanel.GetComponent<RectTransform>();
            eventRect.anchorMin = new Vector2(0, 0.15f);
            eventRect.anchorMax = new Vector2(1, 0.61f);
            eventRect.offsetMin = new Vector2(10, 5);
            eventRect.offsetMax = new Vector2(-10, -5);

            // Event başlık
            var titleObj = _factory.CreateText(_eventPanel.transform, "Olay", UIStyles.SubtitleText);
            _eventTitle = titleObj.GetComponent<Text>();
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.05f, 0.85f);
            titleRect.anchorMax = new Vector2(0.95f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;
            _eventTitle.alignment = TextAnchor.MiddleCenter;

            // Event açıklama
            var descObj = _factory.CreateText(_eventPanel.transform, "Açıklama", UIStyles.BodyText);
            _eventDescription = descObj.GetComponent<Text>();
            var descRect = descObj.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0.05f, 0.55f);
            descRect.anchorMax = new Vector2(0.95f, 0.83f);
            descRect.offsetMin = Vector2.zero;
            descRect.offsetMax = Vector2.zero;
            _eventDescription.alignment = TextAnchor.UpperCenter;

            // Seçenekler container
            _choicesContainer = new GameObject("ChoicesContainer");
            _choicesContainer.transform.SetParent(_eventPanel.transform, false);
            var choicesRect = _choicesContainer.AddComponent<RectTransform>();
            choicesRect.anchorMin = new Vector2(0.05f, 0.05f);
            choicesRect.anchorMax = new Vector2(0.95f, 0.52f);
            choicesRect.offsetMin = Vector2.zero;
            choicesRect.offsetMax = Vector2.zero;
        }

        private void BuildBottomBar()
        {
            // Bottom bar container
            _bottomBar = _factory.CreatePanel(transform, UIStyles.CardPanel);
            var bottomRect = _bottomBar.GetComponent<RectTransform>();
            bottomRect.anchorMin = new Vector2(0, 0);
            bottomRect.anchorMax = new Vector2(1, 0.14f);
            bottomRect.offsetMin = new Vector2(10, 10);
            bottomRect.offsetMax = new Vector2(-10, -5);

            // Profil butonu
            var profileBtn = _factory.CreateButton(_bottomBar.transform, "Profil", () =>
            {
                UIManager.Instance.ShowScreen(ScreenType.Profile);
            }, UIStyles.SecondaryButton);
            var profileRect = profileBtn.GetComponent<RectTransform>();
            profileRect.anchorMin = new Vector2(0.02f, 0.15f);
            profileRect.anchorMax = new Vector2(0.32f, 0.85f);
            profileRect.offsetMin = Vector2.zero;
            profileRect.offsetMax = Vector2.zero;

            // Yaşla butonu
            var ageBtn = _factory.CreateButton(_bottomBar.transform, "Yaşla", () =>
            {
                GameManager.Instance.ProgressAge();
            }, UIStyles.PrimaryButton);
            var ageRect = ageBtn.GetComponent<RectTransform>();
            ageRect.anchorMin = new Vector2(0.35f, 0.15f);
            ageRect.anchorMax = new Vector2(0.65f, 0.85f);
            ageRect.offsetMin = Vector2.zero;
            ageRect.offsetMax = Vector2.zero;

            // İlişkiler butonu
            var relBtn = _factory.CreateButton(_bottomBar.transform, "İlişkiler", () =>
            {
                UIManager.Instance.ShowScreen(ScreenType.Relationships);
            }, UIStyles.SecondaryButton);
            var relRect = relBtn.GetComponent<RectTransform>();
            relRect.anchorMin = new Vector2(0.68f, 0.15f);
            relRect.anchorMax = new Vector2(0.98f, 0.85f);
            relRect.offsetMin = Vector2.zero;
            relRect.offsetMax = Vector2.zero;
        }

        #endregion

        #region Event Subscription

        private void SubscribeToEvents()
        {
            EventBus.Subscribe<StatChangedEvent>(OnStatChanged);
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);
            EventBus.Subscribe<MoneyChangedEvent>(OnMoneyChanged);
            EventBus.Subscribe<EventStartedEvent>(OnEventStarted);
        }

        private void UnsubscribeFromEvents()
        {
            EventBus.Unsubscribe<StatChangedEvent>(OnStatChanged);
            EventBus.Unsubscribe<AgeProgressedEvent>(OnAgeProgressed);
            EventBus.Unsubscribe<MoneyChangedEvent>(OnMoneyChanged);
            EventBus.Unsubscribe<EventStartedEvent>(OnEventStarted);
        }

        private void OnStatChanged(StatChangedEvent evt)
        {
            RefreshStats();
        }

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            RefreshUI();
        }

        private void OnMoneyChanged(MoneyChangedEvent evt)
        {
            RefreshMoney();
        }

        private void OnEventStarted(EventStartedEvent evt)
        {
            RefreshEvent();
        }

        #endregion

        #region UI Refresh

        public void RefreshUI()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            RefreshCharacterInfo();
            RefreshStats();
            RefreshMoney();
            RefreshEvent();
        }

        private void RefreshCharacterInfo()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            _nameText.text = character.FullName;
            _ageText.text = $"Yaş: {character.Age}";
        }

        private void RefreshStats()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            var stats = character.Stats;

            // Stat barlarını güncelle
            UpdateStatBar(_healthBar, _healthLabel, stats.Health);
            UpdateStatBar(_happinessBar, _happinessLabel, stats.Happiness);
            UpdateStatBar(_intelligenceBar, _intelligenceLabel, stats.Intelligence);
            UpdateStatBar(_appearanceBar, _appearanceLabel, stats.Appearance);
        }

        private void UpdateStatBar(GameObject bar, Text label, int value)
        {
            if (bar == null || label == null) return;

            // Bar fill güncelle
            _factory.UpdateProgressBar(bar, value / 100f);

            // Label güncelle
            label.text = value.ToString();
        }

        private void RefreshMoney()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            _moneyText.text = $"{character.Finances.CurrentMoney:N0} TL";
        }

        private void RefreshEvent()
        {
            var currentEvent = EventManager.Instance.CurrentEvent;

            if (currentEvent == null)
            {
                _eventTitle.text = "Hayat Devam Ediyor";
                _eventDescription.text = "Yaşla butonuna basarak bir yıl ilerle.";
                ClearChoices();
                return;
            }

            _eventTitle.text = currentEvent.title;
            _eventDescription.text = currentEvent.description;

            // Seçenekleri oluştur
            CreateChoiceButtons(currentEvent.choices);
        }

        private void CreateChoiceButtons(List<EventChoice> choices)
        {
            ClearChoices();

            if (choices == null || choices.Count == 0) return;

            float buttonHeight = 1f / choices.Count;

            for (int i = 0; i < choices.Count; i++)
            {
                int choiceIndex = i; // Closure için

                var button = _factory.CreateButton(
                    _choicesContainer.transform,
                    choices[i].text,
                    () => OnChoiceSelected(choiceIndex),
                    UIStyles.ChoiceButton
                );

                var rect = button.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 1 - (i + 1) * buttonHeight + 0.02f);
                rect.anchorMax = new Vector2(1, 1 - i * buttonHeight - 0.02f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                _choiceButtons.Add(button);
            }
        }

        private void ClearChoices()
        {
            foreach (var button in _choiceButtons)
            {
                Destroy(button);
            }
            _choiceButtons.Clear();
        }

        private void OnChoiceSelected(int choiceIndex)
        {
            EventManager.Instance.MakeChoice(choiceIndex);
        }

        #endregion
    }
}
