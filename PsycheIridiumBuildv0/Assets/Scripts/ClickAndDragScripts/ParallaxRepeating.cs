using UnityEngine;

public class ParallaxRepeating : MonoBehaviour
{
    public float speed = 2f; // Adjust this for scroll speed
    public float backgroundWidth = 20f; // Width of your background

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move the background continuously
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Check if the background has moved past its width
        if (transform.position.x <= startPosition.x - backgroundWidth)
        {
            // Reset position to create an infinite loop
            transform.position += new Vector3(backgroundWidth * 2, 0, 0);
        }
    }
}
