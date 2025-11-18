using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// UI Factory - Tüm UI elemanlarını kod ile oluşturur.
    /// </summary>
    public class UIFactory
    {
        private Transform _defaultParent;

        public UIFactory(Transform defaultParent)
        {
            _defaultParent = defaultParent;
        }

        #region Panel Creation

        /// <summary>
        /// Panel oluştur.
        /// </summary>
        public GameObject CreatePanel(Transform parent, PanelStyle style)
        {
            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(parent ?? _defaultParent, false);

            // RectTransform
            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            // Image
            Image image = panel.AddComponent<Image>();
            image.color = style.backgroundColor;
            image.raycastTarget = style.raycastTarget;

            return panel;
        }

        #endregion

        #region Text Creation

        /// <summary>
        /// Text oluştur (Legacy UI).
        /// </summary>
        public GameObject CreateText(Transform parent, string content, TextStyle style)
        {
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(parent ?? _defaultParent, false);

            // RectTransform
            RectTransform rect = textObj.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            // Text component
            Text text = textObj.AddComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = style.fontSize;
            text.color = style.color;
            text.alignment = style.alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.raycastTarget = false;

            return textObj;
        }

        #endregion

        #region Button Creation

        /// <summary>
        /// Buton oluştur.
        /// </summary>
        public GameObject CreateButton(Transform parent, string text, Action onClick, ButtonStyle style)
        {
            GameObject buttonObj = new GameObject("Button");
            buttonObj.transform.SetParent(parent ?? _defaultParent, false);

            // RectTransform
            RectTransform rect = buttonObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(style.width, style.height);

            // Image (background)
            Image image = buttonObj.AddComponent<Image>();
            image.color = style.normalColor;
            image.type = Image.Type.Sliced;

            // Button component
            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = image;

            // Button colors
            ColorBlock colors = button.colors;
            colors.normalColor = style.normalColor;
            colors.highlightedColor = style.highlightedColor;
            colors.pressedColor = style.pressedColor;
            colors.disabledColor = style.disabledColor;
            colors.fadeDuration = 0.1f;
            button.colors = colors;

            // Click event
            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick());
            }

            // Text child
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 5);
            textRect.offsetMax = new Vector2(-10, -5);

            Text buttonText = textObj.AddComponent<Text>();
            buttonText.text = text;
            buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            buttonText.fontSize = style.fontSize;
            buttonText.color = style.textColor;
            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.raycastTarget = false;

            return buttonObj;
        }

        #endregion

        #region ScrollView Creation

        /// <summary>
        /// ScrollView oluştur.
        /// </summary>
        public GameObject CreateScrollView(Transform parent)
        {
            // Ana ScrollView objesi
            GameObject scrollViewObj = new GameObject("ScrollView");
            scrollViewObj.transform.SetParent(parent ?? _defaultParent, false);

            RectTransform scrollRect = scrollViewObj.AddComponent<RectTransform>();
            scrollRect.anchorMin = Vector2.zero;
            scrollRect.anchorMax = Vector2.one;
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;

            ScrollRect scrollView = scrollViewObj.AddComponent<ScrollRect>();
            Image scrollBg = scrollViewObj.AddComponent<Image>();
            scrollBg.color = new Color(0, 0, 0, 0);

            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollViewObj.transform, false);

            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            viewportRect.pivot = new Vector2(0, 1);

            Image viewportImage = viewport.AddComponent<Image>();
            viewportImage.color = Color.white;
            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            // Content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentRect = content.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.offsetMin = Vector2.zero;
            contentRect.offsetMax = Vector2.zero;

            // Layout group
            VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.spacing = 10;
            layout.padding = new RectOffset(10, 10, 10, 10);

            ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // ScrollRect ayarları
            scrollView.content = contentRect;
            scrollView.viewport = viewportRect;
            scrollView.horizontal = false;
            scrollView.vertical = true;
            scrollView.movementType = ScrollRect.MovementType.Elastic;
            scrollView.elasticity = 0.1f;
            scrollView.inertia = true;
            scrollView.decelerationRate = 0.135f;
            scrollView.scrollSensitivity = 1f;

            return scrollViewObj;
        }

        #endregion

        #region Progress Bar Creation

        /// <summary>
        /// Progress bar oluştur.
        /// </summary>
        public GameObject CreateProgressBar(Transform parent, Color fillColor, float initialValue = 0f)
        {
            // Background
            GameObject barObj = new GameObject("ProgressBar");
            barObj.transform.SetParent(parent ?? _defaultParent, false);

            RectTransform rect = barObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 20);

            Image bgImage = barObj.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            // Fill
            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(barObj.transform, false);

            RectTransform fillRect = fill.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(initialValue, 1);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = fillColor;

            return barObj;
        }

        /// <summary>
        /// Progress bar değerini güncelle.
        /// </summary>
        public void UpdateProgressBar(GameObject progressBar, float value)
        {
            if (progressBar == null) return;

            Transform fill = progressBar.transform.Find("Fill");
            if (fill != null)
            {
                RectTransform rect = fill.GetComponent<RectTransform>();
                rect.anchorMax = new Vector2(Mathf.Clamp01(value), 1);
            }
        }

        #endregion

        #region Slider Creation

        /// <summary>
        /// Slider oluştur.
        /// </summary>
        public GameObject CreateSlider(Transform parent, float minValue, float maxValue, float initialValue, Action<float> onValueChanged)
        {
            // Background
            GameObject sliderObj = new GameObject("Slider");
            sliderObj.transform.SetParent(parent ?? _defaultParent, false);

            RectTransform rect = sliderObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 30);

            Slider slider = sliderObj.AddComponent<Slider>();

            // Background
            GameObject background = new GameObject("Background");
            background.transform.SetParent(sliderObj.transform, false);

            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0, 0.25f);
            bgRect.anchorMax = new Vector2(1, 0.75f);
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            // Fill Area
            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObj.transform, false);

            RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0, 0.25f);
            fillAreaRect.anchorMax = new Vector2(1, 0.75f);
            fillAreaRect.offsetMin = new Vector2(5, 0);
            fillAreaRect.offsetMax = new Vector2(-5, 0);

            // Fill
            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);

            RectTransform fillRect = fill.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = UIStyles.PrimaryColor;

            // Handle
            GameObject handleArea = new GameObject("Handle Slide Area");
            handleArea.transform.SetParent(sliderObj.transform, false);

            RectTransform handleAreaRect = handleArea.AddComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.offsetMin = new Vector2(10, 0);
            handleAreaRect.offsetMax = new Vector2(-10, 0);

            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(handleArea.transform, false);

            RectTransform handleRect = handle.AddComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 0);

            Image handleImage = handle.AddComponent<Image>();
            handleImage.color = Color.white;

            // Slider setup
            slider.fillRect = fillRect;
            slider.handleRect = handleRect;
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.value = initialValue;

            if (onValueChanged != null)
            {
                slider.onValueChanged.AddListener((value) => onValueChanged(value));
            }

            return sliderObj;
        }

        #endregion

        #region Input Field Creation

        /// <summary>
        /// Input field oluştur.
        /// </summary>
        public GameObject CreateInputField(Transform parent, string placeholder, Action<string> onValueChanged = null)
        {
            GameObject inputObj = new GameObject("InputField");
            inputObj.transform.SetParent(parent ?? _defaultParent, false);

            RectTransform rect = inputObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 40);

            Image image = inputObj.AddComponent<Image>();
            image.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            InputField inputField = inputObj.AddComponent<InputField>();

            // Text area
            GameObject textArea = new GameObject("Text Area");
            textArea.transform.SetParent(inputObj.transform, false);

            RectTransform textAreaRect = textArea.AddComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.offsetMin = new Vector2(10, 6);
            textAreaRect.offsetMax = new Vector2(-10, -7);

            // Placeholder
            GameObject placeholderObj = new GameObject("Placeholder");
            placeholderObj.transform.SetParent(textArea.transform, false);

            RectTransform placeholderRect = placeholderObj.AddComponent<RectTransform>();
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = Vector2.zero;
            placeholderRect.offsetMax = Vector2.zero;

            Text placeholderText = placeholderObj.AddComponent<Text>();
            placeholderText.text = placeholder;
            placeholderText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            placeholderText.fontSize = 18;
            placeholderText.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            placeholderText.alignment = TextAnchor.MiddleLeft;

            // Text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(textArea.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            Text text = textObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 18;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;
            text.supportRichText = false;

            // Input field setup
            inputField.textComponent = text;
            inputField.placeholder = placeholderText;

            if (onValueChanged != null)
            {
                inputField.onValueChanged.AddListener((value) => onValueChanged(value));
            }

            return inputObj;
        }

        #endregion
    }

    #region Styles

    /// <summary>
    /// UI stil sabitleri.
    /// </summary>
    public static class UIStyles
    {
        // Ana renkler
        public static Color PrimaryColor = new Color(0.2f, 0.6f, 0.9f, 1f);
        public static Color SecondaryColor = new Color(0.4f, 0.4f, 0.45f, 1f);
        public static Color SuccessColor = new Color(0.3f, 0.8f, 0.5f, 1f);
        public static Color DangerColor = new Color(0.9f, 0.4f, 0.3f, 1f);
        public static Color AccentColor = new Color(0.3f, 0.8f, 0.5f, 1f);
        public static Color BackgroundColor = new Color(0.12f, 0.12f, 0.15f, 1f);
        public static Color PanelColor = new Color(0.18f, 0.18f, 0.22f, 1f);
        public static Color TextColor = new Color(0.95f, 0.95f, 0.95f, 1f);
        public static Color SubtextColor = new Color(0.7f, 0.7f, 0.7f, 1f);

        // Stat renkleri
        public static Color HealthColor = new Color(0.9f, 0.3f, 0.3f, 1f);
        public static Color HappinessColor = new Color(1f, 0.8f, 0.2f, 1f);
        public static Color IntelligenceColor = new Color(0.3f, 0.6f, 0.9f, 1f);
        public static Color AppearanceColor = new Color(0.9f, 0.5f, 0.8f, 1f);
        public static Color FameColor = new Color(0.8f, 0.6f, 0.2f, 1f);

        // Panel stilleri
        public static PanelStyle FullScreenPanel = new PanelStyle
        {
            backgroundColor = BackgroundColor,
            raycastTarget = true
        };

        public static PanelStyle PopupOverlay = new PanelStyle
        {
            backgroundColor = new Color(0, 0, 0, 0.7f),
            raycastTarget = true
        };

        public static PanelStyle PopupPanel = new PanelStyle
        {
            backgroundColor = PanelColor,
            raycastTarget = true
        };

        public static PanelStyle CardPanel = new PanelStyle
        {
            backgroundColor = new Color(0.22f, 0.22f, 0.26f, 1f),
            raycastTarget = false
        };

        // Text stilleri
        public static TextStyle TitleText = new TextStyle
        {
            fontSize = 42,
            color = TextColor,
            alignment = TextAnchor.MiddleCenter
        };

        public static TextStyle SubtitleText = new TextStyle
        {
            fontSize = 28,
            color = TextColor,
            alignment = TextAnchor.MiddleCenter
        };

        public static TextStyle BodyText = new TextStyle
        {
            fontSize = 22,
            color = TextColor,
            alignment = TextAnchor.MiddleCenter
        };

        public static TextStyle SmallText = new TextStyle
        {
            fontSize = 18,
            color = SubtextColor,
            alignment = TextAnchor.MiddleLeft
        };

        public static TextStyle StatLabelText = new TextStyle
        {
            fontSize = 20,
            color = TextColor,
            alignment = TextAnchor.MiddleLeft
        };

        // Button stilleri
        public static ButtonStyle PrimaryButton = new ButtonStyle
        {
            width = 300,
            height = 60,
            fontSize = 24,
            normalColor = PrimaryColor,
            highlightedColor = new Color(0.3f, 0.7f, 1f, 1f),
            pressedColor = new Color(0.15f, 0.5f, 0.8f, 1f),
            disabledColor = new Color(0.3f, 0.3f, 0.3f, 1f),
            textColor = Color.white
        };

        public static ButtonStyle SecondaryButton = new ButtonStyle
        {
            width = 200,
            height = 50,
            fontSize = 20,
            normalColor = new Color(0.4f, 0.4f, 0.45f, 1f),
            highlightedColor = new Color(0.5f, 0.5f, 0.55f, 1f),
            pressedColor = new Color(0.3f, 0.3f, 0.35f, 1f),
            disabledColor = new Color(0.25f, 0.25f, 0.25f, 1f),
            textColor = Color.white
        };

        public static ButtonStyle ChoiceButton = new ButtonStyle
        {
            width = 400,
            height = 70,
            fontSize = 20,
            normalColor = new Color(0.25f, 0.25f, 0.3f, 1f),
            highlightedColor = new Color(0.35f, 0.35f, 0.4f, 1f),
            pressedColor = new Color(0.2f, 0.2f, 0.25f, 1f),
            disabledColor = new Color(0.2f, 0.2f, 0.2f, 1f),
            textColor = Color.white
        };

        public static ButtonStyle DangerButton = new ButtonStyle
        {
            width = 200,
            height = 50,
            fontSize = 20,
            normalColor = DangerColor,
            highlightedColor = new Color(1f, 0.5f, 0.4f, 1f),
            pressedColor = new Color(0.7f, 0.3f, 0.25f, 1f),
            disabledColor = new Color(0.4f, 0.3f, 0.3f, 1f),
            textColor = Color.white
        };

        public static ButtonStyle SuccessButton = new ButtonStyle
        {
            width = 200,
            height = 50,
            fontSize = 20,
            normalColor = SuccessColor,
            highlightedColor = new Color(0.4f, 0.9f, 0.6f, 1f),
            pressedColor = new Color(0.2f, 0.7f, 0.4f, 1f),
            disabledColor = new Color(0.3f, 0.4f, 0.35f, 1f),
            textColor = Color.white
        };
    }

    /// <summary>
    /// Panel stili.
    /// </summary>
    public struct PanelStyle
    {
        public Color backgroundColor;
        public bool raycastTarget;
    }

    /// <summary>
    /// Text stili.
    /// </summary>
    public struct TextStyle
    {
        public int fontSize;
        public Color color;
        public TextAnchor alignment;
    }

    /// <summary>
    /// Button stili.
    /// </summary>
    public struct ButtonStyle
    {
        public float width;
        public float height;
        public int fontSize;
        public Color normalColor;
        public Color highlightedColor;
        public Color pressedColor;
        public Color disabledColor;
        public Color textColor;
    }

    #endregion
}
