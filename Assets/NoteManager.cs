using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public void PlayNote(GameObject note)
    {
        // Check if the note has already been played
        MoveNote noteMover = note.GetComponent<MoveNote>();
        if (noteMover != null && noteMover.played == false)
        {
            // Play the sound
            AudioSource audioSource = note.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Hide the note and mark it as played
            note.GetComponent<Renderer>().enabled = false;
            noteMover.played = true;
        }
    }
}
