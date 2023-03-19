using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball3D : MonoBehaviour
{
    public Transform vector;
    public Rigidbody rigid;

    private float power = 150f;
    private float windPower = 0;
    public int index = 0;
    public int ballPos = 0;

    private float time = 0f;
    private float midWindPower = 0f;

    public bool move = false;

    public RouletteManager rouletteManager;
    public PhotonView PV;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.useGravity = false;
        rigid.isKinematic = true;

    }

    void Update()
    {
        if (!move) return;

        if (transform.position.y < -2)
        {
            StartRotate(index);
        }

        if (rigid.velocity.magnitude < 0.3f)
        {
            time += Time.deltaTime;

            if (time >= 2)
            {
                EndPinball();
            }
        }
        else
        {
            time = 0;
        }
    }

    public void MyTurn(int number)
    {
        PV.RequestOwnership();

        rigid.useGravity = true;
        rigid.isKinematic = false;

        StartRotate(number);
    }

    public void StartRotate(int number)
    {
        index = number;

        if (number == 0)
        {
            transform.position = new Vector3(-1.4f, 0.25f, 3.3f);
        }
        else
        {
            transform.position = new Vector3(4.55f, 0.25f, 3.3f);
        }

        transform.rotation = Quaternion.Euler(0, 20, 0);

        rigid.AddForce(vector.forward * 200);

        move = true;
    }

    public void BlowingWind(float force, int number)
    {
        //Debug.Log(number + "에서 발사 / 공위치 : " + ballPos);

        windPower = power;

        if (index == 0) //현재 룰렛 위치는? 왼쪽 오른쪽
        {
            if(number == 0)
            {
                switch(ballPos)
                {
                    case 0:
                        transform.LookAt(rouletteManager.leftWindPoint[4].position + new Vector3(-1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower * 1.5f + force));
                        break;
                    case 1:
                        transform.LookAt(rouletteManager.leftWindPoint[4].position + new Vector3(-1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                    case 2:
                        transform.LookAt(rouletteManager.leftWindPoint[5].position + new Vector3(-1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                }
            }
            else
            {
                switch (ballPos)
                {
                    case 3:
                        transform.LookAt(rouletteManager.leftWindPoint[0].position + new Vector3(1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                    case 4:
                        transform.LookAt(rouletteManager.leftWindPoint[1].position + new Vector3(1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                    case 5:
                        transform.LookAt(rouletteManager.leftWindPoint[1].position + new Vector3(1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower * 1.5f + force));
                        break;
                }
            }
        }
        else
        {
            if (number == 0)
            {
                switch (ballPos)
                {
                    case 0:
                        transform.LookAt(rouletteManager.rightWindPoint[4].position + new Vector3(-1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower * 1.5f + force));
                        break;
                    case 1:
                        transform.LookAt(rouletteManager.rightWindPoint[4].position + new Vector3(-1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                    case 2:
                        transform.LookAt(rouletteManager.rightWindPoint[5].position + new Vector3(-1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                }
            }
            else
            {
                switch (ballPos)
                {
                    case 3:
                        transform.LookAt(rouletteManager.rightWindPoint[0].position + new Vector3(1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                    case 4:
                        transform.LookAt(rouletteManager.rightWindPoint[1].position + new Vector3(1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower + force));
                        break;
                    case 5:
                        transform.LookAt(rouletteManager.rightWindPoint[1].position + new Vector3(1, 0.5f, -1));

                        rigid.AddForce(vector.forward * (windPower * 1.5f + force));
                        break;
                }
            }
        }
    }

    void EndPinball()
    {
        move = false;

        rigid.useGravity = false;
        rigid.isKinematic = true;

        time = 0;

        rouletteManager.EndPinball();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "WindPoint1")
        {
            ballPos = 0;
        }
        else if (other.transform.tag == "WindPoint2")
        {
            ballPos = 1;
        }
        else if (other.transform.tag == "WindPoint3")
        {
            ballPos = 2;
        }
        else if (other.transform.tag == "WindPoint4")
        {
            ballPos = 3;
        }
        else if (other.transform.tag == "WindPoint5")
        {
            ballPos = 4;
        }
        else if (other.transform.tag == "WindPoint6")
        {
            ballPos = 5;
        }
    }
}
