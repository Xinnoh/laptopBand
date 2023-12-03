using UnityEngine;

public class EndCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            MoveNote note = collision.gameObject.GetComponent<MoveNote>();
            if (note != null)
            {
                note.played = true;
                collision.gameObject.GetComponent<Renderer>().enabled = false; // Disable the renderer of the note
            }
        }
    }
}
