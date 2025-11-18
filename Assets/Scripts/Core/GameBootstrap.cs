using UnityEngine;
using TurkishLifeSim.Managers;

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

            // 5. Career Manager - Kariyer sistemi
            var careerManager = CareerManager.Instance;
            Debug.Log("[GameBootstrap] CareerManager initialized.");

            // 6. Education Manager - Eğitim sistemi
            var educationManager = EducationManager.Instance;
            Debug.Log("[GameBootstrap] EducationManager initialized.");

            // 7. Relationship Manager - İlişki sistemi
            var relationshipManager = RelationshipManager.Instance;
            Debug.Log("[GameBootstrap] RelationshipManager initialized.");

            // 8. UI Manager - Arayüz hazırlanmalı
            var uiManager = UIManager.Instance;
            Debug.Log("[GameBootstrap] UIManager initialized.");

            // 9. Game Manager - En son, diğerleri hazır olunca
            var gameManager = GameManager.Instance;
            Debug.Log("[GameBootstrap] GameManager initialized.");
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
