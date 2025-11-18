using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Events
{
    /// <summary>
    /// Ek olaylar - Daha zengin oyun deneyimi için.
    /// </summary>
    public static class AdditionalEvents
    {
        public static List<GameEvent> GetAllAdditionalEvents()
        {
            var events = new List<GameEvent>();

            events.AddRange(GetSchoolEvents());
            events.AddRange(GetHealthEvents());
            events.AddRange(GetCareerEvents());
            events.AddRange(GetSocialEvents());
            events.AddRange(GetRomanceEvents());
            events.AddRange(GetFamilyEvents());
            events.AddRange(GetRandomEncounters());
            events.AddRange(GetFinancialEvents());

            return events;
        }

        private static List<GameEvent> GetSchoolEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "school_competition",
                    title = "Bilgi Yarışması",
                    description = "Okulda bir bilgi yarışması düzenleniyor. Katılmak ister misin?",
                    ageRange = new AgeRange { min = 8, max = 17 },
                    category = EventCategory.School,
                    probability = 0.4f,
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
                                    probability = 0.5f,
                                    resultText = "Yarışmayı kazandın! Herkes seni tebrik ediyor."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.5f,
                                    resultText = "Yarışmayı kaybettin ama güzel bir deneyim oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Katılma",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Yarışmayı izlemekle yetindin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "school_project",
                    title = "Grup Projesi",
                    description = "Öğretmen bir grup projesi verdi. Takımında sorunlar çıkıyor.",
                    ageRange = new AgeRange { min = 10, max = 17 },
                    category = EventCategory.School,
                    probability = 0.45f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Projeyi tek başına yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 7,
                                    probability = 0.7f,
                                    resultText = "Projeyi bitirdin ve öğretmen beğendi!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -3,
                                    probability = 0.3f,
                                    resultText = "Çok yoruldun ama iyi not aldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Takımla çöz",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.6f,
                                    resultText = "Birlikte güzel bir iş çıkardınız!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -5,
                                    probability = 0.4f,
                                    resultText = "Takım çalışmadı, proje kötü oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Öğretmene şikayet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 1f,
                                    resultText = "Arkadaşların sana kızgın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "school_talent_show",
                    title = "Yetenek Gösterisi",
                    description = "Okulda yetenek gösterisi var. Sahneye çıkacak mısın?",
                    ageRange = new AgeRange { min = 7, max = 17 },
                    category = EventCategory.School,
                    probability = 0.35f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sahneye çık",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.4f,
                                    resultText = "Harika bir performans! Herkes alkışlıyor."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.3f,
                                    resultText = "Sahne korkusu yaşadın ve berbat oldu."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Fena değildi, alkış aldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İzle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Diğerlerini izledin."
                                }
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
                    title = "Grip Salgını",
                    description = "Okulda/İşte grip salgını var. Sen de hastalandın!",
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
                                    resultText = "Birkaç gün yattın ve iyileştin."
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
                                    type = OutcomeType.MoneyChange,
                                    minValue = -300,
                                    maxValue = -200,
                                    probability = 1f,
                                    resultText = "Doktor ilaç verdi, çabuk iyileştin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Hasta hasta devam et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -15,
                                    maxValue = -8,
                                    probability = 0.7f,
                                    resultText = "Hastalık kötüleşti!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.3f,
                                    resultText = "Bir şekilde atlatttın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_accident",
                    title = "Kaza",
                    description = "Yolda yürürken bir araba sana çarptı!",
                    ageRange = new AgeRange { min = 5, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.15f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Ambulans çağır",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 0.7f,
                                    resultText = "Hastaneye kaldırıldın. Tedavi aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -40,
                                    maxValue = -25,
                                    probability = 0.3f,
                                    resultText = "Ciddi yaralandın! Uzun tedavi gerekiyor."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_food_poisoning",
                    title = "Gıda Zehirlenmesi",
                    description = "Yediğin bir şeyden zehirlendin!",
                    ageRange = new AgeRange { min = 3, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Evde bekle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 1f,
                                    resultText = "Birkaç gün kötü geçirdin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Acile git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -500,
                                    maxValue = -300,
                                    probability = 1f,
                                    resultText = "Serum taktılar, çabuk toparladın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "health_depression",
                    title = "Depresyon",
                    description = "Son zamanlarda çok mutsuz hissediyorsun. Her şey anlamsız geliyor.",
                    ageRange = new AgeRange { min = 14, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Psikologa git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.8f,
                                    resultText = "Terapi çok yardımcı oldu. Daha iyi hissediyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -1000,
                                    maxValue = -500,
                                    probability = 0.2f,
                                    resultText = "Terapi pahalı ama faydalı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Arkadaşlarınla konuş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 12,
                                    probability = 1f,
                                    resultText = "Arkadaşların seni dinledi. Biraz rahatladın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kendi kendine çöz",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -5,
                                    probability = 0.7f,
                                    resultText = "Durum daha da kötüleşti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Zamanla daha iyi hissettin."
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
                    id = "career_coworker_conflict",
                    title = "İş Arkadaşı Sorunu",
                    description = "Bir iş arkadaşın seninle sürekli problem çıkarıyor.",
                    ageRange = new AgeRange { min = 18, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.35f,
                    conditions = new List<EventCondition>
                    {
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
                            text = "Müdüre şikayet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.5f,
                                    resultText = "Müdür sorunu çözdü."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.5f,
                                    resultText = "Müdür seni dinlemedi, sorun devam ediyor."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Doğrudan konuş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 0.6f,
                                    resultText = "Konuştunuz ve anlaştınız."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.4f,
                                    resultText = "Kavga çıktı, durum kötüleşti."
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
                                    resultText = "Stres birikti ama idare ettin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "career_business_trip",
                    title = "İş Gezisi",
                    description = "Patronun seni bir iş gezisine göndermek istiyor.",
                    ageRange = new AgeRange { min = 22, max = 60 },
                    category = EventCategory.Career,
                    probability = 0.3f,
                    conditions = new List<EventCondition>
                    {
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
                            text = "Kabul et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 2000,
                                    maxValue = 5000,
                                    probability = 0.7f,
                                    resultText = "Gezi başarılı geçti, harcırah aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Yorucu bir gezi oldu."
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
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Patron biraz gücendi."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "career_fired",
                    title = "İşten Çıkarılma Riski",
                    description = "Şirket küçülüyor ve senin pozisyonun risk altında.",
                    ageRange = new AgeRange { min = 20, max = 60 },
                    category = EventCategory.Career,
                    probability = 0.2f,
                    conditions = new List<EventCondition>
                    {
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
                            text = "Ekstra çalış ve kendini kanıtla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 15,
                                    probability = 0.6f,
                                    resultText = "Çaban karşılığını buldu, işin güvende!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -25,
                                    maxValue = -20,
                                    probability = 0.4f,
                                    resultText = "Çabaların yeterli olmadı, işten çıkarıldın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Yeni iş ara",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.5f,
                                    resultText = "Başka bir şirkette iş buldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.5f,
                                    resultText = "Henüz yeni iş bulamadın."
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
                    id = "social_celebrity_encounter",
                    title = "Ünlü ile Karşılaşma",
                    description = "Sokakta ünlü biriyle karşılaştın!",
                    ageRange = new AgeRange { min = 10, max = 100 },
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
                                    resultText = "Fotoğraf çektirdin! Çok mutlusun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Reddedildin, acele işi varmış."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Rahatsız etme",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Saygılı davrandın ve geçtin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_neighborhood_gossip",
                    title = "Mahalle Dedikodusu",
                    description = "Hakkında dedikodu yapıldığını duydun!",
                    ageRange = new AgeRange { min = 15, max = 100 },
                    category = EventCategory.Social,
                    probability = 0.25f,
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
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.5f,
                                    resultText = "Dedikodu yapanlar özür diledi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.5f,
                                    resultText = "Kavga çıktı, durum kötüleşti."
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
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Dedikodular bir süre sonra durdu."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "social_volunteer",
                    title = "Gönüllü Çalışma",
                    description = "Bir yardım kuruluşu gönüllü arıyor. Katılmak ister misin?",
                    ageRange = new AgeRange { min = 14, max = 100 },
                    category = EventCategory.Social,
                    probability = 0.3f,
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
                                    resultText = "İnsanlara yardım etmek çok güzel hissettirdi!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Geçiştir",
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

        private static List<GameEvent> GetRomanceEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "romance_secret_admirer",
                    title = "Gizli Hayran",
                    description = "Birileri sana anonim bir aşk mektubu göndermiş!",
                    ageRange = new AgeRange { min = 14, max = 50 },
                    category = EventCategory.Romance,
                    probability = 0.25f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kim olduğunu araştır",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 0.5f,
                                    resultText = "Hayranını buldun ve tanıştınız! Çok tatlı biri."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.5f,
                                    resultText = "Bulamadın, gizem devam ediyor."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Umursama",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Mektubu çöpe attın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "romance_breakup",
                    title = "Ayrılık",
                    description = "Sevgilin seninle ayrılmak istiyor.",
                    ageRange = new AgeRange { min = 16, max = 100 },
                    category = EventCategory.Romance,
                    probability = 0.2f,
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
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 1f,
                                    resultText = "Ayrıldınız. Kalbin kırık."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İkna etmeye çalış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.3f,
                                    resultText = "İkna ettin! İlişkiniz devam ediyor."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -25,
                                    maxValue = -15,
                                    probability = 0.7f,
                                    resultText = "Çabaların işe yaramadı. Ayrıldınız."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "romance_anniversary",
                    title = "Yıldönümü",
                    description = "Bugün sevgilinle yıldönümünüz!",
                    ageRange = new AgeRange { min = 16, max = 100 },
                    category = EventCategory.Romance,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sürpriz yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 0.8f,
                                    resultText = "Harika bir gece geçirdiniz!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -2000,
                                    maxValue = -500,
                                    probability = 0.2f,
                                    resultText = "Romantik bir akşam yemeği yediniz."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Unutmuş gibi yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 1f,
                                    resultText = "Sevgilin çok kırıldı!"
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetFamilyEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "family_inheritance",
                    title = "Miras",
                    description = "Uzak bir akraban vefat etti ve sana miras bıraktı!",
                    ageRange = new AgeRange { min = 18, max = 100 },
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
                                    minValue = 50000,
                                    maxValue = 500000,
                                    probability = 0.7f,
                                    resultText = "Güzel bir miras aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -5000,
                                    probability = 0.3f,
                                    resultText = "Miras borçlarla birlikte geldi."
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
                                    resultText = "Mirası reddettın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "family_reunion",
                    title = "Aile Toplantısı",
                    description = "Tüm aile bir araya geliyor!",
                    ageRange = new AgeRange { min = 5, max = 100 },
                    category = EventCategory.Family,
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
                                    resultText = "Aileyle güzel vakit geçirdin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Akrabalar sinir bozucuydu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bahane uydur",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Ailen biraz küstü."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "family_sibling_fight",
                    title = "Kardeş Kavgası",
                    description = "Kardeşinle büyük bir kavga ettiniz!",
                    ageRange = new AgeRange { min = 5, max = 50 },
                    category = EventCategory.Family,
                    probability = 0.3f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition
                        {
                            type = ConditionType.HasSibling,
                            comparison = ComparisonType.Equal,
                            targetValue = 1
                        }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Özür dile",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.8f,
                                    resultText = "Barıştınız."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.2f,
                                    resultText = "Özrünü kabul etmedi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İnat et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 1f,
                                    resultText = "Soğuk savaş devam ediyor."
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<GameEvent> GetRandomEncounters()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "random_stray_animal",
                    title = "Sahipsiz Hayvan",
                    description = "Sokakta aç bir kedi/köpek buldun.",
                    ageRange = new AgeRange { min = 5, max = 100 },
                    category = EventCategory.Random,
                    probability = 0.25f,
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
                                    resultText = "Yeni bir dostun var!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Yiyecek ver ve bırak",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 7,
                                    probability = 1f,
                                    resultText = "İyi bir şey yaptın."
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
                new GameEvent
                {
                    id = "random_wallet_found",
                    title = "Kayıp Cüzdan",
                    description = "Yerde içi para dolu bir cüzdan buldun!",
                    ageRange = new AgeRange { min = 8, max = 100 },
                    category = EventCategory.Random,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sahibini bul",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.7f,
                                    resultText = "Sahibi çok teşekkür etti ve ödül verdi!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Doğru olanı yaptın, kendini iyi hissediyorsun."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Parayı al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 500,
                                    maxValue = 2000,
                                    probability = 0.8f,
                                    resultText = "Parayı cebine attın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.2f,
                                    resultText = "Sahibi seni gördü ve bağırdı!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "random_rainy_day",
                    title = "Yağmurlu Gün",
                    description = "Şemsiyesiz yağmura yakalandın!",
                    ageRange = new AgeRange { min = 0, max = 100 },
                    category = EventCategory.Random,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Koş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.5f,
                                    resultText = "Biraz ıslandın ama idare ettin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -8,
                                    maxValue = -5,
                                    probability = 0.5f,
                                    resultText = "Sırılsıklam oldun, üşüttün."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bir yere sığın",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Yağmurun dinmesini bekledin."
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
                    id = "financial_investment",
                    title = "Yatırım Fırsatı",
                    description = "Arkadaşın sana bir yatırım fırsatı sunuyor.",
                    ageRange = new AgeRange { min = 20, max = 100 },
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
                                    minValue = 10000,
                                    maxValue = 100000,
                                    probability = 0.4f,
                                    resultText = "Yatırım karşılığını verdi! Büyük kar!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -20000,
                                    maxValue = -5000,
                                    probability = 0.6f,
                                    resultText = "Yatırım battı, paranı kaybettin."
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
                                    resultText = "Temkinli davrandın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "financial_tax_audit",
                    title = "Vergi Denetimi",
                    description = "Vergi dairesi seni denetime çağırıyor!",
                    ageRange = new AgeRange { min = 22, max = 100 },
                    category = EventCategory.Financial,
                    probability = 0.15f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Dürüst ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.7f,
                                    resultText = "Denetim sorunsuz geçti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -1000,
                                    probability = 0.3f,
                                    resultText = "Küçük bir ceza kesildi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Belgeleri sakla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.5f,
                                    resultText = "Bir şey bulamadılar."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -50000,
                                    maxValue = -20000,
                                    probability = 0.5f,
                                    resultText = "Yakalandın! Ağır ceza kesildi."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "financial_crypto",
                    title = "Kripto Para",
                    description = "Arkadaşın kripto para almanı öneriyor.",
                    ageRange = new AgeRange { min = 18, max = 100 },
                    category = EventCategory.Financial,
                    probability = 0.2f,
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
                                    minValue = 20000,
                                    maxValue = 200000,
                                    probability = 0.3f,
                                    resultText = "Kripto uçtu! Büyük kar!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -3000,
                                    probability = 0.7f,
                                    resultText = "Kripto çöktü, zarar ettin."
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
                                    resultText = "Riskten kaçındın."
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
