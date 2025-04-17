using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIpopUps : MonoBehaviour
{
    //objects

    public Image fadeImage;
    public float fadeTime = 2.0f;

    public GameObject circle;
    public float moveTime = 2.0f;

    public GameObject panel;
    public float slideTime = 2.0f;

    [Range(0f, 1f)]
    public float t = 0.0f;

    //choose easing - dropdown

    public EasingFunction.Ease fadeEase = EasingFunction.Ease.EaseInOutQuad;
    public EasingFunction.Ease bounceEase = EasingFunction.Ease.EaseOutBounce;
    public EasingFunction.Ease slideEase = EasingFunction.Ease.EaseOutQuad;

    //positions

    private Vector3 bouncePosition;
    private Vector3 slideStartPosition;

    [SerializeField] private Vector3 rotation;


    void Start()
    {
        //initial positions
        bouncePosition = circle.transform.position;
        slideStartPosition = panel.transform.position;
        panel.transform.position = new Vector2(slideStartPosition.x, slideStartPosition.y - 500f);
    }

    // Update is called once per frame
    void Update()
    {
        Fade();

        Bounce();

        Slide();
    }

    void Fade()
    {
        t = Time.time / fadeTime;

        if (t > 1f) t = 1f;
        EasingFunction.Function fadeFunc = EasingFunction.GetEasingFunction(fadeEase);
        t = fadeFunc(0, 1, t);

        Color currentColor = fadeImage.color;
        currentColor.a = Mathf.Lerp(0, 1, t); //interpolate alpha from 0 to 1
        fadeImage.color = currentColor;

        float scaler = Mathf.Lerp(0.5f, 1.0f, t);
        fadeImage.transform.localScale = new Vector3(scaler, scaler, 1f);
    }

    void Bounce()
    {
        circle.transform.Rotate(rotation * Time.deltaTime);

        t = Time.time / moveTime;

        EasingFunction.Function bounceFunc = EasingFunction.GetEasingFunction(bounceEase);
        t = bounceFunc(0, 1, t);

        Vector3 targetPosition = new Vector3(bouncePosition.x, bouncePosition.y + Mathf.Lerp(-35f, 10f, t));
        circle.transform.position = targetPosition; //bounce applied to target
    }

    void Slide()
    {
        t = Time.time / slideTime;

        if (t > 1f) t = 1f;
        EasingFunction.Function slideFunc = EasingFunction.GetEasingFunction(slideEase);
        t = slideFunc(0, 1, t);

        Vector3 slideTargetPosition = new Vector3(slideStartPosition.x, Mathf.Lerp(slideStartPosition.y - 500f, slideStartPosition.y, t));
        panel.transform.position = slideTargetPosition; //slide applied to target
    }
}
