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
    Image backgroundImg;

    public Image rankImg;
    public Text rankText;

    Sprite[] rankBackgroundArray;
    Sprite[] rankBannerArray;

    public Image blockIcon;
    public Image blockUI;
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

        backgroundImg = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

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

        blockUIArray[(int)blockClass.blockType - 1].gameObject.SetActive(true);

        backgroundImg.sprite = rankBackgroundArray[(int)blockClass.rankType];
        rankImg.sprite = rankBannerArray[(int)blockClass.rankType];
        rankText.text = blockClass.rankType.ToString();

        //valueText.text = MoneyUnitString.ToCurrencyString(this.value);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(blockRootParent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;

        gameManager.blockDrag = true;
        gameManager.blockType = blockClass.blockType;

        blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1 / 255f);
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
            blockUI.color = new Color(blockUI.color.r, blockUI.color.g, blockUI.color.b, 1);
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

        blockMainArray[(int)blockClass.blockType - 1].Betting(false);
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

        blockMainArray[(int)blockClass.blockType - 1].Betting(false);
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
