using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveContent : MonoBehaviour
{
    RewardType rewardType = RewardType.Gold;

    public Image mainBackground;
    public Image icon;
    public Text countText;

    ImageDataBase imageDataBase;

    Sprite[] rewardArray;
    Sprite[] rankBackgroundArray;

    private void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rewardArray = imageDataBase.GetRewardArray();

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();
    }

    public void Initialize(RewardType type, int count)
    {
        icon.sprite = rewardArray[(int)type];

        rewardType = type;

        countText.alignment = (TextAnchor)TextAlignment.Right;
        countText.text = MoneyUnitString.ToCurrencyString(count);

        switch (type)
        {
            case RewardType.Gold:
                mainBackground.sprite = rankBackgroundArray[0];
                break;
            case RewardType.UpgradeTicket:
                mainBackground.sprite = rankBackgroundArray[2];
                break;
            case RewardType.Box:
                mainBackground.sprite = rankBackgroundArray[3];
                break;
            case RewardType.Box_N:
                mainBackground.sprite = rankBackgroundArray[0];
                break;
            case RewardType.Box_R:
                mainBackground.sprite = rankBackgroundArray[1];
                break;
            case RewardType.Box_SR:
                mainBackground.sprite = rankBackgroundArray[2];
                break;
            case RewardType.Box_SSR:
                mainBackground.sprite = rankBackgroundArray[3];
                break;
            case RewardType.Box_UR:
                mainBackground.sprite = rankBackgroundArray[4];
                break;
            case RewardType.Box_NR:
                mainBackground.sprite = rankBackgroundArray[0];
                break;
            case RewardType.Box_RSR:
                mainBackground.sprite = rankBackgroundArray[1];
                break;
            case RewardType.Box_SRSSR:
                mainBackground.sprite = rankBackgroundArray[2];
                break;
            case RewardType.ExclusiveTitle:
                mainBackground.sprite = rankBackgroundArray[2];

                countText.alignment = (TextAnchor)TextAlignment.Center;
                countText.text = LocalizationManager.instance.GetString("ExclusiveTitle");
                break;
        }
    }

    public void OpenInfo()
    {
        ReceiveInfoManager.instance.OpenReceiveInfo(rewardType);
    }
}
