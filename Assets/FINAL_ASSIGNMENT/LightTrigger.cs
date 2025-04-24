using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LightTrigger : MonoBehaviour
{
    //modify at the end
    [Range(0.1f, 35f)]
    public float radius = 0.5f;

    [Range(1, 180f)]
    public float threshold = 45f;

    public GameObject targetObject;
    public GameObject lookAtObject;

    private Color orange = new Color32(0xF0, 0xA5, 0x00, 0xA0);

    public float offPos = 5.0f;


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

        //draw vectors (player & enemy)
        MyDraw.DrawVectorAt(vecTriggerPos, v, Color.magenta, 3.0f); //enemy to player
        MyDraw.DrawVectorAt(vecTriggerPos, normalizedL * radius, orange, 4.0f);  //0x in the beginning //enemy look direction

        //maths
        float dotProduct = Vector3.Dot(normalizedV, normalizedL);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        //enemy look direction/area
        Quaternion rotate = Quaternion.Euler(0, threshold, 0);
        Vector3 rightRotation = rotate * normalizedL;  //right border

        rotate = Quaternion.Euler(0, -threshold, 0);
        Vector3 leftRotation = rotate * normalizedL;  //left border


        //the cheese piece -og-
        Vector3 offsetTriggerPosUp = new(vecTriggerPos.x, vecTriggerPos.y + offPos, vecTriggerPos.z);
        Vector3 offsetTriggerPosDown = new(vecTriggerPos.x, vecTriggerPos.y - offPos, vecTriggerPos.z);

        //check if player is in the area
        if (v.magnitude <= radius && angle < threshold && offsetTriggerPosUp.y > vecTargetPos.y && offsetTriggerPosDown.y < vecTargetPos.y)
        {
            Handles.color = Color.red;
            //gameObject.GetComponentInChildren<Light>().enabled = false;
            StartCoroutine(WaitForSeconds());
            //gameObject.GetComponentInChildren<Light>().enabled = true;


        }
        else Handles.color = Color.white;

        Handles.DrawLine(offsetTriggerPosUp, offsetTriggerPosUp + rightRotation * radius, 3.0f);
        Handles.DrawLine(offsetTriggerPosUp, offsetTriggerPosUp + leftRotation * radius, 3.0f);
        Handles.DrawWireArc(offsetTriggerPosUp, new Vector3(0, 1, 0), leftRotation, threshold * 2, radius, 3.0f);
        Handles.DrawLine(offsetTriggerPosDown, offsetTriggerPosDown + rightRotation * radius, 3.0f);
        Handles.DrawLine(offsetTriggerPosDown, offsetTriggerPosDown + leftRotation * radius, 3.0f);
        Handles.DrawWireArc(offsetTriggerPosDown, new Vector3(0, 1, 0), leftRotation, threshold * 2, radius, 3.0f);

        Handles.DrawLine(offsetTriggerPosDown, offsetTriggerPosUp, 3.0f);
        Handles.DrawLine(offsetTriggerPosUp + rightRotation * radius, offsetTriggerPosDown + rightRotation * radius, 3.0f);
        Handles.DrawLine(offsetTriggerPosUp + leftRotation * radius, offsetTriggerPosDown + leftRotation * radius, 3.0f);
    }

    IEnumerator WaitForSeconds()
    {
        gameObject.GetComponentInChildren<Light>().enabled = false;

        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        yield return new WaitForSeconds(2);

        gameObject.GetComponentInChildren<Light>().enabled = true;

        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

