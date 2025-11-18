using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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

            // Mirası dağıt
            var inheritanceShares = DistributeInheritance();

            EventBus.Publish(new CharacterDiedEvent
            {
                Age = _currentCharacter.Age,
                DeathCause = deathCause,
                Epitaph = epitaph
            });

            // Çocuk olarak devam edilebilir mi kontrol et
            var availableChildren = GetAvailableChildren();
            if (availableChildren.Count > 0)
            {
                // Çocuğun miras payını hesapla
                decimal totalInheritance = 0;
                if (inheritanceShares != null && inheritanceShares.Count > 0)
                {
                    var childShare = inheritanceShares.FirstOrDefault(s =>
                        availableChildren.Any(c => c.npcId == s.npcId));
                    if (childShare != null)
                    {
                        totalInheritance = childShare.amount;
                    }
                }

                EventBus.Publish(new ContinueAsChildAvailableEvent
                {
                    AvailableChildren = availableChildren,
                    TotalInheritance = totalInheritance
                });
            }

            ChangeState(GameState.GameOver);
        }

        /// <summary>
        /// Mirası mirasçılara dağıt.
        /// </summary>
        private List<InheritanceShare> DistributeInheritance()
        {
            if (_currentCharacter == null) return null;

            var shares = new List<InheritanceShare>();
            decimal totalEstate = _currentCharacter.finances.currentMoney;

            // Mirasçıları bul
            var children = _currentCharacter.relationships
                .Where(r => r.type == RelationType.Child && r.status == RelationshipStatus.Active)
                .ToList();
            var spouse = _currentCharacter.relationships
                .FirstOrDefault(r => r.type == RelationType.Spouse && r.status == RelationshipStatus.Active);

            if (children.Count == 0 && spouse == null)
            {
                // Mirasçı yok - devlete kalır
                Debug.Log($"[GameManager] No heirs found. Estate of {totalEstate:N0} TL goes to state.");
                return shares;
            }

            // Vasiyet yoksa varsayılan oluştur
            if (!_currentCharacter.will.hasWrittenWill || _currentCharacter.will.beneficiaries.Count == 0)
            {
                _currentCharacter.will.CreateDefaultWill(children, spouse);
            }

            // Toplam vergi
            decimal taxRate = _currentCharacter.will.inheritanceTaxRate;
            decimal totalTax = totalEstate * taxRate;
            decimal distributableEstate = totalEstate - totalTax;

            // Hayır kurumuna bırakılan
            decimal charityAmount = distributableEstate * (_currentCharacter.will.charityPercentage / 100m);
            distributableEstate -= charityAmount;

            // Her mirasçıya payını dağıt
            foreach (var beneficiary in _currentCharacter.will.beneficiaries)
            {
                decimal shareAmount = distributableEstate * (decimal)(beneficiary.percentage / 100f);
                decimal individualTax = shareAmount * taxRate;

                var share = new InheritanceShare
                {
                    npcId = beneficiary.npcId,
                    npcName = beneficiary.npcName,
                    amount = shareAmount,
                    taxPaid = individualTax,
                    assetsReceived = new List<string>(beneficiary.specificAssets)
                };
                shares.Add(share);

                Debug.Log($"[GameManager] {beneficiary.npcName} inherits {shareAmount:N0} TL " +
                          $"(Tax: {individualTax:N0} TL)");
            }

            // Miras dağıtım event'i yayınla
            EventBus.Publish(new InheritanceDistributedEvent
            {
                DeceasedName = _currentCharacter.FullName,
                TotalEstate = totalEstate,
                TaxPaid = totalTax,
                BeneficiaryCount = shares.Count,
                Shares = shares
            });

            return shares;
        }

        /// <summary>
        /// Devam edilebilecek çocukları getir.
        /// </summary>
        public List<Relationship> GetAvailableChildren()
        {
            if (_currentCharacter == null) return new List<Relationship>();

            return _currentCharacter.relationships
                .Where(r => r.type == RelationType.Child &&
                           r.status == RelationshipStatus.Active &&
                           r.age >= 0) // Yaşayan çocuklar
                .OrderByDescending(r => r.age) // En büyük çocuktan başla
                .ToList();
        }

        /// <summary>
        /// Seçilen çocuk olarak oyuna devam et.
        /// </summary>
        public void ContinueAsChild(string childNpcId)
        {
            if (_currentCharacter == null)
            {
                Debug.LogError("[GameManager] No current character to continue from.");
                return;
            }

            // Çocuğu bul
            var child = _currentCharacter.relationships
                .FirstOrDefault(r => r.npcId == childNpcId && r.type == RelationType.Child);

            if (child == null)
            {
                Debug.LogError($"[GameManager] Child with ID {childNpcId} not found.");
                return;
            }

            // Çocuğun miras payını hesapla
            decimal inheritedMoney = CalculateChildInheritance(child);

            // Önceki nesil bilgilerini sakla
            int oldGeneration = _currentCharacter.legacy.generation;
            string previousCharacterName = _currentCharacter.FullName;

            // Yeni karakter oluştur
            var newCharacter = CharacterFactory.CreateCharacterFromChild(
                _currentCharacter,
                child,
                inheritedMoney);

            // Aktif karakteri değiştir
            _currentCharacter = newCharacter;

            // Nesil değişim event'i yayınla
            EventBus.Publish(new GenerationChangedEvent
            {
                OldGeneration = oldGeneration,
                NewGeneration = newCharacter.legacy.generation,
                PreviousCharacterName = previousCharacterName,
                NewCharacterName = newCharacter.FullName,
                InheritedWealth = inheritedMoney,
                FamilyName = newCharacter.lastName
            });

            // Oyun durumunu tekrar Playing'e çevir
            ChangeState(GameState.Playing);

            // Yeni olay tetikle
            EventManager.Instance?.TriggerNextEvent();

            Debug.Log($"[GameManager] Continued as {newCharacter.FullName}, " +
                      $"Generation {newCharacter.legacy.generation}, " +
                      $"Age {newCharacter.age}, " +
                      $"Inherited {inheritedMoney:N0} TL");
        }

        /// <summary>
        /// Çocuğun miras payını hesapla.
        /// </summary>
        private decimal CalculateChildInheritance(Relationship child)
        {
            if (_currentCharacter == null) return 0;

            decimal totalEstate = _currentCharacter.finances.currentMoney;

            // Vasiyet kontrolü
            if (_currentCharacter.will.hasWrittenWill && _currentCharacter.will.beneficiaries.Count > 0)
            {
                var beneficiary = _currentCharacter.will.beneficiaries
                    .FirstOrDefault(b => b.npcId == child.npcId);

                if (beneficiary != null)
                {
                    decimal taxRate = _currentCharacter.will.inheritanceTaxRate;
                    decimal afterTax = totalEstate * (1 - taxRate);
                    decimal charityDeduction = afterTax * (_currentCharacter.will.charityPercentage / 100m);
                    decimal distributable = afterTax - charityDeduction;

                    return distributable * (decimal)(beneficiary.percentage / 100f);
                }
            }

            // Varsayılan eşit dağılım
            var children = _currentCharacter.relationships
                .Where(r => r.type == RelationType.Child && r.status == RelationshipStatus.Active)
                .ToList();
            var spouse = _currentCharacter.relationships
                .FirstOrDefault(r => r.type == RelationType.Spouse && r.status == RelationshipStatus.Active);

            int totalBeneficiaries = children.Count;
            if (spouse != null) totalBeneficiaries++;

            if (totalBeneficiaries == 0) return 0;

            decimal taxRate2 = _currentCharacter.will.inheritanceTaxRate;
            decimal afterTax2 = totalEstate * (1 - taxRate2);

            return afterTax2 / totalBeneficiaries;
        }

        /// <summary>
        /// Vasiyet yaz veya güncelle.
        /// </summary>
        public void WriteWill(List<WillBeneficiary> beneficiaries, decimal charityPercentage = 0)
        {
            if (_currentCharacter == null) return;

            _currentCharacter.will.beneficiaries = beneficiaries;
            _currentCharacter.will.charityPercentage = charityPercentage;
            _currentCharacter.will.hasWrittenWill = true;

            Debug.Log($"[GameManager] Will updated with {beneficiaries.Count} beneficiaries, " +
                      $"{charityPercentage}% to charity");
        }

        /// <summary>
        /// Mevcut karakterin nesil bilgilerini al.
        /// </summary>
        public LegacyData GetLegacyData()
        {
            return _currentCharacter?.legacy;
        }

        /// <summary>
        /// Aile tarihini al.
        /// </summary>
        public List<LegacyRecord> GetFamilyHistory()
        {
            return _currentCharacter?.legacy?.familyHistory ?? new List<LegacyRecord>();
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
