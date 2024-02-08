using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkinSelectorManager : MonoBehaviour
{
    [SerializeField] List<PlayableCharacter> Skins;
    [SerializeField] int VisibleSkins = 2;
    [SerializeField] float SkinBounds = 5f;
    int CurrentIndex;

    private void Awake()
    {
        CurrentIndex = 0;
    }

    public void IncrementIndex()
    {
        CurrentIndex++;
        if (CurrentIndex > Skins.Count)
            CurrentIndex--;
    }

    public void DecrementIndex()
    {
        CurrentIndex--;
        if (CurrentIndex < Skins.Count)
            CurrentIndex--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(SkinBounds, 7f, 0f));
    }
}
