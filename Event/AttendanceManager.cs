using Firebase.Analytics;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceManager : MonoBehaviour
{
    public GameObject attendanceView;

    public GameObject alarm;

    public Text timerText;

    public AttendanceContent[] attendanceContentArray;

    [Title("7 Day")]
    public LocalizationContent titleText;
    public ReceiveContent[] receiveContentArray;
    public GameObject lockObj;
    public GameObject clearObj;

    string localization_NextQuest = "";
    string localization_Hours = "";
    string localization_Minutes = "";

    private int price = 0;

    DateTime f, g, i, j, l, m;
    TimeSpan h, k, n;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);

    PlayerDataBase playerDataBase;
    RankDataBase rankDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (rankDataBase == null) rankDataBase = Resources.Load("RankDataBase") as RankDataBase;

        attendanceView.SetActive(false);
        lockObj.SetActive(true);
        clearObj.SetActive(false);
        alarm.SetActive(false);
    }

    [Button]
    public void NextDay()
    {
        playerDataBase.AttendanceCheck = false;

        CheckAttendance();
    }

    public void OpenAttendanceView()
    {
        if(!attendanceView.activeInHierarchy)
        {
            attendanceView.SetActive(true);

            if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
            {
                ResetManager.instance.Initialize();
            }

            timerText.text = "";

            titleText.localizationName = "7Day";
            titleText.ReLoad();

            Initialize();

            CheckAttendance();

            StopAllCoroutines();
            StartCoroutine(TimerCoroution());

            FirebaseAnalytics.LogEvent("Open_Attendance");
        }
        else
        {
            attendanceView.SetActive(false);
        }
    }

    public void CheckInitialize()
    {
        if (playerDataBase.ChallengeCount >= 6)
        {
            if (!playerDataBase.attendanceCheck)
            {
                OpenAttendanceView();
            }
        }
    }

    public void Initialize()
    {
        price = rankDataBase.GetRankInformation(GameStateManager.instance.GameRankType).stakes;

        attendanceContentArray[0].receiveContent.Initialize(RewardType.Gold, price);
        attendanceContentArray[1].receiveContent.Initialize(RewardType.Box_Normal, 1);
        attendanceContentArray[2].receiveContent.Initialize(RewardType.Gold, price);
        attendanceContentArray[3].receiveContent.Initialize(RewardType.Box_Normal, 1);
        attendanceContentArray[4].receiveContent.Initialize(RewardType.Gold, price);
        attendanceContentArray[5].receiveContent.Initialize(RewardType.Box_Normal, 1);

        receiveContentArray[0].Initialize(RewardType.Gold, price * 2);
        receiveContentArray[1].Initialize(RewardType.Box_Normal, 3);
        receiveContentArray[2].Initialize(RewardType.UpgradeTicket, 1);
    }

    public void CheckAttendance()
    {
        localization_NextQuest = LocalizationManager.instance.GetString("NextRewardDay");
        localization_Hours = LocalizationManager.instance.GetString("Hours");
        localization_Minutes = LocalizationManager.instance.GetString("Minutes");

        for (int i = 0; i < attendanceContentArray.Length; i ++)
        {
            attendanceContentArray[i].Initialize(playerDataBase.AttendanceCount, playerDataBase.AttendanceCheck, this);
        }

        lockObj.SetActive(true);

        if (playerDataBase.AttendanceCount == 6 && !playerDataBase.AttendanceCheck)
        {
            lockObj.SetActive(false);
        }

        clearObj.SetActive(false);

        if (playerDataBase.AttendanceCount >= 7 && playerDataBase.AttendanceCheck)
        {
            lockObj.SetActive(false);
            clearObj.SetActive(true);
        }
    }

    public void ReceiveButton7Day()
    {
        ReceiveButton(6, SuccessReceive);
    }

    public void SuccessReceive()
    {
        clearObj.SetActive(true);
    }

    public void ReceiveButton(int number, Action action)
    {
        if (playerDataBase.AttendanceCheck) return;

        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        switch (number)
        {
            case 0:
                PlayfabManager.instance.UpdateAddGold(price);
                break;
            case 1:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 1);
                        break;
                }
                break;
            case 2:
                PlayfabManager.instance.UpdateAddGold(price);
                break;
            case 3:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 1);
                        break;
                }
                break;
            case 4:
                PlayfabManager.instance.UpdateAddGold(price);
                break;
            case 5:
                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox__Normal", 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 1);
                        break;
                }
                break;
            case 6:
                PlayfabManager.instance.UpdateAddGold(price * 2);

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_Normal = 3;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 3);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_Normal = 3;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 3);
                        break;
                }

                ItemAnimManager.instance.GetUpgradeTicket(1);

                clearObj.SetActive(true);

                break;
        }

        playerDataBase.AttendanceCount += 1;
        playerDataBase.AttendanceCheck = true;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCount", playerDataBase.AttendanceCount);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCheck", 1);

        action.Invoke();

        CheckAttendance();

        OnCheckAlarm();

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetReward);

        FirebaseAnalytics.LogEvent("Clear_Attendance");
    }

    IEnumerator TimerCoroution()
    {
        f = DateTime.Now;
        g = DateTime.Today.AddDays(1);
        h = g - f;

        timerText.text = localization_NextQuest + " : " + h.Hours.ToString("D2") + localization_Hours + " " + h.Minutes.ToString("D2") + localization_Minutes;

        if (playerDataBase.AttendanceDay == DateTime.Today.ToString("yyyyMMdd"))
        {
            ResetManager.instance.Initialize();

            Invoke("RestartTimer", 2.0f);
            yield break;
        }

        yield return waitForSeconds;
        StartCoroutine(TimerCoroution());
    }

    void ReStartTimer()
    {
        StartCoroutine(TimerCoroution());
    }

    public void OnSetAlarm()
    {
        alarm.SetActive(true);
    }

    public void OnCheckAlarm()
    {
        alarm.SetActive(false);
    }
}
