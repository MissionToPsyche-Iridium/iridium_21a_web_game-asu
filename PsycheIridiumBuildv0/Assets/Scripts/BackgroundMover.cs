using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float speed = 50f; // Movement speed in pixels per second
    private RectTransform backgroundRect; // RectTransform of the background
    private float moveRange; // Total range the background can move in pixels
    private float startPosition; // Starting X position of the background
    private bool movingLeft = true; // Direction of movement

    void Start()
    {
        // Get the RectTransform of the background image
        backgroundRect = GetComponent<RectTransform>();

        // Calculate the width of the background in pixels
        float backgroundWidth = backgroundRect.rect.width;

        // Calculate the width of the canvas (viewport) in pixels
        Canvas canvas = GetComponentInParent<Canvas>();
        float screenWidth = canvas.GetComponent<RectTransform>().rect.width;

        // Calculate the movement range in pixels
        moveRange = backgroundWidth - screenWidth;

        if (moveRange <= 0)
        {
            Debug.LogWarning("Background image is smaller than or equal to the screen size. No movement will occur.");
        }

        // Store the starting X position
        startPosition = backgroundRect.localPosition.x;
    }

    void Update()
    {
        if (moveRange > 0)
        {
            MoveBackground();
        }
    }

    void MoveBackground()
    {
        Vector3 position = backgroundRect.localPosition;

        // Move the background based on the current direction
        if (movingLeft)
        {
            position.x -= speed * Time.deltaTime;

            // If the background reaches the left limit, switch direction
            if (position.x <= startPosition - moveRange / 2)
            {
                movingLeft = false;
            }
        }
        else
        {
            position.x += speed * Time.deltaTime;

            // If the background reaches the right limit, switch direction
            if (position.x >= startPosition + moveRange / 2)
            {
                movingLeft = true;
            }
        }

        // Apply the updated position
        backgroundRect.localPosition = position;
    }
}
