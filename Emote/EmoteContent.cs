using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteContent : MonoBehaviour
{


    WaitForSeconds waitForSeconds = new WaitForSeconds(4);

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return waitForSeconds;

        gameObject.SetActive(false);
    }
}
