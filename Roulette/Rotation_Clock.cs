using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Clock : MonoBehaviour
{
    public float speed = 40f;

    public PhotonView PV;

    WaitForSeconds waitForSecond = new WaitForSeconds(0.01f);

    Vector3 vector3 = new Vector3(0, 1, 0);

    public void Awake()
    {
        transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    [Button]
    public void StartClock()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCoroution());
    }

    [Button]
    public void StopClock()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveCoroution()
    {
        transform.Rotate(vector3 * speed * Time.deltaTime);
        yield return waitForSecond;
        StartCoroutine(MoveCoroution());
    }
}
