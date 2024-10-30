using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    //    [SerializeField] GameObject backgroundPrefab;
    //    GameObject[] backgroundObjects = new GameObject[9];

    //    // Side length of the square background prefab in Unity units, equal to resolution/100
    //    [SerializeField] float backgroundSize;

    //    Vector3 initialPosition;
    //    Vector3 newPosition;

    //    [SerializeField] float parallaxRatio;


    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        // Instantiate a grid of 9 background objects
    //        for (int i = 0; i < 9; i++)
    //        {
    //            backgroundObjects[i] = Instantiate(backgroundPrefab, gameObject.transform);
    //        }
    //        PlaceBackgroundObjects();

    //        Time.timeScale = 0.1f;
    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        // Move center background object when spacecraft leaves it
    //        initialPosition = new Vector3(RoundToMultiple(Camera.main.transform.position.x, backgroundSize), RoundToMultiple(Camera.main.transform.position.y, backgroundSize), 0);

    //        // Parallax effect on center background object
    //        newPosition = initialPosition - Camera.main.transform.position * parallaxRatio;
    //        newPosition.z = backgroundObjects[0].transform.position.z; // Preserve the original z position
    //        backgroundObjects[0].transform.position = newPosition;


    //        PlaceBackgroundObjects();
    //    }

    //    static float RoundToMultiple(float value, float multiple)
    //    {
    //        return Mathf.Round(value / multiple) * multiple;
    //    }

    //    void PlaceBackgroundObjects()
    //    // Place backgroundObjects[1~8] around backgroundObjects[0]
    //    {
    //        int index = 1;
    //        for (int i = -1; i <= 1; i++)
    //        {
    //            for (int j = -1; j <= 1; j++)
    //            {
    //                if (!(i == 0 && j == 0))
    //                {
    //                    backgroundObjects[index++].transform.position = backgroundObjects[0].transform.position + new Vector3(i, j, 0) * backgroundSize;
    //                }
    //            }
    //        }
    //    }

    float sideLength;
    Vector3 startPosition;
    [SerializeField] float parallaxEffect;

    [SerializeField] GameObject backgroundPrefab;


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

    void Update()
    {
        Vector3 distance = Camera.main.transform.position * parallaxEffect;
        Vector3 movement = Camera.main.transform.position * (1 - parallaxEffect);

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