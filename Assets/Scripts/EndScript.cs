using UnityEngine;

public class EndCollisionHandler : MonoBehaviour
{
    public int gamemode;
    public Vector2 detectionSize = new Vector2(1f, 1f); // Size of the detection area
    public Vector2 offset; // Offset for the detection area

    private void Update()
    {
        if (CheckForMiss(detectionSize))
        {
            Debug.Log("Miss");
            // ScoreManager.Instance.RegisterHit("Miss", gamemode); // Uncomment this when ready
        }
    }

    private bool CheckForMiss(Vector2 size)
    {
        Vector2 boxCenter = (Vector2)transform.position + offset;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, size, 0);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Note"))
            {
                MoveNote moveNoteScript = hitCollider.gameObject.GetComponent<MoveNote>();
                if (moveNoteScript != null && !moveNoteScript.played)
                {
                    moveNoteScript.PlayNote(false); 
                    ScoreManager.Instance.RegisterHit("Miss", gamemode);

                    return true;
                }
            }
        }
        return false;
    }

    // Optional: Draw Gizmos to visualize the detection area
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)transform.position + offset;
        Gizmos.DrawWireCube(boxCenter, detectionSize);
    }
}
