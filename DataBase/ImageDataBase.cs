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

    public Sprite[] rankBannerArray;

    public Sprite[] shopContentArray;

    public Sprite[] blockArray;

    public Sprite[] rankIconArray;

    public Sprite[] rewardArray;

    public Sprite[] formationArray;

    public Sprite[] formationRedArray;

    public Sprite[] emoteArray;

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

    public Sprite[] GetRankBannerArray()
    {
        return rankBannerArray;
    }

    public Sprite[] GetShopContentArray()
    {
        return shopContentArray;
    }

    public Sprite[] GetBlockArray()
    {
        return blockArray;
    }

    public Sprite[] GetRankIconArray()
    {
        return rankIconArray;
    }

    public Sprite[] GetRewardArray()
    {
        return rewardArray;
    }

    public Sprite[] GetFormationArray()
    {
        return formationArray;
    }

    public Sprite[] GetFormationRedArray()
    {
        return formationRedArray;
    }

    public Sprite[] GetEmoteArray()
    {
        return emoteArray;
    }
}
