using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialWallTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.Rotate(180.0f, 0.0f, 0.0f, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
