using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : UI_Animator
{
    [SerializeField] private RawImage ObjectToAnimate;
    [SerializeField] private AnimationCurve FadeCurve;
    [SerializeField] private float FadeSpeed = 1f;
    [SerializeField][Range(0f, 1f)] private float StartValue = 0f;
    [SerializeField][Range(0f, 1f)] private float EndValue = 1f;

    private Color OriginalColor;

    private void Start()
    {
        if (ObjectToAnimate == null)
        {
            enabled = false;
            return;
        }

        OriginalColor = ObjectToAnimate.color;
        ObjectToAnimate.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, StartValue);
    }

    public override void StartAnimation()
    {
        StartCoroutine(FadeAnimation());
    }

    private IEnumerator FadeAnimation()
    {
        float progress = 0f;
        float curveProgress = 0f;
        while (progress < 1f)
        {
            float colorFade = Mathf.Lerp(StartValue, EndValue, curveProgress);
            ObjectToAnimate.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, colorFade);

            //get animated fade based on animation curve
            float animatedMovement = FadeCurve.Evaluate(curveProgress);
            //update progresses
            progress += Time.deltaTime * FadeSpeed * animatedMovement;
            curveProgress += Time.deltaTime * FadeSpeed;
            yield return null;
        }
        ObjectToAnimate.color = OriginalColor;
    }
}
