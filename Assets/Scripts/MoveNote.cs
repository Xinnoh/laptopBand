using UnityEngine;

public class MoveNote : MonoBehaviour
{
    public GameObject childSprite;  // Reference to the child GameObject with the sprite
    public ParticleSystem noteParticleSystem; // Reference to the particle system

    public float speed;  // Speed of the note
    public bool played = false; // Flag to indicate if the note is played


    private void Update()
    {

        if (!played)
        {
            // Move the note downwards at a consistent speed
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    public void PlayNote(bool isHit)
    {
        if (!played)
        {
            // If the note is hit, play the audio and particle effect
            if (isHit)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }

                if (noteParticleSystem != null)
                {
                    noteParticleSystem.Play();
                }
            }

            // Disable the collider
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Disable the sprite renderer of the child object
            if (childSprite != null)
            {
                SpriteRenderer spriteRenderer = childSprite.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false;
                }
            }

            // Mark as played and stop movement
            played = true;

            // Schedule the object for deletion after 3 seconds
            Destroy(gameObject, 1f);
        }
    }

}
