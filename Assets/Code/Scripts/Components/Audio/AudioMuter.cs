using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMuter : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Image ImageRenderer;
    [SerializeField] private Sprite MuteImage;
    [SerializeField] private Sprite UnmuteImage;

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("Muted"))
        {
            PlayerPrefs.SetInt("Muted", 0);
        }
    }

    private void Start()
    {
        UpdateButtons();
    }

    public void Trigger()
    {
        if(PlayerPrefs.GetInt("Muted") == 0)
        {
            Mixer.SetFloat("MasterVolume", -80);
            PlayerPrefs.SetInt("Muted", 1);
        }
        else
        {
            Mixer.SetFloat("MasterVolume", 0);
            PlayerPrefs.SetInt("Muted", 0);
        }
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (PlayerPrefs.GetInt("Muted") == 0)
            ImageRenderer.sprite = MuteImage;
        else
            ImageRenderer.sprite = UnmuteImage;
    }

}
