using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    public GameObject control1;
    public GameObject control2;

    public bool drawLines = true;
    public bool drawPoints = true;

    public Vector3 getAnchor()
    {
        return transform.position;
    }

    public Vector3 getControl1()
    {
        return control1.transform.position;
    }

    public Vector3 getControl2()
    {
        return control2.transform.position;
    }

    private void OnDrawGizmos()
    {
        if (drawLines)
        {
            Handles.color = Color.white;
            Handles.DrawLine(transform.position, control1.transform.position, 2f);
            Handles.DrawLine(transform.position, control2.transform.position, 2f);

        }

        if(drawPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(control1.transform.position, 1.5f);
            Gizmos.DrawSphere(control2.transform.position, 1.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1.5f);
        }
    }

    //!!!!-----IM SORRY FOR THE COMMENTS IT HELPS ME LEARN AND REMEMBER (kind of unreadable though)-----!!!!

    //for dragging the controls

    //custom editor instead of default one
    [CustomEditor(typeof(BezierPoint))]
    public class BezierPointEditor : Editor
    {
        //overriding editor's own OnSceneGUI?, gizmos,editor
        private void OnSceneGUI()               
        {
            BezierPoint bezierPoint = (BezierPoint)target;  //->ref to object currently shown in editor

            EditorGUI.BeginChangeCheck();  //monitors changes to any values within the editor

            Vector3 newAnchorPos = Handles.PositionHandle(bezierPoint.transform.position, Quaternion.identity);  //handle in point's current pos, movable in 3D space

            if (EditorGUI.EndChangeCheck())  //checks if any changes were made during the last interaction (anchor), if so ->
            {
                Vector3 direction = bezierPoint.control1.transform.position - bezierPoint.transform.position;  //calc. vector between anchor and control
                bezierPoint.transform.position = newAnchorPos;  //new anchor pos based on movement
                bezierPoint.control1.transform.position = bezierPoint.transform.position + direction;  //maintains position with anchor

                direction = bezierPoint.control2.transform.position - bezierPoint.transform.position;  //calc. vec. between ctrl2 and anc.
                bezierPoint.control2.transform.position = bezierPoint.transform.position - direction;  //maintains position with anchor but opposite dir
            }

            EditorGUI.BeginChangeCheck();  //changes to control 1
            Vector3 newControl1Position = Handles.PositionHandle(bezierPoint.control1.transform.position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())  //was ctrl1 moved?
            {
                Vector3 control1ToAnchor = bezierPoint.control1.transform.position - bezierPoint.transform.position;  //direction from 1 to anc
                bezierPoint.control1.transform.position = newControl1Position;
                bezierPoint.control2.transform.position = bezierPoint.transform.position - control1ToAnchor;  //moves ctrl2 ro maintain relative position (opposite)
            }

            EditorGUI.BeginChangeCheck();  //changes to control 2
            Vector3 newControl2Position = Handles.PositionHandle(bezierPoint.control2.transform.position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())  //was ctrl2 moved?
            {
                Vector3 control2ToAnchor = bezierPoint.control2.transform.position - bezierPoint.transform.position;
                bezierPoint.control2.transform.position = newControl2Position;
                bezierPoint.control1.transform.position = bezierPoint.transform.position - control2ToAnchor;
            }
        }
        
    }
}
