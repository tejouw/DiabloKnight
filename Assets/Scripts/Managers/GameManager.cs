using UnityEngine;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Ana oyun yöneticisi - Oyun durumu ve akışını kontrol eder.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        // Mevcut oyun durumu
        private GameState _currentState = GameState.MainMenu;

        // Aktif karakter verisi
        private CharacterData _currentCharacter;

        // Oyun ayarları
        private GameSettings _settings;

        #region Properties

        /// <summary>
        /// Mevcut oyun durumu.
        /// </summary>
        public GameState CurrentState
        {
            get => _currentState;
            private set
            {
                if (_currentState != value)
                {
                    var oldState = _currentState;
                    _currentState = value;

                    EventBus.Publish(new GameStateChangedEvent
                    {
                        OldState = oldState,
                        NewState = value
                    });
                }
            }
        }

        /// <summary>
        /// Aktif karakter verisi.
        /// </summary>
        public CharacterData CurrentCharacter
        {
            get => _currentCharacter;
            set => _currentCharacter = value;
        }

        /// <summary>
        /// Oyun ayarları.
        /// </summary>
        public GameSettings Settings => _settings;

        /// <summary>
        /// Oyun devam ediyor mu?
        /// </summary>
        public bool IsPlaying => _currentState == GameState.Playing;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();

            // Varsayılan ayarları yükle
            _settings = new GameSettings();

            // Target frame rate ayarla (mobil için)
            Application.targetFrameRate = 60;

            // Uyku modunu engelle
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Debug.Log("[GameManager] Initialized successfully.");
        }

        private void Start()
        {
            // Oyun başladığında ana menüye git
            ChangeState(GameState.MainMenu);
        }

        #endregion

        #region State Management

        /// <summary>
        /// Oyun durumunu değiştir.
        /// </summary>
        public void ChangeState(GameState newState)
        {
            Debug.Log($"[GameManager] State changing from {_currentState} to {newState}");

            // Önceki durumdan çıkış işlemleri
            OnExitState(_currentState);

            // Yeni duruma giriş
            CurrentState = newState;
            OnEnterState(newState);
        }

        private void OnExitState(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    // Otomatik kaydetme
                    if (_currentCharacter != null)
                    {
                        SaveManager.Instance?.AutoSave();
                    }
                    break;
            }
        }

        private void OnEnterState(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    UIManager.Instance?.ShowScreen(ScreenType.MainMenu);
                    break;

                case GameState.Playing:
                    UIManager.Instance?.ShowScreen(ScreenType.Game);
                    break;

                case GameState.GameOver:
                    UIManager.Instance?.ShowScreen(ScreenType.Death);
                    break;

                case GameState.Loading:
                    // Yükleme ekranı göster
                    break;
            }
        }

        #endregion

        #region Game Flow

        /// <summary>
        /// Yeni oyun başlat.
        /// </summary>
        public void StartNewGame()
        {
            Debug.Log("[GameManager] Starting new game...");

            // Yeni karakter oluştur
            _currentCharacter = CharacterFactory.CreateNewCharacter();

            // Oyun durumunu değiştir
            ChangeState(GameState.Playing);

            // İlk olayı tetikle
            EventManager.Instance?.TriggerNextEvent();
        }

        /// <summary>
        /// Kayıtlı oyunu yükle.
        /// </summary>
        public void LoadGame(int slotIndex)
        {
            Debug.Log($"[GameManager] Loading game from slot {slotIndex}...");

            var saveData = SaveManager.Instance?.LoadGame(slotIndex);

            if (saveData != null)
            {
                _currentCharacter = saveData.Character;
                ChangeState(GameState.Playing);

                EventBus.Publish(new GameLoadedEvent { SlotIndex = slotIndex });
            }
            else
            {
                Debug.LogError($"[GameManager] Failed to load game from slot {slotIndex}");
            }
        }

        /// <summary>
        /// Oyunu kaydet.
        /// </summary>
        public void SaveGame(int slotIndex)
        {
            SaveManager.Instance?.SaveGame(slotIndex);
        }

        /// <summary>
        /// Yaş ilerlet.
        /// </summary>
        public void ProgressAge()
        {
            if (_currentCharacter == null) return;

            int oldAge = _currentCharacter.Age;
            var oldStage = _currentCharacter.CurrentLifeStage;

            // Yaşı artır
            _currentCharacter.Age++;

            var newStage = _currentCharacter.CurrentLifeStage;

            // NPC'leri yaşlandır
            AgeNPCs();

            // Maaş öde
            PaySalary();

            // Eğitim ilerlemesi
            ProgressEducation();

            // Yaşa bağlı stat değişimleri
            ApplyAgeEffects();

            // Event yayınla
            EventBus.Publish(new AgeProgressedEvent
            {
                OldAge = oldAge,
                NewAge = _currentCharacter.Age,
                OldStage = oldStage,
                NewStage = newStage
            });

            // Ölüm kontrolü
            if (CheckDeath())
            {
                HandleDeath();
                return;
            }

            // Yeni olay tetikle
            EventManager.Instance?.TriggerNextEvent();
        }

        /// <summary>
        /// Yaşa bağlı stat efektlerini uygula.
        /// </summary>
        private void ApplyAgeEffects()
        {
            if (_currentCharacter == null) return;

            int age = _currentCharacter.Age;
            var stats = _currentCharacter.Stats;

            // Yaşlılıkta sağlık düşüşü
            if (age >= 60)
            {
                int healthLoss = UnityEngine.Random.Range(1, 4);
                stats.ModifyStat(StatType.Health, -healthLoss);
            }

            // Orta yaşta görünüş düşüşü
            if (age >= 40)
            {
                if (UnityEngine.Random.value < 0.3f)
                {
                    stats.ModifyStat(StatType.Appearance, -1);
                }
            }
        }

        /// <summary>
        /// NPC'leri yaşlandır.
        /// </summary>
        private void AgeNPCs()
        {
            if (_currentCharacter == null) return;

            foreach (var rel in _currentCharacter.Relationships)
            {
                // NPC yaşını artır
                rel.age++;

                // Ebeveyn ölüm kontrolü
                if (rel.type == RelationType.Parent && rel.status == RelationshipStatus.Active)
                {
                    if (rel.age >= 70)
                    {
                        float deathChance = (rel.age - 70) * 0.03f;
                        if (UnityEngine.Random.value < deathChance)
                        {
                            rel.status = RelationshipStatus.Deceased;
                            string parentType = rel.gender == Gender.Male ? "Baban" : "Annen";

                            // Üzücü olay
                            _currentCharacter.Stats.ModifyStat(StatType.Happiness, -20);

                            // Miras (basit)
                            decimal inheritance = UnityEngine.Random.Range(5000, 50000);
                            _currentCharacter.Finances.ModifyMoney(inheritance, $"{parentType} vefat etti. Miras.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Maaş öde.
        /// </summary>
        private void PaySalary()
        {
            if (_currentCharacter == null) return;

            var job = _currentCharacter.Career.CurrentJob;
            if (job != null)
            {
                // Yıllık maaş (12 aylık)
                decimal yearlyIncome = job.baseSalary * 12;

                // Performansa göre bonus
                float performanceMultiplier = 0.8f + (_currentCharacter.Career.performanceRating / 100f * 0.4f);
                yearlyIncome *= (decimal)performanceMultiplier;

                _currentCharacter.Finances.ModifyMoney(yearlyIncome, $"{job.title} maaşı");
                _currentCharacter.Career.yearsInJob++;

                // Deneyimle performans artışı
                if (_currentCharacter.Career.performanceRating < 100)
                {
                    _currentCharacter.Career.performanceRating += UnityEngine.Random.Range(1, 3);
                    if (_currentCharacter.Career.performanceRating > 100)
                        _currentCharacter.Career.performanceRating = 100;
                }
            }
        }

        /// <summary>
        /// Eğitim ilerlemesi.
        /// </summary>
        private void ProgressEducation()
        {
            if (_currentCharacter == null) return;

            int age = _currentCharacter.Age;
            var education = _currentCharacter.Education;

            // Otomatik eğitim ilerlemesi
            if (age == 6 && education.currentLevel == EducationLevel.None)
            {
                education.currentLevel = EducationLevel.PrimarySchool;
                education.schoolName = "İlkokul";
            }
            else if (age == 10 && education.currentLevel == EducationLevel.PrimarySchool)
            {
                education.currentLevel = EducationLevel.MiddleSchool;
                education.schoolName = "Ortaokul";
            }
            else if (age == 14 && education.currentLevel == EducationLevel.MiddleSchool)
            {
                education.currentLevel = EducationLevel.HighSchool;
                education.schoolName = "Lise";
            }

            // GPA güncelleme (zekaya bağlı)
            if (age >= 6 && age <= 22 && education.currentLevel != EducationLevel.None)
            {
                float baseGPA = (_currentCharacter.Stats.Intelligence / 100f) * 4f;
                float variance = UnityEngine.Random.Range(-0.5f, 0.5f);
                education.gpa = Mathf.Clamp(baseGPA + variance, 0f, 4f);
            }
        }

        /// <summary>
        /// Ölüm kontrolü.
        /// </summary>
        private bool CheckDeath()
        {
            if (_currentCharacter == null) return false;

            int age = _currentCharacter.Age;
            int health = _currentCharacter.Stats.Health;

            // Sağlık sıfır = ölüm
            if (health <= 0)
            {
                return true;
            }

            // Yaşlılık ölümü - yaş arttıkça olasılık artar
            if (age >= 70)
            {
                float deathChance = (age - 70) * 0.02f + (100 - health) * 0.005f;
                if (UnityEngine.Random.value < deathChance)
                {
                    return true;
                }
            }

            // Maksimum yaş
            if (age >= 120)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ölüm işlemi.
        /// </summary>
        private void HandleDeath()
        {
            string deathCause = DetermineDeathCause();
            string epitaph = GenerateEpitaph();

            EventBus.Publish(new CharacterDiedEvent
            {
                Age = _currentCharacter.Age,
                DeathCause = deathCause,
                Epitaph = epitaph
            });

            ChangeState(GameState.GameOver);
        }

        private string DetermineDeathCause()
        {
            if (_currentCharacter.Stats.Health <= 0)
            {
                return "Sağlık sorunları nedeniyle hayatını kaybetti.";
            }

            if (_currentCharacter.Age >= 90)
            {
                return "Doğal sebeplerden hayatını kaybetti.";
            }

            return "Yaşlılık nedeniyle hayata gözlerini yumdu.";
        }

        private string GenerateEpitaph()
        {
            string name = $"{_currentCharacter.FirstName} {_currentCharacter.LastName}";
            int age = _currentCharacter.Age;

            return $"{name}\n{age} yıllık bir hayatın ardından huzur içinde yatsın.";
        }

        /// <summary>
        /// Oyunu duraklat.
        /// </summary>
        public void PauseGame()
        {
            if (_currentState == GameState.Playing)
            {
                ChangeState(GameState.Paused);
                Time.timeScale = 0f;
            }
        }

        /// <summary>
        /// Oyuna devam et.
        /// </summary>
        public void ResumeGame()
        {
            if (_currentState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
                Time.timeScale = 1f;
            }
        }

        /// <summary>
        /// Ana menüye dön.
        /// </summary>
        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            _currentCharacter = null;
            ChangeState(GameState.MainMenu);
        }

        /// <summary>
        /// Oyundan çık.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameManager] Quitting game...");

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        #endregion
    }

    /// <summary>
    /// Oyun ayarları.
    /// </summary>
    [System.Serializable]
    public class GameSettings
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 0.8f;
        public float SFXVolume = 1f;
        public bool AutoSaveEnabled = true;
        public int AutoSaveIntervalMinutes = 5;
        public bool NotificationsEnabled = true;
        public string Language = "tr";
    }
}
