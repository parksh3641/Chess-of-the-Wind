using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class BlockContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockClass blockClass;

    [Title("Info")]
    public Text valueText;

    [Title("Block")]
    Image backgroundImg;
    public Image blockIcon;
    public GameObject blockMain;

    public BlockChildContent[] blockUIArray;
    public BlockChildContent[] blockMainArray;

    [Title("Drag")]
    private Transform blockRootParent;
    private Transform previousParent;
    private CanvasGroup canvasGroup;
    public bool isDrag = false;

    public int index = 0;
    public int value = 0;

    GameManager gameManager;

    void Awake()
    {
        backgroundImg = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        blockMain.SetActive(true);
    }

    public void Initialize(GameManager manager, Transform root, Transform grid)
    {
        gameManager = manager;
        blockRootParent = root;
        previousParent = grid;
    }

    public void InGame_Initialize(BlockClass block, int number, int value)
    {
        blockClass = block;
        index = number;
        this.value = value;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blockMainArray.Length; i++)
        {
            blockMainArray[i].gameObject.SetActive(false);
        }

        blockUIArray[(int)blockClass.blockType - 1].gameObject.SetActive(true);

        switch (blockClass.rankType)
        {
            case RankType.N:
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                break;
            case RankType.R:
                backgroundImg.color = Color.green;
                break;
            case RankType.SR:
                backgroundImg.color = new Color(0, 150 / 255f, 1);
                break;
            case RankType.SSR:
                backgroundImg.color = new Color(1, 100 / 255f, 1);
                break;
            case RankType.UR:
                backgroundImg.color = Color.yellow;
                break;
            default:
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                break;
        }

        valueText.text = MoneyUnitString.ToCurrencyString(this.value);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(blockRootParent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;

        gameManager.blockDrag = true;
        gameManager.blockType = blockClass.blockType;

        blockMain.SetActive(false);
        blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(true);
        blockIcon = blockMainArray[(int)blockClass.blockType - 1].blockIcon;

        blockIcon.color = new Color(1, 1, 1, 0.5f);

        isDrag = true;

        for (int i = 0; i < blockMainArray[(int)blockClass.blockType - 1].blockChildArray.Length; i++)
        {
            blockMainArray[(int)blockClass.blockType - 1].blockChildArray[i].SetBettingMark(false);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag) transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == blockRootParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            blockMain.SetActive(true);
            blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

            gameManager.CancleBetting(blockClass.blockType);

            gameManager.ResetPosBlock(index);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;
        gameManager.blockType = BlockType.Default;

        blockIcon.color = new Color(1, 1, 1, 1f);

        isDrag = false;

        blockMainArray[(int)blockClass.blockType - 1].Betting(true);
        //blockMainArray[(int)blockClass.blockType - 1].SetBlock(GameStateManager.instance.NickName, value.ToString());
    }
    public void ResetPos()
    {
        if (transform.parent != previousParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            blockMain.SetActive(true);
            blockIcon.color = new Color(1, 1, 1, 1f);
            blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

            isDrag = false;

            gameManager.ResetPosBlock(index);
        }

        blockMainArray[(int)blockClass.blockType - 1].Betting(false);
    }

    public void TimeOver()
    {
        transform.SetParent(previousParent);
        transform.position = previousParent.GetComponent<RectTransform>().position;

        blockMain.SetActive(true);
        blockIcon.color = new Color(1, 1, 1, 1f);
        blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;

        isDrag = false;

        gameManager.ResetPosBlock(index);
    }
}
