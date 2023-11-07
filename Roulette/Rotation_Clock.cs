using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Clock : MonoBehaviour
{
    public int speed = 40;
    public int saveSpeed = 0;

    bool start = false;

    public Transform parentTransform;
    public MeshRenderer meshRenderer;
    public Transform queenPoint;

    public PhotonView PV;

    WaitForSeconds waitForSecond = new WaitForSeconds(0.01f);

    Vector3 vector3 = new Vector3(0, 1, 0);

    public void Awake()
    {
        transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        speed = Random.Range(speed - 2, speed + 2);

        saveSpeed = speed;

        PV = GetComponent<PhotonView>();
    }

    public void StartClock()
    {
        start = true;

        if (GameStateManager.instance.GameEventType == GameEventType.GameEvent4)
        {
            speed = saveSpeed + (saveSpeed / 2);
        }
        else
        {
            speed = saveSpeed;
        }
    }

    public void StopClock()
    {
        start = false;
    }

    void Update()
    {
        if (!start) return;

        transform.Rotate(vector3 * speed * Time.deltaTime);
    }
}
