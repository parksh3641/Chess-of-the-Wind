using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShopClass
{
    public string catalogVersion = "";
    public string itemClass = "";
    public string itemId = "";
    public string itemInstanceId = "";
    public string virtualCurrency = "";
    public uint price = 0;
}

[CreateAssetMenu(fileName = "ShopDataBase", menuName = "ScriptableObjects/ShopDataBase")]
public class ShopDataBase : ScriptableObject
{
    [Title("GooglePlay")]
    public ShopClass removeAds;
    public ShopClass paidProgress;

    [Space]
    [Title("Item")]
    public List<ShopClass> itemList = new List<ShopClass>();

    public void Initialize()
    {
        removeAds = new ShopClass();
        paidProgress = new ShopClass();

        itemList.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(ItemType)).Length; i ++)
        {
            ShopClass shopClass = new ShopClass();
            itemList.Add(shopClass);
        }

    }

    public List<ShopClass> ItemList
    {
        get
        {
            return itemList;
        }
    }


    public ShopClass RemoveAds
    {
        get
        {
            return removeAds;
        }
        set
        {
            removeAds = value;
        }
    }

    public ShopClass PaidProgress
    {
        get
        {
            return paidProgress;
        }
        set
        {
            paidProgress = value;
        }
    }

    public void SetItem(ShopClass shopClass)
    {
        switch(shopClass.itemId)
        {
            case "Clock":
                itemList[0] = shopClass;
                break;
            case "Shield":
                itemList[1] = shopClass;
                break;
            case "Combo":
                itemList[2] = shopClass;
                break;
            case "Exp":
                itemList[3] = shopClass;
                break;
            case "Slow":
                itemList[4] = shopClass;
                break;
        }
        //itemList.Add(shopClass);

        //itemList = Enumerable.Reverse(itemList).ToList();
    }


    public void SetItemInstanceId(string itemid, string instanceid)
    {
        for(int i = 0; i < itemList.Count; i ++)
        {
            if(itemList[i].itemId.Equals(itemid))
            {
                itemList[i].itemInstanceId = instanceid;
            }
        }
    }

    public string GetItemInstanceId(string itemid)
    {
        string itemInstanceId = "";

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemId.Equals(itemid))
            {
                itemInstanceId = itemList[i].itemInstanceId;
            }
        }

        return itemInstanceId;
    }
}
