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
}
