using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public bool isInit = false;

    public NetworkManager networkManager;
    public GameManager gameManager;
    public UIManager uIManager;
    public NickNameManager nickNameManager;
    public MatchingManager matchingManager;
    public FormationUIManager formationUIManager;
    public RankInfoManager rankInfoManager;
    public ResetManager resetManager;
    public EventManager eventManager;
    public MailBoxManager mailBoxManager;
    public LockManager lockManager;
    public NewsManager newsManager;
    public ChallengeManager challengeManager;
    public OptionManager optionManager;
    public TitleManager titleManager;
    public AttendanceManager attendanceManager;

    public GameObject penaltyView;
    public Text penaltyValue;

    int playTime = 0;

    WaitForSeconds waitForSeconds2 = new WaitForSeconds(1.0f);

    PlayerDataBase playerDataBase;

    void Awake()
    {
        instance = this;

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        penaltyView.SetActive(false);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Initialize()
    {
        if (!isInit)
        {
            isInit = true;

            SoundManager.instance.Initialize();

            nickNameManager.Initialize();
            uIManager.Initialize();
            gameManager.Initialize();
            matchingManager.Initialize();
            formationUIManager.Initialize();
            rankInfoManager.Initialize();
            resetManager.Initialize();
            eventManager.Initialize();
            mailBoxManager.Initialize();
            lockManager.Initialize();
            newsManager.Initialize();
            challengeManager.Initialize();
            titleManager.Initialize();
            attendanceManager.CheckInitialize();

            if (GameStateManager.instance.Penalty > 0)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    GameStateManager.instance.Penalty = 0;
                    return;
                }

                DateTime time = DateTime.Parse(PlayerPrefs.GetString("PenaltyTime"));
                DateTime now = DateTime.Now;

                TimeSpan span = time - now;

                GameStateManager.instance.Penalty = 0;

                if (span.TotalSeconds < -60)
                {
                    penaltyView.SetActive(true);

                    penaltyValue.text = "-" + GameStateManager.instance.Stakes.ToString();

                    PlayfabManager.instance.UpdateSubtractGold(GameStateManager.instance.Stakes);

                    GameStateManager.instance.Playing = false;
                    GameStateManager.instance.Win = false;
                    GameStateManager.instance.Lose = true;

                    matchingManager.CheckRankUp();

                    Debug.Log("튕긴 지 60초가 지났기 때문에 패널티 처리");
                }
                else
                {
                    Debug.Log("튕긴 지 60초가 안 지났습니다");
                }
            }

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("Version", int.Parse(Application.version.Replace(".", "")));

#if UNITY_ANDROID
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("OS", 0);
#elif UNITY_IOS
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("OS", 1);
#else
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("OS", 2);
#endif

            playTime = 0;
            StartCoroutine(PlayTimeCoroution());

            Debug.Log("Initialize Complete!");
        }
    }

    public void ClosePenaltyView()
    {
        penaltyView.SetActive(false);
    }

    IEnumerator PlayTimeCoroution()
    {
        if (playTime >= 60)
        {
            playTime = 0;
            playerDataBase.PlayTime += 1;
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("PlayTime", playerDataBase.PlayTime);

            Debug.LogError("1분 지남");
        }
        else
        {
            playTime += 1;
        }

        yield return waitForSeconds2;
        StartCoroutine(PlayTimeCoroution());
    }
}
