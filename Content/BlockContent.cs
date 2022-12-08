using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class BlockContent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockType blockType = BlockType.Default;

    public Text infoText;

    public GameObject blockMain;
    public BlockChildContent[] blockMainArray;
    public BlockChildContent[] blockArray;

    [Title("Drag")]
    private Transform blockRootParent;
    private Transform previousParent;
    private CanvasGroup canvasGroup;
    public bool isDrag = false;

    GameManager gameManager;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        blockMain.SetActive(true);

        for (int i = 0; i < blockMainArray.Length; i++)
        {
            blockMainArray[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < blockArray.Length; i ++)
        {
            blockArray[i].gameObject.SetActive(false);
        }
    }


    public void Initialize(GameManager manager, Transform root, Transform grid, BlockType type)
    {
        gameManager = manager;
        blockRootParent = root;
        previousParent = grid;
        blockType = type;
        infoText.text = blockType.ToString() + "자형 블록";

        blockMainArray[(int)blockType - 1].gameObject.SetActive(true);
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

        blockMain.SetActive(false);
        blockArray[(int)blockType - 1].gameObject.SetActive(true);

        isDrag = true;

        for (int i = 0; i < blockArray[(int)blockType - 1].blockChildArray.Length; i++)
        {
            blockArray[(int)blockType - 1].blockChildArray[i].SetBettingMark(false);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(isDrag) transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == blockRootParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            blockMain.SetActive(true);
            blockArray[(int)blockType - 1].gameObject.SetActive(false);

            gameManager.ResetRouletteContent();
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;
        gameManager.blockType = BlockType.Default;

        isDrag = false;

        for (int i = 0; i < blockArray[(int)blockType - 1].blockChildArray.Length; i++)
        {
            blockArray[(int)blockType - 1].blockChildArray[i].SetBettingMark(true);
        }
    }
    public void ResetPos()
    {
        if (transform.parent != previousParent)
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.GetComponent<RectTransform>().position;

            blockMain.SetActive(true);
            blockArray[(int)blockType - 1].gameObject.SetActive(false);

            isDrag = false;
        }

        for (int i = 0; i < blockArray[(int)blockType - 1].blockChildArray.Length; i++)
        {
            blockArray[(int)blockType - 1].blockChildArray[i].SetBettingMark(false);
        }
    }

    public void TimeOver()
    {
        transform.SetParent(previousParent);
        transform.position = previousParent.GetComponent<RectTransform>().position;

        blockMain.SetActive(true);
        blockArray[(int)blockType - 1].gameObject.SetActive(false);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;

        isDrag = false;
    }
}
