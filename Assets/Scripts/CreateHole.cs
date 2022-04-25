using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHole : MonoBehaviour
{
    private GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        wall = GameObject.Find("Wall");
    }

    // Update is called once per frame
    void Update()
    {

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

    void OnTriggerEnter(Collider other) // TODO: behavior for when the drill is drilling vs not drilling
    {
        // TODO: detect if the colliding object is the drillbit

        RaycastHit hit;

        Vector3 raycastDir = -other.transform.forward;
        Vector3 norm1 = Vector3.Normalize(other.transform.up); // our two basis vectors
        Vector3 norm2 = Vector3.Normalize(other.transform.right);

        Vector3 offsetPosition = other.transform.position; // - (0.01f * raycastDir);

        float radius = other.GetComponent<CapsuleCollider>().radius * 0.05f; // radius of colliding cylinder (*0.05 because of scaling in transform)
        float planeSpacing = wall.GetComponent<CreateWall>().spacing * 2;

        float triDistance = 0.2f / wall.GetComponent<CreateWall>().density;

        // cast several rays from points on the the cylinder's circle
        List<Vector3> positions = new List<Vector3>();
        positions.Add(offsetPosition);

        for (float yMult = -radius - (triDistance/2); yMult < radius + (triDistance/2); yMult += triDistance) // good enough, can probably be improved
        {
            for (float xMult = -radius - (triDistance/2); xMult < radius + (triDistance/2); xMult += triDistance)
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
            if (GetComponent<MeshCollider>().Raycast(ray, out hit, planeSpacing))
            {
                //if (gameObject.name == "Plane 0")
                //{
                //    Debug.DrawRay(position, raycastDir * planeSpacing, Color.red, 100.0f);
                //}

                // Add start of new triangle to forbidden list
                int startIndex = hit.triangleIndex * 3;

                if (!forbiddenTris.Contains(startIndex)) // multiple raycasts may cause overlap
                { 
                    forbiddenTris.Add(startIndex); // startIndex, startIndex + 1, and startIndex + 2 will be discarded
                }
            }
        }

        // Destroy collider
        Destroy(GetComponent<MeshCollider>());

        // Store the mesh from MeshFilter
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Create temporary list for new triangles
        List<int> newMesh = new List<int>();
        newMesh.AddRange(mesh.triangles);

        // Efficient list removal (needed for dense meshes)
        forbiddenTris.Sort();
        FilterOutIndicies(newMesh, forbiddenTris);

        // Update mesh triangles
        mesh.triangles = newMesh.ToArray();

        // Add new collider
        gameObject.AddComponent<MeshCollider>();
    }
}