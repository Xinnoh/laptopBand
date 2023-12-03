using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject note;
    public float spawnWidth = 5f;
    public float xOffset, yOffset;
    public float noteSpeed = 5f;

    public int gamemode;

    private FileInfo[] files = null;

    private List<string> dataLines = new List<string> { };

    void Start()
    {

        if(gamemode== 0)
        {
            dataLines = pianoLines;
        }
        else if(gamemode == 1)
        {
            dataLines = drumLines;
        }
        else if(gamemode == 2)
        {
            dataLines = trumpetLines;
        }

        // Load all wav files
        files = GetResourceFiles("*.wav");
    }


    public void startSpawning(float delay)
    {
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
            

            //Long notes ending
            if (objectType == 128)
            {
                string lastSection = parts.Last();
                string[] subParts = lastSection.Split(':');
                float endTime = float.Parse(subParts[0]);
                float holdLength = (endTime - spawnTime) / 1000f;


                // I need to calculate how far down the startPos of the holdend should be. The higher the holdLength or noteSpeed, the further the distance.
                float distance = holdLength * noteSpeed * .583f;
                Vector3 holdPos = new Vector3(xPosition + xOffset, yOffset - distance, 0);


                GameObject holdEnd = Instantiate(note, holdPos, Quaternion.identity);

                MoveNote holdScript = holdEnd.GetComponent<MoveNote>();
                holdScript.speed = noteSpeed;
                holdScript.holdEnd = true;
            }
        }
    }



    float MapPosition(float originalPosition, float targetMax)
    {
        const float originalMin = 0f;
        const float originalMax = 512f;
        float targetMin = -targetMax;

        return (originalPosition - originalMin) / (originalMax - originalMin) * (targetMax - targetMin) + targetMin;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; // Set the color of the Gizmos

        if (gamemode == 0)
        {

            Vector3 size = new Vector3(0.45f, 0.16f, 0.43f);

            foreach (string line in pianoLines)
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

            foreach (string line in drumLines)
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

            foreach (string line in trumpetLines)
            {
                var parts = line.Split(',');
                float xPosition = MapPosition(float.Parse(parts[0]), spawnWidth);
                Vector3 startPos = new Vector3(xPosition + xOffset, yOffset, 0);
                Gizmos.DrawCube(startPos, size);
            }
        }
    }




    private List<string> pianoLines = new List<string>{
        "0,192,252,5,0,0:0:0:0:,D8",
        "512,192,735,1,0,0:0:0:0:,C5",
        "256,192,1219,1,0,0:0:0:0:,G3",
        "376,192,1703,1,0,0:0:0:0:",
        "160,192,1945,1,0,0:0:0:0:",
        "328,192,2187,1,0,0:0:0:0:",
        "56,192,2429,1,0,0:0:0:0:",
        "448,192,2671,1,0,0:0:0:0:"
    };


    private List<string> drumLines = new List<string>{
        "100,192,600,128,0,1427:0:0:0:0:",
        "448,192,600,1,0,0:0:0:0:",
        "192,192,600,1,0,0:0:0:0:",
        "320,192,1427,1,0,0:0:0:0:",
        "192,192,2668,1,0,0:0:0:0:",
        "320,192,3082,1,0,0:0:0:0:",
        "192,192,3496,1,0,0:0:0:0:",
        "192,192,3910,1,0,0:0:0:0:",
        "320,192,4737,1,0,0:0:0:0:",
        "192,192,5565,1,0,0:0:0:0:",
        "320,192,6393,1,0,0:0:0:0:",
        "320,192,6806,1,0,0:0:0:0:",
        "192,192,7220,1,0,0:0:0:0:",
        "448,192,7220,1,0,0:0:0:0:",
        "320,192,8048,1,0,0:0:0:0:"
    };


    private List<string> trumpetLines = new List<string>{
        "0,192,252,5,0,0:0:0:0:,D8",
        "512,192,735,1,0,0:0:0:0:,C5",
        "256,192,1219,1,0,0:0:0:0:,G3",
        "376,192,1703,1,0,0:0:0:0:",
        "160,192,1945,1,0,0:0:0:0:",
        "328,192,2187,1,0,0:0:0:0:",
        "56,192,2429,1,0,0:0:0:0:",
        "448,192,2671,1,0,0:0:0:0:"
    };
}
