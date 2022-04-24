using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    GameManager game;
    private Vector3 scaleChange;

    void Start()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
        //scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
    }

    void Drillbit(Collider coll)
    {
        Debug.Log("DRILLBIT");
        // TODO: radial menu?
    }
    
    void Lube(Collider coll)
    {
        Debug.Log("LUBE");
        coll.gameObject.GetComponent<Button>().interactable = false;
        coll.enabled = false; // TODO: reset after a hole has been drilled

        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);

        game.lubed = true;
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 position = transform.position;
        Vector3 raycastDir = -transform.up;

        Debug.DrawRay(position, raycastDir * 100, Color.red);
        if (Physics.Raycast(position, raycastDir, out hit))
        {
            switch (hit.collider.gameObject.name)
            {
                case "DrillbitButton":
                    Drillbit(hit.collider);
                    break;
                case "LubeButton":
                    Lube(hit.collider);
                    break;
            }
        }
    }
}