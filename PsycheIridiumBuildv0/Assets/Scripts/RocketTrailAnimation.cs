using UnityEngine;

public class RocketTrailAnimation : MonoBehaviour
{
    public Sprite[] trailSprites;        // Array to hold the trail images
    public float frameRate = 0.1f;       // Time (in seconds) before switching to the next sprite
    public float scaleFactor = 1.0f;     // Multiplier to adjust the size of the sprite

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    void Start()
    {
        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Check if SpriteRenderer exists
        if (spriteRenderer == null)
        {
            Debug.LogError("Error: No SpriteRenderer found on " + gameObject.name + ". Please attach a SpriteRenderer.");
            enabled = false; // Disable the script to avoid further errors
            return;
        }

        // Set the initial sprite and adjust the size
        if (trailSprites.Length > 0)
        {
            spriteRenderer.sprite = trailSprites[0];
            AdjustSpriteScale();  // Apply initial scaling
        }
    }

    void Update()
    {
        // Update the timer based on frame rate
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % trailSprites.Length;
            spriteRenderer.sprite = trailSprites[currentFrame];
            AdjustSpriteScale();  // Adjust size each time the sprite changes
        }
    }

    void AdjustSpriteScale()
    {
        // Set the local scale based on the scale factor
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }
}
