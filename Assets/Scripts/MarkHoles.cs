// Attached to Plane 0 only
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkHoles : MonoBehaviour
{
    private Mesh mesh;
    private Color[] colors;
    private Vector3[] vertices;
    private int[] triangles;

    private Dictionary<string, List<int>> holesToDrill;
    private float defaultRadius;

    private bool drawn = false;
    private float currtime;

    // Start is called before the first frame update
    void Start()
    {
        currtime = Time.time;

        mesh = GetComponent<MeshFilter>().mesh; // attached to CreateHole mesh, GetComponent<MeshFilter>().mesh;

        colors = mesh.colors;
        vertices = mesh.vertices;
        triangles = mesh.triangles;

        holesToDrill = new Dictionary<string, List<int>>();
        holesToDrill.Add("1/4", new List<int> { 20000, 30000 });
        holesToDrill.Add("1/8", new List<int> { 35000, 13100, 45400 });
        holesToDrill.Add("1/2", new List<int> { 48000, 10000, 12400});        

        defaultRadius = GameObject.Find("Drillbit").GetComponent<CapsuleCollider>().radius * 0.002f * 0.25f; // need global scale
    }

    void DrawHoles()
    {
        Vector3 raycastDir = transform.forward;
        Vector3 norm1 = Vector3.Normalize(transform.up); // our two basis vectors
        Vector3 norm2 = Vector3.Normalize(transform.right);

        foreach (KeyValuePair<string, List<int>> entry in holesToDrill)
        {
            float radius = defaultRadius;
            //List<int> forbiddenTris = new List<int>();

            switch (entry.Key)
            {
                case "1/4":
                    radius = defaultRadius;
                    break;
                case "1/8":
                    radius = defaultRadius / 2f;
                    break;
                case "1/2":
                    radius = defaultRadius * 2f;
                    break;
            }
            
            foreach (int holeIdx in entry.Value)
            {
                colors[holeIdx] = Color.red;

                RaycastHit hit;

                Vector3 offsetPosition = transform.TransformPoint(vertices[holeIdx]) - (0.1f * raycastDir);

                List<Vector3> positions = new List<Vector3>();
                positions.Add(offsetPosition);

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

                List<int> forbiddenTris = new List<int>();

                // raycast from those points to the plane
                foreach (Vector3 position in positions)
                {
                    Ray ray = new Ray(position, raycastDir);
                    //Debug.DrawRay(position, raycastDir * 100, Color.red, 100f);
                    if (GetComponent<MeshCollider>().Raycast(ray, out hit, 100))
                    {
                        int startIndex = hit.triangleIndex * 3;

                        if (!forbiddenTris.Contains(startIndex)) // multiple raycasts may cause overlap
                        {
                            forbiddenTris.Add(startIndex); // startIndex, startIndex + 1, and startIndex + 2 will be discarded
                        }
                    }
                }

                foreach (int forbiddenIdx in forbiddenTris)
                {
                    colors[triangles[forbiddenIdx]] = Color.red;
                    colors[triangles[forbiddenIdx + 1]] = Color.red;
                    colors[triangles[forbiddenIdx + 2]] = Color.red;
                }
            }
        }
        
        mesh.colors = colors;
    }

    // Update is called once per frame
    void Update()
    {
        if (!drawn && Time.time > currtime + 1f) // why didn't I use a coroutine
        {
            DrawHoles();
            drawn = true;
        }
    }
}
