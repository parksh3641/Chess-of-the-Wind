using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Pinball2D : MonoBehaviour
{
    public Transform target;

    public Rigidbody2D rigid;

    public float rotateSpeed = 100;
    public float ballSpeed = 300f;
    public float speed = 1f;

    public float downSpeed = 0.001f;

    float randomX;
    float randomY;

    public bool rotate = false;
    public bool wind = false;

    public RouletteManager rouletteManager;
    public PhotonView PV;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    [Button]
    public void StartRotate()
    {
        int random = Random.Range(0, 4);

        switch(random)
        {
            case 0:
                transform.localPosition = new Vector3(0.95f, 0.53f, 4);
                break;
            case 1:
                transform.localPosition = new Vector3(-0.95f, 0.53f, 4);
                break;
            case 2:
                transform.localPosition = new Vector3(0, 1.48f, 4);
                break;
            case 3:
                transform.localPosition = new Vector3(0, -0.38f, 4);
                break;
        }

        speed = 1;
        rigid.drag = 0f;

        rotate = true;

        StartCoroutine(DownSpeed());
    }

    [Button]
    public void StartPinball(float power)
    {
        if (!wind)
        {
            StopAllCoroutines();

            rotate = false;

            randomX = Random.Range(-1f, 1f);
            randomY = Random.Range(-1f, 1f);

            Vector2 dir = new Vector2(randomX, randomY).normalized;

            rigid.AddForce(dir * (ballSpeed + (ballSpeed * power)));

            rigid.drag = 0.2f;

            Invoke("Wait", 3);
        }
    }

    void Wait()
    {
        wind = true;
    }

    public void MyTurn()
    {
        PV.RequestOwnership();

        StartRotate();
    }

    IEnumerator DownSpeed()
    {
        if(speed > 0)
        {
            speed -= downSpeed;
        }
        else
        {
            speed = 0;
            yield break;
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(DownSpeed());
    }

    private void FixedUpdate()
    {
        if(rotate)
        {
            transform.RotateAround(target.transform.position, Vector3.forward, rotateSpeed * speed  * Time.deltaTime);

            if(speed <= 0)
            {
                EndPinball();
            }
        }

        if (wind)
        {
            if (rigid.velocity.x == 0)
            {
                EndPinball();
            }

            if (Mathf.Abs(rigid.velocity.x) <= 0.1f)
            {
                rigid.drag = 2;
            }
        }
    }

    void EndPinball()
    {
        rotate = false;
        wind = false;

        rouletteManager.EndPinball();
    }
}
