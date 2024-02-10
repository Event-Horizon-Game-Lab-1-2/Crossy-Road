using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioMixer Mixer;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        { 

        }
    }

    public void SetValue(float volume)
    {
        Mixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }


}
