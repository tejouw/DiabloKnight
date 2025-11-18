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

            // Anne
            var mother = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{dataManager.GetRandomFemaleName()} {character.lastName}",
                type = RelationType.Parent,
                gender = Gender.Female,
                age = UnityEngine.Random.Range(22, 40),
                intimacy = UnityEngine.Random.Range(70, 100),
                trust = UnityEngine.Random.Range(70, 100),
                status = RelationshipStatus.Active
            };
            character.relationships.Add(mother);

            // Baba
            var father = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = $"{dataManager.GetRandomMaleName()} {character.lastName}",
                type = RelationType.Parent,
                gender = Gender.Male,
                age = UnityEngine.Random.Range(24, 45),
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

                    var sibling = new Relationship
                    {
                        npcId = Guid.NewGuid().ToString(),
                        npcName = $"{siblingName} {character.lastName}",
                        type = RelationType.Sibling,
                        gender = siblingGender,
                        age = UnityEngine.Random.Range(0, 15),
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

        /// <summary>
        /// Çocuk olarak devam et - Önceki karakterin çocuğundan yeni karakter oluştur.
        /// </summary>
        /// <param name="previousCharacter">Ölen karakter</param>
        /// <param name="childRelationship">Devam edilecek çocuk</param>
        /// <param name="inheritedMoney">Miras olarak alınan para</param>
        public static CharacterData CreateCharacterFromChild(
            CharacterData previousCharacter,
            Relationship childRelationship,
            decimal inheritedMoney)
        {
            var dataManager = DataManager.Instance;
            var character = new CharacterData();

            // Temel bilgiler - çocuktan al
            string[] nameParts = childRelationship.npcName.Split(' ');
            character.firstName = nameParts[0];
            character.lastName = previousCharacter.lastName; // Aile soyadını koru
            character.gender = childRelationship.gender;
            character.age = childRelationship.age;
            character.birthCity = previousCharacter.birthCity;

            // Doğum tarihini hesapla (şimdiki yıldan yaşı çıkar)
            int birthYear = DateTime.Now.Year - character.age;
            character.birthDate = $"01/01/{birthYear}";

            // Başlangıç statları - genetik etkisi ile
            int geneticBonus = UnityEngine.Random.Range(-10, 15);
            character.stats.health = Mathf.Clamp(80 + geneticBonus, 50, 100);
            character.stats.happiness = Mathf.Clamp(60 + UnityEngine.Random.Range(-20, 20), 30, 100);

            // Zeka ve görünüş - ebeveynden kalıtım
            int parentIntelligence = previousCharacter.stats.intelligence;
            int parentAppearance = previousCharacter.stats.appearance;
            character.stats.intelligence = Mathf.Clamp(
                (parentIntelligence / 2) + UnityEngine.Random.Range(20, 50),
                20, 100);
            character.stats.appearance = Mathf.Clamp(
                (parentAppearance / 2) + UnityEngine.Random.Range(20, 50),
                20, 100);
            character.stats.fame = 0;

            // Miras parası
            character.finances.currentMoney = inheritedMoney;
            character.finances.totalEarned = inheritedMoney;

            // Varlıkları devral (varsa)
            if (previousCharacter.finances.assets != null)
            {
                character.finances.assets = new System.Collections.Generic.List<string>(previousCharacter.finances.assets);
            }

            // Nesil bilgilerini güncelle
            character.legacy.generation = previousCharacter.legacy.generation + 1;
            character.legacy.familyName = previousCharacter.lastName;
            character.legacy.previousCharacterId = previousCharacter.firstName + "_" + previousCharacter.Age;
            character.legacy.totalFamilyWealth = previousCharacter.legacy.totalFamilyWealth + previousCharacter.finances.currentMoney;
            character.legacy.totalFamilyFame = previousCharacter.legacy.totalFamilyFame + previousCharacter.stats.fame;

            // Aile tarihini kopyala ve önceki karakteri ekle
            character.legacy.familyHistory = new System.Collections.Generic.List<LegacyRecord>(previousCharacter.legacy.familyHistory);

            // Önceki karakteri aile tarihine ekle
            var previousRecord = new LegacyRecord
            {
                characterId = previousCharacter.firstName + "_" + previousCharacter.Age,
                characterName = previousCharacter.FullName,
                birthYear = DateTime.Now.Year - previousCharacter.Age,
                deathYear = DateTime.Now.Year,
                generation = previousCharacter.legacy.generation,
                deathCause = "Doğal sebepler",
                finalWealth = previousCharacter.finances.currentMoney,
                finalFame = previousCharacter.stats.fame,
                notableAchievement = DetermineNotableAchievement(previousCharacter),
                childrenNames = GetChildrenNames(previousCharacter)
            };
            character.legacy.familyHistory.Add(previousRecord);

            // İlk ata ID'sini koru veya ayarla
            if (string.IsNullOrEmpty(previousCharacter.legacy.originalAncestorId))
            {
                character.legacy.originalAncestorId = previousRecord.characterId;
            }
            else
            {
                character.legacy.originalAncestorId = previousCharacter.legacy.originalAncestorId;
            }

            // İlişkileri oluştur
            CreateRelationshipsForNewGeneration(character, previousCharacter, childRelationship);

            // Eğitim durumunu yaşa göre ayarla
            SetEducationForAge(character);

            Debug.Log($"[CharacterFactory] Created new generation character: {character.FullName}, " +
                      $"Generation {character.legacy.generation}, Age {character.age}, " +
                      $"Inherited {inheritedMoney:N0} TL");

            return character;
        }

        /// <summary>
        /// Yeni nesil için ilişkileri oluştur.
        /// </summary>
        private static void CreateRelationshipsForNewGeneration(
            CharacterData newCharacter,
            CharacterData previousCharacter,
            Relationship childRelationship)
        {
            var dataManager = DataManager.Instance;

            // Ölen ebeveyni ekle (Deceased olarak)
            var deceasedParent = new Relationship
            {
                npcId = Guid.NewGuid().ToString(),
                npcName = previousCharacter.FullName,
                type = RelationType.Parent,
                gender = previousCharacter.gender,
                age = previousCharacter.Age,
                intimacy = 80,
                trust = 80,
                status = RelationshipStatus.Deceased
            };
            newCharacter.relationships.Add(deceasedParent);

            // Diğer ebeveyni bul ve ekle (önceki karakterin eşi)
            var spouse = previousCharacter.relationships.Find(r => r.type == RelationType.Spouse && r.status == RelationshipStatus.Active);
            if (spouse != null)
            {
                var otherParent = new Relationship
                {
                    npcId = spouse.npcId,
                    npcName = spouse.npcName,
                    type = RelationType.Parent,
                    gender = spouse.gender,
                    age = spouse.age + 1, // Yaşı 1 artır (zaman geçti)
                    intimacy = UnityEngine.Random.Range(60, 90),
                    trust = UnityEngine.Random.Range(60, 90),
                    status = RelationshipStatus.Active
                };
                newCharacter.relationships.Add(otherParent);
            }
            else
            {
                // Eş yoksa rastgele bir ebeveyn oluştur
                Gender otherParentGender = previousCharacter.gender == Gender.Male ? Gender.Female : Gender.Male;
                var otherParent = new Relationship
                {
                    npcId = Guid.NewGuid().ToString(),
                    npcName = otherParentGender == Gender.Male
                        ? $"{dataManager.GetRandomMaleName()} {newCharacter.lastName}"
                        : $"{dataManager.GetRandomFemaleName()} {newCharacter.lastName}",
                    type = RelationType.Parent,
                    gender = otherParentGender,
                    age = previousCharacter.Age + UnityEngine.Random.Range(-5, 5),
                    intimacy = UnityEngine.Random.Range(50, 80),
                    trust = UnityEngine.Random.Range(50, 80),
                    status = RelationshipStatus.Active
                };
                newCharacter.relationships.Add(otherParent);
            }

            // Kardeşleri ekle (önceki karakterin diğer çocukları)
            foreach (var rel in previousCharacter.relationships)
            {
                if (rel.type == RelationType.Child && rel.npcId != childRelationship.npcId && rel.status == RelationshipStatus.Active)
                {
                    var sibling = new Relationship
                    {
                        npcId = rel.npcId,
                        npcName = rel.npcName,
                        type = RelationType.Sibling,
                        gender = rel.gender,
                        age = rel.age,
                        intimacy = UnityEngine.Random.Range(50, 85),
                        trust = UnityEngine.Random.Range(50, 85),
                        status = RelationshipStatus.Active
                    };
                    newCharacter.relationships.Add(sibling);
                }
            }
        }

        /// <summary>
        /// Yaşa göre eğitim durumunu ayarla.
        /// </summary>
        private static void SetEducationForAge(CharacterData character)
        {
            int age = character.age;

            if (age < 7)
            {
                character.education.currentLevel = EducationLevel.None;
            }
            else if (age < 11)
            {
                character.education.currentLevel = EducationLevel.PrimarySchool;
                character.education.schoolName = "İlkokul";
            }
            else if (age < 15)
            {
                character.education.currentLevel = EducationLevel.MiddleSchool;
                character.education.schoolName = "Ortaokul";
            }
            else if (age < 19)
            {
                character.education.currentLevel = EducationLevel.HighSchool;
                character.education.schoolName = "Lise";
            }
            else
            {
                // 19+ yaş için okul bitmiş varsayılır
                character.education.currentLevel = EducationLevel.HighSchool;
                character.education.isGraduated = true;
            }

            // GPA rastgele ata
            character.education.gpa = UnityEngine.Random.Range(2.0f, 4.0f);
        }

        /// <summary>
        /// Karakterin en önemli başarısını belirle.
        /// </summary>
        private static string DetermineNotableAchievement(CharacterData character)
        {
            if (character.stats.fame >= 80)
                return "Ünlü bir kişilik oldu";
            if (character.finances.currentMoney >= 1000000)
                return "Milyoner oldu";
            if (character.education.currentLevel == EducationLevel.Doctorate)
                return "Doktora derecesi aldı";
            if (character.education.currentLevel == EducationLevel.Masters)
                return "Yüksek lisans tamamladı";
            if (character.career.currentJob != null && character.career.yearsInJob >= 20)
                return $"{character.career.currentJob.title} olarak uzun yıllar çalıştı";
            if (character.stats.happiness >= 90)
                return "Mutlu bir hayat yaşadı";
            if (character.Age >= 90)
                return "Uzun ve dolu bir ömür sürdü";

            return "Ailesine bağlı bir hayat yaşadı";
        }

        /// <summary>
        /// Karakterin çocuklarının isimlerini al.
        /// </summary>
        private static System.Collections.Generic.List<string> GetChildrenNames(CharacterData character)
        {
            var names = new System.Collections.Generic.List<string>();
            foreach (var rel in character.relationships)
            {
                if (rel.type == RelationType.Child)
                {
                    names.Add(rel.npcName);
                }
            }
            return names;
        }
    }
}
