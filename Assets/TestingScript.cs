using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
public class TestingScript : MonoBehaviour
{
    SerialPort sp;
    float next_time;
    int ii = 0;
    GameManager game;
    int hum = -1;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
        // game.SendCommand("118\n");
    }

    // Update is called once per frame
    void Update()
    {


        //transform.Translate(0, -Time.deltaTime, 0);  // move along x axis 1 unit/sec
        /**string command = game.ReadCommand();
        if (command != null)
        {
            if (command.Equals("Holding"))
            {
                Debug.Log("humming");
                game.SendCommand("47\n");
            }
            else if (command.Equals("Released"))
            {
                Debug.Log("not humming");
                game.SendCommand("700\n");
            }
        }/**
        if (Time.time > next_time)
        {
            hum *= -1;
            Debug.Log(game.ReadCommand());
            next_time = Time.time + 10;

            if (hum == 1)
            {
                Debug.Log("humming" + hum);
                game.SendCommand("118\n");
            }
            
            if (++ii > 20) ii = 0;
        }
        else if (hum == 1)
        {
            hum *= -1;
            Debug.Log("not humming" + hum);
            game.SendCommand("150\n");
        }**/
    }

}
