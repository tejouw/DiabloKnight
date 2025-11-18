using UnityEngine;
using UnityEngine.UI;
using TurkishLifeSim.Core;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.UI
{
    /// <summary>
    /// Karakter oluşturma ekranı kontrolcüsü - Yeni karakter özelleştirme.
    /// </summary>
    public class CharacterCreationScreenController : MonoBehaviour
    {
        private UIFactory _factory;

        // Karakter verileri
        private string _firstName = "";
        private string _lastName = "";
        private Gender _selectedGender = Gender.Male;
        private string _selectedCity = "İstanbul";

        // UI referansları
        private GameObject _maleButton;
        private GameObject _femaleButton;
        private Text _cityText;

        // Şehir listesi
        private string[] _cities = new string[]
        {
            "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya",
            "Adana", "Konya", "Gaziantep", "Şanlıurfa", "Kocaeli",
            "Mersin", "Diyarbakır", "Hatay", "Manisa", "Kayseri",
            "Samsun", "Balıkesir", "Kahramanmaraş", "Van", "Aydın"
        };
        private int _currentCityIndex = 0;

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
            scrollRect.anchorMin = new Vector2(0, 0.12f);
            scrollRect.anchorMax = new Vector2(1, 0.9f);
            scrollRect.offsetMin = new Vector2(10, 10);
            scrollRect.offsetMax = new Vector2(-10, -10);

            var content = scrollView.transform.Find("Viewport/Content");

            // Açıklama
            var descText = _factory.CreateText(content, "Yeni hayatınıza başlamadan önce karakterinizi oluşturun.", UIStyles.BodyText);
            var descLayout = descText.AddComponent<LayoutElement>();
            descLayout.minHeight = 60;
            descLayout.preferredHeight = 60;
            descText.GetComponent<Text>().color = UIStyles.SubtextColor;

            // İsim Bölümü
            AddSectionTitle(content, "İsim");

            // Ad input
            CreateInputSection(content, "Ad", "Adınızı girin...", (value) =>
            {
                _firstName = value;
            });

            // Soyad input
            CreateInputSection(content, "Soyad", "Soyadınızı girin...", (value) =>
            {
                _lastName = value;
            });

            // Cinsiyet Bölümü
            AddSectionTitle(content, "Cinsiyet");
            CreateGenderSelection(content);

            // Doğum Şehri Bölümü
            AddSectionTitle(content, "Doğum Şehri");
            CreateCitySelection(content);

            // Boşluk
            AddSpacer(content, 30);

            // Rastgele İsim Butonu
            var randomNameButton = _factory.CreateButton(content, "Rastgele İsim Oluştur", () =>
            {
                GenerateRandomName();
            }, UIStyles.SecondaryButton);
            var randomLayout = randomNameButton.AddComponent<LayoutElement>();
            randomLayout.minHeight = 60;
            randomLayout.preferredHeight = 60;

            // Boşluk
            AddSpacer(content, 50);

            // Alt butonlar
            // Oyunu Başlat butonu
            var startButton = _factory.CreateButton(transform, "Hayata Başla", () =>
            {
                StartGame();
            }, UIStyles.PrimaryButton);
            var startRect = startButton.GetComponent<RectTransform>();
            startRect.anchorMin = new Vector2(0.1f, 0.02f);
            startRect.anchorMax = new Vector2(0.9f, 0.1f);
            startRect.offsetMin = Vector2.zero;
            startRect.offsetMax = Vector2.zero;
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

        private void CreateInputSection(Transform parent, string label, string placeholder, System.Action<string> onValueChanged)
        {
            // Container
            var container = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var containerLayout = container.AddComponent<LayoutElement>();
            containerLayout.minHeight = 90;
            containerLayout.preferredHeight = 90;

            // Label
            var labelObj = _factory.CreateText(container.transform, label, UIStyles.SmallText);
            var labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.05f, 0.6f);
            labelRect.anchorMax = new Vector2(0.95f, 0.95f);
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            labelObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

            // Input field
            var inputField = _factory.CreateInputField(container.transform, placeholder, onValueChanged);
            var inputRect = inputField.GetComponent<RectTransform>();
            inputRect.anchorMin = new Vector2(0.05f, 0.1f);
            inputRect.anchorMax = new Vector2(0.95f, 0.55f);
            inputRect.offsetMin = Vector2.zero;
            inputRect.offsetMax = Vector2.zero;
        }

        private void CreateGenderSelection(Transform parent)
        {
            // Container
            var container = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var containerLayout = container.AddComponent<LayoutElement>();
            containerLayout.minHeight = 80;
            containerLayout.preferredHeight = 80;

            // Erkek butonu
            _maleButton = _factory.CreateButton(container.transform, "Erkek", () =>
            {
                SelectGender(Gender.Male);
            }, UIStyles.PrimaryButton);
            var maleRect = _maleButton.GetComponent<RectTransform>();
            maleRect.anchorMin = new Vector2(0.05f, 0.15f);
            maleRect.anchorMax = new Vector2(0.48f, 0.85f);
            maleRect.offsetMin = Vector2.zero;
            maleRect.offsetMax = Vector2.zero;

            // Kadın butonu
            _femaleButton = _factory.CreateButton(container.transform, "Kadın", () =>
            {
                SelectGender(Gender.Female);
            }, UIStyles.SecondaryButton);
            var femaleRect = _femaleButton.GetComponent<RectTransform>();
            femaleRect.anchorMin = new Vector2(0.52f, 0.15f);
            femaleRect.anchorMax = new Vector2(0.95f, 0.85f);
            femaleRect.offsetMin = Vector2.zero;
            femaleRect.offsetMax = Vector2.zero;
        }

        private void SelectGender(Gender gender)
        {
            _selectedGender = gender;

            // Buton renklerini güncelle
            var maleImage = _maleButton.GetComponent<Image>();
            var femaleImage = _femaleButton.GetComponent<Image>();

            if (gender == Gender.Male)
            {
                maleImage.color = UIStyles.PrimaryColor;
                femaleImage.color = new Color(0.4f, 0.4f, 0.45f, 1f);
            }
            else
            {
                maleImage.color = new Color(0.4f, 0.4f, 0.45f, 1f);
                femaleImage.color = UIStyles.PrimaryColor;
            }
        }

        private void CreateCitySelection(Transform parent)
        {
            // Container
            var container = _factory.CreatePanel(parent, UIStyles.CardPanel);
            var containerLayout = container.AddComponent<LayoutElement>();
            containerLayout.minHeight = 80;
            containerLayout.preferredHeight = 80;

            // Önceki butonu
            var prevButton = _factory.CreateButton(container.transform, "<", () =>
            {
                _currentCityIndex = (_currentCityIndex - 1 + _cities.Length) % _cities.Length;
                _selectedCity = _cities[_currentCityIndex];
                UpdateCityText();
            }, UIStyles.SecondaryButton);
            var prevRect = prevButton.GetComponent<RectTransform>();
            prevRect.anchorMin = new Vector2(0.05f, 0.2f);
            prevRect.anchorMax = new Vector2(0.2f, 0.8f);
            prevRect.offsetMin = Vector2.zero;
            prevRect.offsetMax = Vector2.zero;

            // Şehir adı
            var cityObj = _factory.CreateText(container.transform, _selectedCity, UIStyles.BodyText);
            var cityRect = cityObj.GetComponent<RectTransform>();
            cityRect.anchorMin = new Vector2(0.22f, 0.2f);
            cityRect.anchorMax = new Vector2(0.78f, 0.8f);
            cityRect.offsetMin = Vector2.zero;
            cityRect.offsetMax = Vector2.zero;
            _cityText = cityObj.GetComponent<Text>();
            _cityText.alignment = TextAnchor.MiddleCenter;

            // Sonraki butonu
            var nextButton = _factory.CreateButton(container.transform, ">", () =>
            {
                _currentCityIndex = (_currentCityIndex + 1) % _cities.Length;
                _selectedCity = _cities[_currentCityIndex];
                UpdateCityText();
            }, UIStyles.SecondaryButton);
            var nextRect = nextButton.GetComponent<RectTransform>();
            nextRect.anchorMin = new Vector2(0.8f, 0.2f);
            nextRect.anchorMax = new Vector2(0.95f, 0.8f);
            nextRect.offsetMin = Vector2.zero;
            nextRect.offsetMax = Vector2.zero;
        }

        private void UpdateCityText()
        {
            if (_cityText != null)
            {
                _cityText.text = _selectedCity;
            }
        }

        private void GenerateRandomName()
        {
            // Türk isimleri
            string[] maleNames = { "Ahmet", "Mehmet", "Mustafa", "Ali", "Hüseyin", "Hasan", "İbrahim", "Osman", "Yusuf", "Emre", "Burak", "Murat", "Serkan", "Özgür", "Cem" };
            string[] femaleNames = { "Fatma", "Ayşe", "Emine", "Hatice", "Zeynep", "Elif", "Merve", "Esra", "Derya", "Selin", "Cansu", "Büşra", "Özlem", "Aslı", "Ebru" };
            string[] lastNames = { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Yıldırım", "Öztürk", "Aydın", "Özdemir", "Arslan", "Doğan", "Kılıç", "Aslan", "Çetin" };

            if (_selectedGender == Gender.Male)
            {
                _firstName = maleNames[Random.Range(0, maleNames.Length)];
            }
            else
            {
                _firstName = femaleNames[Random.Range(0, femaleNames.Length)];
            }

            _lastName = lastNames[Random.Range(0, lastNames.Length)];

            // Input field'ları güncelle - bunun için sayfayı yeniden oluşturacağız
            UIManager.Instance.ShowInfo("İsim Oluşturuldu", $"{_firstName} {_lastName}");
        }

        private void StartGame()
        {
            // İsim kontrolü
            if (string.IsNullOrEmpty(_firstName) || string.IsNullOrEmpty(_lastName))
            {
                // Rastgele isim kullan
                GenerateRandomName();
            }

            // Karakter oluştur
            var character = CharacterFactory.CreateNewCharacter();
            character.FirstName = _firstName;
            character.LastName = _lastName;
            character.Gender = _selectedGender;
            character.BirthCity = _selectedCity;

            // GameManager'a ata
            GameManager.Instance.CurrentCharacter = character;

            // Oyunu başlat
            GameManager.Instance.ChangeState(GameState.Playing);

            // İlk olayı tetikle
            EventManager.Instance?.TriggerNextEvent();
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
    }
}
