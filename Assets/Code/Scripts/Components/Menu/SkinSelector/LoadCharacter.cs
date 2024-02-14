using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] CharacterPrefab;
    public Transform SpawnPoint;
    void Start()
    {
        int selectedSkin = PlayerPrefs.GetInt("selectedSkin");
        GameObject prefab = CharacterPrefab[selectedSkin];
        GameObject clone = Instantiate(prefab, SpawnPoint.position, Quaternion.identity);

    }
}

