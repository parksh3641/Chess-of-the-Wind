using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball : MonoBehaviour
{
    public Rigidbody rigid;
    public float power = 100f;

    public Transform vector;

    public RouletteManager rouletteManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

    }

    public void StartPinball()
    {
        transform.position = new Vector3(1.6f, 0.74f, 3.5f);
        transform.rotation = Quaternion.Euler(0, 15, 0);

        rigid.AddForce(vector.forward * power);
    }

    public void AddSpeed(float number)
    {
        rigid.AddForce(vector.forward * 200 * number);
    }
}
