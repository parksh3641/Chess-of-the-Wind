using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using System.Linq;

public class RouletteContent : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public RouletteType rouletteType = RouletteType.Default;
    public RouletteColorType rouletteColorType = RouletteColorType.White;
    public int[] index = new int[2];
    public int number = 0;

    [Title("Active")]
    public BlockType[] blockType;
    public bool isActive = false;
    bool initialize = false;

    [Title("Main")]
    public Text numberText;
    public Image backgroundImg;

    private Transform blockParent;
    GameManager gameManager;

    void Awake()
    {
        numberText.text = "";

        blockType = new BlockType[System.Enum.GetValues(typeof(BlockType)).Length];
    }


    public void Initialize(GameManager manager, Transform parent, RouletteType type, int[] setIndex, int num)
    {
        gameManager = manager;
        blockParent = parent;
        rouletteType = type;
        index = setIndex;

        switch (rouletteType)
        {
            case RouletteType.SplitBet_Horizontal:
                backgroundImg.enabled = false;
                break;
            case RouletteType.SplitBet_Vertical:
                backgroundImg.enabled = false;
                break;
            case RouletteType.SquareBet:
                backgroundImg.enabled = false;
                break;
        }

        number = num + 1;
        //numberText.text = number.ToString();

        if (number % 2 == 0)
        {
            rouletteColorType = RouletteColorType.White;
        }
        else
        {
            rouletteColorType = RouletteColorType.Black;
        }

        SetBackgroundColor(rouletteColorType);

        initialize = true;
    }

    public void SetBackgroundColor(RouletteColorType type)
    {
        if(initialize)
        {
            switch (rouletteType)
            {
                case RouletteType.SplitBet_Horizontal:
                    backgroundImg.enabled = true;
                    break;
                case RouletteType.SplitBet_Vertical:
                    backgroundImg.enabled = true;
                    break;
                case RouletteType.SquareBet:
                    backgroundImg.enabled = true;
                    break;
            }
        }

        switch (type)
        {
            case RouletteColorType.White:
                backgroundImg.color = Color.white;
                break;
            case RouletteColorType.Black:
                backgroundImg.color = Color.black;
                break;
            case RouletteColorType.Yellow:
                backgroundImg.color = Color.yellow;
                break;
        }
    }

    public void ResetBackgroundColor()
    {
        if (initialize)
        {
            switch (rouletteType)
            {
                case RouletteType.SplitBet_Horizontal:
                    backgroundImg.enabled = false;
                    break;
                case RouletteType.SplitBet_Vertical:
                    backgroundImg.enabled = false;
                    break;
                case RouletteType.SquareBet:
                    backgroundImg.enabled = false;
                    break;
            }

            initialize = false;
        }

        SetBackgroundColor(rouletteColorType);

        initialize = true;
    }

    public void SetActiveTrue(BlockType type)
    {
        if (initialize)
        {
            switch (rouletteType)
            {
                case RouletteType.SplitBet_Horizontal:
                    backgroundImg.enabled = false;
                    break;
                case RouletteType.SplitBet_Vertical:
                    backgroundImg.enabled = false;
                    break;
                case RouletteType.SquareBet:
                    backgroundImg.enabled = false;
                    break;
            }

            initialize = false;
        }

        blockType[(int)type] = type;
        isActive = true;

        SetBackgroundColor(rouletteColorType);

        initialize = true;
    }

    public void SetActiveFalse(BlockType type)
    {
        blockType[(int)type] = BlockType.Default;
    }

    public void SetActiveFalseAll()
    {
        blockType = new BlockType[System.Enum.GetValues(typeof(BlockType)).Length];

        isActive = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameManager.blockDrag)
        {
            gameManager.EnterBlock(this, eventData.pointerDrag.transform.GetComponent<BlockContent>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.transform.tag != "Block") return;

        if (eventData.pointerDrag != null && !gameManager.blockOverlap && gameManager.blockDrop)
        {
            if (gameManager.money >= gameManager.blockInformation.bettingPrice * gameManager.blockInformation.size)
            {
                eventData.pointerDrag.transform.SetParent(blockParent);
                eventData.pointerDrag.GetComponent<RectTransform>().position = transform.position;

                gameManager.ExitBlock(eventData.pointerDrag.GetComponent<BlockContent>());
            }
            else
            {
                NotionManager.instance.UseNotion(NotionType.NotEnoughMoney);
            }
        }
    }
}
