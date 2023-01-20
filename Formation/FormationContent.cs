using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationContent : MonoBehaviour
{
    public FormationType formationType = FormationType.Winter;

    public Text titleText;
    public Text selectText;

    public OtherBlockContent[] blockContents;

    void Start()
    {
        if(formationType == FormationType.Winter)
        {
            blockContents[0].ShowInitialize(BlockType.LeftQueen_2,"");
            blockContents[1].ShowInitialize(BlockType.LeftNight,"");
            blockContents[2].ShowInitialize(BlockType.Rook_V2, "");
            blockContents[3].ShowInitialize(BlockType.Pawn, "");

            titleText.text = "���� ����";
            selectText.text = "���� ����\n�����ϱ�";
        }
        else
        {
            blockContents[0].ShowInitialize(BlockType.RightQueen_2, "");
            blockContents[1].ShowInitialize(BlockType.RightNight, "");
            blockContents[2].ShowInitialize(BlockType.Rook_V2H2, "");
            blockContents[3].ShowInitialize(BlockType.Pawn, "");

            titleText.text = "���� ����";
            selectText.text = "���� ����\n�����ϱ�";
        }
    }
}
