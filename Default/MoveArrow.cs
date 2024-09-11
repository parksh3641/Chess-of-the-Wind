using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public Vector2 targetPosition;
    public float moveSpeed = 2f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        originalPosition = rectTransform.anchoredPosition;

        MoveToTarget();
    }

    private void OnDisable()
    {
        rectTransform.anchoredPosition = originalPosition;

        StopAllCoroutines();
    }


    void MoveToTarget()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            yield return MoveTo(targetPosition);
            yield return MoveTo(originalPosition);
        }
    }

    IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(rectTransform.anchoredPosition, target) > 50f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition;
    }
}