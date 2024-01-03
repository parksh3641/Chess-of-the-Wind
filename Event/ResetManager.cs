using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResetManager : MonoBehaviour
{
    public static ResetManager instance;

    DateTime serverTime;
    DateTime nextMondey;

    public AttendanceManager attendanceManager;
    public EventManager eventManager;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.6f);

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        instance = this;

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void Initialize()
    {
        if (!playerDataBase.AttendanceCheck)
        {
            attendanceManager.OnSetAlarm();
        }

        OnCheckAttendanceDay();
    }

    public void OnCheckAttendanceDay()
    {
        if (PlayfabManager.instance.isActive) PlayfabManager.instance.GetServerTime(SetModeContent);
    }

    private void SetModeContent(DateTime time)
    {
        if (playerDataBase.AttendanceDay.Length < 2)
        {
            Debug.Log("데일리 미션 맨 처음 초기화");

            ResetValue();

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AccessDate", playerDataBase.AccessDate);
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(playerDataBase.AttendanceDay));
        }
        else
        {
            if (ComparisonDate(playerDataBase.AttendanceDay, time))
            {
                Debug.Log("하루가 지났습니다");

                ResetValue();
                StartCoroutine(ResetCoroution());
            }
            else
            {
                Debug.Log("아직 하루가 안 지났습니다.");
            }
        }

        if (playerDataBase.NextMonday.Length < 2)
        {
            Debug.Log("주간 미션 초기화");

            nextMondey = DateTime.Today.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek + 7) % 7);

            if (nextMondey == DateTime.Today)
            {
                nextMondey = nextMondey.AddDays(7);
            }

            playerDataBase.NextMonday = nextMondey.ToString("yyyyMMdd");
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NextMonday", int.Parse(playerDataBase.NextMonday));
        }
        else
        {
            if (ComparisonDate(playerDataBase.NextMonday, time))
            {
                Debug.Log("월요일이 되었습니다");

                nextMondey = DateTime.Today.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek + 7) % 7);

                if (nextMondey == DateTime.Today)
                {
                    nextMondey = nextMondey.AddDays(7);
                }

                playerDataBase.NextMonday = nextMondey.ToString("yyyyMMdd");

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("NextMonday", int.Parse(playerDataBase.NextMonday));
            }
            else
            {
                Debug.Log("아직 다음주 월요일이 아닙니다");
            }
        }
    }

    public bool ComparisonDate(string target, System.DateTime time)
    {
        System.DateTime server = time;
        System.DateTime system = System.DateTime.ParseExact(target, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        bool c = false;

        if (server.Year > system.Year)
        {
            c = true;
        }
        else
        {
            if (server.Year == system.Year)
            {
                if (server.Month > system.Month)
                {
                    c = true;
                }
                else
                {
                    if (server.Month == system.Month)
                    {
                        if (server.Day >= system.Day)
                        {
                            c = true;
                        }
                        else
                        {
                            c = false;
                        }
                    }
                    else
                    {
                        c = false;
                    }
                }
            }
            else
            {
                c = false;
            }
        }

        return c;
    }

    [Button]
    void ResetInitialize()
    {
        ResetValue();
        StartCoroutine(ResetCoroution());
    }

    void ResetValue()
    {
        playerDataBase.AttendanceDay = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
        playerDataBase.AccessDate += 1;

        if (playerDataBase.AttendanceCheck)
        {
            playerDataBase.AttendanceCheck = false;

            if (playerDataBase.AttendanceCount >= 7)
            {
                playerDataBase.AttendanceCount = 0;

                Debug.Log("출석 체크 보상 리셋");
            }
            else
            {
                Debug.Log("다음 출석 체크 보상 오픈");
            }

            attendanceManager.OnSetAlarm();
        }

        if (playerDataBase.WelcomeCheck)
        {
            playerDataBase.WelcomeCheck = false;

            if (playerDataBase.WelcomeCount < 7)
            {
                eventManager.OnWelcomeAlarm();
            }
        }

        if (playerDataBase.WelcomeBoxCheck)
        {
            playerDataBase.WelcomeBoxCheck = false;

            if (playerDataBase.welcomeBoxCount < 9)
            {
                eventManager.OnWelcomeBoxAlarm();
            }
        }

        playerDataBase.DailyWin = 0;
        playerDataBase.DailyNormalBox_1 = 3;
        playerDataBase.DailyNormalBox_10 = 1;
        playerDataBase.DailyEpicBox_1 = 3;
        playerDataBase.DailyEpicBox_10 = 1;
        playerDataBase.DailyReward = 0;
        playerDataBase.DailyBuy1 = 0;
        playerDataBase.DailyBuy2 = 0;
        playerDataBase.DailyBuyCount1 = 0;
        playerDataBase.DailyBuyCount2 = 0;
        playerDataBase.DailyAdsReward = 0;
        playerDataBase.DailyAdsReward2 = 0;
        playerDataBase.DailyAdsReward3 = 0;
        playerDataBase.DailyGoldReward = 0;
    }

    IEnumerator ResetCoroution()
    {
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AccessDate", playerDataBase.AccessDate);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(playerDataBase.AttendanceDay));
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCheck", 0);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCount", 0);

        yield return waitForSeconds;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCheck", 0);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeBoxCheck", 0);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyWin", playerDataBase.DailyWin);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyNormalBox_1", playerDataBase.DailyNormalBox_1);

        yield return waitForSeconds;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyNormalBox_10", playerDataBase.DailyNormalBox_10);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyEpicBox_1", playerDataBase.DailyEpicBox_1);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyEpicBox_10", playerDataBase.DailyEpicBox_10);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyReward", playerDataBase.DailyReward);

        yield return waitForSeconds;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyBuy1", playerDataBase.DailyBuy1);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyBuy2", playerDataBase.DailyBuy2);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyBuyCount1", playerDataBase.DailyBuyCount1);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyBuyCount2", playerDataBase.DailyBuyCount2);

        yield return waitForSeconds;

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyAdsReward", playerDataBase.DailyAdsReward);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyAdsReward2", playerDataBase.DailyAdsReward2);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyAdsReward3", playerDataBase.DailyAdsReward3);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("DailyGoldReward", playerDataBase.DailyGoldReward);

        yield return waitForSeconds;
    }
}
