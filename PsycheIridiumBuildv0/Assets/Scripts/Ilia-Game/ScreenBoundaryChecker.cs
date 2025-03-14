using UnityEngine;

public class ScreenBoundaryChecker : MonoBehaviour
{
    private AsteroidSpawner spawner;
    private GameObject asteroid;

    public void Init(AsteroidSpawner asteroidSpawner, GameObject asteroidObject)
    {
        spawner = asteroidSpawner;
        asteroid = asteroidObject;
    }

    void OnBecameInvisible()
    {
        if (spawner != null && asteroid != null)
        {
            spawner.RemoveAsteroid(asteroid); // Notify the spawner to remove this asteroid
            Debug.Log("Asteroid flew off the screen and was removed.");
        }
    }
}
