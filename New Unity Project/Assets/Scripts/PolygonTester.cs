using UnityEngine;
using System.Collections;

public class PolygonTester: MonoBehaviour
{
    [SerializeField]
    [Range(3, 20)]
    public int NumberOfSides = 3;

    [SerializeField]
    public float RadialSize = 50f;
    public MeshFilter filter;
    private MeshRenderer meshy;
    public Vector2[] newShape = null;

    void Start()
    {
        // Set up game object with mesh;
        meshy = gameObject.AddComponent<MeshRenderer>();
        filter = gameObject.AddComponent<MeshFilter>();
    }

    Vector2[] NewVerticies()
    {
        Vector2[] verts = new Vector2[NumberOfSides];

        for (int i = 0; i < NumberOfSides; ++i)
        {
            verts[i].x = RadialSize * Mathf.Cos(2 * Mathf.PI * i / NumberOfSides);
            verts[i].y = RadialSize * Mathf.Sin(2 * Mathf.PI * i / NumberOfSides);
        }

        return verts;
    }

    Mesh calcMesh()
    {
        // Use the triangulator to get indices for creating triangles
        Vector2[] newVerticies = NewVerticies();
        Triangulator tr = new Triangulator(newVerticies);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[newVerticies.Length];
        for (int i=0; i < newVerticies.Length; i++)
        {
            vertices[i] = new Vector3(newVerticies[i].x, newVerticies[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        return msh;
    }

    void Update()
    {
        filter.mesh = calcMesh();
    }

    public void hideMesh()
    {
        meshy.enabled = false;
    }
}