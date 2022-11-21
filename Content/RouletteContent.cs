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
    public RouletteColorType rouletteColorType = RouletteColorType.Red;
    public int[] index = new int[2];

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


    public void Initialize(GameManager manager, Transform parent, RouletteType type, int[] setIndex, int number)
    {
        gameManager = manager;
        blockParent = parent;
        rouletteType = type;
        index = setIndex;

        numberText.text = (number + 1).ToString();

        if (number % 2 == 0)
        {
            rouletteColorType = RouletteColorType.Red;
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
            case RouletteColorType.Red:
                backgroundImg.color = Color.red;
                break;
            case RouletteColorType.Black:
                backgroundImg.color = Color.black;
                break;
            case RouletteColorType.Yellow:
                backgroundImg.color = Color.yellow;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameManager.blockDrag)
        {
            SetBackgroundColor(RouletteColorType.Yellow);

            gameManager.EnterBlock(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetBackgroundColor(rouletteColorType);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(blockParent);
            eventData.pointerDrag.GetComponent<RectTransform>().position = transform.position;

            gameManager.ExitBlock(this);
        }
    }
}
