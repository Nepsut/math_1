using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Interpolation : MonoBehaviour
{
    public GameObject objA;
    public GameObject objB;
    public GameObject cube;

    [Range(0f, 1f)]
    public float t = 0.0f;

    public float interpTime = 5.0f;

    public EasingFunction.Ease ease = EasingFunction.Ease.EaseInOutQuad;

    private Vector3 posA = Vector3.zero;
    private Vector3 posB = Vector3.zero;

    private void DrawVector(Vector3 pos, Vector3 vec, Color c, float thickness)
    {
        Handles.color = c;
        Handles.DrawLine(pos, pos + vec, thickness);
        Handles.ConeHandleCap(0, pos + vec - 0.20f * vec.normalized, Quaternion.LookRotation(vec), 0.3f, EventType.Repaint);
    }

    private void OnDrawGizmos()
    {
        posA = objA.transform.position;
        posB = objB.transform.position;

        //Draw vectors from origin to A and B
        DrawVector(Vector3.zero,posA, Color.blue, 4f);
        DrawVector(Vector3.zero, posB, Color.red, 4f);

        //Draw like between A & B
        Handles.color = Color.white;
        Handles.DrawLine(posA, posB, 3f);


        //interpolation: X = (1-t)A + tB    //(X, A, B = vectors)
        Vector3 tmpA = (1 - t) * posA;
        Vector3 tmpB = t * posB;

        //part A
        DrawVector(Vector3.zero, tmpA, Color.magenta, 3f);
        //part B
        DrawVector(tmpA,tmpB, Color.magenta, 3f);

        //Vector3 posX = tmpA + tmpB;
        //cube.transform.position = posX;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get positions a & b
        posA = objA.transform.position;
        posB = objB.transform.position;

        //InterpTime = 5.0f;

        //compute t value
        t = Time.time / interpTime;

        //limit t to max 1
        if (t> 1f) t = 1f;


        //easing
        //t = t * t * t;

        //EasingFunction class
        EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);
        t = func(0,1,t);

        //interpolate
        Vector3 pos = (1 - t) * posA + t * posB;
        //Vector3 pos = Vector3.Lerp(posA, posB, t);

        //move the game obj
        cube.transform.position= pos;
    }
}
