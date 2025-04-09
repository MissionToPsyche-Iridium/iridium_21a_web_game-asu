using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableText : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    public Vector3 originalPosition; // Stores the initial position

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position; // Save the original position when the game starts
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Nothing special here, just begin dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position; // Move text with mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // The actual position reset or snapping is handled by DropSlotText.cs
    }
}
