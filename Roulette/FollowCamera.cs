using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;

    public float offsetX = 0.0f;
    public float offsetY = 5.0f;
    public float offsetZ = -5.0f;

    public float speed = 5.0f;

    Vector3 TargetPos;

    public void SetBall(GameObject ball)
    {
        target = ball;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            TargetPos = new Vector3(target.transform.position.x + offsetX, target.transform.position.y + offsetY, target.transform.position.z + offsetZ
                );

            transform.position = TargetPos;
        }
    }
}
