using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball3D : MonoBehaviour
{
    public Rigidbody rigid;
    public float power = 100f;
    private float time = 0f;

    public bool move = false;
    public bool wind = false;

    public Transform vector;

    public RouletteManager rouletteManager;
    public PhotonView PV;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

    }

    public void MyTurn()
    {
        PV.RequestOwnership();

        StartRotate();
    }

    public void StartRotate()
    {
        transform.position = new Vector3(1.6f, 0.74f, 3.5f);
        transform.rotation = Quaternion.Euler(0, 15, 0);

        rigid.AddForce(vector.forward * power);

        move = true;
    }

    public void StopPinabll()
    {
        move = false;
        wind = false;
    }

    public void StartPinball(float number)
    {
        rigid.AddForce(vector.forward * power * number);

        wind = true;
    }

    private void LateUpdate()
    {
        if (!move) return;

        if(rigid.velocity.magnitude < 0.5f)
        {
            time += Time.deltaTime;

            if(time >= 3)
            {
                EndPinball();
            }
        }
        else
        {
            time = 0;
        }
    }

    void EndPinball()
    {
        move = false;
        wind = false;

        time = 0;

        rouletteManager.EndPinball();
    }
}
