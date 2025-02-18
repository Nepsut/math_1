using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Animations;

public class BazierMath : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;
    public GameObject pointD;

    [Range(0f, 1f)]
    public float t = 0.0f;

    private void DrawVector(Vector3 pos, Vector3 vec, Color c, float thickness)
    {
        Handles.color = c;
        Handles.DrawLine(pos, pos + vec, thickness);
    }

    private void OnDrawGizmos()
    {
        Vector3 A = pointA.transform.position;
        Vector3 B = pointB.transform.position;
        Vector3 C = pointC.transform.position;
        Vector3 D = pointD.transform.position;

        //Draw line between A, B, C, D
        Handles.color = Color.white;
        Handles.DrawLine(A, B, 3f);
        Handles.DrawLine(B, C, 3f);
        Handles.DrawLine(C, D, 3f);

        //interpolate 1st stage
        Vector3 X = (1 - t) * A + t * B;
        Vector3 Y = (1 - t) * B + t * C;
        Vector3 Z = (1 - t) * C + t * D;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(X, 0.3f);
        Gizmos.DrawSphere(Y, 0.3f);
        Gizmos.DrawSphere(Z, 0.3f);

        Handles.color = Color.magenta;
        Handles.DrawLine(X, Y, 2f);
        Handles.DrawLine(Y, Z, 2f);


        //interpolate 2nd stage
        Vector3 P = (1 - t) * X + t * Y;
        Vector3 Q = (1 - t) * Y + t * Z;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(P, 0.3f);
        Gizmos.DrawSphere(Q, 0.3f);

        Handles.color = Color.black;
        Handles.DrawLine(P, Q, 2f);

        //interpolate 3rd stage
        Vector3 O = (1 - t) * P + t * Q;

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(O, 0.3f);  //sphere on the bezier

        Handles.DrawBezier(A, D, B, C, Color.white, null, 5.0f);
    }
}


//original code for the whole thing just in case

//private void OnDrawGizmos()
//{
//    Vector3 A = pointA.transform.position;
//    Vector3 B = pointB.transform.position;
//    Vector3 C = pointC.transform.position;
//    Vector3 D = pointD.transform.position;

//    //Draw line between A, B, C, D
//    Handles.color = Color.white;
//    Handles.DrawLine(A, B, 3f);
//    Handles.DrawLine(B, C, 3f);
//    Handles.DrawLine(C, D, 3f);

//    //interpolate 1st stage
//    Vector3 X = (1 - t) * A + t * B;
//    Vector3 Y = (1 - t) * B + t * C;
//    Vector3 Z = (1 - t) * C + t * D;

//    Gizmos.color = Color.red;
//    Gizmos.DrawSphere(X, 0.3f);
//    Gizmos.DrawSphere(Y, 0.3f);
//    Gizmos.DrawSphere(Z, 0.3f);

//    Handles.color = Color.magenta;
//    Handles.DrawLine(X, Y, 2f);
//    Handles.DrawLine(Y, Z, 2f);


//    //interpolate 2nd stage
//    Vector3 P = (1 - t) * X + t * Y;
//    Vector3 Q = (1 - t) * Y + t * Z;

//    Gizmos.color = Color.green;
//    Gizmos.DrawSphere(P, 0.3f);
//    Gizmos.DrawSphere(Q, 0.3f);

//    Handles.color = Color.black;
//    Handles.DrawLine(P, Q, 2f);

//    //interpolate 3rd stage
//    Vector3 O = (1 - t) * P + t * Q;

//    Gizmos.color = Color.white;
//    Gizmos.DrawSphere(O, 0.3f);

//    Handles.DrawBezier(A, D, B, C, Color.white, null, 5.0f);
//}
