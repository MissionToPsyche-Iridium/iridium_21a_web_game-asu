using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    static Dictionary<Vector2Int, bool> objectAtPosition = new();
    public static float backgroundSideLength;
    static float spawnDistance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeObstacleManager()
    {
        objectAtPosition.Clear();
        objectAtPosition.Add(new Vector2Int(0, 0), true);
        SpawnObstacles(new Vector2Int(0, 0));

        spawnDistance = backgroundSideLength / 3;
    }

    public void SpawnObstacles(Vector2Int position)
    {
        Vector2Int originalPosition = position;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                position = originalPosition + new Vector2Int(i, j);
                if (!objectAtPosition.ContainsKey(position))
                {
                    objectAtPosition.Add(position, true);
                    Instantiate(obstaclePrefab, new Vector3(position.x * backgroundSideLength + Random.Range(-spawnDistance, spawnDistance), position.y * backgroundSideLength + Random.Range(-spawnDistance, spawnDistance), 0f), Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)));
                }
            }
        }
    }
}
