using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Kayıt/Yükleme ekranı kontrolcüsü.
    /// </summary>
    public class SaveLoadScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private bool _isSaveMode;
        private List<GameObject> _slotButtons = new List<GameObject>();
        private Text _titleText;

        public void Initialize(bool saveMode)
        {
            _isSaveMode = saveMode;
        }

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
        }

        private void BuildUI()
        {
            // Başlık
            string title = _isSaveMode ? "OYUNU KAYDET" : "OYUNU YÜKLE";
            var titleObj = _factory.CreateText(transform, title, UIStyles.TitleText);
            _titleText = titleObj.GetComponent<Text>();
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.88f);
            titleRect.anchorMax = new Vector2(1, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;
            _titleText.alignment = TextAnchor.MiddleCenter;

            // Slot paneli
            var slotPanel = _factory.CreatePanel(transform, UIStyles.CardPanel);
            var slotRect = slotPanel.GetComponent<RectTransform>();
            slotRect.anchorMin = new Vector2(0.05f, 0.15f);
            slotRect.anchorMax = new Vector2(0.95f, 0.85f);
            slotRect.offsetMin = Vector2.zero;
            slotRect.offsetMax = Vector2.zero;

            // Save slotları
            CreateSaveSlots(slotPanel.transform);

            // Geri butonu
            var backBtn = _factory.CreateButton(transform, "Geri", () =>
            {
                if (GameManager.Instance.IsPlaying)
                {
                    UIManager.Instance.ShowScreen(ScreenType.Game);
                }
                else
                {
                    UIManager.Instance.ShowScreen(ScreenType.MainMenu);
                }
            }, UIStyles.SecondaryButton);
            var backRect = backBtn.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.3f, 0.03f);
            backRect.anchorMax = new Vector2(0.7f, 0.12f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;
        }

        private void CreateSaveSlots(Transform parent)
        {
            var saveSlots = SaveManager.Instance.GetAllSaveSlots();
            float yStart = 0.92f;
            float slotHeight = 0.15f;
            float spacing = 0.02f;

            for (int i = 0; i < 5; i++)
            {
                int slotIndex = i;
                float yPos = yStart - (i * (slotHeight + spacing));

                var slotPanel = _factory.CreatePanel(parent, new PanelStyle
                {
                    backgroundColor = UIStyles.CardBackgroundColor
                });
                var slotRect = slotPanel.GetComponent<RectTransform>();
                slotRect.anchorMin = new Vector2(0.03f, yPos - slotHeight);
                slotRect.anchorMax = new Vector2(0.97f, yPos);
                slotRect.offsetMin = Vector2.zero;
                slotRect.offsetMax = Vector2.zero;

                // Slot info
                string slotText;
                bool hasData = i < saveSlots.Count && saveSlots[i] != null;

                if (hasData)
                {
                    var save = saveSlots[i];
                    slotText = $"Slot {i + 1}: {save.CharacterName}\n" +
                               $"Yaş: {save.Age} | {save.SaveDate}";
                }
                else
                {
                    slotText = $"Slot {i + 1}: Boş";
                }

                // Slot metni
                var textObj = _factory.CreateText(slotPanel.transform, slotText, UIStyles.BodyText);
                var textRect = textObj.GetComponent<RectTransform>();
                textRect.anchorMin = new Vector2(0.05f, 0.1f);
                textRect.anchorMax = new Vector2(0.65f, 0.9f);
                textRect.offsetMin = Vector2.zero;
                textRect.offsetMax = Vector2.zero;
                textObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Ana buton
                ButtonStyle btnStyle = _isSaveMode ? UIStyles.PrimaryButton : UIStyles.SecondaryButton;
                string btnText = _isSaveMode ? "Kaydet" : "Yükle";

                if (!_isSaveMode && !hasData)
                {
                    // Yükleme modunda boş slot için disabled buton
                    btnText = "Boş";
                    btnStyle = new ButtonStyle
                    {
                        backgroundColor = UIStyles.CardBackgroundColor,
                        textColor = new Color(0.5f, 0.5f, 0.5f)
                    };
                }

                var actionBtn = _factory.CreateButton(slotPanel.transform, btnText, () =>
                {
                    OnSlotAction(slotIndex);
                }, btnStyle);
                var actionRect = actionBtn.GetComponent<RectTransform>();
                actionRect.anchorMin = new Vector2(0.67f, 0.2f);
                actionRect.anchorMax = new Vector2(0.97f, 0.8f);
                actionRect.offsetMin = Vector2.zero;
                actionRect.offsetMax = Vector2.zero;

                // Yükleme modunda boş slot için butonu devre dışı bırak
                if (!_isSaveMode && !hasData)
                {
                    var btn = actionBtn.GetComponent<Button>();
                    if (btn != null) btn.interactable = false;
                }

                _slotButtons.Add(slotPanel);
            }
        }

        private void OnSlotAction(int slotIndex)
        {
            if (_isSaveMode)
            {
                // Kaydet
                SaveManager.Instance.SaveGame(slotIndex);

                // Başarı popup'ı
                UIManager.Instance.ShowPopup("Oyun Kaydedildi",
                    $"Oyun slot {slotIndex + 1}'e kaydedildi.",
                    new string[] { "Tamam" },
                    new System.Action[] { () => RefreshSlots() });
            }
            else
            {
                // Yükle
                var saveData = SaveManager.Instance.LoadGame(slotIndex);
                if (saveData != null)
                {
                    GameManager.Instance.CurrentCharacter = saveData.Character;
                    GameManager.Instance.ChangeState(GameState.Playing);
                }
            }
        }

        private void RefreshSlots()
        {
            // Slotları temizle ve yeniden oluştur
            foreach (var slot in _slotButtons)
            {
                Destroy(slot);
            }
            _slotButtons.Clear();

            // Ana paneli bul ve slotları yeniden oluştur
            var panels = GetComponentsInChildren<RectTransform>();
            foreach (var panel in panels)
            {
                if (panel.anchorMin.y == 0.15f && panel.anchorMax.y == 0.85f)
                {
                    CreateSaveSlots(panel);
                    break;
                }
            }
        }
    }
}
