using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/* Controls all audio functions besides the background music. */
public class AudioController : MonoBehaviour
{
    public AudioMixer audioMixer; // audio mixer
    public Slider[] volumes; // volume sliders, [0] = master, [1] = background, [2] = SFX
    public AudioSource click; // audio for click sfx

    // Creates volume sliders and imports values when switching scenes
    public void Start()
    {
        if (PlayerPrefs.HasKey("master"))
            volumes[0].value = (PlayerPrefs.GetFloat("master"));
        else
        {
            volumes[0].value = 0;
            PlayerPrefs.SetFloat("master", 0);
        }
        if (PlayerPrefs.HasKey("bg"))
            volumes[1].value = (PlayerPrefs.GetFloat("bg"));
        else
        {
            volumes[1].value = 0;
            PlayerPrefs.SetFloat("bg", 0);
        }
        if (PlayerPrefs.HasKey("sfx"))
            volumes[2].value = (PlayerPrefs.GetFloat("sfx"));
        else
        {
            volumes[2].value = 0;
            PlayerPrefs.SetFloat("sfx", 0);
        }
    }

    // Plays click audio
    public void clickAudio()
    {
        click.Play();
    }

    /* Volume Slider Functions */

    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("master", volume);
        PlayerPrefs.SetFloat("master", volume);
    }

    public void setBgVolume(float volume)
    {
        audioMixer.SetFloat("bg", volume);
        PlayerPrefs.SetFloat("bg", volume);
    }

    public void setSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfx", volume);
        PlayerPrefs.SetFloat("sfx", volume);
    }

}
