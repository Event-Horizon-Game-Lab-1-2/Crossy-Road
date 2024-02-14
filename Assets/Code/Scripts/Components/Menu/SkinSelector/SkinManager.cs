using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour
{

    public int selectedSkin = 0;

    //public GameObject[] skins;
    [SerializeField] List<PlayableCharacter> Skins = new List<PlayableCharacter> ();

    private void Awake()
    {
        Debug.Log(Skins.Count);
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
    }

    public void PlayGame()
    {
      
        PlayerPrefs.SetInt("selectedSkin", selectedSkin);
        SceneManager.LoadScene("TestScene_1");
    }

    private void Update()
    {
        //Debug.Log(selectedSkin);

    }
}
