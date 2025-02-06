using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WedgeTrigger : MonoBehaviour
{
    //modify at the end
    [Range(0.1f, 20f)]
    public float radius = 5.0f;

    [Range(1, 180f)]
    public float threshold = 45f;

    public GameObject targetObject;
    public GameObject lookAtObject;

    private Color orange = new Color32(0xF0, 0xA5, 0x00, 0xA0);

    private void OnDrawGizmos()
    {
        //positions and vectors (base)
        Vector3 vecTriggerPos = transform.position;
        Vector3 vecTargetPos = targetObject.transform.position;
        Vector3 v = vecTargetPos - vecTriggerPos;  //vector
        Vector3 l = lookAtObject.transform.position - vecTriggerPos;

        //normalized vectors (base)
        Vector3 normalizedV = v.normalized;
        Vector3 normalizedL = l.normalized;

        //draw vectors
        MyDraw.DrawVectorAt(vecTriggerPos, v, Color.magenta, 3.0f);
        MyDraw.DrawVectorAt(vecTriggerPos, l, orange, 4.0f);  //0x in the beginning

        //maths
        float dotProduct = Vector3.Dot(normalizedV, normalizedL);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        //check if player is in the area
        Color playerColor = angle >= threshold ? Color.green : Color.red;

        if (v.magnitude <= radius && angle < threshold)
        {
            Handles.color = Color.red;
        } else Handles.color = Color.green;

        //enemy look direction/area
        Quaternion rotate = Quaternion.Euler(0, threshold, 0);
        Vector3 rightRotation = rotate * normalizedL;  //right border
        MyDraw.DrawVectorAt(transform.position, rightRotation * radius, playerColor);

        rotate = Quaternion.Euler(0, -threshold, 0);
        Vector3 leftRotation = rotate * normalizedL;  //left border
        MyDraw.DrawVectorAt(transform.position, leftRotation * radius, playerColor);


        //the cheese piece -og-
        Handles.DrawWireArc(vecTriggerPos, new Vector3(0, 1, 0), leftRotation, threshold * 2, radius, 3.0f);
    }
}

