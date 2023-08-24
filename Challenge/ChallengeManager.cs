using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public GameObject challengeView;

    public RectTransform challengeGrid;
    public GameObject challengeAlarm;
    public GameObject exitButton;

    public ChallengeContent[] challengeContentArray;

    public LockManager lockManager;
    public UIManager uIManager;
    public MatchingManager matchingManager;

    List<string> itemList = new List<string>();

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        challengeView.SetActive(false);

        challengeAlarm.SetActive(false);

        challengeGrid.anchoredPosition = new Vector2(0, -999);
    }

    public void Initialize()
    {
        if(playerDataBase.ChallengeCount < 6)
        {
            StartCoroutine(CheckCoroution());
        }
    }

    IEnumerator CheckCoroution()
    {
        if (!challengeView.activeSelf)
        {
            CheckChallengeTutorial();
        }

        yield return waitForSeconds;

        StartCoroutine(CheckCoroution());
    }

    public void OpenChallengeView()
    {
        if (!uIManager.mainCanvas.enabled) return;

        if (!challengeView.activeSelf)
        {
            challengeView.SetActive(true);

            exitButton.SetActive(true);

            InitializeChallenge();

            CheckChallenge();
        }
        else
        {
            if(exitButton.activeInHierarchy)
            {
                challengeView.SetActive(false);
            }
        }
    }

    void CheckChallengeTutorial()
    {
        switch(playerDataBase.ChallengeCount)
        {
            case 0:
                if (playerDataBase.CheckEquipBlock_Newbie())
                {
                    OpenChallengeView();

                    exitButton.SetActive(false);

                    challengeContentArray[0].lockObj[0].SetActive(true);

                    challengeAlarm.SetActive(true);
                }
                break;
            case 1:
                if (playerDataBase.NewbieWin > 0)
                {
                    OpenChallengeView();

                    exitButton.SetActive(false);

                    challengeContentArray[1].lockObj[0].SetActive(true);

                    challengeAlarm.SetActive(true);
                }
                break;
            case 2:
                if (playerDataBase.NewbieWin > 1)
                {
                    OpenChallengeView();

                    exitButton.SetActive(false);

                    challengeContentArray[2].lockObj[0].SetActive(true);

                    challengeAlarm.SetActive(true);
                }
                break;
            case 3:
                if (playerDataBase.CheckEquipBlock_Gosu() == 3)
                {
                    OpenChallengeView();

                    exitButton.SetActive(false);

                    challengeContentArray[3].lockObj[0].SetActive(true);

                    challengeAlarm.SetActive(true);
                }
                break;
            case 4:
                if (playerDataBase.CheckBlockLevelCount(0) > 2)
                {
                    OpenChallengeView();

                    exitButton.SetActive(false);

                    challengeContentArray[4].lockObj[0].SetActive(true);

                    challengeGrid.anchoredPosition = new Vector2(0, 999);

                    challengeAlarm.SetActive(true);
                }
                break;
            case 5:
                if (playerDataBase.GosuWin > 0)
                {
                    OpenChallengeView();

                    exitButton.SetActive(false);

                    challengeContentArray[5].lockObj[0].SetActive(true);

                    challengeGrid.anchoredPosition = new Vector2(0, 999);

                    challengeAlarm.SetActive(true);
                }
                break;
        }
    }

    void InitializeChallenge()
    {
        challengeContentArray[0].receiveContent.Initialize(RewardType.Gold, 200);
        challengeContentArray[1].receiveContent.Initialize(RewardType.Gold, 200);
        challengeContentArray[2].receiveContent.Initialize(RewardType.Gold, 1000);
        challengeContentArray[3].receiveContent.Initialize(RewardType.Gold, 2000);
        challengeContentArray[4].receiveContent.Initialize(RewardType.Gold, 2000);
        challengeContentArray[5].receiveContent.Initialize(RewardType.Gold, 2000);
    }

    public void CheckChallenge()
    {
        challengeContentArray[0].Initialize(playerDataBase.ChallengeCount, this);
        challengeContentArray[1].Initialize(playerDataBase.ChallengeCount, this);
        challengeContentArray[2].Initialize(playerDataBase.ChallengeCount, this);
        challengeContentArray[3].Initialize(playerDataBase.ChallengeCount, this);
        challengeContentArray[4].Initialize(playerDataBase.ChallengeCount, this);
        challengeContentArray[5].Initialize(playerDataBase.ChallengeCount, this);
    }

    public void ReceiveButton(int number, Action action)
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        switch (number)
        {
            case 0:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 200);
                break;
            case 1:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 200);
                break;
            case 2:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 1000);

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:

                        itemList.Clear();
                        itemList.Add("LeftQueen_2_N");
                        itemList.Add("LeftNight_N");
                        itemList.Add("Rook_V2_N");

                        PlayfabManager.instance.GrantItemsToUser("Kingdom of Snow", itemList);

                        break;
                    case WindCharacterType.UnderWorld:

                        itemList.Clear();
                        itemList.Add("RightQueen_2_N");
                        itemList.Add("RightNight_N");
                        itemList.Add("Rook_V4_N");

                        PlayfabManager.instance.GrantItemsToUser("Kingdom of the Underworld", itemList);
                        break;
                }
                break;
            case 3:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);
                break;
            case 4:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);
                break;
            case 5:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);

                exitButton.SetActive(true);

                StopAllCoroutines();
                break;
        }

        playerDataBase.ChallengeCount += 1;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("ChallengeCount", playerDataBase.ChallengeCount);

        action.Invoke();

        CheckChallenge();

        lockManager.Initialize();

        challengeAlarm.SetActive(false);

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.GetReward);
    }

    public void ShortCutButton(int number)
    {
        challengeView.SetActive(false);

        switch(number)
        {
            case 0:
                uIManager.OpenMainCanvas(2);
                break;
            case 1:
                uIManager.OpenMainCanvas(1);

                matchingManager.GameStartButton_Newbie();
                break;
            case 2:
                uIManager.OpenMainCanvas(1);

                matchingManager.GameStartButton_Newbie();
                break;
            case 3:
                uIManager.OpenMainCanvas(2);
                break;
            case 4:
                uIManager.OpenMainCanvas(2);
                break;
            case 5:
                uIManager.OpenMainCanvas(1);

                matchingManager.GameStartButton_Gosu();
                break;
        }
    }
}
