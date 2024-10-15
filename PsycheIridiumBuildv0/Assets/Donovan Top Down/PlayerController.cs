using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player Input
    [SerializeField] private PlayerInput playerInput;

    // Settings
    public float moveSpeed;

    // Sprites
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    // Other Player Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Input Actions
    private InputActionAsset inputActions;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction leftAction;
    private InputAction rightAction;

    // Start is called before the first frame update
    void Start()
    {
        // Get the other needed components of the player.
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get the input actions asset.
        inputActions = playerInput.actions;

        // Get the input actions.
        upAction = inputActions.FindAction("Up");
        downAction = inputActions.FindAction("Down");
        leftAction = inputActions.FindAction("Left");
        rightAction = inputActions.FindAction("Right");

        // Set the initial sprite.
        spriteRenderer.sprite = downSprite;
    }

    // Update is called once per frame
    void Update()
    {
        // X and Y direction variables.
        int directionX;
        int directionY;

        // Determine the horizontal direction of velocity.
        if (leftAction.inProgress && !rightAction.inProgress) directionX = -1;
        else if (!leftAction.inProgress && rightAction.inProgress) directionX = 1;
        else directionX = 0;

        // Determine the vertical direction of velocity.
        if (upAction.inProgress && !downAction.inProgress) directionY = 1;
        else if (!upAction.inProgress && downAction.inProgress) directionY = -1;
        else directionY = 0;

        // Set the correct sprite (vertical priority).
        if (directionX < 0) spriteRenderer.sprite = rightSprite;
        else if (directionX > 0) spriteRenderer.sprite = leftSprite;
        if (directionY < 0) spriteRenderer.sprite = downSprite;
        else if (directionY > 0) spriteRenderer.sprite = upSprite;

        // Direction of Velocity
        Vector3 direction = new Vector3(directionX, directionY, 0);

        // Add force to the player.
        rb.AddForce(direction * moveSpeed);
    }

    // Unused event callbacks.
    public void UpAction_performed(InputAction.CallbackContext context)
    {

    }

    public void DownAction_performed(InputAction.CallbackContext context)
    {

    }

    public void LeftAction_performed(InputAction.CallbackContext context)
    {

    }

    public void RightAction_performed(InputAction.CallbackContext context)
    {

    }
}
