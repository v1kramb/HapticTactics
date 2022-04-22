// Script should be called whenever user is pressing the button

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBitScript : MonoBehaviour
{
    // private GameObject drill;
    private float[] allSpeeds = { 100f, 75f }; // TODO: use slider instead of discrete values
    private float speed = 100f;
    private bool isRotating = false; // TODO: using in this script for debug purposes

    // Start is called before the first frame update
    void Start()
    {
        // drill = GameObject.Find("Drill");
    }

    // Update is called once per frame
    void Update() // TODO: add input from button to change isRotating
    {
        if (isRotating)
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
    }

    void OnTriggerEnter(Collider other) // TODO: check specific collider to change speed to approriate value
    {
        
        isRotating = true; // delete after testing
        speed = allSpeeds[1];  // on a collision, lower the drill bit's speed
    }

    private void OnTriggerExit(Collider other)
    {
        isRotating = false; // delete after testing
        speed = allSpeeds[0];
    }
}
