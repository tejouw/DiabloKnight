using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Ses Yöneticisi - Müzik ve ses efektlerini yönetir.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        // Audio kaynakları
        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        // Ses ayarları
        private float _masterVolume = 1f;
        private float _musicVolume = 0.8f;
        private float _sfxVolume = 1f;

        // Ses klipleri cache'i
        private Dictionary<string, AudioClip> _clipCache = new Dictionary<string, AudioClip>();

        #region Properties

        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                _masterVolume = Mathf.Clamp01(value);
                UpdateVolumes();
            }
        }

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = Mathf.Clamp01(value);
                UpdateVolumes();
            }
        }

        public float SFXVolume
        {
            get => _sfxVolume;
            set
            {
                _sfxVolume = Mathf.Clamp01(value);
                UpdateVolumes();
            }
        }

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeAudioSources();
            LoadSettings(); // Kayıtlı ses ayarlarını yükle
        }

        private void InitializeAudioSources()
        {
            // Müzik kaynağı
            GameObject musicObject = new GameObject("MusicSource");
            musicObject.transform.SetParent(transform);
            _musicSource = musicObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
            _musicSource.volume = _musicVolume * _masterVolume;

            // SFX kaynağı
            GameObject sfxObject = new GameObject("SFXSource");
            sfxObject.transform.SetParent(transform);
            _sfxSource = sfxObject.AddComponent<AudioSource>();
            _sfxSource.loop = false;
            _sfxSource.playOnAwake = false;
            _sfxSource.volume = _sfxVolume * _masterVolume;

            Debug.Log("[AudioManager] Initialized successfully.");
        }

        #endregion

        #region Music Control

        /// <summary>
        /// Müzik çal.
        /// </summary>
        public void PlayMusic(string clipName)
        {
            AudioClip clip = GetClip(clipName);
            if (clip == null)
            {
                Debug.LogWarning($"[AudioManager] Music clip not found: {clipName}");
                return;
            }

            if (_musicSource.clip == clip && _musicSource.isPlaying)
            {
                return; // Aynı müzik zaten çalıyor
            }

            _musicSource.clip = clip;
            _musicSource.Play();
        }

        /// <summary>
        /// Müziği durdur.
        /// </summary>
        public void StopMusic()
        {
            _musicSource.Stop();
        }

        /// <summary>
        /// Müziği duraklat.
        /// </summary>
        public void PauseMusic()
        {
            _musicSource.Pause();
        }

        /// <summary>
        /// Müziğe devam et.
        /// </summary>
        public void ResumeMusic()
        {
            _musicSource.UnPause();
        }

        /// <summary>
        /// Fade ile müzik geçişi.
        /// </summary>
        public void CrossfadeMusic(string clipName, float duration = 1f)
        {
            StartCoroutine(CrossfadeMusicCoroutine(clipName, duration));
        }

        private System.Collections.IEnumerator CrossfadeMusicCoroutine(string clipName, float duration)
        {
            float startVolume = _musicSource.volume;

            // Fade out
            float elapsed = 0f;
            while (elapsed < duration / 2)
            {
                elapsed += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0, elapsed / (duration / 2));
                yield return null;
            }

            // Clip değiştir
            AudioClip newClip = GetClip(clipName);
            if (newClip != null)
            {
                _musicSource.clip = newClip;
                _musicSource.Play();
            }

            // Fade in
            elapsed = 0f;
            while (elapsed < duration / 2)
            {
                elapsed += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0, startVolume, elapsed / (duration / 2));
                yield return null;
            }

            _musicSource.volume = startVolume;
        }

        #endregion

        #region SFX Control

        /// <summary>
        /// Ses efekti çal.
        /// </summary>
        public void PlaySFX(string clipName)
        {
            AudioClip clip = GetClip(clipName);
            if (clip == null)
            {
                Debug.LogWarning($"[AudioManager] SFX clip not found: {clipName}");
                return;
            }

            _sfxSource.PlayOneShot(clip, _sfxVolume * _masterVolume);
        }

        /// <summary>
        /// Ses efekti çal (pitch ayarı ile).
        /// </summary>
        public void PlaySFX(string clipName, float pitch)
        {
            AudioClip clip = GetClip(clipName);
            if (clip == null)
            {
                Debug.LogWarning($"[AudioManager] SFX clip not found: {clipName}");
                return;
            }

            float originalPitch = _sfxSource.pitch;
            _sfxSource.pitch = pitch;
            _sfxSource.PlayOneShot(clip, _sfxVolume * _masterVolume);
            _sfxSource.pitch = originalPitch;
        }

        /// <summary>
        /// UI tıklama sesi.
        /// </summary>
        public void PlayButtonClick()
        {
            PlaySFX("UI_Click");
        }

        /// <summary>
        /// Pozitif sonuç sesi.
        /// </summary>
        public void PlayPositiveSound()
        {
            PlaySFX("Positive");
        }

        /// <summary>
        /// Negatif sonuç sesi.
        /// </summary>
        public void PlayNegativeSound()
        {
            PlaySFX("Negative");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Ses klibini yükle veya cache'ten al.
        /// </summary>
        private AudioClip GetClip(string clipName)
        {
            if (_clipCache.ContainsKey(clipName))
            {
                return _clipCache[clipName];
            }

            // Resources'tan yükle
            AudioClip clip = Resources.Load<AudioClip>($"Audio/{clipName}");

            if (clip != null)
            {
                _clipCache[clipName] = clip;
            }

            return clip;
        }

        /// <summary>
        /// Ses seviyelerini güncelle.
        /// </summary>
        private void UpdateVolumes()
        {
            if (_musicSource != null)
            {
                _musicSource.volume = _musicVolume * _masterVolume;
            }

            if (_sfxSource != null)
            {
                _sfxSource.volume = _sfxVolume * _masterVolume;
            }
        }

        /// <summary>
        /// Tüm sesleri sustur.
        /// </summary>
        public void MuteAll()
        {
            _masterVolume = 0f;
            UpdateVolumes();
        }

        /// <summary>
        /// Susturmayı kaldır.
        /// </summary>
        public void UnmuteAll()
        {
            _masterVolume = 1f;
            UpdateVolumes();
        }

        /// <summary>
        /// Ses ayarlarını kaydet.
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Ses ayarlarını yükle.
        /// </summary>
        public void LoadSettings()
        {
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            UpdateVolumes();
        }

        #endregion
    }
}
