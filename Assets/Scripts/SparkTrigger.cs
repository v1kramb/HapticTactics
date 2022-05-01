using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkTrigger : MonoBehaviour
{
    private GameObject spark;
    GameManager gameManager;
    public bool drilling = false;

    //private float startDepth;
    //private float endDepth;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spark = GameObject.Find("Spark");
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.holding)
        {
            spark.SetActive(false);
        }
    }
    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "sparky" && gameManager.holding)
        {
            spark.SetActive(true);
            drilling = true;
        }
        else
        {
            spark.SetActive(false);
            drilling = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "sparky")
        {
            spark.SetActive(false);
            drilling = true;
        }
        

    }
}
