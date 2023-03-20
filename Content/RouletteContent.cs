using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using System.Linq;

[System.Serializable]
public class NumberClass
{
    public int[] numberList = new int[4];

    public int[] GetNumberList()
    {
        return numberList;
    }
}

public class RouletteContent : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public RouletteType rouletteType = RouletteType.Default;
    public RouletteColorType rouletteColorType = RouletteColorType.White;
    public int[] index = new int[2];

    [Space]
    [Title("Number")]
    public int number = 0;
    public int[] numberList = new int[4];

    [Space]
    [Title("Active")]
    public BlockClass blockClass;
    public bool isActive = false;
    bool initialize = false;

    [Space]
    [Title("Main")]
    public Text numberText;
    public Image backgroundImg;
    public GameObject queen;

    private Transform blockParent;
    GameManager gameManager;

    public List<NumberClass> splitBet_HorizontalNumberList = new List<NumberClass>();
    public List<NumberClass> splitBet_VerticalNumberList = new List<NumberClass>();
    public List<NumberClass> squareBetNumberList = new List<NumberClass>();


    void Awake()
    {
        numberText.text = "";

        //blockType = new BlockType[System.Enum.GetValues(typeof(BlockType)).Length];

        queen.SetActive(false);
    }


    public void Initialize(GameManager manager, Transform parent, RouletteType type, int[] setIndex, int num)
    {
        gameManager = manager;
        blockParent = parent;
        rouletteType = type;
        index = setIndex;

        number = num + 1;

        switch (rouletteType)
        {
            case RouletteType.StraightBet:

                if (number % 2 == 0)
                {
                    rouletteColorType = RouletteColorType.White;
                }
                else
                {
                    rouletteColorType = RouletteColorType.Black;
                }

                if (type == RouletteType.StraightBet && number == 13)
                {
                    queen.SetActive(true);
                    rouletteColorType = RouletteColorType.White;
                }

                SetBackgroundColor(rouletteColorType);

                break;
            case RouletteType.SplitBet_Horizontal:

                numberList = splitBet_HorizontalNumberList[num].GetNumberList();

                backgroundImg.enabled = false;
                break;
            case RouletteType.SplitBet_Vertical:

                numberList = splitBet_VerticalNumberList[num].GetNumberList();

                backgroundImg.enabled = false;
                break;
            case RouletteType.SquareBet:

                numberList = squareBetNumberList[num].GetNumberList();

                backgroundImg.enabled = false;
                break;
        }

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

    public void SetActiveTrue(BlockClass block)
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

        blockClass = block;
        isActive = true;

        SetBackgroundColor(rouletteColorType);

        initialize = true;
    }

    public void SetActiveFalse(BlockClass block)
    {
        if (blockClass == null) return;

        if(blockClass.Equals(block))
        {
            blockClass = null;

            isActive = false;
        }
    }

    public void SetActiveFalseAll()
    {
        blockClass = null;

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
            eventData.pointerDrag.transform.SetParent(blockParent);
            eventData.pointerDrag.GetComponent<RectTransform>().position = transform.position;

            gameManager.ExitBlock(eventData.pointerDrag.GetComponent<BlockContent>());
        }
    }
}
