using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    private Camera mainCamera;
    private AsteroidSpawner spawner;

    public void Init(AsteroidSpawner spawner)
    {
        this.spawner = spawner;
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector2 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x < -0.1f || viewportPosition.x > 1.1f || viewportPosition.y < -0.1f || viewportPosition.y > 1.1f)
        {
            spawner.RemoveAsteroid(gameObject);
        }
    }
}
