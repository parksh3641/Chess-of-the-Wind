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
        [Title("Developer")]
        public bool isLogin = false;

        [Space]
        [Title("Login")]
        public string playfabId = "";
        public string customId = "";
        public bool autoLogin = false;
        public LoginType login = LoginType.None;
        public string nickName = "";

        [Space]
        [Title("Language")]
        public LanguageType language = LanguageType.Default;

        [Space]
        [Title("InGame")]
        public WindCharacterType windCharacterType = WindCharacterType.Winter;
        public GameType gameType = GameType.NewBie;
        public GameRankType gameRankType = GameRankType.Bronze_4;
        public bool playing = false;
        public int matchingTime = 6;
        public int stakes = 0;
        public int penalty = 0;

        [Space]
        [Title("Rank")]
        public int winStreak = 0;
        public bool win = false;
        public bool lose = false;

        [Space]
        [Title("Setting")]
        public bool tutorial = true;
        public bool music = true;
        public bool sfx = true;
        public bool vibration = true;
        public bool sleepMode = false;
        public bool inAppReview = false;

        [Space]
        [Title("Betting")]
        public int bettingTime = 11;
        public int bettingWaitTime = 5;
        public bool autoTarget = false;
        public int autoTargetNumber = 0;
        public bool blockOverlap = false;

        [Space]
        [Title("Bouns")]
        public bool checkBouns = false;
    }
    #region Data

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

    public bool InAppReview
    {
        get
        {
            return gameSettings.inAppReview;
        }
        set
        {
            gameSettings.inAppReview = value;
            SaveFile();
        }
    }

    #endregion

    private void Awake()
    {
        instance = this;

        LoadData();
    }
    private void LoadData()
    {
        try
        {
            string stjs = FileIO.LoadData(DEVICESETTINGFILENAME, true);

            if (!string.IsNullOrEmpty(stjs))
            {
                gameSettings = JsonUtility.FromJson<GameSettings>(stjs);
            }
            else
            {
                gameSettings = new GameSettings();
                gameSettings.playing = false;
                gameSettings.matchingTime = 6;
                gameSettings.penalty = 0;
                gameSettings.winStreak = 0;
                gameSettings.win = false;
                gameSettings.lose = false;
                gameSettings.tutorial = false;
                gameSettings.gameRankType = GameRankType.Bronze_4;
                gameSettings.bettingTime = 11;
                gameSettings.bettingWaitTime = 5;
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
