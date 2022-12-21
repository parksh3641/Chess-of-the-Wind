using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject camera;

    public Transform targetPosition;

    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    public bool isActive = false;

    public void Initialize(Transform target)
    {
        transform.position = target.position;
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target;

        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPosition.position, ref velocity, smoothTime);

            if(Vector3.Distance(targetPosition.position, camera.transform.position) < 0.1f)
            {
                isActive = false;
            } 
        }
    }
}
