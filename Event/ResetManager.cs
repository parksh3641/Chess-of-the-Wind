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
    DateTime serverTime;
    DateTime nextMondey;

    public AttendanceManager attendanceManager;
    public EventManager eventManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
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

    private void SetModeContent(System.DateTime time)
    {
        if (playerDataBase.AttendanceDay.Length < 2)
        {
            Debug.Log("데일리 미션 맨 처음 초기화");

            playerDataBase.AttendanceDay = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            playerDataBase.AccessDate += 1;
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AccessDate", playerDataBase.AccessDate);
            PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(playerDataBase.AttendanceDay));

            GameStateManager.instance.DailyWin = false;
            GameStateManager.instance.DailyAdsReward = false;
            GameStateManager.instance.DailyAdsReward2 = false;
            GameStateManager.instance.DailyNormalBox = false;
            GameStateManager.instance.DailyEpicBox = false;
            GameStateManager.instance.DailyNormalBox_1 = 3;
            GameStateManager.instance.DailyNormalBox_10 = 1;
            GameStateManager.instance.DailyEpicBox_1 = 3;
            GameStateManager.instance.DailyEpicBox_10 = 1;

            if (playerDataBase.AttendanceCheck)
            {
                playerDataBase.AttendanceCheck = false;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCheck", 0);

                if (playerDataBase.AttendanceCount >= 7)
                {
                    playerDataBase.AttendanceCount = 0;
                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCount", 0);

                    Debug.Log("출석 체크 보상 리셋");
                }
                else
                {
                    Debug.Log("다음 출석 체크 보상 오픈");
                }

                attendanceManager.OnSetAlarm();
            }

            if(playerDataBase.WelcomeCheck)
            {
                playerDataBase.WelcomeCheck = false;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCheck", 0);

                eventManager.OnSetWelcomeAlarm();
            }
        }
        else
        {
            if (ComparisonDate(playerDataBase.AttendanceDay, time))
            {
                Debug.Log("하루가 지났습니다");

                playerDataBase.AttendanceDay = System.DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                playerDataBase.AccessDate += 1;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("AccessDate", playerDataBase.AccessDate);
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceDay", int.Parse(playerDataBase.AttendanceDay));

                GameStateManager.instance.DailyWin = false;
                GameStateManager.instance.DailyAdsReward = false;
                GameStateManager.instance.DailyAdsReward2 = false;
                GameStateManager.instance.DailyNormalBox = false;
                GameStateManager.instance.DailyEpicBox = false;
                GameStateManager.instance.DailyNormalBox_1 = 3;
                GameStateManager.instance.DailyNormalBox_10 = 1;
                GameStateManager.instance.DailyEpicBox_1 = 3;
                GameStateManager.instance.DailyEpicBox_10 = 1;

                if (playerDataBase.AttendanceCheck)
                {
                    playerDataBase.AttendanceCheck = false;
                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCheck", 0);

                    if(playerDataBase.AttendanceCount >= 7)
                    {
                        playerDataBase.AttendanceCount = 0;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("AttendanceCount", 0);

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
                    PlayfabManager.instance.UpdatePlayerStatisticsInsert("WelcomeCheck", 0);

                    eventManager.OnSetWelcomeAlarm();
                }
            }
            else
            {
                Debug.Log("아직 하루가 안 지났습니다.");
            }
        }

        if (playerDataBase.NextMonday.Length < 2)
        {
            Debug.Log("위클리 미션 초기화");

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
}
