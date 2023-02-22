using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball3D : MonoBehaviour
{
    private float power = 300f;
    public int index = 0;
    public int ballPos = 0;

    private float time = 0f;

    private float midWindPower = 0.5f;

    public bool move = false;

    public Transform vector;

    public Transform[] leftWindPoint;
    public Transform[] rightWindPoint;

    Rigidbody rigid;

    public RouletteManager rouletteManager;
    public PhotonView PV;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.useGravity = false;
        rigid.isKinematic = true;

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

        rigid.AddForce(vector.forward * power);

        move = true;
    }

    public void BlowingWind(float force, int number)
    {
        Debug.Log(number + "���� �߻� / ����ġ : " + ballPos);

        float windPower = 0;

        if (index == 0) //���� ������
        {
            switch (number) //�ٶ��� �� ��ġ
            {
                case 0:
                    transform.LookAt(leftWindPoint[1].position + new Vector3(1, 0.5f, -1));

                    switch (ballPos) //���� �� �ִ� ��ġ
                    {
                        case 0:
                            windPower = power;
                            break;
                        case 1:
                            windPower = power * midWindPower;
                            break;
                    }
                    break;
                case 1:
                    transform.LookAt(leftWindPoint[0].position + new Vector3(-1, 0.5f, -1));

                    switch (ballPos) //���� �� �ִ� ��ġ
                    {
                        case 0:
                            windPower = power * midWindPower;
                            break;
                        case 1:
                            windPower = power;
                            break;
                    }
                    break;
            }
        }
        else
        {
            switch (number) //�ٶ��� �� ��ġ
            {
                case 0:
                    transform.LookAt(rightWindPoint[1].position + new Vector3(1, 0.5f, -1));

                    switch (ballPos) //���� �� �ִ� ��ġ
                    {
                        case 0:
                            windPower = power;
                            break;
                        case 1:
                            windPower = power * midWindPower;
                            break;
                    }
                    break;
                case 1:
                    transform.LookAt(rightWindPoint[0].position + new Vector3(-1, 0.5f, -1));

                    switch (ballPos) //���� �� �ִ� ��ġ
                    {
                        case 0:
                            windPower = power * midWindPower;
                            break;
                        case 1:
                            windPower = power;
                            break;
                    }
                    break;
            }
        }

        rigid.AddForce(vector.forward * windPower);
        //rigid.AddForce(vector.forward * (windPower * (0.02f + force)));
    }

    private void FixedUpdate()
    {
        if (!move) return;

        if(transform.position.y < -2)
        {
            StartRotate(index);
        }

        if(rigid.velocity.magnitude < 0.3f)
        {
            time += Time.deltaTime;

            if(time >= 2)
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

        rigid.useGravity = false;
        rigid.isKinematic = true;

        time = 0;

        rouletteManager.EndPinball();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(index == 0)
        {
            if (other.transform.tag == "LeftWindPoint1")
            {
                ballPos = 0;
            }
            else if (other.transform.tag == "LeftWindPoint2")
            {
                ballPos = 1;
            }
        }
        else
        {
            if (other.transform.tag == "RightWindPoint1")
            {
                ballPos = 0;
            }
            else if (other.transform.tag == "RightWindPoint2")
            {
                ballPos = 1;
            }
        }
    }
}
