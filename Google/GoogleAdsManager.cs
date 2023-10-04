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
    public AdmobReward bronze;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void WatchAd(int number)
    {
        switch(number)
        {
            case 0:
                if (GameStateManager.instance.DailyNormalBox) return;

                boxNR.ShowAd(number);
                break;
            case 1:
                if (GameStateManager.instance.DailyEpicBox) return;

                boxRSR.ShowAd(number);
                break;
            case 2:
                if (GameStateManager.instance.DailyAdsReward) return;

                adShop1.ShowAd(number);
                break;
            case 3:
                if (GameStateManager.instance.DailyAdsReward2) return;

                adShop2.ShowAd(number);
                break;
            case 4:
                if (GameStateManager.instance.DailyAdsReward3) return;

                adShop3.ShowAd(number);
                break;
            case 5:
                if (GameStateManager.instance.DailyGoldReward) return;

                goldShop.ShowAd(number);
                break;
            case 6:
                bronze.ShowAd(number);
                break;
        }
    }

}
