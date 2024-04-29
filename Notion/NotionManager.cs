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

    private Color whiteColor = new Color(1, 1, 1);
    private Color redColor = new Color(1, 0, 0);
    private Color orangeColor = new Color(1, 150f / 255f, 0);
    private Color yellowColor = new Color(1, 1, 0);
    private Color greenColor = new Color(0, 1, 0);
    private Color skyblueColor = new Color(0, 1, 1);
    private Color blueColor = new Color(1, 150f / 255f, 1);
    private Color purpleColor = new Color(1, 50f / 255f, 1);
    private Color pinkColor = new Color(1, 150 / 255f, 1);
    private Color blackColor = new Color(0, 0, 0);


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

    void SetColor(Text txt, ColorType type)
    {
        switch (type)
        {
            case ColorType.White:
                txt.color = whiteColor;
                break;
            case ColorType.Red:
                txt.color = redColor;
                break;
            case ColorType.Orange:
                txt.color = orangeColor;
                break;
            case ColorType.Yellow:
                txt.color = yellowColor;
                break;
            case ColorType.Green:
                txt.color = greenColor;
                break;
            case ColorType.SkyBlue:
                txt.color = skyblueColor;
                break;
            case ColorType.Blue:
                txt.color = blueColor;
                break;
            case ColorType.Purple:
                txt.color = purpleColor;
                break;
            case ColorType.Pink:
                txt.color = pinkColor;
                break;
            case ColorType.Black:
                txt.color = blackColor;
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