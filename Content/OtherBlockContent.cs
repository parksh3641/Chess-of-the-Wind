using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherBlockContent : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;

    Image main;

    public string nickName;

    public GameObject blockMain;
    public BlockChildContent[] blockMainArray;
    public BlockChildContent[] blockArray;

    private void Awake()
    {
        main = GetComponent<Image>();
    }

    public void ShowInitialize(BlockType type, string name)
    {
        nickName = name;
        blockType = type;

        blockMain.SetActive(false);
        blockArray[(int)blockType - 1].gameObject.SetActive(true);

        for (int i = 0; i < blockArray[(int)blockType - 1].blockChildArray.Length; i++)
        {
            blockArray[(int)blockType - 1].blockChildArray[i].SetNickName(nickName);
        }

        main.raycastTarget = false;
    }
}
