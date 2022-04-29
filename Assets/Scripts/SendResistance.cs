using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class SendResistance : MonoBehaviour
{
    GameManager game;
    private GameObject wall;
    private int currWall;
    float next_time;

    private bool collided;
    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameManager>();
        wall = GameObject.Find("Wall");
        currWall = 0;
        next_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (Time.time > next_time && game.holding)
        {
            next_time = Time.time + 0.2f;

            if (other.gameObject.name.StartsWith("Plane ")) {
                int planeIdx = Int32.Parse(other.gameObject.name.Split(' ')[1]);

                if (currWall != wall.GetComponent<CreateWall>().resistances.Length - 1 && planeIdx == wall.GetComponent<CreateWall>().materialIndexes[currWall])
                    currWall++;

                if (planeIdx == wall.GetComponent<CreateWall>().depth - 1) // reset when we hit the last plane
                { 
                    currWall = 0;
                    game.holding = false;
                }

                game.SendCommand(wall.GetComponent<CreateWall>().resistances[currWall].ToString() + "\n");
            }
        }
    }
}
