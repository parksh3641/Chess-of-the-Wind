using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyContent : MonoBehaviour
{
    private float posX = 0;
    private float posY = 0;

    private Vector3 startPos;
    private Vector3 endPos;

    private Vector3 vel = Vector3.zero;


    void OnDisable()
    {
        StopAllCoroutines();
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

        posX = Random.Range(-300, 300);
        posY = Random.Range(-150, 150);

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