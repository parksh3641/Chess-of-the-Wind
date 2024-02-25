using DG.Tweening;
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
    public Notion notion2;


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

        if(notion2 != null)
        {
            notion2.gameObject.SetActive(false);
        }
    }

    void SetColor(Text text, ColorType type)
    {
        switch (type)
        {
            case ColorType.White:
                text.color = new Color(1, 1, 1);
                break;
            case ColorType.Red:
                text.color = new Color(1, 0, 0);
                break;
            case ColorType.Orange:
                text.color = new Color(1, 255f / 150f, 0);
                break;
            case ColorType.Yellow:
                text.color = new Color(1, 1, 0);
                break;
            case ColorType.Green:
                text.color = new Color(0, 1, 0);
                break;
            case ColorType.SkyBlue:
                text.color = new Color(0, 1, 1);
                break;
            case ColorType.Blue:
                text.color = new Color(0, 0, 1);
                break;
            case ColorType.Purple:
                text.color = new Color(255f / 200f, 0, 1);
                break;
            case ColorType.Pink:
                text.color = new Color(1, 255f / 150f, 1);
                break;
            case ColorType.Black:
                text.color = new Color(0, 0, 0);
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
                notion.txt.text = LocalizationManager.instance.GetString(list.notionType.ToString());
                SetColor(notion.txt, list.colorType);
                SetEffect(notion.txt, list.effectType);
            }
        }

        notion.gameObject.SetActive(true);
    }

    public void UseNotion(Color color, string txt)
    {
        notion.gameObject.SetActive(false);

        notion.txt.text = txt;
        notion.txt.color = color;

        notion.gameObject.SetActive(true);
    }

    public void UseNotion2(NotionType type)
    {
        notion2.gameObject.SetActive(false);

        foreach (var list in notionColor)
        {
            if (list.notionType.Equals(type))
            {
                notion2.txt.text = LocalizationManager.instance.GetString(list.notionType.ToString());
                SetColor(notion2.txt, list.colorType);
                SetEffect(notion2.txt, list.effectType);
            }
        }

        notion2.gameObject.SetActive(true);
    }

    public void UseNotion(string str, ColorType color)
    {
        notion.gameObject.SetActive(false);
        notion.txt.text = str;
        SetColor(notion.txt, color);
        notion.gameObject.SetActive(true);
    }

    public void SetEffect(Text text, EffectType type)
    {
        switch (type)
        {
            case EffectType.Default:

                break;
            case EffectType.Vibration:
                text.gameObject.transform.DOPunchPosition(Vector3.left * 50, 0.5f);
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