using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float thrustForce = .04f;
    public float maxVelocity = .4f;
    public float damping = 0.00001f;
    public float wrapDelayMargin = 0.1f;
    public float rotationSpeed = 10000f;
    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Apply thrust
        Vector2 thrust = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.AddForce(thrust * thrustForce);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
        rb.velocity *= damping;

        // Handle rotation based on player input
        HandleRotation();

        // Wrap the spaceship around the screen
        ScreenWrap();
    }

    void HandleRotation()
    {
        // Apply torque (rotational speed) based on Q and E input
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddTorque(rotationSpeed * Time.deltaTime); // Turn counter-clockwise
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rb.AddTorque(-rotationSpeed * Time.deltaTime); // Turn clockwise
        }
    }

    void ScreenWrap()
    {
        Vector2 newPosition = transform.position;
        Vector2 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1 + wrapDelayMargin)
        {
            newPosition.x = mainCamera.ViewportToWorldPoint(new Vector2(0, 0)).x;
        }
        else if (viewportPosition.x < 0 - wrapDelayMargin)
        {
            newPosition.x = mainCamera.ViewportToWorldPoint(new Vector2(1, 0)).x;
        }

        if (viewportPosition.y > 1 + wrapDelayMargin)
        {
            newPosition.y = mainCamera.ViewportToWorldPoint(new Vector2(0, 0)).y;
        }
        else if (viewportPosition.y < 0 - wrapDelayMargin)
        {
            newPosition.y = mainCamera.ViewportToWorldPoint(new Vector2(0, 1)).y;
        }

        transform.position = newPosition;
    }
}

