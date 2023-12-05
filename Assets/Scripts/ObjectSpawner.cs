using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.Examples.TMP_ExampleScript_01;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject note;
    public GameObject rectanglePrefab;
    public float spawnWidth = 5f;
    public float xOffset, yOffset;
    public float noteSpeed = 5f;
    private float timer;

    public bool spawning, ending, complete;
    public int gamemode, difficulty;

    private int currentDataLineIndex = 0;

    private float lastSpawnTime, spawnTime, xPosition, objectType, endTime;
    private string lastSection;
    private string[] subParts;
    private Vector3 nextStartPos;

    private FileInfo[] files = null;

    private List<string> dataLines = new List<string> { };

    // data for next object


    void Start()
    {
        difficulty = GameData.difficulty;
        ResetVariables();
        SetNoteSpeed();
        loadObjects();
        GetObjectData();
    }

    private void Update()
    {
        if (spawning)
        {
            timer += Time.deltaTime;
            if (timer >= spawnTime)
            {
                SpawnObject();
                GetObjectData();
            }
        }
    }

    void GetObjectData()
    {
        if (currentDataLineIndex < dataLines.Count)
        {
            string line = dataLines[currentDataLineIndex];
            var parts = line.Split(',');
            spawnTime = float.Parse(parts[2]) / 1000.0f;
            xPosition = MapPosition(float.Parse(parts[0]), spawnWidth);
            nextStartPos = new Vector3(xPosition + xOffset, yOffset, 0);
            objectType = float.Parse(parts[3]);     // if 1: note, if 128: hold note
            lastSection = parts.Last();
            subParts = lastSection.Split(':');
            endTime = float.Parse(subParts[0]) / 1000f;

            currentDataLineIndex++;
        }
        else
        {
            spawning = false;
            ending = true;
        }
    }

    void SpawnObject()
    {
        float upwardAdjustment = CalculateUpwardAdjustment(spawnTime, timer, noteSpeed);

        Vector3 adjustedPosition = new Vector3(nextStartPos.x, nextStartPos.y + upwardAdjustment, nextStartPos.z);

        GameObject newNote = Instantiate(note, adjustedPosition, Quaternion.identity);
        MoveNote moveNoteScript = newNote.GetComponent<MoveNote>();
        moveNoteScript.speed = noteSpeed;

        if (objectType == 128){
            SpawnHold(adjustedPosition);
        }
        if(ending){
            complete = true;
        }
    }

    // Because unity runs at 60fps, it won't get the spawn position perfect. This fixes it
    float CalculateUpwardAdjustment(float spawnTime, float timer, float noteSpeed)
    {
        float frameDuration = 1f / 60f;
        float maxMovementPerFrame = noteSpeed * frameDuration;

        float timeDifference = timer - spawnTime;

        if (timeDifference <= 0)
        {
            return 0f;
        }

        return Mathf.Min(timeDifference / frameDuration, 1) * maxMovementPerFrame;
    }

    void SpawnHold(Vector3 adjustedPosition)
    {
        float holdLength = (endTime - spawnTime);

        float distance = holdLength * noteSpeed;
        Vector3 holdPos = new Vector3(xPosition + xOffset, yOffset - distance, 0);


        GameObject holdEnd = Instantiate(note, holdPos, Quaternion.identity);

        MoveNote holdScript = holdEnd.GetComponent<MoveNote>();
        holdScript.speed = noteSpeed;
        holdScript.holdEnd = true;

        GameObject rectangle = Instantiate(rectanglePrefab, transform);
        MoveSliderBody rectScript = rectangle.GetComponent<MoveSliderBody>();
        rectScript.speed = noteSpeed;

        PositionRectangleBetweenPoints(adjustedPosition, holdPos, rectangle);
    }


    public IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawning = true;
    }


    public void startSpawning(float delay)
    {
        spawning = true;
        StartCoroutine(StartGame(delay));
    }

    float MapPosition(float originalPosition, float targetMax)
    {
        // Maps relative value. Eg 512 becomes 1, 256 becomes 0.5
        const float originalMin = 0f;
        const float originalMax = 512f;
        float targetMin = -targetMax;

        return (originalPosition - originalMin) / (originalMax - originalMin) * (targetMax - targetMin) + targetMin;
    }

    // Hold note body
    void PositionRectangleBetweenPoints(Vector3 startTransform, Vector3 endPos, GameObject rectangle)
    {
        Vector3 centerPos = (startTransform + endPos) / 2f;
        rectangle.transform.position = centerPos;


        float scaleX = Vector3.Distance(startTransform, endPos) / rectangle.transform.localScale.y;
        rectangle.transform.localScale = new Vector3(scaleX, rectangle.transform.localScale.y * .8f, rectangle.transform.localScale.z);

        float angle = Mathf.Atan2(endPos.y - startTransform.y, endPos.x - startTransform.x) * Mathf.Rad2Deg;
        rectangle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }


    void SetNoteSpeed()
    {
        switch (gamemode)
        {
            case 0:
                noteSpeed = GameData.pSpeed;
                break;
            case 1:
                noteSpeed = GameData.dSpeed;
                break;
            case 2:
                noteSpeed = GameData.tSpeed;
                break;
            default:
                Debug.LogError("Invalid gamemode");
                break;
        }
    }


    void ResetVariables()
    {
        timer = 0;
        complete = false;
        ending = false;
        spawning = false;
    }





    // Audio file loading
    private FileInfo[] GetResourceFiles(string searchPattern)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "\\Resources");
        FileInfo[] files = dirInfo.GetFiles(searchPattern);
        return files;
    }
    void loadObjects()
    {

        string mode = "";
        switch (gamemode)
        {
            case 0: mode = "Piano"; break;
            case 1: mode = "Drums"; break;
            case 2: mode = "Sax"; break;
            default: Debug.LogError("Invalid game mode: " + gamemode); return;
        }

        string difficultyLevel = "";
        switch (difficulty)
        {
            case 0: difficultyLevel = "Tut"; break;
            case 1: difficultyLevel = "Easy"; break;
            case 2: difficultyLevel = "Normal"; break;
            case 3: difficultyLevel = "Hard"; break;
            default: Debug.LogError("Invalid difficulty: " + difficulty); return;
        }

        string key = mode + difficultyLevel;
        switch (key)
        {
            case "PianoTut": dataLines = GameData.PianoTut; break;
            case "PianoEasy": dataLines = GameData.PianoEasy; break;
            case "PianoNormal": dataLines = GameData.PianoNormal; break;
            case "PianoHard": dataLines = GameData.PianoHard; break;

            case "DrumsTut": dataLines = GameData.DrumsTut; break;
            case "DrumsEasy": dataLines = GameData.DrumsEasy; break;
            case "DrumsNormal": dataLines = GameData.DrumsNormal; break;
            case "DrumsHard": dataLines = GameData.DrumsHard; break;

            case "SaxTut": dataLines = GameData.SaxTut; break;
            case "SaxEasy": dataLines = GameData.SaxEasy; break;
            case "SaxNormal": dataLines = GameData.SaxNormal; break;
            case "SaxHard": dataLines = GameData.SaxHard; break;
            default: Debug.LogError("No data found for combination: " + key); return;
        }

        // Load all wav files
        files = GetResourceFiles("*.wav");
    }













    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; // Set the color of the Gizmos

        if (gamemode == 0)
        {

            Vector3 size = new Vector3(0.45f, 0.16f, 0.43f);

            foreach (string line in dataLines)
            {
                var parts = line.Split(',');
                float xPosition = MapPosition(float.Parse(parts[0]), spawnWidth);
                Vector3 startPos = new Vector3(xPosition + xOffset, yOffset, 0);
                Gizmos.DrawCube(startPos, size);

            }
        }

        else if(gamemode == 1)
        {
            Gizmos.color = Color.red;
            Vector3 size = new Vector3(0.45f, 0.16f, 0.43f);

            foreach (string line in dataLines)
            {
                var parts = line.Split(',');
                float xPosition = MapPosition(float.Parse(parts[0]), spawnWidth);
                Vector3 startPos = new Vector3(xPosition + xOffset, yOffset, 0);
                Gizmos.DrawCube(startPos, size);
            }
        }
        else if (gamemode == 2)
        {

            Gizmos.color = Color.blue;
            Vector3 size = new Vector3(0.45f, 0.16f, 0.43f);

            foreach (string line in dataLines)
            {
                var parts = line.Split(',');
                float xPosition = MapPosition(float.Parse(parts[0]), spawnWidth);
                Vector3 startPos = new Vector3(xPosition + xOffset, yOffset, 0);
                Gizmos.DrawCube(startPos, size);
            }
        }
    }
}
