using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    

    public KeyCode toggleKey = KeyCode.Space; // Set your desired key here
    private SpriteRenderer spriteRenderer;
    public float detectWidth = 2f;
    public float detectHeight = 2f;
    public float offsetX, offsetY;
    public float perfectDetectHeight; // Smaller width for the 'Perfect' hit detection

    public int keyType;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            SetOpacity(1f); 
            CheckAndPlayNote();
        }

        if (Input.GetKeyUp(toggleKey))
        {
            SetOpacity(0.5f); 
        }
    }

    void SetOpacity(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
    private void CheckAndPlayNote()
    {
        string hitType = "Miss"; // Default to "Miss" if no hit is detected

        // Central 'Perfect' hit detection area
        if (CheckHit(new Vector2(detectWidth, perfectDetectHeight), "Perfect"))
        {
            hitType = "Perfect";
        }
        else if (CheckHit(new Vector2(detectWidth, (detectHeight - perfectDetectHeight) / 2), "Great", new Vector2(0, -(perfectDetectHeight + (detectHeight - perfectDetectHeight) / 2) / 2)))
        {
            hitType = "Great";
        }
        else if (CheckHit(new Vector2(detectWidth, (detectHeight - perfectDetectHeight) / 2), "Great", new Vector2(0, (perfectDetectHeight + (detectHeight - perfectDetectHeight) / 2) / 2)))
        {
            hitType = "Great";
        }

        if(hitType != "Miss")
        {
            // Register the hit with the ScoreManager
            ScoreManager.Instance.RegisterHit(hitType, keyType);
        }
    }


    private bool CheckHit(Vector2 size, string hitType, Vector2 additionalOffset = default)
    {
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(offsetX, offsetY) + additionalOffset;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, size, 0);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Note"))
            {
                MoveNote moveNoteScript = hitCollider.gameObject.GetComponent<MoveNote>();
                if (moveNoteScript != null && !moveNoteScript.played)
                {
                    moveNoteScript.PlayNote(true);
                    Debug.Log(hitType + " hit!");
                    return true;
                }
            }
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        // Central 'Perfect' hit detection area
        DrawHitDetectionGizmo(new Vector2(detectWidth, perfectDetectHeight), new Color(0, 1, 0, 0.5f)); // Green, semi-transparent

        // Top 'Great' early hit detection area
        DrawHitDetectionGizmo(new Vector2(detectWidth, (detectHeight - perfectDetectHeight) / 2), new Color(1, 1, 0, 0.5f), new Vector2(0, -(perfectDetectHeight + (detectHeight - perfectDetectHeight) / 2) / 2)); // Yellow, semi-transparent

        // Bottom 'Great' late hit detection area
        DrawHitDetectionGizmo(new Vector2(detectWidth, (detectHeight - perfectDetectHeight) / 2), new Color(1, 1, 0, 0.5f), new Vector2(0, (perfectDetectHeight + (detectHeight - perfectDetectHeight) / 2) / 2)); // Yellow, semi-transparent
    }

    private void DrawHitDetectionGizmo(Vector2 size, Color color, Vector2 additionalOffset = default)
    {
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(offsetX, offsetY) + additionalOffset;
        Gizmos.color = color;
        Gizmos.DrawCube(boxCenter, size); // Draw solid, semi-transparent cube
    }



}
