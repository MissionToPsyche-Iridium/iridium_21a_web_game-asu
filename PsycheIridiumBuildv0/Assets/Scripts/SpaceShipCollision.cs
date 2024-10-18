using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCollision : MonoBehaviour
{
    public ResourceManager resourceManager;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Vector2 collisionPoint = collision.contacts[0].point;
            Vector2 asteroidTop = collision.transform.position + Vector3.up * collision.collider.bounds.extents.y;

            if (collisionPoint.y >= asteroidTop.y - (collision.collider.bounds.extents.y * 0.2f))
            {
                Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
                resourceManager.CollectResource(asteroid.asteroidType, asteroid.resourceAmount);
                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("Spaceship destroyed!");
            }
        }
    }
}
