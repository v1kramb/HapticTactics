using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
// using System.Threading.Tasks;

public class CreateWall : MonoBehaviour
{
	public float xSize, ySize;
	public int density;
	public int depth; // number of wall planes
	public float spacing;
    public int[] materialIndexes; // length 1 if 2 materials
    public Color[] colors;
    public int[] resistances; // length should equal the number of walls
    public Material transparent;
    //public Color initialColor;
    //public Color finalColor;
    //public Material metalMaterial;
    public Shader wallShader;
    // Start is called before the first frame update
    float darkness = 0.95f;
    void Start()
    {
        // create color increment for RGB
        //float rDiff = (finalColor.r - initialColor.r) / depth;
        //float gDiff = (finalColor.g - initialColor.g) / depth;
        //float bDiff = (finalColor.b - initialColor.b) / depth;

        // create depth planes with spacing
        float initialZ = this.transform.position.z; // last plane
        int colorIndex = 0;
        
        for (int i = 0; i < depth; i++)
        {   
            //Color planeColor = new Color(initialColor.r + rDiff * i, initialColor.g + gDiff * i, initialColor.b + bDiff * i);
            if (colorIndex != colors.Length-1 && i == materialIndexes[colorIndex]) {
                colorIndex++;
            }
            CreatePlane(i, initialZ + i * spacing, colors[colorIndex]);
        }
        //metalMaterial = Resources.Load("metal11_diffuse", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatePlane(int index, float planeZ, Color planeColor)
    {
        GameObject plane = new GameObject();
        plane.name = "Plane " + index; // now we can map collision to material type easily
        plane.tag = "drillingwall";
        // define position
        plane.transform.parent = this.transform;
        plane.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, planeZ);
        
        // add components programmatically
        //plane.AddComponent<Rigidbody>();
        //plane.GetComponent<Rigidbody>().useGravity = false;
        //plane.GetComponent<Rigidbody>().isKinematic = true;

        // create custom mesh filter
        plane.AddComponent<MeshFilter>();
        Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
        mesh.name = "Plane Mesh " + index;

        // Generate mesh vertices
        Vector3[] vertices = new Vector3[(int) ((xSize * density + 1) * (ySize * density + 1))];
        for (int i = 0, y = 0; y <= ySize * density; y++)
        {
            for (int x = 0; x <= xSize * density; x++, i++)
            {
                float newX = x * (1f / density);
                float newY = y * (1f / density);
                vertices[i] = new Vector3(newX, newY);
            }
        }

        //Mesh mesh = plane.GetComponent<MeshFilter>().mesh;

        mesh.vertices = vertices;

        // Generate mesh triangles
        int[] triangles = new int[(int) (xSize * ySize * 6 * density * density)];

        for (int ti = 0, vi = 0, y = 0; y < ySize * density; y++, vi++)
        {
            for (int x = 0; x < xSize * density; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = (int) (vi + (xSize * density) + 1);
                triangles[ti + 5] = (int) (vi + (xSize * density) + 2);
            }
        }
        mesh.triangles = triangles;
        Color newPlaneColor = new Color(planeColor.r * darkness, planeColor.g * darkness, planeColor.b * darkness, 1);
        
        darkness *= darkness;
        // Set colors of tris
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
            colors[i] = newPlaneColor;

        // Mark holes to drill
        List<int> holesToDrill = new List<int>() { 23, 94, 119 };

        foreach (int idx in holesToDrill)
            colors[idx] = Color.red;

        mesh.colors = colors;

        // Add mesh renderer with material that makes triangle color visible
        MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Particles/Standard Surface")); // Particles/Standard surface
        meshRenderer.material.enableInstancing = true;

        // meshRenderer.material.SetFloat("_Mode", 2);
       //meshRenderer.material = transparent;
        //plane.GetComponent<Renderer>().material = metalMaterial;
        //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;

        // Add mesh collider at the end and script to make holes in mesh
        plane.AddComponent<MeshCollider>(); // must be added AFTER editing mesh
        plane.AddComponent<CreateHole>();
       // plane.AddComponent<SparkTrigger>();
       //plane.transform.Rotate(180.0f, 0.0f, 0.0f, Space.Self);

    }
}
