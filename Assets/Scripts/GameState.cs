using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public int gameState = 0;

    public int songDiff = 1;


    public ObjectSpawner piano;
    public ObjectSpawner drum;
    public ObjectSpawner trumpet;

    private float pianoDelay, drumDelay, trumpetDelay, songDelay, songOffset;

    private AudioSource audioSource;

    private FileInfo[] files = null;
    private List<string> dataLines = new List<string> { };

    private string audioFileName;
    public float travelDistance = 6.17f; // Distance to travel

    private FileInfo[] GetResourceFiles(string searchPattern)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "\\Resources");
        FileInfo[] files = dirInfo.GetFiles(searchPattern);
        return files;
    }

    private void Start()
    {

        // Load all wav files
        files = GetResourceFiles("*.wav");


        if (songDiff == 0)
        {

        }

        else if(songDiff == 1)
        {
            audioFileName = "easy_underfellSans";
            songOffset = 0; //should be 0.615f for this song
        }
        
        else if(songDiff == 2)
        {

        }

        else if (songDiff == 3)
        {

        }
        else
        {
            Debug.Log("no song loaded: " + songDiff);
        }

        string filename = Path.GetFileNameWithoutExtension(audioFileName);
        AudioClip clip = Resources.Load<AudioClip>(filename); 
        Debug.Log("Attempting to load: " + filename);

        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.Pause();
        }
        else
        {
            Debug.LogError("Audio clip or AudioSource is null. Clip: " + (clip != null) + ", AudioSource: " + (audioSource != null));
        }

        pianoDelay = CalculateTimeToTravel(travelDistance, piano.noteSpeed);
        drumDelay = CalculateTimeToTravel(travelDistance, drum.noteSpeed);
        trumpetDelay = CalculateTimeToTravel(travelDistance, trumpet.noteSpeed);

        float maxDelay = Mathf.Max(pianoDelay, drumDelay, trumpetDelay);
        
        pianoDelay = maxDelay - pianoDelay;
        drumDelay = maxDelay - drumDelay;
        trumpetDelay = maxDelay - trumpetDelay;
        songDelay = maxDelay + songOffset;
    }

    private float CalculateTimeToTravel(float distance, float speed)
    {
        if (speed <= 0)
        {
            Debug.LogError("Must be positive speed");
            return 4;
        }
        return distance / speed;
    }


    public void startGame()
    {
        piano.startSpawning(pianoDelay);
        drum.startSpawning(drumDelay);
        trumpet.startSpawning(trumpetDelay);

        StartCoroutine(StartSong(songDelay));
    }


    IEnumerator StartSong(float delay)
    {
        Debug.Log("Enter");
        yield return new WaitForSeconds(delay);

        Debug.Log("Delay");

        audioSource.UnPause();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
