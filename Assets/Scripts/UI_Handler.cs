using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    GameManager game;
    private GameObject one_8;
    private GameObject one_4;
    private GameObject one_2;
    private GameObject drillButton;
    private GameObject lubeButton;

    // if needed we can repeat this with the local position of the drillbit
    private Vector3 one_8_scale;
    private Vector3 one_4_scale;
    private Vector3 one_2_scale;

    void Start()
    {
        game = FindObjectOfType<GameManager>();
        
        one_8 = GameObject.Find("1/8");
        one_4 = GameObject.Find("1/4");
        one_2 = GameObject.Find("1/2");
        drillButton = GameObject.Find("DrillbitButton");
        lubeButton = GameObject.Find("LubeButton");

        one_4_scale = transform.localScale;
        one_2_scale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, transform.localScale.z);
        one_8_scale = new Vector3(transform.localScale.x / 2f, transform.localScale.y / 2f, transform.localScale.z);

        // Hide the drill bit size options
        Drillbit();
    }

    void SizeButton(string size)
    {
        // Change size of drillbit/collider
        switch (size)
        {
            case "1/8":
                transform.localScale = one_8_scale;
                break;
            case "1/4":
                transform.localScale = one_4_scale;
                break;
            case "1/2":
                transform.localScale = one_2_scale;
                break;
        }

        // Toggle off the size buttons
        Drillbit();
    }

    void Drillbit()
    {
        // Toggle 1/8, 1/4, and 1/2 options
        one_8.SetActive(!one_8.activeInHierarchy);
        one_4.SetActive(!one_4.activeInHierarchy);
        one_2.SetActive(!one_2.activeInHierarchy);


    }
    
    void Lube()
    {
        // TODO: reset this to true after hole has been drilled
        lubeButton.GetComponent<Button>().interactable = false;

        GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);

        game.lubed = true;
    }

    void Update()
    {
        Vector3 raycastDir = -transform.forward;
        Vector3 position = transform.position + (0.1f * raycastDir); // so it doesn't collide with itself

        //Debug.DrawRay(position, raycastDir * 1000, Color.red);
        //Debug.DrawRay(position, -raycastDir * 1000, Color.blue);
        string readSignal = game.inputStr;
        if (readSignal == "Double Click")
        {
            RaycastHit hit;

            Debug.DrawRay(position, raycastDir * 1000, Color.red, 10f);
            if (Physics.Raycast(position, raycastDir, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                switch (hit.collider.gameObject.name)
                {
                    case "DrillbitButton":
                        Drillbit();
                        break;
                    case "LubeButton":
                        Lube();
                        break;
                    case "1/8":
                        SizeButton("1/8");
                        break;
                    case "1/4":
                        SizeButton("1/4");
                        break;
                    case "1/2":
                        SizeButton("1/2");
                        break;
                }
            }
            
        }
    }
}