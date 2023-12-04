using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSpawner : MonoBehaviour
{
    //Variable
    public float pSpeed, dSpeed, tSpeed;
    public int difficulty;
    public TextMeshProUGUI pText, dText, tText;
    private float timer;

    //Unchanging
    public float spawnInterval, variationRange;
    public Vector2 spawnPosition;
    public float spawnWidth = 1f;
    public GameObject pianoPrefab, drumPrefab, thirdPrefab;


    private void Start()
    {
        ResetVariables();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            // Reset timer
            timer = 0;

            float pianoOffset = Random.Range(-variationRange, variationRange);
            float drumOffset = Random.Range(-variationRange, variationRange);
            float trumpetOffset = Random.Range(-variationRange, variationRange);

            SpawnPrefab(pianoPrefab, pSpeed, -spawnWidth + pianoOffset, 0);
            SpawnPrefab(drumPrefab, dSpeed, drumOffset, 1); 
            SpawnPrefab(pianoPrefab, tSpeed, spawnWidth + trumpetOffset, 2); 
        }
    }

    void SpawnPrefab(GameObject prefab, float speed, float xOffset, int gamemode)
    {
        Vector2 position = spawnPosition + new Vector2(xOffset, 0);
        GameObject newNote = Instantiate(prefab, position, Quaternion.identity);
        MoveNote moveNoteScript = newNote.GetComponent<MoveNote>();
        moveNoteScript.speed = speed;
        moveNoteScript.gamemode = gamemode;
    }


    public void AdjustSpeed(ref float speed, float amount)
    {
        speed = Mathf.Clamp(speed + amount, 1, 12);
        pText.text = $"{pSpeed:F1}";
        dText.text = $"{dSpeed:F1}";
        tText.text = $"{tSpeed:F1}";
        UpdateAllNoteSpeeds();
    }

    public void pLower() {AdjustSpeed(ref pSpeed, -0.5f); }
    public void pRaise() { AdjustSpeed(ref pSpeed, 0.5f); }
    public void dLower() { AdjustSpeed(ref dSpeed, -0.5f); }
    public void dRaise() { AdjustSpeed(ref dSpeed, 0.5f); }
    public void tLower() { AdjustSpeed(ref tSpeed, -0.5f); }
    public void tRaise() { AdjustSpeed(ref tSpeed, 0.5f); }

    public void UpdateAllNoteSpeeds()
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
        foreach (GameObject note in notes)
        {
            MoveNote moveNoteScript = note.GetComponent<MoveNote>();
            if (moveNoteScript != null)
            {
                // Update the speed based on the prefab type
                if (moveNoteScript.gamemode == 0)
                {
                    moveNoteScript.speed = pSpeed;
                }
                else if (moveNoteScript.gamemode == 1)
                {
                    moveNoteScript.speed = dSpeed;
                }
                else if (moveNoteScript.gamemode == 2)
                {
                    moveNoteScript.speed = tSpeed;
                }
            }
        }
    }

    public void changeDiff(int diff)
    {
        difficulty = diff;
    }

    public void startGame()
    {
        if(difficulty > 3 || difficulty < 0)
        {
            Debug.Log("No difficulty selected");
            return;
        }

        GameData.pSpeed = pSpeed;
        GameData.dSpeed = dSpeed;
        GameData.tSpeed = tSpeed;
        GameData.difficulty = difficulty;
        SceneManager.LoadScene(2);

    }

    void ResetVariables()
    {
        pSpeed = GameData.pSpeed;
        dSpeed = GameData.dSpeed;
        tSpeed = GameData.tSpeed;
        difficulty = GameData.difficulty;
        pText.text = $"{pSpeed:F1}";
        dText.text = $"{dSpeed:F1}";
        tText.text = $"{tSpeed:F1}";
        timer = 0f;
    }



    void OnDrawGizmosSelected()
    {
        // Drawing fixed spawn points
        DrawSpawnPoint(0, Color.red); // Center
        DrawSpawnPoint(spawnWidth, Color.green); // Right
        DrawSpawnPoint(-spawnWidth, Color.blue); // Left

        // Drawing variation ranges
        DrawVariationRange(0, Color.red); // Center variation
        DrawVariationRange(spawnWidth, Color.green); // Right variation
        DrawVariationRange(-spawnWidth, Color.blue); // Left variation
    }

    void DrawSpawnPoint(float xOffset, Color color)
    {
        Gizmos.color = color;
        Vector3 position = new Vector3(spawnPosition.x + xOffset, spawnPosition.y, 0);
        Vector3 size = new Vector3(0.45f, 0.16f, 0.43f);
        Gizmos.DrawCube(position, size);
    }

    void DrawVariationRange(float xOffset, Color color)
    {
        Gizmos.color = new Color(color.r, color.g, color.b, 0.4f); // Slightly transparent
        Vector3 leftBound = new Vector3(spawnPosition.x + xOffset - variationRange, spawnPosition.y, 0);
        Vector3 rightBound = new Vector3(spawnPosition.x + xOffset + variationRange, spawnPosition.y, 0);
        Vector3 size = new Vector3(0.1f, 0.3f, 0.43f); // Thin and tall to represent range

        Gizmos.DrawLine(leftBound, rightBound);
    }
}
