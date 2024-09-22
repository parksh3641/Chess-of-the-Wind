using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdsManager : MonoBehaviour
{
    public static GoogleAdsManager instance;

    public AdmobScreen admobScreen;
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
        instance = this;

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
                if (playerDataBase.ResetInfo.dailyNormalBox == 1) return;

                boxNR.ShowAd(number);
                break;
            case 1:
                if (playerDataBase.ResetInfo.dailyEpicBox == 1) return;

                boxRSR.ShowAd(number);
                break;
            case 2:
                if (playerDataBase.ResetInfo.dailyAdsReward == 1) return;

                adShop1.ShowAd(number);
                break;
            case 3:
                if (playerDataBase.ResetInfo.dailyAdsReward2 == 1) return;

                adShop2.ShowAd(number);
                break;
            case 4:
                if (playerDataBase.ResetInfo.dailyAdsReward3 == 1) return;

                adShop3.ShowAd(number);
                break;
            case 5:
                if (playerDataBase.ResetInfo.dailyGoldReward == 1) return;

                goldShop.ShowAd(number);
                break;
            case 6:
                if (playerDataBase.ResetInfo.dailyReset == 1) return;

                all.ShowAd(number);
                break;

            case 10:
                all.ShowAd(number);
                break;
        }
    }

}
