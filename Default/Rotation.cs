using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 30f; // Adjust the rotation speed as needed
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Rotate the UI element to the right on the Z-axis
        rectTransform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}
