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

    public bool spawning, complete;
    public int gamemode, difficulty;

    private float lastSpawnTime;

    private FileInfo[] files = null;

    private List<string> dataLines = new List<string> { };


    void Start()
    {
        timer = 0;
        difficulty = GameData.difficulty;
        SetNoteSpeed();
        loadObjects();
        complete = false;
    }

    private void Update()
    {
        if (spawning)
        {
            timer += Time.deltaTime;
        }
    }


    public void startSpawning(float delay)
    {
        spawning = true;
        StartCoroutine(SpawnObjects(delay));
    }


    // Audio file loading
    private FileInfo[] GetResourceFiles(string searchPattern)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "\\Resources");
        FileInfo[] files = dirInfo.GetFiles(searchPattern);
        return files;
    }


    IEnumerator SpawnObjects(float delay)
    {
        yield return new WaitForSeconds(delay);

        float lastSpawnTime = 0.0f;

        foreach (string line in dataLines)
        {
            var parts = line.Split(',');
            // Convert 0 to 512 to a custom range
            float xPosition = MapPosition(float.Parse(parts[0]), spawnWidth);
            Vector3 startPos = new Vector3(xPosition + xOffset, yOffset, 0);

            //Find time difference between current and next note

            float spawnTime = float.Parse(parts[2]) / 1000.0f;

            Debug.Log("Diff: "+ (timer - spawnTime));

            // Check if the coroutine is behind schedule
            if (timer > spawnTime)
            {
                // Calculate how much the coroutine is behind
                float delayAdjustment = timer - spawnTime;

                // Adjust the wait time to try and catch up
                float adjustedWaitTime = spawnTime - lastSpawnTime - delayAdjustment;

                // Ensure the wait time is not negative
                adjustedWaitTime = Mathf.Max(0, adjustedWaitTime);

                yield return new WaitForSeconds(adjustedWaitTime);
            }
            else
            {
                yield return new WaitForSeconds(spawnTime - lastSpawnTime);
            }

            lastSpawnTime = spawnTime;

            if (spawnTime > lastSpawnTime)
            {
                yield return new WaitForSeconds(spawnTime - lastSpawnTime);
                lastSpawnTime = spawnTime;
            }

            float objectType = float.Parse(parts[3]);

            //Normal Notes

            GameObject newNote = Instantiate(note, startPos, Quaternion.identity);
            MoveNote moveNoteScript = newNote.GetComponent<MoveNote>();
            moveNoteScript.speed = noteSpeed;

            if (gamemode == 2)
            {
                moveNoteScript.holdEnd = true;
            }

            //Long notes ending
            if (objectType == 128)
            {
                string lastSection = parts.Last();
                string[] subParts = lastSection.Split(':');
                float endTime = float.Parse(subParts[0]) / 1000f;
                float holdLength = (endTime - spawnTime);


                // I need to calculate how far down the startPos of the holdend should be. The higher the holdLength or noteSpeed, the further the distance.
                float distance = holdLength * noteSpeed;
                Vector3 holdPos = new Vector3(xPosition + xOffset, yOffset - distance, 0);


                GameObject holdEnd = Instantiate(note, holdPos, Quaternion.identity);

                MoveNote holdScript = holdEnd.GetComponent<MoveNote>();
                holdScript.speed = noteSpeed;
                holdScript.holdEnd = true;

                GameObject rectangle = Instantiate(rectanglePrefab, transform);
                MoveSliderBody rectScript = rectangle.GetComponent<MoveSliderBody>();
                rectScript.speed = noteSpeed;


                PositionRectangleBetweenPoints(startPos, holdPos, rectangle);
            }
        }
        complete = true;
        spawning = false;
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
