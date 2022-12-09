using UnityEngine;

public class Rotate_Sphere : MonoBehaviour
{
    public GameObject target;
    public Transform mainParent;
    public Transform rouletteParent;
    public float speed = 100;
    public float minusSpeed = 0.1f;

    public float magnetSpeed = 0.1f;

    public bool magnet = false;


    private void FixedUpdate()
    {
        OrbitAround();

        Magnet();
    }
    public void StartSphere()
    {
        transform.parent = mainParent.transform;

        transform.localPosition = new Vector3(1.03f, 0.6f, 4);

        magnet = false;

        speed = 300;

        minusSpeed = 0.3f;
    }

    public void AddSpeed(float value)
    {
        speed += (300 * value);

        minusSpeed = 0.3f;
    }

    public void StopSphere()
    {
        transform.parent = rouletteParent.transform;
    }

    void OrbitAround()
    {
        if (speed > 0)
        {
            if(speed > 100)
            {
                speed -= (minusSpeed * 3);
            }
            else
            {
                speed -= minusSpeed;
            }

        }
        else
        {
            speed = 0;
        }

        transform.RotateAround(target.transform.position, Vector3.forward, speed * Time.deltaTime);
    }

    void Magnet()
    {
        if (magnet == true && speed > 0)
        {
            Vector2 relativePos = target.transform.position - transform.position;
            float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
            transform.Translate(transform.up * magnetSpeed * Time.deltaTime, Space.World);
        }
    }
}