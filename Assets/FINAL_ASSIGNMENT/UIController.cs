using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //FADE

    [Range(0f, 1f)]
    public float t = 0.0f; //progress of the fade (0 = invisible, 1 = fully visible)

    public Image fadeInImage; 
    public float fadeTime = 2.0f; 

    public EasingFunction.Ease fadeEase = EasingFunction.Ease.EaseInOutQuad; //easing function

    private bool isFading = false; //track whether the fade has started

    //CAMERA
    public GameObject mainCamera;
    public GameObject carCamera;
    public int camManager;

    void Update()
    {
        if (isFading)
        {
            Fade();
        }
    }

    void StartFade()
    {
        isFading = true;
        t = 0f;
    }

    void Fade()
    {
        t += Time.deltaTime / fadeTime;

        //ensure t stays between 0 and 1
        if (t > 1f)
        {
            t = 1f;
            isFading = false; //stop fading once we've reached full visibility
        }

        EasingFunction.Function fadeFunc = EasingFunction.GetEasingFunction(fadeEase);
        float easedT = fadeFunc(1, 0, t); //apply easing to t

        Color currentColor = fadeInImage.color;
        currentColor.a = easedT;
        fadeInImage.color = currentColor;
    }

    public void CameraButtonClick()
    {
        StartFade();
        ManageCamera();
    }

    public void ManageCamera()
    {
        if (camManager == 0)
        {
            camM();
            camManager = 1;
        }
        else
        {
            camC();
            camManager = 0;
        }
    }

    void camM()
    {
        mainCamera.SetActive(true);
        carCamera.SetActive(false);
    }
    void camC()
    {
        mainCamera.SetActive(false);
        carCamera.SetActive(true);
    }
}

