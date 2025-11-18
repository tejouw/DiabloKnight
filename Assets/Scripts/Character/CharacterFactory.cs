using UnityEngine;
using System;
using TurkishLifeSim.Managers;

namespace TurkishLifeSim.Character
{
    /// <summary>
    /// Karakter fabrikası - Yeni karakterler oluşturur.
    /// </summary>
    public static class CharacterFactory
    {
        /// <summary>
        /// Yeni rastgele karakter oluştur.
        /// </summary>
        public static CharacterData CreateNewCharacter()
        {
            var dataManager = DataManager.Instance;
            var character = new CharacterData();

            // Cinsiyet belirle (50/50)
            character.gender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;

            // İsim ata
            if (character.gender == Gender.Male)
            {
                character.firstName = dataManager.GetRandomMaleName();
            }
            else
            {
                character.firstName = dataManager.GetRandomFemaleName();
            }
            character.lastName = dataManager.GetRandomSurname();

            // Doğum bilgileri
            character.age = 0;
            character.birthDate = DateTime.Now.ToString("dd/MM/yyyy");
            character.birthCity = dataManager.GetRandomCity().name;

            // Başlangıç statları (biraz rastgelelik ile)
            character.stats.health = UnityEngine.Random.Range(80, 100);
            character.stats.happiness = UnityEngine.Random.Range(60, 90);
            character.stats.intelligence = UnityEngine.Random.Range(30, 70);
            character.stats.appearance = UnityEngine.Random.Range(30, 70);
            character.stats.fame = 0;

            // Başlangıç parası (aile durumuna göre)
            int familyWealth = UnityEngine.Random.Range(0, 100);
            if (familyWealth < 30)
            {
                // Dar gelirli aile
                character.finances.currentMoney = 0;
            }
            else if (familyWealth < 70)
            {
                // Orta gelirli aile
                character.finances.currentMoney = UnityEngine.Random.Range(1000, 5000);
            }
            else
            {
                // Varlıklı aile
                character.finances.currentMoney = UnityEngine.Random.Range(10000, 50000);
            }

            // Aileyi oluştur
            CreateFamily(character);

            Debug.Log($"[CharacterFactory] Created new character: {character.FullName} from {character.birthCity}");

            return character;
        }

        /// <summary>
        /// Belirli parametrelerle karakter oluştur.
        /// </summary>
        public static CharacterData CreateCharacter(string firstName, string lastName, Gender gender, string city)
        {
            var character = new CharacterData();

            character.firstName = firstName;
            character.lastName = lastName;
            character.gender = gender;
            character.age = 0;
            character.birthDate = DateTime.Now.ToString("dd/MM/yyyy");
            character.birthCity = city;

            // Varsayılan statlar
            character.stats.health = 100;
            character.stats.happiness = 75;
            character.stats.intelligence = 50;
            character.stats.appearance = 50;
            character.stats.fame = 0;

            // Aileyi oluştur
            CreateFamily(character);

            return character;
        }

        /// <summary>
        /// Aile üyelerini oluştur.
        /// </summary>
        private static void CreateFamily(CharacterData character)
        {
            var dataManager = DataManager.Instance;

            // Anne - karakter yaşına göre en az 18 yaş büyük olmalı
            int motherAge = character.age + UnityEngine.Random.Range(18, 35);
            var mother = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{dataManager.GetRandomFemaleName()} {character.lastName}",
                type = RelationType.Parent,
                gender = Gender.Female,
                age = motherAge,
                intimacy = UnityEngine.Random.Range(70, 100),
                trust = UnityEngine.Random.Range(70, 100),
                status = RelationshipStatus.Active
            };
            character.relationships.Add(mother);

            // Baba - anneden biraz büyük olabilir
            int fatherAge = motherAge + UnityEngine.Random.Range(0, 8);
            var father = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{dataManager.GetRandomMaleName()} {character.lastName}",
                type = RelationType.Parent,
                gender = Gender.Male,
                age = fatherAge,
                intimacy = UnityEngine.Random.Range(60, 95),
                trust = UnityEngine.Random.Range(60, 95),
                status = RelationshipStatus.Active
            };
            character.relationships.Add(father);

            // Kardeş şansı (%60)
            if (UnityEngine.Random.value < 0.6f)
            {
                int siblingCount = UnityEngine.Random.Range(1, 4);

                for (int i = 0; i < siblingCount; i++)
                {
                    Gender siblingGender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
                    string siblingName = siblingGender == Gender.Male
                        ? dataManager.GetRandomMaleName()
                        : dataManager.GetRandomFemaleName();

                    // Kardeş yaşı: karakter yaşından 10 yaş küçük ile 15 yaş büyük arası
                    int siblingAge = character.age + UnityEngine.Random.Range(-10, 16);
                    siblingAge = Mathf.Max(0, siblingAge); // Negatif yaş olamaz

                    var sibling = new Relationship
                    {
                        npcId = Guid.NewGuid().ToString(),
                        npcName = $"{siblingName} {character.lastName}",
                        type = RelationType.Sibling,
                        gender = siblingGender,
                        age = siblingAge,
                        intimacy = UnityEngine.Random.Range(40, 90),
                        trust = UnityEngine.Random.Range(40, 90),
                        status = RelationshipStatus.Active
                    };
                    character.relationships.Add(sibling);
                }
            }
        }

        /// <summary>
        /// NPC oluştur (arkadaş, sevgili vb. için).
        /// </summary>
        public static Relationship CreateNPC(RelationType type, Gender gender, int age, string lastName = null)
        {
            var dataManager = DataManager.Instance;

            string firstName = gender == Gender.Male
                ? dataManager.GetRandomMaleName()
                : dataManager.GetRandomFemaleName();

            string surname = lastName ?? dataManager.GetRandomSurname();

            return new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{firstName} {surname}",
                type = type,
                gender = gender,
                age = age,
                intimacy = UnityEngine.Random.Range(20, 50),
                trust = UnityEngine.Random.Range(20, 50),
                status = RelationshipStatus.Active
            };
        }

        /// <summary>
        /// Rastgele arkadaş oluştur.
        /// </summary>
        public static Relationship CreateRandomFriend(int characterAge)
        {
            Gender gender = UnityEngine.Random.value < 0.5f ? Gender.Male : Gender.Female;
            int age = characterAge + UnityEngine.Random.Range(-3, 4);
            age = Mathf.Max(1, age);

            return CreateNPC(RelationType.Friend, gender, age);
        }

        /// <summary>
        /// Sevgi ilgisi oluştur.
        /// </summary>
        public static Relationship CreateLoveInterest(int characterAge, Gender preferredGender)
        {
            int age = characterAge + UnityEngine.Random.Range(-5, 6);
            age = Mathf.Max(16, age);

            RelationType type = preferredGender == Gender.Male
                ? RelationType.Boyfriend
                : RelationType.Girlfriend;

            return CreateNPC(type, preferredGender, age);
        }
    }
}
