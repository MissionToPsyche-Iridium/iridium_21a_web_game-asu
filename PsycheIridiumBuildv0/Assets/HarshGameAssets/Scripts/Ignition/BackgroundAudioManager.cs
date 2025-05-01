using System.Collections;
using UnityEngine;

public class BackgroundAudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource radioSource;

    [Header("Delay Settings")]
    public float musicStartDelay = 5f; // Set this in the Inspector

    void Start()
    {
        // Start radio immediately
        if (radioSource != null)
        {
            radioSource.Play();
        }

        // Start music after the configured delay
        StartCoroutine(PlayMusicAfterDelayCoroutine());
    }

    private IEnumerator PlayMusicAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(musicStartDelay);

        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}
