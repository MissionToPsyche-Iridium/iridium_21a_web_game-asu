using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class BackgroundController : MonoBehaviour
{
    static Vector3 startPosition;
    public static float sideLength;
    [SerializeField] GameObject backgroundPrefab;
    [SerializeField][Range(0, 1)] float parallaxScale;
    ObstacleManager obstacleManager;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        sideLength = GetComponent<SpriteRenderer>().bounds.size.x;

        obstacleManager = GetComponent<ObstacleManager>();
        ObstacleManager.backgroundSideLength = sideLength;
        obstacleManager.InitializeObstacleManager();

        // Initialize 8 surrounding background objects
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    Instantiate(backgroundPrefab, new Vector3(startPosition.x + i * sideLength, startPosition.y + j * sideLength, 0f), Quaternion.identity, gameObject.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = Camera.main.transform.position * parallaxScale;
        Vector3 movement = Camera.main.transform.position * (1 - parallaxScale);

        Vector3 newPosition = new Vector3(startPosition.x + distance.x, startPosition.y + distance.y, 0f);
        if (transform.position != newPosition)
        {
            transform.position = newPosition;
            obstacleManager.SpawnObstacles(new Vector2Int((int)newPosition.x, (int)newPosition.y) / (int)sideLength);
            Vector2Int test = new(1, 2);
        }

        if (movement.x > startPosition.x + sideLength)
        {
            startPosition.x += sideLength;
        }
        else if (movement.x < startPosition.x - sideLength)
        {
            startPosition.x -= sideLength;
        }

        if (movement.y > startPosition.y + sideLength)
        {
            startPosition.y += sideLength;
        }
        else if (movement.y < startPosition.y - sideLength)
        {
            startPosition.y -= sideLength;
        }
    }
}