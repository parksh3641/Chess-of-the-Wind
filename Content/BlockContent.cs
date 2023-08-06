using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class BlockContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockClass blockClass;

    [Title("Block")]
    public Image backgroundImg;
    public Image blockUI;

    public Image rankImg;
    public Text rankText;
    public Text levelText;

    public GameObject gradient;

    Sprite[] rankBackgroundArray;
    Sprite[] rankBannerArray;

    public Image blockIcon;
    public GameObject blockMain;

    public GameObject[] blockUIArray;
    public BlockChildContent[] blockMainArray;

    [Title("Info")]
    public Text valueText;

    [Title("Drag")]
    private Transform blockRootParent;
    private Transform previousParent;
    private CanvasGroup canvasGroup;
    public bool isDrag = false;

    public int index = 0;
    public int value = 0;

    GameManager gameManager;

    ImageDataBase imageDataBase;

    void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();
        rankBannerArray = imageDataBase.GetRankBannerArray();

        canvasGroup = GetComponent<CanvasGroup>();

        gradient.SetActive(false);

        blockMain.SetActive(true);
        blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1);
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

        if (blockClass.rankType > RankType.SR)
        {
            gradient.SetActive(true);
        }
        else
        {
            gradient.SetActive(false);
        }

        blockUIArray[(int)blockClass.blockType - 1].gameObject.SetActive(true);

        backgroundImg.sprite = rankBackgroundArray[(int)blockClass.rankType];
        rankImg.sprite = rankBannerArray[(int)blockClass.rankType];
        rankText.text = blockClass.rankType.ToString();
        levelText.text = "Lv." + (blockClass.level + 1).ToString();

        //valueText.text = MoneyUnitString.ToCurrencyString(this.value);
    }

    public void InGame_SetLevel(int number)
    {
        levelText.text = "Lv." + (number + 1).ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(blockRootParent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;

        if(gameManager != null)
        {
            gameManager.blockDrag = true;
            gameManager.dragBlockType = blockClass.blockType;
        }

        blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1 / 255f);
        blockMain.SetActive(false);
        blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(true);
        blockIcon = blockMainArray[(int)blockClass.blockType - 1].blockIcon;

        blockIcon.color = new Color(1, 1, 1, 0.5f);

        gradient.SetActive(false);

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
            blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1);
            blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

            if (gameManager != null)
            {
                gameManager.CancleBetting(blockClass.blockType);
                gameManager.ResetPosBlock(index);
            }
        }
        else
        {
            gameManager.blockType = blockClass.blockType;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (gameManager != null)
        {
            gameManager.blockDrag = false;
            gameManager.dragBlockType = BlockType.Default;
        }

        blockIcon.color = new Color(1, 1, 1, 1f);

        isDrag = false;

        blockMainArray[(int)blockClass.blockType - 1].Betting(true, this);
        //blockMainArray[(int)blockClass.blockType - 1].SetBlock(GameStateManager.instance.NickName, value.ToString());
    }

    public void CancleBetting()
    {
        if (transform.parent != previousParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            blockMain.SetActive(true);
            blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1);
            blockIcon.color = new Color(1, 1, 1, 1f);
            blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

            isDrag = false;
        }

        if (blockClass.rankType > RankType.SR)
        {
            gradient.SetActive(true);
        }
        else
        {
            gradient.SetActive(false);
        }

        blockMainArray[(int)blockClass.blockType - 1].Betting(false, this);
    }

    public void ResetPos()
    {
        if (transform.parent != previousParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            blockMain.SetActive(true);
            blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1);
            blockIcon.color = new Color(1, 1, 1, 1f);
            blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

            isDrag = false;

            gameManager.ResetPosBlock(index);
        }

        if (blockClass.rankType > RankType.SR)
        {
            gradient.SetActive(true);
        }
        else
        {
            gradient.SetActive(false);
        }

        blockMainArray[(int)blockClass.blockType - 1].Betting(false, this);
    }

    public void TimeOver()
    {
        transform.SetParent(previousParent);
        transform.position = previousParent.GetComponent<RectTransform>().position;

        blockMain.SetActive(true);
        blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1);
        blockIcon.color = new Color(1, 1, 1, 1f);
        blockMainArray[(int)blockClass.blockType - 1].gameObject.SetActive(false);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;

        isDrag = false;

        gameManager.ResetPosBlock(index);
    }
}
