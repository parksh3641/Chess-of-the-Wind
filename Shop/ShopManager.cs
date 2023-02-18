using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopView;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        shopView.SetActive(false);
    }

    public void OpenShopView()
    {
        if(!shopView.activeSelf)
        {
            shopView.SetActive(true);
        }
    }

    public void CloseShopView()
    {
        shopView.SetActive(false);
    }

    #region RandomBox


    #endregion
}
