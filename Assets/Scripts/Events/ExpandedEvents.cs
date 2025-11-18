using System.Collections.Generic;
using TurkishLifeSim.Core;

namespace TurkishLifeSim.Events
{
    /// <summary>
    /// Genişletilmiş olay seti - Daha zengin oyun deneyimi için ek olaylar.
    /// </summary>
    public static class ExpandedEvents
    {
        public static List<GameEvent> GetAllExpandedEvents()
        {
            var events = new List<GameEvent>();

            events.AddRange(GetSchoolEvents());
            events.AddRange(GetRomanceEvents());
            events.AddRange(GetCareerEvents());
            events.AddRange(GetHealthEvents());
            events.AddRange(GetSocialEvents());
            events.AddRange(GetFinancialEvents());
            events.AddRange(GetFamilyEvents());
            events.AddRange(GetCrimeEvents());
            events.AddRange(GetMilestoneEvents());

            return events;
        }

        #region School Events

        private static List<GameEvent> GetSchoolEvents()
        {
            return new List<GameEvent>
            {
                // İlkokul
                new GameEvent
                {
                    id = "school_bully",
                    title = "Okul Zorbalığı",
                    description = "Okulda bir çocuk sana zorbalık yapıyor.",
                    ageRange = new AgeRange { min = 6, max = 11 },
                    category = EventCategory.School,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Öğretmene söyle",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.7f,
                                    resultText = "Öğretmen duruma el attı. Zorbalık sona erdi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Öğretmen pek yardımcı olmadı. Ispiyoncu olarak anıldın."
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
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.5f,
                                    resultText = "Kendini savundun! Zorbalık bitti ve saygı kazandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.5f,
                                    resultText = "Kavgada yaralandın ve ceza aldın."
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
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 1f,
                                    resultText = "Zorbalık devam etti. Mutsuz hissediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "school_talent_show",
                    title = "Yetenek Gösterisi",
                    description = "Okulda yetenek gösterisi var. Katılmak ister misin?",
                    ageRange = new AgeRange { min = 7, max = 17 },
                    category = EventCategory.School,
                    probability = 0.4f,
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
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.6f,
                                    resultText = "Harika bir performans sergiledin! Herkes seni alkışladı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.4f,
                                    resultText = "Sahne korkusu yaşadın ve tökezledin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İzleyici ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 1,
                                    maxValue = 3,
                                    probability = 1f,
                                    resultText = "Arkadaşlarını izlemek güzeldi."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "exam_stress",
                    title = "Sınav Stresi",
                    description = "Yarın önemli bir sınav var ve çok streslisin.",
                    ageRange = new AgeRange { min = 10, max = 17 },
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
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 0.7f,
                                    resultText = "Sınavda başarılı oldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Uykusuzluktan hasta oldun."
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
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -10,
                                    probability = 0.4f,
                                    resultText = "Yakalandın! Disiplin cezası aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.6f,
                                    resultText = "Kopya çektin ama yakalanmadın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Rahatla ve uyu",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Dinlenmiş halde sınava girdin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "school_club",
                    title = "Okul Kulübü",
                    description = "Bir okul kulübüne katılma fırsatın var.",
                    ageRange = new AgeRange { min = 12, max = 17 },
                    category = EventCategory.School,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Spor kulübüne katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Spor kulübüne katıldın! Sağlığın gelişti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Bilim kulübüne katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Intelligence",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Bilim kulübüne katıldın! Zekan gelişti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Drama kulübüne katıl",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Drama kulübüne katıldın! Şöhretin arttı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "field_trip",
                    title = "Okul Gezisi",
                    description = "Okul gezisi düzenleniyor.",
                    ageRange = new AgeRange { min = 6, max = 17 },
                    category = EventCategory.School,
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
                                    maxValue = 12,
                                    probability = 0.8f,
                                    resultText = "Harika bir gezi oldu! Çok eğlendin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.2f,
                                    resultText = "Gezide yoruldun ve biraz hastalandın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Evde kal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Geziyi kaçırdın. Arkadaşların eğlenirken sen evde kaldın."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Romance Events

        private static List<GameEvent> GetRomanceEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "first_crush",
                    title = "İlk Aşk",
                    description = "Sınıftan birine ilgi duymaya başladın.",
                    ageRange = new AgeRange { min = 13, max = 17 },
                    category = EventCategory.Romance,
                    probability = 0.5f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Duygularını açıkla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.4f,
                                    resultText = "Duygularına karşılık buldu! Artık birliktesiniz."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Reddedildin. Kalbin kırıldı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Gizli tut",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Duygularını içinde tuttun. Biraz hüzünlüsün."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "romantic_date",
                    title = "Romantik Buluşma",
                    description = "Sevgilinle özel bir akşam yemeği planlıyorsun.",
                    ageRange = new AgeRange { min = 18, max = 50 },
                    category = EventCategory.Romance,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Pahalı restoran",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -500,
                                    maxValue = -300,
                                    probability = 1f,
                                    resultText = "Harika bir gece geçirdiniz!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "İlişkiniz güçlendi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Ev yemeği yap",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.7f,
                                    resultText = "Samimi ve güzel bir akşam geçirdiniz."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.3f,
                                    resultText = "Yemek biraz yanık çıktı ama yine de güzel vakit geçirdiniz."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "marriage_proposal",
                    title = "Evlilik Teklifi",
                    description = "Uzun süredir birlikte olduğun kişiye evlilik teklif etmeyi düşünüyorsun.",
                    ageRange = new AgeRange { min = 21, max = 50 },
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
                                    maxValue = 35,
                                    probability = 0.7f,
                                    resultText = "EVET dedi! Nişanlandınız!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -20,
                                    maxValue = -10,
                                    probability = 0.3f,
                                    resultText = "Reddedildin. Henüz hazır değilmiş."
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
                                    resultText = "Doğru zamanı beklemeye karar verdin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "relationship_argument",
                    title = "İlişki Kavgası",
                    description = "Partnerinle ciddi bir tartışma yaşadın.",
                    ageRange = new AgeRange { min = 16, max = 70 },
                    category = EventCategory.Romance,
                    probability = 0.4f,
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
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.8f,
                                    resultText = "Barıştınız. İlişkiniz daha güçlü."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.2f,
                                    resultText = "Özrün kabul edilmedi. Gerginlik devam ediyor."
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
                                    resultText = "Kavga büyüdü. İlişkiniz zarar gördü."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Zaman tanı",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 1,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Biraz soğuduktan sonra her şey yoluna girdi."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "anniversary",
                    title = "Yıl Dönümü",
                    description = "Bugün partnerinle yıl dönümünüz.",
                    ageRange = new AgeRange { min = 18, max = 80 },
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
                                    type = OutcomeType.MoneyChange,
                                    minValue = -300,
                                    maxValue = -100,
                                    probability = 1f,
                                    resultText = "Güzel bir hediye aldın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 18,
                                    probability = 1f,
                                    resultText = "Partnerin çok mutlu oldu!"
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
                                    minValue = -15,
                                    maxValue = -8,
                                    probability = 1f,
                                    resultText = "Yıl dönümünü unuttun! Partnerin çok kırıldı."
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
                    id = "job_interview",
                    title = "İş Görüşmesi",
                    description = "Hayalindeki iş için görüşmeye çağrıldın.",
                    ageRange = new AgeRange { min = 18, max = 60 },
                    category = EventCategory.Career,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "İyi hazırlan",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 0.6f,
                                    resultText = "Görüşme harika geçti! İşe alındın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.4f,
                                    resultText = "Maalesef bu sefer olmadı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Şansına güven",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 0.3f,
                                    resultText = "Şansın yaver gitti! İşe alındın!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 0.7f,
                                    resultText = "Hazırlıksız yakalandın. İşi kaçırdın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "work_conflict",
                    title = "İş Arkadaşı Sorunu",
                    description = "Bir iş arkadaşınla anlaşmazlık yaşıyorsun.",
                    ageRange = new AgeRange { min = 18, max = 65 },
                    category = EventCategory.Career,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yöneticiye bildir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.5f,
                                    resultText = "Yönetici sorunu çözdü."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.5f,
                                    resultText = "İş arkadaşın sana daha çok kızdı."
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
                                    minValue = 5,
                                    maxValue = 12,
                                    probability = 0.6f,
                                    resultText = "Konuştunuz ve anlaştınız."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 0.4f,
                                    resultText = "Konuşma kavgaya dönüştü."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "promotion_opportunity",
                    title = "Terfi Fırsatı",
                    description = "Şirkette bir pozisyon açıldı ve sen adaysın.",
                    ageRange = new AgeRange { min = 22, max = 60 },
                    category = EventCategory.Career,
                    probability = 0.3f,
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
                                    minValue = 2000,
                                    maxValue = 5000,
                                    probability = 0.5f,
                                    resultText = "Terfi aldın! Maaşın arttı!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.5f,
                                    resultText = "Başka biri tercih edildi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Mevcut pozisyonda kal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Rahatına baktın. Fırsat başkasına gitti."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "business_trip",
                    title = "İş Seyahati",
                    description = "Yurt dışına iş seyahati fırsatı çıktı.",
                    ageRange = new AgeRange { min = 22, max = 55 },
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
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 0.7f,
                                    resultText = "Harika bir seyahat oldu! Yeni deneyimler kazandın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Jet lag'den çok yoruldun."
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
                                    resultText = "Evde kalmayı tercih ettin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "startup_idea",
                    title = "Girişim Fikri",
                    description = "Harika bir iş fikrin var.",
                    ageRange = new AgeRange { min = 20, max = 50 },
                    category = EventCategory.Career,
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
                                    maxValue = 50000,
                                    probability = 0.3f,
                                    resultText = "Girişimin tuttu! Büyük kar elde ettin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -10000,
                                    maxValue = -5000,
                                    probability = 0.7f,
                                    resultText = "Girişim başarısız oldu. Para kaybettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Riskten kaçın",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Güvenli yolu seçtin. Fikir rafa kalktı."
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
                    id = "food_poisoning",
                    title = "Gıda Zehirlenmesi",
                    description = "Dışarıda yediğin yemekten zehirlendin.",
                    ageRange = new AgeRange { min = 5, max = 100 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Hastaneye git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -500,
                                    maxValue = -200,
                                    probability = 1f,
                                    resultText = "Tedavi oldun."
                                },
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
                            text = "Evde dinlen",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Evde dinlendin ama uzun sürdü."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -20,
                                    maxValue = -15,
                                    probability = 0.4f,
                                    resultText = "Durum kötüleşti. Hastaneye gitmek zorunda kaldın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "sports_injury",
                    title = "Spor Yaralanması",
                    description = "Spor yaparken sakatlandın.",
                    ageRange = new AgeRange { min = 10, max = 60 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Fizyoterapi al",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -1000,
                                    maxValue = -500,
                                    probability = 1f,
                                    resultText = "Tedavi gördün."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Düzgün iyileştin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kendi kendine iyileş",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -15,
                                    maxValue = -8,
                                    probability = 1f,
                                    resultText = "Yaralanma tam iyileşmedi. Kalıcı hasar kaldı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "mental_health",
                    title = "Ruh Sağlığı",
                    description = "Son zamanlarda kendini çok stresli hissediyorsun.",
                    ageRange = new AgeRange { min = 15, max = 80 },
                    category = EventCategory.Health,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Terapiste git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -300,
                                    maxValue = -100,
                                    probability = 1f,
                                    resultText = "Terapi seansı aldın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "Konuşmak iyi geldi. Daha iyi hissediyorsun."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Kendi başına çöz",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = 3,
                                    probability = 1f,
                                    resultText = "Kendi başına mücadele ettin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "healthy_lifestyle",
                    title = "Sağlıklı Yaşam",
                    description = "Sağlıklı yaşam tarzı benimsemeye karar verdin.",
                    ageRange = new AgeRange { min = 18, max = 70 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Diyet ve egzersiz",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 0.7f,
                                    resultText = "Disiplinli oldun! Sağlığın önemli ölçüde iyileşti."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 0.3f,
                                    resultText = "Biraz gevşedin ama yine de faydasını gördün."
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
                                    resultText = "Eski alışkanlıklarına döndün."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "dental_problem",
                    title = "Diş Problemi",
                    description = "Dişin ağrıyor ve diş hekimine gitmen gerekiyor.",
                    ageRange = new AgeRange { min = 8, max = 90 },
                    category = EventCategory.Health,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Hemen git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -400,
                                    maxValue = -150,
                                    probability = 1f,
                                    resultText = "Diş tedavisi yaptırdın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Ağrı geçti. Daha iyi hissediyorsun."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Ertele",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 1f,
                                    resultText = "Ağrı arttı ve daha pahalı tedavi gerekti."
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
                    id = "social_media_drama",
                    title = "Sosyal Medya Dramı",
                    description = "Sosyal medyada tartışmalı bir paylaşım yaptın.",
                    ageRange = new AgeRange { min = 13, max = 50 },
                    category = EventCategory.Social,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sil ve özür dile",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Özür diledin ve unutuldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Arkasında dur",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.4f,
                                    resultText = "Destekçiler buldun! Şöhretin arttı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 0.6f,
                                    resultText = "Çok eleştiri aldın. Linç edildin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "neighborhood_help",
                    title = "Komşuya Yardım",
                    description = "Yaşlı komşun yardım istiyor.",
                    ageRange = new AgeRange { min = 12, max = 80 },
                    category = EventCategory.Social,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yardım et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 12,
                                    probability = 1f,
                                    resultText = "Komşuna yardım ettin. Kendini iyi hissediyorsun."
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
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Komşun üzüldü. Biraz suçlu hissediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "volunteer_work",
                    title = "Gönüllü Çalışma",
                    description = "Bir hayır kurumunda gönüllü olma fırsatı var.",
                    ageRange = new AgeRange { min = 16, max = 70 },
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
                                    minValue = 8,
                                    maxValue = 15,
                                    probability = 1f,
                                    resultText = "Gönüllü çalıştın. Çok mutlu hissediyorsun."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "İnsanlar seni takdir etti."
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
                                    resultText = "Başka zaman belki."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "friend_betrayal",
                    title = "Arkadaş İhaneti",
                    description = "Yakın arkadaşın sırrını başkalarına söylemiş.",
                    ageRange = new AgeRange { min = 12, max = 60 },
                    category = EventCategory.Social,
                    probability = 0.3f,
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
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 0.5f,
                                    resultText = "Arkadaşın özür diledi. Barıştınız."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 0.5f,
                                    resultText = "Arkadaşlığınız bitti."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İçine at",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 1f,
                                    resultText = "İçine attın ama çok mutsuz hissediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "reunion",
                    title = "Eski Arkadaş Buluşması",
                    description = "Yıllardır görmediğin eski bir arkadaşınla karşılaştın.",
                    ageRange = new AgeRange { min = 20, max = 80 },
                    category = EventCategory.Social,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sohbet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 12,
                                    probability = 0.8f,
                                    resultText = "Eski günleri andınız. Harika bir sohbet oldu."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.2f,
                                    resultText = "Artık pek ortak noktanız yok. Garip bir sohbet oldu."
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
                                    resultText = "Fark etmemiş gibi yaptın."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Financial Events

        private static List<GameEvent> GetFinancialEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "unexpected_bill",
                    title = "Beklenmedik Fatura",
                    description = "Büyük bir fatura geldi.",
                    ageRange = new AgeRange { min = 18, max = 90 },
                    category = EventCategory.Financial,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Hemen öde",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -2000,
                                    maxValue = -500,
                                    probability = 1f,
                                    resultText = "Faturayı ödedin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Ertele",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -3000,
                                    maxValue = -1000,
                                    probability = 1f,
                                    resultText = "Gecikme faizi eklendi. Daha fazla ödedin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "investment_tip",
                    title = "Yatırım Tüyosu",
                    description = "Bir arkadaşın sana yatırım tüyosu verdi.",
                    ageRange = new AgeRange { min = 20, max = 70 },
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
                                    minValue = 5000,
                                    maxValue = 20000,
                                    probability = 0.4f,
                                    resultText = "Tüyo tuttu! Kar elde ettin!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -1000,
                                    probability = 0.6f,
                                    resultText = "Tüyo yanlıştı. Para kaybettin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Güvenme",
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
                    id = "inheritance",
                    title = "Miras",
                    description = "Uzak bir akrabandan miras kaldı.",
                    ageRange = new AgeRange { min = 25, max = 80 },
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
                                    minValue = 10000,
                                    maxValue = 100000,
                                    probability = 0.7f,
                                    resultText = "Miras hesabına yattı!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 1000,
                                    maxValue = 5000,
                                    probability = 0.3f,
                                    resultText = "Miras borçluymuş. Sadece küçük bir miktar kaldı."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "wallet_lost",
                    title = "Kayıp Cüzdan",
                    description = "Cüzdanını kaybettin!",
                    ageRange = new AgeRange { min = 12, max = 80 },
                    category = EventCategory.Financial,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Ara",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.3f,
                                    resultText = "Cüzdanını buldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -1000,
                                    maxValue = -200,
                                    probability = 0.7f,
                                    resultText = "Cüzdanı bulamadın. Para kaybettin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "tax_refund",
                    title = "Vergi İadesi",
                    description = "Vergi iadesi aldın!",
                    ageRange = new AgeRange { min = 22, max = 70 },
                    category = EventCategory.Financial,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Biriktir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 1000,
                                    maxValue = 5000,
                                    probability = 1f,
                                    resultText = "Vergi iadeni biriktirdin."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Harca",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 12,
                                    probability = 1f,
                                    resultText = "Kendine güzel bir şeyler aldın!"
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
                    id = "family_reunion",
                    title = "Aile Toplantısı",
                    description = "Bayramda aile toplantısı var.",
                    ageRange = new AgeRange { min = 5, max = 90 },
                    category = EventCategory.Family,
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
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 15,
                                    probability = 0.7f,
                                    resultText = "Aile ile güzel vakit geçirdin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Aile içi tartışmalar oldu."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Mazeret bul",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Aile üzüldü ama sen rahat ettin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "parent_sick",
                    title = "Hasta Ebeveyn",
                    description = "Ebeveynin ciddi şekilde hastalandı.",
                    ageRange = new AgeRange { min = 30, max = 70 },
                    category = EventCategory.Family,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Yanında ol",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Ebeveyninin yanında oldun. Zor ama doğru bir karardı."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Uzaktan destek",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -10,
                                    maxValue = -5,
                                    probability = 1f,
                                    resultText = "Yanında olamadın. Suçluluk hissediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "sibling_fight",
                    title = "Kardeş Kavgası",
                    description = "Kardeşinle ciddi bir tartışma yaşadın.",
                    ageRange = new AgeRange { min = 5, max = 60 },
                    category = EventCategory.Family,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Barış",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 5,
                                    maxValue = 10,
                                    probability = 0.7f,
                                    resultText = "Barıştınız. İlişkiniz güçlendi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 0.3f,
                                    resultText = "Kardeşin hala kızgın."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Küs kal",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -8,
                                    maxValue = -3,
                                    probability = 1f,
                                    resultText = "Küs kaldınız. Aile ortamı gergin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "baby_born",
                    title = "Bebek Doğdu",
                    description = "Çocuğun dünyaya geldi!",
                    ageRange = new AgeRange { min = 22, max = 45 },
                    category = EventCategory.Family,
                    probability = 0.2f,
                    conditions = new List<EventCondition>
                    {
                        new EventCondition { type = ConditionType.IsMarried, targetValue = 1 }
                    },
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kutla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 20,
                                    maxValue = 35,
                                    probability = 1f,
                                    resultText = "Hayatının en mutlu günlerinden biri!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "family_trip",
                    title = "Aile Tatili",
                    description = "Aile ile tatile gitme planı var.",
                    ageRange = new AgeRange { min = 5, max = 70 },
                    category = EventCategory.Family,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Git",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -3000,
                                    maxValue = -1000,
                                    probability = 1f,
                                    resultText = "Tatil masrafları ödendi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 1f,
                                    resultText = "Harika bir aile tatili geçirdiniz!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "İptal et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 1f,
                                    resultText = "Aile üzüldü."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Crime Events

        private static List<GameEvent> GetCrimeEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "witness_crime",
                    title = "Suça Tanık",
                    description = "Sokakta bir hırsızlığa tanık oldun.",
                    ageRange = new AgeRange { min = 15, max = 70 },
                    category = EventCategory.Crime,
                    probability = 0.2f,
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
                                    maxValue = 10,
                                    probability = 0.7f,
                                    resultText = "Hırsız yakalandı. Doğru olanı yaptın."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Health",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.3f,
                                    resultText = "Hırsız kaçarken sana çarptı."
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
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -3,
                                    maxValue = -1,
                                    probability = 1f,
                                    resultText = "Bir şey yapmadın. Biraz suçlu hissediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "car_theft",
                    title = "Araba Hırsızlığı",
                    description = "Araban çalındı!",
                    ageRange = new AgeRange { min = 18, max = 80 },
                    category = EventCategory.Crime,
                    probability = 0.15f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Polise bildir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = 10000,
                                    maxValue = 30000,
                                    probability = 0.3f,
                                    resultText = "Araba bulundu! Sigorta hasarı karşıladı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -15,
                                    maxValue = -8,
                                    probability = 0.7f,
                                    resultText = "Araba bulunamadı. Büyük kayıp."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "scam_victim",
                    title = "Dolandırıcılık Kurbanı",
                    description = "İnternette dolandırıldın!",
                    ageRange = new AgeRange { min = 15, max = 80 },
                    category = EventCategory.Crime,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Şikayet et",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -2000,
                                    maxValue = -500,
                                    probability = 1f,
                                    resultText = "Para geri alınamadı ama şikayet edildi."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Unut gitsin",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -2000,
                                    maxValue = -500,
                                    probability = 1f,
                                    resultText = "Para kayboldu. Ders aldın."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "illegal_download",
                    title = "Korsan İndirme",
                    description = "İnternetten korsan içerik indirdin ve uyarı aldın.",
                    ageRange = new AgeRange { min = 12, max = 50 },
                    category = EventCategory.Crime,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sil ve özür dile",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "İçerikleri sildin. Uyarı kaldırıldı."
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
                                    type = OutcomeType.MoneyChange,
                                    minValue = -5000,
                                    maxValue = -1000,
                                    probability = 0.5f,
                                    resultText = "Dava açıldı. Ceza ödedin."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 0.5f,
                                    resultText = "Şanslısın, bir şey olmadı."
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Milestone Events

        private static List<GameEvent> GetMilestoneEvents()
        {
            return new List<GameEvent>
            {
                new GameEvent
                {
                    id = "graduation",
                    title = "Mezuniyet",
                    description = "Bugün mezuniyet günün!",
                    ageRange = new AgeRange { min = 17, max = 25 },
                    category = EventCategory.Milestone,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kutla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 15,
                                    maxValue = 25,
                                    probability = 1f,
                                    resultText = "Mezun oldun! Yeni bir sayfa açılıyor."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Fame",
                                    minValue = 2,
                                    maxValue = 5,
                                    probability = 1f,
                                    resultText = "Herkes seni kutladı!"
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "big_birthday",
                    title = "Önemli Doğum Günü",
                    description = "Bugün büyük bir doğum günü kutluyorsun!",
                    ageRange = new AgeRange { min = 18, max = 100 },
                    category = EventCategory.Milestone,
                    probability = 0.4f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Parti ver",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.MoneyChange,
                                    minValue = -1000,
                                    maxValue = -300,
                                    probability = 1f,
                                    resultText = "Parti masrafları ödendi."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 20,
                                    probability = 1f,
                                    resultText = "Harika bir parti oldu!"
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Sessiz kutla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 3,
                                    maxValue = 8,
                                    probability = 1f,
                                    resultText = "Huzurlu bir doğum günü geçirdin."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "driving_license",
                    title = "Ehliyet Sınavı",
                    description = "Ehliyet sınavına giriyorsun.",
                    ageRange = new AgeRange { min = 18, max = 30 },
                    category = EventCategory.Milestone,
                    probability = 0.3f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Sınava gir",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 10,
                                    maxValue = 18,
                                    probability = 0.6f,
                                    resultText = "Sınavı geçtin! Ehliyet sahibi oldun!"
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = -2,
                                    probability = 0.4f,
                                    resultText = "Sınavda kaldın. Tekrar denemen gerek."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "retirement",
                    title = "Emeklilik",
                    description = "Emekliye ayrılma zamanı geldi.",
                    ageRange = new AgeRange { min = 60, max = 70 },
                    category = EventCategory.Milestone,
                    probability = 0.5f,
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
                                    maxValue = 20,
                                    probability = 0.7f,
                                    resultText = "Emekli oldun! Artık dinlenme zamanı."
                                },
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = -5,
                                    maxValue = 5,
                                    probability = 0.3f,
                                    resultText = "Emekli oldun ama işi özlüyorsun."
                                }
                            }
                        },
                        new EventChoice
                        {
                            text = "Çalışmaya devam",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.None,
                                    probability = 1f,
                                    resultText = "Çalışmaya devam ediyorsun."
                                }
                            }
                        }
                    }
                },
                new GameEvent
                {
                    id = "first_home",
                    title = "İlk Ev",
                    description = "İlk evini satın aldın!",
                    ageRange = new AgeRange { min = 25, max = 50 },
                    category = EventCategory.Milestone,
                    probability = 0.2f,
                    choices = new List<EventChoice>
                    {
                        new EventChoice
                        {
                            text = "Kutla",
                            outcomes = new List<EventOutcome>
                            {
                                new EventOutcome
                                {
                                    type = OutcomeType.StatChange,
                                    targetStat = "Happiness",
                                    minValue = 20,
                                    maxValue = 30,
                                    probability = 1f,
                                    resultText = "Ev sahibi oldun! Büyük bir başarı!"
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
