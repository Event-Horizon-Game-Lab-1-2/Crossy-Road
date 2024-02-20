using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideElement : UI_Animator
{
    [Tooltip("Object that will be animated")]
    [SerializeField] private RectTransform ObjectToAnimate;

    public override void StartAnimation()
    {
        ObjectToAnimate.gameObject.SetActive(false);
    }
}
