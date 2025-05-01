using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Static instance to hold the singleton reference.
    public static MusicPlayer instance;
    private AudioSource audioSource;

    void Awake()
    {
        // If an instance already exists and it's not this, destroy this duplicate.
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            // Set this as the singleton instance and persist across scenes.
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void DestroyMusic()
    {
        if (instance != null)
        {
            instance = null;
            Destroy(gameObject);
        }
    }
}
