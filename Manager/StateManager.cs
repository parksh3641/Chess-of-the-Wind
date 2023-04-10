using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
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

            if (GameStateManager.instance.Penalty > 0)
            {
                if(Application.platform == RuntimePlatform.WindowsEditor)
                {
                    GameStateManager.instance.Penalty = 0;
                    return;
                }

                PlayfabManager.instance.UpdateSubtractCurrency(MoneyType.Gold, GameStateManager.instance.Penalty);

                GameStateManager.instance.Penalty = 0;
        
                Debug.Log("패널티가 적용되었습니다");
            }
        }
    }

    public void ServerConnectComplete()
    {
        uIManager.OnLoginSuccess();
    }
}
