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

    private int pianoScore, pianoMax, pianoPerfects, pianoGreats, pianoMisses;
    private int drumScore, drumMax, drumPerfects, drumGreats, drumMisses;
    private int trumpetScore, trumpetMax, trumpetPerfects, trumpetGreats, trumpetMisses;
    private int overallScore, overallMax;

    public TextMeshProUGUI pianoScoreText; 
    public TextMeshProUGUI drumScoreText; 
    public TextMeshProUGUI trumpetScoreText;
    public TextMeshProUGUI overallScoreText;

    void Awake()
    {
        // No idea what this does but it doesn't work without it
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
                if (hitType == "Perfect") pianoPerfects++;
                else if (hitType == "Great") pianoGreats++;
                else if (hitType == "Miss") pianoMisses++;
                UpdateScoreDisplay(pianoScore, pianoMax, pianoScoreText);
                break;
            case 1:
                drumScore += points;
                drumMax += pointsForPerfect;
                if (hitType == "Perfect") drumPerfects++;
                else if (hitType == "Great") drumGreats++;
                else if (hitType == "Miss") drumMisses++;
                UpdateScoreDisplay(drumScore, drumMax, drumScoreText);
                break;
            case 2:
                trumpetScore += points;
                trumpetMax += pointsForPerfect;
                if (hitType == "Perfect") trumpetPerfects++;
                else if (hitType == "Great") trumpetGreats++;
                else if (hitType == "Miss") trumpetMisses++;
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


    private void FinalScore()
    {
        GameData.pianoScore = pianoScore;
        GameData.pianoMax = pianoMax;
        GameData.pianoPerfects = pianoPerfects;
        GameData.pianoGreats = pianoGreats;
        GameData.pianoMisses = pianoMisses;

        GameData.drumScore = drumScore;
        GameData.drumMax = drumMax;
        GameData.drumPerfects = drumPerfects;
        GameData.drumGreats = drumGreats;
        GameData.drumMisses = drumMisses;

        GameData.trumpetScore = trumpetScore;
        GameData.trumpetMax = trumpetMax;
        GameData.trumpetPerfects = trumpetPerfects;
        GameData.trumpetGreats = trumpetGreats;
        GameData.trumpetMisses = trumpetMisses;

        GameData.overallScore = overallScore;
        GameData.overallMax = overallMax;
    }

}

