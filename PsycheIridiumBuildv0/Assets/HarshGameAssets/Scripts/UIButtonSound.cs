using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure audioSource is created or reused
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        // Always rebind listener when button becomes active
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    void OnDisable()
    {
        // Clean up listener to avoid duplicates on scene reloads
        GetComponent<Button>().onClick.RemoveListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
