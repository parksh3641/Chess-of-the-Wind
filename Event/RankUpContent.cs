using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankUpContent : MonoBehaviour
{
    public int index = 0;

    public GameRankType gameRankType = GameRankType.Sliver_4;

    public Image icon;

    public LocalizationContent titleText;

    public ReceiveContent[] receiveContents;

    public GameObject foucsObj;
    public GameObject lockObj;
    public GameObject clearObj;

    public EventManager eventManager;

    PlayerDataBase playerDataBase;
    ImageDataBase imageDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;
    }

    public void Initialize(int number, RankUpInfomation rankup, Sprite sp, string name ,string name2, EventManager manager)
    {
        index = number;

        gameRankType = rankup.gameRankType;

        eventManager = manager;

        icon.sprite = sp;
        titleText.localizationName = name;
        titleText.plusText = " " + name2;
        titleText.ReLoad();

        receiveContents[0].gameObject.SetActive(false);
        receiveContents[1].gameObject.SetActive(false);

        if (rankup.receiveInformationList.Count >= 2)
        {
            receiveContents[0].gameObject.SetActive(true);
            receiveContents[1].gameObject.SetActive(true);

            receiveContents[0].Initialize(rankup.receiveInformationList[0].rewardType, rankup.receiveInformationList[0].count);
            receiveContents[1].Initialize(rankup.receiveInformationList[1].rewardType, rankup.receiveInformationList[1].count);
        }
        else
        {
            receiveContents[0].gameObject.SetActive(true);

            receiveContents[0].Initialize(rankup.receiveInformationList[0].rewardType, rankup.receiveInformationList[0].count);
        }
    }

    public void CheckReceived()
    {
        foucsObj.SetActive(false);
        lockObj.SetActive(true);
        clearObj.SetActive(false);

        if (index == playerDataBase.RankUpCount)
        {
            foucsObj.SetActive(true);

            if (gameRankType <= GameStateManager.instance.GameRankType)
            {
                lockObj.SetActive(false);
            }
        }
        else if (index < playerDataBase.RankUpCount)
        {
            clearObj.SetActive(true);
        }
        else
        {
            lockObj.SetActive(true);
        }
    }

    public void ReceiveButton()
    {
        eventManager.RankUpReceiveButton(index);
    }
}
