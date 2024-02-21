using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private SkinManager SkinManager;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button BuyButton;
    [SerializeField] private TMP_Text SkinPriceText;
    [SerializeField] private TMP_Text CurrentPlayerMoney;

    private List<PlayableCharacter> Skins;

    private void Awake()
    {
        Skins = SkinManager.GetSkins();
        //create prefs
        for (int i = 0; i < Skins.Count; i++)
        {
            if (Skins[i].Unlocked)
            {
                if (!PlayerPrefs.HasKey("Skin_" + i))
                    PlayerPrefs.SetInt("Skin_" + i, 1);
            }
            else
            {
                if (!PlayerPrefs.HasKey("Skin_" + i))
                    PlayerPrefs.SetInt("Skin_" + i, 0);
            }
        }
        //update Skins locked
        for(int i = 0;i < Skins.Count;i++)
        {
            //the the skin prefs
            if (PlayerPrefs.HasKey("Skin_" + i))
            {
                if (PlayerPrefs.GetInt("Skin_" + i) > 0)
                    Skins[i].Unlocked = true;
                else
                    Skins[i].Unlocked = false;
            }
        }

        CurrentPlayerMoney.SetText(PlayerPrefs.GetInt("PlayerMoney").ToString() + " $");
        BuyButton.gameObject.SetActive(false);
    }

    //called by button
    public void BuyCurrentSkin()
    {
        //buy skin
        if(PlayerPrefs.GetInt("PlayerMoney") >= Skins[SkinManager.SelectedSkin].SkinPrize)
        {
            int newMoney = PlayerPrefs.GetInt("PlayerMoney");
            newMoney -= Skins[SkinManager.SelectedSkin].SkinPrize;
            PlayerPrefs.SetInt("PlayerMoney", newMoney);
            CurrentPlayerMoney.SetText(PlayerPrefs.GetInt("PlayerMoney").ToString() + " $");
            //Unlock Skin
            PlayerPrefs.SetInt("Skin_" + SkinManager.SelectedSkin, 1);
            Skins[SkinManager.SelectedSkin].Unlocked = true;
            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        if(Skins[SkinManager.SelectedSkin].Unlocked)
        {
            //play with the skin
            BuyButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
        }
        else
        {
            //buy the skin barbone
            BuyButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SkinManager.OnSkinIndexChanged += UpdateButtons;
    }
    private void OnDisable()
    {
        SkinManager.OnSkinIndexChanged -= UpdateButtons;
    }
}
