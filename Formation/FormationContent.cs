using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationContent : MonoBehaviour
{
    public WindCharacterType formationType = WindCharacterType.Winter;

    public Text titleText;
    public Text selectText;

    public BlockUIContent[] blockContents;

    void Start()
    {
        if(formationType == WindCharacterType.Winter)
        {
            blockContents[0].Initialize(BlockType.LeftQueen_2);
            blockContents[1].Initialize(BlockType.LeftNight);
            blockContents[2].Initialize(BlockType.Rook_V2);
            blockContents[3].Initialize(BlockType.Pawn_Snow);

            titleText.text = "눈의 세계";
        }
        else
        {
            blockContents[0].Initialize(BlockType.RightQueen_2);
            blockContents[1].Initialize(BlockType.RightNight);
            blockContents[2].Initialize(BlockType.Rook_V4);
            blockContents[3].Initialize(BlockType.Pawn_Under);

            titleText.text = "지하 세계";
        }

        selectText.text = "선택하기";
    }
}
