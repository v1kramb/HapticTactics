using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    void OnTriggerEnter(Collider other) // TODO: behavior for when the drill is drilling vs not drilling
    {
        // TODO: change raycast logic to cast multiple rays from drillbit
        // TODO: detect if the colliding object is the drillbit

        RaycastHit hit;

        Vector3 raycastDir = -other.transform.up;
        Vector3 norm1 = Vector3.Normalize(other.transform.forward); // our two basis vectors
        Vector3 norm2 = Vector3.Normalize(other.transform.right);

        // Starting point of raycast needs to be offset from other.transform.position
        // Find a point further back on the line created by position and up
        Vector3 offsetPosition = other.transform.position - (0.1f * raycastDir);

        // Debug.DrawRay(offsetPosition, -other.transform.up, Color.red, 100);

        float radius = other.GetComponent<CapsuleCollider>().radius * 0.05f; // radius of colliding cylinder
        // int density = GameObject.Find("Wall").GetComponent<CreateWall>().density;

        // cast several rays from points on the the cylinder's circle
        List<Vector3> positions = new List<Vector3>();

        // goal: find N points (A, B) on a circle of radius R
        // solution: simplify complexity by finding points on a square whose "radius" increases from 0 to R
        int numCircles = 2;
        for (float r = 0; r <= radius; r += (radius/(numCircles)))
        {
            float multiplier = r / Mathf.Sqrt(2); // Think of an isoceles triangle with hypotenuse r

            Vector3 position1 = offsetPosition + (multiplier * norm1) + (multiplier * norm2);
            Vector3 position2 = offsetPosition - (multiplier * norm1) + (multiplier * norm2);
            Vector3 position3 = offsetPosition + (multiplier * norm1) - (multiplier * norm2);
            Vector3 position4 = offsetPosition - (multiplier * norm1) - (multiplier * norm2);

            positions.Add(position1);
            positions.Add(position2);
            positions.Add(position3);
            positions.Add(position4);
        }

        // TODO: update this logic to accomodate multiple tris being removed

        List<int> forbiddenTris = new List<int>(); // TODO: add indexes of collided tris and axe them

        foreach (Vector3 position in positions)
        {
            if (Physics.Raycast(position, -other.transform.up, out hit, 10.0f)) // TODO: may want to incorporate density of planes
            {
                Debug.DrawRay(position, -other.transform.up, Color.red, 100);

                // // Destroy collider
                // Destroy(hit.collider.GetComponent<MeshCollider>());

                // Store the mesh from MeshFilter
                Mesh mesh = hit.collider.GetComponent<MeshFilter>().mesh;

                // // Store the triangles in a list
                // List<int> triangles = new List<int>();

                // triangles.AddRange(mesh.triangles);

                // Calculate the startIndex (At what number we start removing)
                int startIndex = hit.triangleIndex * 3;

                // // delete the three vertices
                // triangles.RemoveRange(startIndex, 3);

                // // update mesh triangles
                // mesh.triangles = triangles.ToArray();
                //mesh.triangles[startIndex] = null;
                //mesh.triangles[startIndex + 1] = null;
                //mesh.triangles[startIndex + 2] = null;

                // // Add new Collider
                // hit.collider.gameObject.AddComponent<MeshCollider>();
            }
        }

        //mesh.triangles = mesh.triangles.Where(c => c != null).ToArray();


    }
}