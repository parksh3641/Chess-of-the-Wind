using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockChild : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;

    public Text nickNameText;
    public GameObject bettingMark;

    void Awake()
    {
        switch (blockType)
        {
            case BlockType.Default:
                transform.GetComponent<Image>().color = Color.white;
                break;
            case BlockType.I:
                transform.GetComponent<Image>().color = new Color(0, 200 / 255f, 1);
                break;
            case BlockType.O:
                transform.GetComponent<Image>().color = Color.yellow;
                break;
            case BlockType.T:
                transform.GetComponent<Image>().color = new Color(200 / 255f, 0, 1);
                break;
            case BlockType.L:
                transform.GetComponent<Image>().color = new Color(1, 150 / 255f, 0);
                break;
            case BlockType.J:
                transform.GetComponent<Image>().color = Color.blue;
                break;
            case BlockType.S:
                transform.GetComponent<Image>().color = Color.green;
                break;
            case BlockType.Z:
                transform.GetComponent<Image>().color = Color.red;
                break;
            case BlockType.BigO:
                transform.GetComponent<Image>().color = Color.cyan;
                break;
            case BlockType.I_Horizontal:
                transform.GetComponent<Image>().color = new Color(0, 200 / 255f, 1);
                break;
            case BlockType.One:
                transform.GetComponent<Image>().color = Color.gray;
                break;
        }

        bettingMark.SetActive(false);
    }

    public void SetBettingMark(bool check)
    {
        bettingMark.SetActive(check);
    }

    public void SetNickName(string name)
    {
        nickNameText.text = name;
    }
}
