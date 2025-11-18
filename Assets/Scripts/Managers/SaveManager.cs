using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Kayıt Yöneticisi - Oyun kaydetme ve yükleme işlemlerini yönetir.
    /// </summary>
    public class SaveManager : Singleton<SaveManager>
    {
        private const string SAVE_KEY_PREFIX = "TurkishLifeSim_Save_";
        private const string AUTO_SAVE_KEY = "TurkishLifeSim_AutoSave";
        private const int MAX_SAVE_SLOTS = 5;

        // Auto-save zamanlayıcı
        private float _autoSaveTimer;
        private const float AUTO_SAVE_INTERVAL = 300f; // 5 dakika

        #region Properties

        public int MaxSaveSlots => MAX_SAVE_SLOTS;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[SaveManager] Initialized successfully.");
        }

        private void Update()
        {
            // Auto-save kontrolü
            if (GameManager.Instance != null &&
                GameManager.Instance.IsPlaying &&
                GameManager.Instance.Settings.AutoSaveEnabled)
            {
                _autoSaveTimer += Time.deltaTime;

                if (_autoSaveTimer >= AUTO_SAVE_INTERVAL)
                {
                    AutoSave();
                    _autoSaveTimer = 0f;
                }
            }
        }

        #endregion

        #region Save Operations

        /// <summary>
        /// Oyunu belirtilen slota kaydet.
        /// </summary>
        public void SaveGame(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS)
            {
                Debug.LogError($"[SaveManager] Invalid slot index: {slotIndex}");
                return;
            }

            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null)
            {
                Debug.LogError("[SaveManager] No character to save!");
                return;
            }

            try
            {
                var saveData = CreateSaveData(character);
                string json = JsonUtility.ToJson(saveData, true);
                string key = SAVE_KEY_PREFIX + slotIndex;

                PlayerPrefs.SetString(key, json);
                PlayerPrefs.Save();

                // Event yayınla
                EventBus.Publish(new GameSavedEvent
                {
                    SlotIndex = slotIndex,
                    SaveTime = DateTime.Now
                });

                Debug.Log($"[SaveManager] Game saved to slot {slotIndex}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Error saving game: {e.Message}");
            }
        }

        /// <summary>
        /// Otomatik kaydetme.
        /// </summary>
        public void AutoSave()
        {
            var character = GameManager.Instance?.CurrentCharacter;
            if (character == null) return;

            try
            {
                var saveData = CreateSaveData(character);
                string json = JsonUtility.ToJson(saveData, true);

                PlayerPrefs.SetString(AUTO_SAVE_KEY, json);
                PlayerPrefs.Save();

                Debug.Log("[SaveManager] Auto-save completed.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Error auto-saving: {e.Message}");
            }
        }

        /// <summary>
        /// SaveData oluştur.
        /// </summary>
        private SaveData CreateSaveData(CharacterData character)
        {
            return new SaveData
            {
                saveId = Guid.NewGuid().ToString(),
                saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                characterName = $"{character.FirstName} {character.LastName}",
                characterAge = character.Age,
                Character = character
            };
        }

        #endregion

        #region Load Operations

        /// <summary>
        /// Belirtilen slottan oyunu yükle.
        /// </summary>
        public SaveData LoadGame(int slotIndex)
        {
            string key = slotIndex == -1 ? AUTO_SAVE_KEY : SAVE_KEY_PREFIX + slotIndex;

            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"[SaveManager] No save found in slot {slotIndex}");
                return null;
            }

            try
            {
                string json = PlayerPrefs.GetString(key);
                var saveData = JsonUtility.FromJson<SaveData>(json);

                Debug.Log($"[SaveManager] Game loaded from slot {slotIndex}");
                return saveData;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Error loading game: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Auto-save'i yükle.
        /// </summary>
        public SaveData LoadAutoSave()
        {
            return LoadGame(-1);
        }

        /// <summary>
        /// Belirtilen slotta kayıt var mı kontrol et.
        /// </summary>
        public bool HasSave(int slotIndex)
        {
            string key = SAVE_KEY_PREFIX + slotIndex;
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// Auto-save var mı kontrol et.
        /// </summary>
        public bool HasAutoSave()
        {
            return PlayerPrefs.HasKey(AUTO_SAVE_KEY);
        }

        /// <summary>
        /// Tüm kayıt slotlarının bilgilerini al.
        /// </summary>
        public List<SaveSlotInfo> GetAllSaveSlots()
        {
            var slots = new List<SaveSlotInfo>();

            for (int i = 0; i < MAX_SAVE_SLOTS; i++)
            {
                string key = SAVE_KEY_PREFIX + i;

                if (PlayerPrefs.HasKey(key))
                {
                    try
                    {
                        string json = PlayerPrefs.GetString(key);
                        var saveData = JsonUtility.FromJson<SaveData>(json);

                        slots.Add(new SaveSlotInfo
                        {
                            slotIndex = i,
                            characterName = saveData.characterName,
                            characterAge = saveData.characterAge,
                            saveDate = saveData.saveDate,
                            isEmpty = false
                        });
                    }
                    catch
                    {
                        slots.Add(new SaveSlotInfo
                        {
                            slotIndex = i,
                            isEmpty = true
                        });
                    }
                }
                else
                {
                    slots.Add(new SaveSlotInfo
                    {
                        slotIndex = i,
                        isEmpty = true
                    });
                }
            }

            return slots;
        }

        #endregion

        #region Delete Operations

        /// <summary>
        /// Belirtilen slottaki kaydı sil.
        /// </summary>
        public void DeleteSave(int slotIndex)
        {
            string key = SAVE_KEY_PREFIX + slotIndex;

            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
                Debug.Log($"[SaveManager] Save deleted from slot {slotIndex}");
            }
        }

        /// <summary>
        /// Auto-save'i sil.
        /// </summary>
        public void DeleteAutoSave()
        {
            if (PlayerPrefs.HasKey(AUTO_SAVE_KEY))
            {
                PlayerPrefs.DeleteKey(AUTO_SAVE_KEY);
                PlayerPrefs.Save();
                Debug.Log("[SaveManager] Auto-save deleted");
            }
        }

        /// <summary>
        /// Tüm kayıtları sil.
        /// </summary>
        public void DeleteAllSaves()
        {
            for (int i = 0; i < MAX_SAVE_SLOTS; i++)
            {
                DeleteSave(i);
            }
            DeleteAutoSave();
            Debug.Log("[SaveManager] All saves deleted");
        }

        #endregion
    }

    /// <summary>
    /// Kayıt verisi.
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public string saveId;
        public string saveDate;
        public string characterName;
        public int characterAge;
        public CharacterData Character;
    }

    /// <summary>
    /// Kayıt slotu bilgisi.
    /// </summary>
    [System.Serializable]
    public class SaveSlotInfo
    {
        public int slotIndex;
        public string characterName;
        public int characterAge;
        public string saveDate;
        public bool isEmpty;
    }
}
