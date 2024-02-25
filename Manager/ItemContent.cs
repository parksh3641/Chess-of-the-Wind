using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContent : MonoBehaviour
{
    Image icon;

    private float posX = 0;
    private float posY = 0;

    private Vector3 startPos;
    private Vector3 endPos;

    private Vector3 vel = Vector3.zero;

    Color color_N = new Color(107 / 255f, 173 / 255f, 255 / 255f);
    Color color_R = new Color(122 / 255f, 182 / 255f, 101 / 255f);
    Color color_SR = new Color(120 / 255f, 92 / 255f, 200 / 255f);
    Color color_SSR = new Color(222 / 255f, 170 / 255f, 57 / 255f);
    Color color_UR = new Color(240 / 255f, 82 / 255f, 160 / 255f);

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetRank(RankType type)
    {
        switch (type)
        {
            case RankType.N:
                icon.color = color_N; 
                break;
            case RankType.R:
                icon.color = color_R;
                break;
            case RankType.SR:
                icon.color = color_SR;
                break;
            case RankType.SSR:
                icon.color = color_SSR;
                break;
            case RankType.UR:
                icon.color = color_UR;
                break;
        }
    }

    IEnumerator RandomMoveCorution()
    {
        float time = 0;

        while (time <= 1.0f)
        {
            time += Time.deltaTime;

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, startPos, ref vel, 0.5f);

            yield return null;
        }

        StartCoroutine(GoToTargetCoroution());
    }

    public void GoToTarget(Vector3 start, Vector3 end)
    {
        StopAllCoroutines();

        transform.localPosition = start;

        endPos = end;

        posX = Random.Range(-400, 400);
        posY = Random.Range(-200, 200);

        startPos = start + new Vector3(posX, posY, 0);

        StartCoroutine(RandomMoveCorution());
    }

    IEnumerator GoToTargetCoroution()
    {
        float time = 0;

        while (time <= 1.0f)
        {
            time += Time.deltaTime;

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, endPos, ref vel, 0.25f);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
