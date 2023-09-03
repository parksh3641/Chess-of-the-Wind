using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryView;

    public GameObject changeBoxView;

    public Text[] inventoryText;

    public Image pieceImg;
    public Text pieceText;

    public ReceiveContent receiveContent;

    private int boxIndex = 0;
    private int boxCount = 0;

    bool isDelay = false;

    Sprite[] rankBackgroundArray;


    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;


    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();

        inventoryView.SetActive(false);
        changeBoxView.SetActive(false);
    }


    public void OpenInventoryView()
    {
        if (!inventoryView.activeInHierarchy)
        {
            inventoryView.SetActive(true);

            Initialize();

            FirebaseAnalytics.LogEvent("Inventory");
        }
        else
        {
            inventoryView.SetActive(false);
        }
    }

    void Initialize()
    {
        inventoryText[0].text = playerDataBase.GetUpgradeTicket(RankType.N).ToString();
        inventoryText[1].text = playerDataBase.BoxPiece_N.ToString() + "/3";
        inventoryText[2].text = playerDataBase.BoxPiece_R.ToString() + "/3";
        inventoryText[3].text = playerDataBase.BoxPiece_SR.ToString() + "/3";
        inventoryText[4].text = playerDataBase.BoxPiece_SSR.ToString() + "/3";
        inventoryText[5].text = playerDataBase.BoxPiece_UR.ToString() + "/3";
    }

    public void ChangeBox(int number)
    {
        if (isDelay) return;

        switch(number)
        {
            case 0:
                if(playerDataBase.BoxPiece_N < 3)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.LowPiece);

                    return;
                }



                break;
            case 1:
                if (playerDataBase.BoxPiece_R < 3)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.LowPiece);

                    return;
                }

                break;
            case 2:
                if (playerDataBase.BoxPiece_SR < 3)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.LowPiece);

                    return;
                }


                break;
            case 3:
                if (playerDataBase.BoxPiece_SSR < 3)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.LowPiece);

                    return;
                }


                break;
            case 4:
                if (playerDataBase.BoxPiece_UR < 3)
                {
                    SoundManager.instance.PlaySFX(GameSfxType.Wrong);

                    NotionManager.instance.UseNotion(NotionType.LowPiece);

                    return;
                }


                break;
        }

        ChangeBoxInitialize(number);
    }

    void ChangeBoxInitialize(int number)
    {
        boxIndex = number;

        changeBoxView.SetActive(true);

        pieceImg.sprite = rankBackgroundArray[number];

        boxCount = 0;

        CountInitialize(boxCount);
    }

    public void CloseChangeBoxView()
    {
        changeBoxView.SetActive(false);
    }

    void CountInitialize(int number)
    {
        switch (boxIndex)
        {
            case 0:
                pieceText.text = playerDataBase.BoxPiece_N + "/" + (3 * (number + 1));

                receiveContent.Initialize(RewardType.Box_N, number + 1);

                break;
            case 1:
                pieceText.text = playerDataBase.BoxPiece_R + "/" + (3 * (number + 1));

                receiveContent.Initialize(RewardType.Box_R, number + 1);
                break;
            case 2:
                pieceText.text = playerDataBase.BoxPiece_SR + "/" + (3 * (number + 1));

                receiveContent.Initialize(RewardType.Box_SR, number + 1);
                break;
            case 3:
                pieceText.text = playerDataBase.BoxPiece_SSR + "/" + (3 * (number + 1));

                receiveContent.Initialize(RewardType.Box_SSR, number + 1);
                break;
            case 4:
                pieceText.text = playerDataBase.BoxPiece_UR + "/" + (3 * (number + 1));

                receiveContent.Initialize(RewardType.Box_UR, number + 1);
                break;
        }
    }


    public void CountUp()
    {
        switch (boxIndex)
        {
            case 0:
                if(playerDataBase.BoxPiece_N < 3 * (boxCount + 2))
                {
                    return;
                }

                break;
            case 1:
                if (playerDataBase.BoxPiece_R < 3 * (boxCount + 2))
                {
                    return;
                }
                break;
            case 2:
                if (playerDataBase.BoxPiece_SR < 3 * (boxCount + 2))
                {
                    return;
                }
                break;
            case 3:
                if (playerDataBase.BoxPiece_SSR < 3 * (boxCount + 2))
                {
                    return;
                }
                break;
            case 4:
                if (playerDataBase.BoxPiece_UR < 3 * (boxCount + 2))
                {
                    return;
                }
                break;
        }

        boxCount++;

        CountInitialize(boxCount);
    }

    public void CountDown()
    {
        if (boxCount <= 0) return;

        boxCount--;

        CountInitialize(boxCount);
    }


    public void ChangeBox()
    {
        if (!NetworkConnect.instance.CheckConnectInternet())
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);

            NotionManager.instance.UseNotion(NotionType.CheckInternet);
            return;
        }

        switch (boxIndex)
        {
            case 0:
                playerDataBase.BoxPiece_N -= (3 * (boxCount + 1));

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_N", playerDataBase.BoxPiece_N);

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_N = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_N", boxCount + 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_N = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_N", boxCount + 1);
                        break;
                }
                break;
            case 1:
                playerDataBase.BoxPiece_R -= (3 * (boxCount + 1));

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_R", playerDataBase.BoxPiece_R);

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_R = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_R", boxCount + 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_R = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_R", boxCount + 1);
                        break;
                }
                break;
            case 2:
                playerDataBase.BoxPiece_SR -= (3 * (boxCount + 1));

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SR", playerDataBase.BoxPiece_SR);

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_SR = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SR", boxCount + 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_SR = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SR", boxCount + 1);
                        break;
                }

                break;
            case 3:
                playerDataBase.BoxPiece_SSR -= (3 * (boxCount + 1));

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SSR", playerDataBase.BoxPiece_SSR);

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_SSR = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_SSR", boxCount + 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_SSR = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_SSR", boxCount + 1);
                        break;
                }

                break;
            case 4:
                playerDataBase.BoxPiece_UR -= (3 * (boxCount + 1));

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_UR", playerDataBase.BoxPiece_UR);

                switch (GameStateManager.instance.WindCharacterType)
                {
                    case WindCharacterType.Winter:
                        playerDataBase.SnowBox_UR = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_UR", boxCount + 1);
                        break;
                    case WindCharacterType.UnderWorld:
                        playerDataBase.UnderworldBox_UR = boxCount + 1;
                        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_UR", boxCount + 1);
                        break;
                }

                break;
        }

        SoundManager.instance.PlaySFX(GameSfxType.BuyShopItem);

        NotionManager.instance.UseNotion(NotionType.GetReward);

        Initialize();

        CloseChangeBoxView();

        isDelay = true;
        Invoke("Delay", 1.0f);
    }

    void Delay()
    {
        isDelay = false;
    }
}
