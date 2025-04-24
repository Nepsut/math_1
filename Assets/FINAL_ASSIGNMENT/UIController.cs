using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float t = 0.0f;

    public GameObject fadeInPanel;
    public float fadeTime = 2.0f;

    public EasingFunction.Ease fadeEase = EasingFunction.Ease.EaseInOutQuad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fade()
    {
        //FADE IN PANEL COLOR WHEN CHANGING CAMERA

        //t = Time.time / fadeTime;

        //if (t > 1f) t = 1f;
        //EasingFunction.Function fadeFunc = EasingFunction.GetEasingFunction(fadeEase);
        //t = fadeFunc(0, 1, t);

        //Color currentColor = fadeInPanel.GetComponent<Image>().color;
        //currentColor.a = Mathf.Lerp(0, 1, t); //interpolate alpha from 0 to 1
        //fadeInPanel.color = currentColor;

        //float scaler = Mathf.Lerp(0.5f, 1.0f, t);
        //fadeInPanel.transform.localScale = new Vector3(scaler, scaler, 1f);
    }
}
