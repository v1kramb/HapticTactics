using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class SendResistance : MonoBehaviour
{
    GameManager game;
    public float delay;

    private GameObject wall;
    private int currWall;
    float next_time;
    private bool hitEnd;
    private float hitTime;

    private bool collided;
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameManager>();
        wall = GameObject.Find("Wall");
        currWall = 0;
        next_time = Time.time;
        delay = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitEnd && Time.time > hitTime + delay)
        {
            game.SendCommand("21\n");
            hitEnd = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (Time.time > next_time && game.holding)
        {
            next_time = Time.time + 0.2f;

            if (other.gameObject.name.StartsWith("Plane ")) {
                int planeIdx = Int32.Parse(other.gameObject.name.Split(' ')[1]);

                // currWall = 0;
                // resistances: [3, 10, 20]
                // material_indexes = [2, 4]

                // collide with Plane 0
                // send resistance[0]

                // collide with Plane 2
                // second resistance[1]
                
                // collide with Plane 4
                // send resistance[2]
                
                // collide with Plane 7
                // send 3
                // send reset after delay
                
                // 0, 1, 2
                
                if (planeIdx == 0)
                {
                    game.SendCommand(wall.GetComponent<CreateWall>().resistances[0].ToString() + "\n");
                }

                if (planeIdx == 2)
                {
                    game.SendCommand(wall.GetComponent<CreateWall>().resistances[1].ToString() + "\n");
                }

                if (planeIdx == 4)
                {
                    game.SendCommand(wall.GetComponent<CreateWall>().resistances[2].ToString() + "\n");
                }

                if (planeIdx == 7)
                {
                    game.SendCommand("3\n");
                    game.holding = false;
                    
                    GetComponent<UI_Handler>().ResetLubeButton(); // resets game.lubed

                    hitEnd = true;
                    hitTime = Time.time;
                }
                
                //if (planeIdx == 0 || (currWall != 0 && planeIdx == wall.GetComponent<CreateWall>().materialIndexes[currWall - 1]))
                //{
                //    game.SendCommand(wall.GetComponent<CreateWall>().resistances[currWall].ToString() + "\n");
                //}
                //else if (planeIdx == wall.GetComponent<CreateWall>().depth - 1)
                //{
                //    game.SendCommand("3\n");
                //    game.holding = false;

                //    hitEnd = true;
                //    hitTime = Time.time;
                //}

                //currWall++;
                //if (currWall == wall.GetComponent<CreateWall>().resistances.Length)
                //    currWall = 0;
            }
        }
    }
}
