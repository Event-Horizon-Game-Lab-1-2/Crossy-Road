using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayableCharacter
{
    [SerializeField] string SkinName;
    [SerializeField] Transform SkinPrefabTransform;
    [SerializeField] int LevelIndex;
}
