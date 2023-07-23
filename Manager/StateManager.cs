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

    public GameObject penaltyView;
    public Text penaltyValue;


    void Awake()
    {
        instance = this;

        penaltyView.SetActive(false);
    }

    public void ServerInitialize()
    {
        networkManager.Initialize();
    }

    public void ServerConnectComplete()
    {
        uIManager.OnLoginSuccess();

        Initialize();
    }

    public void Initialize()
    {
        if (!isInit)
        {
            isInit = true;

            SoundManager.instance.Initialize();

            uIManager.Initialize();
            //networkManager.Initialize();
            gameManager.Initialize();
            nickNameManager.Initialize();
            matchingManager.Initialize();
            storyManager.Initialize();
            rankInfoManager.Initialize();

            if (GameStateManager.instance.Penalty > 0)
            {
                if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    GameStateManager.instance.Penalty = 0;
                    return;
                }

                penaltyView.SetActive(true);

                penaltyValue.text = "-" + GameStateManager.instance.Penalty.ToString();

                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, GameStateManager.instance.Penalty);

                GameStateManager.instance.Penalty = 0;
       
            }

            Debug.Log("모든 초기화가 완료되었습니다");
        }
    }

    public void ClosePenaltyView()
    {
        penaltyView.SetActive(false);
    }
}
