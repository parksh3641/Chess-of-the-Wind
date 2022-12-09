using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Roulette : MonoBehaviour
{
    public float speed = 0f;

    public float minusSpeed = 0f;

    private void Awake()
    {
        speed = 0;

        minusSpeed = 0;
    }

    public void StartRoulette()
    {
        speed = 300;

        minusSpeed = 1f;
    }

    void FixedUpdate()
    {
        if(speed > 0)
        {
            speed -= minusSpeed;
        }
        else
        {
            speed = 0;
        }

        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}