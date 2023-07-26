using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCharacter : MonoBehaviour
{
    public float smoothTime = 0.3f;
    public bool move = false;

    public Transform startPos;
    public Transform endPos;

    private Vector3 velocity = Vector3.zero;

    public void Initialize(Transform target)
    {
        transform.LookAt(target);

        transform.position = endPos.position;

        //move = true;
        //StartCoroutine(MoveCoroution());
    }

    IEnumerator MoveCoroution()
    {
        while (move)
        {
            transform.position = Vector3.SmoothDamp(transform.position, endPos.position, ref velocity, smoothTime);

            if (Vector3.Distance(endPos.position, transform.position) < 0.1f)
            {
                move = false;
            }
            yield return null;
        }
    }
}
