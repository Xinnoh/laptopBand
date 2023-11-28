using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab; // Assign the prefab in the inspector
    private List<string> dataLines = new List<string>{
        "48,192,252,1,0,0:0:0:0:",
        "136,192,735,1,0,0:0:0:0:",
        "240,192,1219,1,0,0:0:0:0:",
        "336,128,1703,1,0,0:0:0:0:"
    };

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        foreach (string line in dataLines)
        {
            var parts = line.Split(',');
            float xPosition = MapPosition(float.Parse(parts[0]));
            float spawnTime = float.Parse(parts[2]) / 1000.0f; // Convert milliseconds to seconds

            yield return new WaitForSeconds(spawnTime);
            Instantiate(prefab, new Vector3(xPosition, 6, 0), Quaternion.identity);
        }
    }

    float MapPosition(float originalPosition)
    {
        return (originalPosition / 512.0f) * 15.0f - 7.5f;
    }
}
