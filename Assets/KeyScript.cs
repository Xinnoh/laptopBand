using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    public KeyCode toggleKey = KeyCode.Space; // Set your desired key here
    private SpriteRenderer spriteRenderer;
    public NoteManager noteManager;
    public float detectionRadius = 2f;

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
            SetOpacity(1f); // Fully opaque
            CheckAndPlayNote();

        }

        if (Input.GetKeyUp(toggleKey))
        {
            SetOpacity(0.5f); // Semi-transparent, change as needed
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
        // Define the size of the box (2 units width and 4 units height)
        Vector2 size = new Vector2(2f, 4f);
        // Center of the box at the object's position
        Vector2 boxCenter = (Vector2)transform.position;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, size, 0);

        bool noteFound = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Note"))
            {
                MoveNote moveNoteScript = hitCollider.gameObject.GetComponent<MoveNote>();
                if (moveNoteScript != null && !moveNoteScript.played)
                {
                    noteManager.PlayNote(hitCollider.gameObject);
                    noteFound = true;
                    break; // Play only the first unplayed note encountered
                }
            }
        }

        if (!noteFound)
        {
            Debug.Log("No unplayed notes found within box");
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Draw a gizmo in the editor to show the detection area
        Vector2 size = new Vector2(2f, 4f);
        Vector2 boxCenter = (Vector2)transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCenter, size);
    }


}
