using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class BlockContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockType blockType = BlockType.Default;

    Image image;
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
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        blockMain.SetActive(true);
    }

    public void Initialize(GameManager manager, Transform root, Transform grid, BlockType type)
    {
        gameManager = manager;
        blockRootParent = root;
        previousParent = grid;
        blockType = type;

        for (int i = 0; i < blockUIArray.Length; i++)
        {
            blockUIArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blockMainArray.Length; i++)
        {
            blockMainArray[i].gameObject.SetActive(false);
        }

        blockUIArray[(int)blockType - 1].gameObject.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //previousParent = transform.parent;

        transform.SetParent(blockRootParent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;

        gameManager.blockDrag = true;
        gameManager.blockType = blockType;

        image.color = new Color(1, 1, 1, 1 / 255f);
        blockMain.SetActive(false);
        blockMainArray[(int)blockType - 1].gameObject.SetActive(true);

        isDrag = true;

        for (int i = 0; i < blockMainArray[(int)blockType - 1].blockChildArray.Length; i++)
        {
            blockMainArray[(int)blockType - 1].blockChildArray[i].SetBettingMark(false);
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

            image.color = new Color(1, 1, 1, 1);
            blockMain.SetActive(true);
            blockMainArray[(int)blockType - 1].gameObject.SetActive(false);

            gameManager.CancleBetting(blockType);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;
        gameManager.blockType = BlockType.Default;

        isDrag = false;

        for (int i = 0; i < blockMainArray[(int)blockType - 1].blockChildArray.Length; i++)
        {
            blockMainArray[(int)blockType - 1].blockChildArray[i].SetBettingMark(true);
        }
    }
    public void ResetPos()
    {
        if (transform.parent != previousParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            image.color = new Color(1, 1, 1, 1);
            blockMain.SetActive(true);
            blockMainArray[(int)blockType - 1].gameObject.SetActive(false);

            isDrag = false;
        }

        for (int i = 0; i < blockMainArray[(int)blockType - 1].blockChildArray.Length; i++)
        {
            blockMainArray[(int)blockType - 1].blockChildArray[i].SetBettingMark(false);
        }
    }

    public void TimeOver()
    {
        transform.SetParent(previousParent);
        transform.position = previousParent.GetComponent<RectTransform>().position;

        image.color = new Color(1, 1, 1, 1);
        blockMain.SetActive(true);
        blockMainArray[(int)blockType - 1].gameObject.SetActive(false);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;

        isDrag = false;
    }
}
