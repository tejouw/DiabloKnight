using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Kayıt/Yükleme ekranı kontrolcüsü - Oyun kayıtlarını yönetir.
    /// </summary>
    public class SaveLoadScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private Transform _slotsContainer;
        private List<GameObject> _slotCards = new List<GameObject>();

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
                UIManager.Instance.ShowScreen(ScreenType.MainMenu);
            }, UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(transform, "Kayıtlı Oyunlar", UIStyles.TitleText);
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

            _slotsContainer = scrollView.transform.Find("Viewport/Content");

            // Kayıt slotlarını yükle
            RefreshSlots();

            // Auto-save varsa göster
            if (SaveManager.Instance != null && SaveManager.Instance.HasAutoSave())
            {
                AddAutoSaveSlot();
            }

            // Tüm kayıtları sil butonu
            var deleteAllButton = _factory.CreateButton(transform, "Tümünü Sil", () =>
            {
                UIManager.Instance.ShowConfirmation(
                    "Tüm kayıtları silmek istediğinize emin misiniz?",
                    () => {
                        SaveManager.Instance?.DeleteAllSaves();
                        RefreshSlots();
                    }
                );
            }, UIStyles.DangerButton);
            var deleteAllRect = deleteAllButton.GetComponent<RectTransform>();
            deleteAllRect.anchorMin = new Vector2(0.25f, 0.01f);
            deleteAllRect.anchorMax = new Vector2(0.75f, 0.07f);
            deleteAllRect.offsetMin = Vector2.zero;
            deleteAllRect.offsetMax = Vector2.zero;
        }

        private void RefreshSlots()
        {
            // Eski slotları temizle
            foreach (var card in _slotCards)
            {
                Destroy(card);
            }
            _slotCards.Clear();

            // Kayıt slotlarını al
            var slots = SaveManager.Instance?.GetAllSaveSlots();
            if (slots == null) return;

            foreach (var slotInfo in slots)
            {
                CreateSlotCard(slotInfo);
            }
        }

        private void AddAutoSaveSlot()
        {
            // Otomatik kayıt bölüm başlığı
            var autoTitle = _factory.CreateText(_slotsContainer, "Otomatik Kayıt", UIStyles.SubtitleText);
            var autoLayout = autoTitle.AddComponent<LayoutElement>();
            autoLayout.minHeight = 50;
            autoLayout.preferredHeight = 50;
            var autoText = autoTitle.GetComponent<Text>();
            autoText.alignment = TextAnchor.MiddleLeft;
            autoText.color = UIStyles.AccentColor;

            // Auto-save kartı
            var autoCard = _factory.CreatePanel(_slotsContainer, UIStyles.CardPanel);
            var cardLayout = autoCard.AddComponent<LayoutElement>();
            cardLayout.minHeight = 100;
            cardLayout.preferredHeight = 100;

            // Auto-save bilgisi
            var autoInfo = _factory.CreateText(autoCard.transform, "Son otomatik kayıt", UIStyles.BodyText);
            var infoRect = autoInfo.GetComponent<RectTransform>();
            infoRect.anchorMin = new Vector2(0.05f, 0.5f);
            infoRect.anchorMax = new Vector2(0.6f, 0.9f);
            infoRect.offsetMin = Vector2.zero;
            infoRect.offsetMax = Vector2.zero;
            autoInfo.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Yükle butonu
            var loadButton = _factory.CreateButton(autoCard.transform, "Yükle", () =>
            {
                var saveData = SaveManager.Instance?.LoadAutoSave();
                if (saveData != null)
                {
                    GameManager.Instance?.LoadGame(-1);
                }
            }, UIStyles.PrimaryButton);
            var loadRect = loadButton.GetComponent<RectTransform>();
            loadRect.anchorMin = new Vector2(0.65f, 0.2f);
            loadRect.anchorMax = new Vector2(0.95f, 0.8f);
            loadRect.offsetMin = Vector2.zero;
            loadRect.offsetMax = Vector2.zero;

            _slotCards.Add(autoTitle);
            _slotCards.Add(autoCard);

            // Normal kayıtlar başlığı
            var normalTitle = _factory.CreateText(_slotsContainer, "Kayıt Slotları", UIStyles.SubtitleText);
            var normalLayout = normalTitle.AddComponent<LayoutElement>();
            normalLayout.minHeight = 50;
            normalLayout.preferredHeight = 50;
            var normalText = normalTitle.GetComponent<Text>();
            normalText.alignment = TextAnchor.MiddleLeft;
            normalText.color = UIStyles.PrimaryColor;

            _slotCards.Add(normalTitle);
        }

        private void CreateSlotCard(SaveSlotInfo slotInfo)
        {
            var card = _factory.CreatePanel(_slotsContainer, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 120;
            cardLayout.preferredHeight = 120;

            int slotIndex = slotInfo.slotIndex;

            if (slotInfo.isEmpty)
            {
                // Boş slot
                var emptyText = _factory.CreateText(card.transform, $"Slot {slotIndex + 1} - Boş", UIStyles.BodyText);
                var emptyRect = emptyText.GetComponent<RectTransform>();
                emptyRect.anchorMin = new Vector2(0.05f, 0.5f);
                emptyRect.anchorMax = new Vector2(0.6f, 0.9f);
                emptyRect.offsetMin = Vector2.zero;
                emptyRect.offsetMax = Vector2.zero;
                emptyText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                emptyText.GetComponent<Text>().color = UIStyles.SubtextColor;

                // Kaydet butonu (sadece oyun oynanıyorsa)
                if (GameManager.Instance != null && GameManager.Instance.IsPlaying)
                {
                    var saveButton = _factory.CreateButton(card.transform, "Kaydet", () =>
                    {
                        SaveManager.Instance?.SaveGame(slotIndex);
                        RefreshSlots();
                    }, UIStyles.PrimaryButton);
                    var saveRect = saveButton.GetComponent<RectTransform>();
                    saveRect.anchorMin = new Vector2(0.65f, 0.2f);
                    saveRect.anchorMax = new Vector2(0.95f, 0.8f);
                    saveRect.offsetMin = Vector2.zero;
                    saveRect.offsetMax = Vector2.zero;
                }
            }
            else
            {
                // Dolu slot - Karakter bilgileri
                var nameText = _factory.CreateText(card.transform, slotInfo.characterName, UIStyles.BodyText);
                var nameRect = nameText.GetComponent<RectTransform>();
                nameRect.anchorMin = new Vector2(0.05f, 0.6f);
                nameRect.anchorMax = new Vector2(0.6f, 0.95f);
                nameRect.offsetMin = Vector2.zero;
                nameRect.offsetMax = Vector2.zero;
                nameText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Yaş ve tarih
                var infoText = _factory.CreateText(card.transform, $"Yaş: {slotInfo.characterAge} | {slotInfo.saveDate}", UIStyles.SmallText);
                var infoRect = infoText.GetComponent<RectTransform>();
                infoRect.anchorMin = new Vector2(0.05f, 0.35f);
                infoRect.anchorMax = new Vector2(0.6f, 0.6f);
                infoRect.offsetMin = Vector2.zero;
                infoRect.offsetMax = Vector2.zero;
                infoText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Slot numarası
                var slotNumText = _factory.CreateText(card.transform, $"Slot {slotIndex + 1}", UIStyles.SmallText);
                var slotNumRect = slotNumText.GetComponent<RectTransform>();
                slotNumRect.anchorMin = new Vector2(0.05f, 0.05f);
                slotNumRect.anchorMax = new Vector2(0.3f, 0.35f);
                slotNumRect.offsetMin = Vector2.zero;
                slotNumRect.offsetMax = Vector2.zero;
                slotNumText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Yükle butonu
                var loadButton = _factory.CreateButton(card.transform, "Yükle", () =>
                {
                    GameManager.Instance?.LoadGame(slotIndex);
                }, UIStyles.PrimaryButton);
                var loadRect = loadButton.GetComponent<RectTransform>();
                loadRect.anchorMin = new Vector2(0.42f, 0.15f);
                loadRect.anchorMax = new Vector2(0.68f, 0.85f);
                loadRect.offsetMin = Vector2.zero;
                loadRect.offsetMax = Vector2.zero;

                // Sil butonu
                var deleteButton = _factory.CreateButton(card.transform, "Sil", () =>
                {
                    UIManager.Instance.ShowConfirmation(
                        $"{slotInfo.characterName} kaydını silmek istediğinize emin misiniz?",
                        () => {
                            SaveManager.Instance?.DeleteSave(slotIndex);
                            RefreshSlots();
                        }
                    );
                }, UIStyles.DangerButton);
                var deleteRect = deleteButton.GetComponent<RectTransform>();
                deleteRect.anchorMin = new Vector2(0.72f, 0.15f);
                deleteRect.anchorMax = new Vector2(0.95f, 0.85f);
                deleteRect.offsetMin = Vector2.zero;
                deleteRect.offsetMax = Vector2.zero;
            }

            _slotCards.Add(card);
        }
    }
}
