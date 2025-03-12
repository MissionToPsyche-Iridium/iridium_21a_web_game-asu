using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string correctDraggableName; // Assign the correct object name in Inspector
    private Image slotImage;

    private void Start()
    {
        slotImage = GetComponent<Image>(); // Get the Image component
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject != null && droppedObject.name == correctDraggableName)
        {
            droppedObject.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            Debug.Log("Correct placement!");

            // Set placeholder transparency to 0%
            if (slotImage != null)
            {
                Color newColor = slotImage.color;
                newColor.a = 0f; // Fully transparent
                slotImage.color = newColor;
            }
        }
        else
        {
            Debug.Log("Incorrect placement! Try again.");
            droppedObject.GetComponent<RectTransform>().position = droppedObject.GetComponent<DraggableUI>().originalPosition;
        }
    }
}
