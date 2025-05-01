using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCollision : MonoBehaviour
{
    public ResourceManager resourceManager; // Reference to ResourceManager script

    [SerializeField] private AudioClip collectAudio;
    [SerializeField] private AudioSource audioSource;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Vector2 collisionPoint = collision.contacts[0].point;
            Vector2 asteroidTop = (Vector2)collision.transform.position + Vector2.up * collision.collider.bounds.extents.y;

            // Collect resources from the asteroid
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                // Use the asteroidType enum value instead of a string
                audioSource.PlayOneShot(collectAudio);
                resourceManager.CollectResource(asteroid.asteroidType, asteroid.resourceAmount);
                Debug.Log($"Collected {asteroid.resourceAmount} of {asteroid.asteroidType}");
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.LogWarning("Asteroid component not found on collided object!");
            }
        }
    }
}
