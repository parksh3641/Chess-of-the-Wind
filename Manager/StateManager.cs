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
    public StoryManager storyManager;
    public RankInfoManager rankInfoManager;
    public ResetManager resetManager;
    public EventManager eventManager;
    public MailBoxManager mailBoxManager;
    public LockManager lockManager;

    public GameObject penaltyView;
    public Text penaltyValue;


    void Awake()
    {
        instance = this;

        penaltyView.SetActive(false);
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
            storyManager.Initialize();
            rankInfoManager.Initialize();
            resetManager.Initialize();
            eventManager.Initialize();
            mailBoxManager.Initialize();
            lockManager.Initialize();

            if (GameStateManager.instance.Penalty > 0)
            {
                //if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                //{
                //    GameStateManager.instance.Penalty = 0;
                //    return;
                //}

                penaltyView.SetActive(true);

                penaltyValue.text = "-" + GameStateManager.instance.Stakes.ToString();

                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, GameStateManager.instance.Stakes);

                GameStateManager.instance.Win = false;
                GameStateManager.instance.Lose = true;

                GameStateManager.instance.Penalty = 0;

                matchingManager.CheckRankUp();
            }

            Debug.Log("Initialize Complete!");
        }
    }

    public void ClosePenaltyView()
    {
        penaltyView.SetActive(false);
    }
}
