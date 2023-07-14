using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerController : MonoBehaviour
{
    public int index = 0;

    public float fingerSmoothTime = 1f;
    public float fingerPushTime = 1f;

    [Title("Finger")]
    public GameObject finger;

    public Transform firstFingerPos;
    public Transform endFingerPos;
    public Transform pushFingerPos;

    public bool fingerMove = false;
    public bool fingerPush = false;

    [Title("Button")]
    public GameObject button;

    public Transform firstButtonPos;
    public Transform endButtonPos;

    private Vector3 velocity = Vector3.zero;

    public RouletteManager rouletteManager;

    private void Awake()
    {

    }

    [Button]
    public void Initialize()
    {
        finger.transform.position = firstFingerPos.position;
        button.transform.position = firstButtonPos.position;

        fingerMove = false;
        fingerPush = false;

        finger.SetActive(false);
    }
    
    public void Disable()
    {
        finger.SetActive(false);
    }

    [Button]
    public void MoveFinger()
    {
        finger.SetActive(true);

        fingerMove = true;

        StartCoroutine(FingerMoveCoroution());
    }

    IEnumerator FingerMoveCoroution()
    {
        while(fingerMove)
        {
            finger.transform.position = Vector3.SmoothDamp(finger.transform.position, endFingerPos.position, ref velocity, fingerSmoothTime);

            if (Vector3.Distance(endFingerPos.position, finger.transform.position) < 0.1f)
            {
                fingerMove = false;
                fingerPush = true;
            }
            yield return null;
        }

        while(fingerPush)
        {
            finger.transform.position = Vector3.SmoothDamp(finger.transform.position, pushFingerPos.position, ref velocity, fingerPushTime);
            button.transform.position = Vector3.SmoothDamp(button.transform.position, endButtonPos.position, ref velocity, fingerPushTime);

            if (Vector3.Distance(pushFingerPos.position, finger.transform.position) < 0.1f)
            {
                fingerPush = false;

                yield return new WaitForSeconds(0.2f);

                EndMoveFinger();
            }
            yield return null;
        }
    }
    public void EndMoveFinger()
    {
        rouletteManager.EndMoveFinger(index);
    }
}
