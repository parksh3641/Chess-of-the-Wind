using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAnimManager : MonoBehaviour
{
    public static ItemAnimManager instance;

    public ItemAnimation upgradeTicketAnimation;

    public ItemAnimation[] boxPieceAnimation;

    public ItemAnimation seasonPassAnimation;

    private int random = 0;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        instance = this;

        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;
    }

    public void GetUpgradeTicket(int number)
    {
        playerDataBase.SetUpgradeTicket(RankType.N, number);
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("UpgradeTicket", playerDataBase.GetUpgradeTicket(RankType.N));

        upgradeTicketAnimation.PlusItem(number);
    }

    public void GetBoxPiece(RankType type, int number)
    {
        switch (type)
        {
            case RankType.N:
                playerDataBase.BoxPiece_N += number;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_N", playerDataBase.BoxPiece_N);
                break;
            case RankType.R:
                playerDataBase.BoxPiece_R += number;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_R", playerDataBase.BoxPiece_R);
                break;
            case RankType.SR:
                playerDataBase.BoxPiece_SR += number;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SR", playerDataBase.BoxPiece_SR);
                break;
            case RankType.SSR:
                playerDataBase.BoxPiece_SSR += number;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_SSR", playerDataBase.BoxPiece_SSR);
                break;
            case RankType.UR:
                playerDataBase.BoxPiece_UR += number;

                PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxPiece_UR", playerDataBase.BoxPiece_UR);
                break;
        }

        boxPieceAnimation[(int)type].SetRank(type);
        boxPieceAnimation[(int)type].PlusItem(number);
    }

    public void GetSeasonPass(int number)
    {
        seasonPassAnimation.PlusItem(number);
    }

    [Button]
    public void GetUpgradeTicket()
    {
        GetUpgradeTicket(Random.Range(1, 16));
    }

    [Button]
    public void GetBoxPiece(int number)
    {
        GetBoxPiece(RankType.N + number, Random.Range(1, 16));
    }
}
