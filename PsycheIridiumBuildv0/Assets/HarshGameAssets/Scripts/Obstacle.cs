using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameObject player;
    private ProgressBarController progressBarController;

    [Header("Collision Sound")]
    public AudioClip collisionSound;         // Assign in Inspector
    public float volume = 1f;

    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        progressBarController = FindObjectOfType<ProgressBarController>();

        // Optional: Add AudioSource if not already on the GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Player")
        {
            if (collisionSound != null)
            {
                AudioSource.PlayClipAtPoint(collisionSound, transform.position, volume);
            }

            if (progressBarController != null)
            {
                progressBarController.StopProgress();
            }

            Destroy(player.gameObject);
        }
    }
}
