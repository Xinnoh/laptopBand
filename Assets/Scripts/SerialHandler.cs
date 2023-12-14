using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
public class SerialHandler : MonoBehaviour
{
    // This script gets data from the arduino's serial and converts it to usable information


    SerialPort streamCOM7 = new SerialPort("COM7", 9600);
    SerialPort streamCOM10 = new SerialPort("COM10", 9600); // New instance for COM6

    public int button1, button2, button3, button4;
    private float trumpetVal;
    public TrumpetControl trumpetKey;
    public KeyScript drum1, drum2, drum3, drum4;

    void Start()
    {
        try
        {
            streamCOM7.Open();
            streamCOM7.ReadTimeout = 50;
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening COM7: " + e.Message);
        }

        try
        {
            streamCOM10.Open();
            streamCOM10.ReadTimeout = 50;
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening COM10: " + e.Message);
        }
    }

    void Update()
    {

        //Trumpet
        if (streamCOM7.IsOpen)
        {
            try
            {
                string serVal = streamCOM7.ReadLine();
                string[] sArray = serVal.Split(',');

                trumpetVal = float.Parse(sArray[0]);
                trumpetKey.arduinoVal = trumpetVal;
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                Debug.LogError("Error reading from COM7: " + e.Message);
            }
        }


        //Drums
        if (streamCOM10.IsOpen)
        {
            try
            {
                string buttonVal = streamCOM10.ReadLine();
                string[] bArray = buttonVal.Split(',');

                    button1 = int.Parse(bArray[1]);
                    button2 = int.Parse(bArray[2]);
                    button3 = int.Parse(bArray[3]);
                    button4 = int.Parse(bArray[4]);

                Debug.Log(button1 + "1");//h
                Debug.Log(button2 + "2");//s
                Debug.Log(button3 + "3");//f
                Debug.Log(button4 + "4");//k

                drum1.drumDown = button3 != 0;
                drum2.drumDown = button4 != 0;
                drum3.drumDown = button3 != 0;
                drum4.drumDown = button1 != 0;

            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                Debug.LogError("Error reading from COM9: " + e.Message);
            }
        }
    }
}