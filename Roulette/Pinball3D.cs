using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball3D : MonoBehaviour
{
    public Transform vector;
    public Rigidbody rigid;

    private float windPower = 0;

    public int index = 0;
    public int ballPos = 0;

    private int rotate = 0;

    private float time = 0f;
    private int random = 0;

    public bool tutorial = false;
    public bool move = false;

    Vector3 normal = new Vector3(0.15f, 0.15f, 0.15f);
    Vector3 sizeUp = new Vector3(0.225f, 0.225f, 0.225f);

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
    WaitForSeconds waitForSeconds2 = new WaitForSeconds(10f);
    WaitForSeconds waitForSeconds3 = new WaitForSeconds(20f);

    Vector3 defaultGravity = new Vector3(0, -9.81f, 0);
    Vector3 setGravity = new Vector3(0, -19.62f, 0);

    public RouletteManager rouletteManager;
    public PhotonView PV;
    public TutorialManager tutorialManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.useGravity = false;
        rigid.isKinematic = true;

    }

    void FixedUpdate()
    {
        if (transform.position.y < -5)
        {
            Debug.Log("공이 밖으로 나감");

            rigid.useGravity = false;
            rigid.isKinematic = true;

            StartRotate(index);
        }

        if (!move) return;

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
        if(PV != null) PV.RequestOwnership();

        if(GameStateManager.instance.GameEventType == GameEventType.GameEvent5)
        {
            transform.localScale = sizeUp;
        }
        else
        {
            transform.localScale = normal;
        }

        StartRotate(number);
    }

    public void StartRotate(int number)
    {
        Physics.gravity = defaultGravity;

        rigid.useGravity = true;
        rigid.isKinematic = false;

        index = number;

        if(GameStateManager.instance.GameRankType > GameRankType.Sliver_1)
        {
            random = Random.Range(0, 2);
        }
        else
        {
            random = 0;
        }

        if (tutorial) random = 0;

        if (number == 0)
        {
            if(random == 0)
            {
                transform.position = new Vector3(-0.9f, 0.45f, 3.3f);
                transform.rotation = Quaternion.Euler(0, 20, 0);

                rotate = 0;
            }
            else
            {
                transform.position = new Vector3(-4.1f, 0.45f, 3.3f);
                transform.rotation = Quaternion.Euler(0, -20, 0);

                rotate = 1;
            }
        }
        else
        {
            if (random == 0)
            {
                transform.position = new Vector3(4.1f, 0.45f, 3.3f);
                transform.rotation = Quaternion.Euler(0, 20, 0);

                rotate = 0;
            }
            else
            {
                transform.position = new Vector3(0.9f, 0.45f, 3.3f);
                transform.rotation = Quaternion.Euler(0, -20, 0);

                rotate = 1;
            }
        }

        rigid.mass = 1;

        rigid.AddForce(vector.forward * 200);

        move = true;

        StopAllCoroutines();
        StartCoroutine(GravityCoroution());
    }

    IEnumerator GravityCoroution()
    {
        if(GameStateManager.instance.GameType == GameType.NewBie)
        {
            yield return waitForSeconds3;
        }
        else
        {
            yield return waitForSeconds2;
        }

        Physics.gravity = setGravity;

        Debug.Log("공에 중력 적용됩니다");
    }

    public void BlowingWind(float defaultPower, float force, int number)
    {
        StopAllCoroutines();

        rigid.mass = 1;

        windPower = defaultPower;

        if (index == 0) //왼쪽
        {
            if(number == 0) //아래
            {
                switch(ballPos)
                {
                    case 0:
                        transform.LookAt(rouletteManager.leftWindPoint[4].position);

                        if(rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        break;
                    case 1:
                        transform.LookAt(rouletteManager.leftWindPoint[5].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }

                        break;
                    case 2:
                        transform.LookAt(rouletteManager.leftWindPoint[6].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }

                        break;
                    case 3:
                        transform.LookAt(rouletteManager.leftWindPoint[7].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }

                        break;
                }
            }
            else
            {
                switch (ballPos)
                {
                    case 4:
                        transform.LookAt(rouletteManager.leftWindPoint[0].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }

                        break;
                    case 5:
                        transform.LookAt(rouletteManager.leftWindPoint[1].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }

                        break;
                    case 6:
                        transform.LookAt(rouletteManager.leftWindPoint[2].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }

                        break;
                    case 7:
                        transform.LookAt(rouletteManager.leftWindPoint[3].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }

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
                        transform.LookAt(rouletteManager.rightWindPoint[4].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        break;
                    case 1:
                        transform.LookAt(rouletteManager.rightWindPoint[5].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }

                        break;
                    case 2:
                        transform.LookAt(rouletteManager.rightWindPoint[6].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }

                        break;
                    case 3:
                        transform.LookAt(rouletteManager.rightWindPoint[7].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }

                        break;
                }
            }
            else
            {
                switch (ballPos)
                {
                    case 4:
                        transform.LookAt(rouletteManager.rightWindPoint[0].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }

                        break;
                    case 5:
                        transform.LookAt(rouletteManager.rightWindPoint[1].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }

                        break;
                    case 6:
                        transform.LookAt(rouletteManager.rightWindPoint[2].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.2f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }

                        break;
                    case 7:
                        transform.LookAt(rouletteManager.rightWindPoint[3].position);

                        if (rotate == 0) //반 시계 방향 회전중
                        {
                            rigid.AddForce(vector.forward * (windPower * 1.4f + force));
                        }
                        else
                        {
                            rigid.AddForce(vector.forward * (windPower + force));
                        }

                        break;
                }
            }
        }
    }

    void EndPinball()
    {
        Physics.gravity = defaultGravity;

        move = false;
        rigid.useGravity = false;
        rigid.isKinematic = true;

        time = 0;
        ballPos = 0;

        StopAllCoroutines();

        if (rouletteManager != null) rouletteManager.EndPinball();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("WindPoint1"))
        {
            ballPos = 0;
        }
        else if (other.transform.CompareTag("WindPoint2"))
        {
            ballPos = 1;
        }
        else if (other.transform.CompareTag("WindPoint3"))
        {
            ballPos = 2;
        }
        else if (other.transform.CompareTag("WindPoint4"))
        {
            ballPos = 3;
        }
        else if (other.transform.CompareTag("WindPoint5"))
        {
            ballPos = 4;
        }
        else if (other.transform.CompareTag("WindPoint6"))
        {
            ballPos = 5;
        }
        else if (other.transform.CompareTag("WindPoint7"))
        {
            ballPos = 6;
        }
        else if (other.transform.CompareTag("WindPoint8"))
        {
            ballPos = 7;
        }
    }

    public void BlowTargetBlow(Transform target)
    {
        move = false;

        rigid.useGravity = false;
        rigid.isKinematic = true;

        StartCoroutine(MoveTarget(target));
    }

    Vector3 speed = Vector3.zero;

    IEnumerator MoveTarget(Transform target)
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref speed, 0.1f);

        yield return waitForSeconds;

        if (Vector3.Distance(target.position, transform.position) < 0.1f)
        {
            tutorialManager.EndBallAi();

            EndPinball();

            yield break;
        }

        StartCoroutine(MoveTarget(target));
    }

    public void Stop()
    {
        move = false;

        rigid.useGravity = false;
        rigid.isKinematic = true;
    }
}
