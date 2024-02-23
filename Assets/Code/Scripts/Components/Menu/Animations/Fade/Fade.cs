using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : UI_Animator
{
    [SerializeField] private CanvasGroup ObjectToAnimate;
    [SerializeField] private AnimationCurve FadeCurve;
    [SerializeField] private float FadeSpeed = 1f;
    [SerializeField][Range(0f, 1f)] private float StartValue = 0f;
    [SerializeField][Range(0f, 1f)] private float EndValue = 1f;
    [Tooltip("Dellay time for the animation")]
    [SerializeField] private float StartAnimationAfterSec = 0f;
    private void Start()
    {
        if (ObjectToAnimate == null)
        {
            enabled = false;
            return;
        }
        ObjectToAnimate.alpha = StartValue;
    }

    public override IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(StartAnimationAfterSec);
        yield return FadeAnimation();
    }

    private IEnumerator FadeAnimation()
    {
        float progress = 0f;
        float curveProgress = 0f;
        while (progress < 1f)
        {
            float colorFade = Mathf.Lerp(StartValue, EndValue, curveProgress);
            ObjectToAnimate.alpha = colorFade;

            //get animated fade based on animation curve
            float animatedMovement = FadeCurve.Evaluate(curveProgress);
            //update progresses
            progress += Time.deltaTime * FadeSpeed * animatedMovement;
            curveProgress += Time.deltaTime * FadeSpeed;
            yield return null;
        }
    }
}
