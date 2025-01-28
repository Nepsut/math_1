using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PopUpHomework : MonoBehaviour
{
    [SerializeField] GameObject bg, ball, textPanel, pointA, pointB;

    private Vector3 posA = Vector3.zero;
    private Vector3 posB = Vector3.zero;

    [Range(0f, 1f)]
    public float t = 0.0f;

    public float interpTime = 5.0f;

    public EasingFunction.Ease ease = EasingFunction.Ease.EaseInOutQuad;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posA = pointA.transform.position;
        posB = pointB.transform.position;

        t = Time.time / interpTime;

        EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);
        t = func(0, 1, t);

        Vector3 pos = Vector3.Lerp(posA, posB, t*t);

        //move the game obj
        bg.transform.position = pos;
    }
}
