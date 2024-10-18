using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public string asteroidType;
    public int resourceAmount;

    void Start()
    {
        SetResourceAmount();
    }

    void SetResourceAmount()
    {
        switch (asteroidType)
        {
            case "Iron":
                resourceAmount = Random.Range(5, 15); // Random range for Iron
                break;
            case "Gold":
                resourceAmount = Random.Range(10, 20); // Random range for Gold
                break;
            case "Tungsten":
                resourceAmount = Random.Range(20, 30); // Random range for Tungsten
                break;
        }
    }
}
