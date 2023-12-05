using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Getscore : MonoBehaviour
{

    public TextMeshProUGUI pScore, pPerf, pGreat, pMiss;
    public TextMeshProUGUI dScore, dPerf, dGreat, dMiss;
    public TextMeshProUGUI tScore, tPerf, tGreat, tMiss;
    public TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        // Set piano scores
        pScore.text = CalculatePercentage(GameData.pianoScore, GameData.pianoMax) + "%";
        pPerf.text = "Perfect - " + GameData.pianoPerfects;
        pGreat.text = "Great - " + GameData.pianoGreats;
        pMiss.text = "Miss - " + GameData.pianoMisses;

        // Set drum scores
        dScore.text = CalculatePercentage(GameData.drumScore, GameData.drumMax) + "%";
        dPerf.text = "Perfect - " + GameData.drumPerfects;
        dGreat.text = "Great - " + GameData.drumGreats;
        dMiss.text = "Miss - " + GameData.drumMisses;

        // Set trumpet scores
        tScore.text = CalculatePercentage(GameData.trumpetScore, GameData.trumpetMax) + "%";
        tPerf.text = "Perfect - " + GameData.trumpetPerfects;
        tGreat.text = "Great - " + GameData.trumpetGreats;
        tMiss.text = "Miss - " + GameData.trumpetMisses;

        // Set overall score
        score.text = "Overall Score - " + CalculatePercentage(GameData.overallScore, GameData.overallMax) + "%";
    }

    private string CalculatePercentage(int score, int scoreMax)
    {
        if (scoreMax == 0) return "N/A"; // Prevent division by zero
        return ((float)score / scoreMax * 100).ToString("F2"); // Format to 2 decimal places
    }
}
