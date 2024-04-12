using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public GameSettings gameSettings;

    [NonSerialized]
    public const string DEVICESETTINGFILENAME = "DeviceSetting.bin";

    [Serializable]
    public class GameSettings
    {
        public StoreType storeType = StoreType.None;
        [Space]

        [Title("Developer")]
        public bool loginNone = false;

        [Title("Agree")]
        public bool privacypolicy = false;

        [Space]
        [Title("Login")]
        public string playfabId = "";
        public string customId = "";
        public bool autoLogin = false;
        public bool isLogin = false;
        public LoginType login = LoginType.None;
        public string nickName = "";

        [Space]
        [Title("Language")]
        public LanguageType language = LanguageType.Default;

        [Space]
        [Title("Info")]
        public WindCharacterType windCharacterType = WindCharacterType.Winter;
        public GameRankType gameRankType = GameRankType.Bronze_4;
        public GameRankType playRankType = GameRankType.Bronze_4;

        [Space]
        [Title("InGame")]
        public GameType gameType = GameType.NewBie;
        public GameEventType gameEventType = GameEventType.GameEvent1;
        public string room = "";
        public int stakes = 0;
        public int penalty = 0;
        public bool playing = false;
        public bool reEnter = false;
        public int consumeGold = 0;
        public bool newbie = false; //맨 처음 몇 판 데미지 보정시키기

        [Space]
        [Title("Rank")]
        public int winStreak = 0;
        public int loseStreak = 0;
        public bool win = false;
        public bool lose = false;

        [Space]
        [Title("Setting")]
        public bool music = true;
        public bool sfx = true;
        public bool vibration = true;
        public bool sleepMode = false;
        public bool appReview = false;

        [Space]
        [Title("Betting")]
        public int matchingTime = 9;
        public int bettingTime = 9;
        public int bettingWaitTime = 3;
        public bool autoTarget = false;
        public int autoTargetNumber = 0;
        public bool blockOverlap = false;

        [Space]
        [Title("Bouns")]
        public bool checkBouns = false;

        [Space]
        [Title("Tutorial")]
        public bool tutorial = false;

    }
    #region Data

    public bool LoginNone
    {
        get
        {
            return gameSettings.loginNone;
        }
        set
        {
            gameSettings.loginNone = value;
            SaveFile();
        }
    }

    public bool PrivacyPolicy
    {
        get
        {
            return gameSettings.privacypolicy;
        }
        set
        {
            gameSettings.privacypolicy = value;
            SaveFile();
        }
    }

    public int BettingTime
    {
        get
        {
            return gameSettings.bettingTime;
        }
        set
        {
            gameSettings.bettingTime = value;
            SaveFile();
        }
    }

    public int BettingWaitTime
    {
        get
        {
            return gameSettings.bettingWaitTime;
        }
        set
        {
            gameSettings.bettingWaitTime = value;
            SaveFile();
        }
    }

    public bool CheckBouns
    {
        get
        {
            return gameSettings.checkBouns;
        }
        set
        {
            gameSettings.checkBouns = value;
            SaveFile();
        }
    }

    public bool AutoTarget
    {
        get
        {
            return gameSettings.autoTarget;
        }
        set
        {
            gameSettings.autoTarget = value;
            SaveFile();
        }
    }

    public int AutoTargetNumber
    {
        get
        {
            return gameSettings.autoTargetNumber;
        }
        set
        {
            gameSettings.autoTargetNumber = value;
            SaveFile();
        }
    }

    public bool BlockOverlap
    {
        get
        {
            return gameSettings.blockOverlap;
        }
        set
        {
            gameSettings.blockOverlap = value;
            SaveFile();
        }
    }

    public string PlayfabId
    {
        get
        {
            return gameSettings.playfabId;
        }
        set
        {
            gameSettings.playfabId = value;
            SaveFile();
        }
    }

    public string CustomId
    {
        get
        {
            return gameSettings.customId;
        }
        set
        {
            gameSettings.customId = value;
            SaveFile();
        }
    }

    public string NickName
    {
        get
        {
            return gameSettings.nickName;
        }
        set
        {
            gameSettings.nickName = value;
            SaveFile();
        }
    }

    public LanguageType Language
    {
        get
        {
            return gameSettings.language;
        }
        set
        {
            gameSettings.language = value;
            SaveFile();
        }
    }
    public bool AutoLogin
    {
        get
        {
            return gameSettings.autoLogin;
        }
        set
        {
            gameSettings.autoLogin = value;
            SaveFile();
        }
    }

    public bool IsLogin
    {
        get
        {
            return gameSettings.isLogin;
        }
        set
        {
            gameSettings.isLogin = value;
            SaveFile();
        }
    }

    public LoginType Login
    {
        get
        {
            return gameSettings.login;
        }
        set
        {
            gameSettings.login = value;
            SaveFile();
        }
    }

    public StoreType StoreType
    {
        get
        {
            return gameSettings.storeType;
        }
        set
        {
            gameSettings.storeType = value;
            SaveFile();
        }
    }

    public WindCharacterType WindCharacterType
    {
        get
        {
            return gameSettings.windCharacterType;
        }
        set
        {
            gameSettings.windCharacterType = value;
            SaveFile();
        }
    }

    public GameType GameType
    {
        get
        {
            return gameSettings.gameType;
        }
        set
        {
            gameSettings.gameType = value;
            SaveFile();
        }
    }

    public GameEventType GameEventType
    {
        get
        {
            return gameSettings.gameEventType;
        }
        set
        {
            gameSettings.gameEventType = value;
            SaveFile();
        }
    }

    public GameRankType GameRankType
    {
        get
        {
            return gameSettings.gameRankType;
        }
        set
        {
            gameSettings.gameRankType = value;
            SaveFile();
        }
    }

    public GameRankType PlayRankType
    {
        get
        {
            return gameSettings.playRankType;
        }
        set
        {
            gameSettings.playRankType = value;
            SaveFile();
        }
    }

    public bool Playing
    {
        get
        {
            return gameSettings.playing;
        }
        set
        {
            gameSettings.playing = value;
            SaveFile();
        }
    }

    public bool ReEnter
    {
        get
        {
            return gameSettings.reEnter;
        }
        set
        {
            gameSettings.reEnter = value;
            SaveFile();
        }
    }

    public int ConsumeGold
    {
        get
        {
            return gameSettings.consumeGold;
        }
        set
        {
            gameSettings.consumeGold = value;
            SaveFile();
        }
    }

    public bool Newbie
    {
        get
        {
            return gameSettings.newbie;
        }
        set
        {
            gameSettings.newbie = value;
            SaveFile();
        }
    }

    public string Room
    {
        get
        {
            return gameSettings.room;
        }
        set
        {
            gameSettings.room = value;
            SaveFile();
        }
    }

    public int MatchingTime
    {
        get
        {
            return gameSettings.matchingTime;
        }
        set
        {
            gameSettings.matchingTime = value;
            SaveFile();
        }
    }

    public int Stakes
    {
        get
        {
            return gameSettings.stakes;
        }
        set
        {
            gameSettings.stakes = value;
            SaveFile();
        }
    }

    public int Penalty
    {
        get
        {
            return gameSettings.penalty;
        }
        set
        {
            gameSettings.penalty = value;
            SaveFile();
        }
    }

    public int WinStreak
    {
        get
        {
            return gameSettings.winStreak;
        }
        set
        {
            gameSettings.winStreak = value;
            SaveFile();
        }
    }

    public int LoseStreak
    {
        get
        {
            return gameSettings.loseStreak;
        }
        set
        {
            gameSettings.loseStreak = value;
            SaveFile();
        }
    }

    public bool Win
    {
        get
        {
            return gameSettings.win;
        }
        set
        {
            gameSettings.win = value;
            SaveFile();
        }
    }

    public bool Lose
    {
        get
        {
            return gameSettings.lose;
        }
        set
        {
            gameSettings.lose = value;
            SaveFile();
        }
    }


    public bool Tutorial
    {
        get
        {
            return gameSettings.tutorial;
        }
        set
        {
            gameSettings.tutorial = value;
            SaveFile();
        }
    }


    public bool Music
    {
        get
        {
            return gameSettings.music;
        }
        set
        {
            gameSettings.music = value;
            SaveFile();
        }
    }

    public bool Sfx
    {
        get
        {
            return gameSettings.sfx;
        }
        set
        {
            gameSettings.sfx = value;
            SaveFile();
        }
    }

    public bool Vibration
    {
        get
        {
            return gameSettings.vibration;
        }
        set
        {
            gameSettings.vibration = value;
            SaveFile();
        }
    }

    public bool SleepMode
    {
        get
        {
            return gameSettings.sleepMode;
        }
        set
        {
            gameSettings.sleepMode = value;
            SaveFile();
        }
    }

    public bool AppReview
    {
        get
        {
            return gameSettings.appReview;
        }
        set
        {
            gameSettings.appReview = value;
            SaveFile();
        }
    }
    #endregion

    private void Awake()
    {
        instance = this;

        LoadData();
    }

    public void Initialize()
    {
        gameSettings = new GameSettings();

        string str = JsonUtility.ToJson(gameSettings);
        FileIO.SaveData(DEVICESETTINGFILENAME, str, true);
    }

    private void LoadData()
    {
        try
        {
            string stjs = FileIO.LoadData(DEVICESETTINGFILENAME, true);

            if (!string.IsNullOrEmpty(stjs))
            {
                gameSettings = JsonUtility.FromJson<GameSettings>(stjs);

                gameSettings.matchingTime = 9;
                gameSettings.bettingTime = 9;
                gameSettings.bettingWaitTime = 3;
            }
            else
            {
                gameSettings = new GameSettings();

                        string str = JsonUtility.ToJson(gameSettings);
        FileIO.SaveData(DEVICESETTINGFILENAME, str, true);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Load Error \n" + e.Message);
        }
    }

    public void SaveFile()
    {
        try
        {
            string str = JsonUtility.ToJson(gameSettings);
            FileIO.SaveData(DEVICESETTINGFILENAME, str, true);
        }
        catch (Exception e)
        {
            Debug.LogError("Save Error \n" + e.Message);
        }
    }
}
