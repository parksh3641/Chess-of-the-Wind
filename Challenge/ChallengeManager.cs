using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public GameObject challengeView;

    public RectTransform challengeGrid;
    public GameObject challengeAlarm;

    public ChallengeContent[] challengeContentArray;

    public LockManager lockManager;
    public UIManager uIManager;
    public MatchingManager matchingManager;

    PlayerDataBase playerDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        challengeView.SetActive(false);

        challengeGrid.anchoredPosition = new Vector2(0, -999);
    }


    public void OpenChallengeView()
    {
        if (!challengeView.activeSelf)
        {
            challengeView.SetActive(true);

            InitializeChallenge();

            CheckChallenge();
        }
        else
        {
            challengeView.SetActive(false);
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
                break;
            case 3:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);
                break;
            case 4:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);
                break;
            case 5:
                PlayfabManager.instance.UpdateAddCurrency(MoneyType.Gold, 2000);
                break;
        }

        playerDataBase.ChallengeCount += 1;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("ChallengeCount", playerDataBase.ChallengeCount);

        action.Invoke();

        CheckChallenge();

        lockManager.Initialize();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);
        NotionManager.instance.UseNotion(NotionType.GetReward);
    }

    public void ShortCutButton(int number)
    {
        OpenChallengeView();

        switch(number)
        {
            case 0:
                uIManager.OpenMainCanvas(2);
                break;
            case 1:
                matchingManager.GameStartButton_Newbie();
                break;
            case 2:
                matchingManager.GameStartButton_Newbie();
                break;
            case 3:
                uIManager.OpenMainCanvas(2);
                break;
            case 4:
                uIManager.OpenMainCanvas(2);
                break;
            case 5:
                matchingManager.GameStartButton_Gosu();
                break;
        }
    }
}
