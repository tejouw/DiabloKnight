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

            // Kariyer olayları
            events.AddRange(GetCareerEvents());

            // Sosyal olaylar
            events.AddRange(GetSocialEvents());

            // Sağlık olayları
            events.AddRange(GetHealthEvents());

            // Finansal olaylar
            events.AddRange(GetFinancialEvents());

            // Romantik olaylar
            events.AddRange(GetRomanticEvents());

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

        private static List<GameEvent> GetCareerEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "career_startup",
                    title = "Girişim Fırsatı",
                    description = "Bir arkadaşın seninle iş kurmak istiyor.",
                    ageRange = new AgeRange { min = 22, max = 45 },
                    category = EventCategory.Career,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yatırım yap ve ortak ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -30000,
                                    maxValue = -10000,
                                    probability = 0.5f,
                                    resultText = "Yatırım yaptın ama iş batık."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 50000,
                                    maxValue = 200000,
                                    probability = 0.5f,
                                    resultText = "İş tuttu! Büyük kar elde ettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Güvenli yolu seçtin." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "career_fired",
                    title = "İşten Çıkarılma",
                    description = "Şirket küçülüyor, senin işin tehlikede.",
                    ageRange = new AgeRange { min = 20, max = 60 },
                    category = EventCategory.Career,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Patron ile konuş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.4f,
                                    resultText = "Patron seni tutmaya karar verdi!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -15,
                                    probability = 0.6f,
                                    resultText = "Maalesef işten çıkarıldın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "career_freelance",
                    title = "Freelance İş",
                    description = "Online bir platform üzerinden freelance iş teklifi aldın.",
                    ageRange = new AgeRange { min = 18, max = 55 },
                    category = EventCategory.Career,
                    probability = 0.3f,
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
                                    minValue = 3000,
                                    maxValue = 15000,
                                    probability = 0.7f,
                                    resultText = "İşi başarıyla tamamladın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "Müşteri ödeme yapmadı!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "İşi reddettin." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "career_interview",
                    title = "İş Görüşmesi",
                    description = "Prestijli bir şirketten iş görüşmesine çağrıldın.",
                    ageRange = new AgeRange { min = 20, max = 50 },
                    category = EventCategory.Career,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Hazırlan ve git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 20000,
                                    maxValue = 40000,
                                    probability = 0.5f,
                                    resultText = "İşe alındın! Yüksek maaş alıyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.5f,
                                    resultText = "Görüşme iyi gitmedi."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "career_coworker_conflict",
                    title = "İş Arkadaşı Sorunu",
                    description = "Bir iş arkadaşın seninle sürtüşme halinde.",
                    ageRange = new AgeRange { min = 18, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Konuşarak çöz",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.6f,
                                    resultText = "Sorunu konuşarak çözdün."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.4f,
                                    resultText = "Tartışma büyüdü."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İnsan kaynaklarına şikayet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "İK sorunu çözdü."
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
                                    resultText = "Stres birikiyor."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "career_bonus",
                    title = "Performans Primi",
                    description = "Bu yılki performansın çok iyi! Prim alabilirsin.",
                    ageRange = new AgeRange { min = 20, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.35f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Primi talep et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 20000,
                                    probability = 0.7f,
                                    resultText = "Primi aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Bütçe yok denildi."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetSocialEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "social_reunion",
                    title = "Okul Buluşması",
                    description = "Eski okul arkadaşların buluşma düzenliyor.",
                    ageRange = new AgeRange { min = 25, max = 60 },
                    category = EventCategory.Social,
                    probability = 0.3f,
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
                                    probability = 0.7f,
                                    resultText = "Harika vakit geçirdin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Tuhaf bir akşamdı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Mazeret uydur",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Evde kaldın." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_neighbor",
                    title = "Komşu Problemi",
                    description = "Komşun gece çok gürültü yapıyor.",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Social,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kibarca uyar",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.6f,
                                    resultText = "Komşun özür diledi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 0.4f,
                                    resultText = "Kavga çıktı!"
                                }
                            }
                        },
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
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Polis uyardı, sessizlik oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kulaklık tak",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "İdare ettin." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_volunteer",
                    title = "Gönüllü Çalışma",
                    description = "Bir hayır kurumu gönüllü arıyor.",
                    ageRange = new AgeRange { min = 16, max = 70 },
                    category = EventCategory.Social,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Gönüllü ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 1f,
                                    resultText = "İnsanlara yardım etmek harika hissettirdi!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Vaktim yok",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Başka zaman." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_pet",
                    title = "Evcil Hayvan",
                    description = "Barınakta sevimli bir köpek seni bekliyor.",
                    ageRange = new AgeRange { min = 10, max = 70 },
                    category = EventCategory.Social,
                    probability = 0.2f,
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
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 1f,
                                    resultText = "Yeni bir dost edindin!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hayır",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Belki başka zaman." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_betrayal",
                    title = "Arkadaş İhaneti",
                    description = "Yakın arkadaşın sırrını başkalarına anlatmış.",
                    ageRange = new AgeRange { min = 12, max = 60 },
                    category = EventCategory.Social,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yüzleş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Arkadaşlık bitti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.4f,
                                    resultText = "Özür diledi, affettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Uzaklaş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Sessizce uzaklaştın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_famous_encounter",
                    title = "Ünlü Karşılaşması",
                    description = "Sokakta ünlü biriyle karşılaştın!",
                    ageRange = new AgeRange { min = 10, max = 80 },
                    category = EventCategory.Social,
                    probability = 0.15f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Fotoğraf iste",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.7f,
                                    resultText = "Çok nazik biriymiş!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Reddetti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Rahat bırak",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Saygılı davrandın." }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetHealthEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "health_flu",
                    title = "Grip",
                    description = "Grip oldun, kendini çok kötü hissediyorsun.",
                    ageRange = new AgeRange { min = 5, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Evde dinlen",
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
                        },
                        new EventChoice
                        {
                            text = "Doktora git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "İlaçlarla hızlı iyileştin."
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
                    ageRange = new AgeRange { min = 16, max = 80 },
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
                                    minValue = -25,
                                    maxValue = -10,
                                    probability = 0.7f,
                                    resultText = "Ciddi yaralandın ama tedavi edildin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Hafif sıyrıklarla kurtuldun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_diet",
                    title = "Diyet",
                    description = "Kilo vermek için diyet yapmak istiyor musun?",
                    ageRange = new AgeRange { min = 15, max = 70 },
                    category = EventCategory.Health,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Diyet yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Appearance",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.6f,
                                    resultText = "Kilo verdin, harika görünüyorsun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.4f,
                                    resultText = "Diyet çok zor, bıraktın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Gerek yok",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Kendini olduğun gibi kabul ettin." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_dental",
                    title = "Diş Ağrısı",
                    description = "Dişin çok ağrıyor!",
                    ageRange = new AgeRange { min = 8, max = 90 },
                    category = EventCategory.Health,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Dişçiye git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Diş tedavi edildi, rahatladın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -500,
                                    maxValue = -200,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Ağrı kesici iç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.7f,
                                    resultText = "Ağrı daha da kötüleşti!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 1,
                                    maxValue = 3,
                                    probability = 0.3f,
                                    resultText = "Geçici olarak rahatladın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_addiction",
                    title = "Kötü Alışkanlık",
                    description = "Arkadaşların seni sigara içmeye zorluyor.",
                    ageRange = new AgeRange { min = 14, max = 30 },
                    category = EventCategory.Health,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 3,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Sağlıklı kararı verdin!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bir kere dene",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Bağımlı oldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -2,
                                    maxValue = 0,
                                    probability = 0.4f,
                                    resultText = "Bir kerelik kaldı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_depression",
                    title = "Depresyon Belirtileri",
                    description = "Son zamanlarda çok mutsuz hissediyorsun.",
                    ageRange = new AgeRange { min = 14, max = 80 },
                    category = EventCategory.Health,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Profesyonel yardım al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 1f,
                                    resultText = "Terapi çok yardımcı oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kendi başıma atlatırım",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Durum kötüleşti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.4f,
                                    resultText = "Zamanla iyileştin."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetFinancialEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "financial_inheritance",
                    title = "Miras",
                    description = "Uzak bir akraban sana miras bıraktı!",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.1f,
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
                                    minValue = 20000,
                                    maxValue = 200000,
                                    probability = 1f,
                                    resultText = "Miras hesabına yatırıldı!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "financial_scam",
                    title = "Dolandırıcılık",
                    description = "Şüpheli bir yatırım fırsatı teklif edildi.",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.2f,
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
                                    minValue = -20000,
                                    maxValue = -5000,
                                    probability = 0.8f,
                                    resultText = "Dolandırıldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 10000,
                                    maxValue = 30000,
                                    probability = 0.2f,
                                    resultText = "Şaşırtıcı şekilde kar ettin!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Akıllıca bir karar." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "financial_tax",
                    title = "Vergi Denetimi",
                    description = "Vergi dairesi seni denetime aldı!",
                    ageRange = new AgeRange { min = 20, max = 70 },
                    category = EventCategory.Financial,
                    probability = 0.15f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Tam işbirliği yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.7f,
                                    resultText = "Her şey yolunda çıktı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -3000,
                                    probability = 0.3f,
                                    resultText = "Ufak bir vergi cezası ödedin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "financial_stock",
                    title = "Borsa Fırsatı",
                    description = "Bir hisse senedi çok yükseliyor, yatırım yapacak mısın?",
                    ageRange = new AgeRange { min = 18, max = 70 },
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
                                    minValue = 5000,
                                    maxValue = 50000,
                                    probability = 0.4f,
                                    resultText = "Harika! Hisse yükseldi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -15000,
                                    maxValue = -5000,
                                    probability = 0.6f,
                                    resultText = "Hisse çöktü, para kaybettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Riskli, geçiyorum",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Temkinli davrandın." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "financial_crypto",
                    title = "Kripto Para",
                    description = "Arkadaşın kripto para yatırımı öneriyor.",
                    ageRange = new AgeRange { min = 18, max = 50 },
                    category = EventCategory.Financial,
                    probability = 0.2f,
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
                                    minValue = 10000,
                                    maxValue = 100000,
                                    probability = 0.3f,
                                    resultText = "To the moon! Çok kazandın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -20000,
                                    maxValue = -5000,
                                    probability = 0.7f,
                                    resultText = "Piyasa çöktü, kaybettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Anlamıyorum, geç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Bilmediğin işe karışmadın." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "financial_robbery",
                    title = "Soygun",
                    description = "Evine hırsız girdi!",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Polisi ara",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -2000,
                                    probability = 0.7f,
                                    resultText = "Bazı eşyalar çalındı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "Hırsız kaçtı ama bir şey çalmadı."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetRomanticEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "romantic_jealousy",
                    title = "Kıskançlık",
                    description = "Partnerinin eski sevgilisiyle konuştuğunu gördün.",
                    ageRange = new AgeRange { min = 16, max = 60 },
                    category = EventCategory.Romance,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Güven, bir şey sorma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.7f,
                                    resultText = "Güvenin karşılığını buldu."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.3f,
                                    resultText = "Seni aldatıyormuş!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sorgula",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Tartışma çıktı, ilişki gerginleşti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.4f,
                                    resultText = "Merak etme dedi, sadece arkadaş."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "romantic_anniversary",
                    title = "Yıldönümü",
                    description = "Bugün ilişkinizin yıldönümü!",
                    ageRange = new AgeRange { min = 16, max = 80 },
                    category = EventCategory.Romance,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sürpriz hazırla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 1f,
                                    resultText = "Harika bir gece geçirdiniz!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -2000,
                                    maxValue = -500,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Unut",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -15,
                                    probability = 1f,
                                    resultText = "Partnerin çok kızdı!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "romantic_breakup",
                    title = "Ayrılık",
                    description = "Partnerinle aralarınız çok bozuldu.",
                    ageRange = new AgeRange { min = 14, max = 60 },
                    category = EventCategory.Romance,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İlişkiyi bitir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 0.7f,
                                    resultText = "Ayrılık acı verdi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Rahatladın, doğru kararı verdin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Düzeltmeye çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.4f,
                                    resultText = "İlişkiyi kurtardın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.6f,
                                    resultText = "İşe yaramadı, ayrıldınız."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "romantic_date",
                    title = "Randevu",
                    description = "Biri seni dışarı çıkmaya davet etti!",
                    ageRange = new AgeRange { min = 14, max = 50 },
                    category = EventCategory.Romance,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kabul et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.6f,
                                    resultText = "Harika bir randevu oldu!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.4f,
                                    resultText = "Pek uyumlu değildiniz."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Reddet",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Teklifi kibarca reddettin." }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "romantic_online",
                    title = "Online Tanışma",
                    description = "Bir arkadaşlık uygulamasında eşleştin!",
                    ageRange = new AgeRange { min = 18, max = 45 },
                    category = EventCategory.Romance,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Buluşmaya çık",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.5f,
                                    resultText = "Gerçek hayatta da uyumluymuşsunuz!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "Fotoğraflarından çok farklıymış."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.2f,
                                    resultText = "Ortalama bir buluşmaydı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Konuşmaya devam et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome { type = OutcomeType.None, probability = 1f, resultText = "Online sohbete devam ettiniz." }
                            }
                        }
                    }
                }
            };
        }
    }
}
