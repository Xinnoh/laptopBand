using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSpawner : MonoBehaviour
{
    public float pSpeed, dSpeed, tSpeed;
    public int difficulty;
    public float spawnInterval;

    public Vector2 spawnPosition;
    public float spawnWidth = 1f;
    public GameObject pianoPrefab, drumPrefab, thirdPrefab;
    public TextMeshProUGUI pText, dText, tText;

    private float timer;


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            // Reset timer
            timer = 0;

            // Spawn prefabs
            SpawnPrefab(pianoPrefab, pSpeed, -spawnWidth, 0);
            SpawnPrefab(drumPrefab, dSpeed, 0, 1);
            SpawnPrefab(pianoPrefab, tSpeed, spawnWidth, 2);   //trumpet
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DrawSpawnPoint(0); // Center
        Gizmos.color = Color.green;
        DrawSpawnPoint(spawnWidth); // Right
        Gizmos.color = Color.blue;
        DrawSpawnPoint(-spawnWidth); // Left
    }

    void DrawSpawnPoint(float xOffset)
    {
        Vector3 position = new Vector3(spawnPosition.x + xOffset, spawnPosition.y, 0);
        Vector3 size = new Vector3(0.45f, 0.16f, 0.43f);

        Gizmos.DrawCube(position, size); 
    }

    public void AdjustSpeed(ref float speed, float amount)
    {
        speed = Mathf.Clamp(speed + amount, 1, 10);
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

}
