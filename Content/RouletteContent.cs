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
    public BlockType blockType = BlockType.Default;
    public bool isActive = false;

    [Title("Main")]
    public Text numberText;
    public Image backgroundImg;

    private Transform blockParent;
    GameManager gameManager;

    void Awake()
    {

    }


    public void Initialize(GameManager manager, Transform parent, RouletteType type, int[] setIndex, int num)
    {
        gameManager = manager;
        blockParent = parent;
        rouletteType = type;
        index = setIndex;

        number = num + 1;
        numberText.text = number.ToString();

        if (number % 2 == 0)
        {
            rouletteColorType = RouletteColorType.White;
        }
        else
        {
            rouletteColorType = RouletteColorType.Black;
        }

        SetBackgroundColor(rouletteColorType);
    }

    public void SetBackgroundColor(RouletteColorType type)
    {
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
        SetBackgroundColor(rouletteColorType);
    }

    public void SetActiveTrue(BlockType type)
    {
        blockType = type;
        isActive = true;

        SetBackgroundColor(rouletteColorType);
    }

    public void SetActiveFalse()
    {
        blockType = BlockType.Default;

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
        if (eventData.pointerDrag != null && !gameManager.blockOverlap && gameManager.blockDrop)
        {
            if(gameManager.money >= 4000)
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

        if(gameManager.blockOverlap)
        {
            NotionManager.instance.UseNotion(NotionType.NotBettingLocation);
        }
    }
}
