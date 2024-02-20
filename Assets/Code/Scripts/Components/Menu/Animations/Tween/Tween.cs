using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : UI_Animator
{
    [Tooltip("Object that will be animated")]
    [SerializeField] private RectTransform ObjectToAnimate;
    [Tooltip("Movement Curve of the animation")]
    [SerializeField] private AnimationCurve MovementCurve;
    [Tooltip("Animation of the animation")]
    [SerializeField] private float AnimationSpeed = 1f;
    
    [SerializeField] private Vector2 Direction = Vector2.up;
    [SerializeField] private float Distance = 0f;
    private Vector2 RectSize = Vector2.zero;

    private void Start()
    {
        RectSize = ObjectToAnimate.rect.size;
    }

    public override void StartAnimation()
    {
        StartCoroutine(AnimationCoroutine());
    }

    IEnumerator AnimationCoroutine()
    {
        //get target position
        Vector2 targetPos = ObjectToAnimate.anchoredPosition + (Direction * (Distance + RectSize.x));

        float progress = 0f;
        float curveProgress = 0f;
        while(progress < 1f)
        {
            ObjectToAnimate.anchoredPosition = Vector3.Lerp(ObjectToAnimate.anchoredPosition, targetPos, progress);

            //get animated movement based on animation curve
            float animatedMovement = MovementCurve.Evaluate(curveProgress);
            //update progresses
            progress += Time.deltaTime * AnimationSpeed * animatedMovement;
            curveProgress += Time.deltaTime * AnimationSpeed;
            yield return null;
        }
        ObjectToAnimate.anchoredPosition = targetPos;
    }
}
