using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpetControl : MonoBehaviour
{

    // This script sets the position of the trumpet's position based on the arduino.

    [Range(0.0f, 256.0f)]
    public float arduinoVal;
    public float arduinoMax = 900;

    public float xOffset;
    public float yOffset;

    public float targetMax = 5f; // Maximum range for movement

    void Start()
    {
        // Set the initial position with the offset
        transform.position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
    }

    void Update()
    {
        // Update only the x position based on the Arduino value
        float xPosition = MapPosition(arduinoVal, targetMax);
        transform.position = new Vector3(xPosition + xOffset, transform.position.y, transform.position.z);
    }

    // Map the Arduino value to the target range
    float MapPosition(float originalPosition, float targetMax)
    {
        const float originalMin = 0f;
        float targetMin = -targetMax;

        return (originalPosition - originalMin) / (arduinoMax - originalMin) * (targetMax - targetMin) + targetMin;
    }


    // Draw Gizmos in the Editor to show the range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-targetMax + xOffset, transform.position.y + yOffset, transform.position.z),
                        new Vector3(targetMax + xOffset, transform.position.y + yOffset, transform.position.z));
    }
}
