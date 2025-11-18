using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TurkishLifeSim.Core;
using TurkishLifeSim.Character;

namespace TurkishLifeSim.Managers
{
    /// <summary>
    /// Yaşam Sistemi Yöneticisi - Tüm yaşam mekaniklerini yönetir.
    /// Eğitim, Kariyer, İlişki, Suç, Sağlık, Mülk, Askerlik sistemleri.
    /// </summary>
    public class LifeSystemManager : Singleton<LifeSystemManager>
    {
        #region Education System

        /// <summary>
        /// Okula başla.
        /// </summary>
        public void StartSchool(CharacterData character)
        {
            if (character.Age < 6) return;

            var education = character.Education;

            if (character.Age >= 6 && character.Age < 11)
            {
                education.currentLevel = EducationLevel.PrimarySchool;
                education.schoolName = GetRandomSchoolName("İlkokul");
            }
            else if (character.Age >= 11 && character.Age < 15)
            {
                education.currentLevel = EducationLevel.MiddleSchool;
                education.schoolName = GetRandomSchoolName("Ortaokul");
            }
            else if (character.Age >= 15 && character.Age < 19)
            {
                education.currentLevel = EducationLevel.HighSchool;
                education.schoolName = GetRandomSchoolName("Lise");
            }

            education.gpa = 0;
            education.isGraduated = false;

            EventBus.Publish(new EducationStartedEvent
            {
                Level = education.currentLevel,
                SchoolName = education.schoolName
            });
        }

        /// <summary>
        /// Ders çalış.
        /// </summary>
        public void Study(CharacterData character, int hours)
        {
            var education = character.Education;
            if (education.currentLevel == EducationLevel.None) return;

            // Zeka ve çalışma saatine göre not artışı
            float gpaIncrease = (character.Stats.Intelligence / 100f) * hours * 0.5f;
            education.gpa = Mathf.Clamp(education.gpa + gpaIncrease, 0, 4);

            // Mutluluk düşüşü
            character.Stats.ModifyStat(StatType.Happiness, -hours);

            // Zeka artışı
            if (UnityEngine.Random.value < 0.3f)
            {
                character.Stats.ModifyStat(StatType.Intelligence, 1);
            }
        }

        /// <summary>
        /// YKS sınavına gir.
        /// </summary>
        public int TakeYKSExam(CharacterData character)
        {
            var education = character.Education;
            if (education.currentLevel != EducationLevel.HighSchool) return 0;

            // Skor hesapla: Zeka + GPA + Rastgele faktör
            int baseScore = (int)(character.Stats.Intelligence * 4);
            int gpaBonus = (int)(education.gpa * 50);
            int randomFactor = UnityEngine.Random.Range(-50, 51);

            education.yksScore = Mathf.Clamp(baseScore + gpaBonus + randomFactor, 0, 500);

            EventBus.Publish(new ExamTakenEvent
            {
                ExamName = "YKS",
                Score = education.yksScore
            });

            return education.yksScore;
        }

        /// <summary>
        /// Üniversiteye başvur.
        /// </summary>
        public bool ApplyToUniversity(CharacterData character, string universityName, string department, int requiredScore)
        {
            var education = character.Education;

            if (education.yksScore >= requiredScore)
            {
                education.currentLevel = EducationLevel.University;
                education.universityName = universityName;
                education.department = department;
                education.gpa = 0;
                education.isGraduated = false;

                EventBus.Publish(new EducationStartedEvent
                {
                    Level = EducationLevel.University,
                    SchoolName = $"{universityName} - {department}"
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// Okuldan mezun ol.
        /// </summary>
        public void Graduate(CharacterData character)
        {
            var education = character.Education;
            education.isGraduated = true;

            EventBus.Publish(new GraduatedEvent
            {
                Level = education.currentLevel,
                GPA = education.gpa
            });

            // Başarı ekle
            string achievement = education.currentLevel switch
            {
                EducationLevel.PrimarySchool => "İlkokul Diploması",
                EducationLevel.MiddleSchool => "Ortaokul Diploması",
                EducationLevel.HighSchool => "Lise Diploması",
                EducationLevel.University => $"{education.department} Lisans Derecesi",
                EducationLevel.Masters => $"{education.department} Yüksek Lisans Derecesi",
                EducationLevel.Doctorate => $"{education.department} Doktora Derecesi",
                _ => "Diploma"
            };

            education.achievements.Add(achievement);
        }

        /// <summary>
        /// Okuldan atıl veya bırak.
        /// </summary>
        public void DropOutOfSchool(CharacterData character, bool expelled)
        {
            var education = character.Education;
            var previousLevel = education.currentLevel;

            education.currentLevel = education.currentLevel switch
            {
                EducationLevel.University => EducationLevel.HighSchool,
                EducationLevel.Masters => EducationLevel.University,
                EducationLevel.Doctorate => EducationLevel.Masters,
                _ => EducationLevel.None
            };

            education.schoolName = "";
            education.gpa = 0;

            if (expelled)
            {
                character.Stats.ModifyStat(StatType.Happiness, -20);
            }
        }

        private string GetRandomSchoolName(string type)
        {
            string[] names = type switch
            {
                "İlkokul" => new[] { "Atatürk İlkokulu", "Cumhuriyet İlkokulu", "Fatih İlkokulu", "Mehmet Akif Ersoy İlkokulu" },
                "Ortaokul" => new[] { "Atatürk Ortaokulu", "İnönü Ortaokulu", "Namık Kemal Ortaokulu", "Yunus Emre Ortaokulu" },
                "Lise" => new[] { "Anadolu Lisesi", "Fen Lisesi", "Galatasaray Lisesi", "İstanbul Lisesi", "Robert Koleji" },
                _ => new[] { "Okul" }
            };
            return names[UnityEngine.Random.Range(0, names.Length)];
        }

        #endregion

        #region Career System

        /// <summary>
        /// İş başvurusu yap.
        /// </summary>
        public bool ApplyForJob(CharacterData character, Job job)
        {
            if (character.Age < 16) return false;
            if (character.isInPrison) return false;

            // İşe alınma şansı hesapla
            float baseChance = 0.3f;

            // Eğitim bonusu
            baseChance += (int)character.Education.currentLevel * 0.1f;

            // Zeka bonusu
            baseChance += character.Stats.Intelligence / 200f;

            // Görünüş bonusu
            baseChance += character.Stats.Appearance / 300f;

            // Sabıka kaydı malus
            if (character.Crime.hasCriminalRecord)
            {
                baseChance -= 0.3f;
            }

            if (UnityEngine.Random.value < baseChance)
            {
                // Eski işi geçmişe ekle
                if (character.Career.currentJob != null)
                {
                    character.Career.jobHistory.Add(character.Career.currentJob);
                }

                character.Career.currentJob = job;
                character.Career.yearsInJob = 0;
                character.Career.performanceRating = 50;
                character.isEmployed = true;

                EventBus.Publish(new JobStartedEvent
                {
                    JobTitle = job.title,
                    Company = job.company,
                    Salary = job.baseSalary
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// İşten ayrıl.
        /// </summary>
        public void QuitJob(CharacterData character)
        {
            if (character.Career.currentJob == null) return;

            character.Career.jobHistory.Add(character.Career.currentJob);
            character.Career.currentJob = null;
            character.isEmployed = false;

            EventBus.Publish(new JobLostEvent
            {
                Reason = "İstifa"
            });
        }

        /// <summary>
        /// İşten çıkarıl.
        /// </summary>
        public void GetFired(CharacterData character, string reason)
        {
            if (character.Career.currentJob == null) return;

            character.Career.jobHistory.Add(character.Career.currentJob);
            character.Career.currentJob = null;
            character.isEmployed = false;

            character.Stats.ModifyStat(StatType.Happiness, -30);

            EventBus.Publish(new JobLostEvent
            {
                Reason = reason
            });
        }

        /// <summary>
        /// Terfi al.
        /// </summary>
        public bool GetPromotion(CharacterData character)
        {
            var job = character.Career.currentJob;
            if (job == null) return false;

            // Terfi şansı
            float promotionChance = character.Career.performanceRating / 200f +
                                   character.Career.yearsInJob * 0.05f;

            if (UnityEngine.Random.value < promotionChance)
            {
                decimal salaryIncrease = job.baseSalary * 0.15m;
                job.baseSalary += salaryIncrease;

                character.Stats.ModifyStat(StatType.Happiness, 15);

                EventBus.Publish(new PromotionEvent
                {
                    NewSalary = job.baseSalary
                });

                return true;
            }

            return false;
        }

        /// <summary>
        /// Maaş al.
        /// </summary>
        public void ReceiveSalary(CharacterData character)
        {
            var job = character.Career.currentJob;
            if (job == null) return;

            decimal monthlySalary = job.baseSalary / 12;
            character.Finances.ModifyMoney(monthlySalary, $"Maaş - {job.company}");

            // Performans güncellemesi
            if (UnityEngine.Random.value < 0.1f)
            {
                character.Career.performanceRating += UnityEngine.Random.Range(-5, 10);
                character.Career.performanceRating = Mathf.Clamp(character.Career.performanceRating, 0, 100);
            }
        }

        /// <summary>
        /// Emekli ol.
        /// </summary>
        public void Retire(CharacterData character)
        {
            if (character.Career.currentJob != null)
            {
                character.Career.jobHistory.Add(character.Career.currentJob);
                character.Career.currentJob = null;
            }

            character.isEmployed = false;
            character.isRetired = true;

            // Emekli maaşı hesapla
            decimal pension = character.Finances.totalEarned * 0.001m;
            pension = Math.Max(pension, 5000);

            EventBus.Publish(new RetiredEvent
            {
                MonthlyPension = pension
            });
        }

        /// <summary>
        /// Serbest meslek başlat.
        /// </summary>
        public bool StartFreelancing(CharacterData character, string field)
        {
            if (character.Age < 18) return false;

            var job = new Job
            {
                id = Guid.NewGuid().ToString(),
                title = $"Freelance {field}",
                company = "Serbest",
                category = field,
                baseSalary = UnityEngine.Random.Range(20000, 60000),
                yearsWorked = 0
            };

            return ApplyForJob(character, job);
        }

        #endregion

        #region Relationship System

        /// <summary>
        /// Yeni biriyle tanış.
        /// </summary>
        public Relationship MeetNewPerson(CharacterData character, RelationType type)
        {
            Gender gender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
            int age = character.Age + UnityEngine.Random.Range(-10, 11);
            age = Mathf.Max(1, age);

            var newPerson = CharacterFactory.CreateNPC(type, gender, age);
            character.Relationships.Add(newPerson);

            EventBus.Publish(new RelationshipCreatedEvent
            {
                Name = newPerson.npcName,
                Type = type
            });

            return newPerson;
        }

        /// <summary>
        /// İlişkiyi geliştir.
        /// </summary>
        public void ImproveRelationship(CharacterData character, string npcId, int amount)
        {
            var relationship = character.Relationships.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null) return;

            relationship.intimacy = Mathf.Clamp(relationship.intimacy + amount, 0, 100);
            relationship.trust = Mathf.Clamp(relationship.trust + amount / 2, 0, 100);

            // İlişki türü yükseltme
            if (relationship.intimacy > 80 && relationship.type == RelationType.Friend)
            {
                relationship.type = RelationType.BestFriend;
            }

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = npcId,
                NewIntimacy = relationship.intimacy,
                NewTrust = relationship.trust
            });
        }

        /// <summary>
        /// İlişkiyi zayıflat.
        /// </summary>
        public void DamageRelationship(CharacterData character, string npcId, int amount)
        {
            var relationship = character.Relationships.FirstOrDefault(r => r.npcId == npcId);
            if (relationship == null) return;

            relationship.intimacy = Mathf.Clamp(relationship.intimacy - amount, 0, 100);
            relationship.trust = Mathf.Clamp(relationship.trust - amount, 0, 100);

            // İlişki kopması
            if (relationship.intimacy < 10 || relationship.trust < 10)
            {
                relationship.status = RelationshipStatus.Broken;
            }

            EventBus.Publish(new RelationshipChangedEvent
            {
                NpcId = npcId,
                NewIntimacy = relationship.intimacy,
                NewTrust = relationship.trust
            });
        }

        /// <summary>
        /// Evlilik teklifi yap.
        /// </summary>
        public bool ProposeMarriage(CharacterData character, string npcId)
        {
            if (character.Age < 18) return false;
            if (character.isMarried) return false;

            var partner = character.Relationships.FirstOrDefault(r => r.npcId == npcId);
            if (partner == null) return false;

            if (partner.type != RelationType.Boyfriend && partner.type != RelationType.Girlfriend)
                return false;

            // Kabul şansı
            float acceptChance = (partner.intimacy + partner.trust) / 200f;
            acceptChance += character.Stats.Appearance / 200f;

            if (UnityEngine.Random.value < acceptChance)
            {
                partner.type = RelationType.Spouse;
                character.isMarried = true;

                character.Stats.ModifyStat(StatType.Happiness, 30);

                EventBus.Publish(new MarriedEvent
                {
                    SpouseName = partner.npcName
                });

                return true;
            }

            character.Stats.ModifyStat(StatType.Happiness, -20);
            return false;
        }

        /// <summary>
        /// Boşan.
        /// </summary>
        public void Divorce(CharacterData character, string spouseId)
        {
            var spouse = character.Relationships.FirstOrDefault(r => r.npcId == spouseId);
            if (spouse == null || spouse.type != RelationType.Spouse) return;

            spouse.type = RelationType.ExSpouse;
            spouse.status = RelationshipStatus.Distant;
            character.isMarried = false;

            // Mal paylaşımı
            decimal divorce_settlement = character.Finances.CurrentMoney * 0.4m;
            character.Finances.ModifyMoney(-divorce_settlement, "Boşanma nafakası");

            character.Stats.ModifyStat(StatType.Happiness, -40);

            EventBus.Publish(new DivorcedEvent
            {
                ExSpouseName = spouse.npcName,
                SettlementAmount = divorce_settlement
            });
        }

        /// <summary>
        /// Çocuk sahibi ol.
        /// </summary>
        public Relationship HaveChild(CharacterData character)
        {
            if (!character.isMarried && UnityEngine.Random.value > 0.3f) return null;
            if (character.Gender == Gender.Male && character.Age > 70) return null;
            if (character.Gender == Gender.Female && character.Age > 45) return null;

            // Doğurganlık kontrolü
            if (UnityEngine.Random.Range(0, 100) > character.Health.fertilityStat)
            {
                return null;
            }

            Gender childGender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
            string childName = childGender == Gender.Male
                ? DataManager.Instance.GetRandomMaleName()
                : DataManager.Instance.GetRandomFemaleName();

            var child = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{childName} {character.LastName}",
                type = RelationType.Child,
                gender = childGender,
                age = 0,
                intimacy = 100,
                trust = 100,
                status = RelationshipStatus.Active
            };

            character.Relationships.Add(child);

            character.Stats.ModifyStat(StatType.Happiness, 25);

            EventBus.Publish(new ChildBornEvent
            {
                ChildName = child.npcName,
                Gender = childGender
            });

            return child;
        }

        #endregion

        #region Crime System

        /// <summary>
        /// Suç işle.
        /// </summary>
        public bool CommitCrime(CharacterData character, CrimeType crimeType)
        {
            if (character.isInPrison) return false;

            var crime = new Crime
            {
                id = Guid.NewGuid().ToString(),
                name = GetCrimeName(crimeType),
                type = crimeType,
                yearCommitted = DateTime.Now.Year,
                wasCaught = false,
                sentenceYears = 0,
                fine = 0,
                wasConvicted = false
            };

            // Yakalanma şansı
            float catchChance = GetCatchChance(crimeType);

            // Zeka düşürür yakalanma şansını
            catchChance -= character.Stats.Intelligence / 200f;

            if (UnityEngine.Random.value < catchChance)
            {
                crime.wasCaught = true;
                crime.wasConvicted = true;
                crime.sentenceYears = GetSentence(crimeType);
                crime.fine = GetFine(crimeType);

                character.Crime.crimeHistory.Add(crime);
                character.Crime.hasCriminalRecord = true;

                // Cezayı uygula
                if (crime.sentenceYears > 0)
                {
                    character.isInPrison = true;
                    character.Crime.prisonYearsRemaining = crime.sentenceYears;

                    if (character.isEmployed)
                    {
                        GetFired(character, "Tutuklanma");
                    }
                }

                character.Finances.ModifyMoney(-crime.fine, $"Para cezası - {crime.name}");
                character.Stats.ModifyStat(StatType.Happiness, -30);

                EventBus.Publish(new ArrestedEvent
                {
                    CrimeName = crime.name,
                    SentenceYears = crime.sentenceYears,
                    Fine = crime.fine
                });

                return false;
            }

            character.Crime.crimeHistory.Add(crime);

            // Suçtan kazanç
            decimal earnings = GetCrimeEarnings(crimeType);
            if (earnings > 0)
            {
                character.Finances.ModifyMoney(earnings, $"Yasadışı kazanç - {crime.name}");
            }

            return true;
        }

        /// <summary>
        /// Hapisten kaçmayı dene.
        /// </summary>
        public bool AttemptPrisonEscape(CharacterData character)
        {
            if (!character.isInPrison) return false;

            character.Crime.escapeAttempts++;

            // Kaçış şansı
            float escapeChance = 0.1f + character.Stats.Intelligence / 500f;

            if (UnityEngine.Random.value < escapeChance)
            {
                character.isInPrison = false;
                character.Crime.hasEscaped = true;
                character.Crime.prisonYearsRemaining = 0;

                EventBus.Publish(new PrisonEscapeEvent
                {
                    Success = true
                });

                return true;
            }

            // Başarısız - ceza artışı
            character.Crime.prisonYearsRemaining += 2;
            character.Stats.ModifyStat(StatType.Health, -10);

            EventBus.Publish(new PrisonEscapeEvent
            {
                Success = false,
                AdditionalYears = 2
            });

            return false;
        }

        /// <summary>
        /// Hapiste yıl geçir.
        /// </summary>
        public void ServePrisonTime(CharacterData character)
        {
            if (!character.isInPrison) return;

            character.Crime.prisonYearsRemaining--;
            character.Crime.totalPrisonTime++;

            // Hapiste stat etkileri
            character.Stats.ModifyStat(StatType.Happiness, -5);
            character.Stats.ModifyStat(StatType.Health, -2);

            // Hapiste olaylar
            if (UnityEngine.Random.value < 0.2f)
            {
                // Kavga
                character.Stats.ModifyStat(StatType.Health, -10);
                character.Crime.prisonReputation += 5;
            }

            if (character.Crime.prisonYearsRemaining <= 0)
            {
                character.isInPrison = false;
                character.Crime.isOnProbation = true;
                character.Crime.probationYearsRemaining = 2;

                EventBus.Publish(new ReleasedFromPrisonEvent());
            }
        }

        private string GetCrimeName(CrimeType type) => type switch
        {
            CrimeType.Theft => "Hırsızlık",
            CrimeType.Robbery => "Gasp",
            CrimeType.Burglary => "Ev soygunu",
            CrimeType.Assault => "Saldırı",
            CrimeType.Murder => "Cinayet",
            CrimeType.Fraud => "Dolandırıcılık",
            CrimeType.DrugPossession => "Uyuşturucu bulundurma",
            CrimeType.DrugTrafficking => "Uyuşturucu ticareti",
            CrimeType.DUI => "Alkollü araç kullanma",
            CrimeType.Vandalism => "Mala zarar verme",
            CrimeType.Shoplifting => "Mağaza hırsızlığı",
            CrimeType.TaxEvasion => "Vergi kaçakçılığı",
            CrimeType.Bribery => "Rüşvet",
            CrimeType.Extortion => "Şantaj",
            CrimeType.Kidnapping => "Adam kaçırma",
            CrimeType.Arson => "Kundakçılık",
            _ => "Suç"
        };

        private float GetCatchChance(CrimeType type) => type switch
        {
            CrimeType.Murder => 0.7f,
            CrimeType.Kidnapping => 0.6f,
            CrimeType.Robbery => 0.5f,
            CrimeType.Burglary => 0.4f,
            CrimeType.DrugTrafficking => 0.4f,
            CrimeType.Arson => 0.5f,
            CrimeType.Assault => 0.4f,
            CrimeType.Fraud => 0.3f,
            CrimeType.TaxEvasion => 0.3f,
            CrimeType.Shoplifting => 0.3f,
            _ => 0.35f
        };

        private int GetSentence(CrimeType type) => type switch
        {
            CrimeType.Murder => UnityEngine.Random.Range(15, 30),
            CrimeType.Kidnapping => UnityEngine.Random.Range(8, 15),
            CrimeType.DrugTrafficking => UnityEngine.Random.Range(5, 12),
            CrimeType.Robbery => UnityEngine.Random.Range(3, 8),
            CrimeType.Arson => UnityEngine.Random.Range(3, 7),
            CrimeType.Burglary => UnityEngine.Random.Range(2, 5),
            CrimeType.Assault => UnityEngine.Random.Range(1, 4),
            CrimeType.Fraud => UnityEngine.Random.Range(1, 5),
            CrimeType.DrugPossession => UnityEngine.Random.Range(0, 2),
            CrimeType.DUI => 0,
            CrimeType.Shoplifting => 0,
            CrimeType.Vandalism => 0,
            _ => UnityEngine.Random.Range(0, 3)
        };

        private decimal GetFine(CrimeType type) => type switch
        {
            CrimeType.Murder => 0,
            CrimeType.TaxEvasion => UnityEngine.Random.Range(50000, 200000),
            CrimeType.Fraud => UnityEngine.Random.Range(20000, 100000),
            CrimeType.DUI => UnityEngine.Random.Range(5000, 15000),
            CrimeType.Vandalism => UnityEngine.Random.Range(1000, 10000),
            CrimeType.Shoplifting => UnityEngine.Random.Range(500, 5000),
            _ => UnityEngine.Random.Range(1000, 20000)
        };

        private decimal GetCrimeEarnings(CrimeType type) => type switch
        {
            CrimeType.Robbery => UnityEngine.Random.Range(5000, 50000),
            CrimeType.Burglary => UnityEngine.Random.Range(2000, 30000),
            CrimeType.DrugTrafficking => UnityEngine.Random.Range(10000, 100000),
            CrimeType.Theft => UnityEngine.Random.Range(500, 5000),
            CrimeType.Shoplifting => UnityEngine.Random.Range(100, 1000),
            CrimeType.Fraud => UnityEngine.Random.Range(5000, 50000),
            CrimeType.Extortion => UnityEngine.Random.Range(5000, 30000),
            _ => 0
        };

        #endregion

        #region Health System

        /// <summary>
        /// Doktora git.
        /// </summary>
        public void VisitDoctor(CharacterData character)
        {
            decimal cost = character.Health.hasInsurance ? 50 : 500;
            character.Finances.ModifyMoney(-cost, "Doktor ziyareti");
            character.Health.doctorVisitsThisYear++;

            // Hastalık tespiti
            if (UnityEngine.Random.value < 0.2f)
            {
                var disease = GenerateRandomDisease(character);
                character.Health.diseases.Add(disease);

                EventBus.Publish(new DiseaseDiagnosedEvent
                {
                    DiseaseName = disease.name,
                    Severity = disease.severity
                });
            }

            // Sağlık iyileşmesi
            character.Stats.ModifyStat(StatType.Health, 5);
        }

        /// <summary>
        /// Hastalığı tedavi et.
        /// </summary>
        public bool TreatDisease(CharacterData character, string diseaseId)
        {
            var disease = character.Health.diseases.FirstOrDefault(d => d.id == diseaseId);
            if (disease == null) return false;

            decimal cost = disease.treatmentCost;
            if (character.Health.hasInsurance)
            {
                cost *= character.Health.insuranceType == InsuranceType.Premium ? 0.1m : 0.5m;
            }

            if (character.Finances.CurrentMoney < cost) return false;

            character.Finances.ModifyMoney(-cost, $"Tedavi - {disease.name}");

            // Tedavi başarısı
            float successChance = disease.severity switch
            {
                DiseaseSeverity.Mild => 0.95f,
                DiseaseSeverity.Moderate => 0.8f,
                DiseaseSeverity.Severe => 0.6f,
                DiseaseSeverity.Critical => 0.4f,
                DiseaseSeverity.Terminal => 0.1f,
                _ => 0.5f
            };

            if (UnityEngine.Random.value < successChance)
            {
                character.Health.diseases.Remove(disease);
                character.Stats.ModifyStat(StatType.Health, 20);

                EventBus.Publish(new DiseaseCuredEvent
                {
                    DiseaseName = disease.name
                });

                return true;
            }

            disease.isBeingTreated = true;
            return false;
        }

        /// <summary>
        /// Sigorta satın al.
        /// </summary>
        public void PurchaseInsurance(CharacterData character, InsuranceType type)
        {
            decimal monthlyCost = type switch
            {
                InsuranceType.SGK => 500,
                InsuranceType.Private => 1500,
                InsuranceType.Premium => 5000,
                _ => 0
            };

            character.Health.hasInsurance = true;
            character.Health.insuranceType = type;
        }

        /// <summary>
        /// Spor yap.
        /// </summary>
        public void Exercise(CharacterData character, int hours)
        {
            character.Stats.ModifyStat(StatType.Health, hours * 2);
            character.Stats.ModifyStat(StatType.Happiness, hours);
            character.Stats.ModifyStat(StatType.Appearance, UnityEngine.Random.Range(0, 2));

            // Sakatlanma riski
            if (UnityEngine.Random.value < 0.05f * hours)
            {
                character.Stats.ModifyStat(StatType.Health, -15);
            }
        }

        /// <summary>
        /// Meditasyon yap.
        /// </summary>
        public void Meditate(CharacterData character)
        {
            character.Health.mentalHealth = Mathf.Clamp(character.Health.mentalHealth + 10, 0, 100);
            character.Stats.ModifyStat(StatType.Happiness, 5);
        }

        /// <summary>
        /// Sigara içmeye başla/bırak.
        /// </summary>
        public void ToggleSmoking(CharacterData character)
        {
            character.Health.isSmoking = !character.Health.isSmoking;

            if (character.Health.isSmoking)
            {
                character.isAddicted = true;
            }
        }

        /// <summary>
        /// Ameliyat ol.
        /// </summary>
        public bool HaveSurgery(CharacterData character, string surgeryName, decimal cost)
        {
            if (character.Health.hasInsurance)
            {
                cost *= 0.3m;
            }

            if (character.Finances.CurrentMoney < cost) return false;

            character.Finances.ModifyMoney(-cost, $"Ameliyat - {surgeryName}");

            bool success = UnityEngine.Random.value < 0.85f;

            var surgery = new Surgery
            {
                name = surgeryName,
                year = DateTime.Now.Year,
                wasSuccessful = success,
                cost = (float)cost
            };

            character.Health.surgeries.Add(surgery);

            if (success)
            {
                character.Stats.ModifyStat(StatType.Health, 30);
            }
            else
            {
                character.Stats.ModifyStat(StatType.Health, -20);
            }

            return success;
        }

        private Disease GenerateRandomDisease(CharacterData character)
        {
            string[] mildDiseases = { "Grip", "Soğuk algınlığı", "Migren", "Alerjik reaksiyon" };
            string[] moderateDiseases = { "Bronşit", "Gastrit", "Hipertansiyon", "Diyabet" };
            string[] severeDiseases = { "Zatürre", "Hepatit", "Kalp hastalığı", "Böbrek yetmezliği" };

            DiseaseSeverity severity;
            string name;

            float roll = UnityEngine.Random.value;
            if (roll < 0.7f)
            {
                severity = DiseaseSeverity.Mild;
                name = mildDiseases[UnityEngine.Random.Range(0, mildDiseases.Length)];
            }
            else if (roll < 0.95f)
            {
                severity = DiseaseSeverity.Moderate;
                name = moderateDiseases[UnityEngine.Random.Range(0, moderateDiseases.Length)];
            }
            else
            {
                severity = DiseaseSeverity.Severe;
                name = severeDiseases[UnityEngine.Random.Range(0, severeDiseases.Length)];
            }

            return new Disease
            {
                id = Guid.NewGuid().ToString(),
                name = name,
                severity = severity,
                isChronic = severity >= DiseaseSeverity.Moderate && UnityEngine.Random.value < 0.3f,
                isTreatable = true,
                treatmentCost = severity switch
                {
                    DiseaseSeverity.Mild => UnityEngine.Random.Range(500, 2000),
                    DiseaseSeverity.Moderate => UnityEngine.Random.Range(5000, 20000),
                    DiseaseSeverity.Severe => UnityEngine.Random.Range(20000, 100000),
                    _ => 1000
                },
                yearDiagnosed = DateTime.Now.Year,
                isBeingTreated = false
            };
        }

        #endregion

        #region Property System

        /// <summary>
        /// Ev satın al.
        /// </summary>
        public bool BuyProperty(CharacterData character, Property property)
        {
            if (character.Finances.CurrentMoney < property.purchasePrice) return false;

            character.Finances.ModifyMoney(-property.purchasePrice, $"Mülk alımı - {property.name}");
            property.yearPurchased = DateTime.Now.Year;
            character.Properties.ownedProperties.Add(property);

            if (character.Properties.currentResidence == null)
            {
                character.Properties.currentResidence = property;
                character.Properties.isRenting = false;
            }

            EventBus.Publish(new PropertyPurchasedEvent
            {
                PropertyName = property.name,
                Price = property.purchasePrice
            });

            return true;
        }

        /// <summary>
        /// Ev sat.
        /// </summary>
        public bool SellProperty(CharacterData character, string propertyId)
        {
            var property = character.Properties.ownedProperties.FirstOrDefault(p => p.id == propertyId);
            if (property == null) return false;

            character.Finances.ModifyMoney(property.currentValue, $"Mülk satışı - {property.name}");
            character.Properties.ownedProperties.Remove(property);

            if (character.Properties.currentResidence?.id == propertyId)
            {
                character.Properties.currentResidence = null;
                character.isHomeless = true;
            }

            return true;
        }

        /// <summary>
        /// Araç satın al.
        /// </summary>
        public bool BuyVehicle(CharacterData character, Vehicle vehicle)
        {
            if (character.Finances.CurrentMoney < vehicle.purchasePrice) return false;

            character.Finances.ModifyMoney(-vehicle.purchasePrice, $"Araç alımı - {vehicle.brand} {vehicle.model}");
            character.Properties.ownedVehicles.Add(vehicle);

            EventBus.Publish(new VehiclePurchasedEvent
            {
                VehicleName = $"{vehicle.brand} {vehicle.model}",
                Price = vehicle.purchasePrice
            });

            return true;
        }

        /// <summary>
        /// Araç sat.
        /// </summary>
        public bool SellVehicle(CharacterData character, string vehicleId)
        {
            var vehicle = character.Properties.ownedVehicles.FirstOrDefault(v => v.id == vehicleId);
            if (vehicle == null) return false;

            character.Finances.ModifyMoney(vehicle.currentValue, $"Araç satışı - {vehicle.brand} {vehicle.model}");
            character.Properties.ownedVehicles.Remove(vehicle);

            return true;
        }

        /// <summary>
        /// Kira öde.
        /// </summary>
        public void PayRent(CharacterData character)
        {
            if (!character.Properties.isRenting) return;

            character.Finances.ModifyMoney(-character.Properties.monthlyRent, "Kira ödemesi");
        }

        /// <summary>
        /// Kiralık eve taşın.
        /// </summary>
        public void RentProperty(CharacterData character, decimal monthlyRent)
        {
            character.Properties.isRenting = true;
            character.Properties.monthlyRent = monthlyRent;
            character.isHomeless = false;
        }

        #endregion

        #region Military System

        /// <summary>
        /// Askere git.
        /// </summary>
        public void StartMilitaryService(CharacterData character)
        {
            if (character.Gender != Gender.Male) return;
            if (character.Age < 20 || character.Age > 41) return;
            if (character.Military.status != MilitaryStatus.NotServed) return;

            character.Military.status = MilitaryStatus.Serving;
            character.Military.branch = (MilitaryBranch)UnityEngine.Random.Range(1, 5);
            character.Military.rank = MilitaryRank.Private;
            character.Military.serviceMonthsRemaining = character.Education.currentLevel >= EducationLevel.University ? 6 : 12;

            if (character.isEmployed)
            {
                QuitJob(character);
            }

            EventBus.Publish(new MilitaryServiceStartedEvent
            {
                Branch = character.Military.branch,
                DurationMonths = character.Military.serviceMonthsRemaining
            });
        }

        /// <summary>
        /// Askerlik süresini geçir.
        /// </summary>
        public void ServeMilitaryTime(CharacterData character)
        {
            if (character.Military.status != MilitaryStatus.Serving) return;

            character.Military.serviceMonthsRemaining -= 12; // Yıllık
            character.Military.totalServiceMonths += 12;

            // Askerlikte stat etkileri
            character.Stats.ModifyStat(StatType.Health, 5);
            character.Stats.ModifyStat(StatType.Happiness, -5);

            // Terfi şansı
            if (UnityEngine.Random.value < 0.2f)
            {
                character.Military.rank = (MilitaryRank)Mathf.Min((int)character.Military.rank + 1, (int)MilitaryRank.Sergeant);
            }

            if (character.Military.serviceMonthsRemaining <= 0)
            {
                character.Military.status = MilitaryStatus.Completed;
                character.hasCompletedMilitary = true;

                EventBus.Publish(new MilitaryServiceCompletedEvent
                {
                    FinalRank = character.Military.rank
                });
            }
        }

        /// <summary>
        /// Bedelli askerlik yap.
        /// </summary>
        public bool PayForMilitaryExemption(CharacterData character, decimal amount)
        {
            if (character.Finances.CurrentMoney < amount) return false;

            character.Finances.ModifyMoney(-amount, "Bedelli askerlik");
            character.Military.status = MilitaryStatus.Exempted;
            character.Military.paidExemption = true;
            character.hasCompletedMilitary = true;

            return true;
        }

        #endregion

        #region Social Media & Fame

        /// <summary>
        /// Sosyal medya hesabı oluştur.
        /// </summary>
        public void CreateSocialMediaAccount(CharacterData character, string platform)
        {
            var account = new SocialMediaAccount
            {
                platform = platform,
                username = $"{character.FirstName.ToLower()}_{UnityEngine.Random.Range(100, 9999)}",
                followers = 0,
                posts = 0,
                engagementRate = 0.05f,
                isVerified = false
            };

            character.SocialMedia.accounts.Add(account);
        }

        /// <summary>
        /// İçerik paylaş.
        /// </summary>
        public void PostContent(CharacterData character, string platform)
        {
            var account = character.SocialMedia.accounts.FirstOrDefault(a => a.platform == platform);
            if (account == null) return;

            account.posts++;

            // Takipçi kazanımı
            int newFollowers = (int)(account.engagementRate * account.followers * 0.1f) +
                              UnityEngine.Random.Range(0, 100);

            // Görünüş ve zeka etkisi
            newFollowers += character.Stats.Appearance / 10;
            newFollowers += character.Stats.Intelligence / 20;

            account.followers += newFollowers;
            character.SocialMedia.totalFollowers = character.SocialMedia.accounts.Sum(a => a.followers);

            // Fenomen olma kontrolü
            if (character.SocialMedia.totalFollowers >= 100000 && !character.SocialMedia.isInfluencer)
            {
                character.SocialMedia.isInfluencer = true;
                character.Stats.ModifyStat(StatType.Fame, 20);
            }

            // Doğrulama rozeti
            if (character.SocialMedia.totalFollowers >= 50000)
            {
                account.isVerified = true;
            }
        }

        /// <summary>
        /// Sosyal medyadan para kazan.
        /// </summary>
        public void EarnFromSocialMedia(CharacterData character)
        {
            if (!character.SocialMedia.isInfluencer) return;

            decimal earnings = character.SocialMedia.totalFollowers * 0.01m;
            character.SocialMedia.monthlyEarnings = earnings;
            character.Finances.ModifyMoney(earnings, "Sosyal medya geliri");
        }

        #endregion

        #region Hobbies

        /// <summary>
        /// Hobi başlat.
        /// </summary>
        public void StartHobby(CharacterData character, string hobbyName, HobbyCategory category)
        {
            var hobby = new HobbyData
            {
                id = Guid.NewGuid().ToString(),
                name = hobbyName,
                category = category,
                skillLevel = 0,
                yearsActive = 0,
                totalInvestment = 0,
                achievements = new List<string>()
            };

            character.Hobbies.Add(hobby);
            character.Stats.ModifyStat(StatType.Happiness, 5);
        }

        /// <summary>
        /// Hobiyle uğraş.
        /// </summary>
        public void PracticeHobby(CharacterData character, string hobbyId, int hours)
        {
            var hobby = character.Hobbies.FirstOrDefault(h => h.id == hobbyId);
            if (hobby == null) return;

            // Beceri artışı
            int skillGain = hours + character.Stats.Intelligence / 50;
            hobby.skillLevel = Mathf.Clamp(hobby.skillLevel + skillGain, 0, 100);

            character.Stats.ModifyStat(StatType.Happiness, hours);

            // Başarı kontrolü
            if (hobby.skillLevel >= 50 && !hobby.achievements.Contains("Orta Seviye"))
            {
                hobby.achievements.Add("Orta Seviye");
            }
            if (hobby.skillLevel >= 80 && !hobby.achievements.Contains("İleri Seviye"))
            {
                hobby.achievements.Add("İleri Seviye");
            }
            if (hobby.skillLevel >= 100 && !hobby.achievements.Contains("Uzman"))
            {
                hobby.achievements.Add("Uzman");
                character.Stats.ModifyStat(StatType.Fame, 5);
            }
        }

        #endregion

        #region Lottery & Gambling

        /// <summary>
        /// Piyango bileti al.
        /// </summary>
        public decimal PlayLottery(CharacterData character, decimal ticketPrice)
        {
            if (character.Finances.CurrentMoney < ticketPrice) return 0;

            character.Finances.ModifyMoney(-ticketPrice, "Piyango bileti");

            // Kazanma şansı
            float roll = UnityEngine.Random.value;

            if (roll < 0.0001f) // Büyük ikramiye
            {
                decimal prize = ticketPrice * 100000;
                character.Finances.ModifyMoney(prize, "Piyango - Büyük İkramiye!");
                character.Stats.ModifyStat(StatType.Happiness, 50);
                character.Stats.ModifyStat(StatType.Fame, 20);
                return prize;
            }
            else if (roll < 0.001f) // Orta ödül
            {
                decimal prize = ticketPrice * 1000;
                character.Finances.ModifyMoney(prize, "Piyango kazancı");
                character.Stats.ModifyStat(StatType.Happiness, 20);
                return prize;
            }
            else if (roll < 0.01f) // Küçük ödül
            {
                decimal prize = ticketPrice * 10;
                character.Finances.ModifyMoney(prize, "Piyango kazancı");
                character.Stats.ModifyStat(StatType.Happiness, 5);
                return prize;
            }

            return 0;
        }

        /// <summary>
        /// Kumar oyna.
        /// </summary>
        public decimal Gamble(CharacterData character, decimal amount)
        {
            if (character.Finances.CurrentMoney < amount) return 0;

            character.Finances.ModifyMoney(-amount, "Kumar");

            if (UnityEngine.Random.value < 0.45f) // %45 kazanma şansı
            {
                decimal winnings = amount * 2;
                character.Finances.ModifyMoney(winnings, "Kumar kazancı");
                character.Stats.ModifyStat(StatType.Happiness, 10);
                return winnings;
            }

            character.Stats.ModifyStat(StatType.Happiness, -5);
            return 0;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Yıllık olayları işle.
        /// </summary>
        public void ProcessYearlyEvents(CharacterData character)
        {
            // Maaş
            if (character.isEmployed)
            {
                for (int i = 0; i < 12; i++)
                {
                    ReceiveSalary(character);
                }
                character.Career.yearsInJob++;
            }

            // Kira
            if (character.Properties.isRenting)
            {
                for (int i = 0; i < 12; i++)
                {
                    PayRent(character);
                }
            }

            // İlişki yaşlanması
            foreach (var relationship in character.Relationships)
            {
                relationship.age++;
            }

            // Hastalık etkisi
            foreach (var disease in character.Health.diseases)
            {
                if (!disease.isBeingTreated)
                {
                    int healthLoss = disease.severity switch
                    {
                        DiseaseSeverity.Mild => 1,
                        DiseaseSeverity.Moderate => 3,
                        DiseaseSeverity.Severe => 5,
                        DiseaseSeverity.Critical => 10,
                        DiseaseSeverity.Terminal => 20,
                        _ => 0
                    };
                    character.Stats.ModifyStat(StatType.Health, -healthLoss);
                }
            }

            // Hapis süresi
            if (character.isInPrison)
            {
                ServePrisonTime(character);
            }

            // Askerlik süresi
            if (character.Military.status == MilitaryStatus.Serving)
            {
                ServeMilitaryTime(character);
            }

            // Hobi süresi
            foreach (var hobby in character.Hobbies)
            {
                hobby.yearsActive++;
            }

            // Sosyal medya geliri
            EarnFromSocialMedia(character);

            // Araç değer kaybı
            foreach (var vehicle in character.Properties.ownedVehicles)
            {
                vehicle.currentValue *= 0.9m;
                vehicle.mileage += UnityEngine.Random.Range(5000, 20000);
            }

            // Mülk değer değişimi
            foreach (var property in character.Properties.ownedProperties)
            {
                float valueChange = UnityEngine.Random.Range(-0.05f, 0.1f);
                property.currentValue *= (1 + (decimal)valueChange);
            }
        }

        #endregion
    }

    #region Events

    // Eğitim Events
    public class EducationStartedEvent
    {
        public EducationLevel Level;
        public string SchoolName;
    }

    public class ExamTakenEvent
    {
        public string ExamName;
        public int Score;
    }

    public class GraduatedEvent
    {
        public EducationLevel Level;
        public float GPA;
    }

    // Kariyer Events
    public class JobStartedEvent
    {
        public string JobTitle;
        public string Company;
        public decimal Salary;
    }

    public class JobLostEvent
    {
        public string Reason;
    }

    public class PromotionEvent
    {
        public decimal NewSalary;
    }

    public class RetiredEvent
    {
        public decimal MonthlyPension;
    }

    // İlişki Events
    public class RelationshipCreatedEvent
    {
        public string Name;
        public RelationType Type;
    }

    public class RelationshipChangedEvent
    {
        public string NpcId;
        public int NewIntimacy;
        public int NewTrust;
    }

    public class MarriedEvent
    {
        public string SpouseName;
    }

    public class DivorcedEvent
    {
        public string ExSpouseName;
        public decimal SettlementAmount;
    }

    public class ChildBornEvent
    {
        public string ChildName;
        public Gender Gender;
    }

    // Suç Events
    public class ArrestedEvent
    {
        public string CrimeName;
        public int SentenceYears;
        public decimal Fine;
    }

    public class PrisonEscapeEvent
    {
        public bool Success;
        public int AdditionalYears;
    }

    public class ReleasedFromPrisonEvent { }

    // Sağlık Events
    public class DiseaseDiagnosedEvent
    {
        public string DiseaseName;
        public DiseaseSeverity Severity;
    }

    public class DiseaseCuredEvent
    {
        public string DiseaseName;
    }

    // Mülk Events
    public class PropertyPurchasedEvent
    {
        public string PropertyName;
        public decimal Price;
    }

    public class VehiclePurchasedEvent
    {
        public string VehicleName;
        public decimal Price;
    }

    // Askerlik Events
    public class MilitaryServiceStartedEvent
    {
        public MilitaryBranch Branch;
        public int DurationMonths;
    }

    public class MilitaryServiceCompletedEvent
    {
        public MilitaryRank FinalRank;
    }

    #endregion
}
