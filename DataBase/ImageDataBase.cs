using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageDataBase", menuName = "ScriptableObjects/ImageDataBase")]
public class ImageDataBase : ScriptableObject
{
    public Sprite moneyIcon;

    public Sprite[] iconArray;

    public Sprite[] rankBackgroundArray;

    public Sprite[] shopContentArray;

    public Sprite GetMoneyIcon()
    {
        return moneyIcon;
    }

    public Sprite[] GetIconArray()
    {
        return iconArray;
    }

    public Sprite[] GetRankBackgroundArray()
    {
        return rankBackgroundArray;
    }

    public Sprite[] GetShopContentArray()
    {
        return shopContentArray;
    }
}
