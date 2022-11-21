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

    public GameObject[] blockArray;

    [Title("Drag")]
    private Transform blockRootParent;
    private Transform previousParent;
    private CanvasGroup canvasGroup;

    GameManager gameManager;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        for (int i = 0; i < blockArray.Length; i ++)
        {
            blockArray[i].gameObject.SetActive(false);
        }
    }


    public void Initialize(GameManager manager, Transform root, BlockType type)
    {
        gameManager = manager;
        blockRootParent = root;
        blockType = type;
        infoText.text = blockType.ToString() + "자형 블록";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        transform.SetParent(blockRootParent);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        gameManager.blockDrag = true;
        gameManager.blockType = blockType;

        blockArray[(int)blockType - 1].gameObject.SetActive(true);
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

            blockArray[(int)blockType - 1].gameObject.SetActive(false);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        gameManager.blockDrag = false;
        gameManager.blockType = BlockType.Default;
    }
}
