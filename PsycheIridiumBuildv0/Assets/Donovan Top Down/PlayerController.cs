using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    // Start is called before the first frame update
    void Start()
    {
        // Get input actions.
        InputAction upAction = inputActions.FindActionMap("Top Down").FindAction("Up");
        InputAction downAction = inputActions.FindActionMap("Top Down").FindAction("Down");
        InputAction leftAction = inputActions.FindActionMap("Top Down").FindAction("Left");
        InputAction rightAction = inputActions.FindActionMap("Top Down").FindAction("Right");

        // Assign action implementation to their actions.
        upAction.performed += UpAction_performed;
        downAction.performed += DownAction_performed;
        leftAction.performed += LeftAction_performed;
        rightAction.performed += RightAction_performed;

        // Enable input actions.
        upAction.Enable();
        downAction.Enable();
        leftAction.Enable();
        rightAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Up");
    }

    private void DownAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Down");
    }

    private void LeftAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Left");
    }

    private void RightAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Right");
    }
}
