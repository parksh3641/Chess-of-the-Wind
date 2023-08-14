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
                boxNR.ShowAd(number);
                break;
            case 1:
                boxRSR.ShowAd(number);
                break;
            case 2:
                adShop1.ShowAd(number);
                break;
            case 3:
                adShop2.ShowAd(number);
                break;
            case 4:
                adShop3.ShowAd(number);
                break;
            case 5:
                goldShop.ShowAd(number);
                break;
        }
    }

}
