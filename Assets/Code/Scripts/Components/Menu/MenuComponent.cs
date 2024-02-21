using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuComponent : MonoBehaviour
{
    [Tooltip("Animate function will be called after this delay ends")]
    [SerializeField] private float AnimationStartDelay = 0f;
    [Tooltip("List Of animation that will be shown at the end of animation start delay")]
    [SerializeField] private List<UI_Animator> StartAnimationList;
    [Tooltip("List Of animation that will be shown at the hide menu")]
    [SerializeField] private List<UI_Animator> EndAnimationList;

    WaitForSeconds WaitTime;

    private void Awake()
    {
        WaitTime = new WaitForSeconds(AnimationStartDelay);
    }

    public void StartAnimation()
    {
        if (gameObject.activeSelf)
            StartCoroutine(AnimateCoroutine());
    }

    public void EndAnimation()
    {
        if (gameObject.activeSelf)
            StartCoroutine(EndAnimateCoroutine());
    }

    private IEnumerator AnimateCoroutine()
    {
        if (StartAnimationList == null)
            StopAllCoroutines();

        yield return WaitTime;

        for (int i = 0; i < StartAnimationList.Count; i++)
        {
            StartCoroutine(StartAnimationList[i].StartAnimation());
        }
    }

    private List<IEnumerator> animations = new List<IEnumerator>();
    private IEnumerator EndAnimateCoroutine()
    {
        //wait
        yield return WaitTime;

        if (EndAnimationList != null)
        {
            foreach (var animation in EndAnimationList)
                animations.Add(animation.StartAnimation());
            //wait the end of all animations
            yield return WaitForAll(animations);

            gameObject.SetActive(false);
            StopAllCoroutines();
        }
    }

    public IEnumerator WaitForAll(List<IEnumerator> coroutines)
    {
        int count = 0;

        foreach (IEnumerator coroutine in coroutines)
        {
            StartCoroutine(RunCoroutine(coroutine));
        }

        while (count > 0)
        {
            yield return null;
        }

        IEnumerator RunCoroutine(IEnumerator c)
        {
            count++;
            yield return StartCoroutine(c);
            count--;

        }
    }
}
