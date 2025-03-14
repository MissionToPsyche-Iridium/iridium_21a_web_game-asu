using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidType
    {
        Iron,
        Gold,
        Tungsten
    }

    public AsteroidType asteroidType;
    public int resourceAmount { get; private set; } // Encapsulation with a public getter

    void Start()
    {
        InitializeResourceAmount();
    }

    void InitializeResourceAmount()
    {
        switch (asteroidType)
        {
            case AsteroidType.Iron:
                resourceAmount = Random.Range(5, 15);
                break;
            case AsteroidType.Gold:
                resourceAmount = Random.Range(10, 20);
                break;
            case AsteroidType.Tungsten:
                resourceAmount = Random.Range(20, 30);
                break;
            default:
                Debug.LogError($"Unsupported asteroid type: {asteroidType}");
                resourceAmount = 0; // Fallback value
                break;
        }
        Debug.Log($"Asteroid initialized: Type={asteroidType}, Resources={resourceAmount}");
    }
}
