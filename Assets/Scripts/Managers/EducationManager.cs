using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;
using TurkishLifeSim.Data;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Eğitim Yöneticisi - Okul, sınav ve eğitim sistemini yönetir.
    /// </summary>
    public class EducationManager : Singleton<EducationManager>
    {
        #region Constants

        public const int PRIMARY_SCHOOL_START_AGE = 6;
        public const int PRIMARY_SCHOOL_END_AGE = 10;
        public const int MIDDLE_SCHOOL_START_AGE = 10;
        public const int MIDDLE_SCHOOL_END_AGE = 14;
        public const int HIGH_SCHOOL_START_AGE = 14;
        public const int HIGH_SCHOOL_END_AGE = 18;
        public const int YKS_AGE = 18;
        public const int UNIVERSITY_DURATION = 4;

        #endregion

        #region Lifecycle

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();
            Debug.Log("[EducationManager] Initialized successfully.");
        }

        #endregion

        #region Education Progress

        /// <summary>
        /// Yıllık eğitim güncellemesi.
        /// </summary>
        public void ProcessYearlyEducationUpdate(CharacterData character)
        {
            if (character == null) return;

            int age = character.Age;
            var education = character.Education;

            // Otomatik okul başlatma
            if (age == PRIMARY_SCHOOL_START_AGE && education.CurrentLevel == EducationLevel.None)
            {
                StartPrimarySchool(character);
            }
            else if (age == MIDDLE_SCHOOL_START_AGE && education.CurrentLevel == EducationLevel.PrimarySchool)
            {
                GraduateAndAdvance(character, EducationLevel.MiddleSchool, "Ortaokul");
            }
            else if (age == HIGH_SCHOOL_START_AGE && education.CurrentLevel == EducationLevel.MiddleSchool)
            {
                GraduateAndAdvance(character, EducationLevel.HighSchool, "Anadolu Lisesi");
            }

            // Üniversite ilerlemesi
            if (education.CurrentLevel == EducationLevel.University && !education.isGraduated)
            {
                ProcessUniversityYear(character);
            }

            // GPA hesapla
            if (education.CurrentLevel != EducationLevel.None && !education.isGraduated)
            {
                UpdateGPA(character);
            }
        }

        /// <summary>
        /// İlkokula başla.
        /// </summary>
        private void StartPrimarySchool(CharacterData character)
        {
            character.Education.currentLevel = EducationLevel.PrimarySchool;
            character.Education.schoolName = GenerateSchoolName("İlkokul");
            character.Education.gpa = 0f;
            character.Education.isGraduated = false;

            Debug.Log($"[EducationManager] {character.FullName} started primary school at {character.Education.schoolName}");
        }

        /// <summary>
        /// Mezun ol ve üst seviyeye geç.
        /// </summary>
        private void GraduateAndAdvance(CharacterData character, EducationLevel newLevel, string schoolType)
        {
            // Başarı kontrolü
            float graduationChance = 0.7f + character.Stats.Intelligence * 0.003f;

            if (Random.value < graduationChance)
            {
                character.Education.currentLevel = newLevel;
                character.Education.schoolName = GenerateSchoolName(schoolType);
                character.Education.gpa = 0f;

                // Zeka artışı
                character.Stats.ModifyStat(StatType.Intelligence, Random.Range(1, 3));

                Debug.Log($"[EducationManager] {character.FullName} advanced to {newLevel}");
            }
            else
            {
                // Sınıfta kaldı
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(-10, -5));
                Debug.Log($"[EducationManager] {character.FullName} failed to graduate");
            }
        }

        /// <summary>
        /// Okul ismi oluştur.
        /// </summary>
        private string GenerateSchoolName(string type)
        {
            string[] prefixes = { "Atatürk", "Cumhuriyet", "Fatih", "Mimar Sinan", "Yunus Emre", "Mehmet Akif" };
            string prefix = prefixes[Random.Range(0, prefixes.Length)];
            return $"{prefix} {type}";
        }

        /// <summary>
        /// GPA güncelle.
        /// </summary>
        private void UpdateGPA(CharacterData character)
        {
            // Zeka, çalışma ve şans bazlı GPA
            float baseGPA = character.Stats.Intelligence / 100f * 3f;
            float randomFactor = Random.Range(-0.5f, 0.5f);
            float newGPA = Mathf.Clamp(baseGPA + randomFactor + 1f, 0f, 4f);

            // Ağırlıklı ortalama
            if (character.Education.gpa > 0)
            {
                character.Education.gpa = (character.Education.gpa + newGPA) / 2f;
            }
            else
            {
                character.Education.gpa = newGPA;
            }
        }

        #endregion

        #region University & YKS

        /// <summary>
        /// YKS sınavına gir.
        /// </summary>
        public int TakeYKSExam(CharacterData character)
        {
            if (character == null || character.Education.CurrentLevel != EducationLevel.HighSchool)
                return 0;

            // Temel puan (zeka + GPA bazlı)
            float baseFactor = character.Stats.Intelligence / 100f;
            float gpaFactor = character.Education.gpa / 4f;

            // 0-500 arası puan
            int score = (int)((baseFactor * 0.6f + gpaFactor * 0.4f) * 400 + Random.Range(50, 100));
            score = Mathf.Clamp(score, 0, 500);

            character.Education.yksScore = score;

            Debug.Log($"[EducationManager] {character.FullName} scored {score} in YKS");

            return score;
        }

        /// <summary>
        /// Üniversiteye kayıt ol.
        /// </summary>
        public bool EnrollInUniversity(CharacterData character, UniversityData university)
        {
            if (character == null || university == null) return false;

            if (character.Education.yksScore < university.minScore)
            {
                return false;
            }

            character.Education.currentLevel = EducationLevel.University;
            character.Education.schoolName = university.name;
            character.Education.universityName = university.name;
            character.Education.department = GetRandomDepartment();
            character.Education.gpa = 0f;
            character.Education.isGraduated = false;

            // Mutluluk artışı
            character.Stats.ModifyStat(StatType.Happiness, Random.Range(10, 20));

            Debug.Log($"[EducationManager] {character.FullName} enrolled in {university.name} - {character.Education.department}");

            return true;
        }

        /// <summary>
        /// Rastgele bölüm getir.
        /// </summary>
        private string GetRandomDepartment()
        {
            string[] departments = {
                "Bilgisayar Mühendisliği", "Elektrik Mühendisliği", "Makine Mühendisliği",
                "Tıp", "Hukuk", "İşletme", "Ekonomi", "Psikoloji", "Mimarlık",
                "İletişim", "Tarih", "Edebiyat", "Fizik", "Kimya", "Biyoloji"
            };
            return departments[Random.Range(0, departments.Length)];
        }

        /// <summary>
        /// Üniversite yılı işle.
        /// </summary>
        private void ProcessUniversityYear(CharacterData character)
        {
            // Yıl sayacı (achievements içinde tutulabilir)
            int yearsInUni = character.Education.achievements.Count(a => a.StartsWith("UNI_YEAR_"));

            if (yearsInUni >= UNIVERSITY_DURATION)
            {
                // Mezuniyet
                GraduateFromUniversity(character);
            }
            else
            {
                // Yıl ilerle
                character.Education.achievements.Add($"UNI_YEAR_{yearsInUni + 1}");
                UpdateGPA(character);
            }
        }

        /// <summary>
        /// Üniversiteden mezun ol.
        /// </summary>
        public void GraduateFromUniversity(CharacterData character)
        {
            if (character?.Education == null) return;

            // GPA kontrolü
            if (character.Education.gpa >= 2.0f)
            {
                character.Education.isGraduated = true;
                character.Education.achievements.Add("GRADUATED");

                // Büyük zeka ve mutluluk artışı
                character.Stats.ModifyStat(StatType.Intelligence, Random.Range(5, 10));
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(15, 25));

                // Şöhret artışı
                if (character.Education.gpa >= 3.5f)
                {
                    character.Stats.ModifyStat(StatType.Fame, Random.Range(3, 8));
                    character.Education.achievements.Add("HONOR_GRADUATE");
                }

                Debug.Log($"[EducationManager] {character.FullName} graduated from university with GPA {character.Education.gpa:F2}!");
            }
            else
            {
                // Başarısız - üniversiteden atıldı
                character.Education.currentLevel = EducationLevel.HighSchool;
                character.Stats.ModifyStat(StatType.Happiness, Random.Range(-20, -10));

                Debug.Log($"[EducationManager] {character.FullName} failed to graduate from university.");
            }
        }

        #endregion

        #region Advanced Education

        /// <summary>
        /// Yüksek lisansa başla.
        /// </summary>
        public bool StartMasters(CharacterData character)
        {
            if (character?.Education == null) return false;
            if (character.Education.CurrentLevel != EducationLevel.University || !character.Education.isGraduated)
                return false;

            if (character.Education.gpa < 2.5f) return false;

            character.Education.currentLevel = EducationLevel.Masters;
            character.Education.isGraduated = false;
            character.Education.gpa = 0f;

            Debug.Log($"[EducationManager] {character.FullName} started Masters program");
            return true;
        }

        /// <summary>
        /// Doktoraya başla.
        /// </summary>
        public bool StartDoctorate(CharacterData character)
        {
            if (character?.Education == null) return false;
            if (character.Education.CurrentLevel != EducationLevel.Masters || !character.Education.isGraduated)
                return false;

            if (character.Education.gpa < 3.0f) return false;

            character.Education.currentLevel = EducationLevel.Doctorate;
            character.Education.isGraduated = false;
            character.Education.gpa = 0f;

            Debug.Log($"[EducationManager] {character.FullName} started Doctorate program");
            return true;
        }

        #endregion

        #region Study Actions

        /// <summary>
        /// Ders çalış.
        /// </summary>
        public void Study(CharacterData character, int hours)
        {
            if (character == null) return;

            // Zeka artışı
            int intelligenceGain = Mathf.CeilToInt(hours * 0.5f);
            character.Stats.ModifyStat(StatType.Intelligence, intelligenceGain);

            // Sağlık ve mutluluk kaybı
            if (hours > 3)
            {
                character.Stats.ModifyStat(StatType.Happiness, -hours / 2);
                character.Stats.ModifyStat(StatType.Health, -hours / 3);
            }

            // GPA boost
            character.Education.gpa = Mathf.Min(character.Education.gpa + hours * 0.05f, 4f);
        }

        /// <summary>
        /// Okulu bırak.
        /// </summary>
        public void DropOut(CharacterData character)
        {
            if (character?.Education == null) return;

            // Bir önceki seviyeye düş
            if (character.Education.CurrentLevel > EducationLevel.None)
            {
                character.Education.currentLevel = (EducationLevel)((int)character.Education.CurrentLevel - 1);
            }

            character.Education.schoolName = "";
            character.Education.isGraduated = true; // Bu seviyeyi tamamlamış sayılmaz

            character.Stats.ModifyStat(StatType.Happiness, Random.Range(-15, -5));

            Debug.Log($"[EducationManager] {character.FullName} dropped out of school");
        }

        #endregion

        #region Utility

        /// <summary>
        /// Mevcut üniversiteleri getir.
        /// </summary>
        public List<UniversityData> GetAvailableUniversities(CharacterData character)
        {
            if (character == null) return new List<UniversityData>();
            return DataManager.Instance.GetAvailableUniversities(character.Education.yksScore);
        }

        /// <summary>
        /// Eğitim seviyesi string getir.
        /// </summary>
        public string GetEducationLevelString(EducationLevel level)
        {
            return level switch
            {
                EducationLevel.None => "Eğitimsiz",
                EducationLevel.PrimarySchool => "İlkokul",
                EducationLevel.MiddleSchool => "Ortaokul",
                EducationLevel.HighSchool => "Lise",
                EducationLevel.University => "Üniversite",
                EducationLevel.Masters => "Yüksek Lisans",
                EducationLevel.Doctorate => "Doktora",
                _ => "Bilinmiyor"
            };
        }

        #endregion
    }
}
