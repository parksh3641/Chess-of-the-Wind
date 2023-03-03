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

            if(GameStateManager.instance.Penalty > 0)
            {
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
