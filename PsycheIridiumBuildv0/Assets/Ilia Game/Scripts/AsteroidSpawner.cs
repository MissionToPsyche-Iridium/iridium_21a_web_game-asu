using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // Array of asteroid prefabs to spawn
    public float spawnInterval = 0.5f;   // Default time between spawns
    public int maxAsteroids = 20;        // Default maximum number of active asteroids
    private List<GameObject> activeAsteroids = new List<GameObject>(); // List of currently active asteroids
    private Camera mainCamera;           // Reference to the main camera

    void Start()
    {
        // Load parameters from GameState
        maxAsteroids = GameState.Instance.maxAsteroids;
        spawnInterval = GameState.Instance.asteroidInterval;

        Debug.Log($"AsteroidSpawner initialized with maxAsteroids={maxAsteroids}, spawnInterval={spawnInterval}");

        mainCamera = Camera.main;
        InvokeRepeating(nameof(TrySpawnAsteroid), spawnInterval, spawnInterval); // Start the spawning process
    }

    void Update()
    {
        CleanupAsteroidList(); // Regularly clean up destroyed asteroids from the list
    }

    void CleanupAsteroidList()
    {
        for (int i = activeAsteroids.Count - 1; i >= 0; i--)
        {
            if (activeAsteroids[i] == null) // Remove any null (destroyed) asteroids
            {
                activeAsteroids.RemoveAt(i);
                Debug.Log("Removed a destroyed asteroid from the activeAsteroids list.");
            }
        }
    }

    void TrySpawnAsteroid()
    {
        if (activeAsteroids.Count >= maxAsteroids)
        {
            // Debug.Log("Max asteroids reached. No new asteroids can be spawned until some are removed.");
            return;
        }

        Debug.Log("Attempting to spawn asteroid.");
        SpawnAsteroid();
    }

    void SpawnAsteroid()
    {
        Vector2 spawnPosition = GetRandomEdgePosition();
        Vector2 screenCenter = GetScreenCenter();

        GameObject asteroid = Instantiate(GetRandomAsteroidPrefab(), spawnPosition, Quaternion.identity);
        Debug.Log($"Asteroid spawned at {spawnPosition}.");
        activeAsteroids.Add(asteroid);

        // Add and configure the animation component
        AsteroidSpinInAnimation spinInAnimation = asteroid.AddComponent<AsteroidSpinInAnimation>();
        spinInAnimation.moveDirection = (screenCenter - spawnPosition).normalized; // Move toward screen center
        spinInAnimation.moveSpeed = Random.Range(2f, 6f); // Random movement speed

        // Add screen boundary detection
        asteroid.AddComponent<ScreenBoundaryChecker>().Init(this, asteroid);
    }

    GameObject GetRandomAsteroidPrefab()
    {
        // Randomly select one of the asteroid prefabs
        return asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
    }

    Vector2 GetScreenCenter()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Ensure the camera reference is set
        }
        return mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f)); // Center of the screen
    }

    Vector2 GetRandomEdgePosition()
    {
        float x, y;
        int side = Random.Range(0, 4); // Randomly choose an edge: 0=top, 1=bottom, 2=left, 3=right

        if (side == 0)      // Top edge
        {
            x = Random.Range(0f, 1f);
            y = 1f;
        }
        else if (side == 1) // Bottom edge
        {
            x = Random.Range(0f, 1f);
            y = 0f;
        }
        else if (side == 2) // Left edge
        {
            x = 0f;
            y = Random.Range(0f, 1f);
        }
        else                // Right edge
        {
            x = 1f;
            y = Random.Range(0f, 1f);
        }

        return mainCamera.ViewportToWorldPoint(new Vector2(x, y));
    }

    public void RemoveAsteroid(GameObject asteroid)
    {
        if (activeAsteroids.Contains(asteroid))
        {
            activeAsteroids.Remove(asteroid); // Safely remove the asteroid from the list
            Debug.Log($"Asteroid removed. Remaining asteroids: {activeAsteroids.Count}");
        }
        Destroy(asteroid); // Destroy the asteroid GameObject
    }
}
