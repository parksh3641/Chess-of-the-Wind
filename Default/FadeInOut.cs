using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;

    public float fadeSpeed = 1.0f;
    public float alpha = 1f;

    private Image image;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

    public CanvasGroup canvasGroup;

    void Awake()
    {
        instance = this;

        image = canvasGroup.GetComponent<Image>();
    }

    [Button]
    public void FadeIn() //���������� ������ �������
    {
        StartCoroutine(Fade(true));
    }

    [Button]
    public void FadeOut() //������� õõ�� ����������
    {
        StartCoroutine(Fade());
    }

    [Button]
    public void FadeOutToIn() //������� ���������� ���
    {
        StartCoroutine(Fade(false));
    }

    [Button]
    public void StoryFadeOut()
    {
        StartCoroutine(FadeOutCoroution());
    }

    [Button]
    public void InGameFadeIn_Plus()
    {
        InGameFadeIn(Color.green);
    }

    [Button]
    public void InGameFadeIn_Minus()
    {
        InGameFadeIn(Color.red);
    }

    public void InGameFadeIn(Color color)
    {
        image.color = color;
        FadeOutToIn();
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        if (isFadeIn)
        {
            canvasGroup.alpha = alpha;
            Tween tween = canvasGroup.DOFade(0f, fadeSpeed);
            yield return tween.WaitForCompletion();
            canvasGroup.gameObject.SetActive(false);
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(true);
            Tween tween = canvasGroup.DOFade(alpha, fadeSpeed);
            yield return tween.WaitForCompletion();
            yield return waitForSeconds;
            StartCoroutine(Fade(true));
        }
    }

    private IEnumerator FadeOutCoroution()
    {
        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(true);
        Tween tween = canvasGroup.DOFade(alpha, fadeSpeed);
        yield return tween.WaitForCompletion();
        canvasGroup.alpha = 0;
    }

    private IEnumerator Fade()
    {
        canvasGroup.alpha = alpha;
        Tween tween = canvasGroup.DOFade(0f, fadeSpeed);
        yield return tween.WaitForCompletion();
        canvasGroup.gameObject.SetActive(false);
        canvasGroup.alpha = alpha;
    }
}