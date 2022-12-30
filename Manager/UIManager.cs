using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Space]
    [Title("Main")]
    public Text coinText;
    public Text crystalText;
    public Text rankPointText;
    public Text nickNameText;

    [Space]
    [Title("Login")]
    public GameObject loginUI;
    public GameObject[] loginButtonList;

    [Space]
    [Title("View")]
    public GameObject loginView;
    public GameObject mainView;
    public GameObject bettingView;
    public GameObject rouletteView;
    public GameObject bounsView;

    public GameObject dontTouchObj;
    public GameObject waitingObj;

    [Space]
    [Title("DataBase")]
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        loginView.SetActive(true);
        mainView.SetActive(false);
        bettingView.SetActive(false);
        rouletteView.SetActive(false);
        bounsView.SetActive(false);

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);
    }

    private void Start()
    {
        SetLoginUI();
    }

    public void Initialize()
    {
        RenewalVC();
    }

    public void RenewalVC()
    {
        Debug.Log("Renewal VC");

        coinText.text = playerDataBase.Coin.ToString();
        crystalText.text = playerDataBase.Crystal.ToString();
        rankPointText.text = "0";
        nickNameText.text = GameStateManager.instance.NickName;
    }


    public void SetLoginUI()
    {
        loginUI.SetActive(false);

        for (int i = 0; i < loginButtonList.Length; i++)
        {
            loginButtonList[i].SetActive(false);
        }

        if (!GameStateManager.instance.AutoLogin)
        {
            loginUI.SetActive(true);

//#if UNITY_ANDROID
//            loginButtonList[0].SetActive(true);
//#elif UNITY_IOS
//            loginButtonList[1].SetActive(true);
//#endif
            loginButtonList[2].SetActive(true);
        }
    }

    public void OnLoginSuccess()
    {
        loginView.SetActive(false);
        mainView.SetActive(true);
    }

    public void OnGameStart()
    {
        mainView.SetActive(false);
        bettingView.SetActive(true);

        dontTouchObj.SetActive(true);
    }

    public void OnRestartGame()
    {
        dontTouchObj.SetActive(false);
    }

    public void OnGameStop()
    {
        mainView.SetActive(true);
        bettingView.SetActive(false);
        rouletteView.SetActive(false);

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);
    }

    public void OpenRouletteView()
    {
        bettingView.SetActive(false);
        rouletteView.SetActive(true);
    }

    public void CloseRouletteView()
    {
        bettingView.SetActive(true);
        rouletteView.SetActive(false);

        dontTouchObj.SetActive(true);
    }

    public void SetWaiting(bool check)
    {
        waitingObj.SetActive(check);
    }

    public void OpenBounsView(bool check)
    {
        bounsView.SetActive(check);
    }
}
