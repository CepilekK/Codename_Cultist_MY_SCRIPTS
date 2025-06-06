using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemSO item;

    private void Awake()
    {
        item = GetComponent<ItemUIData>()?.itemData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        ItemTooltipDisplay tooltip = FindObjectOfType<ItemTooltipDisplay>();
        if (tooltip != null)
        {
            tooltip.ShowTooltip(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipDisplay tooltip = FindObjectOfType<ItemTooltipDisplay>();
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
    }
}
