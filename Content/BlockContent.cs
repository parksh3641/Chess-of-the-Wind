using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class BlockContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockClass blockClass;
    public int index = 0;

    Image backgroundImg;
    [Title("Level")]
    public Text levelText;
    public int level = 0;

    [Title("Block")]
    public GameObject blockMain;

    public BlockChildContent[] blockUIArray;
    public BlockChildContent[] blockMainArray;

    [Title("Drag")]
    private Transform blockRootParent;
    private Transform previousParent;
    private CanvasGroup canvasGroup;
    public bool isDrag = false;

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

    public void Collection_Initialize(BlockClass block, int number)
    {
        blockClass = block;
        index = number;

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
                backgroundImg.color = Color.blue;
                break;
            case RankType.SSR:
                backgroundImg.color = new Color(1, 0, 1);
                break;
            case RankType.UR:
                backgroundImg.color = Color.yellow;
                break;
            default:
                backgroundImg.color = new Color(200 / 255f, 200 / 255f, 200 / 255f);
                break;
        }

        level = blockClass.level;

        if (level > 0)
        {
            levelText.text = (level + 1).ToString();
        }
        else
        {
            levelText.text = "";
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //previousParent = transform.parent;

        transform.SetParent(blockRootParent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;

        gameManager.blockDrag = true;
        gameManager.blockType = blockClass.blockType;

        //backgroundImg.color = new Color(1, 1, 1, 1 / 255f);
        blockMain.SetActive(false);
        blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(true);

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

            //backgroundImg.color = new Color(1, 1, 1, 1);
            blockMain.SetActive(true);
            blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

            gameManager.CancleBetting(blockClass.blockType);

            gameManager.ResetPosBlock(index);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;
        gameManager.blockType = BlockType.Default;

        isDrag = false;

        for (int i = 0; i < blockMainArray[(int)blockClass.blockType - 1].blockChildArray.Length; i++)
        {
            blockMainArray[(int)blockClass.blockType - 1].blockChildArray[i].SetBettingMark(true);
        }
    }
    public void ResetPos()
    {
        if (transform.parent != previousParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            //backgroundImg.color = new Color(1, 1, 1, 1);
            blockMain.SetActive(true);
            blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

            isDrag = false;

            gameManager.ResetPosBlock(index);
        }

        for (int i = 0; i < blockMainArray[(int)blockClass.blockType - 1].blockChildArray.Length; i++)
        {
            blockMainArray[(int)blockClass.blockType - 1].blockChildArray[i].SetBettingMark(false);
        }
    }

    public void TimeOver()
    {
        transform.SetParent(previousParent);
        transform.position = previousParent.GetComponent<RectTransform>().position;

        backgroundImg.color = new Color(1, 1, 1, 1);
        blockMain.SetActive(true);
        blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;

        isDrag = false;
    }
}
