using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BezierRoad : MonoBehaviour
{
    public GameObject[] points;

    //2d cross section of the road
    public Mesh2D crossSection;

    public bool connector = false;

    [Range(0f, 1f)]
    public float t = 0.0f;

    public int segments => connector ? points.Length : points.Length - 1;

    [Range(0, 500)]
    public int roadSegments = 100;

    public bool SpheresON = false;

    public bool LinesON = false;

    [Range(0f, 10f)]
    public float roadScaler = 1.0f;

    //our mesh
    private Mesh mesh;




    private int getSegment()
    {
        return Mathf.FloorToInt(segments * t) < segments ? Mathf.FloorToInt(segments * t) : segments - 1;
    }

    private float adjustTvalue(int segment)
    {
        return (t - ((float)segment / (float)segments)) / (1.0f / (float)segments);
    }

    private Vector3 getBezierPoint(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        //interpolate 1st stage
        Vector3 X = (1 - t) * A + t * B;
        Vector3 Y = (1 - t) * B + t * C;
        Vector3 Z = (1 - t) * C + t * D;

        //interpolate 2nd stage
        Vector3 P = (1 - t) * X + t * Y;
        Vector3 Q = (1 - t) * Y + t * Z;

        return (1 - t) * P + t * Q;
    }

    Vector3 getBezierForwardVector(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        //interpolate 1st stage
        Vector3 X = (1 - t) * A + t * B;
        Vector3 Y = (1 - t) * B + t * C;
        Vector3 Z = (1 - t) * C + t * D;

        //interpolate 2nd stage
        Vector3 P = (1 - t) * X + t * Y;
        Vector3 Q = (1 - t) * Y + t * Z;

        return (Q - P).normalized;
    }


    //-----------------------------------------------------------------------------------------------------
    // The road mesh ***************************************************************************
    //-----------------------------------------------------------------------------------------------------
    void GenerateRoadMesh()
    {
        if (mesh == null)
            mesh = new Mesh();
        else
            mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        List<Vector2> uvs = new();

        int segmentsPerBezier = roadSegments / segments;

        //store previous road cross-section
        Vector3[] prevRoadPoints = null;

        for (int i = 0; i < segments; i++)
        {
            int nextIndex = (i + 1) % points.Length; //looping back

            Vector3 _Apoint = points[i].GetComponent<BezierPoint>().getAnchor();
            Vector3 _Bpoint = points[i].GetComponent<BezierPoint>().getControl2();
            Vector3 _Cpoint = points[nextIndex].GetComponent<BezierPoint>().getControl1();
            Vector3 _Dpoint = points[nextIndex].GetComponent<BezierPoint>().getAnchor();

            for (int j = 0; j <= segmentsPerBezier; j++)
            {
                float t = (float)j / segmentsPerBezier;
                Vector3 bezPoint = getBezierPoint(_Apoint, _Bpoint, _Cpoint, _Dpoint, t);
                Vector3 forw = getBezierForwardVector(_Apoint, _Bpoint, _Cpoint, _Dpoint, t);

                Vector3 right = Vector3.Cross(Vector3.up, forw).normalized;
                Vector3 up = Vector3.Cross(forw, right).normalized;

                Vector3[] roadPoints = new Vector3[crossSection.vertices.Length];

                for (int k = 0; k < crossSection.vertices.Length; k++)
                {
                    Vector3 point = crossSection.vertices[k].point.x * right + crossSection.vertices[k].point.y * up;
                    point *= roadScaler;
                    point += bezPoint;

                    roadPoints[k] = point;
                    uvs.Add(new Vector2(crossSection.vertices[k].u.x, t));
                    vertices.Add(point);
                }

                //connect current roadPoints to previousRoadPoints with triangles
                if (prevRoadPoints != null)
                {
                    for (int k = 0; k < roadPoints.Length - 1; k++)
                    {
                        int baseIndex = vertices.Count - crossSection.vertices.Length;
                        int upperLeft = baseIndex + k;
                        int upperRight = upperLeft + 1;
                        int lowerLeft = upperLeft - crossSection.vertices.Length;
                        int lowerRight = upperRight - crossSection.vertices.Length;

                        //triangle 1
                        triangles.Add(lowerLeft);
                        triangles.Add(upperLeft);
                        triangles.Add(upperRight);

                        //triangle 2
                        triangles.Add(lowerLeft);
                        triangles.Add(upperRight);
                        triangles.Add(lowerRight);
                    }

                    //closing
                    int prevLast = vertices.Count - 1;
                    int prevFirst = prevLast - (crossSection.vertices.Length - 1);
                    int last = prevLast - crossSection.vertices.Length;
                    int first = last - (crossSection.vertices.Length - 1);

                    triangles.Add(last);
                    triangles.Add(prevLast);
                    triangles.Add(prevFirst);

                    triangles.Add(last);
                    triangles.Add(prevFirst);
                    triangles.Add(first);
                }

                prevRoadPoints = roadPoints;
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();
    }




    private void OnDrawGizmos()
    {
        GenerateRoadMesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        if (points == null || points.Length < 2)
        {
            return; //error prevention
        }

        //draw the bezier segments
        for (int i = 0; i < segments; i++)
        {
            int nextIndex = (i + 1) % points.Length;  //connector

            Vector3 A = points[i].GetComponent<BezierPoint>().getAnchor();
            Vector3 B = points[i].GetComponent<BezierPoint>().getControl2();
            Vector3 C = points[nextIndex].GetComponent<BezierPoint>().getControl1();
            Vector3 D = points[nextIndex].GetComponent<BezierPoint>().getAnchor(); //3 + 1 % points-lenght(aka 4) = 0 -> to the loop beginning  //connector

            Handles.DrawBezier(A, D, B, C, Color.blue, null, 2f);
        }

        //FOR THE BALLLLLL

        int segment = getSegment();
        if (segment >= segments)  //t=1, segment should be segment-1
        {
            segment = segments - 1;  //connector
        }

        //the value of t on the current segment
        float myTvalue = adjustTvalue(segment);

        int nextSegment = (segment + 1) % points.Length;   //connector

        //get the points
        Vector3 Apoint = points[segment].GetComponent<BezierPoint>().getAnchor();
        Vector3 Bpoint = points[segment].GetComponent<BezierPoint>().getControl2();
        Vector3 Cpoint = points[nextSegment].GetComponent<BezierPoint>().getControl1();  //connector
        Vector3 Dpoint = points[nextSegment].GetComponent<BezierPoint>().getAnchor();

        Vector3 bezPoint = getBezierPoint(Apoint, Bpoint, Cpoint, Dpoint, myTvalue);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bezPoint, 3f);

        ///END THE BALLLL
        


        Vector3?[] previousRoadPoint = new Vector3?[crossSection.vertices.Length]; //connecting
        for (int i = 0; i < previousRoadPoint.Length; i++)
        {
            previousRoadPoint[i] = null;
        }

        bool isFirstSegment = true;

        for (int i = 0; i < segments; i++ )
        {
            int segmentsPerBezier = roadSegments / segments;

            for (int j = 0; j <= segmentsPerBezier; j++)
            {
                float _segmentT = (float)j / (float)segmentsPerBezier;

                int nextIndex = (i + 1) % points.Length;

                Vector3 _Apoint = points[i].GetComponent<BezierPoint>().getAnchor();
                Vector3 _Bpoint = points[i].GetComponent<BezierPoint>().getControl2();
                Vector3 _Cpoint = points[nextIndex].GetComponent<BezierPoint>().getControl1();  //connector
                Vector3 _Dpoint = points[nextIndex].GetComponent<BezierPoint>().getAnchor();

                //get bezpoint here
                Vector3 _bezPoint = getBezierPoint(_Apoint, _Bpoint, _Cpoint, _Dpoint, _segmentT);

                //get forward, right and up vectors here 

                //get the forward vector
                Vector3 forw = getBezierForwardVector(_Apoint, _Bpoint, _Cpoint, _Dpoint, _segmentT);
                //MyDraw.DrawVectorAt(bezPoint, 30.0f * forw, Color.blue, 5.0f);

                //get vector3.up to get the "right vector2
                Vector3 right = Vector3.Cross(Vector3.up, forw);
                //MyDraw.DrawVectorAt(bezPoint, 30f * right, Color.red, 3f);

                //use the forward vector and "right" to get correct "up" vector
                Vector3 up = Vector3.Cross(forw, right);
                //MyDraw.DrawVectorAt(bezPoint, 30f * up, Color.green, 3f);


                Vector3[] roadPoints = new Vector3[crossSection.vertices.Length]; //place to hold the point locations?
                for (int k = 0; k < crossSection.vertices.Length; k++)
                {
                    //2d point x-coord times right vector + y-coord times up vector
                    Vector3 point = crossSection.vertices[k].point.x * right + crossSection.vertices[k].point.y * up;

                    point *= roadScaler;

                    //add bezier point to the above
                    point += _bezPoint;

                    if (SpheresON == true)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(point, 0.5f);
                    }

                    roadPoints[k] = point; //store the roadpoint?
                }

                for (int k = 0; k < roadPoints.Length - 1; k++)
                {
                    if (LinesON)
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(roadPoints[k], roadPoints[k + 1]);

                        if (previousRoadPoint[k] != null)
                            Gizmos.DrawLine(roadPoints[k], (Vector3)previousRoadPoint[k]);
                        previousRoadPoint[k] = roadPoints[k];
                    }
                    
                }
                //Gizmos.DrawLine(roadPoints[roadPoints.Length - 1], roadPoints[0]);

                if (!isFirstSegment && LinesON)
                {
                    Gizmos.DrawLine(roadPoints[roadPoints.Length - 1], (Vector3)previousRoadPoint[roadPoints.Length - 1]);
                    previousRoadPoint[roadPoints.Length - 1] = roadPoints[roadPoints.Length - 1];
                }

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //GenerateRoadMesh();
        //GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
