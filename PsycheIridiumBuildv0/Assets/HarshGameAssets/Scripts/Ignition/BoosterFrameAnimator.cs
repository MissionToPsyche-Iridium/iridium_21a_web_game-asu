using UnityEngine;
using System.Collections;

public class BoosterFrameAnimator : MonoBehaviour
{
    public SpriteRenderer flameRenderer;          // SpriteRenderer that displays the flame frames
    public Sprite[] flameFrames;                  // Frame-by-frame sprites
    public float frameDelay = 0.05f;
    public float ignitionDelay = 1f;
    public float liftoffDelay = 2f;
    public float liftSpeed = 2f;

    [Header("Scale Settings")]
    public float scaleX = 1f;
    public float scaleY = 1f;
    public float scaleZ = 1f;

    private int currentFrame = 0;
    private bool isAnimating = false;
    private bool liftOff = false;

    private void Start()
    {
        // Apply scaling to the object this script is attached to
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        flameRenderer.enabled = false; // Hide flame at start
        StartCoroutine(StartIgnition());
    }

    private IEnumerator StartIgnition()
    {
        yield return new WaitForSeconds(ignitionDelay);

        flameRenderer.enabled = true;
        isAnimating = true;
        StartCoroutine(PlayFlameLoop());

        yield return new WaitForSeconds(liftoffDelay);
        liftOff = true;
    }

    private IEnumerator PlayFlameLoop()
    {
        while (isAnimating)
        {
            flameRenderer.sprite = flameFrames[currentFrame];
            currentFrame = (currentFrame + 1) % flameFrames.Length;
            yield return new WaitForSeconds(frameDelay);
        }
    }

    private void Update()
    {
        if (liftOff)
        {
            transform.position += Vector3.up * liftSpeed * Time.deltaTime;
        }
    }
}
