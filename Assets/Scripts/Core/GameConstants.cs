namespace TurkishLifeSim.Core
{
    /// <summary>
    /// Oyun sabitleri - Magic number'ları merkezi hale getirir.
    /// </summary>
    public static class GameConstants
    {
        #region Life Stages

        public const int BABY_MAX_AGE = 4;
        public const int CHILD_MIN_AGE = 5;
        public const int CHILD_MAX_AGE = 11;
        public const int TEEN_MIN_AGE = 12;
        public const int TEEN_MAX_AGE = 17;
        public const int YOUNG_ADULT_MIN_AGE = 18;
        public const int YOUNG_ADULT_MAX_AGE = 29;
        public const int ADULT_MIN_AGE = 30;
        public const int ADULT_MAX_AGE = 59;
        public const int SENIOR_MIN_AGE = 60;
        public const int MAX_AGE = 120;

        #endregion

        #region Stats

        public const int MIN_STAT_VALUE = 0;
        public const int MAX_STAT_VALUE = 100;
        public const int DEFAULT_STAT_VALUE = 50;
        public const int DEFAULT_HEALTH = 100;

        #endregion

        #region Probabilities

        public const float EVENT_PROBABILITY_LOW = 0.3f;
        public const float EVENT_PROBABILITY_MEDIUM = 0.5f;
        public const float EVENT_PROBABILITY_HIGH = 0.7f;
        public const float EVENT_PROBABILITY_VERY_HIGH = 0.9f;

        public const float SIBLING_CHANCE = 0.6f;
        public const float GENDER_MALE_CHANCE = 0.5f;

        #endregion

        #region Family

        public const int MOTHER_MIN_AGE = 22;
        public const int MOTHER_MAX_AGE = 40;
        public const int FATHER_MIN_AGE = 24;
        public const int FATHER_MAX_AGE = 45;
        public const int SIBLING_AGE_DIFF_MIN = -5;
        public const int SIBLING_AGE_DIFF_MAX = 10;
        public const int MAX_SIBLINGS = 3;

        public const int PARENT_MIN_INTIMACY = 60;
        public const int PARENT_MAX_INTIMACY = 100;
        public const int SIBLING_MIN_INTIMACY = 40;
        public const int SIBLING_MAX_INTIMACY = 90;

        #endregion

        #region Financial

        public const int POOR_FAMILY_THRESHOLD = 30;
        public const int MIDDLE_CLASS_THRESHOLD = 70;
        public const int POOR_FAMILY_MONEY = 0;
        public const int MIDDLE_CLASS_MIN_MONEY = 1000;
        public const int MIDDLE_CLASS_MAX_MONEY = 5000;
        public const int WEALTHY_MIN_MONEY = 10000;
        public const int WEALTHY_MAX_MONEY = 50000;

        #endregion

        #region Save System

        public const int MAX_SAVE_SLOTS = 5;
        public const int AUTO_SAVE_INTERVAL_SECONDS = 300;
        public const string SAVE_KEY_PREFIX = "TurkishLifeSim_Save_";
        public const string AUTO_SAVE_KEY = "TurkishLifeSim_AutoSave";

        #endregion

        #region Events

        public const int MAX_RECENT_EVENTS = 20;
        public const int MIN_MARRIAGE_AGE = 18;
        public const int MIN_DRIVING_AGE = 18;
        public const int MIN_WORK_AGE = 15;
        public const int MILITARY_AGE = 20;

        #endregion

        #region Health & Aging

        public const int APPEARANCE_DECAY_AGE = 40;
        public const int HEALTH_DECAY_AGE = 60;
        public const int NATURAL_DEATH_START_AGE = 70;
        public const float APPEARANCE_DECAY_CHANCE = 0.3f;
        public const int HEALTH_DECAY_MIN = 1;
        public const int HEALTH_DECAY_MAX = 3;

        #endregion

        #region Relationships

        public const int MAX_RELATIONSHIP_VALUE = 100;
        public const int MIN_RELATIONSHIP_VALUE = 0;
        public const int RELATIONSHIP_DECAY_PER_YEAR = 2;
        public const int FRIEND_INITIAL_MIN_INTIMACY = 20;
        public const int FRIEND_INITIAL_MAX_INTIMACY = 50;

        #endregion

        #region Crime & Prison

        public const int MIN_PRISON_AGE = 12;
        public const int ADULT_CRIME_AGE = 18;
        public const float CRIME_SUCCESS_BASE_CHANCE = 0.5f;
        public const float CRIME_DETECTION_BASE_CHANCE = 0.3f;

        #endregion

        #region Education

        public const int PRIMARY_SCHOOL_START_AGE = 6;
        public const int MIDDLE_SCHOOL_START_AGE = 10;
        public const int HIGH_SCHOOL_START_AGE = 14;
        public const int UNIVERSITY_START_AGE = 18;
        public const float MAX_GPA = 4.0f;

        #endregion

        #region UI

        public const int TARGET_FRAME_RATE = 60;
        public const float UI_ANIMATION_DURATION = 0.3f;
        public const float POPUP_FADE_DURATION = 0.2f;

        #endregion

        #region Assets

        public const decimal CAR_MIN_PRICE = 50000m;
        public const decimal CAR_MAX_PRICE = 5000000m;
        public const decimal HOUSE_MIN_PRICE = 500000m;
        public const decimal HOUSE_MAX_PRICE = 50000000m;
        public const float ASSET_DEPRECIATION_RATE = 0.1f;
        public const float PROPERTY_APPRECIATION_RATE = 0.05f;

        #endregion
    }
}
