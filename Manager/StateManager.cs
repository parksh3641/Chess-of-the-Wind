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

    public void Initialize()
    {
        if (!isInit)
        {
            isInit = true;

            networkManager.Initialize();
            gameManager.Initialize();
            uIManager.Initialize();
            nickNameManager.Initialize();
            matchingManager.Initialize();
            storyManager.Initialize();
            SoundManager.instance.Initialize();
            rankInfoManager.Initialize();

            if (GameStateManager.instance.Penalty > 0)
            {
                if(Application.platform == RuntimePlatform.WindowsEditor)
                {
                    GameStateManager.instance.Penalty = 0;
                    return;
                }

                penaltyView.SetActive(true);

                penaltyValue.text = "-" + GameStateManager.instance.Penalty.ToString();

                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, GameStateManager.instance.Penalty);

                GameStateManager.instance.Penalty = 0;
        
                Debug.Log("패널티가 적용되었습니다");
            }
        }
    }

    public void ClosePenaltyView()
    {
        penaltyView.SetActive(false);
    }

    public void ServerConnectComplete()
    {
        uIManager.OnLoginSuccess();
    }
}
