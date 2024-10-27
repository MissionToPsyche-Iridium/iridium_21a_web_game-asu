using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 startPosition;

    [Header("Sprites")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("Object References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerInput playerInput;

    // Events
    public delegate void InteractEvent();
    public event InteractEvent Interact;

    // Other Player Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Input Actions
    private InputActionAsset inputActions;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction interactAction;
    private InputAction respawnAction;

    // Other Attributes
    private bool interacting;

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
        interactAction = inputActions.FindAction("Interact");
        respawnAction = inputActions.FindAction("Respawn");

        // Bind action events.
        interactAction.started += InteractAction_performed;
        respawnAction.started += RespawnAction_performed;

        // Set the initial sprite.
        spriteRenderer.sprite = downSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!interacting)
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
            if (!interacting) rb.AddForce(direction * moveSpeed);
        }
        else rb.velocity = Vector3.zero;
    }

    public void EnterInteraction()
    {
        interacting = true;
        rb.velocity = Vector3.zero;
    }

    public void ExitInteraction()
    {
        interacting = false;
    }

    // Getter method for whether the player is interacting or not.
    public bool Interacting()
    {
        return interacting;
    }

    public void Respawn()
    {
        transform.position = new Vector3(startPosition.x, startPosition.y, transform.position.z);
        rb.velocity = Vector3.zero;
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

    public void InteractAction_performed(InputAction.CallbackContext context)
    {
        Interact();
    }

    public void RespawnAction_performed(InputAction.CallbackContext context)
    {
        Respawn();
    }
}
