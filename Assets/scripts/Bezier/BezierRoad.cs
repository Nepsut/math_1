using System.Collections;
using System.Collections.Generic;
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
    public int roadSegments = 100;  //FOR HOMEWORK FRAMES(2D CUT SEGMENTS)




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

    private void OnDrawGizmos()
    {
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

            Handles.DrawBezier(A, D, B, C, Color.white, null, 2f);
        }

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
        Gizmos.DrawSphere(bezPoint, 0.5f);

        //get the forward vector
        Vector3 forw = getBezierForwardVector(Apoint, Bpoint, Cpoint, Dpoint, myTvalue);
        MyDraw.DrawVectorAt(bezPoint, 30.0f * forw, Color.blue, 5.0f);

        //get vector3.up to get thr "right vector2
        Vector3 right = Vector3.Cross(Vector3.up, forw);
        MyDraw.DrawVectorAt(bezPoint, 30f * right, Color.red, 3f);

        //use the forward vector and "right" to get correct "up" vector
        Vector3 up = Vector3.Cross(forw, right);
        MyDraw.DrawVectorAt(bezPoint, 30f * up, Color.green, 3f);

        //draw the points using the cross section of the road
        for (int i = 0; i < crossSection.vertices.Length; i++)
        {
            //2d point x-coord times right vector + y-coord times up vector
            Vector3 point = crossSection.vertices[i].point.x * right + crossSection.vertices[i].point.y * up;

            //add bezier point to the above
            point += bezPoint;

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(point, 0.5f);
        }


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
