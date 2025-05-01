using UnityEngine;

public class HubBackgroundController : MonoBehaviour
{
    static Vector3 startPosition;
    public static float length;
    public static float height;
    [SerializeField] GameObject backgroundPrefab;
    [SerializeField][Range(0, 1)] float parallaxScale;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;

        // Initialize the background prefab
        backgroundPrefab.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

        // Initialize surrounding background objects
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    Instantiate(backgroundPrefab, new Vector3(startPosition.x + i * length, startPosition.y + j * height, 0f), Quaternion.identity, gameObject.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = Camera.main.transform.position * parallaxScale;
        Vector3 movement = Camera.main.transform.position * (1 - parallaxScale);

        Vector3 newPosition = new(startPosition.x + distance.x, startPosition.y + distance.y, 0f);
        if (transform.position != newPosition)
        {
            transform.position = newPosition;
        }

        if (movement.x > startPosition.x + length)
        {
            startPosition.x += length;
        }
        else if (movement.x < startPosition.x - length)
        {
            startPosition.x -= length;
        }

        if (movement.y > startPosition.y + height)
        {
            startPosition.y += height;
        }
        else if (movement.y < startPosition.y - height)
        {
            startPosition.y -= height;
        }
    }
}