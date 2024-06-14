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

    public BlockUIContent blockUIContent;

    public GameObject effect;


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

        effect.SetActive(false);
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
        blockUIContent.gameObject.SetActive(false);

        titleText.text = LocalizationManager.instance.GetString(rewardType.ToString());

        icon.sprite = rewardArray[(int)rewardType];

        effect.SetActive(false);

        switch (rewardType)
        {
            case RewardType.Gold:
                mainBackground.sprite = rankBackgroundArray[0];
                effect.SetActive(true);
                break;
            case RewardType.UpgradeTicket:
                mainBackground.sprite = rankBackgroundArray[2];
                effect.SetActive(true);
                break;
            case RewardType.Box_Normal:
                mainBackground.sprite = rankBackgroundArray[0];

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
            case RewardType.Box_Epic:
                mainBackground.sprite = rankBackgroundArray[2];

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
            case RewardType.Box_Speical:
                mainBackground.sprite = rankBackgroundArray[4];

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
            case RewardType.ExclusiveTitle:
                mainBackground.sprite = rankBackgroundArray[2];
                effect.SetActive(true);
                break;
            case RewardType.None:
                break;
            case RewardType.GoldShop1:
                mainBackground.sprite = rankBackgroundArray[0];
                break;
            case RewardType.GoldShop2:
                mainBackground.sprite = rankBackgroundArray[0];
                break;
            case RewardType.GoldShop3:
                mainBackground.sprite = rankBackgroundArray[0];
                effect.SetActive(true);
                break;
        }

        infoText.text = LocalizationManager.instance.GetString(rewardType.ToString() + "_Info");
    }

    public void OpenBlockInfo(BlockClass block)
    {
        if (!receiveInfoView.activeInHierarchy)
        {
            receiveInfoView.SetActive(true);

            Initialize_Block(block);
        }
        else
        {
            receiveInfoView.SetActive(false);
        }
    }

    void Initialize_Block(BlockClass block)
    {
        blockUIContent.gameObject.SetActive(true);

        blockUIContent.Initialize(block.blockType);
        blockUIContent.Initialize_Rank(block.rankType);

        titleText.text = LocalizationManager.instance.GetString(block.blockType.ToString());

        infoText.text = LocalizationManager.instance.GetString(block.blockType.ToString() + "_Story");
    }
}
