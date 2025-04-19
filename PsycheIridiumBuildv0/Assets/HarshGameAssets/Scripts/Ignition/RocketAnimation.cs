using UnityEngine;
using UnityEngine.UI;

public class RocketAnimation : MonoBehaviour
{
    public Sprite[] rocketFrames;
    public float frameRate = 10f;

    [Header("Vertical Movement")]
    public bool move = true;
    public float moveSpeed = 100f;
    public float maxTravelDistance = 500f;
    public float movementDelay = 1f; // New: delay before liftoff

    private Image imageComponent;
    private int currentFrame = 0;
    private float frameTimer = 0f;
    private float movementTimer = 0f;

    private RectTransform rectTransform;
    private Vector2 startPos;
    private float distanceTraveled = 0f;
    private bool isLifting = false;

    void Start()
    {
        imageComponent = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        AnimateFlame();

        if (move)
        {
            movementTimer += Time.deltaTime;

            if (!isLifting && movementTimer >= movementDelay)
            {
                isLifting = true;
            }

            if (isLifting)
            {
                MoveUpward();
            }
        }
    }

    void AnimateFlame()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % rocketFrames.Length;
            imageComponent.sprite = rocketFrames[currentFrame];
        }
    }

    void MoveUpward()
    {
        float verticalMove = moveSpeed * Time.deltaTime;
        rectTransform.anchoredPosition += new Vector2(0f, verticalMove);
        distanceTraveled += verticalMove;

        if (distanceTraveled >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }
}
