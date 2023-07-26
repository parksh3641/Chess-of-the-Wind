using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform targetPosition;

    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    public bool isActive = false;
    //public bool isRotation = false;

    public RouletteManager rouletteManager;

    public void Initialize(Transform target)
    {
        transform.position = target.position;
        //transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target;

        isActive = true;

        StartCoroutine(MoveCameraCoroution());
    }

    IEnumerator MoveCameraCoroution()
    {
        while (isActive)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition.position, ref velocity, smoothTime);

            if (Vector3.Distance(targetPosition.position, transform.position) < 0.1f)
            {
                isActive = false;
                //isRotation = true;

                rouletteManager.PlayRouletteDelay();
            }
            yield return null;
        }

        //while(isRotation)
        //{
        //    camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetPosition.rotation, smoothTime);

        //    if(camera.transform.rotation.x <= targetPosition.rotation.x)
        //    {
        //        isRotation = false;
        //    }

        //    yield return null;
        //}
    }
}
