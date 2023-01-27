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


    void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        Application.targetFrameRate = 60;

        if (!isInit)
        {
            isInit = true;

            networkManager.Initialize();
            gameManager.Initialize();
            uIManager.Initialize();
            nickNameManager.Initialize();
        }
    }

    public void ServerConnectComplete()
    {
        uIManager.OnLoginSuccess();
    }
}
