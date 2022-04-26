using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{
    SerialPort sp;
    float next_time;
    string the_com = "COM5";
    public bool holding = false;
    public AudioSource drillAudio;

    public GameObject wall;
    public GameObject drill;
    float angleTolerance = 10.0f;
    public string inputStr;
    public bool lubed = false;
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
        //checkDrillAngle();

        if (!sp.IsOpen)
        {
            print("Opening " + the_com + ", baud 115200");
            sp.Open();
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen) { print("Open"); }
        }
        inputStr = ReadCommand();
        if (inputStr != null)
        {
            Debug.Log("Input" + inputStr);
            if (inputStr.Equals("Holding"))
            {

                holding = true;
            }
            else if (inputStr.Equals("Released"))
            {
                holding = false;
            }
        }
        //holding = true;
        if (holding)
        {
            if (!drillAudio.isPlaying)
                drillAudio.Play();
            //Debug.Log("Holding");
            if (!lubed)
            {
                checkLube();
            }
            else
            {
                checkDrillAngle();
            }
        }
        else
        {
            //Debug.Log("Released");
            if (drillAudio.isPlaying)
                drillAudio.Stop();
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
        int bytesToRead = sp.BytesToRead;
        if (bytesToRead > 0)
        {
            try
            {
                ret = sp.ReadLine();
                //Debug.Log("Read " + ret);
            }
            catch (System.Exception e)
            {
                print(e);
                return null;
            }
        }
        
        return ret;
    }

    public void checkDrillAngle()
    {
        double x = drill.transform.eulerAngles.x;
        double y = drill.transform.eulerAngles.y;
        double z = drill.transform.eulerAngles.z;

        
        float angleToTarget = Vector3.Angle(drill.transform.forward, wall.transform.forward);
        /*if (Time.time > next_time)
        {
            Debug.Log(angleToTarget);
        }*/
        if (angleToTarget>angleTolerance)
        {
            if (Time.time > next_time)
            {
                next_time = Time.time + 0.5f;
                SendCommand("100\n");
            }
        }
        else
        {
            SendCommand("7000\n");
        }

    }

    public void checkLube()
    {
        if (!lubed)
        {
            if (Time.time > next_time)
            {
                next_time = Time.time + 0.5f;
                SendCommand("100\n");
            }
        }
        
    }
}
