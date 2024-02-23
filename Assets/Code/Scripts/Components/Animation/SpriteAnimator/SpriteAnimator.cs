using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private RawImage Renderer;
    
    [SerializeField] private Texture[] TextureList;
    [SerializeField] private int Framerate = 30;
    [SerializeField] private bool Loop = true;
    [SerializeField] private bool DestroyOnEnd = true;
    //frame time
    private float Frequency;

    private void Awake()
    {
        Frequency = 1 / (float)Framerate;
    }
    
    IEnumerator Animate()
    {
        int i = 0;
        do
        {
            while (i < TextureList.Length)
            {
                Renderer.texture = TextureList[i];
                i++;
                yield return new WaitForSeconds(Frequency);
            }
            i = 0;
        } while (Loop);
        if (DestroyOnEnd)
            Destroy(this.gameObject);
    }

    public void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
