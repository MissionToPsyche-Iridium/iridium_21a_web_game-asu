using UnityEngine;
using UnityEngine.UI;

public class BirdUIAnimator : MonoBehaviour
{
    public Sprite[] birdFrames;
    public float frameRate = 10f;
    public float moveSpeed = 100f;
    public float travelDistance = 400f; // distance before turning
    public bool move;
    private Image imageComponent;
    private int currentFrame = 0;
    private float timer = 0f;
    private RectTransform rectTransform;

    private Vector2 startPos;
    private bool goingRight = true;
    private float distanceTraveled = 0f;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        AnimateFlap();
        if(move)
        {

            MoveBird();
        }
    }

    void AnimateFlap()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % birdFrames.Length;
            imageComponent.sprite = birdFrames[currentFrame];
        }
    }

    void MoveBird()
    {
        float direction = goingRight ? 1f : -1f;
        Vector2 move = new Vector2(direction * moveSpeed * Time.deltaTime, 0f);
        rectTransform.anchoredPosition += move;

        distanceTraveled += move.magnitude;

        if (distanceTraveled >= travelDistance)
        {
            goingRight = !goingRight;
            distanceTraveled = 0f;

            // Optional: flip the bird visually
            rectTransform.localScale = new Vector3(goingRight ? 1 : -1, 1, 1);
        }
    }
}
