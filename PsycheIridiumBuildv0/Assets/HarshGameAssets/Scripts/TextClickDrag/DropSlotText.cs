using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class DropSlotText : MonoBehaviour, IDropHandler
{
    public string correctText; // Assign this in Inspector per slot
    public Image statusImage; // 
    public Sprite tickSprite; // Sprite for correct answer
    public Sprite crossSprite; // Sprite for incorrect answer

    private void Start()
    {
        // Ensure the status image starts as 
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
            // Get the text component from the dragged object
            TextMeshProUGUI textComponent = droppedObject.GetComponent<TextMeshProUGUI>();

            // Snap the text to the drop slot
            droppedObject.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            if (textComponent.text == correctText) // If text matches
            {
                Debug.Log("Correct placement!");

                // Update status image to tick 
                if (statusImage != null)
                {
                    statusImage.sprite = tickSprite;
                }
            }
            else
            {
                Debug.Log(" Incorrect placement!");

                // Keep status image 
                if (statusImage != null)
                {
                    statusImage.sprite = crossSprite;
                }

                // Reset text position
                droppedObject.GetComponent<RectTransform>().position = droppedObject.GetComponent<DraggableText>().originalPosition;
            }
        }
    }
}
