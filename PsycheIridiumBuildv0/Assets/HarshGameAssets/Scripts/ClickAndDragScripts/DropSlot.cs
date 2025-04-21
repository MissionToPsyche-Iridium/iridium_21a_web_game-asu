using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string correctDraggableName; // The correct object name
    private Image slotImage; // The image of the slot itself
    public Image statusImage; // Separate image for tick/cross
    public Sprite tickSprite; //  Assign a tick sprite in Inspector
    public Sprite crossSprite; // Assign a cross sprite in Inspector
    private DragAndDropManager manager;
    private bool alreadyCompleted = false;


    private void Start()
    {
        slotImage = GetComponent<Image>(); // Get the slot's Image component
        manager = FindObjectOfType<DragAndDropManager>();

        // Ensure the status image starts as a cross 
        if (statusImage != null)
        {
            statusImage.sprite = crossSprite;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null)
        {
            // Get reference to the draggable script
            DraggableUI draggable = droppedObject.GetComponent<DraggableUI>();

            // Make sure we have a valid draggable object
            if (draggable != null)
            {
                // Correct placement
                if (droppedObject.name == correctDraggableName)
                {
                    Debug.Log("Correct placement!");

                    // Snap to the slot position
                    droppedObject.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

                    // Hide placeholder
                    if (slotImage != null)
                    {
                        Color newColor = slotImage.color;
                        newColor.a = 0f;
                        slotImage.color = newColor;
                    }

                    // Show tick sprite
                    if (statusImage != null)
                    {
                        statusImage.sprite = tickSprite;
                    }

                    if (!alreadyCompleted)
                    {
                        alreadyCompleted = true;

                        if (manager != null)
                        {
                            manager.RegisterCorrectPlacement();
                        }
                    }

                    // Optional: disable dragging after correct placement
                    draggable.enabled = false;
                }
                // Incorrect placement
                else
                {
                    Debug.Log("Incorrect placement!");

                    // Show cross sprite
                    if (statusImage != null)
                    {
                        statusImage.sprite = crossSprite;
                    }

                    // Snap back to original position
                    droppedObject.GetComponent<RectTransform>().position = draggable.originalPosition;
                }
            }
        }
    }
}
