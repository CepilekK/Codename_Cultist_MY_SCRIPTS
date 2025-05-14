using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackSlot : MonoBehaviour, IDropHandler
{
    private GameObject currentItem;

    public void SetItem(GameObject item)
    {
        currentItem = item;
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public void ClearItem()
    {
        currentItem = null;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
