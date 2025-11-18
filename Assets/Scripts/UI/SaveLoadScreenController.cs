using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Kayıt/Yükleme ekranı kontrolcüsü - Kayıt slotlarını yönetir.
    /// </summary>
    public class SaveLoadScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private List<GameObject> _slotCards = new List<GameObject>();

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
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

            // Auto-save slotu
            if (SaveManager.Instance.HasAutoSave())
            {
                AddSectionTitle(content, "Otomatik Kayıt");
                CreateAutoSaveSlot(content);
            }

            // Kayıt slotları
            AddSectionTitle(content, "Kayıt Slotları");

            var slots = SaveManager.Instance.GetAllSaveSlots();
            foreach (var slot in slots)
            {
                CreateSaveSlot(content, slot);
            }

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

        private void CreateAutoSaveSlot(Transform parent)
        {
            var saveData = SaveManager.Instance.LoadAutoSave();
            if (saveData == null) return;

            // Card container
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 120;
            cardLayout.preferredHeight = 120;

            // Karakter bilgisi
            var nameObj = _factory.CreateText(card.transform, saveData.characterName, UIStyles.BodyText);
            var nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0.05f, 0.6f);
            nameRect.anchorMax = new Vector2(0.7f, 0.95f);
            nameRect.offsetMin = Vector2.zero;
            nameRect.offsetMax = Vector2.zero;
            nameObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            nameObj.GetComponent<Text>().fontStyle = FontStyle.Bold;

            // Yaş ve tarih
            var infoText = $"Yaş: {saveData.characterAge} | {saveData.saveDate}";
            var infoObj = _factory.CreateText(card.transform, infoText, UIStyles.SmallText);
            var infoRect = infoObj.GetComponent<RectTransform>();
            infoRect.anchorMin = new Vector2(0.05f, 0.35f);
            infoRect.anchorMax = new Vector2(0.95f, 0.6f);
            infoRect.offsetMin = Vector2.zero;
            infoRect.offsetMax = Vector2.zero;
            infoObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Yükle butonu
            var loadButton = _factory.CreateButton(card.transform, "Yükle", () =>
            {
                UIManager.Instance.ShowConfirmation(
                    "Otomatik kayıt yüklensin mi?\nKaydedilmemiş ilerleme kaybolacak.",
                    () =>
                    {
                        var data = SaveManager.Instance.LoadAutoSave();
                        if (data != null)
                        {
                            GameManager.Instance.CurrentCharacter = data.Character;
                            GameManager.Instance.ChangeState(GameState.Playing);
                        }
                    }
                );
            }, UIStyles.PrimaryButton);
            var loadRect = loadButton.GetComponent<RectTransform>();
            loadRect.anchorMin = new Vector2(0.55f, 0.05f);
            loadRect.anchorMax = new Vector2(0.95f, 0.32f);
            loadRect.offsetMin = Vector2.zero;
            loadRect.offsetMax = Vector2.zero;

            _slotCards.Add(card);
        }

        private void CreateSaveSlot(Transform parent, SaveSlotInfo slot)
        {
            // Card container
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 140;
            cardLayout.preferredHeight = 140;

            if (slot.isEmpty)
            {
                // Boş slot
                var emptyText = _factory.CreateText(card.transform, $"Slot {slot.slotIndex + 1} - Boş", UIStyles.BodyText);
                var emptyRect = emptyText.GetComponent<RectTransform>();
                emptyRect.anchorMin = new Vector2(0.05f, 0.5f);
                emptyRect.anchorMax = new Vector2(0.6f, 0.9f);
                emptyRect.offsetMin = Vector2.zero;
                emptyRect.offsetMax = Vector2.zero;
                emptyText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                emptyText.GetComponent<Text>().color = UIStyles.SubtextColor;

                // Sadece oyun oynanıyorsa kaydet butonu göster
                if (GameManager.Instance.IsPlaying)
                {
                    var saveButton = _factory.CreateButton(card.transform, "Kaydet", () =>
                    {
                        SaveGame(slot.slotIndex);
                    }, UIStyles.PrimaryButton);
                    var saveRect = saveButton.GetComponent<RectTransform>();
                    saveRect.anchorMin = new Vector2(0.55f, 0.3f);
                    saveRect.anchorMax = new Vector2(0.95f, 0.7f);
                    saveRect.offsetMin = Vector2.zero;
                    saveRect.offsetMax = Vector2.zero;
                }
            }
            else
            {
                // Dolu slot
                // Slot numarası ve karakter adı
                var nameObj = _factory.CreateText(card.transform, $"Slot {slot.slotIndex + 1}: {slot.characterName}", UIStyles.BodyText);
                var nameRect = nameObj.GetComponent<RectTransform>();
                nameRect.anchorMin = new Vector2(0.05f, 0.65f);
                nameRect.anchorMax = new Vector2(0.95f, 0.95f);
                nameRect.offsetMin = Vector2.zero;
                nameRect.offsetMax = Vector2.zero;
                nameObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                nameObj.GetComponent<Text>().fontStyle = FontStyle.Bold;

                // Yaş ve tarih
                var infoText = $"Yaş: {slot.characterAge} | {slot.saveDate}";
                var infoObj = _factory.CreateText(card.transform, infoText, UIStyles.SmallText);
                var infoRect = infoObj.GetComponent<RectTransform>();
                infoRect.anchorMin = new Vector2(0.05f, 0.4f);
                infoRect.anchorMax = new Vector2(0.95f, 0.65f);
                infoRect.offsetMin = Vector2.zero;
                infoRect.offsetMax = Vector2.zero;
                infoObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

                // Butonlar
                // Yükle butonu
                var loadButton = _factory.CreateButton(card.transform, "Yükle", () =>
                {
                    LoadGame(slot.slotIndex);
                }, UIStyles.PrimaryButton);
                var loadRect = loadButton.GetComponent<RectTransform>();
                loadRect.anchorMin = new Vector2(0.05f, 0.05f);
                loadRect.anchorMax = new Vector2(0.32f, 0.35f);
                loadRect.offsetMin = Vector2.zero;
                loadRect.offsetMax = Vector2.zero;

                // Üzerine Yaz butonu (sadece oyun oynanıyorsa)
                if (GameManager.Instance.IsPlaying)
                {
                    var overwriteButton = _factory.CreateButton(card.transform, "Üzerine Yaz", () =>
                    {
                        UIManager.Instance.ShowConfirmation(
                            $"'{slot.characterName}' kaydının üzerine yazılacak.\n\nEmin misiniz?",
                            () => SaveGame(slot.slotIndex)
                        );
                    }, UIStyles.SecondaryButton);
                    var overwriteRect = overwriteButton.GetComponent<RectTransform>();
                    overwriteRect.anchorMin = new Vector2(0.35f, 0.05f);
                    overwriteRect.anchorMax = new Vector2(0.65f, 0.35f);
                    overwriteRect.offsetMin = Vector2.zero;
                    overwriteRect.offsetMax = Vector2.zero;
                }

                // Sil butonu
                var deleteButton = _factory.CreateButton(card.transform, "Sil", () =>
                {
                    UIManager.Instance.ShowConfirmation(
                        $"'{slot.characterName}' kaydı silinecek.\n\nEmin misiniz?",
                        () =>
                        {
                            SaveManager.Instance.DeleteSave(slot.slotIndex);
                            RefreshUI();
                        }
                    );
                }, UIStyles.DangerButton);
                var deleteRect = deleteButton.GetComponent<RectTransform>();
                deleteRect.anchorMin = new Vector2(0.68f, 0.05f);
                deleteRect.anchorMax = new Vector2(0.95f, 0.35f);
                deleteRect.offsetMin = Vector2.zero;
                deleteRect.offsetMax = Vector2.zero;
            }

            _slotCards.Add(card);
        }

        private void SaveGame(int slotIndex)
        {
            SaveManager.Instance.SaveGame(slotIndex);
            UIManager.Instance.ShowInfo("Başarılı", "Oyun kaydedildi.");
            RefreshUI();
        }

        private void LoadGame(int slotIndex)
        {
            UIManager.Instance.ShowConfirmation(
                "Oyun yüklensin mi?\nKaydedilmemiş ilerleme kaybolacak.",
                () =>
                {
                    GameManager.Instance.LoadGame(slotIndex);
                }
            );
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

        private void RefreshUI()
        {
            // Mevcut kartları temizle
            _slotCards.Clear();

            // UI'ı yeniden oluştur
            foreach (Transform child in transform)
            {
                // Geri butonu ve başlığı koruyalım (ilk iki child)
                if (child.GetSiblingIndex() >= 2)
                {
                    Destroy(child.gameObject);
                }
            }
            BuildUI();
        }
    }
}
