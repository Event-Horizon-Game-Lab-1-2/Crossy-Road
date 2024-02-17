using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MenuComponent
{
    [SerializeField] float MenuDelay = 3f;
    [SerializeField] RectTransform Button;
    [SerializeField] float Offset = 200f;
    [SerializeField] Vector2 Direction = Vector2.up;
    [SerializeField] float AnimationSpeed = 1f;

    private Vector2 RectSize;

    private void Start()
    {
        RectSize = Button.rect.size;
    }

    public override void Show()
    {
        Animate();
    }

    public void Animate()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateButton());
    }

    IEnumerator AnimateButton()
    {
        Vector2 targetPos = Button.anchoredPosition + RectSize + (Direction * Offset);
        yield return new WaitForSeconds(MenuDelay);
        float progress = 0f;
        while(Vector2.Distance(Button.anchoredPosition, targetPos) > 0.1f)
        {
            Vector2 pos = Button.anchoredPosition;
            pos += Direction * AnimationSpeed;
            Button.anchoredPosition = pos;
            progress += Time.deltaTime*AnimationSpeed;
            yield return null;
        }
    }
}
