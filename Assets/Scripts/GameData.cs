using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{

    public static int difficulty = 1;
    public static float pSpeed = 4;
    public static float dSpeed = 6;
    public static float tSpeed = 8;


    public static int pianoScore, pianoMax, pianoPerfects, pianoGreats, pianoMisses;
    public static int drumScore, drumMax, drumPerfects, drumGreats, drumMisses;
    public static int trumpetScore, trumpetMax, trumpetPerfects, trumpetGreats, trumpetMisses;
    public static int overallScore, overallMax;

    public static List<string> PianoEasy = new List<string>
    {
        "0,192,252,5,0,0:0:0:0:,D8",
        "512,192,735,1,0,0:0:0:0:,C5",
        "256,192,1219,1,0,0:0:0:0:,G3",
        "376,192,1703,1,0,0:0:0:0:",
        "160,192,1945,1,0,0:0:0:0:",
        "328,192,2187,1,0,0:0:0:0:",
        "56,192,2429,1,0,0:0:0:0:",
        "448,192,2671,1,0,0:0:0:0:"
    };

    public static List<string> DrumsEasy = new List<string>
    {
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

    public static List<string> SaxEasy = new List<string>
    {
        "0,192,252,5,0,0:0:0:0:,D8",
        "512,192,735,1,0,0:0:0:0:,C5",
        "256,192,1219,1,0,0:0:0:0:,G3",
        "376,192,1703,1,0,0:0:0:0:",
        "160,192,1945,1,0,0:0:0:0:",
        "328,192,2187,1,0,0:0:0:0:",
        "56,192,2429,1,0,0:0:0:0:",
        "448,192,2671,1,0,0:0:0:0:"
    };



    public static List<string> PianoNormal = new List<string>
    {

    };
    public static List<string> DrumsNormal = new List<string>
    {

    };
    public static List<string> SaxNormal = new List<string>
    {

    };



    public static List<string> PianoHard = new List<string>
    {

    };
    public static List<string> DrumsHard = new List<string>
    {

    };
    public static List<string> SaxHard = new List<string>
    {

    };



    public static List<string> PianoTut = new List<string>
    {

    };
    public static List<string> DrumsTut = new List<string>
    {

    };
    public static List<string> SaxTut = new List<string>
    {

    };
}
