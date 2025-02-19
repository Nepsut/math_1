using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    public GameObject[] points;
    public bool connector = false;

    [Range(0f, 1f)]
    public float t = 0.0f;

    private int getSegment()
    {
        //number of bezier segments 
        int segments = points.Length - 1;

        float seg = t * segments;

        return Mathf.FloorToInt(seg);
    }

    private float adjustTvalue(int segment)
    {
        int segments = points.Length - 1;

        return (t - ((float)segment /(float)segments)) / (1.0f / (float) segments);
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

    private void OnDrawGizmos()
    {
        if (points == null || points.Length < 2)
        {
            return; //error prevention
        }

        //number of bezier segments 
        int segments = connector ? points.Length : points.Length - 1;  //connector

        //draw the bezier segments
        for (int i = 0; i < segments; i++)
        {
            int nextIndex = (i + 1) % points.Length;  //connector

            Vector3 A = points[i].GetComponent<BezierPoint>().getAnchor();
            Vector3 B = points[i].GetComponent<BezierPoint>().getControl2();
            Vector3 C = points[nextIndex].GetComponent<BezierPoint>().getControl1();
            Vector3 D = points[nextIndex].GetComponent<BezierPoint>().getAnchor(); //3 + 1 % points-lenght(aka 4) = 0 -> to the loop beginning  //connector

            Handles.DrawBezier(A, D, B, C, Color.white, null, 3f);
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
        Gizmos.DrawSphere(bezPoint, 3f);
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
