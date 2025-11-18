using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Aktiviteler Ekranı - BitLife benzeri aktivite menüsü.
    /// </summary>
    public class ActivitiesScreenController : MonoBehaviour
    {
        private UIFactory _factory;
        private Transform _contentParent;
        private GameObject _currentSubMenu;

        // Kategori renkleri
        private static readonly Color EducationColor = new Color(0.3f, 0.6f, 0.9f, 1f);
        private static readonly Color CareerColor = new Color(0.9f, 0.6f, 0.2f, 1f);
        private static readonly Color SocialColor = new Color(0.9f, 0.4f, 0.6f, 1f);
        private static readonly Color HealthColor = new Color(0.4f, 0.8f, 0.4f, 1f);
        private static readonly Color CrimeColor = new Color(0.6f, 0.3f, 0.3f, 1f);
        private static readonly Color AssetsColor = new Color(0.8f, 0.7f, 0.3f, 1f);

        private void Start()
        {
            _factory = UIManager.Instance.Factory;
            BuildUI();
        }

        private void BuildUI()
        {
            // ScrollView oluştur
            var scrollView = _factory.CreateScrollView(transform);
            var scrollRect = scrollView.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.02f, 0.02f);
            scrollRect.anchorMax = new Vector2(0.98f, 0.9f);
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            _contentParent = scrollView.transform.Find("Viewport/Content");

            // Ana kategorileri oluştur
            CreateMainCategories();
        }

        private void CreateMainCategories()
        {
            ClearContent();

            // Eğitim
            CreateCategoryButton("Eğitim", EducationColor, ShowEducationMenu);

            // Kariyer
            CreateCategoryButton("Kariyer", CareerColor, ShowCareerMenu);

            // Sosyal
            CreateCategoryButton("Sosyal", SocialColor, ShowSocialMenu);

            // Sağlık
            CreateCategoryButton("Sağlık & Fitness", HealthColor, ShowHealthMenu);

            // Varlıklar
            CreateCategoryButton("Varlıklar", AssetsColor, ShowAssetsMenu);

            // Suç
            CreateCategoryButton("Suç", CrimeColor, ShowCrimeMenu);

            // Diğer
            CreateCategoryButton("Diğer", UIStyles.SecondaryColor, ShowOtherMenu);
        }

        private void CreateCategoryButton(string title, Color color, Action onClick)
        {
            var button = new GameObject(title);
            button.transform.SetParent(_contentParent, false);

            var rect = button.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 80);

            var image = button.AddComponent<Image>();
            image.color = color;

            var btn = button.AddComponent<Button>();
            btn.targetGraphic = image;
            btn.onClick.AddListener(() => onClick?.Invoke());

            // Text
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(button.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(20, 0);
            textRect.offsetMax = new Vector2(-20, 0);

            var text = textObj.AddComponent<Text>();
            text.text = title;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 28;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;
        }

        private void CreateActionButton(string title, Action onClick)
        {
            var button = new GameObject(title);
            button.transform.SetParent(_contentParent, false);

            var rect = button.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 60);

            var image = button.AddComponent<Image>();
            image.color = new Color(0.25f, 0.25f, 0.3f, 1f);

            var btn = button.AddComponent<Button>();
            btn.targetGraphic = image;

            var colors = btn.colors;
            colors.highlightedColor = new Color(0.35f, 0.35f, 0.4f, 1f);
            colors.pressedColor = new Color(0.2f, 0.2f, 0.25f, 1f);
            btn.colors = colors;

            btn.onClick.AddListener(() => {
                onClick?.Invoke();
            });

            // Text
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(button.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(30, 0);
            textRect.offsetMax = new Vector2(-10, 0);

            var text = textObj.AddComponent<Text>();
            text.text = title;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 22;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;
        }

        private void CreateBackButton()
        {
            var button = new GameObject("Back");
            button.transform.SetParent(_contentParent, false);
            button.transform.SetAsFirstSibling();

            var rect = button.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 50);

            var image = button.AddComponent<Image>();
            image.color = new Color(0.4f, 0.4f, 0.45f, 1f);

            var btn = button.AddComponent<Button>();
            btn.targetGraphic = image;
            btn.onClick.AddListener(CreateMainCategories);

            // Text
            var textObj = new GameObject("Text");
            textObj.transform.SetParent(button.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(20, 0);
            textRect.offsetMax = new Vector2(-10, 0);

            var text = textObj.AddComponent<Text>();
            text.text = "< Geri";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 20;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;
        }

        private void ClearContent()
        {
            if (_contentParent == null) return;

            foreach (Transform child in _contentParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void ExecuteAction(Func<ActionResult> action)
        {
            var result = action();
            UIManager.Instance.ShowEventResult(result.Message);
            UIManager.Instance.RefreshUI();
        }

        #region Menu Builders

        private void ShowEducationMenu()
        {
            ClearContent();
            CreateBackButton();

            var character = GameManager.Instance.CurrentCharacter;

            // Yaşa göre seçenekleri göster
            if (character.Age >= 6 && character.Education.currentLevel == EducationLevel.None)
            {
                CreateActionButton("Okula Başla", () => ExecuteAction(ActionsManager.Instance.StartSchool));
            }

            if (character.Education.currentLevel != EducationLevel.None)
            {
                CreateActionButton("Ders Çalış", () => ExecuteAction(ActionsManager.Instance.Study));
                CreateActionButton("Okulu Bırak", () => ExecuteAction(ActionsManager.Instance.DropOut));
            }

            CreateActionButton("Kütüphaneye Git", () => ExecuteAction(ActionsManager.Instance.GoToLibrary));

            if (character.Age >= 17 && character.Education.currentLevel >= EducationLevel.HighSchool)
            {
                CreateActionButton("YKS Sınavına Gir", () => ExecuteAction(ActionsManager.Instance.TakeYKS));
            }

            if (character.Education.yksScore > 0 && character.Education.currentLevel != EducationLevel.University)
            {
                CreateActionButton("Üniversiteye Başvur", () => {
                    // Basit üniversite başvurusu
                    var result = ActionsManager.Instance.ApplyToUniversity("istanbul_uni");
                    UIManager.Instance.ShowEventResult(result.Message);
                    UIManager.Instance.RefreshUI();
                });
            }

            if (character.Education.currentLevel == EducationLevel.University)
            {
                CreateActionButton("Mezun Ol", () => ExecuteAction(ActionsManager.Instance.GraduateUniversity));
            }
        }

        private void ShowCareerMenu()
        {
            ClearContent();
            CreateBackButton();

            var character = GameManager.Instance.CurrentCharacter;

            if (character.Age >= 16)
            {
                CreateActionButton("İş Ara", () => ExecuteAction(ActionsManager.Instance.SearchForJob));

                // Rastgele iş başvurusu
                CreateActionButton("İşe Başvur", () => {
                    var jobs = DataManager.Instance?.GetAllJobs();
                    if (jobs != null && jobs.Count > 0)
                    {
                        var randomJob = jobs[UnityEngine.Random.Range(0, jobs.Count)];
                        var result = ActionsManager.Instance.ApplyForJob(randomJob.id);
                        UIManager.Instance.ShowEventResult(result.Message);
                        UIManager.Instance.RefreshUI();
                    }
                });

                if (character.Career.currentJob != null)
                {
                    CreateActionButton("Sıkı Çalış", () => ExecuteAction(ActionsManager.Instance.WorkHard));
                    CreateActionButton("Terfi İste", () => ExecuteAction(ActionsManager.Instance.AskForPromotion));
                    CreateActionButton("İşten Ayrıl", () => ExecuteAction(ActionsManager.Instance.QuitJob));
                }
            }
            else
            {
                CreateActionButton("Çalışmak için 16 yaşını doldurmalısın", null);
            }
        }

        private void ShowSocialMenu()
        {
            ClearContent();
            CreateBackButton();

            var character = GameManager.Instance.CurrentCharacter;

            CreateActionButton("Arkadaş Edin", () => ExecuteAction(ActionsManager.Instance.MakeFriend));
            CreateActionButton("Aile ile Vakit Geçir", () => ExecuteAction(ActionsManager.Instance.SpendTimeWithFamily));

            if (character.Age >= 14)
            {
                CreateActionButton("Sevgili Bul", () => ExecuteAction(ActionsManager.Instance.FindLove));
            }

            // Mevcut sevgili varsa
            var partner = character.Relationships.Find(r =>
                r.type == RelationType.Boyfriend || r.type == RelationType.Girlfriend);

            if (partner != null && character.Age >= 18)
            {
                CreateActionButton("Evlilik Teklifi Yap", () => ExecuteAction(ActionsManager.Instance.Propose));
            }

            if (character.IsMarried)
            {
                CreateActionButton("Çocuk Yap", () => ExecuteAction(ActionsManager.Instance.HaveChild));
                CreateActionButton("Boşan", () => ExecuteAction(ActionsManager.Instance.Divorce));
            }

            CreateActionButton("Sosyal Medya Paylaşımı", () => ExecuteAction(ActionsManager.Instance.PostOnSocialMedia));
        }

        private void ShowHealthMenu()
        {
            ClearContent();
            CreateBackButton();

            CreateActionButton("Doktora Git", () => ExecuteAction(ActionsManager.Instance.VisitDoctor));
            CreateActionButton("Spor Salonu", () => ExecuteAction(ActionsManager.Instance.GoToGym));
            CreateActionButton("Meditasyon", () => ExecuteAction(ActionsManager.Instance.Meditate));
            CreateActionButton("Yürüyüş Yap", () => ExecuteAction(ActionsManager.Instance.TakeWalk));
        }

        private void ShowAssetsMenu()
        {
            ClearContent();
            CreateBackButton();

            CreateActionButton("Ev Satın Al", () => ExecuteAction(ActionsManager.Instance.BuyHouse));
            CreateActionButton("Araba Satın Al", () => ExecuteAction(ActionsManager.Instance.BuyCar));

            // Mevcut varlıkları göster
            var character = GameManager.Instance.CurrentCharacter;
            if (character.Finances.assets.Count > 0)
            {
                CreateSectionHeader("Sahip Olduklarım:");
                foreach (var asset in character.Finances.assets)
                {
                    CreateInfoText($"• {asset}");
                }
            }
        }

        private void ShowCrimeMenu()
        {
            ClearContent();
            CreateBackButton();

            CreateActionButton("Hırsızlık Yap", () => ExecuteAction(ActionsManager.Instance.CommitTheft));

            CreateActionButton("Kumar Oyna (1000 TL)", () => {
                var result = ActionsManager.Instance.Gamble(1000);
                UIManager.Instance.ShowEventResult(result.Message);
                UIManager.Instance.RefreshUI();
            });

            CreateActionButton("Kumar Oyna (5000 TL)", () => {
                var result = ActionsManager.Instance.Gamble(5000);
                UIManager.Instance.ShowEventResult(result.Message);
                UIManager.Instance.RefreshUI();
            });
        }

        private void ShowOtherMenu()
        {
            ClearContent();
            CreateBackButton();

            var character = GameManager.Instance.CurrentCharacter;

            // Askerlik
            if (character.Gender == Gender.Male && character.Age >= 20 && !character.hasCompletedMilitary)
            {
                CreateActionButton("Askerlik Yap", () => ExecuteAction(ActionsManager.Instance.DoMilitaryService));
            }

            // Şehir değiştir
            CreateActionButton("Şehir Değiştir", () => {
                var cities = DataManager.Instance?.GetAllCities();
                if (cities != null && cities.Count > 0)
                {
                    var randomCity = cities[UnityEngine.Random.Range(0, cities.Count)];
                    var result = ActionsManager.Instance.MoveToCity(randomCity.id);
                    UIManager.Instance.ShowEventResult(result.Message);
                    UIManager.Instance.RefreshUI();
                }
            });
        }

        private void CreateSectionHeader(string text)
        {
            var header = new GameObject("Header");
            header.transform.SetParent(_contentParent, false);

            var rect = header.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 40);

            var textComp = header.AddComponent<Text>();
            textComp.text = text;
            textComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComp.fontSize = 20;
            textComp.color = UIStyles.SubtextColor;
            textComp.alignment = TextAnchor.MiddleLeft;

            var layout = header.AddComponent<LayoutElement>();
            layout.minHeight = 40;
        }

        private void CreateInfoText(string text)
        {
            var info = new GameObject("Info");
            info.transform.SetParent(_contentParent, false);

            var rect = info.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 30);

            var textComp = info.AddComponent<Text>();
            textComp.text = text;
            textComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComp.fontSize = 18;
            textComp.color = Color.white;
            textComp.alignment = TextAnchor.MiddleLeft;

            var layout = info.AddComponent<LayoutElement>();
            layout.minHeight = 30;
        }

        #endregion
    }
}
