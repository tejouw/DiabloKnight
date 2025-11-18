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

            // BitLife özellikleri - Yeni olaylar
            events.AddRange(GetCrimeEvents());
            events.AddRange(GetAssetEvents());
            events.AddRange(GetGamblingEvents());
            events.AddRange(GetHealthEvents());
            events.AddRange(GetSocialMediaEvents());
            events.AddRange(GetPetEvents());
            events.AddRange(GetTravelEvents());
            events.AddRange(GetBusinessEvents());

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

        #region Crime Events

        private static List<GameEvent> GetCrimeEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "crime_shoplifting",
                    title = "Hırsızlık Fırsatı",
                    description = "Markette güvenlik kamerası çalışmıyor. Bir şey çalmayı düşünüyor musun?",
                    ageRange = new AgeRange { min = 12, max = 60 },
                    category = EventCategory.Crime,
                    probability = 0.3f,
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
                                    probability = 0.6f,
                                    resultText = "Başarıyla çaldın ve yakalanmadın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -30,
                                    maxValue = -20,
                                    probability = 0.4f,
                                    resultText = "Yakalandın! Polise teslim edildin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Çalma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Doğru olanı yaptın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "crime_car_theft",
                    title = "Araba Hırsızlığı",
                    description = "Parkta anahtarı üzerinde bırakılmış bir araba gördün.",
                    ageRange = new AgeRange { min = 18, max = 50 },
                    category = EventCategory.Crime,
                    probability = 0.2f,
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
                                    minValue = 20000,
                                    maxValue = 50000,
                                    probability = 0.4f,
                                    resultText = "Arabayı sattın ve iyi para kazandın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -50,
                                    maxValue = -40,
                                    probability = 0.6f,
                                    resultText = "Polis seni yakaladı! Hapis cezası aldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sahibini bul",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 500,
                                    maxValue = 2000,
                                    probability = 0.5f,
                                    resultText = "Sahibi seni ödüllendirdi!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.5f,
                                    resultText = "İyi bir iş yaptın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Görmezden gel",
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
                new GameEvent
                {
                    id = "crime_drug_dealing",
                    title = "Uyuşturucu Satışı",
                    description = "Bir tanıdık sana kolay para teklif ediyor: uyuşturucu satışı.",
                    ageRange = new AgeRange { min = 16, max = 45 },
                    category = EventCategory.Crime,
                    probability = 0.15f,
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
                                    minValue = 10000,
                                    maxValue = 50000,
                                    probability = 0.3f,
                                    resultText = "Çok para kazandın ama riskli bir iş."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -60,
                                    maxValue = -50,
                                    probability = 0.7f,
                                    resultText = "Polis baskın yaptı! Ağır ceza aldın."
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
                                    resultText = "Akıllıca bir karar verdin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "crime_bank_robbery",
                    title = "Banka Soygunu",
                    description = "Çılgın bir fikrin var: Bankayı soymak.",
                    ageRange = new AgeRange { min = 20, max = 50 },
                    category = EventCategory.Crime,
                    probability = 0.1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Soygunu gerçekleştir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 500000,
                                    maxValue = 2000000,
                                    probability = 0.15f,
                                    resultText = "İNANILMAZ! Soygun başarılı! Milyoner oldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -80,
                                    maxValue = -60,
                                    probability = 0.5f,
                                    resultText = "Güvenlik seni vurdu! Ağır yaralandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.Death,
                                    probability = 0.35f,
                                    resultText = "Polis seni öldürdü!"
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
                                    resultText = "Akıllıca karar."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "crime_witness",
                    title = "Suça Tanık",
                    description = "Sokakta bir suça tanık oldun. Ne yapacaksın?",
                    ageRange = new AgeRange { min = 10, max = 80 },
                    category = EventCategory.Crime,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Polisi ara",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.7f,
                                    resultText = "Suçlu yakalandı, vatandaşlık görevini yaptın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 0.3f,
                                    resultText = "Suçlu seni fark etti ve tehdit etti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Müdahale et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.4f,
                                    resultText = "Kahraman oldun! Medyaya çıktın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -40,
                                    maxValue = -20,
                                    probability = 0.6f,
                                    resultText = "Suçlu sana saldırdı ve yaralandın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Görmezden gel",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Vicdanın sızladı."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Asset Events

        private static List<GameEvent> GetAssetEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "asset_buy_car",
                    title = "Araba Satın Alma",
                    description = "İkinci el güzel bir araba buldun. Almak ister misin?",
                    ageRange = new AgeRange { min = 18, max = 70 },
                    category = EventCategory.Financial,
                    probability = 0.35f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -80000,
                                    maxValue = -40000,
                                    probability = 0.8f,
                                    resultText = "Arabayı aldın! Artık arabalısın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -100000,
                                    maxValue = -80000,
                                    probability = 0.2f,
                                    resultText = "Araba sorunlu çıktı, tamir masrafı oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Alma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Belki daha iyisini bulursun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "asset_buy_house",
                    title = "Ev Satın Alma",
                    description = "Güzel bir daire satılık. Almayı düşünür müsün?",
                    ageRange = new AgeRange { min = 25, max = 70 },
                    category = EventCategory.Financial,
                    probability = 0.25f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Money,
                            comparison = ComparisonType.GreaterThan,
                            targetValue = 500000
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -2000000,
                                    maxValue = -800000,
                                    probability = 0.9f,
                                    resultText = "Ev sahibi oldun! Artık kira ödemiyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 20,
                                    maxValue = 30,
                                    probability = 0.1f,
                                    resultText = "Rüya evini buldun!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Alma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Kira ödemeye devam."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "asset_investment",
                    title = "Yatırım Fırsatı",
                    description = "Bir arkadaş hisse senedi önerisi yapıyor.",
                    ageRange = new AgeRange { min = 20, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yatırım yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 50000,
                                    maxValue = 200000,
                                    probability = 0.35f,
                                    resultText = "Hisse değer kazandı! Büyük kar ettin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -50000,
                                    maxValue = -20000,
                                    probability = 0.65f,
                                    resultText = "Hisse düştü, para kaybettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Yatırım yapma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Temkinli davrandın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "asset_crypto",
                    title = "Kripto Para",
                    description = "Yeni bir kripto para çıktı. Yatırım yapar mısın?",
                    ageRange = new AgeRange { min = 18, max = 60 },
                    category = EventCategory.Financial,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yatırım yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 100000,
                                    maxValue = 1000000,
                                    probability = 0.2f,
                                    resultText = "TO THE MOON! Kripto patladı!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -30000,
                                    maxValue = -10000,
                                    probability = 0.8f,
                                    resultText = "Kripto çöktü, para kaybettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Yatırım yapma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Riskten kaçındın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "asset_gold",
                    title = "Altın Yatırımı",
                    description = "Kuyumcuda altın almayı düşünüyor musun?",
                    ageRange = new AgeRange { min = 20, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Altın al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 20000,
                                    probability = 0.5f,
                                    resultText = "Altın değer kazandı!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -5000,
                                    probability = 0.5f,
                                    resultText = "Altın değer kaybetti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Alma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Nakit tutmaya devam."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Gambling Events

        private static List<GameEvent> GetGamblingEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "gambling_casino",
                    title = "Casino",
                    description = "Arkadaşların seni casinoya davet ediyor.",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Blackjack oyna",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 10000,
                                    maxValue = 100000,
                                    probability = 0.4f,
                                    resultText = "Kazandın! Şanslı gece!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -50000,
                                    maxValue = -10000,
                                    probability = 0.6f,
                                    resultText = "Kaybettin. Kumar tehlikeli."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Slot makinesi oyna",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 50000,
                                    maxValue = 500000,
                                    probability = 0.1f,
                                    resultText = "JACKPOT! İnanılmaz şans!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -20000,
                                    maxValue = -5000,
                                    probability = 0.9f,
                                    resultText = "Slot makinesi parayı yuttu."
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
                                    resultText = "Akıllıca karar."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "gambling_horse_racing",
                    title = "At Yarışı",
                    description = "At yarışlarında bahis oynayacak mısın?",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Favori ata bahis yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 1000,
                                    maxValue = 5000,
                                    probability = 0.5f,
                                    resultText = "At kazandı! Biraz para aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -3000,
                                    maxValue = -1000,
                                    probability = 0.5f,
                                    resultText = "At kaybetti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sürpriz ata bahis yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 20000,
                                    maxValue = 100000,
                                    probability = 0.15f,
                                    resultText = "Sürpriz at kazandı! Büyük ikramiye!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -2000,
                                    probability = 0.85f,
                                    resultText = "Sürpriz at kaybetti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bahis oynama",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Sadece izledin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "gambling_sports_bet",
                    title = "Spor Bahsi",
                    description = "Büyük maç var! Bahis oynar mısın?",
                    ageRange = new AgeRange { min = 18, max = 70 },
                    category = EventCategory.Financial,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Favori takıma bas",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 2000,
                                    maxValue = 10000,
                                    probability = 0.45f,
                                    resultText = "Takımın kazandı! Para kazandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -1000,
                                    probability = 0.55f,
                                    resultText = "Takımın kaybetti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bahis oynama",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Maçı izlemekle yetindin."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Health Events

        private static List<GameEvent> GetHealthEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "health_flu",
                    title = "Grip Salgını",
                    description = "Çevrende grip salgını var. Aşı olacak mısın?",
                    ageRange = new AgeRange { min = 5, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.35f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Aşı ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 0.9f,
                                    resultText = "Aşı oldun, korunuyorsun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.1f,
                                    resultText = "Aşıdan hafif yan etki yaşadın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Aşı olma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -15,
                                    maxValue = -8,
                                    probability = 0.4f,
                                    resultText = "Grip oldun! Bir hafta yataktasın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.6f,
                                    resultText = "Bu sefer atlattın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_depression",
                    title = "Depresyon Belirtileri",
                    description = "Son zamanlarda kendini mutsuz ve enerjisiz hissediyorsun.",
                    ageRange = new AgeRange { min = 15, max = 80 },
                    category = EventCategory.Health,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Psikolog/Psikiyatriste git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.8f,
                                    resultText = "Terapi çok yardımcı oldu!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -2000,
                                    probability = 0.2f,
                                    resultText = "Tedavi pahalı ama işe yarıyor."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kendi başına atlatmaya çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 0.7f,
                                    resultText = "Depresyon kötüleşti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Kendini topladın!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_smoking",
                    title = "Sigara Teklifi",
                    description = "Arkadaşların sana sigara teklif ediyor.",
                    ageRange = new AgeRange { min = 14, max = 30 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Sigaraya başladın. Bağımlılık tehlikesi!"
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
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Sağlığını korudun!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_accident",
                    title = "Kaza",
                    description = "Trafik kazası geçirdin!",
                    ageRange = new AgeRange { min = 5, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.15f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Ambulansı bekle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -30,
                                    maxValue = -10,
                                    probability = 0.7f,
                                    resultText = "Hastaneye kaldırıldın, tedavi altındasın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -60,
                                    maxValue = -40,
                                    probability = 0.3f,
                                    resultText = "Ciddi yaralandın, yoğun bakımdasın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_diet",
                    title = "Diyet",
                    description = "Sağlıklı beslenmeye başlamak ister misin?",
                    ageRange = new AgeRange { min = 15, max = 80 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Diyete başla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.6f,
                                    resultText = "Diyet işe yarıyor! Kendini daha iyi hissediyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Appearance",
                                    minValue = 3,
                                    maxValue = 7,
                                    probability = 0.4f,
                                    resultText = "Kilo verdin, daha iyi görünüyorsun!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Diyet yapma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Fast food'a devam."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Social Media Events

        private static List<GameEvent> GetSocialMediaEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "social_viral",
                    title = "Viral İçerik",
                    description = "Paylaştığın bir video viral oldu!",
                    ageRange = new AgeRange { min = 13, max = 50 },
                    category = EventCategory.Social,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İçerik üretmeye devam et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 10,
                                    maxValue = 25,
                                    probability = 0.5f,
                                    resultText = "Influencer olma yolundasın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 20000,
                                    probability = 0.3f,
                                    resultText = "Sponsorluk teklifi geldi!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.2f,
                                    resultText = "Hate yorum çok geldi, moralin bozuldu."
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
                                    resultText = "İlgi zamanla azaldı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_cyberbullying",
                    title = "Siber Zorbalık",
                    description = "İnternette sana saldıran birisi var.",
                    ageRange = new AgeRange { min = 12, max = 40 },
                    category = EventCategory.Social,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Engelle ve şikayet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.7f,
                                    resultText = "Engellemeye rağmen rahatsız oldum."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Hesabı kapatıldı, rahat ettik!"
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
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.7f,
                                    resultText = "Tartışma büyüdü, daha kötü oldu."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "İtibarın zedelendi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Görmezden gel",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = 0,
                                    probability = 1f,
                                    resultText = "Zaman içinde durdu."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_youtube",
                    title = "YouTube Kanalı",
                    description = "YouTube kanalı açmak ister misin?",
                    ageRange = new AgeRange { min = 13, max = 50 },
                    category = EventCategory.Social,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kanal aç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.4f,
                                    resultText = "Kanalın büyümeye başladı!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.6f,
                                    resultText = "Pek izlenmedi ama devam ediyorsun."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Açma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "İzleyici olarak kaldın."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Pet Events

        private static List<GameEvent> GetPetEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "pet_dog",
                    title = "Köpek Sahiplenmek",
                    description = "Barınakta sevimli bir köpek gördün. Sahiplenmek ister misin?",
                    ageRange = new AgeRange { min = 8, max = 70 },
                    category = EventCategory.Family,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sahiplen",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 1f,
                                    resultText = "Köpeğin seni çok mutlu ediyor!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sahiplenme",
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
                },
                new GameEvent
                {
                    id = "pet_cat",
                    title = "Kedi Sahiplenmek",
                    description = "Kapının önünde terk edilmiş bir kedi yavrusu var.",
                    ageRange = new AgeRange { min = 6, max = 80 },
                    category = EventCategory.Family,
                    probability = 0.35f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İçeri al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "Kedin hayatına neşe kattı!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Barınağa ver",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "İyi bir yuva bulmasını umdun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "pet_sick",
                    title = "Evcil Hayvan Hasta",
                    description = "Evcil hayvanın hastalandı!",
                    ageRange = new AgeRange { min = 6, max = 80 },
                    category = EventCategory.Health,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Veterinere götür",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -3000,
                                    maxValue = -1000,
                                    probability = 0.8f,
                                    resultText = "Tedavi pahalı ama iyileşti!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -15,
                                    probability = 0.2f,
                                    resultText = "Maalesef kurtarılamadı..."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Evde tedavi et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = 5,
                                    probability = 0.5f,
                                    resultText = "Kendiliğinden iyileşti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -25,
                                    maxValue = -20,
                                    probability = 0.5f,
                                    resultText = "Durumu kötüleşti ve kaybettin..."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Travel Events

        private static List<GameEvent> GetTravelEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "travel_vacation",
                    title = "Tatil Fırsatı",
                    description = "Ucuz bir tatil fırsatı buldun!",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Random,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Tatile git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 0.8f,
                                    resultText = "Harika bir tatil geçirdin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -15000,
                                    maxValue = -5000,
                                    probability = 0.2f,
                                    resultText = "Tatilde fazla harcadın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Tatile gitme",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Para biriktirmeye devam."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "travel_abroad",
                    title = "Yurt Dışı Seyahat",
                    description = "Yurt dışına seyahat etmek ister misin?",
                    ageRange = new AgeRange { min = 20, max = 70 },
                    category = EventCategory.Random,
                    probability = 0.2f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Money,
                            comparison = ComparisonType.GreaterThan,
                            targetValue = 50000
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Avrupa turu",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 20,
                                    maxValue = 30,
                                    probability = 0.9f,
                                    resultText = "Avrupa'da harika anılar biriktirdin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -50000,
                                    maxValue = -30000,
                                    probability = 0.1f,
                                    resultText = "Seyahat çok pahalıydı."
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
                                    resultText = "Belki başka zaman."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Business Events

        private static List<GameEvent> GetBusinessEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "business_startup",
                    title = "Girişim Fikri",
                    description = "Harika bir iş fikrin var! Kendi işini kurmak ister misin?",
                    ageRange = new AgeRange { min = 20, max = 60 },
                    category = EventCategory.Career,
                    probability = 0.2f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Money,
                            comparison = ComparisonType.GreaterThan,
                            targetValue = 100000
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İşi kur",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 200000,
                                    maxValue = 1000000,
                                    probability = 0.3f,
                                    resultText = "İşin patladı! Çok para kazandın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -100000,
                                    maxValue = -50000,
                                    probability = 0.7f,
                                    resultText = "İş başarısız oldu, yatırımını kaybettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Maaşlı işte kal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Güvenli yolu seçtin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "business_restaurant",
                    title = "Restoran Açma",
                    description = "Bir restoran açmak ister misin?",
                    ageRange = new AgeRange { min = 25, max = 60 },
                    category = EventCategory.Career,
                    probability = 0.15f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Money,
                            comparison = ComparisonType.GreaterThan,
                            targetValue = 200000
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Restoran aç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 100000,
                                    maxValue = 500000,
                                    probability = 0.35f,
                                    resultText = "Restoranın çok popüler oldu!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -200000,
                                    maxValue = -100000,
                                    probability = 0.65f,
                                    resultText = "Restoran müşteri bulamadı, kapattın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Açma",
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
                },
                new GameEvent
                {
                    id = "business_freelance",
                    title = "Freelance İş",
                    description = "Freelance iş fırsatı! Yan gelir elde edebilirsin.",
                    ageRange = new AgeRange { min = 18, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İşi al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 20000,
                                    probability = 0.7f,
                                    resultText = "İşi başarıyla tamamladın, para kazandın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "İş çok stresli oldu."
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
                                    resultText = "Serbest zamanını tercih ettin."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion
    }
}
