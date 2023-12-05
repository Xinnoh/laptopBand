using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public int gameState = 0;
    public int songDiff = 1;    // which difficulty was selected

    public ObjectSpawner piano, drum, trumpet;
    public AudioSource instSource, pianoSound, drumSound, trumpetSound;
    public TextMeshProUGUI songTitle;

    private float pianoDelay, drumDelay, trumpetDelay;
    private float songBPM, songDelay, songOffset;
    private float travelDistance = 6.17f;
    private float maxDelay;

    private string audioFileName;
    private string pianoFile, drumFile, trumpetFile, instFile;
    private FileInfo[] files = null;
    private List<string> dataLines = new List<string> { };
    private bool ending;
    public GameObject startButton;

    public MoveUp cover;

    private void Start()
    {
        resetValues();
        songDiff = GameData.difficulty;

        piano.noteSpeed = GameData.pSpeed;
        drum.noteSpeed = GameData.dSpeed;
        trumpet.noteSpeed = GameData.tSpeed;

        // Load all wav files
        files = GetResourceFiles("*.wav", "*.ogg");

        string[] filePrefixes = { "tut", "easy", "normal", "hard" };
        string[] fileTypes = { "Piano", "Drums", "Sax", "Inst" };

        if (songDiff >= 0 && songDiff < filePrefixes.Length)
        {
            string prefix = filePrefixes[songDiff];
            LoadAndPlay(prefix + fileTypes[0], pianoSound);
            LoadAndPlay(prefix + fileTypes[1], drumSound);
            LoadAndPlay(prefix + fileTypes[2], trumpetSound);
            LoadAndPlay(prefix + fileTypes[3], instSource);

        }
        else
        {
            Debug.Log("no song loaded: " + songDiff);
        }

        pianoDelay = CalculateTimeToTravel(travelDistance, piano.noteSpeed);
        drumDelay = CalculateTimeToTravel(travelDistance, drum.noteSpeed);
        trumpetDelay = CalculateTimeToTravel(travelDistance, trumpet.noteSpeed);

        maxDelay = Mathf.Max(pianoDelay, drumDelay, trumpetDelay);
        
        pianoDelay = maxDelay - pianoDelay;
        drumDelay = maxDelay - drumDelay;
        trumpetDelay = maxDelay - trumpetDelay;
        songDelay = maxDelay + songOffset;

        switch(songDiff)
        {
            case 0:
                songTitle.text = "Tutorial";
                return;
            case 1:
                songTitle.text = "Easy - Underfell Sans";
                return;
            case 2:
                songTitle.text = "Normal - Enemy Approaching";
                return;
            case 3:
                songTitle.text = "Prove Your Worth";
                return;
        }
    }

    private void Update()
    {
        if(piano.complete && drum.complete && trumpet.complete && !ending) 
        {
            StartCoroutine(Complete(maxDelay));
            ending = true;
        }
    }

    IEnumerator Complete(float delay)
    {
        yield return new WaitForSeconds(delay + 2f - 0.3f);
        cover.moveUp = true;
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(3);
    }

    void LoadAndPlay(string fileName, AudioSource audioSource)
    {
        string filename = Path.GetFileNameWithoutExtension(fileName);
        AudioClip clip = Resources.Load<AudioClip>(filename);

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
        StartCoroutine(StartGame(1f)); 
    }

    IEnumerator StartGame(float delay)
    {
        Destroy(startButton);
        yield return new WaitForSeconds(delay);

        piano.startSpawning(pianoDelay);
        drum.startSpawning(drumDelay);
        trumpet.startSpawning(trumpetDelay);

        StartCoroutine(StartSong(songDelay));
    }


    IEnumerator StartSong(float delay)
    {
        yield return new WaitForSeconds(delay);

        instSource.UnPause();
        pianoSound.UnPause();
        drumSound.UnPause();
        trumpetSound.UnPause();
    }
    private FileInfo[] GetResourceFiles(string searchPatternWav, string searchPatternOgg)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "\\Resources");
        FileInfo[] wavFiles = dirInfo.GetFiles(searchPatternWav);
        FileInfo[] oggFiles = dirInfo.GetFiles(searchPatternOgg);

        // Combine wav and ogg files into a single array
        FileInfo[] allFiles = wavFiles.Concat(oggFiles).ToArray();
        return allFiles;
    }

    private void resetValues()
    {
        ending = false;
    }

}
