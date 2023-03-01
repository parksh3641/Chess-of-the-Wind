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
    public LimitManager limitManager;


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
            limitManager.Initialize();
        }
    }

    public void ServerConnectComplete()
    {
        uIManager.OnLoginSuccess();
    }
}
