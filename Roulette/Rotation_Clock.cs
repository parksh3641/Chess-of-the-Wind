using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Clock : MonoBehaviour
{
    public float speed = 40f;

    public bool move = false;

    public PhotonView PV;


    public void Awake()
    {
        transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    [Button]
    public void StartClock()
    {
        move = true;
    }

    [Button]
    public void StopClock()
    {
        move = false;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
