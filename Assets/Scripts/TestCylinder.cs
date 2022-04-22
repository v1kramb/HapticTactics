using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
public class TestCylinder : MonoBehaviour
{
    SerialPort sp;
    float next_time;
    int ii = 0;
    GameManager game;
    private bool collided;
    
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
        collided = false;
        //game.SendCommand("h4\n");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!collided)
            transform.Translate(0, -Time.deltaTime, 0);  // move along x axis 1 unit/sec

        if (Time.time > next_time)
        {
            //Debug.Log(game.ReadCommand());
            next_time = Time.time + 5;
            game.SendCommand("4\n");

            if (++ii > 20) ii = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        game.SendCommand(ii.ToString() + "\n");
        collided = true;
    }
}
