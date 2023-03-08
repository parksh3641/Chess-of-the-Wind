using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Clock : MonoBehaviour
{
    public float speed = 40f;

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

        PV = GetComponent<PhotonView>();
    }

    public void StartClock()
    {
        start = true;
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
