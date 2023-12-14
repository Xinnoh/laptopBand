using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRandomColour : MonoBehaviour
{
    ParticleSystem particleSystem;
    // This generates a random bright color for particle systems, not sure what it's for

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem == null)
        {
            Debug.LogError("Particle System component not found on the object.");
            return;
        }

        var mainModule = particleSystem.main;
        mainModule.startColor = GenerateRandomBrightColor();
    }

    Color GenerateRandomBrightColor()
    {
        // Ensure at least one of the RGB components is at maximum (1) to guarantee brightness
        float red = Random.Range(0.5f, 1f);
        float green = Random.Range(0.5f, 1f);
        float blue = Random.Range(0.5f, 1f);

        return new Color(red, green, blue);
    }
}

