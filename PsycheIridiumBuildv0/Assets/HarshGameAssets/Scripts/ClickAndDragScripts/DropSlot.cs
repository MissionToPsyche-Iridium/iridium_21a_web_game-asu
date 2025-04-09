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

    private void Start()
    {
        slotImage = GetComponent<Image>(); // Get the slot's Image component

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
            droppedObject.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position; // Snap into place

            // Correct placement
            if (droppedObject.name == correctDraggableName)
            {
                Debug.Log(" Correct placement!");

                // Set placeholder transparency to 0%
                if (slotImage != null)
                {
                    Color newColor = slotImage.color;
                    newColor.a = 0f; // Fully transparent
                    slotImage.color = newColor;
                }

                // Update status image to tick 
                if (statusImage != null)
                {
                    statusImage.sprite = tickSprite;
                }
            }
            // Incorrect placement
            else
            {
                Debug.Log(" Incorrect placement!");

                // Keep status image as a cross 
                if (statusImage != null)
                {
                    statusImage.sprite = crossSprite;
                }

                // Reset object position
                droppedObject.GetComponent<RectTransform>().position = droppedObject.GetComponent<DraggableUI>().originalPosition;
            }
        }
    }
}
