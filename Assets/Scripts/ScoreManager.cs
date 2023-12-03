using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMesh Pro namespace

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Configurable point values
    private int pointsForPerfect = 3;
    private int pointsForGreat = 1;
    private int pointsForMiss = 0;

    private int pianoScore, pianoMax;
    private int drumScore, drumMax;
    private int trumpetScore, trumpetMax;
    private int overallScore, overallMax;

    public TextMeshProUGUI pianoScoreText; 
    public TextMeshProUGUI drumScoreText; 
    public TextMeshProUGUI trumpetScoreText;
    public TextMeshProUGUI overallScoreText;



    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterHit(string hitType, int gameMode)
    {
        int points = hitType switch
        {
            "Perfect" => pointsForPerfect,
            "Great" => pointsForGreat,
            "Miss" => pointsForMiss,
            _ => 0
        };


        switch (gameMode)
        {
            case 0:
                pianoScore += points;
                pianoMax += pointsForPerfect;
                UpdateScoreDisplay(pianoScore, pianoMax, pianoScoreText);
                break;
            case 1:
                drumScore += points;
                drumMax += pointsForPerfect;
                UpdateScoreDisplay(drumScore, drumMax, drumScoreText);
                break;
            case 2:
                trumpetScore += points;
                trumpetMax += pointsForPerfect;
                UpdateScoreDisplay(trumpetScore, trumpetMax, trumpetScoreText);
                break;
        }

        UpdateOverallScore();

    }

    private void UpdateScoreDisplay(int totalScore, int totalPossiblePoints, TextMeshProUGUI scoreText)
    {
        float scorePercentage = (float)totalScore / totalPossiblePoints * 100f;
        scoreText.text = $"{scorePercentage:F2}%";
    }


    private void UpdateOverallScore()
    {
        overallScore = pianoScore + drumScore + trumpetScore;
        overallMax = pianoMax + drumMax + trumpetMax;
        float overallScorePercentage = (float)overallScore / overallMax * 100f;
        overallScoreText.text = $"{overallScorePercentage:F2}%";
    }

}

