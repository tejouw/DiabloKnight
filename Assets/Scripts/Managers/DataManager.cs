using UnityEngine;
using System;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Data;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Veri Yöneticisi - JSON verilerini yükler ve yönetir.
    /// </summary>
    public class DataManager : Singleton<DataManager>
    {
        // İsim verileri
        private NameData _nameData;

        // Şehir verileri
        private CityData _cityData;

        // Meslek verileri
        private JobDatabase _jobDatabase;

        // Üniversite verileri
        private UniversityDatabase _universityDatabase;

        #region Properties

        public NameData Names => _nameData;
        public CityData Cities => _cityData;
        public JobDatabase Jobs => _jobDatabase;
        public UniversityDatabase Universities => _universityDatabase;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            LoadAllData();
        }

        /// <summary>
        /// Tüm verileri yükle.
        /// </summary>
        private void LoadAllData()
        {
            bool allLoaded = true;

            LoadNameData();
            if (_nameData == null || _nameData.maleNames == null)
            {
                Debug.LogError("[DataManager] Failed to load name data!");
                allLoaded = false;
            }

            LoadCityData();
            if (_cityData == null || _cityData.cities == null)
            {
                Debug.LogError("[DataManager] Failed to load city data!");
                allLoaded = false;
            }

            LoadJobData();
            if (_jobDatabase == null || _jobDatabase.jobs == null)
            {
                Debug.LogError("[DataManager] Failed to load job data!");
                allLoaded = false;
            }

            LoadUniversityData();
            if (_universityDatabase == null || _universityDatabase.universities == null)
            {
                Debug.LogError("[DataManager] Failed to load university data!");
                allLoaded = false;
            }

            if (allLoaded)
            {
                Debug.Log("[DataManager] All data loaded successfully.");
            }
            else
            {
                Debug.LogWarning("[DataManager] Some data failed to load. Using defaults where possible.");
            }
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// İsim verilerini yükle.
        /// </summary>
        private void LoadNameData()
        {
            var textAsset = Resources.Load<TextAsset>("Names/TurkishNames");

            if (textAsset != null)
            {
                _nameData = JsonUtility.FromJson<NameData>(textAsset.text);
            }
            else
            {
                // Varsayılan isimler
                _nameData = GetDefaultNameData();
            }

            Debug.Log($"[DataManager] Loaded {_nameData.maleNames.Length} male names, {_nameData.femaleNames.Length} female names, {_nameData.surnames.Length} surnames.");
        }

        /// <summary>
        /// Şehir verilerini yükle.
        /// </summary>
        private void LoadCityData()
        {
            var textAsset = Resources.Load<TextAsset>("Localization/Cities");

            if (textAsset != null)
            {
                _cityData = JsonUtility.FromJson<CityData>(textAsset.text);
            }
            else
            {
                // Varsayılan şehirler
                _cityData = GetDefaultCityData();
            }

            Debug.Log($"[DataManager] Loaded {_cityData.cities.Length} cities.");
        }

        /// <summary>
        /// Meslek verilerini yükle.
        /// </summary>
        private void LoadJobData()
        {
            var textAsset = Resources.Load<TextAsset>("Jobs/JobDatabase");

            if (textAsset != null)
            {
                _jobDatabase = JsonUtility.FromJson<JobDatabase>(textAsset.text);
            }
            else
            {
                // Varsayılan meslekler
                _jobDatabase = GetDefaultJobData();
            }

            Debug.Log($"[DataManager] Loaded {_jobDatabase.jobs.Length} jobs.");
        }

        /// <summary>
        /// Üniversite verilerini yükle.
        /// </summary>
        private void LoadUniversityData()
        {
            var textAsset = Resources.Load<TextAsset>("Jobs/Universities");

            if (textAsset != null)
            {
                _universityDatabase = JsonUtility.FromJson<UniversityDatabase>(textAsset.text);
            }
            else
            {
                // Varsayılan üniversiteler
                _universityDatabase = GetDefaultUniversityData();
            }

            Debug.Log($"[DataManager] Loaded {_universityDatabase.universities.Length} universities.");
        }

        #endregion

        #region Default Data

        private NameData GetDefaultNameData()
        {
            return new NameData
            {
                maleNames = new string[]
                {
                    "Ahmet", "Mehmet", "Mustafa", "Ali", "Hüseyin", "Hasan", "İbrahim", "İsmail",
                    "Yusuf", "Osman", "Murat", "Ömer", "Halil", "Süleyman", "Abdullah", "Mahmut",
                    "Recep", "Salih", "Ramazan", "Dursun", "Şaban", "Bayram", "Kemal", "Cemal",
                    "Adem", "Yaşar", "Bekir", "Kadir", "Fatih", "Emre", "Burak", "Enes",
                    "Muhammed", "Emir", "Yiğit", "Arda", "Berat", "Kerem", "Eyüp", "Alperen"
                },
                femaleNames = new string[]
                {
                    "Fatma", "Ayşe", "Emine", "Hatice", "Zeynep", "Elif", "Meryem", "Şerife",
                    "Zehra", "Sultan", "Hanife", "Merve", "Büşra", "Esra", "Nur", "Gamze",
                    "Gülsüm", "Havva", "Hacer", "Fadime", "Yasemin", "Özlem", "Sibel", "Derya",
                    "Seda", "Melek", "Ebru", "Pınar", "Asya", "Defne", "Ecrin", "Azra",
                    "Nehir", "Ela", "Lina", "Mira", "Duru", "Ada", "Ceren", "İrem"
                },
                surnames = new string[]
                {
                    "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Yıldırım", "Öztürk",
                    "Aydın", "Özdemir", "Arslan", "Doğan", "Kılıç", "Aslan", "Çetin", "Kara",
                    "Koç", "Kurt", "Özkan", "Şimşek", "Polat", "Korkmaz", "Çakır", "Erdoğan",
                    "Aktaş", "Ünal", "Tan", "Acar", "Bulut", "Özer", "Güneş", "Kaplan",
                    "Tekin", "Keskin", "Karaca", "Sarı", "Sönmez", "Balcı", "Başar", "Erdem"
                }
            };
        }

        private CityData GetDefaultCityData()
        {
            return new CityData
            {
                cities = new City[]
                {
                    new City { name = "İstanbul", population = 15840900, region = "Marmara" },
                    new City { name = "Ankara", population = 5663322, region = "İç Anadolu" },
                    new City { name = "İzmir", population = 4394694, region = "Ege" },
                    new City { name = "Bursa", population = 3101833, region = "Marmara" },
                    new City { name = "Antalya", population = 2548308, region = "Akdeniz" },
                    new City { name = "Adana", population = 2237940, region = "Akdeniz" },
                    new City { name = "Konya", population = 2250020, region = "İç Anadolu" },
                    new City { name = "Gaziantep", population = 2101157, region = "Güneydoğu Anadolu" },
                    new City { name = "Şanlıurfa", population = 2073614, region = "Güneydoğu Anadolu" },
                    new City { name = "Kocaeli", population = 1997258, region = "Marmara" },
                    new City { name = "Mersin", population = 1868757, region = "Akdeniz" },
                    new City { name = "Diyarbakır", population = 1783431, region = "Güneydoğu Anadolu" },
                    new City { name = "Hatay", population = 1659320, region = "Akdeniz" },
                    new City { name = "Manisa", population = 1440611, region = "Ege" },
                    new City { name = "Kayseri", population = 1421455, region = "İç Anadolu" },
                    new City { name = "Samsun", population = 1356079, region = "Karadeniz" },
                    new City { name = "Balıkesir", population = 1240285, region = "Marmara" },
                    new City { name = "Kahramanmaraş", population = 1168163, region = "Akdeniz" },
                    new City { name = "Van", population = 1136757, region = "Doğu Anadolu" },
                    new City { name = "Aydın", population = 1119084, region = "Ege" }
                }
            };
        }

        private JobDatabase GetDefaultJobData()
        {
            return new JobDatabase
            {
                jobs = new JobData[]
                {
                    // Kamu
                    new JobData { id = "memur", title = "Devlet Memuru", category = "Kamu", baseSalary = 22000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "ogretmen", title = "Öğretmen", category = "Kamu", baseSalary = 25000, minEducation = 4, minIntelligence = 50 },
                    new JobData { id = "polis", title = "Polis Memuru", category = "Kamu", baseSalary = 28000, minEducation = 3, minIntelligence = 40 },

                    // Sağlık
                    new JobData { id = "doktor", title = "Doktor", category = "Sağlık", baseSalary = 80000, minEducation = 4, minIntelligence = 80 },
                    new JobData { id = "hemsire", title = "Hemşire", category = "Sağlık", baseSalary = 25000, minEducation = 4, minIntelligence = 50 },
                    new JobData { id = "eczaci", title = "Eczacı", category = "Sağlık", baseSalary = 45000, minEducation = 4, minIntelligence = 70 },

                    // Mühendislik
                    new JobData { id = "yazilimci", title = "Yazılım Mühendisi", category = "Mühendislik", baseSalary = 60000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "insaat_muh", title = "İnşaat Mühendisi", category = "Mühendislik", baseSalary = 40000, minEducation = 4, minIntelligence = 60 },
                    new JobData { id = "elektrik_muh", title = "Elektrik Mühendisi", category = "Mühendislik", baseSalary = 45000, minEducation = 4, minIntelligence = 65 },

                    // Ticaret
                    new JobData { id = "esnaf", title = "Esnaf", category = "Ticaret", baseSalary = 15000, minEducation = 2, minIntelligence = 30 },
                    new JobData { id = "emlakci", title = "Emlakçı", category = "Ticaret", baseSalary = 20000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "sigortaci", title = "Sigortacı", category = "Ticaret", baseSalary = 18000, minEducation = 3, minIntelligence = 45 },

                    // Hizmet
                    new JobData { id = "taksici", title = "Taksici", category = "Hizmet", baseSalary = 12000, minEducation = 2, minIntelligence = 20 },
                    new JobData { id = "garson", title = "Garson", category = "Hizmet", baseSalary = 11000, minEducation = 2, minIntelligence = 20 },
                    new JobData { id = "kuafor", title = "Kuaför", category = "Hizmet", baseSalary = 14000, minEducation = 2, minIntelligence = 30 },

                    // Medya
                    new JobData { id = "youtuber", title = "YouTuber", category = "Medya", baseSalary = 25000, minEducation = 2, minIntelligence = 40 },
                    new JobData { id = "gazeteci", title = "Gazeteci", category = "Medya", baseSalary = 20000, minEducation = 4, minIntelligence = 55 },

                    // Hukuk
                    new JobData { id = "avukat", title = "Avukat", category = "Hukuk", baseSalary = 50000, minEducation = 4, minIntelligence = 75 },
                    new JobData { id = "noter", title = "Noter", category = "Hukuk", baseSalary = 60000, minEducation = 4, minIntelligence = 70 },

                    // Tarım
                    new JobData { id = "ciftci", title = "Çiftçi", category = "Tarım", baseSalary = 10000, minEducation = 1, minIntelligence = 20 },
                    new JobData { id = "balikci", title = "Balıkçı", category = "Tarım", baseSalary = 12000, minEducation = 1, minIntelligence = 20 }
                }
            };
        }

        private UniversityDatabase GetDefaultUniversityData()
        {
            return new UniversityDatabase
            {
                universities = new UniversityData[]
                {
                    new UniversityData { name = "Boğaziçi Üniversitesi", city = "İstanbul", minScore = 480, type = "Devlet" },
                    new UniversityData { name = "ODTÜ", city = "Ankara", minScore = 475, type = "Devlet" },
                    new UniversityData { name = "İTÜ", city = "İstanbul", minScore = 470, type = "Devlet" },
                    new UniversityData { name = "Hacettepe Üniversitesi", city = "Ankara", minScore = 450, type = "Devlet" },
                    new UniversityData { name = "Ankara Üniversitesi", city = "Ankara", minScore = 420, type = "Devlet" },
                    new UniversityData { name = "İstanbul Üniversitesi", city = "İstanbul", minScore = 430, type = "Devlet" },
                    new UniversityData { name = "Ege Üniversitesi", city = "İzmir", minScore = 410, type = "Devlet" },
                    new UniversityData { name = "Dokuz Eylül Üniversitesi", city = "İzmir", minScore = 400, type = "Devlet" },
                    new UniversityData { name = "Koç Üniversitesi", city = "İstanbul", minScore = 460, type = "Vakıf" },
                    new UniversityData { name = "Sabancı Üniversitesi", city = "İstanbul", minScore = 455, type = "Vakıf" },
                    new UniversityData { name = "Bilkent Üniversitesi", city = "Ankara", minScore = 450, type = "Vakıf" },
                    new UniversityData { name = "Yeditepe Üniversitesi", city = "İstanbul", minScore = 350, type = "Vakıf" },
                    new UniversityData { name = "Gazi Üniversitesi", city = "Ankara", minScore = 380, type = "Devlet" },
                    new UniversityData { name = "Marmara Üniversitesi", city = "İstanbul", minScore = 390, type = "Devlet" },
                    new UniversityData { name = "Uludağ Üniversitesi", city = "Bursa", minScore = 360, type = "Devlet" }
                }
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Rastgele erkek ismi al.
        /// </summary>
        public string GetRandomMaleName()
        {
            if (_nameData == null || _nameData.maleNames == null || _nameData.maleNames.Length == 0)
            {
                Debug.LogError("[DataManager] Male names data not loaded!");
                return "Ali"; // Varsayılan isim
            }
            return _nameData.maleNames[UnityEngine.Random.Range(0, _nameData.maleNames.Length)];
        }

        /// <summary>
        /// Rastgele kadın ismi al.
        /// </summary>
        public string GetRandomFemaleName()
        {
            if (_nameData == null || _nameData.femaleNames == null || _nameData.femaleNames.Length == 0)
            {
                Debug.LogError("[DataManager] Female names data not loaded!");
                return "Ayşe"; // Varsayılan isim
            }
            return _nameData.femaleNames[UnityEngine.Random.Range(0, _nameData.femaleNames.Length)];
        }

        /// <summary>
        /// Rastgele soyisim al.
        /// </summary>
        public string GetRandomSurname()
        {
            if (_nameData == null || _nameData.surnames == null || _nameData.surnames.Length == 0)
            {
                Debug.LogError("[DataManager] Surnames data not loaded!");
                return "Yılmaz"; // Varsayılan soyisim
            }
            return _nameData.surnames[UnityEngine.Random.Range(0, _nameData.surnames.Length)];
        }

        /// <summary>
        /// Rastgele şehir al.
        /// </summary>
        public City GetRandomCity()
        {
            if (_cityData == null || _cityData.cities == null || _cityData.cities.Length == 0)
            {
                Debug.LogError("[DataManager] City data not loaded!");
                return new City { name = "İstanbul", population = 15840900, region = "Marmara" }; // Varsayılan şehir
            }
            return _cityData.cities[UnityEngine.Random.Range(0, _cityData.cities.Length)];
        }

        /// <summary>
        /// Meslekleri kriterlere göre filtrele.
        /// </summary>
        public List<JobData> GetAvailableJobs(int educationLevel, int intelligence)
        {
            var availableJobs = new List<JobData>();

            if (_jobDatabase == null || _jobDatabase.jobs == null)
            {
                Debug.LogError("[DataManager] Job database not loaded!");
                return availableJobs;
            }

            foreach (var job in _jobDatabase.jobs)
            {
                if (educationLevel >= job.minEducation && intelligence >= job.minIntelligence)
                {
                    availableJobs.Add(job);
                }
            }

            return availableJobs;
        }

        /// <summary>
        /// Üniversiteleri puana göre filtrele.
        /// </summary>
        public List<UniversityData> GetAvailableUniversities(int examScore)
        {
            var availableUniversities = new List<UniversityData>();

            if (_universityDatabase == null || _universityDatabase.universities == null)
            {
                Debug.LogError("[DataManager] University database not loaded!");
                return availableUniversities;
            }

            foreach (var uni in _universityDatabase.universities)
            {
                if (examScore >= uni.minScore)
                {
                    availableUniversities.Add(uni);
                }
            }

            return availableUniversities;
        }

        #endregion
    }
}

namespace TurkishLifeSim.Data
{
    [System.Serializable]
    public class NameData
    {
        public string[] maleNames;
        public string[] femaleNames;
        public string[] surnames;
    }

    [System.Serializable]
    public class CityData
    {
        public City[] cities;
    }

    [System.Serializable]
    public class City
    {
        public string name;
        public int population;
        public string region;
    }

    [System.Serializable]
    public class JobDatabase
    {
        public JobData[] jobs;
    }

    [System.Serializable]
    public class JobData
    {
        public string id;
        public string title;
        public string category;
        public decimal baseSalary;
        public int minEducation;
        public int minIntelligence;
    }

    [System.Serializable]
    public class UniversityDatabase
    {
        public UniversityData[] universities;
    }

    [System.Serializable]
    public class UniversityData
    {
        public string name;
        public string city;
        public int minScore;
        public string type;
    }
}
