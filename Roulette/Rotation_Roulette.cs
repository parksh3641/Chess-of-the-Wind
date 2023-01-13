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

    public int GetRotate()
    {
        return (int)transform.rotation.eulerAngles.z;
    }

    public void StartRoulette()
    {
        speed = Random.Range(300, 600);

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
