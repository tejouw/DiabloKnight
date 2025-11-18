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
            LoadNameData();
            LoadCityData();
            LoadJobData();
            LoadUniversityData();

            Debug.Log("[DataManager] All data loaded successfully.");
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
                    // Kamu (8 iş)
                    new JobData { id = "memur", title = "Devlet Memuru", category = "Kamu", baseSalary = 22000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "ogretmen", title = "Öğretmen", category = "Kamu", baseSalary = 25000, minEducation = 4, minIntelligence = 50 },
                    new JobData { id = "polis", title = "Polis Memuru", category = "Kamu", baseSalary = 28000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "itfaiyeci", title = "İtfaiyeci", category = "Kamu", baseSalary = 26000, minEducation = 3, minIntelligence = 35 },
                    new JobData { id = "belediye", title = "Belediye Çalışanı", category = "Kamu", baseSalary = 20000, minEducation = 2, minIntelligence = 30 },
                    new JobData { id = "hakim", title = "Hakim", category = "Kamu", baseSalary = 70000, minEducation = 4, minIntelligence = 85 },
                    new JobData { id = "savci", title = "Savcı", category = "Kamu", baseSalary = 65000, minEducation = 4, minIntelligence = 80 },
                    new JobData { id = "diplomat", title = "Diplomat", category = "Kamu", baseSalary = 55000, minEducation = 4, minIntelligence = 75 },

                    // Sağlık (10 iş)
                    new JobData { id = "doktor", title = "Doktor", category = "Sağlık", baseSalary = 80000, minEducation = 4, minIntelligence = 80 },
                    new JobData { id = "hemsire", title = "Hemşire", category = "Sağlık", baseSalary = 25000, minEducation = 4, minIntelligence = 50 },
                    new JobData { id = "eczaci", title = "Eczacı", category = "Sağlık", baseSalary = 45000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "dis_hekimi", title = "Diş Hekimi", category = "Sağlık", baseSalary = 70000, minEducation = 4, minIntelligence = 75 },
                    new JobData { id = "veteriner", title = "Veteriner", category = "Sağlık", baseSalary = 40000, minEducation = 4, minIntelligence = 65 },
                    new JobData { id = "psikolog", title = "Psikolog", category = "Sağlık", baseSalary = 35000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "fizyoterapist", title = "Fizyoterapist", category = "Sağlık", baseSalary = 30000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "laborant", title = "Laborant", category = "Sağlık", baseSalary = 22000, minEducation = 3, minIntelligence = 45 },
                    new JobData { id = "ambulans", title = "Ambulans Şoförü", category = "Sağlık", baseSalary = 18000, minEducation = 2, minIntelligence = 30 },
                    new JobData { id = "cerrah", title = "Cerrah", category = "Sağlık", baseSalary = 120000, minEducation = 6, minIntelligence = 90 },

                    // Mühendislik (12 iş)
                    new JobData { id = "yazilimci", title = "Yazılım Mühendisi", category = "Mühendislik", baseSalary = 60000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "insaat_muh", title = "İnşaat Mühendisi", category = "Mühendislik", baseSalary = 40000, minEducation = 4, minIntelligence = 60 },
                    new JobData { id = "elektrik_muh", title = "Elektrik Mühendisi", category = "Mühendislik", baseSalary = 45000, minEducation = 4, minIntelligence = 65 },
                    new JobData { id = "makine_muh", title = "Makine Mühendisi", category = "Mühendislik", baseSalary = 42000, minEducation = 4, minIntelligence = 65 },
                    new JobData { id = "kimya_muh", title = "Kimya Mühendisi", category = "Mühendislik", baseSalary = 40000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "endustri_muh", title = "Endüstri Mühendisi", category = "Mühendislik", baseSalary = 38000, minEducation = 4, minIntelligence = 60 },
                    new JobData { id = "mimarlik", title = "Mimar", category = "Mühendislik", baseSalary = 45000, minEducation = 4, minIntelligence = 65 },
                    new JobData { id = "veri_bilimci", title = "Veri Bilimci", category = "Mühendislik", baseSalary = 70000, minEducation = 5, minIntelligence = 80 },
                    new JobData { id = "siber_guvenlik", title = "Siber Güvenlik Uzmanı", category = "Mühendislik", baseSalary = 65000, minEducation = 4, minIntelligence = 75 },
                    new JobData { id = "robotik", title = "Robotik Mühendisi", category = "Mühendislik", baseSalary = 55000, minEducation = 4, minIntelligence = 75 },
                    new JobData { id = "cevre_muh", title = "Çevre Mühendisi", category = "Mühendislik", baseSalary = 35000, minEducation = 4, minIntelligence = 60 },
                    new JobData { id = "biyomedikal", title = "Biyomedikal Mühendisi", category = "Mühendislik", baseSalary = 50000, minEducation = 4, minIntelligence = 70 },

                    // Ticaret (10 iş)
                    new JobData { id = "esnaf", title = "Esnaf", category = "Ticaret", baseSalary = 15000, minEducation = 2, minIntelligence = 30 },
                    new JobData { id = "emlakci", title = "Emlakçı", category = "Ticaret", baseSalary = 20000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "sigortaci", title = "Sigortacı", category = "Ticaret", baseSalary = 18000, minEducation = 3, minIntelligence = 45 },
                    new JobData { id = "bankaci", title = "Bankacı", category = "Ticaret", baseSalary = 28000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "muhasebeci", title = "Muhasebeci", category = "Ticaret", baseSalary = 25000, minEducation = 4, minIntelligence = 50 },
                    new JobData { id = "dis_ticaret", title = "Dış Ticaret Uzmanı", category = "Ticaret", baseSalary = 35000, minEducation = 4, minIntelligence = 60 },
                    new JobData { id = "borsa", title = "Borsa Uzmanı", category = "Ticaret", baseSalary = 50000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "pazarlama", title = "Pazarlama Uzmanı", category = "Ticaret", baseSalary = 30000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "satis_temsilcisi", title = "Satış Temsilcisi", category = "Ticaret", baseSalary = 20000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "is_gelistirme", title = "İş Geliştirme Uzmanı", category = "Ticaret", baseSalary = 40000, minEducation = 4, minIntelligence = 60 },

                    // Hizmet (12 iş)
                    new JobData { id = "taksici", title = "Taksici", category = "Hizmet", baseSalary = 12000, minEducation = 2, minIntelligence = 20 },
                    new JobData { id = "garson", title = "Garson", category = "Hizmet", baseSalary = 11000, minEducation = 2, minIntelligence = 20 },
                    new JobData { id = "kuafor", title = "Kuaför", category = "Hizmet", baseSalary = 14000, minEducation = 2, minIntelligence = 30 },
                    new JobData { id = "asci", title = "Aşçı", category = "Hizmet", baseSalary = 16000, minEducation = 2, minIntelligence = 35 },
                    new JobData { id = "otel_resepsiyon", title = "Otel Resepsiyonisti", category = "Hizmet", baseSalary = 15000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "guvenlik", title = "Güvenlik Görevlisi", category = "Hizmet", baseSalary = 13000, minEducation = 2, minIntelligence = 25 },
                    new JobData { id = "temizlik", title = "Temizlik Görevlisi", category = "Hizmet", baseSalary = 11000, minEducation = 1, minIntelligence = 15 },
                    new JobData { id = "kurye", title = "Kurye", category = "Hizmet", baseSalary = 12000, minEducation = 2, minIntelligence = 20 },
                    new JobData { id = "tur_rehberi", title = "Tur Rehberi", category = "Hizmet", baseSalary = 18000, minEducation = 4, minIntelligence = 50 },
                    new JobData { id = "fitness", title = "Fitness Eğitmeni", category = "Hizmet", baseSalary = 16000, minEducation = 3, minIntelligence = 35 },
                    new JobData { id = "kahveci", title = "Barista", category = "Hizmet", baseSalary = 13000, minEducation = 2, minIntelligence = 25 },
                    new JobData { id = "surucu", title = "Özel Şoför", category = "Hizmet", baseSalary = 15000, minEducation = 2, minIntelligence = 25 },

                    // Medya ve Sanat (10 iş)
                    new JobData { id = "youtuber", title = "YouTuber", category = "Medya", baseSalary = 25000, minEducation = 2, minIntelligence = 40 },
                    new JobData { id = "gazeteci", title = "Gazeteci", category = "Medya", baseSalary = 20000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "influencer", title = "Sosyal Medya Influencer", category = "Medya", baseSalary = 30000, minEducation = 2, minIntelligence = 35 },
                    new JobData { id = "fotografci", title = "Fotoğrafçı", category = "Medya", baseSalary = 18000, minEducation = 3, minIntelligence = 45 },
                    new JobData { id = "kameraman", title = "Kameraman", category = "Medya", baseSalary = 22000, minEducation = 3, minIntelligence = 45 },
                    new JobData { id = "grafik_tasarimci", title = "Grafik Tasarımcı", category = "Medya", baseSalary = 25000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "muzisyen", title = "Müzisyen", category = "Medya", baseSalary = 20000, minEducation = 2, minIntelligence = 40 },
                    new JobData { id = "oyuncu", title = "Oyuncu", category = "Medya", baseSalary = 35000, minEducation = 3, minIntelligence = 45 },
                    new JobData { id = "yonetmen", title = "Yönetmen", category = "Medya", baseSalary = 50000, minEducation = 4, minIntelligence = 65 },
                    new JobData { id = "ses_muhendisi", title = "Ses Mühendisi", category = "Medya", baseSalary = 28000, minEducation = 4, minIntelligence = 55 },

                    // Hukuk (6 iş)
                    new JobData { id = "avukat", title = "Avukat", category = "Hukuk", baseSalary = 50000, minEducation = 4, minIntelligence = 75 },
                    new JobData { id = "noter", title = "Noter", category = "Hukuk", baseSalary = 60000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "hukuk_danismani", title = "Hukuk Danışmanı", category = "Hukuk", baseSalary = 45000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "icra_memuru", title = "İcra Memuru", category = "Hukuk", baseSalary = 25000, minEducation = 3, minIntelligence = 45 },
                    new JobData { id = "arabulucu", title = "Arabulucu", category = "Hukuk", baseSalary = 40000, minEducation = 4, minIntelligence = 65 },
                    new JobData { id = "patent_vekili", title = "Patent Vekili", category = "Hukuk", baseSalary = 55000, minEducation = 4, minIntelligence = 70 },

                    // Tarım (6 iş)
                    new JobData { id = "ciftci", title = "Çiftçi", category = "Tarım", baseSalary = 10000, minEducation = 1, minIntelligence = 20 },
                    new JobData { id = "balikci", title = "Balıkçı", category = "Tarım", baseSalary = 12000, minEducation = 1, minIntelligence = 20 },
                    new JobData { id = "ziraat_muh", title = "Ziraat Mühendisi", category = "Tarım", baseSalary = 30000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "hayvancilik", title = "Hayvancılık Uzmanı", category = "Tarım", baseSalary = 22000, minEducation = 3, minIntelligence = 40 },
                    new JobData { id = "ormancilik", title = "Orman Mühendisi", category = "Tarım", baseSalary = 28000, minEducation = 4, minIntelligence = 50 },
                    new JobData { id = "gida_muh", title = "Gıda Mühendisi", category = "Tarım", baseSalary = 35000, minEducation = 4, minIntelligence = 60 },

                    // Teknoloji (8 iş)
                    new JobData { id = "sistem_admin", title = "Sistem Yöneticisi", category = "Teknoloji", baseSalary = 40000, minEducation = 4, minIntelligence = 60 },
                    new JobData { id = "mobil_gelistirici", title = "Mobil Uygulama Geliştirici", category = "Teknoloji", baseSalary = 55000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "oyun_gelistirici", title = "Oyun Geliştirici", category = "Teknoloji", baseSalary = 50000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "devops", title = "DevOps Mühendisi", category = "Teknoloji", baseSalary = 60000, minEducation = 4, minIntelligence = 70 },
                    new JobData { id = "ui_ux", title = "UI/UX Tasarımcı", category = "Teknoloji", baseSalary = 45000, minEducation = 4, minIntelligence = 60 },
                    new JobData { id = "qa_engineer", title = "Test Mühendisi", category = "Teknoloji", baseSalary = 35000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "blockchain", title = "Blockchain Geliştirici", category = "Teknoloji", baseSalary = 75000, minEducation = 4, minIntelligence = 80 },
                    new JobData { id = "ai_engineer", title = "Yapay Zeka Mühendisi", category = "Teknoloji", baseSalary = 80000, minEducation = 5, minIntelligence = 85 },

                    // Spor (6 iş)
                    new JobData { id = "futbolcu", title = "Profesyonel Futbolcu", category = "Spor", baseSalary = 100000, minEducation = 2, minIntelligence = 30 },
                    new JobData { id = "basketbolcu", title = "Profesyonel Basketbolcu", category = "Spor", baseSalary = 80000, minEducation = 2, minIntelligence = 30 },
                    new JobData { id = "antrenor", title = "Spor Antrenörü", category = "Spor", baseSalary = 25000, minEducation = 3, minIntelligence = 45 },
                    new JobData { id = "spor_yorumcusu", title = "Spor Yorumcusu", category = "Spor", baseSalary = 35000, minEducation = 4, minIntelligence = 55 },
                    new JobData { id = "esports", title = "E-Spor Oyuncusu", category = "Spor", baseSalary = 40000, minEducation = 2, minIntelligence = 50 },
                    new JobData { id = "menajer", title = "Sporcu Menajeri", category = "Spor", baseSalary = 45000, minEducation = 4, minIntelligence = 60 }
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
            return _nameData.maleNames[UnityEngine.Random.Range(0, _nameData.maleNames.Length)];
        }

        /// <summary>
        /// Rastgele kadın ismi al.
        /// </summary>
        public string GetRandomFemaleName()
        {
            return _nameData.femaleNames[UnityEngine.Random.Range(0, _nameData.femaleNames.Length)];
        }

        /// <summary>
        /// Rastgele soyisim al.
        /// </summary>
        public string GetRandomSurname()
        {
            return _nameData.surnames[UnityEngine.Random.Range(0, _nameData.surnames.Length)];
        }

        /// <summary>
        /// Rastgele şehir al.
        /// </summary>
        public City GetRandomCity()
        {
            return _cityData.cities[UnityEngine.Random.Range(0, _cityData.cities.Length)];
        }

        /// <summary>
        /// Meslekleri kriterlere göre filtrele.
        /// </summary>
        public List<JobData> GetAvailableJobs(int educationLevel, int intelligence)
        {
            var availableJobs = new List<JobData>();

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
