using System.Collections;
using UnityEngine;

public class IgnitionSoundSequence : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Countdown Clips (10 to 1)")]
    public AudioClip[] countdownClips;

    [Header("Booster Sound")]
    public AudioClip liftoffClip;

    [Header("Rocket Controller")]
    public GameObject rocket; // Will call "StartLiftoff" on this object

    public float delayBetweenClips = 1f;

    void Start()
    {
        StartCoroutine(PlayIgnitionSequence());
    }

    IEnumerator PlayIgnitionSequence()
    {
        // Pause the scene logic
        Time.timeScale = 0f;

        for (int i = 0; i < countdownClips.Length; i++)
        {
            if (countdownClips[i] != null)
            {
                // Use unscaled time to delay while paused
                audioSource.PlayOneShot(countdownClips[i]);
                yield return new WaitForSecondsRealtime(delayBetweenClips);
            }
        }

        // Unpause the game
        Time.timeScale = 1f;

        // Play booster/liftoff sound
        if (liftoffClip != null)
        {
            audioSource.PlayOneShot(liftoffClip);
        }

        // Optional slight delay after liftoff sound starts
        yield return new WaitForSeconds(0.5f);

        // Trigger rocket movement
        if (rocket != null)
        {
            rocket.SendMessage("StartLiftoff", SendMessageOptions.DontRequireReceiver);
        }
    }
}
