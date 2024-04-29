using Firebase.Analytics;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ResetInfo
{
    public int dailyWin = 0;
    public int dailyStar = 0;
    public int dailyReward = 0;
    public int dailyBuy1 = 0;
    public int dailyBuy2 = 0;
    public int dailyBuyCount1 = 0;
    public int dailyBuyCount2 = 0;
    public int dailyNormalBox = 0;
    public int dailyEpicBox = 0;
    public int dailyNormalBox_1 = 3;
    public int dailyNormalBox_10 = 1;
    public int dailyEpicBox_1 = 3;
    public int dailyEpicBox_10 = 1;
    public int dailyAdsReward = 0;
    public int dailyAdsReward2 = 0;
    public int dailyAdsReward3 = 0;
    public int dailyGoldReward = 0;
    public int dailyReset = 0;

    public int package_Daily1 = 0;
    public int package_Daily2 = 0;
    public int package_Daily3 = 0;
    public int package_Daily4 = 0;
    public int package_Daily5 = 0;

    public int package_Weekly1 = 0;
    public int package_Weekly2 = 0;
    public int package_Weekly3 = 0;
    public int package_Weekly4 = 0;
    public int package_Weekly5 = 0;

    public int package_Monthly1 = 0;
    public int package_Monthly2 = 0;
    public int package_Monthly3 = 0;
    public int package_Monthly4 = 0;
    public int package_Monthly5 = 0;
}


public class ResetManager : MonoBehaviour
{
    public static ResetManager instance;

    private ResetInfo resetInfo = new ResetInfo();

    DateTime serverTime;
    DateTime nextMondey;
    DateTime nextMonth;

    bool isFirst = false;
    bool isNextDay = false;
    bool isNextMonday = false;
    bool isNextMonth = false;

    public AttendanceManager attendanceManager;
    public EventManager eventManager;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.6f);

    private Dictionary<string, string> playerData = new Dictionary<string, string>();

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
        PlayfabManager.instance.GetServerTime(SetModeContent);
    }

    private void SetModeContent(DateTime time)
    {
        isFirst = false;
        isNextMonth = false;
        isNextMonday = false;
        isNextDay = false;

        if (playerDataBase.NextMonth.Length < 2)
        {
            Debug.Log("월간 미션 초기화");

            nextMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);

            playerDataBase.NextMonth = nextMonth.ToString("yyyyMMdd");
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("NextMonth", int.Parse(playerDataBase.NextMonth));

            isNextMonth = true;
        }
        else
        {
            if (ComparisonDate(playerDataBase.NextMonth, time))
            {
                Debug.Log("다음 달 1일 되었습니다");

                nextMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);

                playerDataBase.NextMonth = nextMonth.ToString("yyyyMMdd");
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("NextMonth", int.Parse(playerDataBase.NextMonth));

                isNextMonth = true;
            }
            else
            {
                Debug.Log("아직 다음 달 1일이 아닙니다");
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

            isNextMonday = true;
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

                isNextMonday = true;
            }
            else
            {
                Debug.Log("아직 다음 주 월요일이 아닙니다");
            }
        }

        if (playerDataBase.AttendanceDay.Length < 2)
        {
            Debug.Log("일일 미션 초기화");

            isNextDay = true;
            isFirst = true;
        }
        else
        {
            if (ComparisonDate(playerDataBase.AttendanceDay, time))
            {
                Debug.Log("하루가 지났습니다");

                isNextDay = true;
            }
            else
            {
                Debug.Log("아직 하루가 안 지났습니다.");
            }
        }

        ResetValue();

        if (!isFirst)
        {
            StartCoroutine(ResetCoroution());
        }

        StateManager.instance.OtherInitialize();
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
        if (!isNextDay && !isNextMonday && !isNextMonth)
        {
            return;
        }

        FirebaseAnalytics.LogEvent("Reset");

        playerDataBase.AttendanceDay = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
        playerDataBase.AccessDate += 1;

        if (isNextDay)
        {
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

            Debug.Log("일일 초기화");

            playerDataBase.ResetInfo.dailyWin = 0;
            playerDataBase.ResetInfo.dailyStar = 0;
            playerDataBase.ResetInfo.dailyReward = 0;
            playerDataBase.ResetInfo.dailyBuy1 = 0;
            playerDataBase.ResetInfo.dailyBuy2 = 0;
            playerDataBase.ResetInfo.dailyBuyCount1 = 0;
            playerDataBase.ResetInfo.dailyBuyCount2 = 0;
            playerDataBase.ResetInfo.dailyNormalBox = 0;
            playerDataBase.ResetInfo.dailyEpicBox = 0;
            playerDataBase.ResetInfo.dailyNormalBox_1 = 3;
            playerDataBase.ResetInfo.dailyNormalBox_10 = 1;
            playerDataBase.ResetInfo.dailyEpicBox_1 = 3;
            playerDataBase.ResetInfo.dailyEpicBox_10 = 1;
            playerDataBase.ResetInfo.dailyAdsReward = 0;
            playerDataBase.ResetInfo.dailyAdsReward2 = 0;
            playerDataBase.ResetInfo.dailyAdsReward3 = 0;
            playerDataBase.ResetInfo.dailyGoldReward = 0;
            playerDataBase.ResetInfo.dailyReset = 0;

            playerDataBase.ResetInfo.package_Daily1 = 0;
            playerDataBase.ResetInfo.package_Daily2 = 0;
            playerDataBase.ResetInfo.package_Daily3 = 0;
            playerDataBase.ResetInfo.package_Daily4 = 0;
            playerDataBase.ResetInfo.package_Daily5 = 0;
        }

        if (isNextMonday)
        {
            Debug.Log("주간 초기화");

            playerDataBase.ResetInfo.package_Weekly1 = 0;
            playerDataBase.ResetInfo.package_Weekly2 = 0;
            playerDataBase.ResetInfo.package_Weekly3 = 0;
            playerDataBase.ResetInfo.package_Weekly4 = 0;
            playerDataBase.ResetInfo.package_Weekly5 = 0;
        }

        if (isNextMonth)
        {
            Debug.Log("월간 초기화");

            playerDataBase.ResetInfo.package_Monthly1 = 0;
            playerDataBase.ResetInfo.package_Monthly2 = 0;
            playerDataBase.ResetInfo.package_Monthly3 = 0;
            playerDataBase.ResetInfo.package_Monthly4 = 0;
            playerDataBase.ResetInfo.package_Monthly5 = 0;
        }

    }

    IEnumerator ResetCoroution()
    {
        Debug.LogError("초기화 작업 시작");

        if (isNextDay)
        {
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AccessDate", playerDataBase.AccessDate);
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(playerDataBase.AttendanceDay));
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCheck", 0);
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCount", 0);

            yield return waitForSeconds;

            PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCheck", 0);
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeBoxCheck", 0);

        }

        yield return waitForSeconds;

        playerData.Clear();
        playerData.Add("ResetInfo", JsonUtility.ToJson(playerDataBase.ResetInfo));
        PlayfabManager.instance.SetPlayerData(playerData);

        Debug.LogError("초기화 작업 완료");
    }

    public void SetResetInfo(ResetType type)
    {
        switch (type)
        {
            case ResetType.DailyWin:
                playerDataBase.ResetInfo.dailyWin = 1;
                break;
            case ResetType.DailyStar:
                playerDataBase.ResetInfo.dailyStar = 1;
                break;
            case ResetType.DailyReward:
                playerDataBase.ResetInfo.dailyReward = 1;
                break;
            case ResetType.DailyBuy1:
                playerDataBase.ResetInfo.dailyBuy1 = 1;
                break;
            case ResetType.DailyBuy2:
                playerDataBase.ResetInfo.dailyBuy2 = 1;
                break;
            case ResetType.DailyBuyCount1:
                playerDataBase.ResetInfo.dailyBuyCount1 = 1;
                break;
            case ResetType.DailyBuyCount2:
                playerDataBase.ResetInfo.dailyBuyCount2 = 1;
                break;
            case ResetType.DailyNormalBox:
                playerDataBase.ResetInfo.dailyNormalBox = 1;
                break;
            case ResetType.DailyEpicBox:
                playerDataBase.ResetInfo.dailyEpicBox = 1;
                break;
            case ResetType.DailyNormalBox_1:
                playerDataBase.ResetInfo.dailyNormalBox_1 -= 1;
                break;
            case ResetType.DailyNormalBox_10:
                playerDataBase.ResetInfo.dailyNormalBox_10 -= 1;
                break;
            case ResetType.DailyEpicBox_1:
                playerDataBase.ResetInfo.dailyEpicBox_1 -= 1;
                break;
            case ResetType.DailyEpicBox_10:
                playerDataBase.ResetInfo.dailyEpicBox_10 -= 1;
                break;
            case ResetType.DailyAdsReward:
                playerDataBase.ResetInfo.dailyAdsReward = 1;
                break;
            case ResetType.DailyAdsReward2:
                playerDataBase.ResetInfo.dailyAdsReward2 = 1;
                break;
            case ResetType.DailyAdsReward3:
                playerDataBase.ResetInfo.dailyAdsReward3 = 1;
                break;
            case ResetType.DailyGoldReward:
                playerDataBase.ResetInfo.dailyGoldReward = 1;
                break;
            case ResetType.DailyReset:
                playerDataBase.ResetInfo.dailyReset = 1;
                break;
            case ResetType.Package_Daily1:
                playerDataBase.ResetInfo.package_Daily1 = 1;
                break;
            case ResetType.Package_Daily2:
                playerDataBase.ResetInfo.package_Daily2 = 1;
                break;
            case ResetType.Package_Daily3:
                playerDataBase.ResetInfo.package_Daily3 = 1;
                break;
            case ResetType.Package_Daily4:
                playerDataBase.ResetInfo.package_Daily4 = 1;
                break;
            case ResetType.Package_Daily5:
                playerDataBase.ResetInfo.package_Daily5 = 1;
                break;
            case ResetType.Package_Weekly1:
                playerDataBase.ResetInfo.package_Weekly1 = 1;
                break;
            case ResetType.Package_Weekly2:
                playerDataBase.ResetInfo.package_Weekly2 = 1;
                break;
            case ResetType.Package_Weekly3:
                playerDataBase.ResetInfo.package_Weekly3 = 1;
                break;
            case ResetType.Package_Weekly4:
                playerDataBase.ResetInfo.package_Weekly4 = 1;
                break;
            case ResetType.Package_Weekly5:
                playerDataBase.ResetInfo.package_Weekly5 = 1;
                break;
            case ResetType.Package_Monthly1:
                playerDataBase.ResetInfo.package_Monthly1 = 1;
                break;
            case ResetType.Package_Monthly2:
                playerDataBase.ResetInfo.package_Monthly2 = 1;
                break;
            case ResetType.Package_Monthly3:
                playerDataBase.ResetInfo.package_Monthly3 = 1;
                break;
            case ResetType.Package_Monthly4:
                playerDataBase.ResetInfo.package_Monthly4 = 1;
                break;
            case ResetType.Package_Monthly5:
                playerDataBase.ResetInfo.package_Monthly5 = 1;
                break;
        }

        playerData.Clear();
        playerData.Add("ResetInfo", JsonUtility.ToJson(playerDataBase.ResetInfo));
        PlayfabManager.instance.SetPlayerData(playerData);
    }
}
