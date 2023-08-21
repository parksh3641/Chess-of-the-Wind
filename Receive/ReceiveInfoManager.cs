using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveInfoManager : MonoBehaviour
{
    public static ReceiveInfoManager instance;

    public GameObject receiveInfoView;

    public Image mainBackground;
    public Image icon;
    public Text titleText;
    public Text infoText;


    ImageDataBase imageDataBase;

    Sprite[] rewardArray;
    Sprite[] rankBackgroundArray;

    private void Awake()
    {
        instance = this;

        receiveInfoView.SetActive(false);

        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rewardArray = imageDataBase.GetRewardArray();

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();
    }


    public void CloseReceiveInfo()
    {
        receiveInfoView.SetActive(false);
    }


    public void OpenReceiveInfo(RewardType rewardType)
    {
        if (!receiveInfoView.activeInHierarchy)
        {
            receiveInfoView.SetActive(true);

            Initialize(rewardType);
        }
        else
        {
            receiveInfoView.SetActive(false);
        }
    }

    void Initialize(RewardType rewardType)
    {
        titleText.text = LocalizationManager.instance.GetString(rewardType.ToString());

        icon.sprite = rewardArray[(int)rewardType];

        switch (rewardType)
        {
            case RewardType.Gold:
                mainBackground.sprite = rankBackgroundArray[0];
                break;
            case RewardType.UpgradeTicket:
                mainBackground.sprite = rankBackgroundArray[2];
                break;
            case RewardType.Box:
                mainBackground.sprite = rankBackgroundArray[3];

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        titleText.text = LocalizationManager.instance.GetString("Box_Winter");
                        break;
                    case WindCharacterType.UnderWorld:
                        titleText.text = LocalizationManager.instance.GetString("Box_Under");
                        break;
                }

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
        }

        infoText.text = LocalizationManager.instance.GetString(rewardType.ToString() + "_Info");
    }
}
