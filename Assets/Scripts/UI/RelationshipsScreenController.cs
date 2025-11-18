using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// İlişkiler ekranı kontrolcüsü - Aile ve sosyal ilişkileri gösterir.
    /// </summary>
    public class RelationshipsScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private Transform _contentContainer;
        private GameObject _scrollView;

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            EventBus.Subscribe<RelationshipChangedEvent>(OnRelationshipChanged);
            EventBus.Subscribe<AgeProgressedEvent>(OnAgeProgressed);
        }

        private void UnsubscribeFromEvents()
        {
            EventBus.Unsubscribe<RelationshipChangedEvent>(OnRelationshipChanged);
            EventBus.Unsubscribe<AgeProgressedEvent>(OnAgeProgressed);
        }

        private void OnRelationshipChanged(RelationshipChangedEvent evt)
        {
            RefreshRelationships();
        }

        private void OnAgeProgressed(AgeProgressedEvent evt)
        {
            RefreshRelationships();
        }

        public void RefreshRelationships()
        {
            if (_contentContainer == null) return;

            // Clear existing content
            foreach (Transform child in _contentContainer)
            {
                Destroy(child.gameObject);
            }

            // Rebuild relationships
            PopulateRelationships();
        }

        private void BuildUI()
        {
            // Geri butonu
            var backButton = _factory.CreateButton(transform, "< Geri", () =>
            {
                UIManager.Instance.ShowScreen(ScreenType.Game);
            }, UIStyles.SecondaryButton);
            var backRect = backButton.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.02f, 0.93f);
            backRect.anchorMax = new Vector2(0.25f, 0.98f);
            backRect.offsetMin = Vector2.zero;
            backRect.offsetMax = Vector2.zero;

            // Başlık
            var title = _factory.CreateText(transform, "İlişkiler", UIStyles.TitleText);
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.3f, 0.93f);
            titleRect.anchorMax = new Vector2(0.7f, 0.98f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            // ScrollView
            _scrollView = _factory.CreateScrollView(transform);
            var scrollRect = _scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 0.9f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            _contentContainer = _scrollView.transform.Find("Viewport/Content");

            // Populate relationships
            PopulateRelationships();
        }

        private void PopulateRelationships()
        {
            var character = GameManager.Instance.CurrentCharacter;
            if (character == null) return;

            var content = _contentContainer;

            // İlişkileri kategorilere göre sırala
            var parents = character.Relationships.FindAll(r => r.type == RelationType.Parent);
            var spouse = character.Relationships.FindAll(r => r.type == RelationType.Spouse);
            var siblings = character.Relationships.FindAll(r => r.type == RelationType.Sibling);
            var children = character.Relationships.FindAll(r => r.type == RelationType.Child);
            var friends = character.Relationships.FindAll(r => r.type == RelationType.Friend || r.type == RelationType.BestFriend);
            var romantic = character.Relationships.FindAll(r => r.type == RelationType.Boyfriend || r.type == RelationType.Girlfriend);
            var others = character.Relationships.FindAll(r =>
                r.type != RelationType.Parent &&
                r.type != RelationType.Spouse &&
                r.type != RelationType.Sibling &&
                r.type != RelationType.Child &&
                r.type != RelationType.Friend &&
                r.type != RelationType.BestFriend &&
                r.type != RelationType.Boyfriend &&
                r.type != RelationType.Girlfriend);

            // Aile
            if (parents.Count > 0 || spouse.Count > 0 || siblings.Count > 0 || children.Count > 0)
            {
                AddSectionTitle(content, "Aile");

                foreach (var rel in spouse)
                {
                    AddRelationshipCard(content, rel, "Eş");
                }

                foreach (var rel in parents)
                {
                    string label = rel.gender == Gender.Male ? "Baba" : "Anne";
                    AddRelationshipCard(content, rel, label);
                }

                foreach (var rel in siblings)
                {
                    string label = rel.gender == Gender.Male ? "Kardeş (E)" : "Kardeş (K)";
                    AddRelationshipCard(content, rel, label);
                }

                foreach (var rel in children)
                {
                    string label = rel.gender == Gender.Male ? "Oğul" : "Kız";
                    AddRelationshipCard(content, rel, label);
                }
            }

            // Romantik
            if (romantic.Count > 0)
            {
                AddSectionTitle(content, "Romantik İlişkiler");

                foreach (var rel in romantic)
                {
                    string label = rel.type == RelationType.Boyfriend ? "Erkek Arkadaş" : "Kız Arkadaş";
                    AddRelationshipCard(content, rel, label);
                }
            }

            // Arkadaşlar
            if (friends.Count > 0)
            {
                AddSectionTitle(content, "Arkadaşlar");

                foreach (var rel in friends)
                {
                    string label = rel.type == RelationType.BestFriend ? "Yakın Arkadaş" : "Arkadaş";
                    AddRelationshipCard(content, rel, label);
                }
            }

            // Diğer
            if (others.Count > 0)
            {
                AddSectionTitle(content, "Diğer");

                foreach (var rel in others)
                {
                    AddRelationshipCard(content, rel, GetRelationTypeText(rel.type));
                }
            }

            // İlişki yoksa
            if (character.Relationships.Count == 0)
            {
                var noRelText = _factory.CreateText(content, "Henüz bir ilişkin yok.", UIStyles.BodyText);
                var layoutElement = noRelText.AddComponent<LayoutElement>();
                layoutElement.minHeight = 100;
            }
        }

        private void AddSectionTitle(Transform parent, string title)
        {
            var titleObj = _factory.CreateText(parent, title, UIStyles.SubtitleText);
            var layoutElement = titleObj.AddComponent<LayoutElement>();
            layoutElement.minHeight = 50;
            layoutElement.preferredHeight = 50;

            var text = titleObj.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleLeft;
            text.color = UIStyles.PrimaryColor;
        }

        private void AddRelationshipCard(Transform parent, Relationship rel, string typeLabel)
        {
            // Card container
            var card = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var cardLayout = card.AddComponent<LayoutElement>();
            cardLayout.minHeight = 100;
            cardLayout.preferredHeight = 100;

            // İsim ve tip
            var nameObj = _factory.CreateText(card.transform, rel.npcName, UIStyles.BodyText);
            var nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0.05f, 0.6f);
            nameRect.anchorMax = new Vector2(0.7f, 0.95f);
            nameRect.offsetMin = Vector2.zero;
            nameRect.offsetMax = Vector2.zero;
            nameObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Tip etiketi
            var typeObj = _factory.CreateText(card.transform, typeLabel, UIStyles.SmallText);
            var typeRect = typeObj.GetComponent<RectTransform>();
            typeRect.anchorMin = new Vector2(0.7f, 0.6f);
            typeRect.anchorMax = new Vector2(0.95f, 0.95f);
            typeRect.offsetMin = Vector2.zero;
            typeRect.offsetMax = Vector2.zero;
            var typeText = typeObj.GetComponent<Text>();
            typeText.alignment = TextAnchor.MiddleRight;
            typeText.color = UIStyles.SubtextColor;

            // Yaş
            if (rel.age > 0)
            {
                var ageObj = _factory.CreateText(card.transform, $"Yaş: {rel.age}", UIStyles.SmallText);
                var ageRect = ageObj.GetComponent<RectTransform>();
                ageRect.anchorMin = new Vector2(0.05f, 0.35f);
                ageRect.anchorMax = new Vector2(0.3f, 0.6f);
                ageRect.offsetMin = Vector2.zero;
                ageRect.offsetMax = Vector2.zero;
                ageObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            }

            // İlişki durumu
            var statusObj = _factory.CreateText(card.transform, GetStatusText(rel.status), UIStyles.SmallText);
            var statusRect = statusObj.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0.7f, 0.35f);
            statusRect.anchorMax = new Vector2(0.95f, 0.6f);
            statusRect.offsetMin = Vector2.zero;
            statusRect.offsetMax = Vector2.zero;
            var statusText = statusObj.GetComponent<Text>();
            statusText.alignment = TextAnchor.MiddleRight;
            statusText.color = GetStatusColor(rel.status);

            // Yakınlık barı
            var intimacyLabel = _factory.CreateText(card.transform, "Yakınlık", UIStyles.SmallText);
            var intimacyLabelRect = intimacyLabel.GetComponent<RectTransform>();
            intimacyLabelRect.anchorMin = new Vector2(0.05f, 0.05f);
            intimacyLabelRect.anchorMax = new Vector2(0.2f, 0.35f);
            intimacyLabelRect.offsetMin = Vector2.zero;
            intimacyLabelRect.offsetMax = Vector2.zero;
            intimacyLabel.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            var intimacyBar = _factory.CreateProgressBar(card.transform, UIStyles.PrimaryColor, rel.intimacy / 100f);
            var intimacyBarRect = intimacyBar.GetComponent<RectTransform>();
            intimacyBarRect.anchorMin = new Vector2(0.22f, 0.12f);
            intimacyBarRect.anchorMax = new Vector2(0.85f, 0.28f);
            intimacyBarRect.offsetMin = Vector2.zero;
            intimacyBarRect.offsetMax = Vector2.zero;

            var intimacyValue = _factory.CreateText(card.transform, rel.intimacy.ToString(), UIStyles.SmallText);
            var intimacyValueRect = intimacyValue.GetComponent<RectTransform>();
            intimacyValueRect.anchorMin = new Vector2(0.87f, 0.05f);
            intimacyValueRect.anchorMax = new Vector2(0.95f, 0.35f);
            intimacyValueRect.offsetMin = Vector2.zero;
            intimacyValueRect.offsetMax = Vector2.zero;
            intimacyValue.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
        }

        private string GetRelationTypeText(RelationType type)
        {
            return type switch
            {
                RelationType.Parent => "Ebeveyn",
                RelationType.Sibling => "Kardeş",
                RelationType.Child => "Çocuk",
                RelationType.Spouse => "Eş",
                RelationType.ExSpouse => "Eski Eş",
                RelationType.Friend => "Arkadaş",
                RelationType.BestFriend => "Yakın Arkadaş",
                RelationType.Enemy => "Düşman",
                RelationType.Colleague => "İş Arkadaşı",
                RelationType.Boyfriend => "Erkek Arkadaş",
                RelationType.Girlfriend => "Kız Arkadaş",
                RelationType.Ex => "Eski Sevgili",
                RelationType.Acquaintance => "Tanıdık",
                _ => "Diğer"
            };
        }

        private string GetStatusText(RelationshipStatus status)
        {
            return status switch
            {
                RelationshipStatus.Active => "Aktif",
                RelationshipStatus.Distant => "Uzak",
                RelationshipStatus.Broken => "Kopuk",
                RelationshipStatus.Deceased => "Vefat",
                _ => ""
            };
        }

        private Color GetStatusColor(RelationshipStatus status)
        {
            return status switch
            {
                RelationshipStatus.Active => UIStyles.AccentColor,
                RelationshipStatus.Distant => UIStyles.SubtextColor,
                RelationshipStatus.Broken => UIStyles.SecondaryColor,
                RelationshipStatus.Deceased => Color.gray,
                _ => UIStyles.TextColor
            };
        }
    }
}
