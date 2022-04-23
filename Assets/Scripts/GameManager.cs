using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{
    SerialPort sp;
    float next_time;
    string the_com = "COM5";
    bool holding = false;
    public AudioSource drillAudio;

    public GameObject drill;
    int minAngleX = 85;
    int minAngleY = 85;
    int maxAngleX = 95;
    int maxAngleY = 95;

    public bool lubed = false;
    // Start is called before the first frame update
    void Start()
    {
        
        //next_time = Time.time;

        //foreach (string mysps in SerialPort.GetPortNames())
        //{
        //    print(mysps);
        //    if (mysps != "COM5") { the_com = mysps; break; }
        //}
        //sp = new SerialPort(the_com, 115200);

        //if (!sp.IsOpen)
        //{
        //    print("Opening " + the_com + ", baud 115200");
        //    sp.Open();
        //    sp.ReadTimeout = 100;
        //    sp.Handshake = Handshake.None;
        //    if (sp.IsOpen) { print("Open"); }

        //}
    }

    // Update is called once per frame
    void Update()
    {
        

        //if (!sp.IsOpen)
        //{
        //    print("Opening " + the_com + ", baud 115200");
        //    sp.Open();
        //    sp.ReadTimeout = 100;
        //    sp.Handshake = Handshake.None;
        //    if (sp.IsOpen) { print("Open"); }
        //}
        //string inputStr = ReadCommand();
        //if (inputStr != null)
        //{
        //    Debug.Log("Input"+ inputStr);
        //    if (inputStr.Equals("Holding"))
        //    {
                
        //        holding = true;
        //    }
        //    else if (inputStr.Equals("Released"))
        //    {
        //        holding = false;
        //    }
        //}
        //if (holding)
        //{
        //    if (!drillAudio.isPlaying)
        //        drillAudio.Play();
        //    //Debug.Log("Holding");
        //    if (!lubed)
        //    {
        //        checkLube();
        //    }
        //    else
        //    {
        //        checkDrillAngle();
        //    }
        //}
        //else
        //{
        //    //Debug.Log("Released");
        //    if (drillAudio.isPlaying)
        //        drillAudio.Stop();
        //}
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
        if (x<minAngleX || y<minAngleY || x>maxAngleX || y > maxAngleY)
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
        if (lubed)
        {
            if (Time.time > next_time)
            {
                next_time = Time.time + 0.5f;
                SendCommand("69\n");
            }
        }
        else
        {
            SendCommand("40\n");
        }
    }
}
