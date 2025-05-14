using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private bool isDragging = false;
    private ItemSO itemData;

    private void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        itemData = GetComponent<ItemUIData>()?.itemData;
    }

    private void Update()
    {
        if (isDragging)
        {
            rectTransform.position = Input.mousePosition;
        }
        if (isDragging && originalParent == null && Input.GetMouseButtonUp(0))
        {
            OnEndDrag(new PointerEventData(EventSystem.current) { position = Input.mousePosition });
        }
    }

    public void BeginDragManually()
    {
        isDragging = true;
        originalParent = null;

        transform.SetParent(BackpackUI.Instance.GetCanvasRoot(), worldPositionStays: true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalParent = transform.parent;

        IItemSlot slot = originalParent.GetComponent<IItemSlot>();
        if (slot != null)
        {
            itemData = slot.ExtractItem();

            if (itemData != null)
            {
                GameObject dragUI = new GameObject(itemData.name);
                dragUI.transform.SetParent(BackpackUI.Instance.GetCanvasRoot(), false);

                Image img = dragUI.AddComponent<Image>();
                img.sprite = itemData.icon;
                img.raycastTarget = true;

                dragUI.AddComponent<ItemUIData>().itemData = itemData;
                DraggableItemUI drag = dragUI.AddComponent<DraggableItemUI>();
                drag.BeginDragManually();

                Destroy(gameObject);
                return;
            }
        }

        transform.SetParent(BackpackUI.Instance.GetCanvasRoot(), worldPositionStays: true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;

        PointerEventData pointer = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        foreach (RaycastResult hit in results)
        {
            BackpackSlot slot = hit.gameObject.GetComponentInParent<BackpackSlot>();
            if (slot != null && !slot.HasItem())
            {
                transform.SetParent(slot.transform, worldPositionStays: false);
                rectTransform.localPosition = Vector3.zero;
                slot.SetItem(this.gameObject);
                BackpackUI.Instance.ClearDraggedItem();
                return;
            }

            PotionSlotUI potionSlot = hit.gameObject.GetComponentInParent<PotionSlotUI>();
            if (potionSlot != null && itemData != null && itemData.itemType == ItemType.Potion)
            {
                potionSlot.SetPotion(itemData);
                BackpackUI.Instance.ClearDraggedItem();
                Destroy(gameObject);
                return;
            }

            CompanionSlotUI companionSlot = hit.gameObject.GetComponentInParent<CompanionSlotUI>();
            if (companionSlot != null && itemData != null && itemData.itemType == ItemType.Companion)
            {
                companionSlot.SetCompanion(itemData);
                BackpackUI.Instance.ClearDraggedItem();
                Destroy(gameObject);
                return;
            }

            EquipmentSlotUI equipSlot = hit.gameObject.GetComponentInParent<EquipmentSlotUI>();
            if (equipSlot != null && itemData != null && equipSlot.slotType == itemData.itemType)
            {
                if (!equipSlot.IsEmpty())
                    BackpackUI.Instance.TryAddItem(equipSlot.GetEquippedItem());

                equipSlot.Equip(itemData);
                BackpackUI.Instance.ClearDraggedItem();
                Destroy(gameObject);
                return;
            }

            WeaponSlotUI weaponSlot = hit.gameObject.GetComponentInParent<WeaponSlotUI>();
            if (weaponSlot != null && itemData != null)
            {
                if (!(itemData.itemType == ItemType.OneHandedWeapon ||
                      itemData.itemType == ItemType.TwoHandedWeapon ||
                      itemData.itemType == ItemType.Bow ||
                      itemData.itemType == ItemType.Wand ||
                      itemData.itemType == ItemType.Staff ||
                      itemData.itemType == ItemType.Spear ||
                      itemData.itemType == ItemType.Shield ||
                      itemData.itemType == ItemType.Quiver))
                {
                    Debug.LogWarning("Ten przedmiot nie jest bronią.");
                    break;
                }

                if (!weaponSlot.IsValidForThisSlot(itemData) || !weaponSlot.IsValidTogether(itemData))
                {
                    Debug.LogWarning("Nie można włożyć tego przedmiotu do slotu broni.");
                    break;
                }

                if (!weaponSlot.IsEmpty())
                {
                    BackpackUI.Instance.TryAddItem(weaponSlot.ExtractItem());
                }

                weaponSlot.Equip(itemData);
                BackpackUI.Instance.ClearDraggedItem();
                Destroy(gameObject);
                return;
            }
        }

        bool hitVisibleUI = false;
        foreach (RaycastResult hit in results)
        {
            if (hit.gameObject == this.gameObject)
                continue;

            Image image = hit.gameObject.GetComponent<Image>();
            if (image != null && image.raycastTarget && image.enabled && image.gameObject.activeInHierarchy)
            {
                hitVisibleUI = true;
                break;
            }
        }

        if (!hitVisibleUI)
        {
            if (itemData != null)
            {
                BackpackUI.Instance.DropItemToWorld(itemData);
            }
        }
        else
        {
            if (itemData != null)
            {
                BackpackUI.Instance.TryAddItem(itemData);
            }
        }

        BackpackUI.Instance.ClearDraggedItem();
        Destroy(gameObject);
    }
}
