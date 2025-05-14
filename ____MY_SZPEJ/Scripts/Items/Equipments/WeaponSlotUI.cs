using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour, IItemSlot
{
    public WeaponSlotType slotType;
    [SerializeField] private Image iconImage;

    private ItemSO equippedItem;
    private GameObject currentItemUI;

    public static WeaponSlotUI MainHandSlot;
    public static WeaponSlotUI OffHandSlot;

    private void Awake()
    {
        if (slotType == WeaponSlotType.MainHand)
            MainHandSlot = this;
        else if (slotType == WeaponSlotType.OffHand)
            OffHandSlot = this;
    }

    public void Equip(ItemSO item)
    {
        if (!IsValidForThisSlot(item))
        {
            Debug.LogWarning($"Item {item.name} nie może zostać założony w {slotType}");
            return;
        }

        if (!IsValidTogether(item))
        {
            Debug.LogWarning($"Item {item.name} koliduje z przedmiotem w drugim slocie!");
            return;
        }

       
        if (currentItemUI != null)
            Destroy(currentItemUI);

        equippedItem = item;
        iconImage.enabled = false;

        currentItemUI = new GameObject(item.name);
        currentItemUI.transform.SetParent(transform, false);

        var image = currentItemUI.AddComponent<Image>();
        image.sprite = item.icon;
        image.raycastTarget = true;

        currentItemUI.AddComponent<ItemUIData>().itemData = item;
        currentItemUI.AddComponent<DraggableItemUI>();
    }

    public bool IsValidForThisSlot(ItemSO item)
    {
        if (item == null) return false;

        ItemType itemType = item.itemType;

        if (slotType == WeaponSlotType.OffHand)
        {
            //  Niedozwolone typy dla OffHand
            if (itemType == ItemType.TwoHandedWeapon ||
                itemType == ItemType.Staff ||
                itemType == ItemType.Bow ||
                itemType == ItemType.Spear)
                return false;

            //  Jeśli MainHand ma Spear, to w OffHand może być tylko Shield
            if (MainHandSlot != null && MainHandSlot.GetEquippedItem() != null &&
                MainHandSlot.GetEquippedItem().itemType == ItemType.Spear)
            {
                return itemType == ItemType.Shield;
            }

            //  Quiver tylko jeśli w MainHand jest Bow
            if (itemType == ItemType.Quiver)
            {
                if (MainHandSlot == null || MainHandSlot.GetEquippedItem() == null)
                    return false;

                ItemType mainHandType = MainHandSlot.GetEquippedItem().itemType;
                return mainHandType == ItemType.Bow;
            }

            //  Shield i inne bronie jednoręczne są dozwolone (jeśli Spear NIE jest w MainHand)
            return true;
        }

        if (slotType == WeaponSlotType.MainHand)
        {
            //  Quiver i Shield nie mogą wejść do MainHand
            if (itemType == ItemType.Quiver || itemType == ItemType.Shield)
                return false;

            //  Spear – tylko jeśli OffHand pusty lub ma Shield
            if (itemType == ItemType.Spear)
            {
                if (OffHandSlot == null || OffHandSlot.GetEquippedItem() == null)
                    return true;

                ItemType offHandType = OffHandSlot.GetEquippedItem().itemType;
                return offHandType == ItemType.Shield;
            }

            return true; 
        }

        return false;
    }





    public bool IsValidTogether(ItemSO item)
    {
        WeaponSlotUI otherSlot = slotType == WeaponSlotType.MainHand ? OffHandSlot : MainHandSlot;
        ItemSO otherItem = otherSlot?.equippedItem;

        if (item == null) return true;
        if (otherItem == null) return true;

       
        if (item.itemType == ItemType.TwoHandedWeapon || item.itemType == ItemType.Staff)
            return otherItem == null;

      
        if (item.itemType == ItemType.Bow)
            return otherItem.itemType == ItemType.Quiver;

      
        if (item.itemType == ItemType.Quiver)
            return MainHandSlot?.equippedItem?.itemType == ItemType.Bow;

      
        if (item.itemType == ItemType.Shield && slotType == WeaponSlotType.OffHand)
        {
            var main = MainHandSlot?.equippedItem?.itemType;
            return main == ItemType.OneHandedWeapon || main == ItemType.Wand || main == ItemType.Spear;
        }

       
        if (item.itemType == ItemType.Spear && slotType == WeaponSlotType.MainHand)
        {
            var off = OffHandSlot?.equippedItem?.itemType;
            return off == null || off == ItemType.Shield;
        }

        return true;
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

        iconImage.enabled = false;

        return temp;
    }

    public ItemSO GetEquippedItem() => equippedItem;
    public bool IsEmpty() => equippedItem == null;
}
