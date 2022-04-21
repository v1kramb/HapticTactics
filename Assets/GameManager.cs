using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{
    SerialPort sp;
    float next_time;
    string the_com = "COM5";

    // Start is called before the first frame update
    void Start()
    {
        next_time = Time.time;

        foreach (string mysps in SerialPort.GetPortNames())
        {
            print(mysps);
            if (mysps != "COM5") { the_com = mysps; break; }
        }
        sp = new SerialPort(the_com, 115200);

        if (!sp.IsOpen)
        {
            print("Opening " + the_com + ", baud 115200");
            sp.Open();
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen) { print("Open"); }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!sp.IsOpen)
        {
            print("Opening " + the_com + ", baud 115200");
            sp.Open();
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen) { print("Open"); }
        }
    }

    public void SendCommand(string command) {
        if (sp.IsOpen)
        {
            sp.Write(command);
            print("Sent: " + command);
        }
        else
        {
            Debug.Log("Serial port not open");
            return;
        }
    }

    public string ReadCommand()
    {
        string ret = null;
        if (!sp.IsOpen)
        {
            sp.Open();
            ret = "opened sp";
        }
        try
        {
            ret = sp.ReadLine();
        }
        catch (System.Exception e)
        {
            print(e);
            return null;
        }
        return ret;
    }
}
