using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Threading.Tasks;

public class CreateWall : MonoBehaviour
{
	public int xSize, ySize;
	public int density;
	public int depth; // number of wall planes
	public float spacing;
    public int[] materialIndexes; // length 1 if 2 materials
    public Color[] colors;
    //public Color initialColor;
    //public Color finalColor;

    // Start is called before the first frame update
    void Start()
    {
        // create color increment for RGB
        //float rDiff = (finalColor.r - initialColor.r) / depth;
        //float gDiff = (finalColor.g - initialColor.g) / depth;
        //float bDiff = (finalColor.b - initialColor.b) / depth;

        // create depth planes with spacing
        float initialZ = this.transform.position.z;
        int colorIndex = 0;
        
        for (int i = 0; i < depth; i++)
        {   
            //Color planeColor = new Color(initialColor.r + rDiff * i, initialColor.g + gDiff * i, initialColor.b + bDiff * i);
            if (colorIndex != colors.Length-1 && i == materialIndexes[colorIndex]) {
                colorIndex++;
            }
            CreatePlane(i, initialZ + i * spacing, colors[colorIndex]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePlane(int index, float planeZ, Color planeColor)
    {
        GameObject plane = new GameObject();
        plane.name = "Plane " + index; // now we can map collision to material type easily
        
        // define position
        plane.transform.parent = this.transform;
        plane.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, planeZ);

        // add components programmatically
        // plane.AddComponent<Rigidbody>();
        //plane.GetComponent<Rigidbody>().useGravity = false;
        // plane.GetComponent<Rigidbody>().isKinematic = true;

        // create custom mesh filter
        plane.AddComponent<MeshFilter>();
        plane.GetComponent<MeshFilter>().mesh = new Mesh();
        plane.GetComponent<MeshFilter>().mesh.name = "Plane Mesh " + index;

        // Generate mesh vertices
        Vector3[] vertices = new Vector3[(xSize * density + 1) * (ySize * density + 1)];
        for (int i = 0, y = 0; y <= ySize * density; y++)
        {
            for (int x = 0; x <= xSize * density; x++, i++)
            {
                float newX = x * (1f / density);
                float newY = y * (1f / density);
                vertices[i] = new Vector3(newX, newY);
            }
        }
        plane.GetComponent<MeshFilter>().mesh.vertices = vertices;

        // Generate mesh triangles
        int[] triangles = new int[xSize * ySize * 6 * density * density];

        for (int ti = 0, vi = 0, y = 0; y < ySize * density; y++, vi++)
        {
            for (int x = 0; x < xSize * density; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + (xSize * density) + 1;
                triangles[ti + 5] = vi + (xSize * density) + 2;
            }
        }
        plane.GetComponent<MeshFilter>().mesh.triangles = triangles;

        plane.AddComponent<MeshRenderer>();
        plane.GetComponent<MeshRenderer>().material.SetColor("_Color", planeColor);

        plane.AddComponent<MeshCollider>(); // must be added AFTER editing mesh

        plane.AddComponent<CreateHole>();
    }
}
