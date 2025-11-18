using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Ölüm ekranı kontrolcüsü - Miras özeti ve nesil geçişi seçeneklerini yönetir.
    /// </summary>
    public class DeathScreenController : MonoBehaviour
    {
        // UI referansları
        private Text _titleText;
        private Text _epitaphText;
        private Text _inheritanceText;
        private Text _familyHistoryText;
        private GameObject _childrenContainer;
        private List<GameObject> _childButtons = new List<GameObject>();

        // Cached data
        private List<Relationship> _availableChildren;
        private decimal _totalInheritance;
        private string _deathCause;
        private int _deathAge;

        private UIFactory _factory;

        #region Lifecycle

        private void Start()
        {
            _factory = UIManager.Instance.Factory;

            // Event'lere abone ol
            EventBus.Subscribe<CharacterDiedEvent>(OnCharacterDied);
            EventBus.Subscribe<ContinueAsChildAvailableEvent>(OnContinueAsChildAvailable);
            EventBus.Subscribe<InheritanceDistributedEvent>(OnInheritanceDistributed);

            // UI'ı oluştur
            CreateUI();
        }

        private void OnDestroy()
        {
            // Event aboneliklerini kaldır
            EventBus.Unsubscribe<CharacterDiedEvent>(OnCharacterDied);
            EventBus.Unsubscribe<ContinueAsChildAvailableEvent>(OnContinueAsChildAvailable);
            EventBus.Unsubscribe<InheritanceDistributedEvent>(OnInheritanceDistributed);
        }

        private void OnEnable()
        {
            // Ekran her açıldığında UI'ı güncelle
            RefreshUI();
        }

        #endregion

        #region UI Creation

        private void CreateUI()
        {
            // R.I.P Başlık
            _titleText = _factory.CreateText(transform, "Huzur Icinde Yat", UIStyles.TitleText);
            var titleRect = _titleText.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.1f, 0.85f);
            titleRect.anchorMax = new Vector2(0.9f, 0.95f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // Epitaph (mezar yazısı)
            _epitaphText = _factory.CreateText(transform, "", UIStyles.BodyText);
            var epitaphRect = _epitaphText.GetComponent<RectTransform>();
            epitaphRect.anchorMin = new Vector2(0.1f, 0.7f);
            epitaphRect.anchorMax = new Vector2(0.9f, 0.84f);
            epitaphRect.offsetMin = Vector2.zero;
            epitaphRect.offsetMax = Vector2.zero;

            // Miras özeti
            _inheritanceText = _factory.CreateText(transform, "", UIStyles.BodyText);
            var inheritanceRect = _inheritanceText.GetComponent<RectTransform>();
            inheritanceRect.anchorMin = new Vector2(0.1f, 0.55f);
            inheritanceRect.anchorMax = new Vector2(0.9f, 0.69f);
            inheritanceRect.offsetMin = Vector2.zero;
            inheritanceRect.offsetMax = Vector2.zero;

            // Aile tarihi özeti
            _familyHistoryText = _factory.CreateText(transform, "", UIStyles.SmallText);
            var historyRect = _familyHistoryText.GetComponent<RectTransform>();
            historyRect.anchorMin = new Vector2(0.1f, 0.42f);
            historyRect.anchorMax = new Vector2(0.9f, 0.54f);
            historyRect.offsetMin = Vector2.zero;
            historyRect.offsetMax = Vector2.zero;

            // Çocuklar container
            _childrenContainer = new GameObject("ChildrenContainer");
            _childrenContainer.transform.SetParent(transform, false);
            var childRect = _childrenContainer.AddComponent<RectTransform>();
            childRect.anchorMin = new Vector2(0.1f, 0.22f);
            childRect.anchorMax = new Vector2(0.9f, 0.41f);
            childRect.offsetMin = Vector2.zero;
            childRect.offsetMax = Vector2.zero;

            // Ana Menü butonu
            var menuButton = _factory.CreateButton(transform, "Ana Menu",
                () => GameManager.Instance.ReturnToMainMenu(), UIStyles.SecondaryButton);
            var menuRect = menuButton.GetComponent<RectTransform>();
            menuRect.anchorMin = new Vector2(0.25f, 0.05f);
            menuRect.anchorMax = new Vector2(0.75f, 0.12f);
            menuRect.offsetMin = Vector2.zero;
            menuRect.offsetMax = Vector2.zero;

            // Yeni Hayat butonu
            var newLifeButton = _factory.CreateButton(transform, "Yeni Hayat",
                () => GameManager.Instance.StartNewGame(), UIStyles.PrimaryButton);
            var newLifeRect = newLifeButton.GetComponent<RectTransform>();
            newLifeRect.anchorMin = new Vector2(0.25f, 0.13f);
            newLifeRect.anchorMax = new Vector2(0.75f, 0.20f);
            newLifeRect.offsetMin = Vector2.zero;
            newLifeRect.offsetMax = Vector2.zero;
        }

        #endregion

        #region Event Handlers

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            _deathCause = e.DeathCause;
            _deathAge = e.Age;

            // Epitaph güncelle
            if (_epitaphText != null)
            {
                _epitaphText.text = e.Epitaph + "\n\n" + e.DeathCause;
            }

            RefreshFamilyHistory();
        }

        private void OnContinueAsChildAvailable(ContinueAsChildAvailableEvent e)
        {
            _availableChildren = e.AvailableChildren;
            _totalInheritance = e.TotalInheritance;

            // Çocuk butonlarını oluştur
            CreateChildButtons();
        }

        private void OnInheritanceDistributed(InheritanceDistributedEvent e)
        {
            // Miras özeti güncelle
            if (_inheritanceText != null)
            {
                string inheritanceInfo = $"Miras Dagitimi\n" +
                    $"Toplam Miras: {e.TotalEstate:N0} TL\n" +
                    $"Veraset Vergisi: {e.TaxPaid:N0} TL\n" +
                    $"Mirascilar: {e.BeneficiaryCount} kisi";

                _inheritanceText.text = inheritanceInfo;
            }
        }

        #endregion

        #region UI Updates

        private void RefreshUI()
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return;

            // Nesil bilgisini başlığa ekle
            if (_titleText != null)
            {
                int generation = character.legacy?.generation ?? 1;
                if (generation > 1)
                {
                    _titleText.text = $"Huzur Icinde Yat\n{generation}. Nesil";
                }
            }

            RefreshFamilyHistory();
        }

        private void RefreshFamilyHistory()
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null || _familyHistoryText == null) return;

            var history = character.legacy?.familyHistory;
            if (history != null && history.Count > 0)
            {
                string historyText = "Aile Tarihi:\n";

                // Son 3 nesli göster
                var recentHistory = history.TakeLast(3).ToList();
                foreach (var record in recentHistory)
                {
                    historyText += $"- {record.characterName} ({record.birthYear}-{record.deathYear})\n";
                }

                historyText += $"\nToplam Aile Serveti: {character.legacy.totalFamilyWealth:N0} TL";

                _familyHistoryText.text = historyText;
            }
            else
            {
                _familyHistoryText.text = "";
            }
        }

        private void CreateChildButtons()
        {
            // Önceki butonları temizle
            foreach (var button in _childButtons)
            {
                if (button != null)
                {
                    Destroy(button);
                }
            }
            _childButtons.Clear();

            if (_availableChildren == null || _availableChildren.Count == 0)
            {
                // Çocuk yoksa bilgi mesajı göster
                var noChildText = _factory.CreateText(_childrenContainer.transform,
                    "Mirasci cocuk bulunmuyor.", UIStyles.SmallText);
                var textRect = noChildText.GetComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.offsetMin = Vector2.zero;
                textRect.offsetMax = Vector2.zero;
                _childButtons.Add(noChildText.gameObject);
                return;
            }

            // Başlık
            var titleObj = _factory.CreateText(_childrenContainer.transform,
                "Cocuk Olarak Devam Et:", UIStyles.SubtitleText);
            var titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0f, 0.75f);
            titleRect.anchorMax = new Vector2(1f, 1f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;
            _childButtons.Add(titleObj.gameObject);

            // Her çocuk için buton oluştur
            float buttonHeight = 0.22f;
            float spacing = 0.02f;
            float currentY = 0.72f;

            int maxButtons = Mathf.Min(_availableChildren.Count, 3); // En fazla 3 çocuk göster

            for (int i = 0; i < maxButtons; i++)
            {
                var child = _availableChildren[i];
                string childId = child.npcId;

                // Miras payını hesapla
                decimal childShare = _totalInheritance;
                if (_availableChildren.Count > 1)
                {
                    childShare = _totalInheritance / _availableChildren.Count;
                }

                string buttonText = $"{child.npcName} ({child.age} yas)\nMiras: {childShare:N0} TL";

                var button = _factory.CreateButton(_childrenContainer.transform, buttonText,
                    () => OnChildSelected(childId), UIStyles.PrimaryButton);

                var buttonRect = button.GetComponent<RectTransform>();
                buttonRect.anchorMin = new Vector2(0f, currentY - buttonHeight);
                buttonRect.anchorMax = new Vector2(1f, currentY);
                buttonRect.offsetMin = Vector2.zero;
                buttonRect.offsetMax = Vector2.zero;

                _childButtons.Add(button);
                currentY -= (buttonHeight + spacing);
            }

            // Daha fazla çocuk varsa bilgi göster
            if (_availableChildren.Count > 3)
            {
                var moreText = _factory.CreateText(_childrenContainer.transform,
                    $"+{_availableChildren.Count - 3} daha fazla cocuk...", UIStyles.SmallText);
                var moreRect = moreText.GetComponent<RectTransform>();
                moreRect.anchorMin = new Vector2(0f, 0f);
                moreRect.anchorMax = new Vector2(1f, 0.1f);
                moreRect.offsetMin = Vector2.zero;
                moreRect.offsetMax = Vector2.zero;
                _childButtons.Add(moreText.gameObject);
            }
        }

        private void OnChildSelected(string childNpcId)
        {
            Debug.Log($"[DeathScreenController] Selected child: {childNpcId}");

            // Çocuk olarak devam et
            GameManager.Instance.ContinueAsChild(childNpcId);
        }

        #endregion
    }
}
