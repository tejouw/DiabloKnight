using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Events
{
    /// <summary>
    /// Varsayılan olaylar - JSON dosyası olmadığında kullanılır.
    /// </summary>
    public static class DefaultEvents
    {
        public static List<GameEvent> GetAllEvents()
        {
            var events = new List<GameEvent>();

            // Bebek dönem olayları (0-4)
            events.AddRange(GetBabyEvents());

            // Çocukluk olayları (5-11)
            events.AddRange(GetChildhoodEvents());

            // Ergenlik olayları (12-17)
            events.AddRange(GetTeenEvents());

            // Genç yetişkin olayları (18-29)
            events.AddRange(GetYoungAdultEvents());

            // Yetişkin olayları (30-59)
            events.AddRange(GetAdultEvents());

            // Yaşlılık olayları (60+)
            events.AddRange(GetSeniorEvents());

            // Genel olaylar (her yaş)
            events.AddRange(GetGeneralEvents());

            // Suç olayları
            events.AddRange(GetCrimeEvents());

            // Hapishane olayları
            events.AddRange(GetPrisonEvents());

            // Üniversite olayları
            events.AddRange(GetUniversityEvents());

            return events;
        }

        private static List<GameEvent> GetBabyEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "baby_first_steps",
                    title = "İlk Adımlar",
                    description = "Bugün ilk adımlarını attın! Ailen çok mutlu.",
                    ageRange = new AgeRange { min = 1, max = 2 },
                    category = EventCategory.Milestone,
                    probability = 0.8f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yürümeye devam et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Sağlığın gelişti!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "baby_sick",
                    title = "Hastalık",
                    description = "Hafif bir soğuk algınlığına yakalandın.",
                    ageRange = new AgeRange { min = 0, max = 4 },
                    category = EventCategory.Health,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Dinlen",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Birkaç gün sonra iyileştin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "baby_first_word",
                    title = "İlk Kelime",
                    description = "İlk kelimeni söyledin!",
                    ageRange = new AgeRange { min = 1, max = 3 },
                    category = EventCategory.Milestone,
                    probability = 0.7f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Anne",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Annen çok mutlu oldu!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Baba",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Baban çok mutlu oldu!"
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetChildhoodEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "child_school_start",
                    title = "Okula Başlama",
                    description = "Bugün ilkokula başlıyorsun! Heyecanlı mısın?",
                    ageRange = new AgeRange { min = 6, max = 6 },
                    category = EventCategory.Milestone,
                    probability = 1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Çok heyecanlıyım!",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Okula güzel bir başlangıç yaptın!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Korkuyorum",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.6f,
                                    resultText = "İlk gün biraz zor geçti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 0.4f,
                                    resultText = "Korktuğun kadar kötü değilmiş!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "child_bully",
                    title = "Okul Zorbalığı",
                    description = "Bir çocuk sana laf attı ve önünde küçük düşürdü.",
                    ageRange = new AgeRange { min = 7, max = 11 },
                    category = EventCategory.School,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Görmezden gel",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -5,
                                    probability = 1f,
                                    resultText = "İçine attın ama moralin bozuldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Öğretmene söyle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.6f,
                                    resultText = "Öğretmen müdahale etti, zorba ceza aldı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.4f,
                                    resultText = "Öğretmen pek ciddiye almadı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Karşılık ver",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 15,
                                    probability = 0.4f,
                                    resultText = "Zorba geri adım attı!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.6f,
                                    resultText = "Kavga çıktı ve dayak yedin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "child_good_grades",
                    title = "Karne Günü",
                    description = "Karne günü geldi! Notların nasıl?",
                    ageRange = new AgeRange { min = 7, max = 11 },
                    category = EventCategory.School,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Çok çalıştım",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 7,
                                    probability = 0.7f,
                                    resultText = "Harika notlar aldın! Ailen gurur duyuyor."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 1,
                                    maxValue = 3,
                                    probability = 0.3f,
                                    resultText = "Ortalama notlar aldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Pek çalışmadım",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Notların düşük, ailen kızgın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "child_new_friend",
                    title = "Yeni Arkadaş",
                    description = "Sınıfa yeni bir öğrenci geldi. Onunla tanışmak ister misin?",
                    ageRange = new AgeRange { min = 6, max = 11 },
                    category = EventCategory.Social,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Evet, tanışmak istiyorum",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.7f,
                                    resultText = "Yeni bir arkadaş edindin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -2,
                                    maxValue = 0,
                                    probability = 0.3f,
                                    resultText = "Pek anlaşamadınız."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hayır, ilgilenmiyorum",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Kendi halinde takıldın."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetTeenEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "teen_first_love",
                    title = "İlk Aşk",
                    description = "Sınıftan birini çok beğeniyorsun. Ne yapacaksın?",
                    ageRange = new AgeRange { min = 13, max = 17 },
                    category = EventCategory.Romance,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Açıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 0.4f,
                                    resultText = "O da seni seviyormuş! Birlikte olmaya başladınız."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.6f,
                                    resultText = "Maalesef reddedildin. Kalbin kırıldı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Uzaktan izle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Cesaretini toplayamadın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "teen_party",
                    title = "Parti Daveti",
                    description = "Arkadaşın seni eve parti davet ediyor. Ailene söyleyecek misin?",
                    ageRange = new AgeRange { min = 14, max = 17 },
                    category = EventCategory.Social,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Aileme söyle ve izin al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.6f,
                                    resultText = "İzin verdiler, güzel vakit geçirdin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.4f,
                                    resultText = "İzin vermediler."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Gizlice git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 15,
                                    probability = 0.5f,
                                    resultText = "Harika bir gece geçirdin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -15,
                                    probability = 0.5f,
                                    resultText = "Ailen öğrendi, büyük kavga çıktı!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Gitme",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Evde kaldın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "teen_exam_stress",
                    title = "Sınav Stresi",
                    description = "Yarın önemli bir sınav var ve çok streslisin.",
                    ageRange = new AgeRange { min = 12, max = 17 },
                    category = EventCategory.School,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Gece boyu çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 5,
                                    probability = 0.7f,
                                    resultText = "İyi bir not aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -3,
                                    probability = 0.3f,
                                    resultText = "Uykusuzluktan sınıfta uyuyakaldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Erken yat, dinlenmiş git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 1,
                                    maxValue = 3,
                                    probability = 1f,
                                    resultText = "Ortalama bir not aldın ama sağlıklısın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kopya hazırla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 5,
                                    maxValue = 8,
                                    probability = 0.4f,
                                    resultText = "Kopya işe yaradı, iyi not aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -25,
                                    maxValue = -20,
                                    probability = 0.6f,
                                    resultText = "Yakalandın! Disipline gidiyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "teen_social_media",
                    title = "Sosyal Medya",
                    description = "Paylaştığın bir gönderi viral oldu!",
                    ageRange = new AgeRange { min = 13, max = 17 },
                    category = EventCategory.Social,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Devam et, daha fazla paylaş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.6f,
                                    resultText = "Takipçi sayın arttı!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.4f,
                                    resultText = "Olumsuz yorumlar aldın, moralin bozuldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Düşük profil tut",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "İlgi bir süre sonra azaldı."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetYoungAdultEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "adult_yks",
                    title = "YKS Sınavı",
                    description = "Üniversite sınavı günü geldi! Nasıl hissediyorsun?",
                    ageRange = new AgeRange { min = 18, max = 18 },
                    category = EventCategory.Milestone,
                    probability = 1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Çok çalıştım, hazırım",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.6f,
                                    resultText = "Harika bir puan aldın! İstediğin üniversiteyi kazandın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 2,
                                    maxValue = 4,
                                    probability = 0.4f,
                                    resultText = "Ortalama bir puan aldın. Bazı bölümler açık."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Pek hazır değilim",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.7f,
                                    resultText = "Düşük puan aldın. Gelecek yıl tekrar deneyebilirsin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Şans yardım etti, fena sayılmaz bir puan aldın!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "adult_first_job",
                    title = "İlk İş",
                    description = "Bir iş ilanı gördün. Başvuracak mısın?",
                    ageRange = new AgeRange { min = 18, max = 25 },
                    category = EventCategory.Career,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Başvur",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 15000,
                                    maxValue = 20000,
                                    probability = 0.5f,
                                    resultText = "İşe alındın! Artık maaş alıyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.5f,
                                    resultText = "Maalesef işe alınmadın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Başvurma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Fırsat kaçtı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "adult_military",
                    title = "Askerlik Çağrısı",
                    description = "Askerlik yaşın geldi. Ne yapacaksın?",
                    ageRange = new AgeRange { min = 20, max = 21 },
                    category = EventCategory.Military,
                    probability = 1f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Gender,
                            comparison = ComparisonType.Equal,
                            targetValue = 0 // Male
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Askerliği yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Askerliğini tamamladın. Fiziksel olarak güçlendin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Ertele (üniversite)",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Askerliğini ertelemeye hak kazandın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "adult_move_out",
                    title = "Evden Ayrılma",
                    description = "Kendi evine çıkmak istiyor musun?",
                    ageRange = new AgeRange { min = 22, max = 29 },
                    category = EventCategory.Milestone,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Evet, bağımsız olmak istiyorum",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -5000,
                                    probability = 1f,
                                    resultText = "Kendi evine taşındın! Kira ödemeye başladın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hayır, ailemle kalayım",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 2000,
                                    maxValue = 5000,
                                    probability = 1f,
                                    resultText = "Para biriktirmeye devam ediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "adult_marriage_proposal",
                    title = "Evlilik Teklifi",
                    description = "Uzun süredir birlikte olduğun kişiye evlenme teklifi etmek istiyor musun?",
                    ageRange = new AgeRange { min = 24, max = 35 },
                    category = EventCategory.Romance,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Teklif et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 20,
                                    maxValue = 30,
                                    probability = 0.7f,
                                    resultText = "EVET dedi! Evleniyorsunuz!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -25,
                                    maxValue = -20,
                                    probability = 0.3f,
                                    resultText = "Maalesef reddetti. İlişkiniz bitti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Daha bekle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Henüz hazır değilsin."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetAdultEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "adult_promotion",
                    title = "Terfi Fırsatı",
                    description = "İş yerinde terfi şansın var. Çok çalışıyorsun ama rekabet de var.",
                    ageRange = new AgeRange { min = 28, max = 55 },
                    category = EventCategory.Career,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Ekstra mesai yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 10000,
                                    maxValue = 20000,
                                    probability = 0.6f,
                                    resultText = "Terfi aldın! Maaşın arttı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.4f,
                                    resultText = "Stres yüzünden sağlığın bozuldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Normal tempo devam",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Terfiyi başkası aldı ama sağlıklısın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "adult_midlife_crisis",
                    title = "Orta Yaş Krizi",
                    description = "Hayatını sorgulamaya başladın. Bu kadar mı?",
                    ageRange = new AgeRange { min = 40, max = 50 },
                    category = EventCategory.Random,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yeni bir hobi edin",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "Yeni ilgi alanların hayata anlam kattı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Lüks bir araba al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -100000,
                                    maxValue = -50000,
                                    probability = 1f,
                                    resultText = "Araba aldın ama çok para harcadın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Psikolojik destek al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Terapi yardımcı oldu, kendini daha iyi hissediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "adult_child_born",
                    title = "Bebek Geliyor",
                    description = "Eşin hamile! Çocuk sahibi oluyorsunuz.",
                    ageRange = new AgeRange { min = 25, max = 45 },
                    category = EventCategory.Family,
                    probability = 0.3f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.IsMarried,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Harika haber!",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 1f,
                                    resultText = "Bebeğiniz dünyaya geldi! Artık bir ebeveynsin."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetSeniorEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "senior_retirement",
                    title = "Emeklilik",
                    description = "Emeklilik yaşı geldi. Çalışmaya devam mı, emekli mi?",
                    ageRange = new AgeRange { min = 60, max = 65 },
                    category = EventCategory.Milestone,
                    probability = 1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Emekli ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "Emekli oldun! Artık dinlenme zamanı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Çalışmaya devam et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 10000,
                                    probability = 1f,
                                    resultText = "Çalışmaya devam ediyorsun, para birikiyor."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "senior_grandchild",
                    title = "Torun",
                    description = "Çocuğun seni torun sahibi yaptı!",
                    ageRange = new AgeRange { min = 50, max = 80 },
                    category = EventCategory.Family,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Çok mutluyum!",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 1f,
                                    resultText = "Torunun seni çok mutlu ediyor!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "senior_health_check",
                    title = "Sağlık Kontrolü",
                    description = "Yıllık sağlık kontrolüne gitme zamanı.",
                    ageRange = new AgeRange { min = 60, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kontrole git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 0.7f,
                                    resultText = "Her şey yolunda! Sağlıklısın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "Bazı sağlık sorunları tespit edildi, tedaviye başladın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kontrole gitme",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.5f,
                                    resultText = "Bilmediğin sağlık sorunları kötüleşiyor olabilir."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.5f,
                                    resultText = "Şimdilik bir sorun yok gibi görünüyor."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetGeneralEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "general_lottery",
                    title = "Piyango Bileti",
                    description = "Piyango bileti almak ister misin?",
                    ageRange = new AgeRange { min = 18, max = 100 },
                    category = EventCategory.Financial,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Evet, şansımı deneyeyim",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 50000,
                                    maxValue = 500000,
                                    probability = 0.05f,
                                    resultText = "BÜYÜK İKRAMİYE! Zengin oldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 100,
                                    maxValue = 1000,
                                    probability = 0.2f,
                                    resultText = "Küçük bir şey kazandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -100,
                                    maxValue = -50,
                                    probability = 0.75f,
                                    resultText = "Maalesef kazanamadın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hayır, para israfı",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Paran cebinde kaldı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "general_earthquake",
                    title = "Deprem",
                    description = "Şehirde deprem oldu!",
                    ageRange = new AgeRange { min = 0, max = 100 },
                    category = EventCategory.Random,
                    probability = 0.15f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Güvenli bir yere sığın",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.8f,
                                    resultText = "Korkutucu bir deneyimdi ama sağsın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.2f,
                                    resultText = "Hafif yaralandın ama hayattasın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "general_found_money",
                    title = "Yerde Para",
                    description = "Yerde 500 TL buldun!",
                    ageRange = new AgeRange { min = 6, max = 100 },
                    category = EventCategory.Random,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Cebine at",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 500,
                                    maxValue = 500,
                                    probability = 1f,
                                    resultText = "500 TL kazandın!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Polise ver",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Doğru olanı yaptın, kendini iyi hissediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "general_gym",
                    title = "Spor Salonu",
                    description = "Spora başlamak ister misin?",
                    ageRange = new AgeRange { min = 12, max = 80 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Evet, üye ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.7f,
                                    resultText = "Düzenli spor yapıyorsun, sağlığın iyileşti!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Aşırı zorladın, sakatlandın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hayır, ilgilenmiyorum",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Belki başka zaman."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetCrimeEvents()
        {
            return new List<GameEvent>
            {
                // Mağaza Hırsızlığı (Teen)
                new GameEvent
                {
                    id = "crime_shoplifting_teen",
                    title = "Mağaza Hırsızlığı",
                    description = "Arkadaşların seni marketten bir şey çalmaya teşvik ediyor.",
                    ageRange = new AgeRange { min = 13, max = 17 },
                    category = EventCategory.Crime,
                    probability = 0.25f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Çal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 100,
                                    maxValue = 500,
                                    probability = 0.5f,
                                    resultText = "Başardın! Kimse fark etmedi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -30,
                                    maxValue = -20,
                                    probability = 0.5f,
                                    resultText = "Yakalandın! Güvenlik seni ailene teslim etti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Doğru kararı verdin. Kendini iyi hissediyorsun."
                                }
                            }
                        }
                    }
                },
                // Mağaza Hırsızlığı (Adult)
                new GameEvent
                {
                    id = "crime_shoplifting_adult",
                    title = "Mağaza Hırsızlığı",
                    description = "Para sıkıntısındasın. Marketten bir şey çalmayı düşünüyorsun.",
                    ageRange = new AgeRange { min = 18, max = 50 },
                    category = EventCategory.Crime,
                    probability = 0.2f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Çal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 200,
                                    maxValue = 1000,
                                    probability = 0.4f,
                                    resultText = "Başardın! Kimse görmedi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Hırsızlık",
                                    minValue = 1,
                                    maxValue = 2,
                                    probability = 0.6f,
                                    resultText = "Yakalandın! Hapis cezası aldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Vazgeç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Akıllıca bir karar."
                                }
                            }
                        }
                    }
                },
                // Araba Hırsızlığı
                new GameEvent
                {
                    id = "crime_car_theft",
                    title = "Araba Hırsızlığı",
                    description = "Sokakta anahtarları içinde bırakılmış lüks bir araba gördün.",
                    ageRange = new AgeRange { min = 18, max = 45 },
                    category = EventCategory.Crime,
                    probability = 0.15f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Arabayı çal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 50000,
                                    maxValue = 150000,
                                    probability = 0.3f,
                                    resultText = "Arabayı sattın! Büyük para kazandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Araba Hırsızlığı",
                                    minValue = 3,
                                    maxValue = 6,
                                    probability = 0.7f,
                                    resultText = "Polis seni yakaladı! Ağır hapis cezası aldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Geç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Yoluna devam ettin."
                                }
                            }
                        }
                    }
                },
                // Soygun
                new GameEvent
                {
                    id = "crime_robbery",
                    title = "Soygun Planı",
                    description = "Eski bir arkadaşın seni bir soygun planına dahil etmek istiyor.",
                    ageRange = new AgeRange { min = 20, max = 50 },
                    category = EventCategory.Crime,
                    probability = 0.1f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 100000,
                                    maxValue = 500000,
                                    probability = 0.2f,
                                    resultText = "Soygun başarılı! Zengin oldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Silahlı Soygun",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 0.8f,
                                    resultText = "Yakalandın! Çok ağır ceza aldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Akıllıca bir karar verdin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Polise ihbar et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 10000,
                                    probability = 1f,
                                    resultText = "İhbar ödülü kazandın!"
                                }
                            }
                        }
                    }
                },
                // Dolandırıcılık
                new GameEvent
                {
                    id = "crime_fraud",
                    title = "Dolandırıcılık Fırsatı",
                    description = "İnternette sahte ürün satarak para kazanabilirsin.",
                    ageRange = new AgeRange { min = 18, max = 55 },
                    category = EventCategory.Crime,
                    probability = 0.15f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        },
                        new EventCondition
                        {
                            type = ConditionType.Stat,
                            statType = StatType.Intelligence,
                            comparison = ComparisonType.GreaterOrEqual,
                            targetValue = 40
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Dolandır",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 20000,
                                    maxValue = 100000,
                                    probability = 0.35f,
                                    resultText = "Plan işe yaradı! İyi para kazandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Dolandırıcılık",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 0.65f,
                                    resultText = "Siber suçlar birimi seni yakaladı!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hayır, dürüst kal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Dürüstlük en iyi politika."
                                }
                            }
                        }
                    }
                },
                // Uyuşturucu Satışı
                new GameEvent
                {
                    id = "crime_drug_dealing",
                    title = "Uyuşturucu Teklifi",
                    description = "Biri sana uyuşturucu satarak para kazanmayı teklif ediyor.",
                    ageRange = new AgeRange { min = 18, max = 45 },
                    category = EventCategory.Crime,
                    probability = 0.1f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kabul et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 30000,
                                    maxValue = 80000,
                                    probability = 0.25f,
                                    resultText = "İşler iyi gidiyor, para kazanıyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Uyuşturucu Ticareti",
                                    minValue = 5,
                                    maxValue = 12,
                                    probability = 0.75f,
                                    resultText = "Narkotik operasyonunda yakalandın!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kesinlikle hayır",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Doğru kararı verdin."
                                }
                            }
                        }
                    }
                },
                // Saldırı
                new GameEvent
                {
                    id = "crime_assault",
                    title = "Kavga",
                    description = "Biri sana hakaret etti. Çok sinirliysin.",
                    ageRange = new AgeRange { min = 16, max = 50 },
                    category = EventCategory.Crime,
                    probability = 0.2f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Saldır",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.3f,
                                    resultText = "Hırsını aldın, ama şiddete başvurman yanlıştı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Saldırı",
                                    minValue = 1,
                                    maxValue = 3,
                                    probability = 0.7f,
                                    resultText = "Polis geldi ve tutuklandın!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sakinleş ve uzaklaş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.6f,
                                    resultText = "Sinirini yutmak zor oldu."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.4f,
                                    resultText = "Olgun davranmak seni iyi hissettirdi."
                                }
                            }
                        }
                    }
                },
                // Rüşvet
                new GameEvent
                {
                    id = "crime_bribery",
                    title = "Rüşvet Teklifi",
                    description = "Bir iş adamı sana yasadışı bir iş için rüşvet teklif ediyor.",
                    ageRange = new AgeRange { min = 25, max = 60 },
                    category = EventCategory.Crime,
                    probability = 0.1f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        },
                        new EventCondition
                        {
                            type = ConditionType.HasJob,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Rüşveti al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 50000,
                                    maxValue = 200000,
                                    probability = 0.4f,
                                    resultText = "Parayı aldın, kimse bilmiyor."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Rüşvet",
                                    minValue = 3,
                                    maxValue = 7,
                                    probability = 0.6f,
                                    resultText = "Müfettişler seni yakaladı!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet ve şikayet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.7f,
                                    resultText = "Dürüstlüğün takdir edildi!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Doğru olanı yaptın."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetPrisonEvents()
        {
            return new List<GameEvent>
            {
                // Kaçış Girişimi
                new GameEvent
                {
                    id = "prison_escape_attempt",
                    title = "Kaçış Planı",
                    description = "Hücre arkadaşın bir kaçış planı yaptı. Katılacak mısın?",
                    ageRange = new AgeRange { min = 18, max = 70 },
                    category = EventCategory.Crime,
                    probability = 0.3f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kaçmaya çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonRelease,
                                    minValue = 100,
                                    maxValue = 100,
                                    probability = 0.1f,
                                    resultText = "İnanılmaz! Kaçışın başarılı oldu!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Kaçış Girişimi",
                                    minValue = 3,
                                    maxValue = 5,
                                    probability = 0.9f,
                                    resultText = "Yakalandın! Cezana yıllar eklendi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Karışma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Akıllıca bir karar. Cezanı tamamlayacaksın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Gardiyana ihbar et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonRelease,
                                    minValue = 1,
                                    maxValue = 2,
                                    probability = 0.8f,
                                    resultText = "İyi davranışından dolayı cezandan düşüldü."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 0.2f,
                                    resultText = "Diğer mahkumlar ihbar ettiğini öğrendi..."
                                }
                            }
                        }
                    }
                },
                // İyi Hal İndirimi
                new GameEvent
                {
                    id = "prison_good_behavior",
                    title = "İyi Hal Değerlendirmesi",
                    description = "Hapishane komisyonu iyi hal indirimi için seni değerlendirecek.",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Crime,
                    probability = 0.4f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Komisyona çık",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonRelease,
                                    minValue = 1,
                                    maxValue = 3,
                                    probability = 0.5f,
                                    resultText = "Tebrikler! İyi hal indirimi aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.5f,
                                    resultText = "Maalesef talep reddedildi."
                                }
                            }
                        }
                    }
                },
                // Hapiste Kavga
                new GameEvent
                {
                    id = "prison_fight",
                    title = "Hapishane Kavgası",
                    description = "Bir mahkum sana sataşıyor. Ne yapacaksın?",
                    ageRange = new AgeRange { min = 18, max = 70 },
                    category = EventCategory.Crime,
                    probability = 0.35f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kavga et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -15,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Yaralandın ama saygınlık kazandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Hapiste Kavga",
                                    minValue = 1,
                                    maxValue = 2,
                                    probability = 0.4f,
                                    resultText = "Hücre cezası aldın, süren uzadı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Gardiyanı çağır",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.7f,
                                    resultText = "Diğer mahkumlar seni zayıf görüyor."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.3f,
                                    resultText = "Gardiyan durumu çözdü."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Geri çekil",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 1f,
                                    resultText = "Onurunla oynadılar ama sağlamsın."
                                }
                            }
                        }
                    }
                },
                // Hapiste Eğitim
                new GameEvent
                {
                    id = "prison_education",
                    title = "Hapishane Eğitim Programı",
                    description = "Hapishanede eğitim programına katılabilirsin.",
                    ageRange = new AgeRange { min = 18, max = 60 },
                    category = EventCategory.Crime,
                    probability = 0.3f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.8f,
                                    resultText = "Yeni şeyler öğrendin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonRelease,
                                    minValue = 1,
                                    maxValue = 1,
                                    probability = 0.2f,
                                    resultText = "Başarın takdir edildi, cezandan düşüldü!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İlgilenmiyorum",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Fırsatı kaçırdın."
                                }
                            }
                        }
                    }
                },
                // Hapiste Çete
                new GameEvent
                {
                    id = "prison_gang",
                    title = "Çete Daveti",
                    description = "Bir hapishane çetesi seni aralarına almak istiyor.",
                    ageRange = new AgeRange { min = 18, max = 50 },
                    category = EventCategory.Crime,
                    probability = 0.25f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.5f,
                                    resultText = "Artık korunuyorsun ama tehlikeli bir yola girdin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.PrisonSentence,
                                    targetStat = "Çete Faaliyeti",
                                    minValue = 2,
                                    maxValue = 4,
                                    probability = 0.5f,
                                    resultText = "Çete operasyonunda cezana yıllar eklendi!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.4f,
                                    resultText = "Reddettiğin için tehdit aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.6f,
                                    resultText = "Anladılar ve seni rahat bıraktılar."
                                }
                            }
                        }
                    }
                },
                // Hapiste Ziyaret
                new GameEvent
                {
                    id = "prison_visit",
                    title = "Aile Ziyareti",
                    description = "Ailen seni ziyarete geldi.",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Crime,
                    probability = 0.4f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Görüş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.7f,
                                    resultText = "Aileni görmek çok iyi geldi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "Onları bu halde görmek zor oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Görüşme",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 1f,
                                    resultText = "Yalnızlık seni yıprattı."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetUniversityEvents()
        {
            return new List<GameEvent>
            {
                // Üniversite Seçimi (YKS sonrası)
                new GameEvent
                {
                    id = "university_selection",
                    title = "Üniversite Tercihi",
                    description = "YKS sonuçların açıklandı! Hangi üniversite ve bölümü tercih edeceksin?",
                    ageRange = new AgeRange { min = 18, max = 19 },
                    category = EventCategory.Milestone,
                    probability = 0.9f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Education,
                            comparison = ComparisonType.Equal,
                            targetValue = 3 // Lise
                        },
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Boğaziçi - Bilgisayar Mühendisliği",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 80
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "Boğaziçi Üniversitesi|Bilgisayar Mühendisliği",
                                    minValue = 8,
                                    maxValue = 12,
                                    probability = 1f,
                                    resultText = "Tebrikler! Türkiye'nin en iyi üniversitelerinden birine kabul edildin!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "ODTÜ - Elektrik Elektronik Mühendisliği",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 75
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "ODTÜ|Elektrik Elektronik Mühendisliği",
                                    minValue = 7,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "ODTÜ'ye hoş geldin! Mühendislik kariyerine başlıyorsun."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İTÜ - Makine Mühendisliği",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 70
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "İTÜ|Makine Mühendisliği",
                                    minValue = 6,
                                    maxValue = 9,
                                    probability = 1f,
                                    resultText = "İTÜ'lü oldun! Teknik eğitim seni bekliyor."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hacettepe - Tıp Fakültesi",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 85
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "Hacettepe Üniversitesi|Tıp",
                                    minValue = 10,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "Tıp fakültesine kabul edildin! Uzun ama değerli bir yolculuk başlıyor."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Ankara - Hukuk Fakültesi",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 70
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "Ankara Üniversitesi|Hukuk",
                                    minValue = 6,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Hukuk fakültesine başlıyorsun! Adalet için çalışacaksın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İstanbul - İşletme",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 60
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "İstanbul Üniversitesi|İşletme",
                                    minValue = 5,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "İşletme okuyorsun! İş dünyası seni bekliyor."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Mimar Sinan - Güzel Sanatlar",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 50
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "Mimar Sinan Üniversitesi|Güzel Sanatlar",
                                    minValue = 5,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Sanat eğitimine başlıyorsun! Yaratıcılığını geliştir."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Devlet Üniversitesi - Psikoloji",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 55
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.UniversityEnroll,
                                    targetStat = "Devlet Üniversitesi|Psikoloji",
                                    minValue = 5,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Psikoloji okuyorsun! İnsan zihnini keşfedeceksin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Üniversite okuma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Üniversiteye gitmemeye karar verdin. Farklı bir yol seçtin."
                                }
                            }
                        }
                    }
                },
                // Üniversite Mezuniyet
                new GameEvent
                {
                    id = "university_graduation",
                    title = "Mezuniyet",
                    description = "Dört yıllık eğitimin sona erdi. Mezuniyet töreni yaklaşıyor!",
                    ageRange = new AgeRange { min = 22, max = 24 },
                    category = EventCategory.Milestone,
                    probability = 0.9f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Education,
                            comparison = ComparisonType.Equal,
                            targetValue = 4 // Üniversite
                        },
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Törene katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 1f,
                                    resultText = "Tebrikler! Diploma aldın ve aileni gururlandırdın!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Yüksek lisans yap",
                            requirements = new List<ChoiceCondition>
                            {
                                new ChoiceCondition
                                {
                                    type = ConditionType.Stat,
                                    statType = StatType.Intelligence,
                                    targetValue = 65
                                }
                            },
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 8,
                                    maxValue = 12,
                                    probability = 1f,
                                    resultText = "Akademik kariyerine devam ediyorsun!"
                                }
                            }
                        }
                    }
                },
                // Üniversite Sınav
                new GameEvent
                {
                    id = "university_exam",
                    title = "Final Sınavları",
                    description = "Final dönemi geldi. Çok stresli bir dönem!",
                    ageRange = new AgeRange { min = 19, max = 24 },
                    category = EventCategory.School,
                    probability = 0.5f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Education,
                            comparison = ComparisonType.Equal,
                            targetValue = 4 // Üniversite
                        },
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Çok çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 7,
                                    probability = 0.7f,
                                    resultText = "Harika notlar aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 0.3f,
                                    resultText = "Çok yoruldun, sağlığın etkilendi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Dengeli çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 1,
                                    maxValue = 4,
                                    probability = 1f,
                                    resultText = "Ortalama notlar aldın ama sağlıklısın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kopya çek",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 6,
                                    probability = 0.3f,
                                    resultText = "Yakalanmadan geçtin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -30,
                                    maxValue = -20,
                                    probability = 0.7f,
                                    resultText = "Yakalandın! Disiplin cezası aldın."
                                }
                            }
                        }
                    }
                },
                // Üniversite Staj
                new GameEvent
                {
                    id = "university_internship",
                    title = "Staj Fırsatı",
                    description = "Prestijli bir şirketten staj teklifi aldın.",
                    ageRange = new AgeRange { min = 20, max = 23 },
                    category = EventCategory.Career,
                    probability = 0.4f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Education,
                            comparison = ComparisonType.Equal,
                            targetValue = 4 // Üniversite
                        },
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Staja başla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 15000,
                                    probability = 0.6f,
                                    resultText = "Harika bir deneyim kazandın ve ücret aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 6,
                                    probability = 0.4f,
                                    resultText = "Çok şey öğrendin!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Fırsatı kaçırdın ama tatil yaptın."
                                }
                            }
                        }
                    }
                },
                // Üniversite Sosyal Hayat
                new GameEvent
                {
                    id = "university_social",
                    title = "Kampüs Partisi",
                    description = "Üniversitede büyük bir parti var!",
                    ageRange = new AgeRange { min = 18, max = 24 },
                    category = EventCategory.Social,
                    probability = 0.4f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Education,
                            comparison = ComparisonType.Equal,
                            targetValue = 4 // Üniversite
                        },
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Partiye git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.7f,
                                    resultText = "Harika bir gece geçirdin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "Biraz fazla kaçırdın..."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Evde çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Derslerine odaklandın."
                                }
                            }
                        }
                    }
                },
                // Üniversite Kulüp
                new GameEvent
                {
                    id = "university_club",
                    title = "Öğrenci Kulübü",
                    description = "Bir öğrenci kulübü seni davet ediyor.",
                    ageRange = new AgeRange { min = 18, max = 23 },
                    category = EventCategory.Social,
                    probability = 0.35f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Education,
                            comparison = ComparisonType.Equal,
                            targetValue = 4 // Üniversite
                        },
                        new EventCondition
                        {
                            type = ConditionType.InPrison,
                            comparison = ComparisonType.Equal,
                            targetValue = 0
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 0.8f,
                                    resultText = "Yeni arkadaşlar edindin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.2f,
                                    resultText = "Kulüp başkanı oldun!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Kendi zamanını yönetmeyi tercih ettin."
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
