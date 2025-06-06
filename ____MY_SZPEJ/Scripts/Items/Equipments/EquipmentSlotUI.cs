using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IItemSlot
{
    public ItemType slotType; 
    [SerializeField] private Image iconImage;
    private GameObject currentItemUI; 
    private ItemSO equippedItem;

    public void Equip(ItemSO item)
    {
        if (item.itemType != slotType)
        {
            Debug.LogWarning($"Item {item.name} nie pasuje do slotu {slotType}");
            return;
        }

     
        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
        }

        equippedItem = item;
        iconImage.enabled = false;

       
        currentItemUI = new GameObject(item.name);
        currentItemUI.transform.SetParent(transform, false);

        Image img = currentItemUI.AddComponent<Image>();
        img.sprite = item.icon;
        img.raycastTarget = true;

        currentItemUI.AddComponent<ItemUIData>().itemData = item;
        currentItemUI.AddComponent<DraggableItemUI>();
        currentItemUI.AddComponent<ItemTooltipUIHover>();

        CharacterEquipmentUI.Instance?.RaiseEquipmentChanged();
        ItemStatsManager.Instance?.RecalculateItemStats();


    }

    public ItemSO ExtractItem()
    {
        if (equippedItem == null) return null;

        ItemSO temp = equippedItem;
        equippedItem = null;

        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
            currentItemUI = null;
        }
        CharacterEquipmentUI.Instance?.RaiseEquipmentChanged();
        ItemStatsManager.Instance?.RecalculateItemStats();
        return temp;
    }

    public ItemSO GetEquippedItem() => equippedItem;
    public void Unequip()
    {
        if (equippedItem != null)
        {
            BackpackUI.Instance.TryAddItem(equippedItem);
            equippedItem = null;

            if (currentItemUI != null)
            {
                Destroy(currentItemUI);
                currentItemUI = null;
            }

            iconImage.enabled = false;
        }
        CharacterEquipmentUI.Instance?.RaiseEquipmentChanged();
        ItemStatsManager.Instance?.RecalculateItemStats();


    }

    public bool IsEmpty() => equippedItem == null;
}
