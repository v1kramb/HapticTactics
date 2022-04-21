using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //void deleteTri(int index)
    //{
    //    Destroy(this.gameObject.GetComponent<MeshCollider>());
    //    Mesh mesh = transform.GetComponent<MeshFilter>().mesh;

    //    int[] oldTriangles = mesh.triangles;
    //    int[] newTriangles = new int[mesh.triangles.Length - 3];

    //    int i = 0;
    //    int j = 0;
    //    while (j < mesh.triangles.Length)
    //    {
    //        if (j != index * 3)
    //        {
    //            newTriangles[i++] = oldTriangles[j++];
    //            newTriangles[i++] = oldTriangles[j++];
    //            newTriangles[i++] = oldTriangles[j++];
    //        }
    //        else
    //        {
    //            j += 3;
    //        }
    //    }
    //    transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
    //    this.gameObject.AddComponent<MeshCollider>();
    //}

    void deleteSquare(int index1, int index2)
    {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;

        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;
        while (j < mesh.triangles.Length)
        {
            if (j != index1 * 3 && j != index2 * 3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }
        }
        transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        this.gameObject.AddComponent<MeshCollider>();
    }

    int findVertex(Vector3 v)
    {
        Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i] == v)
                return i;
        }
        return -1;
    }

    int findTriangle(Vector3 v1, Vector3 v2, int notTriIndex)
    {
        int[] triangles = transform.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;

        int i = 0;
        int j = 0;
        int found = 0;

        while (j < triangles.Length)
        {
            if (j / 3 != notTriIndex)
            {
                if (vertices[triangles[j]] == v1 && (vertices[triangles[j + 1]] == v2 || vertices[triangles[j + 2]] == v2))
                    return j / 3;
                else if (vertices[triangles[j]] == v2 && (vertices[triangles[j + 1]] == v1 || vertices[triangles[j + 2]] == v1))
                    return j / 3;
                else if (vertices[triangles[j + 1]] == v2 && (vertices[triangles[j]] == v1 || vertices[triangles[j + 2]] == v1))
                    return j / 3;
                else if (vertices[triangles[j + 1]] == v1 && (vertices[triangles[j]] == v2 || vertices[triangles[j + 2]] == v2))
                    return j / 3;
            }
            j += 3;
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    void OnTriggerEnter(Collider other) // TODO: behavior for when the drill is drilling vs not drilling
    {
        RaycastHit hit;

        // Starting point of raycast needs to be offset from other.transform.position
        // Find a point further back on the line created by position and forward
        Vector3 offsetPosition = other.transform.position - (0.1f * other.transform.forward);

        //Debug.DrawRay(offsetPosition, other.transform.forward, Color.red, 100);
        // other.transform.forward before
        if (Physics.Raycast(offsetPosition, -other.transform.up, out hit, 10.0f)) // TODO: need to decrease other.transform.forward
        {
            // BIG TODO: May need to change logic to see ALL collision points

            // Destroy collider
            Destroy(hit.collider.GetComponent<MeshCollider>());

            // Store the mesh from MeshFilter
            Mesh mesh = hit.collider.GetComponent<MeshFilter>().mesh;

            // Store the triangles in a list
            List<int> triangles = new List<int>();
            triangles.AddRange(mesh.triangles);

            // Calculate the startIndex (At what number we start removing)
            int startIndex = hit.triangleIndex * 3;

            // RemoveRange first parameter is index (at what number we start removing),
            // Which is our earlier calculated startIndex.
            // We want to delete 3 vertices, which is the second parameter here
            triangles.RemoveRange(startIndex, 3);

            // Update the triangles, we must convert our List to an Array here
            mesh.triangles = triangles.ToArray();

            // Add new Collider
            hit.collider.gameObject.AddComponent<MeshCollider>();


            //int hitTri = hit.triangleIndex;

            //int[] triangles = transform.GetComponent<MeshFilter>().mesh.triangles;
            //Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;

            //Vector3 p0 = vertices[triangles[hitTri * 3]];
            //Vector3 p1 = vertices[triangles[hitTri * 3 + 1]];
            //Vector3 p2 = vertices[triangles[hitTri * 3 + 2]];

            //float edge1 = Vector3.Distance(p0, p1);
            //float edge2 = Vector3.Distance(p0, p2);
            //float edge3 = Vector3.Distance(p1, p2);

            //Vector3 shared1;
            //Vector3 shared2;

            //if (edge1 > edge2 && edge1 > edge3)
            //{
            //    shared1 = p0;
            //    shared2 = p1;
            //}
            //else if (edge2 > edge1 && edge2 > edge3)
            //{
            //    shared1 = p0;
            //    shared2 = p2;
            //}
            //else
            //{
            //    shared1 = p1;
            //    shared2 = p2;
            //}

            //int v1 = findVertex(shared1);
            //int v2 = findVertex(shared2);

            //deleteSquare(hitTri, findTriangle(vertices[v1], vertices[v2], hitTri));
            //// deleteTri(hit.triangleIndex);
        }
    }
}