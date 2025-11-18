using UnityEngine;
using TurkishLifeSim.Managers;
using TurkishLifeSim.Systems;

namespace TurkishLifeSim.Core
{
    /// <summary>
    /// Oyun başlatıcı - Tüm sistemleri initialize eder.
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Bootstrap objesini oluştur
            GameObject bootstrapObject = new GameObject("[GameBootstrap]");
            DontDestroyOnLoad(bootstrapObject);
            bootstrapObject.AddComponent<GameBootstrap>();
        }

        private void Awake()
        {
            Debug.Log("[GameBootstrap] Starting game initialization...");

            // Managerları sırasıyla initialize et
            InitializeManagers();

            Debug.Log("[GameBootstrap] Game initialization complete!");
        }

        private void InitializeManagers()
        {
            // Sıralama önemli - bağımlılıklara göre

            // 1. Data Manager - Veriler önce yüklenmeli
            var dataManager = DataManager.Instance;
            Debug.Log("[GameBootstrap] DataManager initialized.");

            // 2. Audio Manager - Sesler hazır olmalı
            var audioManager = AudioManager.Instance;
            Debug.Log("[GameBootstrap] AudioManager initialized.");

            // 3. Save Manager - Kayıt sistemi hazır olmalı
            var saveManager = SaveManager.Instance;
            Debug.Log("[GameBootstrap] SaveManager initialized.");

            // 4. Event Manager - Olaylar yüklenmeli
            var eventManager = EventManager.Instance;
            Debug.Log("[GameBootstrap] EventManager initialized.");

            // 5. UI Manager - Arayüz hazırlanmalı
            var uiManager = UIManager.Instance;
            Debug.Log("[GameBootstrap] UIManager initialized.");

            // 6. Game Manager - En son, diğerleri hazır olunca
            var gameManager = GameManager.Instance;
            Debug.Log("[GameBootstrap] GameManager initialized.");

            // 7. Game Systems - Tüm oyun sistemleri
            var careerSystem = CareerSystem.Instance;
            Debug.Log("[GameBootstrap] CareerSystem initialized.");

            var educationSystem = EducationSystem.Instance;
            Debug.Log("[GameBootstrap] EducationSystem initialized.");

            var activitiesSystem = ActivitiesSystem.Instance;
            Debug.Log("[GameBootstrap] ActivitiesSystem initialized.");

            var relationshipSystem = RelationshipSystem.Instance;
            Debug.Log("[GameBootstrap] RelationshipSystem initialized.");

            var crimeSystem = CrimeSystem.Instance;
            Debug.Log("[GameBootstrap] CrimeSystem initialized.");

            var assetSystem = AssetSystem.Instance;
            Debug.Log("[GameBootstrap] AssetSystem initialized.");
        }

        private void Start()
        {
            // Oyun başlangıcında ses ayarlarını yükle
            AudioManager.Instance?.LoadSettings();

            // Ana menü müziğini başlat (varsa)
            // AudioManager.Instance?.PlayMusic("MainMenu");
        }
    }
}
