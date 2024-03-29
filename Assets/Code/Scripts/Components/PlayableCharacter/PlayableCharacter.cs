using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayableCharacter
{
    [SerializeField] public string SkinName;
    [SerializeField] public Transform SkinPrefabTransform;
    [SerializeField] public int LevelIndex;
    [SerializeField] public int SkinPrize = 100;
    [SerializeField] public bool Unlocked = false;
}
