// Script should be called whenever user is pressing the button

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBitScript : MonoBehaviour
{
    GameManager gameManager;
    private float currSpeed;
    private float maxSpeed = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currSpeed = 1000f;
    }

    void Update()
    {

        if (gameManager.holding)
        {
            transform.Rotate(0, 0, Time.deltaTime * currSpeed);
        }

        if (GetComponent<SparkTrigger>().drilling)
        {
            // TODO: lower speed based on resistance of wall
            currSpeed = 500f;
        }
        else
        {
            currSpeed = maxSpeed;
        }
    }
}
