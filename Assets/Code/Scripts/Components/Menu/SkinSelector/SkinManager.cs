using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class SkinManager : MonoBehaviour
{

    public int selectedSkin = 0;

    public TMP_Text TextSkin;

    //public GameObject[] skins;
    [SerializeField] List<PlayableCharacter> Skins = new List<PlayableCharacter> ();

    private void Awake()
    {
        //Debug.Log(Skins.Count);
        if(TextSkin != null)
            TextSkin.text = Skins[selectedSkin].SkinName;
    }

    private void Start()
    {
        for(int i = 1; i < Skins.Count; i++)
        {
            Skins[i].SkinPrefabTransform.gameObject.SetActive(false);
        }
    }

    public Transform GetSkin(int index)
    {
        return Skins[index].SkinPrefabTransform;
    }

    public void NextOption()
    {

        Skins[selectedSkin].SkinPrefabTransform.gameObject.SetActive(false);
        selectedSkin++;
        if (selectedSkin > Skins.Count - 1)
        {
            selectedSkin = 0;
        }
        Skins[selectedSkin].SkinPrefabTransform.gameObject.SetActive(true);
        Debug.Log(selectedSkin);

        TextSkin.text = Skins[selectedSkin].SkinName;
    }

    public void BackOption()
    {
        Skins[selectedSkin].SkinPrefabTransform.gameObject.SetActive(false);
        selectedSkin--;
        if (selectedSkin < 0)
        {
            selectedSkin = Skins.Count - 1;
        }
        Skins[selectedSkin].SkinPrefabTransform.gameObject.SetActive(true);

        Debug.Log(selectedSkin);

        TextSkin.text = Skins[selectedSkin].SkinName;
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("selectedSkin", selectedSkin);
        SceneManager.LoadScene(Skins[selectedSkin].LevelIndex);
    }
}
