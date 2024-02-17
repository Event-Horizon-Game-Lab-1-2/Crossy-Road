using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField] SkinManager skinManager;

    void Start()
    {
        int selectedSkin = PlayerPrefs.GetInt("selectedSkin");
        Transform prefab = skinManager.GetSkin(selectedSkin);
        GameObject clone = Instantiate(prefab.gameObject, transform.position, Quaternion.identity);
        clone.SetActive(true);
        //gameObject.transform.SetParent(clone.transform);
        clone.GetComponent<AnimationComponent>().Target = transform;
    }
}