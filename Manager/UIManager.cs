﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Space]
    [Title("Login")]
    public GameObject loginUI;
    public GameObject[] loginButtonList;

    [Space]
    [Title("Main")]
    public Text goldText;
    public Text crystalText;
    public Text nickNameText;

    [Space]
    [Title("View")]
    public Canvas mainCanvas;
    public Canvas gameCanvas;

    public GameObject loginView;
    public GameObject mainView;

    [Space]
    public GameObject bettingView;
    public GameObject rouletteView;
    public GameObject bounsView;
    public GameObject surrenderView;
    public GameObject disconnectedView;

    [Space]
    public GameObject dontTouchObj;
    public GameObject waitingObj;

    [Space]
    [Title("VS")]
    public GameObject vsView;
    public Text player1Text;
    public Text player2Text;
    public FadeInOut vsFadeInOut;
    public FadeInOut mainFadeInOut;

    [Space]
    [Title("Result")]
    public GameObject resultView;
    public Text resultTitleText;
    public Text resultGoldText;

    [Space]
    [Title("MainCanvas")]
    public ShopManager shopManager;
    public CollectionManager collectionManager;
    public GameManager gameManager;

    public Image[] bottomUIImg;
    public GameObject[] bottomUIIcon;
    public RectTransform[] bottmUIRect;

    [Space]
    [Title("DataBase")]
    RankDataBase rankDataBase;
    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        goldText.text = "0";
        crystalText.text = "0";
        nickNameText.text = "";

        mainCanvas.enabled = true;
        gameCanvas.enabled = false;

        loginView.SetActive(true);
        mainView.SetActive(false);

        vsView.SetActive(false);
        bettingView.SetActive(false);
        rouletteView.SetActive(false);
        bounsView.SetActive(false);
        resultView.SetActive(false);
        surrenderView.SetActive(false);
        disconnectedView.SetActive(false);

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);
    }

    private void Start()
    {
        GameStateManager.instance.Playing = false;

        SetLoginUI();

        OpenMainCanvas(2);
    }

    public void Initialize()
    {
        Renewal();
    }

    public void Renewal()
    {
        goldText.text = MoneyUnitString.ToCurrencyString(playerDataBase.Gold);
        crystalText.text = MoneyUnitString.ToCurrencyString(playerDataBase.Crystal);
        nickNameText.text = "닉네임 : " + GameStateManager.instance.NickName;

        Debug.Log("Main UI Renewal");
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

    public void OnMatchingSuccess(string player1, string player2)
    {
        StartCoroutine(MatchingCoroution(player1, player2));
    }

    IEnumerator MatchingCoroution(string player1, string player2)
    {
        GameStateManager.instance.Playing = true;

        yield return new WaitForSeconds(1f);

        mainCanvas.enabled = false;
        gameCanvas.enabled = true;

        vsView.SetActive(true);

        player1Text.text = player1;
        player2Text.text = player2;

        yield return new WaitForSeconds(3f);

        GameStart();

        vsFadeInOut.FadeOut();

        yield return new WaitForSeconds(1.5f);

        if (GameStateManager.instance.GameType == GameType.NewBie)
        {
            gameManager.GameStart_Newbie();
        }
        else
        {
            gameManager.GameStart_Gosu();
        }
    }

    public void GameStart()
    {
        mainView.SetActive(false);
        bettingView.SetActive(true);

        dontTouchObj.SetActive(true);
    }

    public void RestartGame()
    {
        dontTouchObj.SetActive(false);
    }

    public void GameEnd()
    {
        mainFadeInOut.FadeOutToIn();

        StartCoroutine(GameEndCoroution());
    }

    IEnumerator GameEndCoroution()
    {
        yield return new WaitForSeconds(1f);

        mainCanvas.enabled = true;
        gameCanvas.enabled = false;

        mainView.SetActive(true);
        bettingView.SetActive(false);
        rouletteView.SetActive(false);

        dontTouchObj.SetActive(false);
        waitingObj.SetActive(false);

        resultView.SetActive(false);
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

    public void SetWaitingView(bool check)
    {
        waitingObj.SetActive(check);
    }

    public void OpenBounsView(bool check)
    {
        bounsView.SetActive(check);
    }

    public void OpenResultView(int number, int gold)
    {
        resultView.SetActive(true);

        if(number == 0)
        {
            resultTitleText.text = "승리";
        }
        else if(number == 1)
        {
            resultTitleText.text = "패배";
        }
        else
        {
            resultTitleText.text = "무승부";
        }

        if(gold > 0)
        {
            resultGoldText.text = "+" + MoneyUnitString.ToCurrencyString(Mathf.Abs(gold));
        }
        else
        {
            resultGoldText.text = "-" + MoneyUnitString.ToCurrencyString(Mathf.Abs(gold));
        }

        RecordManager.instance.OpenRecord();
    }

    public void OpenSurrenderView()
    {
        if (!surrenderView.activeSelf)
        {
            surrenderView.SetActive(true);
        }
        else
        {
            surrenderView.SetActive(false);
        }
    }

    public void CloseSurrenderView()
    {
        surrenderView.SetActive(false);
    }

    public void OpenDisconnectedView()
    {
        disconnectedView.SetActive(true);
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("LoginScene");
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

                shopManager.OpenShopView();
                collectionManager.CloseCollectionView();
                break;
            case 1:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-175f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(37.5f, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(212.5f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(387.5f, 0);

                shopManager.CloseShopView();
                collectionManager.OpenCollectionView();
                break;
            case 2:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-212.5f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(0, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(212.5f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(387.5f, 0);

                shopManager.CloseShopView();
                collectionManager.CloseCollectionView();
                break;
            case 3:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-212.5f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(-37.5f, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(175f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(387.5f, 0);

                shopManager.CloseShopView();
                collectionManager.CloseCollectionView();
                break;
            case 4:
                bottmUIRect[0].anchoredPosition = new Vector2(-387.5f, 0);
                bottmUIRect[1].anchoredPosition = new Vector2(-212.5f, 0);
                bottmUIRect[2].anchoredPosition = new Vector2(-37.5f, 0);
                bottmUIRect[3].anchoredPosition = new Vector2(137.5f, 0);
                bottmUIRect[4].anchoredPosition = new Vector2(350, 0);

                shopManager.CloseShopView();
                collectionManager.CloseCollectionView();
                break;
        }    
    }
}