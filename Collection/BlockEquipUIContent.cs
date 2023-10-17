using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockEquipUIContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockClass blockClass;

    Image backgroundImg;

    public Image rankImg;
    public Text rankText;

    public Image rankSSRImg;
    public Text rankSSRText;


    public GameObject[] blockUIArray;

    public Transform blockRootParent;
    public Transform previousParent;
    private CanvasGroup canvasGroup;

    public Text levelText;

    Sprite[] rankBackgroundArray;
    Sprite[] rankBannerArray;

    ImageDataBase imageDataBase;

    void Awake()
    {
        if (imageDataBase == null) imageDataBase = Resources.Load("ImageDataBase") as ImageDataBase;

        rankBackgroundArray = imageDataBase.GetRankBackgroundArray();
        rankBannerArray = imageDataBase.GetRankBannerArray();

        backgroundImg = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        levelText.text = "";

        rankSSRImg.gameObject.SetActive(false);
    }

    public void Initialize(BlockClass block)
    {
        blockClass = block;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].SetActive(false);
        }

        blockUIArray[(int)blockClass.blockType - 1].SetActive(true);

        backgroundImg.sprite = rankBackgroundArray[(int)blockClass.rankType];
        rankImg.sprite = rankBannerArray[(int)blockClass.rankType];
        rankSSRImg.sprite = rankBannerArray[(int)blockClass.rankType];
        rankText.text = blockClass.rankType.ToString();

        if (blockClass.level > 0)
        {
            levelText.text = "Lv." + (blockClass.level + 1).ToString();
        }
        else
        {
            levelText.text = "Lv.1";
        }

        if (block.ssrLevel > 0)
        {
            rankSSRImg.gameObject.SetActive(true);
            rankSSRText.text = block.ssrLevel.ToString();
        }
        else
        {
            rankSSRImg.gameObject.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(blockRootParent);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == blockRootParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.blocksRaycasts = true;
    }
}
