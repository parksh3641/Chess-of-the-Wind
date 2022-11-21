﻿using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotionManager : MonoBehaviour
{
    public static NotionManager instance;

    [Title("MainNotion")]
    public Notion notion;


    public NotionColor[] notionColor;


    [System.Serializable]
    public class NotionColor
    {
        [Space]
        public NotionType notionType;
        public ColorType colorType;
        [Space]
        public EffectType effectType;
    }
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        notion.gameObject.SetActive(false);
    }

    void SetColor(ColorType type)
    {
        switch (type)
        {
            case ColorType.White:
                notion.txt.color = new Color(1, 1, 1);
                break;
            case ColorType.Red:
                notion.txt.color = new Color(1, 0, 0);
                break;
            case ColorType.Orange:
                notion.txt.color = new Color(1, 255f / 150f, 0);
                break;
            case ColorType.Yellow:
                notion.txt.color = new Color(1, 1, 0);
                break;
            case ColorType.Green:
                notion.txt.color = new Color(0, 1, 0);
                break;
            case ColorType.SkyBlue:
                notion.txt.color = new Color(0, 1, 1);
                break;
            case ColorType.Blue:
                notion.txt.color = new Color(0, 0, 1);
                break;
            case ColorType.Purple:
                notion.txt.color = new Color(255f / 200f, 0, 1);
                break;
            case ColorType.Pink:
                notion.txt.color = new Color(1, 255f / 150f, 1);
                break;
            case ColorType.Black:
                notion.txt.color = new Color(0, 0, 0);
                break;
        }
    }
    public void UseNotion(NotionType type)
    {
        notion.gameObject.SetActive(false);

        foreach (var list in notionColor)
        {
            if (list.notionType.Equals(type))
            {
                switch (type)
                {
                    case NotionType.Test:
                        notion.txt.text = "테스트";
                        break;
                    case NotionType.MaxBetting:
                        notion.txt.text = "더 이상 베팅할 수 없습니다";
                        break;
                    case NotionType.NotEnoughMoney:
                        notion.txt.text = "베팅 할 금액이 부족합니다";
                        break;
                    case NotionType.Double:
                        notion.txt.text = "현재 베팅된 금액을 2배로 높입니다";
                        break;
                    case NotionType.Cancle:
                        notion.txt.text = "베팅을 취소했습니다";
                        break;
                    case NotionType.Repeat:
                        notion.txt.text = "전에 했던 베팅 위치와 금액을 가져옵니다";
                        break;
                }
                SetColor(list.colorType);
                SetEffect(list.effectType);
            }
        }

        notion.gameObject.SetActive(true);
    }

    public void UseNotion(string str, ColorType color)
    {
        notion.gameObject.SetActive(false);
        notion.txt.text = str;
        SetColor(color);
        notion.gameObject.SetActive(true);
    }

    public void SetEffect(EffectType type)
    {
        switch (type)
        {
            case EffectType.Default:

                break;
            case EffectType.Vibration:
                notion.txt.gameObject.transform.DOPunchPosition(Vector3.left * 50, 0.5f);
                break;
        }
    }

    [Button]
    public void TestNotion()
    {
        UseNotion(NotionType.Test);
    }

}

public enum ColorType
{
    White = 0,
    Red,
    Orange,
    Yellow,
    Green,
    SkyBlue,
    Blue,
    Purple,
    Pink,
    Black
}

public enum EffectType
{
    Default = 0,
    Vibration
}

public enum NotionType
{
    Test,
    MaxBetting,
    NotEnoughMoney,
    Double,
    Cancle,
    Repeat,
    
}