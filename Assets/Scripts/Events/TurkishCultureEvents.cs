using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Events
{
    /// <summary>
    /// Türk kültürüne özgü olaylar.
    /// </summary>
    public static class TurkishCultureEvents
    {
        public static List<GameEvent> GetAllEvents()
        {
            var events = new List<GameEvent>();

            // Bayram olayları
            events.AddRange(GetBayramEvents());

            // Eğitim olayları
            events.AddRange(GetEducationEvents());

            // Sosyal olaylar
            events.AddRange(GetSocialEvents());

            // Günlük hayat
            events.AddRange(GetDailyLifeEvents());

            // Kariyer olayları
            events.AddRange(GetCareerEvents());

            // Aile olayları
            events.AddRange(GetFamilyEvents());

            // Romantik olaylar
            events.AddRange(GetRomanticEvents());

            // Rastgele olaylar
            events.AddRange(GetRandomEvents());

            return events;
        }

        #region Bayram Events

        private static List<GameEvent> GetBayramEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "ramazan_baslangic",
                    title = "Ramazan Başladı",
                    description = "Ramazan ayı başladı. Ne yapacaksın?",
                    ageRange = new AgeRange { min = 7, max = 120 },
                    category = EventCategory.Family,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Oruç tut",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -2,
                                    maxValue = 2,
                                    probability = 1f,
                                    resultText = "Oruç tuttun. Disiplinini artırdın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sadece sahura kalk",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 1,
                                    maxValue = 3,
                                    probability = 1f,
                                    resultText = "Aile ile sahur yaptın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Normal hayatına devam et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Ramazan'ı normal geçirdin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "kurban_bayrami",
                    title = "Kurban Bayramı",
                    description = "Kurban Bayramı geldi! Aile ziyaretleri zamanı.",
                    ageRange = new AgeRange { min = 0, max = 120 },
                    category = EventCategory.Family,
                    probability = 0.6f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Büyüklerin elini öp ve harçlık al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 100,
                                    maxValue = 1000,
                                    probability = 1f,
                                    resultText = "Bayram harçlığı aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kurban kes",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -3000,
                                    probability = 1f,
                                    resultText = "Kurban kesip etiyi dağıttın. Sevap kazandın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "seker_bayrami",
                    title = "Ramazan Bayramı",
                    description = "Şeker Bayramı geldi! Çikolata ve şeker zamanı.",
                    ageRange = new AgeRange { min = 0, max = 120 },
                    category = EventCategory.Family,
                    probability = 0.6f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kapı kapı dolaş, şeker topla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.8f,
                                    resultText = "Çok şeker topladın! Muhteşem bir bayramdı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.2f,
                                    resultText = "Çok şeker yedin, miden bulandı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Akrabaları ziyaret et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "Aile ile güzel vakit geçirdin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 50,
                                    maxValue = 500,
                                    probability = 0.8f,
                                    resultText = ""
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Education Events

        private static List<GameEvent> GetEducationEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "lgs_sinavi",
                    title = "LGS Sınavı",
                    description = "Liseye giriş sınavı günü geldi! Çok heyecanlısın.",
                    ageRange = new AgeRange { min = 14, max = 14 },
                    category = EventCategory.School,
                    probability = 1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Elinden gelenin en iyisini yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.7f,
                                    resultText = "LGS'den yüksek puan aldın! İyi bir liseye gidebilirsin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.3f,
                                    resultText = "Sınav istediğin gibi gitmedi. Ama daha liseden mezun olacaksın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Stres yap ve sınava girme",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 1f,
                                    resultText = "Sınava girmedin. Mahalle lisesine yazılacaksın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "yks_sinavi",
                    title = "YKS Sınavı",
                    description = "Üniversite sınavı! Hayatının en önemli sınavı.",
                    ageRange = new AgeRange { min = 17, max = 19 },
                    category = EventCategory.School,
                    probability = 1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Maksimum konsantrasyon",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 10,
                                    maxValue = 25,
                                    probability = 0.6f,
                                    resultText = "Harika bir sınav geçirdin! Top üniversitelere başvurabilirsin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -5,
                                    probability = 0.4f,
                                    resultText = "Sınav zor geçti. Ama yine de bazı bölümlere girebilirsin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Geçen yılın sorularına bak (kopya)",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Şans bu sefer yüzüne güldü."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -30,
                                    maxValue = -20,
                                    probability = 0.7f,
                                    resultText = "Yakalandın! Sınavın iptal edildi."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "universite_kayit",
                    title = "Üniversite Kaydı",
                    description = "Üniversiteye kabul edildin! Hangi şehirde okuyacaksın?",
                    ageRange = new AgeRange { min = 18, max = 20 },
                    category = EventCategory.School,
                    probability = 0.8f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İstanbul'da oku (evden uzak)",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = "İstanbul'da üniversite hayatı başladı! Yeni arkadaşlar edindin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -3000,
                                    maxValue = -1000,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kendi şehrinde oku (evde kal)",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 7,
                                    probability = 1f,
                                    resultText = "Aile yanında okumaya devam ediyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "dershane_kayit",
                    title = "Dershane Kaydı",
                    description = "Sınavlara hazırlanmak için dershane teklifi geldi.",
                    ageRange = new AgeRange { min = 14, max = 18 },
                    category = EventCategory.School,
                    probability = 0.7f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kayıt ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -2000,
                                    probability = 1f,
                                    resultText = "Dershaneye kaydoldun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.8f,
                                    resultText = "Derslerinde ilerleme kaydediyorsun!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kendi kendine çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 2,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Evde çalışmaya devam ediyorsun."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Social Events

        private static List<GameEvent> GetSocialEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "sunnet_dugunu",
                    title = "Sünnet Düğünü",
                    description = "Sünnet olma zamanı geldi!",
                    ageRange = new AgeRange { min = 5, max = 10 },
                    category = EventCategory.Family,
                    probability = 0.9f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Gender,
                            targetValue = 0 // Male
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Cesur ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.7f,
                                    resultText = "Sünnet düğünün muhteşem geçti! Çok hediye aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 500,
                                    maxValue = 5000,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kork",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Biraz zorlandın ama atlattın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 200,
                                    maxValue = 2000,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "kina_gecesi",
                    title = "Kına Gecesi",
                    description = "Bir arkadaşının kına gecesine davet edildin!",
                    ageRange = new AgeRange { min = 18, max = 50 },
                    category = EventCategory.Social,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Git ve eğlen",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 12,
                                    probability = 1f,
                                    resultText = "Harika bir gece geçirdin! Oynadın, söyledin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Mazeret uydur",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Gitmedin. Arkadaşın biraz kırıldı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "asker_ugurlamasi",
                    title = "Asker Uğurlaması",
                    description = "Askerlik zamanı geldi! Vatan borcu.",
                    ageRange = new AgeRange { min = 20, max = 28 },
                    category = EventCategory.Military,
                    probability = 0.9f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.Gender,
                            targetValue = 0 // Male
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Bedelli askerlik yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -90000,
                                    maxValue = -80000,
                                    probability = 1f,
                                    resultText = "Bedelli askerlik yaptın. 1 ayda tamamladın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Uzun dönem git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.8f,
                                    resultText = "6 ay askerlik yaptın. Disiplin kazandın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.2f,
                                    resultText = "Askerlik zor geçti ama tamamladın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "dugun_davetiyesi",
                    title = "Düğün Davetiyesi",
                    description = "Bir tanıdığının düğününe davet edildin.",
                    ageRange = new AgeRange { min = 16, max = 80 },
                    category = EventCategory.Social,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Git ve takı tak",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -1000,
                                    maxValue = -200,
                                    probability = 1f,
                                    resultText = "Güzel bir düğün geçirdin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sadece git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 1,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Düğüne gittim ama takı takmadın. Biraz ayıp oldu."
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
                                    resultText = "Düğüne gitmedin."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Daily Life Events

        private static List<GameEvent> GetDailyLifeEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "cay_molasi",
                    title = "Çay Molası",
                    description = "Komşu çaya davet etti.",
                    ageRange = new AgeRange { min = 10, max = 120 },
                    category = EventCategory.Social,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Git ve sohbet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 2,
                                    maxValue = 6,
                                    probability = 1f,
                                    resultText = "Güzel bir sohbet oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Meşgulüm de",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Bu sefer gitmedin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "mac_izleme",
                    title = "Maç Günü",
                    description = "Büyük derbi günü! Galatasaray - Fenerbahçe.",
                    ageRange = new AgeRange { min = 8, max = 120 },
                    category = EventCategory.Social,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Arkadaşlarla kahvede izle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.5f,
                                    resultText = "Takımın kazandı! Harika bir gece!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -3,
                                    probability = 0.5f,
                                    resultText = "Takımın kaybetti. Moral bozuldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Evde izle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 1,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Evde rahat rahat izledin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "sokak_kedisi",
                    title = "Sokak Kedisi",
                    description = "Sokakta aç bir kedi gördün.",
                    ageRange = new AgeRange { min = 5, max = 120 },
                    category = EventCategory.Random,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yemek ver",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Kedi mutlu oldu. İyi hissediyorsun."
                                }
                            }
                        },
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
                                    resultText = "Yeni bir dostun var! Kediyi sahiplendin."
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
                                    resultText = "Kediyi görmezden geldin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "trafik_cezasi",
                    title = "Trafik Cezası",
                    description = "Trafik kontrolünde ceza yedin!",
                    ageRange = new AgeRange { min = 18, max = 120 },
                    category = EventCategory.Random,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Cezayı öde",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -1000,
                                    maxValue = -200,
                                    probability = 1f,
                                    resultText = "Cezayı ödedin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İtiraz et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -500,
                                    maxValue = 0,
                                    probability = 0.3f,
                                    resultText = "İtiraz kabul edildi, ceza düşürüldü!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -1500,
                                    maxValue = -1000,
                                    probability = 0.7f,
                                    resultText = "İtiraz reddedildi. Ceza katlandı."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Career Events

        private static List<GameEvent> GetCareerEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "is_teklifi",
                    title = "İş Teklifi",
                    description = "Bir şirket seni işe almak istiyor!",
                    ageRange = new AgeRange { min = 18, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.4f,
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
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "Yeni işine başladın! Tebrikler."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 20000,
                                    probability = 1f,
                                    resultText = ""
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
                                    resultText = "Teklifi reddettim."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "zam_talebi",
                    title = "Zam Zamanı",
                    description = "Patronundan zam isteme vakti geldi.",
                    ageRange = new AgeRange { min = 20, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Zam iste",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 2000,
                                    maxValue = 10000,
                                    probability = 0.5f,
                                    resultText = "Zam aldın! Tebrikler."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 0.5f,
                                    resultText = "Zam talebim reddedildi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bekle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Daha uygun bir zaman beklemeye karar verdin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "issizlik",
                    title = "İşten Çıkarılma",
                    description = "Şirket küçülüyor. İşten çıkarıldın!",
                    ageRange = new AgeRange { min = 20, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Tazminat al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 5000,
                                    maxValue = 30000,
                                    probability = 1f,
                                    resultText = "Tazminat aldın. Yeni iş aramaya başla."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -5,
                                    probability = 1f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Dava aç",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 10000,
                                    maxValue = 100000,
                                    probability = 0.3f,
                                    resultText = "Davayı kazandın! Büyük tazminat aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -1000,
                                    probability = 0.7f,
                                    resultText = "Davayı kaybettin. Avukat masraflarını sen ödedin."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Family Events

        private static List<GameEvent> GetFamilyEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "aile_kavgasi",
                    title = "Aile Kavgası",
                    description = "Evde gerginlik var. Anne baba tartışıyor.",
                    ageRange = new AgeRange { min = 8, max = 20 },
                    category = EventCategory.Family,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Araya gir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = 5,
                                    probability = 0.5f,
                                    resultText = "Barıştırmaya çalıştın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -5,
                                    probability = 0.5f,
                                    resultText = "İşler daha da kötüleşti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Odana git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Odanda beklemeyi tercih ettin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "miras",
                    title = "Miras",
                    description = "Bir akraban vefat etti ve sana miras bıraktı.",
                    ageRange = new AgeRange { min = 18, max = 120 },
                    category = EventCategory.Family,
                    probability = 0.1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Mirası kabul et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 10000,
                                    maxValue = 500000,
                                    probability = 1f,
                                    resultText = "Miras aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = 10,
                                    probability = 1f,
                                    resultText = ""
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
                                    resultText = "Mirası reddettim."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Romantic Events

        private static List<GameEvent> GetRomanticEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "goz_goze",
                    title = "Göz Göze",
                    description = "Metroda biriyle göz göze geldin.",
                    ageRange = new AgeRange { min = 16, max = 50 },
                    category = EventCategory.Romance,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Gülümse",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 10,
                                    probability = 0.7f,
                                    resultText = "Gülümsedi! Numarasını aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.3f,
                                    resultText = "Bakışlarını kaçırdı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bakışlarını kaçır",
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
                new GameEvent
                {
                    id = "nisan_teklifi",
                    title = "Nişan Teklifi",
                    description = "Sevgilin nişan etmek istiyor.",
                    ageRange = new AgeRange { min = 20, max = 50 },
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
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 1f,
                                    resultText = "Nişanlandınız! Tebrikler."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -2000,
                                    probability = 1f,
                                    resultText = ""
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
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 1f,
                                    resultText = "Nişan teklifini reddettim. İlişki bitti."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Random Events

        private static List<GameEvent> GetRandomEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "deprem",
                    title = "Deprem!",
                    description = "Şiddetli bir deprem oldu!",
                    ageRange = new AgeRange { min = 0, max = 120 },
                    category = EventCategory.Random,
                    probability = 0.1f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Masanın altına gir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.9f,
                                    resultText = "Korktun ama zarar görmedin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -20,
                                    maxValue = -5,
                                    probability = 0.1f,
                                    resultText = "Yaralandın!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Dışarı koş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -8,
                                    probability = 1f,
                                    resultText = "Panikle dışarı çıktın. Üstüne bir şey düşmedi şans eseri."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "enflasyon",
                    title = "Enflasyon Artışı",
                    description = "Enflasyon patladı! Her şey zamlandı.",
                    ageRange = new AgeRange { min = 18, max = 120 },
                    category = EventCategory.Financial,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Döviz al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 1000,
                                    maxValue = 10000,
                                    probability = 0.6f,
                                    resultText = "İyi karar! Döviz değerlendi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -1000,
                                    probability = 0.4f,
                                    resultText = "Kötü zamanlama! Döviz düştü."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "TL'de kal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -3000,
                                    maxValue = -500,
                                    probability = 0.7f,
                                    resultText = "Paranın değeri eridi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.3f,
                                    resultText = "Bu sefer idare ettin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "telefon_bulma",
                    title = "Telefon Buldun",
                    description = "Yerde bir telefon buldun!",
                    ageRange = new AgeRange { min = 10, max = 120 },
                    category = EventCategory.Random,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sahibine ver",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.8f,
                                    resultText = "Sahibi çok teşekkür etti! Bahşiş verdi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 100,
                                    maxValue = 500,
                                    probability = 0.5f,
                                    resultText = ""
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sat",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 1000,
                                    maxValue = 5000,
                                    probability = 0.7f,
                                    resultText = "Telefonu sattın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Vicdan azabı çekiyorsun."
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
