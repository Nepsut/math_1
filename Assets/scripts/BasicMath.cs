using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEditor.PlayerSettings;

public class BasicMath : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float axisLenght = 3f;

    public GameObject vectorTo;

    private Vector3 vec;

    //rectagle _SCREEN SIZE_
    public GameObject rect;
    [Range(0.1f, 3840f)]                        
    public float width = 1920f;
    [Range(0.1f, 2160f)]
    public float height = 1080f;

    [Range(0f, 1f)]
    public float widthPercentage = 0.6f;  //homework start?
    [Range(0f, 1f)]
    public float heightPercentage = 0.35f;  //homework start?

    [Range(0f, 1f)]
    public float positionPercentageX = 0.5f;  //homework start?
    [Range(0f, 1f)]
    public float positionPercentageY = 0.5f;  //homework start?


    private float popUpX;
    private float popUpY;

    private float popUpWidth;
    private float popUpHeight;

    //private float testX = 1000f;
    //private float testY = 600f;

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

    private void popUpDimensions()
    {
        popUpX = width * positionPercentageX;
        popUpY = height * positionPercentageY;

        popUpWidth = width * widthPercentage;
        popUpHeight = height * heightPercentage;
    }
    private void DrawPopUp()
    {
        //homework start?
    }

    private void OnDrawGizmos()
    {
        DrawAxes(Vector3.zero, axisLenght, 3f);

        ////rectangle
        //vec = rect.transform.position;
        //DrawRect(vec, testX, testY, Color.black, 3.0f);
        //DrawAxes(vec, 1.0f, 3f);

        //homework frame
        DrawRect(Vector3.zero, width, height, Color.black, 5f);

        popUpDimensions();

        Handles.color = Color.red;
        Handles.DrawWireCube(new Vector3(popUpX,popUpY,0), new Vector3(popUpWidth, popUpHeight, 0));

    }
}
