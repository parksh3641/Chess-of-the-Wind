using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdsManager : MonoBehaviour
{
    public AdmobReward boxNR;
    public AdmobReward boxRSR;
    public AdmobReward adShop1;
    public AdmobReward adShop2;
    public AdmobReward adShop3;
    public AdmobReward goldShop;
    public AdmobReward all;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void WatchAd(int number)
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
                if (playerDataBase.DailyNormalBox == 1) return;

                boxNR.ShowAd(number);
                break;
            case 1:
                if (playerDataBase.DailyEpicBox == 1) return;

                boxRSR.ShowAd(number);
                break;
            case 2:
                if (playerDataBase.DailyAdsReward == 1) return;

                adShop1.ShowAd(number);
                break;
            case 3:
                if (playerDataBase.DailyAdsReward2 == 1) return;

                adShop2.ShowAd(number);
                break;
            case 4:
                if (playerDataBase.DailyAdsReward3 == 1) return;

                adShop3.ShowAd(number);
                break;
            case 5:
                if (playerDataBase.DailyGoldReward == 1) return;

                goldShop.ShowAd(number);
                break;
            case 6:
                if (playerDataBase.DailyReset == 1) return;

                all.ShowAd(number);
                break;

            case 10:
                all.ShowAd(number);
                break;
        }
    }

}
