using UnityEngine;
using System.Collections.Generic;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Systems
{
    /// <summary>
    /// Eğitim Sistemi - Okul, üniversite, sınav yönetimi.
    /// </summary>
    public class EducationSystem : Singleton<EducationSystem>
    {
        private List<UniversityDefinition> _universities = new List<UniversityDefinition>();
        private List<string> _departments = new List<string>();

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            InitializeEducationData();
        }

        private void InitializeEducationData()
        {
            // Türk üniversiteleri
            _universities = new List<UniversityDefinition>
            {
                // Devlet Üniversiteleri
                new UniversityDefinition("bogazici", "Boğaziçi Üniversitesi", UniversityType.Public, 480, true),
                new UniversityDefinition("odtu", "ODTÜ", UniversityType.Public, 470, true),
                new UniversityDefinition("itu", "İstanbul Teknik Üniversitesi", UniversityType.Public, 460, true),
                new UniversityDefinition("hacettepe", "Hacettepe Üniversitesi", UniversityType.Public, 450, true),
                new UniversityDefinition("ankara", "Ankara Üniversitesi", UniversityType.Public, 430, true),
                new UniversityDefinition("istanbul", "İstanbul Üniversitesi", UniversityType.Public, 420, true),
                new UniversityDefinition("ege", "Ege Üniversitesi", UniversityType.Public, 400, true),
                new UniversityDefinition("gazi", "Gazi Üniversitesi", UniversityType.Public, 390, true),
                new UniversityDefinition("dokuz_eylul", "Dokuz Eylül Üniversitesi", UniversityType.Public, 380, true),
                new UniversityDefinition("marmara", "Marmara Üniversitesi", UniversityType.Public, 370, true),
                new UniversityDefinition("cukurova", "Çukurova Üniversitesi", UniversityType.Public, 350, false),
                new UniversityDefinition("selcuk", "Selçuk Üniversitesi", UniversityType.Public, 320, false),
                new UniversityDefinition("ataturk", "Atatürk Üniversitesi", UniversityType.Public, 300, false),
                new UniversityDefinition("akdeniz", "Akdeniz Üniversitesi", UniversityType.Public, 310, false),

                // Vakıf Üniversiteleri
                new UniversityDefinition("koc", "Koç Üniversitesi", UniversityType.Private, 460, true, 150000),
                new UniversityDefinition("sabanci", "Sabancı Üniversitesi", UniversityType.Private, 450, true, 145000),
                new UniversityDefinition("bilkent", "Bilkent Üniversitesi", UniversityType.Private, 440, true, 130000),
                new UniversityDefinition("ozyegin", "Özyeğin Üniversitesi", UniversityType.Private, 400, true, 120000),
                new UniversityDefinition("bahcesehir", "Bahçeşehir Üniversitesi", UniversityType.Private, 350, false, 100000),
                new UniversityDefinition("yeditepe", "Yeditepe Üniversitesi", UniversityType.Private, 340, false, 95000),
                new UniversityDefinition("medipol", "Medipol Üniversitesi", UniversityType.Private, 330, false, 90000),
                new UniversityDefinition("istanbul_bilgi", "İstanbul Bilgi Üniversitesi", UniversityType.Private, 320, false, 85000),
            };

            // Bölümler
            _departments = new List<string>
            {
                "Tıp", "Hukuk", "Mühendislik", "İşletme", "Ekonomi", "Psikoloji",
                "Bilgisayar Mühendisliği", "Elektrik Mühendisliği", "Makine Mühendisliği",
                "İnşaat Mühendisliği", "Mimarlık", "Diş Hekimliği", "Eczacılık",
                "Hemşirelik", "Öğretmenlik", "Sosyoloji", "Tarih", "Edebiyat",
                "Fizik", "Kimya", "Biyoloji", "Matematik", "İletişim", "Grafik Tasarım",
                "Uluslararası İlişkiler", "Siyaset Bilimi", "Felsefe", "Müzik", "Güzel Sanatlar"
            };

            Debug.Log($"[EducationSystem] Loaded {_universities.Count} universities and {_departments.Count} departments.");
        }

        #endregion

        #region School Progress

        /// <summary>
        /// Yaşa göre otomatik eğitim ilerlemesi.
        /// </summary>
        public void ProcessEducationProgress(CharacterData character)
        {
            int age = character.Age;

            // 6 yaşında ilkokula başla
            if (age == 6 && character.Education.CurrentLevel == EducationLevel.None)
            {
                StartSchool(character, EducationLevel.PrimarySchool);
            }
            // 10 yaşında ortaokula geç
            else if (age == 10 && character.Education.CurrentLevel == EducationLevel.PrimarySchool)
            {
                GraduateAndProgress(character, EducationLevel.MiddleSchool);
            }
            // 14 yaşında liseye geç
            else if (age == 14 && character.Education.CurrentLevel == EducationLevel.MiddleSchool)
            {
                GraduateAndProgress(character, EducationLevel.HighSchool);
            }
            // 18 yaşında lise bitir (üniversite isteğe bağlı)
            else if (age == 18 && character.Education.CurrentLevel == EducationLevel.HighSchool)
            {
                character.Education.isGraduated = true;
                // YKS sınavı için hazırlık
            }
        }

        private void StartSchool(CharacterData character, EducationLevel level)
        {
            character.Education.currentLevel = level;
            character.Education.schoolName = GenerateSchoolName(level);
            character.Education.gpa = 2.5f + Random.Range(-0.5f, 1f);
            character.Education.isGraduated = false;

            EventBus.Publish(new EducationChangedEvent
            {
                NewLevel = level,
                SchoolName = character.Education.schoolName,
                ChangeType = EducationChangeType.Started
            });
        }

        private void GraduateAndProgress(CharacterData character, EducationLevel newLevel)
        {
            var oldLevel = character.Education.CurrentLevel;

            // Diploma ekle
            character.Education.achievements.Add($"{oldLevel} Diploması");

            // Yeni seviyeye geç
            character.Education.currentLevel = newLevel;
            character.Education.schoolName = GenerateSchoolName(newLevel);

            EventBus.Publish(new EducationChangedEvent
            {
                NewLevel = newLevel,
                SchoolName = character.Education.schoolName,
                ChangeType = EducationChangeType.Graduated
            });
        }

        #endregion

        #region Study & Exams

        /// <summary>
        /// Ders çalış.
        /// </summary>
        public StudyResult Study(CharacterData character, int hours)
        {
            if (character.Education.CurrentLevel == EducationLevel.None)
            {
                return new StudyResult
                {
                    success = false,
                    message = "Şu an okula gitmiyorsun."
                };
            }

            // GPA artışı
            float gpaGain = hours * 0.05f * (character.Stats.Intelligence / 100f);
            character.Education.gpa = Mathf.Min(4.0f, character.Education.gpa + gpaGain);

            // Zeka artışı
            if (Random.value < 0.3f)
            {
                character.Stats.ModifyStat(StatType.Intelligence, 1);
            }

            // Yorgunluk
            character.Stats.ModifyStat(StatType.Happiness, -hours);
            character.Stats.ModifyStat(StatType.Health, -hours / 2);

            return new StudyResult
            {
                success = true,
                message = $"{hours} saat ders çalıştın. GPA: {character.Education.gpa:F2}",
                gpaChange = gpaGain
            };
        }

        /// <summary>
        /// YKS sınavına gir.
        /// </summary>
        public YKSResult TakeYKSExam(CharacterData character)
        {
            if (character.Education.CurrentLevel != EducationLevel.HighSchool || character.Age < 17)
            {
                return new YKSResult
                {
                    success = false,
                    message = "YKS'ye girebilmek için lise son sınıf olmalısın.",
                    score = 0
                };
            }

            // Puan hesapla (0-500)
            float baseScore = character.Stats.Intelligence * 3;
            baseScore += character.Education.gpa * 50;
            baseScore += Random.Range(-30, 30); // Şans faktörü

            int score = Mathf.RoundToInt(Mathf.Clamp(baseScore, 100, 500));
            character.Education.yksScore = score;

            string message;
            if (score >= 450)
                message = $"Muhteşem! {score} puan aldın. En iyi üniversitelere girebilirsin!";
            else if (score >= 400)
                message = $"Çok iyi! {score} puan aldın. İyi üniversiteleri değerlendirebilirsin.";
            else if (score >= 300)
                message = $"Fena değil. {score} puan aldın. Bazı üniversitelere girebilirsin.";
            else if (score >= 200)
                message = $"{score} puan aldın. Seçeneklerin kısıtlı olabilir.";
            else
                message = $"Ne yazık ki {score} puan aldın. Gelecek yıl tekrar deneyebilirsin.";

            return new YKSResult
            {
                success = true,
                score = score,
                message = message
            };
        }

        /// <summary>
        /// Üniversiteye başvur.
        /// </summary>
        public UniversityApplicationResult ApplyToUniversity(CharacterData character, string universityId, string department)
        {
            var university = _universities.Find(u => u.id == universityId);
            if (university == null)
            {
                return new UniversityApplicationResult
                {
                    success = false,
                    message = "Üniversite bulunamadı."
                };
            }

            // YKS puanı kontrolü
            if (character.Education.yksScore < university.requiredScore)
            {
                return new UniversityApplicationResult
                {
                    success = false,
                    message = $"{university.name} için en az {university.requiredScore} puan gerekiyor. Senin puanın: {character.Education.yksScore}"
                };
            }

            // Vakıf üniversitesi ise ücret kontrolü
            if (university.type == UniversityType.Private)
            {
                if (character.Finances.CurrentMoney < university.tuitionFee)
                {
                    return new UniversityApplicationResult
                    {
                        success = false,
                        message = $"{university.name} yıllık ücreti {university.tuitionFee:N0} TL. Yeterli bakiyen yok."
                    };
                }

                // Ücreti düş
                character.Finances.ModifyMoney(-university.tuitionFee, $"{university.name} kayıt ücreti");
            }

            // Kabul
            character.Education.currentLevel = EducationLevel.University;
            character.Education.universityName = university.name;
            character.Education.department = department;
            character.Education.gpa = 2.5f;
            character.Education.isGraduated = false;

            EventBus.Publish(new EducationChangedEvent
            {
                NewLevel = EducationLevel.University,
                SchoolName = university.name,
                ChangeType = EducationChangeType.Started
            });

            return new UniversityApplicationResult
            {
                success = true,
                message = $"Tebrikler! {university.name} {department} bölümüne kabul edildin!"
            };
        }

        /// <summary>
        /// Üniversiteden mezun ol.
        /// </summary>
        public void GraduateFromUniversity(CharacterData character)
        {
            if (character.Education.CurrentLevel != EducationLevel.University)
                return;

            character.Education.isGraduated = true;
            character.Education.achievements.Add($"{character.Education.universityName} - {character.Education.department} Diploması");

            // Zeka bonusu
            character.Stats.ModifyStat(StatType.Intelligence, 10);

            EventBus.Publish(new EducationChangedEvent
            {
                NewLevel = EducationLevel.University,
                SchoolName = character.Education.universityName,
                ChangeType = EducationChangeType.Graduated
            });
        }

        /// <summary>
        /// Yüksek lisansa başvur.
        /// </summary>
        public bool ApplyForMasters(CharacterData character)
        {
            if (character.Education.CurrentLevel != EducationLevel.University || !character.Education.isGraduated)
                return false;

            if (character.Education.gpa < 2.5f)
                return false;

            // Başarı şansı
            float chance = character.Education.gpa / 4f;
            if (Random.value < chance)
            {
                character.Education.currentLevel = EducationLevel.Masters;
                character.Education.isGraduated = false;
                character.Education.gpa = 3.0f;

                EventBus.Publish(new EducationChangedEvent
                {
                    NewLevel = EducationLevel.Masters,
                    SchoolName = character.Education.universityName,
                    ChangeType = EducationChangeType.Started
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// Doktoraya başvur.
        /// </summary>
        public bool ApplyForDoctorate(CharacterData character)
        {
            if (character.Education.CurrentLevel != EducationLevel.Masters || !character.Education.isGraduated)
                return false;

            if (character.Education.gpa < 3.0f)
                return false;

            // Başarı şansı
            float chance = (character.Education.gpa - 2f) / 2f;
            if (Random.value < chance)
            {
                character.Education.currentLevel = EducationLevel.Doctorate;
                character.Education.isGraduated = false;
                character.Education.gpa = 3.0f;

                EventBus.Publish(new EducationChangedEvent
                {
                    NewLevel = EducationLevel.Doctorate,
                    SchoolName = character.Education.universityName,
                    ChangeType = EducationChangeType.Started
                });

                return true;
            }

            return false;
        }

        #endregion

        #region Utilities

        private string GenerateSchoolName(EducationLevel level)
        {
            string[] prefixes = { "Atatürk", "Cumhuriyet", "Fatih", "Mimar Sinan", "Yunus Emre", "Mehmet Akif" };
            string suffix = level switch
            {
                EducationLevel.PrimarySchool => "İlkokulu",
                EducationLevel.MiddleSchool => "Ortaokulu",
                EducationLevel.HighSchool => "Lisesi",
                _ => "Okulu"
            };

            return $"{prefixes[Random.Range(0, prefixes.Length)]} {suffix}";
        }

        public List<UniversityDefinition> GetAvailableUniversities(int yksScore)
        {
            return _universities.FindAll(u => u.requiredScore <= yksScore);
        }

        public List<string> GetDepartments()
        {
            return new List<string>(_departments);
        }

        #endregion
    }

    #region Data Classes

    [System.Serializable]
    public class UniversityDefinition
    {
        public string id;
        public string name;
        public UniversityType type;
        public int requiredScore;
        public bool isPrestigious;
        public decimal tuitionFee;

        public UniversityDefinition(string id, string name, UniversityType type, int requiredScore, bool isPrestigious, decimal tuitionFee = 0)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.requiredScore = requiredScore;
            this.isPrestigious = isPrestigious;
            this.tuitionFee = tuitionFee;
        }
    }

    public enum UniversityType
    {
        Public,
        Private
    }

    public class StudyResult
    {
        public bool success;
        public string message;
        public float gpaChange;
    }

    public class YKSResult
    {
        public bool success;
        public int score;
        public string message;
    }

    public class UniversityApplicationResult
    {
        public bool success;
        public string message;
    }

    public enum EducationChangeType
    {
        Started,
        Graduated,
        DroppedOut
    }

    public struct EducationChangedEvent : IGameEvent
    {
        public EducationLevel NewLevel;
        public string SchoolName;
        public EducationChangeType ChangeType;
    }

    #endregion
}
