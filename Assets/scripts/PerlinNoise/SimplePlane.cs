using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class SimplePlane : MonoBehaviour
{

    [Range(1f, 10000f)]
    public float Size = 1000.0f;

    [Range(10, 255)]
    public int Segments = 100;

    [Range(0.1f, 100f)]
    public float HeightScaler = 10.0f;

    [Range(0.001f, 1f)]
    public float FrequencyScaler = 1.0f;

    public bool UseThresholding = false;

    private Mesh mesh;

    public List<NoiseSettings> noiseSettings = new List<NoiseSettings>();  //layers

    [Range(0f, 100f)]
    public int xShift = 1;
    [Range(0f, 100f)]
    public int yShift = 1;


    private void OnDrawGizmos()
    {
        GenerateMesh();

        float delta = Size / (Segments - 1);

        for (int i = 0; i < Segments; i++)
        {
            for (int j = 0; j < Segments; j++)
            {
                float x = i * delta;
                float y = j * delta;

                Gizmos.color = Color.green;
                //Gizmos.DrawSphere(new Vector3(x, 0, y), .5f);
            }
        }
    }


    void GenerateMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Terrain Mesh";
        }
        else
        {
            mesh.Clear();
        }

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> UVs = new List<Vector3>();
        List<int> tris = new List<int>();


        float delta = Size / (Segments - 1);

        // Generate vertices
        for (int i = 0; i < Segments; i++)
        {
            for (int j = 0; j < Segments; j++)
            {
                float x = i * delta;
                float y = j * delta;

                // Use Perlin Noise
                float noisevalue = 0f;



                for (int k = 0; k < noiseSettings.Count; k++)
                {
                    noisevalue += noiseSettings[k].amplitude * 2.0f * (Mathf.PerlinNoise(noiseSettings[k].frequency * (x + xShift), noiseSettings[k].frequency * (y + yShift)) - 0.5f);
                }

                //Debug.Log(noisevalue);
                if (UseThresholding)
                {
                    if (noisevalue < 0.0f)
                    {
                        noisevalue = 0.0f;
                    }
                }
                float height = HeightScaler * noisevalue;
                // Add the vertex
                vertices.Add(new Vector3(x, height, y));

                //Add UVs
                UVs.Add(new Vector2((x+xShift) / Size, (y+yShift) / Size));

            }
        }

        // Generate triangles
        for (int i = 0; i < Segments - 1; i++)
        {
            for (int j = 0; j < Segments - 1; j++)
            {
                // "Upper left"
                int ul = j * Segments + i;

                // "Upper right"
                int ur = ul + 1;

                // "Lower left"
                int ll = ul + Segments;

                // "Lower right"
                int lr = ll + 1;

                // Triangles:
                tris.Add(ll);
                tris.Add(ul);
                tris.Add(ur);

                tris.Add(ll);
                tris.Add(ur);
                tris.Add(lr);

            }
        }


        mesh.SetVertices(vertices);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, UVs);

        GetComponent<MeshFilter>().sharedMesh = mesh;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
