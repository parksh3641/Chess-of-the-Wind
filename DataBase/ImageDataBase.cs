using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageDataBase", menuName = "ScriptableObjects/ImageDataBase")]
public class ImageDataBase : ScriptableObject
{
    public Sprite moneyIcon;

    public Sprite[] characterArray;

    public Sprite[] rankBackgroundArray;

    public Sprite[] shopContentArray;

    public Sprite GetMoneyIcon()
    {
        return moneyIcon;
    }

    public Sprite[] GetCharacterArray()
    {
        return characterArray;
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
