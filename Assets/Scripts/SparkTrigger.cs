using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkTrigger : MonoBehaviour
{
    private GameObject spark;
    GameManager gameManager;
    bool drilling = false;
    private float maxDepth = -1000f;
    private GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spark = GameObject.Find("Spark");
        wall = GameObject.Find("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= wall.transform.position.z)
        {
            maxDepth = wall.transform.position.z;
        }
        if (transform.position.z >= maxDepth)
        {
            maxDepth = transform.position.z;
        }
        else
        {
            drilling = false;
        }
        if (drilling && gameManager.holding)
        {
            spark.SetActive(true);
        }
        else
        {
            spark.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.tag == "drillingwall")
        {
            Debug.Log("Drillbit");
            drilling = true;
        }
       
    }
}
