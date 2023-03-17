using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherBlockContent : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;

    Image main;

    public string nickName;
    public string value;

    public BlockChildContent[] blockArray;

    private void Awake()
    {
        main = GetComponent<Image>();
    }

    public void SetOtherBlock(BlockType type, string name, string value)
    {
        nickName = name;
        this.value = value;
        blockType = type;

        for (int i = 0; i < blockArray.Length; i++)
        {
            blockArray[i].gameObject.SetActive(false);
        }

        blockArray[(int)blockType - 1].gameObject.SetActive(true);
        blockArray[(int)blockType - 1].SetBlock(nickName, value);

        main.raycastTarget = false;
    }
}
