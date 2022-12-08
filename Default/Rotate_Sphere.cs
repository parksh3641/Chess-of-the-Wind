using UnityEngine;

public class Rotate_Sphere : MonoBehaviour
{
    public GameObject Planet;
    public float speed = 100;
    public float minusSpeed = 0.1f;


    private void FixedUpdate()
    {
        OrbitAround();
    }
    public void StartSphere()
    {
        speed = 150;

        minusSpeed = 0.1f;
    }

    public void AddSpeed(float value)
    {
        speed += (150 * value);

        minusSpeed = 0.2f;
    }

    void OrbitAround()
    {
        if (speed > 0)
        {
            speed -= minusSpeed;
        }
        else
        {
            speed = 0;
        }

        transform.RotateAround(Planet.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
    // RotateAround(Vector3 point, Vector3 axis, float angle)
}