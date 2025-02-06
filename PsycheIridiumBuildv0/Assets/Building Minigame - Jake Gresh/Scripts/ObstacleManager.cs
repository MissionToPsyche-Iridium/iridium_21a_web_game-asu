using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    static HashSet<Vector2Int> objectAtPosition = new();
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
        objectAtPosition.Add(new Vector2Int(0, 0));
        spawnDistance = backgroundSideLength / 3;
        SpawnObstacles(new Vector2Int(0, 0));
    }

    public void SpawnObstacles(Vector2Int position)
    {
        Vector2Int originalPosition = position;
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                position = originalPosition + new Vector2Int(i, j);
                if (!objectAtPosition.Contains(position))
                {
                    objectAtPosition.Add(position);
                    Instantiate(obstaclePrefab, new Vector3(position.x * backgroundSideLength + Random.Range(-spawnDistance, spawnDistance), position.y * backgroundSideLength + Random.Range(-spawnDistance, spawnDistance), 0f), Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)));
                }
            }
        }
    }
}
