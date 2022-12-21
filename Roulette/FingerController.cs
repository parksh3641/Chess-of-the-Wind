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

    public bool buttonPush = false;

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

        buttonPush = false;
    }

    [Button]
    public void MoveFinger()
    {
        fingerMove = true;
    }

    private void Update()
    {
        if(fingerMove)
        {
            finger.transform.position = Vector3.SmoothDamp(finger.transform.position, endFingerPos.position, ref velocity, fingerSmoothTime);

            if (Vector3.Distance(endFingerPos.position, finger.transform.position) < 0.1f)
            {
                fingerMove = false;
                fingerPush = true;
                buttonPush = true;
            }
        }

        if(fingerPush)
        {
            finger.transform.position = Vector3.SmoothDamp(finger.transform.position, pushFingerPos.position, ref velocity, fingerPushTime);

            if (Vector3.Distance(pushFingerPos.position, finger.transform.position) < 0.1f)
            {
                fingerPush = false;
            }
        }

        if(buttonPush)
        {
            button.transform.position = Vector3.SmoothDamp(button.transform.position, endButtonPos.position, ref velocity, fingerPushTime);

            if (Vector3.Distance(endButtonPos.position, button.transform.position) < 0.1f)
            {
                buttonPush = false;

                EndMoveFinger();
            }
        }
    }

    public void EndMoveFinger()
    {
        rouletteManager.EndMoveFinger(index);
    }
}
