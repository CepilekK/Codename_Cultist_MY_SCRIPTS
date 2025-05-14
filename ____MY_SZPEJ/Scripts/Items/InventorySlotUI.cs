using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItemUI draggedItem = eventData.pointerDrag?.GetComponent<DraggableItemUI>();
        if (draggedItem != null)
        {
            draggedItem.transform.SetParent(transform);
            draggedItem.transform.localPosition = Vector3.zero;
        }
    }
}
