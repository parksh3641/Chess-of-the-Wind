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
    [Title("MainCanvas")]
    public GameObject[] mainPanelView;

    public Image[] bottomUIImg;
    public GameObject[] bottomUIIcon;
    public RectTransform[] bottmUIRect;

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

        OpenMainCanvas(2);
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

    public void OpenMainCanvas(int number)
    {
        for (int i = 0; i < bottmUIRect.Length; i++)
        {
            bottomUIImg[i].color = new Color(1, 1, 1, 1);
            bottmUIRect[i].sizeDelta = new Vector2(175, 200);
        }

        bottmUIRect[number].sizeDelta = new Vector2(250, 200);
        bottomUIImg[number].color = Color.yellow;

        switch (number)
        {
            case 0:
                bottmUIRect[0].anchoredPosition = new Vector2(-350, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-137.5f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(37.5f, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(212.5f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(387.5f, 0);
                break;
            case 1:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-175f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(37.5f, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(212.5f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(387.5f, 0);
                break;
            case 2:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-212.5f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(0, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(212.5f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(387.5f, 0);
                break;
            case 3:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-212.5f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(-37.5f, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(175f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(387.5f, 0);
                break;
            case 4:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-212.5f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(-37.5f, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(137.5f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(350, 0);
                break;
        }    
    }
}
