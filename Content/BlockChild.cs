using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockChild : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;

    private Image image;

    public GameObject bettingMark;

    void Awake()
    {
        image = GetComponent<Image>();

        switch (blockType)
        {
            case BlockType.Default:
                image.color = Color.white;
                break;
            case BlockType.RightQueen_2:
                image.color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.LeftQueen_2:
                image.color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.RightQueen_3:
                image.color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.LeftQueen_3:
                image.color = new Color(1, 200 / 255f, 0);
                break;
            case BlockType.RightNight:
                image.color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.LeftNight:
                image.color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.RightDownNight:
                image.color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.LeftDownNight:
                image.color = new Color(200 / 255f, 100 / 255f, 0);
                break;
            case BlockType.Rook_V2:
                image.color = new Color(0, 150 / 255f, 1);
                break;
            case BlockType.Rook_V2H2:
                image.color = new Color(0, 150 / 255f, 1);
                break;
            case BlockType.Pawn_Under:
                image.color = new Color(0, 150 / 255f, 1);
                break;
            case BlockType.Pawn_Snow:
                image.color = new Color(0, 150 / 255f, 1);
                break;
            case BlockType.Spider:
                image.color = Color.red;
                break;
            case BlockType.Rook_V4:
                image.color = new Color(240 / 255f, 240 / 255f, 1);
                break;
            case BlockType.Tetris_I_Hor:
                image.color = new Color(240 / 255f, 240 / 255f, 1);
                break;
            case BlockType.Tetris_T:
                image.color = new Color(160 / 255f, 0, 240 / 255f);
                break;
            case BlockType.Tetris_L:
                image.color = new Color(0, 0, 240 / 255f);
                break;
            case BlockType.Tetris_J:
                image.color = new Color(240 / 255f, 160 / 255f, 0);
                break;
            case BlockType.Tetris_S:
                image.color = new Color(0, 240 / 255f, 0);
                break;
            case BlockType.Tetris_Z:
                image.color = new Color(240 / 255f, 0, 0);
                break;
            case BlockType.Tetris_Speical:
                image.color = new Color(0, 240 / 255f, 240 / 255f);
                break;
        }

        bettingMark.SetActive(false);
    }

    public void SetBettingMark(bool check)
    {
        bettingMark.SetActive(check);
    }
}
