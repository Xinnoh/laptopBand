using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNote : MonoBehaviour
{
    public GameObject childSprite;  // Reference to the child GameObject with the sprite
    public ParticleSystem noteParticleSystem; // Reference to the particle system

    public float yRotation;

    public float initialSpeed; // Initial speed of the note
    public float maxSpeed;     // Maximum speed of the note
    public Vector3 initialScale = new Vector3(0.5f, 0.5f, 0.5f); // Initial scale of the child sprite
    public Vector3 finalScale = new Vector3(1f, 1f, 1f);         // Final scale of the child sprite
    public bool played = false; // Flag to indicate if the note is played

    public Vector3 startPosition;
    public Vector3 endPosition;
    public float totalTimeToTravel;

    private float elapsedTime = 0f;

    private void Start()
    {
        transform.localScale = initialScale;

        // Rotate the child sprite on the Y-axis based on the horizontal position
        if (childSprite != null)
        {
            childSprite.transform.localRotation = Quaternion.Euler(45f, yRotation, 0f);
        }
    }

    void Update()
    {
        if (!played)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / totalTimeToTravel;

            // Use an easing function for a smooth start and acceleration
            float lerpFactor = EaseInCubic(progress);

            // Adjust the lerpFactor based on initial and max speeds
            float speedFactor = Mathf.Lerp(initialSpeed, maxSpeed, lerpFactor) / maxSpeed;
            float adjustedLerpFactor = progress * speedFactor;

            // Interpolate the position of the note
            transform.position = Vector3.Lerp(startPosition, endPosition, adjustedLerpFactor);

            // Interpolate the scale of the parent object
            Vector3 newScale = Vector3.Lerp(initialScale, finalScale, adjustedLerpFactor);
            transform.localScale = newScale;
        }
    }

    public void PlayNote()
    {
        if (!played)
        {
            // Play the audio if available
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Disable the collider if available
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Disable the sprite renderer of the child object instead of the parent renderer
            if (childSprite != null)
            {
                SpriteRenderer spriteRenderer = childSprite.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false;
                }
            }

            if (noteParticleSystem != null)
            {
                noteParticleSystem.Play();
            }


            // Mark as played and stop movement
            played = true;

            // Schedule the object for deletion after 3 seconds
            Destroy(gameObject, 3f);
        }
    }

    // Cubic easing function: starts slow and accelerates
    float EaseInCubic(float x)
    {
        return x * x * x;
    }
}
