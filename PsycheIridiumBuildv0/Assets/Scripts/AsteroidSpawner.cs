using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public float spawnInterval = 0.5f; // More frequent spawns
    public int maxAsteroids = 20;
    private List<GameObject> activeAsteroids = new List<GameObject>();
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(TrySpawnAsteroid), spawnInterval, spawnInterval);
    }

    void TrySpawnAsteroid()
    {
        if (activeAsteroids.Count >= maxAsteroids) return;

        SpawnAsteroid();
    }

    void SpawnAsteroid()
    {
        Vector2 spawnPosition = GetRandomOffScreenPosition();
        GameObject asteroid = Instantiate(GetRandomAsteroidPrefab(), spawnPosition, Quaternion.identity);
        activeAsteroids.Add(asteroid);

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        Vector2 directionToScreen = (GetScreenCenter() - spawnPosition).normalized; // Always move toward screen center
        float speed = Random.Range(2f, 6f);
        rb.velocity = directionToScreen * speed;

        asteroid.AddComponent<AsteroidBehavior>().Init(this);
    }

    GameObject GetRandomAsteroidPrefab()
    {
        return asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
    }

    Vector2 GetRandomOffScreenPosition()
    {
        float x, y;
        int side = Random.Range(0, 4); // 0=top, 1=bottom, 2=left, 3=right

        if (side == 0) // Top
        {
            x = Random.Range(0f, 1f);
            y = 1.1f;
        }
        else if (side == 1) // Bottom
        {
            x = Random.Range(0f, 1f);
            y = -0.1f;
        }
        else if (side == 2) // Left
        {
            x = -0.1f;
            y = Random.Range(0f, 1f);
        }
        else // Right
        {
            x = 1.1f;
            y = Random.Range(0f, 1f);
        }

        return mainCamera.ViewportToWorldPoint(new Vector2(x, y));
    }

    Vector2 GetScreenCenter()
    {
        return mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
    }

    public void RemoveAsteroid(GameObject asteroid)
    {
        activeAsteroids.Remove(asteroid);
        Destroy(asteroid);
    }
}
