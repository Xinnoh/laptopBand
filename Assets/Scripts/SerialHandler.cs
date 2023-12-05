using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class SerialHandler : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM7", 9600);

    public int button1, button2, button3, button4;
    private float trumpetVal;
    public TrumpetControl trumpetKey;

    void Start()
    {
        try
        {
            stream.Open();
            stream.ReadTimeout = 50;
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    void Update()
    {
        if (stream.IsOpen)
        {
            try
            {
                string serVal = stream.ReadLine();
                string[] sArray = serVal.Split(',');
                if (sArray.Length == 5)
                {
                    /*button1 = int.Parse(sArray[0]);
                    button2 = int.Parse(sArray[1]);
                    button3 = int.Parse(sArray[2]);
                    button4 = int.Parse(sArray[3]);*/
                }

                trumpetVal = float.Parse(sArray[0]);
                trumpetKey.arduinoVal = trumpetVal;
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                Debug.LogError("Error reading from serial port: " + e.Message);
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (stream != null && stream.IsOpen)
        {
            stream.Close();
        }
    }
}
