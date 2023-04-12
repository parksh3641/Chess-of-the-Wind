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
                    case NotionType.NotEnoughMoney:
                        notion.txt.text = "골드가 부족합니다";
                        break;
                    case NotionType.NotBettingLocation:
                        notion.txt.text = "그 위치에는 놓을 수 없어요";
                        break;
                    case NotionType.Cancle:
                        notion.txt.text = "배치한 블럭을 취소했어요";
                        break;
                    case NotionType.GoBetting:
                        notion.txt.text = "시간 안에 블럭을 배치하세요";
                        break;
                    case NotionType.YourTurn:
                        notion.txt.text = "당신의 차례예요. 바람을 불어보세요!";
                        break;
                    case NotionType.BuyTicket:
                        notion.txt.text = "강화권을 구매했습니다";
                        break;
                    case NotionType.UpgradeSuccess:
                        notion.txt.text = "강화 성공!";
                        break;
                    case NotionType.UpgradeKeep:
                        notion.txt.text = "강화 유지";
                        break;
                    case NotionType.UpgradeDown:
                        notion.txt.text = "강화 하락";
                        break;
                    case NotionType.UpgradeDestroy:
                        notion.txt.text = "블럭이 파괴되었습니다";
                        break;
                    case NotionType.SellBlock:
                        notion.txt.text = "블럭을 판매했습니다";
                        break;
                    case NotionType.MaxBlockLevel:
                        notion.txt.text = "최대 강화레벨입니다";
                        break;
                    case NotionType.DontSellEquipBlock:
                        notion.txt.text = "장착한 블럭은 판매할 수 없습니다";
                        break;
                    case NotionType.NotEnoughTicket:
                        notion.txt.text = "강화권이 부족합니다";
                        break;
                    case NotionType.DefDestroy:
                        notion.txt.text = "파괴 방지!";
                        break;
                    case NotionType.SameEquipBlock:
                        notion.txt.text = "같은 종류의 블록을 중복 장착할 수 없습니다";
                        break;
                    case NotionType.LimitMaxBlock:
                        notion.txt.text = "입장 가능한 최대 블럭 값을 초과했습니다";
                        break;
                    case NotionType.OverBettingBlock:
                        notion.txt.text = "1개 이상 배팅할 수 없습니다";
                        break;
                    case NotionType.OnlyPawn:
                        notion.txt.text = "초보방은 폰만 장착할 수 있습니다";
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