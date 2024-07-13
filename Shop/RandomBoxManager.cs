using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class RandomBoxInfo
{
    public List<RandomBox> randomBox_Winter_List = new List<RandomBox>();
    public List<RandomBox> randomBox_Underworld_List = new List<RandomBox>();

    List<float> percent = new List<float>();

    private RandomBox_Block randomBox_Block = new RandomBox_Block();

    public List<float> GetPercent(BoxType boxType)
    {
        percent.Clear();

        switch (boxType)
        {
            case BoxType.Normal:
                if(GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_List.Count; i ++)
                    {
                        percent.Add(randomBox_Winter_List[i].totalPercent_Normal);
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_List.Count; i++)
                    {
                        percent.Add(randomBox_Underworld_List[i].totalPercent_Normal);
                    }
                }
                break;
            case BoxType.Epic:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_List.Count; i++)
                    {
                        percent.Add(randomBox_Winter_List[i].totalPercent_Epic);
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_List.Count; i++)
                    {
                        percent.Add(randomBox_Underworld_List[i].totalPercent_Epic);
                    }
                }
                break;
            case BoxType.Speical:
                if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
                {
                    for (int i = 0; i < randomBox_Winter_List.Count; i++)
                    {
                        percent.Add(randomBox_Winter_List[i].totalPercent_Speical);
                    }
                }
                else
                {
                    for (int i = 0; i < randomBox_Underworld_List.Count; i++)
                    {
                        percent.Add(randomBox_Underworld_List[i].totalPercent_Speical);
                    }
                }
                break;
        }

        return percent;
    }

    [Button]
    public void Initialize()
    {
        randomBox_Winter_List.Clear();
        randomBox_Underworld_List.Clear();

        for (int i = 0; i < 36; i++)
        {
            RandomBox randomBox1 = new RandomBox();
            RandomBox randomBox2 = new RandomBox();

            randomBox_Winter_List.Add(randomBox1);
            randomBox_Underworld_List.Add(randomBox2);
        }

        randomBox_Winter_List[0].boxInfoType = BoxInfoType.Pawn_Snow_N; //크리스탈 하임 - 리디아
        randomBox_Winter_List[1].boxInfoType = BoxInfoType.Pawn_Snow_R;
        randomBox_Winter_List[2].boxInfoType = BoxInfoType.Pawn_Snow_SR;
        randomBox_Winter_List[3].boxInfoType = BoxInfoType.Pawn_Snow_SSR;

        randomBox_Winter_List[0].SetPercent(new float[] { 0.195f, 1.17f, 1.17f, 1.17f, 1.17f }, BoxType.Normal);
        randomBox_Winter_List[1].SetPercent(new float[] { 0.05f, 0.3f, 0.3f, 0.3f, 0.3f }, BoxType.Normal);
        randomBox_Winter_List[2].SetPercent(new float[] { 0.025f, 0.15f, 0.15f, 0.15f, 0.15f }, BoxType.Normal);
        randomBox_Winter_List[3].SetPercent(new float[] { 0.01f, 0.06f, 0.06f, 0.06f, 0.06f }, BoxType.Normal);

        randomBox_Winter_List[0].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Winter_List[1].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Winter_List[2].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Winter_List[3].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);

        randomBox_Winter_List[0].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Winter_List[1].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Winter_List[2].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Winter_List[3].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);


        randomBox_Winter_List[4].boxInfoType = BoxInfoType.Pawn_Snow_2_N; //크리스탈 하임 - 타라
        randomBox_Winter_List[5].boxInfoType = BoxInfoType.Pawn_Snow_2_R;
        randomBox_Winter_List[6].boxInfoType = BoxInfoType.Pawn_Snow_2_SR;
        randomBox_Winter_List[7].boxInfoType = BoxInfoType.Pawn_Snow_2_SSR;

        randomBox_Winter_List[4].SetPercent(new float[] { 1.17f, 0.195f, 1.17f, 1.17f, 1.17f }, BoxType.Normal);
        randomBox_Winter_List[5].SetPercent(new float[] { 0.03f, 0.05f, 0.3f, 0.3f, 0.3f }, BoxType.Normal);
        randomBox_Winter_List[6].SetPercent(new float[] { 0.15f, 0.025f, 0.15f, 0.15f, 0.15f }, BoxType.Normal);
        randomBox_Winter_List[7].SetPercent(new float[] { 0.06f, 0.01f, 0.06f, 0.06f, 0.06f }, BoxType.Normal);

        randomBox_Winter_List[4].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Winter_List[5].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Winter_List[6].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Winter_List[7].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);

        randomBox_Winter_List[4].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Winter_List[5].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Winter_List[6].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Winter_List[7].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);


        randomBox_Winter_List[8].boxInfoType = BoxInfoType.LeftQueen_2_N; //크리스탈 하임 - 마르타
        randomBox_Winter_List[9].boxInfoType = BoxInfoType.LeftQueen_2_R;
        randomBox_Winter_List[10].boxInfoType = BoxInfoType.LeftQueen_2_SR;
        randomBox_Winter_List[11].boxInfoType = BoxInfoType.LeftQueen_2_SSR;

        randomBox_Winter_List[8].SetPercent(new float[] { 2, 2, 0.2f, 2, 0 }, BoxType.Normal);
        randomBox_Winter_List[9].SetPercent(new float[] { 0.5f, 0.5f, 0.05f, 0.5f, 0 }, BoxType.Normal);
        randomBox_Winter_List[10].SetPercent(new float[] { 0.25f, 0.25f, 0.025f, 0.25f, 0 }, BoxType.Normal);
        randomBox_Winter_List[11].SetPercent(new float[] { 0.075f, 0.075f, 0.01f, 0.075f, 0 }, BoxType.Normal);

        randomBox_Winter_List[8].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Epic);
        randomBox_Winter_List[9].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Epic);
        randomBox_Winter_List[10].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Epic);
        randomBox_Winter_List[11].SetPercent(new float[] { 0.75f, 0.75f, 0.1f, 0.75f, 0 }, BoxType.Epic);

        randomBox_Winter_List[8].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Speical);
        randomBox_Winter_List[9].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Speical);
        randomBox_Winter_List[10].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Speical);
        randomBox_Winter_List[11].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Speical);


        randomBox_Winter_List[12].boxInfoType = BoxInfoType.Rook_V2_N; //크리스탈 하임 - 이레네
        randomBox_Winter_List[13].boxInfoType = BoxInfoType.Rook_V2_R;
        randomBox_Winter_List[14].boxInfoType = BoxInfoType.Rook_V2_SR;
        randomBox_Winter_List[15].boxInfoType = BoxInfoType.Rook_V2_SSR;

        randomBox_Winter_List[12].SetPercent(new float[] { 2, 2, 2, 0.2f, 0 }, BoxType.Normal);
        randomBox_Winter_List[13].SetPercent(new float[] { 0.5f, 0.5f, 0.5f, 0.05f, 0 }, BoxType.Normal);
        randomBox_Winter_List[14].SetPercent(new float[] { 0.25f, 0.25f, 0.25f, 0.025f, 0 }, BoxType.Normal);
        randomBox_Winter_List[15].SetPercent(new float[] { 0.075f, 0.075f, 0.075f, 0.01f, 0 }, BoxType.Normal);

        randomBox_Winter_List[12].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Epic);
        randomBox_Winter_List[13].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Epic);
        randomBox_Winter_List[14].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Epic);
        randomBox_Winter_List[15].SetPercent(new float[] { 0.75f, 0.75f, 0.75f, 0.1f, 0 }, BoxType.Epic);

        randomBox_Winter_List[12].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);
        randomBox_Winter_List[13].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);
        randomBox_Winter_List[14].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);
        randomBox_Winter_List[15].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);


        randomBox_Winter_List[16].boxInfoType = BoxInfoType.Rook_V2_2_N; //크리스탈 하임 - 오펠리아
        randomBox_Winter_List[17].boxInfoType = BoxInfoType.Rook_V2_2_R;
        randomBox_Winter_List[18].boxInfoType = BoxInfoType.Rook_V2_2_SR;
        randomBox_Winter_List[19].boxInfoType = BoxInfoType.Rook_V2_2_SSR;

        randomBox_Winter_List[16].SetPercent(new float[] { 0.2f, 2, 2, 2, 0 }, BoxType.Normal);
        randomBox_Winter_List[17].SetPercent(new float[] { 0.05f, 0.5f, 0.5f, 0.5f, 0 }, BoxType.Normal);
        randomBox_Winter_List[18].SetPercent(new float[] { 0.025f, 0.25f, 0.25f, 0.25f, 0 }, BoxType.Normal);
        randomBox_Winter_List[19].SetPercent(new float[] { 0.01f, 0.075f, 0.075f, 0.1f, 0 }, BoxType.Normal);

        randomBox_Winter_List[16].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Epic);
        randomBox_Winter_List[17].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Epic);
        randomBox_Winter_List[18].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Epic);
        randomBox_Winter_List[19].SetPercent(new float[] { 0.1f, 0.75f, 0.75f, 1, 0 }, BoxType.Epic);

        randomBox_Winter_List[16].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);
        randomBox_Winter_List[17].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);
        randomBox_Winter_List[18].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);
        randomBox_Winter_List[19].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);



        randomBox_Winter_List[20].boxInfoType = BoxInfoType.LeftQueen_3_N; //크리스탈 하임 - 카타리나
        randomBox_Winter_List[21].boxInfoType = BoxInfoType.LeftQueen_3_R;
        randomBox_Winter_List[22].boxInfoType = BoxInfoType.LeftQueen_3_SR;
        randomBox_Winter_List[23].boxInfoType = BoxInfoType.LeftQueen_3_SSR;

        randomBox_Winter_List[20].SetPercent(new float[] { 2, 0.2f, 2, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[21].SetPercent(new float[] { 0.5f, 0.05f, 0.5f, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[22].SetPercent(new float[] { 0.25f, 0.025f, 0.25f, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[23].SetPercent(new float[] { 0.1f, 0.01f, 0.1f, 0, 0 }, BoxType.Normal);

        randomBox_Winter_List[20].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[21].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[22].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[23].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);

        randomBox_Winter_List[20].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[21].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[22].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[23].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);



        randomBox_Winter_List[24].boxInfoType = BoxInfoType.LeftNight_N; //크리스탈 하임 - 프로에
        randomBox_Winter_List[25].boxInfoType = BoxInfoType.LeftNight_R;
        randomBox_Winter_List[26].boxInfoType = BoxInfoType.LeftNight_SR;
        randomBox_Winter_List[27].boxInfoType = BoxInfoType.LeftNight_SSR;

        randomBox_Winter_List[24].SetPercent(new float[] { 2, 2, 0.2f, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[25].SetPercent(new float[] { 0.5f, 0.5f, 0.05f, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[26].SetPercent(new float[] { 0.25f, 0.25f, 0.025f, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[27].SetPercent(new float[] { 0.1f, 0.1f, 0.01f, 0, 0 }, BoxType.Normal);

        randomBox_Winter_List[24].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[25].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[26].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[27].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);

        randomBox_Winter_List[24].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[25].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[26].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[27].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Speical);



        randomBox_Winter_List[28].boxInfoType = BoxInfoType.LeftNight_Mirror_N; //크리스탈 하임 - 프레아
        randomBox_Winter_List[29].boxInfoType = BoxInfoType.LeftNight_Mirror_R;
        randomBox_Winter_List[30].boxInfoType = BoxInfoType.LeftNight_Mirror_SR;
        randomBox_Winter_List[31].boxInfoType = BoxInfoType.LeftNight_Mirror_SSR;

        randomBox_Winter_List[28].SetPercent(new float[] { 0.2f, 2, 2, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[29].SetPercent(new float[] { 0.05f, 0.5f, 0.5f, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[30].SetPercent(new float[] { 0.025f, 0.25f, 0.25f, 0, 0 }, BoxType.Normal);
        randomBox_Winter_List[31].SetPercent(new float[] { 0.01f, 0.1f, 0.1f, 0, 0 }, BoxType.Normal);

        randomBox_Winter_List[28].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[29].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[30].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);
        randomBox_Winter_List[31].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);

        randomBox_Winter_List[28].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[29].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[30].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);
        randomBox_Winter_List[31].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);

        randomBox_Winter_List[32].boxInfoType = BoxInfoType.Gold_N; //골드
        randomBox_Winter_List[33].boxInfoType = BoxInfoType.Gold_R;
        randomBox_Winter_List[34].boxInfoType = BoxInfoType.UpgradeTicket_N; //티켓
        randomBox_Winter_List[35].boxInfoType = BoxInfoType.UpgradeTicket_R;

        randomBox_Winter_List[32].SetPercent(new float[] { 2, 2, 2, 2, 2 }, BoxType.Normal);
        randomBox_Winter_List[33].SetPercent(new float[] { 2, 2, 2, 2, 2 }, BoxType.Normal);
        randomBox_Winter_List[34].SetPercent(new float[] { 2.5f, 2.5f, 2.5f, 2.28f, 2 }, BoxType.Normal);
        randomBox_Winter_List[35].SetPercent(new float[] { 2, 2, 2, 2, 2 }, BoxType.Normal);

        randomBox_Winter_List[32].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);
        randomBox_Winter_List[33].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);
        randomBox_Winter_List[34].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);
        randomBox_Winter_List[35].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);

        randomBox_Winter_List[32].SetPercent(new float[] { 0.6f, 0.5f, 0.5f, 0.5f, 0.5f }, BoxType.Speical);
        randomBox_Winter_List[33].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Speical);
        randomBox_Winter_List[34].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Speical);
        randomBox_Winter_List[35].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Speical);


        randomBox_Underworld_List[0].boxInfoType = BoxInfoType.Pawn_Under_N; //지하세계 - 카리아
        randomBox_Underworld_List[1].boxInfoType = BoxInfoType.Pawn_Under_R;
        randomBox_Underworld_List[2].boxInfoType = BoxInfoType.Pawn_Under_SR;
        randomBox_Underworld_List[3].boxInfoType = BoxInfoType.Pawn_Under_SSR;

        randomBox_Underworld_List[0].SetPercent(new float[] { 0.195f, 1.17f, 1.17f, 1.17f, 1.17f }, BoxType.Normal);
        randomBox_Underworld_List[1].SetPercent(new float[] { 0.05f, 0.3f, 0.3f, 0.3f, 0.3f }, BoxType.Normal);
        randomBox_Underworld_List[2].SetPercent(new float[] { 0.025f, 0.15f, 0.15f, 0.15f, 0.15f }, BoxType.Normal);
        randomBox_Underworld_List[3].SetPercent(new float[] { 0.01f, 0.06f, 0.06f, 0.06f, 0.06f }, BoxType.Normal);

        randomBox_Underworld_List[0].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Underworld_List[1].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Underworld_List[2].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Underworld_List[3].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);

        randomBox_Underworld_List[0].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Underworld_List[1].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Underworld_List[2].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Underworld_List[3].SetPercent(new float[] { 0.1f, 0.6f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);


        randomBox_Underworld_List[4].boxInfoType = BoxInfoType.Pawn_Under_2_N; //지하세계 - 코르디아
        randomBox_Underworld_List[5].boxInfoType = BoxInfoType.Pawn_Under_2_R;
        randomBox_Underworld_List[6].boxInfoType = BoxInfoType.Pawn_Under_2_SR;
        randomBox_Underworld_List[7].boxInfoType = BoxInfoType.Pawn_Under_2_SSR;

        randomBox_Underworld_List[4].SetPercent(new float[] { 1.17f, 0.195f, 1.17f, 1.17f, 1.17f }, BoxType.Normal);
        randomBox_Underworld_List[5].SetPercent(new float[] { 0.03f, 0.05f, 0.3f, 0.3f, 0.3f }, BoxType.Normal);
        randomBox_Underworld_List[6].SetPercent(new float[] { 0.15f, 0.025f, 0.15f, 0.15f, 0.15f }, BoxType.Normal);
        randomBox_Underworld_List[7].SetPercent(new float[] { 0.06f, 0.01f, 0.06f, 0.06f, 0.06f }, BoxType.Normal);

        randomBox_Underworld_List[4].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Underworld_List[5].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Underworld_List[6].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);
        randomBox_Underworld_List[7].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Epic);

        randomBox_Underworld_List[4].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Underworld_List[5].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Underworld_List[6].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);
        randomBox_Underworld_List[7].SetPercent(new float[] { 0.6f, 0.1f, 0.6f, 0.6f, 0.6f }, BoxType.Speical);


        randomBox_Underworld_List[8].boxInfoType = BoxInfoType.RightQueen_2_N; //지하세계 - 루시아
        randomBox_Underworld_List[9].boxInfoType = BoxInfoType.RightQueen_2_R;
        randomBox_Underworld_List[10].boxInfoType = BoxInfoType.RightQueen_2_SR;
        randomBox_Underworld_List[11].boxInfoType = BoxInfoType.RightQueen_2_SSR;

        randomBox_Underworld_List[8].SetPercent(new float[] { 2, 2, 0.2f, 2, 0 }, BoxType.Normal);
        randomBox_Underworld_List[9].SetPercent(new float[] { 0.5f, 0.5f, 0.05f, 0.5f, 0 }, BoxType.Normal);
        randomBox_Underworld_List[10].SetPercent(new float[] { 0.25f, 0.25f, 0.025f, 0.25f, 0 }, BoxType.Normal);
        randomBox_Underworld_List[11].SetPercent(new float[] { 0.075f, 0.075f, 0.01f, 0.075f, 0 }, BoxType.Normal);

        randomBox_Underworld_List[8].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Epic);
        randomBox_Underworld_List[9].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Epic);
        randomBox_Underworld_List[10].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Epic);
        randomBox_Underworld_List[11].SetPercent(new float[] { 0.75f, 0.75f, 0.1f, 0.75f, 0 }, BoxType.Epic);

        randomBox_Underworld_List[8].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Speical);
        randomBox_Underworld_List[9].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Speical);
        randomBox_Underworld_List[10].SetPercent(new float[] { 1, 1, 0.1f, 1, 0 }, BoxType.Speical);
        randomBox_Underworld_List[11].SetPercent(new float[] { 1f, 1f, 0.1f, 1, 0 }, BoxType.Speical);


        randomBox_Underworld_List[12].boxInfoType = BoxInfoType.Rook_V4_N; //지하세계 - 아가타
        randomBox_Underworld_List[13].boxInfoType = BoxInfoType.Rook_V4_R;
        randomBox_Underworld_List[14].boxInfoType = BoxInfoType.Rook_V4_SR;
        randomBox_Underworld_List[15].boxInfoType = BoxInfoType.Rook_V4_SSR;

        randomBox_Underworld_List[12].SetPercent(new float[] { 2, 2, 2, 0.2f, 0 }, BoxType.Normal);
        randomBox_Underworld_List[13].SetPercent(new float[] { 0.5f, 0.5f, 0.5f, 0.05f, 0 }, BoxType.Normal);
        randomBox_Underworld_List[14].SetPercent(new float[] { 0.25f, 0.25f, 0.25f, 0.025f, 0 }, BoxType.Normal);
        randomBox_Underworld_List[15].SetPercent(new float[] { 0.075f, 0.075f, 0.075f, 0.01f, 0 }, BoxType.Normal);

        randomBox_Underworld_List[12].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Epic);
        randomBox_Underworld_List[13].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Epic);
        randomBox_Underworld_List[14].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Epic);
        randomBox_Underworld_List[15].SetPercent(new float[] { 0.75f, 0.75f, 0.75f, 0.1f, 0 }, BoxType.Epic);

        randomBox_Underworld_List[12].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);
        randomBox_Underworld_List[13].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);
        randomBox_Underworld_List[14].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);
        randomBox_Underworld_List[15].SetPercent(new float[] { 1, 1, 1, 0.1f, 0 }, BoxType.Speical);


        randomBox_Underworld_List[16].boxInfoType = BoxInfoType.Rook_V2_2_N; //지하세계 - 세베라
        randomBox_Underworld_List[17].boxInfoType = BoxInfoType.Rook_V2_2_R;
        randomBox_Underworld_List[18].boxInfoType = BoxInfoType.Rook_V2_2_SR;
        randomBox_Underworld_List[19].boxInfoType = BoxInfoType.Rook_V2_2_SSR;

        randomBox_Underworld_List[16].SetPercent(new float[] { 0.2f, 2, 2, 2, 0 }, BoxType.Normal);
        randomBox_Underworld_List[17].SetPercent(new float[] { 0.05f, 0.5f, 0.5f, 0.5f, 0 }, BoxType.Normal);
        randomBox_Underworld_List[18].SetPercent(new float[] { 0.025f, 0.25f, 0.25f, 0.25f, 0 }, BoxType.Normal);
        randomBox_Underworld_List[19].SetPercent(new float[] { 0.01f, 0.075f, 0.075f, 0.1f, 0 }, BoxType.Normal);

        randomBox_Underworld_List[16].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Epic);
        randomBox_Underworld_List[17].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Epic);
        randomBox_Underworld_List[18].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Epic);
        randomBox_Underworld_List[19].SetPercent(new float[] { 0.1f, 0.75f, 0.75f, 1, 0 }, BoxType.Epic);

        randomBox_Underworld_List[16].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);
        randomBox_Underworld_List[17].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);
        randomBox_Underworld_List[18].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);
        randomBox_Underworld_List[19].SetPercent(new float[] { 0.1f, 1, 1, 1, 0 }, BoxType.Speical);



        randomBox_Underworld_List[20].boxInfoType = BoxInfoType.LeftQueen_3_N; //지하세계 - 베로니카
        randomBox_Underworld_List[21].boxInfoType = BoxInfoType.LeftQueen_3_R;
        randomBox_Underworld_List[22].boxInfoType = BoxInfoType.LeftQueen_3_SR;
        randomBox_Underworld_List[23].boxInfoType = BoxInfoType.LeftQueen_3_SSR;

        randomBox_Underworld_List[20].SetPercent(new float[] { 2, 0.2f, 2, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[21].SetPercent(new float[] { 0.5f, 0.05f, 0.5f, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[22].SetPercent(new float[] { 0.25f, 0.025f, 0.25f, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[23].SetPercent(new float[] { 0.1f, 0.01f, 0.1f, 0, 0 }, BoxType.Normal);

        randomBox_Underworld_List[20].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[21].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[22].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[23].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Epic);

        randomBox_Underworld_List[20].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[21].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[22].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[23].SetPercent(new float[] { 1, 0.1f, 1, 0, 0 }, BoxType.Speical);


        randomBox_Underworld_List[24].boxInfoType = BoxInfoType.RightNight_N; //지하세계 - 요안나
        randomBox_Underworld_List[25].boxInfoType = BoxInfoType.RightNight_R;
        randomBox_Underworld_List[26].boxInfoType = BoxInfoType.RightNight_SR;
        randomBox_Underworld_List[27].boxInfoType = BoxInfoType.RightNight_SSR;

        randomBox_Underworld_List[24].SetPercent(new float[] { 2, 2, 0.2f, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[25].SetPercent(new float[] { 0.5f, 0.5f, 0.05f, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[26].SetPercent(new float[] { 0.25f, 0.25f, 0.025f, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[27].SetPercent(new float[] { 0.1f, 0.1f, 0.01f, 0, 0 }, BoxType.Normal);

        randomBox_Underworld_List[24].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[25].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[26].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[27].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Epic);

        randomBox_Underworld_List[24].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[25].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[26].SetPercent(new float[] { 1, 1, 0.1f, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[27].SetPercent(new float[] { 0.6f, 1, 0.1f, 0, 0 }, BoxType.Speical);



        randomBox_Underworld_List[28].boxInfoType = BoxInfoType.RightNight_Mirror_N; //지하세계 - 카롤리나
        randomBox_Underworld_List[29].boxInfoType = BoxInfoType.RightNight_Mirror_R;
        randomBox_Underworld_List[30].boxInfoType = BoxInfoType.RightNight_Mirror_SR;
        randomBox_Underworld_List[31].boxInfoType = BoxInfoType.RightNight_Mirror_SSR;

        randomBox_Underworld_List[28].SetPercent(new float[] { 0.2f, 2, 2, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[29].SetPercent(new float[] { 0.05f, 0.5f, 0.5f, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[30].SetPercent(new float[] { 0.025f, 0.25f, 0.25f, 0, 0 }, BoxType.Normal);
        randomBox_Underworld_List[31].SetPercent(new float[] { 0.01f, 0.1f, 0.1f, 0, 0 }, BoxType.Normal);

        randomBox_Underworld_List[28].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[29].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[30].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);
        randomBox_Underworld_List[31].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Epic);

        randomBox_Underworld_List[28].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[29].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[30].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);
        randomBox_Underworld_List[31].SetPercent(new float[] { 0.1f, 1, 1, 0, 0 }, BoxType.Speical);

        randomBox_Underworld_List[32].boxInfoType = BoxInfoType.Gold_N; //골드
        randomBox_Underworld_List[33].boxInfoType = BoxInfoType.Gold_R;
        randomBox_Underworld_List[34].boxInfoType = BoxInfoType.UpgradeTicket_N; //티켓
        randomBox_Underworld_List[35].boxInfoType = BoxInfoType.UpgradeTicket_R;

        randomBox_Underworld_List[32].SetPercent(new float[] { 2, 2, 2, 2, 2 }, BoxType.Normal);
        randomBox_Underworld_List[33].SetPercent(new float[] { 2, 2, 2, 2, 2 }, BoxType.Normal);
        randomBox_Underworld_List[34].SetPercent(new float[] { 2.5f, 2.5f, 2.5f, 2.28f, 2 }, BoxType.Normal);
        randomBox_Underworld_List[35].SetPercent(new float[] { 2, 2, 2, 2, 2 }, BoxType.Normal);

        randomBox_Underworld_List[32].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);
        randomBox_Underworld_List[33].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);
        randomBox_Underworld_List[34].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);
        randomBox_Underworld_List[35].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Epic);

        randomBox_Underworld_List[32].SetPercent(new float[] { 0.6f, 0.5f, 0.5f, 0.5f, 0.5f }, BoxType.Speical);
        randomBox_Underworld_List[33].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Speical);
        randomBox_Underworld_List[34].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Speical);
        randomBox_Underworld_List[35].SetPercent(new float[] { 1, 1, 1, 1, 1 }, BoxType.Speical);

        RandomBoxManager.isActive = true;

        Debug.LogError("상자 확률 초기화 완료");
    }

    public RandomBox_Block GetRandom(BoxType boxType, int index)
    {
        if (GameStateManager.instance.WindCharacterType == WindCharacterType.Winter)
        {
            randomBox_Block = randomBox_Winter_List[index].GetRandom(boxType);
        }
        else
        {
            randomBox_Block = randomBox_Underworld_List[index].GetRandom(boxType);
        }

        return randomBox_Block;
    }

}


[System.Serializable]
public class RandomBox
{
    public BoxInfoType boxInfoType = BoxInfoType.RightQueen_2_N;

    public float totalPercent_Normal = 0; //전체 비율에서
    public float totalPercent_Epic = 0; //전체 비율에서
    public float totalPercent_Speical = 0; //전체 비율에서

    public float[] normalPercent = new float[5]; //걸렸을 경우에서 또 나누기
    public float[] epicPercent = new float[5]; //걸렸을 경우에서 또 나누기
    public float[] speicalPercent = new float[5]; //걸렸을 경우에서 또 나누기

    float random = 0;
    float percentTotal = 0;

    public void SetPercent(float[] value, BoxType boxType)
    {
        switch (boxType)
        {
            case BoxType.Normal:
                totalPercent_Normal = 0;
                normalPercent = value;

                for (int i = 0; i < normalPercent.Length; i++)
                {
                    totalPercent_Normal += normalPercent[i];
                }
                break;
            case BoxType.Epic:
                totalPercent_Epic = 0;
                epicPercent = value;

                for (int i = 0; i < epicPercent.Length; i++)
                {
                    totalPercent_Epic += epicPercent[i];
                }
                break;
            case BoxType.Speical:
                totalPercent_Speical = 0;
                speicalPercent = value;

                for (int i = 0; i < speicalPercent.Length; i++)
                {
                    totalPercent_Speical += speicalPercent[i];
                }
                break;
        }
    }

    public RandomBox_Block GetRandom(BoxType type) //몇번째 조각이 당첨되었는지 알려주기
    {
        RandomBox_Block randomBox_Block = new RandomBox_Block();

        switch (type)
        {
            case BoxType.Normal:
                random = Random.Range(0f, totalPercent_Normal);

                percentTotal = 0;

                for (int i = 0; i < normalPercent.Length; i++)
                {
                    percentTotal += normalPercent[i];

                    if (random <= percentTotal)
                    {
                        randomBox_Block.boxInfoType = boxInfoType;
                        randomBox_Block.number = i;
                        break;
                    }
                }
                break;
            case BoxType.Epic:
                random = Random.Range(0f, totalPercent_Epic);

                percentTotal = 0;

                for (int i = 0; i < epicPercent.Length; i++)
                {
                    percentTotal += epicPercent[i];

                    if (random <= percentTotal)
                    {
                        randomBox_Block.boxInfoType = boxInfoType;
                        randomBox_Block.number = i;
                        break;
                    }
                }
                break;
            case BoxType.Speical:
                random = Random.Range(0f, totalPercent_Speical);

                percentTotal = 0;

                for (int i = 0; i < speicalPercent.Length; i++)
                {
                    percentTotal += speicalPercent[i];

                    if (random <= percentTotal)
                    {
                        randomBox_Block.boxInfoType = boxInfoType;
                        randomBox_Block.number = i;
                        break;
                    }
                }
                break;
        }

        return randomBox_Block;
    }
}

[System.Serializable]
public class RandomBox_Block //당첨된 블럭 정보
{
    public BoxInfoType boxInfoType = BoxInfoType.RightQueen_2_N;
    public int number = 0;
    public int value = 0;

    private string infoType = "";
    

    public void SetRank(RankType type)
    {
        infoType = RandomBoxManager.SplitStringAtLastDelimiter(boxInfoType.ToString(),'_')[0];

        infoType += "_" + type.ToString();

        boxInfoType = (BoxInfoType)Enum.Parse(typeof(BoxInfoType), infoType);
    }
}


public class RandomBoxManager : MonoBehaviour
{
    public WindCharacterType windCharacterType = WindCharacterType.Winter;
    public BoxType boxType = BoxType.Normal;

    public GameObject boxView;

    public GameObject boxOpenView;

    public FadeInOut fadeInOut;

    public Text boxCountText;

    [Title("Content")]
    public BlockUIContent blockUIContent;

    public Transform blockUIContentTransform;

    public List<BlockUIContent> blockUIContentList = new List<BlockUIContent>();

    [Title("Box")]
    public Image boxIcon;
    public Sprite[] boxInitIcon;
    public Sprite[] boxOpenIcon;
    public GameObject boxOpenEffect;
    public GameObject gradient;

    public GameObject boxPanel;
    public GameObject closePanel;
    public ButtonScaleAnimation boxAnim;
    public GameObject tapObj;

    [Title("Detail")]
    public BlockUIContent blockUIContent_Detail;
    public Text blockTitleText;
    public Text nextText;
    public GameObject blockUIEffect;
    public Text nextBoxTapObj;

    [Title("Value")]
    public int boxCount = 0;
    public int boxCountSave = 0;
    public int boxIndex = 0;

    private bool isStart = false;
    private bool isDelay = false;

    private float random = 0;
    private int getGold = 0;
    private int totalgold = 0;
    private int getUpgradeTicket = 0;
    private int totalUpgradeTicket = 0;
    private int remainder = 0;

    private float percentTotal = 0;
    public static bool isActive = true;

    private bool confirmationSR = false;
    private bool confirmationSSR = false;

    [SerializeField]
    private List<float> percent = new List<float>(); //확률

    [SerializeField]
    private List<int> prize = new List<int>(); //당첨된 것 (숫자)

    [SerializeField]
    private List<RandomBox_Block> prize_Block = new List<RandomBox_Block>(); //당첨된 블럭 조각

    private string[] titleArray = new string[2];
    private string countStr = "";

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.5f);

    private Dictionary<string, string> playerData = new Dictionary<string, string>();

    public ShopManager shopManager;
    public InventoryManager inventoryManager;

    PlayerDataBase playerDataBase;

    private void Awake()
    {
        if (playerDataBase == null) playerDataBase = Resources.Load("PlayerDataBase") as PlayerDataBase;

        isActive = true;

        for (int i = 0; i < 100; i++)
        {
            BlockUIContent monster = Instantiate(blockUIContent);
            monster.transform.SetParent(blockUIContentTransform);
            monster.transform.position = Vector3.zero;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = Vector3.one;
            monster.gameObject.SetActive(false);

            blockUIContentList.Add(monster);
        }

        ResetView();
    }

    void ResetView()
    {
        isStart = false;
        isDelay = false;

        boxView.SetActive(false);
        boxPanel.SetActive(false);
        closePanel.SetActive(false);

        boxOpenEffect.SetActive(false);

        tapObj.SetActive(false);

        gradient.SetActive(false);

        boxOpenView.SetActive(false);
        blockUIEffect.SetActive(false);

        boxIndex = 0;
    }

    private void OnEnable()
    {
        PlayerDataBase.eGetSnowBox_Normal += OpenSnowBox_Normal;
        PlayerDataBase.eGetSnowBox_Epic += OpenSnowBox_Epic;
        PlayerDataBase.eGetSnowBox_Speical += OpenSnowBox_Speical;

        PlayerDataBase.eGetUnderworldBox_Normal += OpenUnderworldBox_Normal;
        PlayerDataBase.eGetUnderworldBox_Epic += OpenUnderworldBox_Epic;
        PlayerDataBase.eGetUnderworldBox_Speical += OpenUnderworldBox_Speical;
    }

    private void OnDisable()
    {
        PlayerDataBase.eGetSnowBox_Normal -= OpenSnowBox_Normal;
        PlayerDataBase.eGetSnowBox_Epic -= OpenSnowBox_Epic;
        PlayerDataBase.eGetSnowBox_Speical -= OpenSnowBox_Speical;

        PlayerDataBase.eGetUnderworldBox_Normal -= OpenUnderworldBox_Normal;
        PlayerDataBase.eGetUnderworldBox_Epic -= OpenUnderworldBox_Epic;
        PlayerDataBase.eGetUnderworldBox_Speical -= OpenUnderworldBox_Speical;
    }

    private void OnApplicationQuit()
    {
        PlayerDataBase.eGetSnowBox_Normal -= OpenSnowBox_Normal;
        PlayerDataBase.eGetSnowBox_Epic -= OpenSnowBox_Epic;
        PlayerDataBase.eGetSnowBox_Speical -= OpenSnowBox_Speical;

        PlayerDataBase.eGetUnderworldBox_Normal -= OpenUnderworldBox_Normal;
        PlayerDataBase.eGetUnderworldBox_Epic -= OpenUnderworldBox_Epic;
        PlayerDataBase.eGetUnderworldBox_Speical -= OpenUnderworldBox_Speical;
    }

    void OpenSnowBox_Normal()
    {
        StartCoroutine(OpenSnowBox_Normal_Coroution());
    }

    void OpenSnowBox_Epic()
    {
        StartCoroutine(OpenSnowBox_Epic_Coroution());
    }

    void OpenSnowBox_Speical()
    {
        StartCoroutine(OpenSnowBox_Speical_Coroution());
    }

    public IEnumerator OpenSnowBox_Normal_Coroution()
    {
        while (!isActive)
        {
            yield return null;
        }

        Debug.Log("일반 상자 열기 : 눈의 여왕");

        boxType = BoxType.Normal;

        boxCount = playerDataBase.SnowBox_Normal;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public IEnumerator OpenSnowBox_Epic_Coroution()
    {
        while (!isActive)
        {
            yield return null;
        }

        Debug.Log("에픽 상자 열기 : 눈의 여왕");

        boxType = BoxType.Epic;

        boxCount = playerDataBase.SnowBox_Epic;

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    public IEnumerator OpenSnowBox_Speical_Coroution()
    {
        while (!isActive)
        {
            yield return null;
        }

        Debug.Log("전설 상자 열기 : 눈의 여왕");

        boxType = BoxType.Speical;

        boxCount = playerDataBase.SnowBox_Speical;

        playerDataBase.BuySnowBoxSSRCount += boxCount;

        if (playerDataBase.BuySnowBoxSSRCount >= 50)
        {
            playerDataBase.BuySnowBoxSSRCount -= 50;

            confirmationSSR = true;
        }
        else
        {
            confirmationSSR = false;
        }

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuySnowBoxSSRCount", playerDataBase.BuySnowBoxSSRCount);

        if (boxCount >= 10)
        {
            confirmationSR = true;
        }
        else
        {
            confirmationSR = false;
        }

        if (boxCount > 0)
        {
            OpenSnowBox_Initialize();
        }
    }

    void OpenUnderworldBox_Normal()
    {
        StartCoroutine(OpenUnderworldBox_Normal_Coroution());
    }

    void OpenUnderworldBox_Epic()
    {
        StartCoroutine(OpenUnderworldBox_Epic_Coroution());
    }

    void OpenUnderworldBox_Speical()
    {
        StartCoroutine(OpenUnderworldBox_Speical_Coroution());
    }

    public IEnumerator OpenUnderworldBox_Normal_Coroution()
    {
        while (!isActive)
        {
            yield return null;
        }

        Debug.Log("일반 상자 열기 : 지하 세계");

        boxType = BoxType.Normal;

        boxCount = playerDataBase.UnderworldBox_Normal;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public IEnumerator OpenUnderworldBox_Epic_Coroution()
    {
        while (!isActive)
        {
            yield return null;
        }

        Debug.Log("에픽 상자 열기 : 지하 세계");

        boxType = BoxType.Epic;

        boxCount = playerDataBase.UnderworldBox_Epic;

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public IEnumerator OpenUnderworldBox_Speical_Coroution()
    {
        while (!isActive)
        {
            yield return null;
        }

        Debug.Log("전설 상자 열기 : 지하 세계");

        boxType = BoxType.Speical;

        boxCount = playerDataBase.UnderworldBox_Speical;
        playerDataBase.BuyUnderworldBoxSSRCount += boxCount;

        if (playerDataBase.BuyUnderworldBoxSSRCount >= 50)
        {
            playerDataBase.BuyUnderworldBoxSSRCount -= 50;

            confirmationSSR = true;
        }
        else
        {
            confirmationSSR = false;
        }

        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BuyUnderworldBoxSSRCount", playerDataBase.BuyUnderworldBoxSSRCount);

        if (boxCount >= 10)
        {
            confirmationSR = true;
        }
        else
        {
            confirmationSR = false;
        }

        if (boxCount > 0)
        {
            OpenUnderworldBox_Initialize();
        }
    }

    public void OpenSnowBox_Initialize()
    {
        ResetView();

        boxIcon.sprite = boxInitIcon[(int)boxType];

        boxCountText.text = boxCount.ToString();
        boxCountSave = boxCount;

        playerDataBase.BoxOpenCount += boxCount;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxOpenCount", playerDataBase.BoxOpenCount);

        windCharacterType = WindCharacterType.Winter;

        percent = playerDataBase.RandomBoxInfo.GetPercent(boxType);

        prize.Clear();
        prize_Block.Clear();

        boxView.SetActive(true);

        if(boxAnim.gameObject.activeInHierarchy)
        {
            boxAnim.PlayAnim();
        }

        RandomBox();
    }

    public void OpenUnderworldBox_Initialize()
    {
        ResetView();

        boxIcon.sprite = boxInitIcon[(int)boxType];

        boxCountText.text = boxCount.ToString();
        boxCountSave = boxCount;

        playerDataBase.BoxOpenCount += boxCount;
        PlayfabManager.instance.UpdatePlayerStatisticsInsert("BoxOpenCount", playerDataBase.BoxOpenCount);

        windCharacterType = WindCharacterType.UnderWorld;

        percent = playerDataBase.RandomBoxInfo.GetPercent(boxType);

        prize.Clear();
        prize_Block.Clear();

        boxView.SetActive(true);
        if (boxAnim.gameObject.activeInHierarchy)
        {
            boxAnim.PlayAnim();
        }

        RandomBox();
    }

    public void CloseBoxView()
    {
        boxView.SetActive(false);
    }

    public void OpenBox()
    {
        if(!isStart)
        {
            SoundManager.instance.PlaySFX(GameSfxType.Wrong);
            NotionManager.instance.UseNotion(NotionType.WaitTimeNotion);
            return;
        }

        isStart = false;

        boxIcon.sprite = boxOpenIcon[(int)boxType];
        gradient.SetActive(false);
        boxOpenEffect.SetActive(true);
        boxAnim.StopAnim();

        SoundManager.instance.PlaySFX(GameSfxType.BoxOpen);
        SoundManager.instance.PlaySFX(GameSfxType.BoxOpen2);

        StartCoroutine(NextButtonCoroution());
    }

    IEnumerator NextButtonCoroution()
    {
        yield return new WaitForSeconds(0.3f);

        fadeInOut.gameObject.SetActive(true);
        fadeInOut.FadeOutToIn();

        yield return new WaitForSeconds(1.5f);

        NextButton();
    }

    void RandomBox()
    {
        while (boxCount > 0)
        {
            boxCount -= 1;

            random = Random.Range(0f, 100.0f);

            percentTotal = 0;

            for (int i = 0; i < percent.Count; i++)
            {
                percentTotal += percent[i];

                if (random <= percentTotal)
                {
                    prize.Add(i);
                    break;
                }
            }
        }

        if(boxCountSave != prize.Count)
        {
            boxCount = boxCountSave - prize.Count;
            RandomBox();

            Debug.Log("오류가 발생하여 남은 개수만큼 다시 상자를 뽑습니다");
            return;
        }

        isStart = true;

        BoxOff();
    }

    void BoxOff()
    {
        Debug.Log("조각 번호에 따라 조각을 저장중입니다");

        prize_Block.Clear();

        totalgold = 0;
        totalUpgradeTicket = 0;

        for (int i = 0; i < prize.Count; i ++)
        {
            prize_Block.Add(playerDataBase.RandomBoxInfo.GetRandom(boxType, prize[i]));

            if (confirmationSSR)
            {
                if (prize_Block[i].boxInfoType < BoxInfoType.Gold_N && SplitStringAtLastDelimiter(prize_Block[i].boxInfoType.ToString(),'_')[1].Equals("SSR"))
                {
                    confirmationSSR = false;

                    prize_Block[i].SetRank(RankType.SSR);

                    Debug.Log("SSR 확정");
                }
            }
            else if (confirmationSR)
            {
                if (prize_Block[i].boxInfoType < BoxInfoType.Gold_N && SplitStringAtLastDelimiter(prize_Block[i].boxInfoType.ToString(), '_')[1].Equals("SR"))
                {
                    confirmationSR = false;

                    prize_Block[i].SetRank(RankType.SR);

                    Debug.Log("SR 확정");
                }
            }
        }

        if(prize_Block[0].boxInfoType.ToString().Contains("SSR"))
        {
            gradient.SetActive(true);
        }

        for(int i = 0; i < prize_Block.Count; i ++)
        {
            switch (prize_Block[i].boxInfoType)
            {
                case BoxInfoType.RightQueen_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.N, prize_Block[i].number);
                    break;


                case BoxInfoType.RightQueen_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.R, prize_Block[i].number);
                    break;


                case BoxInfoType.RightQueen_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.SR, prize_Block[i].number);
                    break;


                case BoxInfoType.RightQueen_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightQueen_3_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightQueen_3, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.RightNight_Mirror_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.RightNight_Mirror, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V2_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V2_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Under_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Under_2, RankType.SSR, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.N, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_N:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.N, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.R, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_R:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.R, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.SR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_SR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.SR, prize_Block[i].number);
                    break;


                case BoxInfoType.LeftQueen_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftQueen_3_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftQueen_3, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.LeftNight_Mirror_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.LeftNight_Mirror, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Rook_V4_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Rook_V4_2, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow, RankType.SSR, prize_Block[i].number);
                    break;
                case BoxInfoType.Pawn_Snow_2_SSR:
                    playerDataBase.PieceInfo.AddPiece(BlockType.Pawn_Snow_2, RankType.SSR, prize_Block[i].number);
                    break;
            }
        }

        Invoke("ServerDelay", 1.0f);

        playerData.Clear();
        playerData.Add("PieceInfo", JsonUtility.ToJson(playerDataBase.PieceInfo));
        PlayfabManager.instance.SetPlayerData(playerData);

        shopManager.Change();
        inventoryManager.CheckingFusion();

        Debug.LogError("조각 서버 저장 완료");
    }

    void ServerDelay()
    {
        switch (boxType)
        {
            case BoxType.Normal:
                playerDataBase.SnowBox_Normal = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Normal", 0);

                playerDataBase.UnderworldBox_Normal = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Normal", 0);
                break;
            case BoxType.Epic:
                playerDataBase.SnowBox_Epic = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Epic", 0);

                playerDataBase.UnderworldBox_Epic = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Epic", 0);
                break;
            case BoxType.Speical:
                playerDataBase.SnowBox_Speical = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("SnowBox_Speical", 0);

                playerDataBase.UnderworldBox_Speical = 0;
                PlayfabManager.instance.UpdatePlayerStatisticsInsert("UnderworldBox_Speical", 0);
                break;
        }
    }

    public void NextButton()
    {
        if (isDelay) return;

        getGold = 0;
        getUpgradeTicket = 0;

        if (boxIndex >= prize.Count)
        {
            boxOpenView.SetActive(false);

            StartCoroutine(OpenBoxCoroution());
        }

        if (boxIndex < boxCountSave)
        {
            boxOpenView.SetActive(true);

            blockUIEffect.SetActive(false);

            if(prize_Block[boxIndex] == null)
            {

                return;
            }

            if (prize_Block[boxIndex].boxInfoType.ToString().Contains(RankType.SSR.ToString()))
            {
                fadeInOut.gameObject.SetActive(true);
                fadeInOut.FadeOut();

                blockUIEffect.SetActive(true);

                SoundManager.instance.PlaySFX(GameSfxType.BoxOpen);
            }
            else
            {
                SoundManager.instance.PlaySFX(GameSfxType.GetBlock);
            }

            blockUIContent_Detail.Initialize_RandomBox(prize_Block[boxIndex]);

            blockTitleText.text = "";

            titleArray = SplitStringAtLastDelimiter(prize_Block[boxIndex].boxInfoType.ToString(), '_');

            if (prize_Block[boxIndex].boxInfoType < BoxInfoType.Gold_N)
            {
                blockTitleText.text = LocalizationManager.instance.GetString(titleArray[0]) + "  " + titleArray[1] + "\n" + 
    LocalizationManager.instance.GetString("Piece") + "  " + (prize_Block[boxIndex].number + 1);
            }
            else
            {
                switch (prize_Block[boxIndex].boxInfoType)
                {
                    case BoxInfoType.Gold_N:
                        switch(prize_Block[boxIndex].number)
                        {
                            case 0:
                                getGold = Random.Range(5000, 10001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 1:
                                getGold = Random.Range(10000, 20001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 2:
                                getGold = Random.Range(20000, 30001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 3:
                                getGold = Random.Range(30000, 40001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 4:
                                getGold = Random.Range(40000, 50001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                        }

                        break;
                    case BoxInfoType.Gold_R:
                        switch (prize_Block[boxIndex].number)
                        {
                            case 0:
                                getGold = Random.Range(50000, 60001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 1:
                                getGold = Random.Range(60000, 70001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 2:
                                getGold = Random.Range(70000, 80001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 3:
                                getGold = Random.Range(80000, 90001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                            case 4:
                                getGold = Random.Range(90000, 100001);

                                getGold = RoundToNearestTens(getGold);

                                countStr = getGold.ToString();
                                break;
                        }
                        break;
                    case BoxInfoType.UpgradeTicket_N:
                        switch (prize_Block[boxIndex].number)
                        {
                            case 0:
                                getUpgradeTicket = 1;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 1:
                                getUpgradeTicket = 2;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 2:
                                getUpgradeTicket = 3;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 3:
                                getUpgradeTicket = 4;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 4:
                                getUpgradeTicket = 5;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                        }
                        break;
                    case BoxInfoType.UpgradeTicket_R:
                        switch (prize_Block[boxIndex].number)
                        {
                            case 0:
                                getUpgradeTicket = 6;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 1:
                                getUpgradeTicket = 7;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 2:
                                getUpgradeTicket = 8;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 3:
                                getUpgradeTicket = 9;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                            case 4:
                                getUpgradeTicket = 10;

                                countStr = "x" + getUpgradeTicket.ToString();
                                break;
                        }
                        break;
                }

                if(getGold > 0)
                {
                    totalgold += getGold;

                    prize_Block[boxIndex].value = getGold;
                }

                if(getUpgradeTicket > 0)
                {
                    totalUpgradeTicket += getUpgradeTicket;

                    prize_Block[boxIndex].value = getUpgradeTicket;
                }

                blockUIContent_Detail.Initialize_RandomBox(prize_Block[boxIndex]);

                blockTitleText.text = LocalizationManager.instance.GetString(titleArray[0]) + "\n" + countStr;
            }
            nextText.text = (boxIndex + 1) + "/" + boxCountSave;

            boxIndex++;
        }
        else
        {
            boxOpenView.SetActive(false);

            StartCoroutine(OpenBoxCoroution());
        }

        if (boxIndex == boxCountSave)
        {
            nextBoxTapObj.text = LocalizationManager.instance.GetString("EndBox");
        }
        else
        {
            nextBoxTapObj.text = LocalizationManager.instance.GetString("NextBox");
        }

        isDelay = true;
        Invoke("Delay", 0.1f);
    }

    public static string[] SplitStringAtLastDelimiter(string input, char delimiter)
    {
        int lastIndex = input.LastIndexOf(delimiter);
        if (lastIndex == -1 || lastIndex == input.Length - 1)
        {
            return new string[] { input };
        }

        string firstPart = input.Substring(0, lastIndex);
        string secondPart = input.Substring(lastIndex + 1);

        return new string[] { firstPart, secondPart };
    }

    IEnumerator OpenBoxCoroution() //마지막에 전체 보여주기
    {
        boxCountText.text = "0";

        boxPanel.SetActive(true);

        for (int i = 0; i < blockUIContentList.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(false);
        }

        if (totalgold > 0)
        {
            PlayfabManager.instance.UpdateAddGold(totalgold);
        }

        if (totalUpgradeTicket > 0)
        {
            ItemAnimManager.instance.GetUpgradeTicket(totalUpgradeTicket);
        }

        yield return waitForSeconds2;

        for (int i = 0; i < prize_Block.Count; i++)
        {
            blockUIContentList[i].gameObject.SetActive(true);
            blockUIContentList[i].Initialize_RandomBox(prize_Block[i]);

            SoundManager.instance.PlaySFX(GameSfxType.GetBlock);

            yield return waitForSeconds;
        }

        yield return waitForSeconds2;

        tapObj.SetActive(true);

        closePanel.SetActive(true);
    }


    void Delay()
    {
        isDelay = false;
    }

    int RoundToNearestTens(int num)
    {
        // 1의 자리에서 반올림
        remainder = num % 10;
        if (remainder >= 5)
        {
            num += (10 - remainder);
        }
        else
        {
            num -= remainder;
        }
        return num;
    }
}
