using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))] //if components not in place
[RequireComponent(typeof(MeshFilter))]

public class SimpleMesh : MonoBehaviour
{
    [Range(3, 255)]
    public int segments = 10;
    [Range(0.1f, 100)]
    public float innerRadius = 5;
    [Range(0.1f, 25f)]
    public float thickness = 5;

    private Mesh GenerateDonutMesh()
    {
        Mesh mesh = new Mesh();

        //vertices
        List<Vector3> verts = new List<Vector3>();

        List<Vector2> uvs = new List<Vector2>();

        float delta_angle = 360 / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * delta_angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * innerRadius;
            float y = Mathf.Sin(angle) * innerRadius;
            // Add the vertex into a suitable data structure (array, list)

            Vector3 vertex = new Vector3(x, 0, y);
            verts.Add(vertex);

            Vector2 uv = (new Vector2(1,1) + (new Vector2(x, y) / (innerRadius + thickness)))/2.0f;
            uvs.Add(uv);

            x = Mathf.Cos(angle) * (innerRadius + thickness);
            y = Mathf.Sin(angle) * (innerRadius + thickness);

            vertex = new Vector3(x, 0, y);
            verts.Add(vertex);

            uv = (new Vector2(1, 1) + (new Vector2(x, y) / (innerRadius + thickness))) / 2.0f;
            uvs.Add(uv);
        }

        //triangles
        List<int> tris = new List<int>();


        for (int i = 0; i < segments - 1; i++)
        {
            tris.Add(i*2);
            tris.Add(i*2+3);
            tris.Add(i*2+1);

            tris.Add(i*2);
            tris.Add(i*2+2);
            tris.Add(i*2+3);
        }

        //last triangles
        tris.Add((segments-1)*2);
        tris.Add(1);
        tris.Add((segments-1)*2+1);

        tris.Add((segments-1)*2);
        tris.Add(0);
        tris.Add(1);


        //set values
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);

        mesh.SetUVs(0, uvs);

        mesh.RecalculateNormals();


        return mesh;
    }

    //private Mesh GenerateDiscMesh()
    //{
    //    Mesh mesh = new Mesh();

    //    //vertices
    //    List<Vector3> verts = new List<Vector3>();

    //    verts.Add(Vector3.zero);

    //    float delta_angle = 360 / segments;

    //    for (int i = 0; i < segments; i++)
    //    {
    //        float angle = i * delta_angle * Mathf.Deg2Rad;
    //        float x = Mathf.Cos(angle) * innerRadius;
    //        float y = Mathf.Sin(angle) * innerRadius;
    //        // Add the vertex into a suitable data structure (array, list)

    //        Vector3 vertex = new Vector3(x, 0, y);
    //        verts.Add(vertex);
    //    }

    //    //triangles
    //    List<int> tris = new List<int>();


    //    for (int i = 0; i < segments-1; i++)
    //    {
    //        tris.Add(0);
    //        tris.Add(i+2);
    //        tris.Add(i+1);
    //    }

    //    //last triangle
    //    tris.Add(0);
    //    tris.Add(1);
    //    tris.Add(segments);


    //    //set values
    //    mesh.SetVertices(verts);
    //    mesh.SetTriangles(tris, 0);
    //    mesh.RecalculateNormals();


    //    return mesh;
    //}



    //private Mesh GenerateMesh()
    //{
    //    Mesh mesh = new Mesh();

    //    //vertices
    //    Vector3[] verts = new Vector3[4];
    //    verts[0] = new Vector3(0, 0, 0)*10;
    //    verts[1] = new Vector3(1, 0, 0)*10;
    //    verts[2] = new Vector3(0, 0, 1)*10;
    //    verts[3] = new Vector3(1, 0, 1)*10;

    //    //triangles
    //    int[] tris = new int[6];

    //    //1st triangle
    //    tris[0] = 0;
    //    tris[1] = 3;
    //    tris[2] = 1;

    //    //2nd triangle
    //    tris[3] = 0;
    //    tris[4] = 2;
    //    tris[5] = 3;

    //    //set values
    //    mesh.vertices = verts;
    //    mesh.triangles = tris;
    //    mesh.RecalculateNormals();


    //    return mesh;
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = GenerateDonutMesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

}
