using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KeyScript : MonoBehaviour
{
    

    public KeyCode toggleKey, toggleKey2; // Set your desired key here
    private SpriteRenderer spriteRenderer;
    public float detectWidth = 2f;
    public float detectHeight = 2f;
    public float offsetX, offsetY;
    public float perfectDetectHeight; // Smaller width for the 'Perfect' hit detection

    public int gameMode;

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
        if (Input.GetKeyDown(toggleKey) || Input.GetKeyDown(toggleKey2))
        {
            SetOpacity(1f); 
            CheckAndPlayNote();
        }

        if (Input.GetKey(toggleKey) || Input.GetKey(toggleKey2))
        {
            CheckAndPlayHeldNotes();
        }

        if (Input.GetKeyUp(toggleKey) || Input.GetKeyUp(toggleKey2))
        {
            SetOpacity(0.5f); 
        }

        if(gameMode == 2)
        {
            CollideNotes();
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

    void CollideNotes()
    {
        bool isPerfectHit = CheckHit(new Vector2(detectWidth, perfectDetectHeight), "Perfect");

        if (isPerfectHit)
        {
            Debug.Log("Perfect hit!");
            // Update the score for a perfect hit
            ScoreManager.Instance.RegisterHit("Perfect", gameMode);
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
            ScoreManager.Instance.RegisterHit(hitType, gameMode);
        }
    }

    private void CheckAndPlayHeldNotes()
    {
        Vector2 size = new Vector2(detectWidth, (detectHeight - perfectDetectHeight) / 2);
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(offsetX, offsetY);

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, size, 0);

        Collider2D closestNoteCollider = null;
        float minDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Note"))
            {
                float distance = Vector2.Distance(hitCollider.transform.position, boxCenter);
                if (distance < minDistance)
                {
                    closestNoteCollider = hitCollider;
                    minDistance = distance;
                }
            }
        }

        if (closestNoteCollider != null)
        {
            MoveNote moveNoteScript = closestNoteCollider.GetComponent<MoveNote>();
            if (moveNoteScript != null && !moveNoteScript.played && moveNoteScript.holdEnd)
            {
                moveNoteScript.PlayNote(true);
                Debug.Log("Perfect" + " hit!");
            }
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

        if(gameMode != 2)
        {

        // Top 'Great' early hit detection area
        DrawHitDetectionGizmo(new Vector2(detectWidth, (detectHeight - perfectDetectHeight) / 2), new Color(1, 1, 0, 0.5f), new Vector2(0, -(perfectDetectHeight + (detectHeight - perfectDetectHeight) / 2) / 2)); // Yellow, semi-transparent

        // Bottom 'Great' late hit detection area
        DrawHitDetectionGizmo(new Vector2(detectWidth, (detectHeight - perfectDetectHeight) / 2), new Color(1, 1, 0, 0.5f), new Vector2(0, (perfectDetectHeight + (detectHeight - perfectDetectHeight) / 2) / 2)); // Yellow, semi-transparent

        }
    }

    private void DrawHitDetectionGizmo(Vector2 size, Color color, Vector2 additionalOffset = default)
    {
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(offsetX, offsetY) + additionalOffset;
        Gizmos.color = color;
        Gizmos.DrawCube(boxCenter, size); // Draw solid, semi-transparent cube
    }



}
