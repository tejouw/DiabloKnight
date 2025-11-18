using UnityEngine;

namespace TurkishLifeSim.Core
{
    /// <summary>
    /// Generic Singleton base class for MonoBehaviour managers.
    /// Tüm manager sınıfları için kullanılacak temel singleton implementasyonu.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// Singleton instance'a erişim sağlar.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_applicationIsQuitting)
                    {
                        Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                        return null;
                    }

                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError($"[Singleton] Something went really wrong - there should never be more than 1 singleton of type {typeof(T)}! Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"[{typeof(T)}]";

                            DontDestroyOnLoad(singletonObject);

                            Debug.Log($"[Singleton] An instance of {typeof(T)} was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                        }
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Instance var mı kontrol eder (oluşturmadan).
        /// </summary>
        public static bool HasInstance => _instance != null;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
                OnSingletonAwake();
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} found. Destroying the new one.");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Singleton Awake'te çağrılır. Override edilebilir.
        /// </summary>
        protected virtual void OnSingletonAwake()
        {
        }

        protected virtual void OnDestroy()
        {
            lock (_lock)
            {
                if (_instance == this)
                {
                    _instance = null;
                }
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
}
