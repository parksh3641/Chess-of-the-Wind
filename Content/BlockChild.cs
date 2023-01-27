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
            case BlockType.RightQueen_2:
                transform.GetComponent<Image>().color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.LeftQueen_2:
                transform.GetComponent<Image>().color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.RightQueen_3:
                transform.GetComponent<Image>().color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.LeftQueen_3:
                transform.GetComponent<Image>().color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.RightNight:
                transform.GetComponent<Image>().color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.LeftNight:
                transform.GetComponent<Image>().color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.RightDownNight:
                transform.GetComponent<Image>().color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.LeftDownNight:
                transform.GetComponent<Image>().color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.Rook_V2:
                transform.GetComponent<Image>().color = new Color(0, 150 / 255f, 1);
                break;
            case BlockType.Rook_V2H2:
                transform.GetComponent<Image>().color = new Color(0, 150 / 255f, 1);
                break;
            case BlockType.Pawn:
                transform.GetComponent<Image>().color = Color.green;
                break;
            case BlockType.Spider:
                transform.GetComponent<Image>().color = Color.red;
                break;
            case BlockType.Tetris_I:
                transform.GetComponent<Image>().color = new Color(240 / 255f, 240 / 255f, 1);
                break;
            case BlockType.Tetris_I_Hor:
                transform.GetComponent<Image>().color = new Color(240 / 255f, 240 / 255f, 1);
                break;
            case BlockType.Tetris_T:
                transform.GetComponent<Image>().color = new Color(160 / 255f, 0, 240 / 255f);
                break;
            case BlockType.Tetris_L:
                transform.GetComponent<Image>().color = new Color(0, 0, 240 / 255f);
                break;
            case BlockType.Tetris_J:
                transform.GetComponent<Image>().color = new Color(240 / 255f, 160 / 255f, 0);
                break;
            case BlockType.Tetris_S:
                transform.GetComponent<Image>().color = new Color(0, 240 / 255f, 0);
                break;
            case BlockType.Tetris_Z:
                transform.GetComponent<Image>().color = new Color(240 / 255f, 0, 0);
                break;
            case BlockType.Tetris_Speical:
                transform.GetComponent<Image>().color = new Color(0, 240 / 255f, 240 / 255f);
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

        Debug.Log(name);
    }
}
