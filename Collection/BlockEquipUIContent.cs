using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockEquipUIContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockClass blockClass;

    public BlockChildContent[] blockUIArray;

    public Transform blockRootParent;
    public Transform previousParent;
    private CanvasGroup canvasGroup;

    public Text levelText;

    Image backgroundImg;

    void Awake()
    {
        backgroundImg = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        levelText.text = "";
    }

    public void Initialize(BlockClass block)
    {
        blockClass = block;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].gameObject.SetActive(false);
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

        if (blockClass.level > 0)
        {
            levelText.text = (blockClass.level + 1).ToString();
        }
        else
        {
            levelText.text = "";
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
