using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab; // Assign the prefab in the inspector
    public float startRange = 5f;
    public float endRange = 15f;
    public float scrollDistance = 6f;
    public float startPosition = 2.5f;
    public float noteSpeed = 5f;
    public float initialSpeed = 3f;

    private List<string> dataLines = new List<string>{
        "48,192,252,5,0,0:0:0:0:",
        "136,192,735,1,0,0:0:0:0:",
        "240,192,1219,1,0,0:0:0:0:",
        "376,192,1703,1,0,0:0:0:0:",
        "160,192,1945,1,0,0:0:0:0:",
        "328,192,2187,1,0,0:0:0:0:",
        "56,192,2429,1,0,0:0:0:0:",
        "448,192,2671,1,0,0:0:0:0:"
    };

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        float lastSpawnTime = 0.0f;

        foreach (string line in dataLines)
        {
            var parts = line.Split(',');
            float xPosition = MapPosition(float.Parse(parts[0]), startRange);
            float spawnTime = float.Parse(parts[2]) / 1000.0f; // Convert milliseconds to seconds

            if (spawnTime > lastSpawnTime)
            {
                yield return new WaitForSeconds(spawnTime - lastSpawnTime);
                lastSpawnTime = spawnTime;
            }


            Vector3 startPos = new Vector3(xPosition, startPosition, 0);
            Vector3 endPosition = new Vector3(MapPosition(float.Parse(parts[0]), endRange / 2), -scrollDistance, 0);

            // Calculate horizontal speed
            float timeToTravelScrollDistance = scrollDistance / noteSpeed;
            float endXPosition = MapPosition(float.Parse(parts[0]), endRange);
            float horSpeed = (endXPosition - xPosition) / timeToTravelScrollDistance;

            GameObject newNote = Instantiate(prefab, startPos, Quaternion.identity);
            MoveNote moveNoteScript = newNote.GetComponent<MoveNote>();

            moveNoteScript.startPosition = startPos;
            moveNoteScript.endPosition = endPosition;
            moveNoteScript.totalTimeToTravel = scrollDistance / noteSpeed;
            moveNoteScript.initialSpeed = initialSpeed;
            moveNoteScript.maxSpeed = noteSpeed;

            moveNoteScript.yRotation = MapPosition(float.Parse(parts[0]), -33f);

        }
    }


    float MapPosition(float originalPosition, float targetMax)
    {
        /* Takes the osu note position (0-512) and maps it to a value between X and -X
            If targetMax = 5, 512 = 5 and 0 = -5
        */

        const float originalMin = 0f;
        const float originalMax = 512f;
        float targetMin = -targetMax;

        return (originalPosition - originalMin) / (originalMax - originalMin) * (targetMax - targetMin) + targetMin;
    }

}
