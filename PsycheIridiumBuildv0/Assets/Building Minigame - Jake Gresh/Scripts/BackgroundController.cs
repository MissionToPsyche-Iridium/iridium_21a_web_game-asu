using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    Vector3 startPosition;
    float sideLength;
    [SerializeField] GameObject backgroundPrefab;
    [SerializeField][Range(0, 1)] float parallaxScale;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        sideLength = GetComponent<SpriteRenderer>().bounds.size.x;

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

        transform.position = new Vector3(startPosition.x + distance.x, startPosition.y + distance.y, 0f);

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