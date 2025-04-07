using System.Collections;
using UnityEngine;

public class AsteroidSpinInAnimation : MonoBehaviour
{
    public float animationDuration = 0.5f; // Duration of the spin-in animation
    private Vector3 targetScale;          // Target scale of the asteroid
    private Rigidbody2D rb;              // Reference to the asteroid's Rigidbody2D for movement

    public Vector2 moveDirection;        // Direction to move after the animation
    public float moveSpeed;              // Speed to move after the animation

    void Start()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero; // Start with asteroid invisible
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop movement during animation
        }
        StartCoroutine(SpinInAnimation());
    }

    IEnumerator SpinInAnimation()
    {
        float timeElapsed = 0f;
        Vector3 initialScale = Vector3.zero;
        Quaternion initialRotation = Quaternion.Euler(0, 0, 720); // Two full spins

        while (timeElapsed < animationDuration)
        {
            float progress = timeElapsed / animationDuration;

            // Animate scale and rotation
            transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.identity, progress);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final state
        transform.localScale = targetScale;
        transform.rotation = Quaternion.identity;

        // Start asteroid movement
        if (rb != null)
        {
            rb.velocity = moveDirection * moveSpeed;
        }

        Destroy(this); // Remove the animation component
    }
}
