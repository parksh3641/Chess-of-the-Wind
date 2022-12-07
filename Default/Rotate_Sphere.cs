using UnityEngine;

public class Rotate_Sphere : MonoBehaviour
{
    public GameObject Planet;
    public float speed;
    public float minusSpeed = 0.1f;

    private void Update()
    {
        OrbitAround();
    }

    public void SetSpeed(float value)
    {
        speed = 200 + (400 * value);
    }    

    void OrbitAround()
    {
        if(speed > 0)
        {
            speed -= minusSpeed;
        }
        else
        {
            speed = 0;
        }

        transform.RotateAround(Planet.transform.position, Vector3.back, speed * Time.deltaTime);
    }
    // RotateAround(Vector3 point, Vector3 axis, float angle)
}