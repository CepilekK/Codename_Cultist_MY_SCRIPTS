using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ItemWorld : MonoBehaviour
{
    [SerializeField] private GameObject nameUI;
    [SerializeField] private Transform visualMeshContainer;

    [SerializeField] private ItemSO itemData;
    private ModifierDatabase modifierDatabase;

    public ItemSO GetItemData() => itemData;

    private void Awake()
    {
        if (itemData != null)
            UpdateVisuals();
    }

    private void Start()
    {
        if (itemData != null && nameUI != null)
        {
            TextMeshProUGUI text = nameUI.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = itemData.itemName;
            }
        }
    }

    public void SetItem(ItemSO baseItem, ModifierDatabase modifierDb = null)
    {
        // Tworzymy kopiê itemu by unikn¹æ modyfikacji orygina³u
        itemData = Instantiate(baseItem);
        modifierDatabase = modifierDb;

        if (modifierDatabase != null)
        {
            List<ItemModifierSO> combined = new();
            combined.AddRange(modifierDatabase.GetPrefixesForType(itemData.itemType));
            combined.AddRange(modifierDatabase.GetSuffixesForType(itemData.itemType));
            ItemAffixGenerator.ApplyAffixes(itemData, combined);
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // Mesh
        if (visualMeshContainer != null && itemData.mesh != null)
        {
            foreach (Transform child in visualMeshContainer)
                Destroy(child.gameObject);

            GameObject meshInstance = Instantiate(itemData.mesh, visualMeshContainer);
            meshInstance.transform.localPosition = Vector3.zero;
            meshInstance.transform.localRotation = Quaternion.identity;
        }

        // Nazwa
        if (nameUI != null)
        {
            var text = nameUI.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = itemData.itemName;
            }
        }
    }

    public void OnClickedByPlayer()
    {
        if (BackpackUI.Instance == null || itemData == null) return;

        if (BackpackUI.Instance.IsOpen())
        {
            BackpackUI.Instance.StartDraggingItem(itemData);
            Destroy(gameObject);
        }
        else
        {
            bool added = BackpackUI.Instance.TryAddItem(itemData);
            if (added)
                Destroy(gameObject);
        }
    }

    void OnMouseEnter()
    {
        FindObjectOfType<ItemTooltipDisplay>()?.ShowTooltip(itemData);
    }

    void OnMouseExit()
    {
        FindObjectOfType<ItemTooltipDisplay>()?.HideTooltip();
    }

}
