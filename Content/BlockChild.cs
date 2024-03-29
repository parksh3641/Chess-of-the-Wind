using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockChild : MonoBehaviour, IBeginDragHandler
{
    RectTransform rectTransform;

    public BlockType blockType = BlockType.Default;

    private Color snowWorldColor = new Color(35 / 255f, 154 / 255f, 217 / 255f);
    private Color underWorldColor = new Color(217 / 255f, 77 / 255f, 129 / 255f);

    private Image image;

    public GameObject bettingMark;

    public BlockContent blockContent;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(190, 190);

        image = GetComponent<Image>();
        image.color = snowWorldColor;

        bettingMark.SetActive(false);
    }

    public void Initialize(BlockContent block)
    {
        blockContent = block;
    }

    public void SetBettingMark(bool check)
    {
        bettingMark.SetActive(check);
    }

    public void SetEnemy()
    {
        image.color = underWorldColor;
        image.raycastTarget = false;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        blockContent.OnBeginDrag(eventData);
    }
}
