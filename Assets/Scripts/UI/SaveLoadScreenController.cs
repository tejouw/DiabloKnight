using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Kayıt/Yükleme ekranı kontrolcüsü.
    /// </summary>
    public class SaveLoadScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private List<GameObject> _slotButtons = new List<GameObject>();
        private bool _isSaveMode = false;

        private void Start()
        {
            _factory = UIManager.Instance.Factory;

            // Eğer oyun oynuyorsa kaydetme modu, değilse yükleme modu
            _isSaveMode = GameManager.Instance.IsPlaying;

            BuildUI();
        }

        private void BuildUI()
        {
            // Geri butonu
            var backButton = _factory.CreateButton(transform, "< Geri", () =>
            {
                if (_isSaveMode)
                    UIManager.Instance.ShowScreen(ScreenType.Game);
                else
                    UIManager.Instance.ShowScreen(ScreenType.MainMenu);
            }, UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            string titleText = _isSaveMode ? "Oyunu Kaydet" : "Oyun Yükle";
            var title = _factory.CreateText(transform, titleText, UIStyles.TitleText);
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

            // Auto-save slotu
            if (SaveManager.Instance.HasAutoSave())
            {
                var autoSave = SaveManager.Instance.LoadAutoSave();
                if (autoSave != null)
                {
                    CreateSaveSlot(content, -1, autoSave.characterName, autoSave.characterAge, autoSave.saveDate, true);
                }
            }

            // Manuel kayıt slotları
            var saveSlots = SaveManager.Instance.GetAllSaveSlots();
            foreach (var slot in saveSlots)
            {
                if (slot.isEmpty)
                {
                    CreateEmptySlot(content, slot.slotIndex);
                }
                else
                {
                    CreateSaveSlot(content, slot.slotIndex, slot.characterName, slot.characterAge, slot.saveDate, false);
                }
            }
        }

        private void CreateSaveSlot(Transform parent, int slotIndex, string characterName, int age, string saveDate, bool isAutoSave)
        {
            // Card container
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 120;
            cardLayout.preferredHeight = 120;

            // Slot numarası
            string slotLabel = isAutoSave ? "Otomatik Kayıt" : $"Kayıt {slotIndex + 1}";
            var slotText = _factory.CreateText(card.transform, slotLabel, UIStyles.SmallText);
            var slotRect = slotText.GetComponent<RectTransform>();
            slotRect.anchorMin = new Vector2(0.05f, 0.7f);
            slotRect.anchorMax = new Vector2(0.5f, 0.95f);
            slotRect.offsetMin = Vector2.zero;
            slotRect.offsetMax = Vector2.zero;
            slotText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Tarih
            var dateText = _factory.CreateText(card.transform, saveDate, UIStyles.SmallText);
            var dateRect = dateText.GetComponent<RectTransform>();
            dateRect.anchorMin = new Vector2(0.5f, 0.7f);
            dateRect.anchorMax = new Vector2(0.95f, 0.95f);
            dateRect.offsetMin = Vector2.zero;
            dateRect.offsetMax = Vector2.zero;
            var dateTextComp = dateText.GetComponent<Text>();
            dateTextComp.alignment = TextAnchor.MiddleRight;
            dateTextComp.color = UIStyles.SubtextColor;

            // Karakter bilgisi
            var charText = _factory.CreateText(card.transform, $"{characterName} - Yaş: {age}", UIStyles.BodyText);
            var charRect = charText.GetComponent<RectTransform>();
            charRect.anchorMin = new Vector2(0.05f, 0.35f);
            charRect.anchorMax = new Vector2(0.95f, 0.7f);
            charRect.offsetMin = Vector2.zero;
            charRect.offsetMax = Vector2.zero;
            charText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Butonlar
            if (_isSaveMode && !isAutoSave)
            {
                // Kaydet butonu
                var saveBtn = _factory.CreateButton(card.transform, "Üzerine Yaz", () =>
                {
                    SaveToSlot(slotIndex);
                }, UIStyles.PrimaryButton);
                var saveBtnRect = saveBtn.GetComponent<RectTransform>();
                saveBtnRect.anchorMin = new Vector2(0.05f, 0.05f);
                saveBtnRect.anchorMax = new Vector2(0.45f, 0.3f);
                saveBtnRect.offsetMin = Vector2.zero;
                saveBtnRect.offsetMax = Vector2.zero;

                // Sil butonu
                var deleteBtn = _factory.CreateButton(card.transform, "Sil", () =>
                {
                    DeleteSlot(slotIndex);
                }, UIStyles.DangerButton);
                var deleteBtnRect = deleteBtn.GetComponent<RectTransform>();
                deleteBtnRect.anchorMin = new Vector2(0.55f, 0.05f);
                deleteBtnRect.anchorMax = new Vector2(0.95f, 0.3f);
                deleteBtnRect.offsetMin = Vector2.zero;
                deleteBtnRect.offsetMax = Vector2.zero;
            }
            else
            {
                // Yükle butonu
                var loadBtn = _factory.CreateButton(card.transform, "Yükle", () =>
                {
                    LoadFromSlot(slotIndex);
                }, UIStyles.PrimaryButton);
                var loadBtnRect = loadBtn.GetComponent<RectTransform>();
                loadBtnRect.anchorMin = new Vector2(0.3f, 0.05f);
                loadBtnRect.anchorMax = new Vector2(0.7f, 0.3f);
                loadBtnRect.offsetMin = Vector2.zero;
                loadBtnRect.offsetMax = Vector2.zero;
            }

            _slotButtons.Add(card);
        }

        private void CreateEmptySlot(Transform parent, int slotIndex)
        {
            // Card container
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 100;
            cardLayout.preferredHeight = 100;

            // Slot numarası
            var slotText = _factory.CreateText(card.transform, $"Kayıt {slotIndex + 1}", UIStyles.SmallText);
            var slotRect = slotText.GetComponent<RectTransform>();
            slotRect.anchorMin = new Vector2(0.05f, 0.6f);
            slotRect.anchorMax = new Vector2(0.95f, 0.95f);
            slotRect.offsetMin = Vector2.zero;
            slotRect.offsetMax = Vector2.zero;
            slotText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Boş mesajı
            var emptyText = _factory.CreateText(card.transform, "- Boş Slot -", UIStyles.SmallText);
            var emptyRect = emptyText.GetComponent<RectTransform>();
            emptyRect.anchorMin = new Vector2(0.05f, 0.35f);
            emptyRect.anchorMax = new Vector2(0.95f, 0.6f);
            emptyRect.offsetMin = Vector2.zero;
            emptyRect.offsetMax = Vector2.zero;
            var emptyTextComp = emptyText.GetComponent<Text>();
            emptyTextComp.alignment = TextAnchor.MiddleCenter;
            emptyTextComp.color = UIStyles.SubtextColor;

            if (_isSaveMode)
            {
                // Kaydet butonu
                var saveBtn = _factory.CreateButton(card.transform, "Buraya Kaydet", () =>
                {
                    SaveToSlot(slotIndex);
                }, UIStyles.PrimaryButton);
                var saveBtnRect = saveBtn.GetComponent<RectTransform>();
                saveBtnRect.anchorMin = new Vector2(0.2f, 0.05f);
                saveBtnRect.anchorMax = new Vector2(0.8f, 0.3f);
                saveBtnRect.offsetMin = Vector2.zero;
                saveBtnRect.offsetMax = Vector2.zero;
            }

            _slotButtons.Add(card);
        }

        private void SaveToSlot(int slotIndex)
        {
            SaveManager.Instance.SaveGame(slotIndex);
            UIManager.Instance.ShowInfo("Başarılı", "Oyun kaydedildi!");
            RefreshUI();
        }

        private void LoadFromSlot(int slotIndex)
        {
            GameManager.Instance.LoadGame(slotIndex);
        }

        private void DeleteSlot(int slotIndex)
        {
            UIManager.Instance.ShowConfirmation("Bu kaydı silmek istediğinize emin misiniz?", () =>
            {
                SaveManager.Instance.DeleteSave(slotIndex);
                RefreshUI();
            });
        }

        private void RefreshUI()
        {
            // Mevcut slotları temizle
            foreach (var slot in _slotButtons)
            {
                Destroy(slot);
            }
            _slotButtons.Clear();

            // UI'ı yeniden oluştur
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            BuildUI();
        }
    }
}
