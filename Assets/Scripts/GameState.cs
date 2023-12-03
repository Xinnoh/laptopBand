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

    private AudioSource audioSource;

    private FileInfo[] files = null;
    private List<string> dataLines = new List<string> { };

    private string audioFileName;

    void Start()
    {
    }
    private FileInfo[] GetResourceFiles(string searchPattern)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "\\Songs");
        FileInfo[] files = dirInfo.GetFiles(searchPattern);
        return files;
    }

    private void Awake()
    {

        // Load all wav files
        files = GetResourceFiles("*.wav");


        if (songDiff == 0)
        {

        }

        else if(songDiff == 1)
        {
            audioFileName = "easy_underfellSans";
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
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
