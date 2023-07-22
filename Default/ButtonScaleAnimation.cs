using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScaleAnimation : MonoBehaviour
{
    [Header("Regular")]
    public bool regular = false;

    [Header("First Delay")]
    public bool first = false;
    private bool isStop = false;
    [Range(1, 10)]
    public float firstDelay = 2f;

    [Header("Scale Delay")]
    [Range(1, 10)]
    public float delay = 5f;

    private float speed = 0.015f;
    private float minScale = 0.9f;
    private float maxScale = 1.15f;

    float scale = 0;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);

    private void OnEnable()
    {
        if (!isStop)
        {
            StopAllCoroutines();
            StartCoroutine(ButtonAnimation());
        }
    }

    public void StopAnim()
    {
        StopAllCoroutines();

        isStop = true;

        transform.localScale = Vector3.one;
    }

    public void PlayAnim()
    {
        StopAllCoroutines();

        StartCoroutine(ButtonAnimation());
    }

    IEnumerator ButtonAnimation()
    {
        scale = 1;

        if (first)
        {
            yield return new WaitForSeconds(firstDelay);
        }

        while (transform.localScale.x > minScale)
        {
            scale -= speed;

            transform.localScale = Vector3.one * scale;

            yield return waitForSeconds;
        }

        while (transform.localScale.x < maxScale)
        {
            scale += speed;

            transform.localScale = Vector3.one * scale;

            yield return waitForSeconds;
        }

        while (transform.localScale.x > 1)
        {
            scale -= speed;

            transform.localScale = Vector3.one * scale;

            yield return waitForSeconds;
        }

        transform.localScale = Vector3.one;

        if (regular)
        {
            yield return new WaitForSeconds(delay);
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(delay * 0.8f, delay * 1.2f));
        }

        StartCoroutine(ButtonAnimation());
    }
}