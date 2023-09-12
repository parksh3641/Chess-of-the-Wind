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

    public GameObject penaltyView;
    public Text penaltyValue;


    void Awake()
    {
        instance = this;

        penaltyView.SetActive(false);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        //GameStateManager.instance.Playing = false;
        //GameStateManager.instance.Win = false;
        //GameStateManager.instance.Lose = false;
    }

    public void Initialize()
    {
        if (!isInit)
        {
            isInit = true;

            SoundManager.instance.Initialize();

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
            optionManager.Initialize_First();

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

                    PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, GameStateManager.instance.Stakes);

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

            Debug.Log("Initialize Complete!");
        }
    }

    public void ClosePenaltyView()
    {
        penaltyView.SetActive(false);
    }
}
