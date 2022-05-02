using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHole : MonoBehaviour
{
    private GameObject wall;
    GameManager game;
    private GameObject drillBit;

    private float radiusReg;
    private float radiusDiv;
    private float radiusMult;

    private float radius;
    
    // storing locally improves performance dramatically
    private Mesh mesh;
    private Color[] colors;
    private int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        wall = GameObject.Find("Wall");
        drillBit = GameObject.Find("Drillbit");
        game = FindObjectOfType<GameManager>();
        // wall.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 2);

        mesh = GetComponent<MeshFilter>().mesh;
        colors = mesh.colors;
        triangles = mesh.triangles;

        radiusReg = drillBit.GetComponent<CapsuleCollider>().radius * 0.002f * 0.25f; // need global transform
        radiusDiv = radiusReg / 2f;
        radiusMult = radiusReg * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        switch(game.scale)
        {
            case 0:
                radius = radiusDiv;
                break;
            case 1:
                radius = radiusReg;
                break;
            case 2:
                radius = radiusMult;
                break;
        }
    }

    // Optimized method to filter out particular indices from list (forbidden triangles from mesh's triangles)
    // Source: https://stackoverflow.com/questions/63495264/how-can-i-efficiently-remove-elements-by-index-from-a-very-large-list
    static void FilterOutIndicies(List<int> values, List<int> sortedIndicies)
    {
        int sourceStartIndex = 0;
        int destStartIndex = 0;
        int spanLength = 0;

        int skipCount = 0;

        // Copy items up to last index to be skipped
        foreach (var skipIndex in sortedIndicies)
        {
            spanLength = skipIndex - sourceStartIndex;
            destStartIndex = sourceStartIndex - skipCount;

            for (int i = sourceStartIndex; i < sourceStartIndex + spanLength; i++)
            {
                values[destStartIndex] = values[i];
                destStartIndex++;
            }

            sourceStartIndex = skipIndex + 3; // skips entire triangle
            skipCount += 3;
        }

        values.RemoveRange(destStartIndex, sortedIndicies.Count * 3);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "drillbit" || !game.holding)
            return;

        RaycastHit hit;
        
        Vector3 raycastDir = -other.transform.forward;
        Vector3 norm1 = Vector3.Normalize(other.transform.up); // our two basis vectors
        Vector3 norm2 = Vector3.Normalize(other.transform.right);

        Vector3 offsetPosition = other.transform.position; // - (0.01f * raycastDir);

        //float radius = drillbitCollider.radius * 0.002f * 0.25f; // radius of colliding cylinder 
        //float planeSpacing = wall.GetComponent<CreateWall>().spacing * 2;
        //float triDistance = 0.2f / wall.GetComponent<CreateWall>().density;

        // cast several rays from points on the the cylinder's circle
        List<Vector3> positions = new List<Vector3>();
        positions.Add(offsetPosition);

        //Debug.Log(radius);

        for (float yMult = -radius; yMult < radius; yMult += 0.0005f)
        {
            for (float xMult = -radius; xMult < radius; xMult += 0.0005f)
            {
                Vector3 sum = (xMult * norm1) + (yMult * norm2);

                if (sum.magnitude <= radius)
                {
                    positions.Add(offsetPosition + sum);
                }
            }
        }

        // raycast from those points to the plane
        List<int> forbiddenTris = new List<int>(); // stores indices of vertices to be removed

        foreach (Vector3 position in positions)
        {
            Ray ray = new Ray(position, raycastDir);
            //if (gameObject.name == "Plane 0")
            //{
            //    Debug.DrawRay(position, raycastDir * planeSpacing, Color.red, 100.0f);
            //}

            if (GetComponent<MeshCollider>().Raycast(ray, out hit, 100))
            {

                // Add start of new triangle to forbidden list

                int startIndex = hit.triangleIndex * 3;

                if (!forbiddenTris.Contains(startIndex)) // multiple raycasts may cause overlap
                { 
                    forbiddenTris.Add(startIndex); // startIndex, startIndex + 1, and startIndex + 2 will be discarded
                }
            }
        }

        //// Destroy collider
        //Destroy(GetComponent<MeshCollider>());
        //Destroy(GetComponent<MeshRenderer>());

        // Store the mesh from MeshFilter
        //Mesh mesh = GetComponent<MeshFilter>().mesh;

        //// Create temporary list for new triangles
        //List<int> newMesh = new List<int>();
        //newMesh.AddRange(mesh.triangles);

        //// Efficient list removal (needed for dense meshes)
        //forbiddenTris.Sort();
        //FilterOutIndicies(newMesh, forbiddenTris);

        //// Update mesh triangles
        //mesh.triangles = newMesh.ToArray();


        //Instead of editing the mesh, make the color of the tris transparent
        //forbiddenTris contain indices corresponding to mesh.triangles, not mesh.vertices

        /*Color[] colors = new Color[mesh.colors.Length];
         for (int i = 0; i < mesh.colors.Length; i++)
             colors[i] = mesh.colors[i];

         Color temp = Color.clear;  // Color.red; // mesh.colors[0]; // have to cache color to change its albedo
         temp.a = 0.0f; // transparent

         foreach (int idx in forbiddenTris)
         {
             colors[mesh.triangles[idx]] = temp;
             colors[mesh.triangles[idx + 1]] = temp;
             colors[mesh.triangles[idx + 2]] = temp;
         }

         mesh.colors = colors;

         mesh.RecalculateNormals();*/

        // Create black etching on first plane
        Color temp = Color.black;

        if (gameObject.name == "Plane 0")
        {
            foreach (int idx in forbiddenTris)
            {
                colors[triangles[idx]] = temp;
                colors[triangles[idx + 1]] = temp;
                colors[triangles[idx + 2]] = temp;
            }
        }

        mesh.colors = colors;
        
        // creates garbage triangles, better than removing them
        foreach (int idx in forbiddenTris)
        {
            triangles[idx] = 0;
            triangles[idx + 1] = 0;
            triangles[idx + 2] = 0;
        }

        mesh.triangles = triangles;

        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();

        //// Add new collider
        //gameObject.AddComponent<MeshRenderer>();
        //gameObject.AddComponent<MeshCollider>();
    }
}