using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockChild : MonoBehaviour
{
    public BlockType blockType = BlockType.Default;

    private Color snowWorldColor = new Color(0 / 255f, 200 / 255f, 255 / 255f);
    private Color underWorldColor = new Color(205 / 255f, 92 / 255f, 92 / 255f);

    private Image image;

    public GameObject bettingMark;

    void Awake()
    {
        image = GetComponent<Image>();

        switch (blockType)
        {
            case BlockType.RightQueen_2:
                image.color = underWorldColor;
                break;
            case BlockType.LeftQueen_2:
                image.color = snowWorldColor;
                break;
            case BlockType.RightQueen_3:
                image.color = underWorldColor;
                break;
            case BlockType.LeftQueen_3:
                image.color = snowWorldColor;
                break;
            case BlockType.RightNight:
                image.color = underWorldColor;
                break;
            case BlockType.LeftNight:
                image.color = snowWorldColor;
                break;
            case BlockType.RightDownNight:
                image.color = underWorldColor;
                break;
            case BlockType.LeftDownNight:
                image.color = snowWorldColor;
                break;
            case BlockType.Rook_V2:
                image.color = snowWorldColor;
                break;
            case BlockType.Rook_V2H2:
                image.color = Color.white; //╬х╬╡юс
                break;
            case BlockType.Pawn_Under:
                image.color = underWorldColor;
                break;
            case BlockType.Pawn_Snow:
                image.color = snowWorldColor;
                break;
            case BlockType.Spider:
                image.color = Color.white;
                break;
            case BlockType.Rook_V4:
                image.color = underWorldColor;
                break;
            case BlockType.Tetris_I_Hor:
                image.color = Color.white;
                break;
            case BlockType.Tetris_T:
                image.color = Color.white;
                break;
            case BlockType.Tetris_L:
                image.color = Color.white;
                break;
            case BlockType.Tetris_J:
                image.color = Color.white;
                break;
            case BlockType.Tetris_S:
                image.color = Color.white;
                break;
            case BlockType.Tetris_Z:
                image.color = Color.white;
                break;
            case BlockType.Tetris_Speical:
                image.color = Color.white;
                break;
        }

        bettingMark.SetActive(false);
    }

    public void SetBettingMark(bool check)
    {
        bettingMark.SetActive(check);
    }
}
