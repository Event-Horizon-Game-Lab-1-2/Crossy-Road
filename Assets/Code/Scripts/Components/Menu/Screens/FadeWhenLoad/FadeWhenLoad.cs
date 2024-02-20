using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWhenLoad : MonoBehaviour
{
    [Tooltip("Animate function will be called after this delay ends")]
    [SerializeField] private float AnimationStartDelay = 0f;
    [Tooltip("List Of animation that will be shown at the end of animation start delay")]
    [SerializeField] private List<UI_Animator> StartAnimationList;
    [Tooltip("List Of animation that will be shown at the hide menu")]
    [SerializeField] private List<UI_Animator> EndAnimationList;

    private void Start()
    {
        Animate();
    }

    public void Animate()
    {
        StartCoroutine(AnimateCoroutine());
    }

    public void End()
    {
        StartCoroutine(EndAnimateCoroutine());
    }

    private IEnumerator AnimateCoroutine()
    {
        if (StartAnimationList == null)
            StopAllCoroutines();

        yield return new WaitForSeconds(AnimationStartDelay);
        for (int i = 0; i < StartAnimationList.Count; i++)
        {
            StartAnimationList[i].StartAnimation();
        }
    }

    private IEnumerator EndAnimateCoroutine()
    {
        if (EndAnimationList == null)
            StopAllCoroutines();

        yield return new WaitForSeconds(AnimationStartDelay);
        for (int i = 0; i < EndAnimationList.Count; i++)
        {
            EndAnimationList[i].StartAnimation();
        }
    }
}
