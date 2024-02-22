using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using static SkinManager;

public class SkinManager : MonoBehaviour
{
    public delegate void SkinIndexChanged();
    public event SkinIndexChanged OnSkinIndexChanged;

    [SerializeField] public int SelectedSkin;
    [SerializeField] private TMP_Text TextSkin;
    [SerializeField] List<PlayableCharacter> Skins = new List<PlayableCharacter>();

    private void Awake()
    {
        if(PlayerPrefs.HasKey("selectedSkin"))
            SelectedSkin = PlayerPrefs.GetInt("selectedSkin");
        else
            SelectedSkin = 0;

        if (TextSkin != null)
            TextSkin.text = Skins[SelectedSkin].SkinName;        
    }

    private void Start()
    {
        for(int i = 0; i < Skins.Count; i++)
        {
            if(i != SelectedSkin)
                Skins[i].SkinPrefabTransform.gameObject.SetActive(false);
        }
    }

    public Transform GetSkin(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex != Skins[index].LevelIndex)
            SceneManager.LoadScene(Skins[index].LevelIndex);
        return Skins[index].SkinPrefabTransform;
    }

    public List<PlayableCharacter> GetSkins()
    {
        return Skins;
    }


    public void NextOption()
    {
        //Hide old skin
        Skins[SelectedSkin].SkinPrefabTransform.gameObject.SetActive(false);
        SelectedSkin++;

        if (SelectedSkin > Skins.Count - 1)
        {
            SelectedSkin = 0;
        }
        OnSkinIndexChanged();

        //Show new skin
        Skins[SelectedSkin].SkinPrefabTransform.gameObject.SetActive(true);
        TextSkin.text = Skins[SelectedSkin].SkinName;
    }

    public void BackOption()
    {
        //Hide old skin
        Skins[SelectedSkin].SkinPrefabTransform.gameObject.SetActive(false);
        SelectedSkin--;

        if (SelectedSkin < 0)
        {
            SelectedSkin = Skins.Count - 1;
        }
        OnSkinIndexChanged();

        //Show new skin
        Skins[SelectedSkin].SkinPrefabTransform.gameObject.SetActive(true);
        TextSkin.text = Skins[SelectedSkin].SkinName;
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("selectedSkin", SelectedSkin);
        SceneManager.LoadScene(Skins[SelectedSkin].LevelIndex);
    }

    private void OnDisable()
    {
        OnSkinIndexChanged -= OnSkinIndexChanged;
    }
}
