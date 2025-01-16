using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BasicMath : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float axisLenght = 3f;

    public GameObject vectorTo;

    private Vector3 vec;

    //rectagle
    public GameObject rect;
    //[Range(0.1f, 20f)]                        HOX FIXED!!
    public float width = 1920f;
    //[Range(0.1f, 20f)]
    public float height = 1080f;

    [Range(0f, 100f)]
    public float widthPercentage = 60f;  //homework start?
    [Range(0f, 100f)]
    public float heightPercentage = 35f;  //homework start?

    [Range(0f, 100f)]
    public float positionPercentageX = 50f;  //homework start?
    [Range(0f, 100f)]
    public float positionPercentageY = 50f;  //homework start?


    private void DrawVector(Vector3 pos, Vector3 vec, Color c, float thickness)
    {
        Handles.color = c;
        Handles.DrawLine(pos, pos + vec, thickness);
        Handles.ConeHandleCap(0, pos + vec - 0.20f*vec.normalized, Quaternion.LookRotation(vec), 0.3f, EventType.Repaint);
    }

    private void DrawRect(Vector3 pos, float width, float height, Color c, float thickness)
    {
        //rectangle
        Handles.color = c;
        Handles.DrawLine(pos, pos + new Vector3(width, 0, 0), thickness);
        Handles.DrawLine(pos, pos + new Vector3(0, height, 0), thickness);
        Handles.DrawLine(pos + new Vector3(width, height, 0), pos + new Vector3(width, 0, 0), thickness);
        Handles.DrawLine(pos + new Vector3(0, height, 0), pos + new Vector3(width, height, 0), thickness);
    }

    private void DrawAxes(Vector3 pos, float length, float thickness)
    {
        //x
        DrawVector(pos, new Vector3(length, 0, 0), Color.red, thickness);

        //y
        DrawVector(pos, new Vector3(0, length, 0), Color.green, thickness);
    }

    private void DrawPopUp()
    {
        //homework start?
    }

    private void OnDrawGizmos()
    {
        vec = vectorTo.transform.position;  //endpoint of the targetA

        DrawAxes(Vector3.zero, axisLenght, 3f);

        //circle
        Handles.color = Color.white;
        Handles.DrawWireDisc(Vector3.zero, Vector3.forward, 1.5f);

        //line to object
        DrawVector(Vector3.zero, vec, Color.black, 3f);
        DrawAxes(vec, 1.0f, 3f);

        ////rectangle
        //vec = rect.transform.position;
        //DrawRect(vec, width, height, Color.black, 3.0f);
        //DrawAxes(vec, 1.0f, 3f);

        //homework frame
        DrawRect(Vector3.zero, width, height, Color.black, 5f);

    }
}
