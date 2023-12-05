using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
public class SerialHandler : MonoBehaviour
{
    SerialPort streamCOM7 = new SerialPort("COM7", 9600);
    SerialPort streamCOM6 = new SerialPort("COM6", 9600); // New instance for COM6

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
            streamCOM6.Open();
            streamCOM6.ReadTimeout = 50;
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening COM6: " + e.Message);
        }
    }

    void Update()
    {
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

        if (streamCOM6.IsOpen)
        {
            try
            {
                string buttonVal = streamCOM6.ReadLine();
                string[] bArray = buttonVal.Split(',');

                if (bArray.Length >= 4)
                {
                    button1 = int.Parse(bArray[0]);
                    button2 = int.Parse(bArray[1]);
                    button3 = int.Parse(bArray[2]);
                    button4 = int.Parse(bArray[3]);
                }
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                Debug.LogError("Error reading from COM6: " + e.Message);
            }
        }
    }
}