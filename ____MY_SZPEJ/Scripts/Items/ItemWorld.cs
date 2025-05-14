using TMPro;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private GameObject nameUI;
    [SerializeField] private Transform visualMeshContainer;

    public ItemSO GetItemData() => itemData;

    private void Awake()
    {
        if (itemData != null && itemData.mesh != null && visualMeshContainer != null)
        {
            // Usuñ wszystkie dzieci z kontenera mesha
            foreach (Transform child in visualMeshContainer)
            {
                Destroy(child.gameObject);
            }

            // Wstaw prefab mesha z SO
            GameObject meshInstance = Instantiate(itemData.mesh, visualMeshContainer);
            meshInstance.transform.localPosition = Vector3.zero;
            meshInstance.transform.localRotation = Quaternion.identity;
        }
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
    public void SetItem(ItemSO newItem)
    {
        itemData = newItem;

        // Zaktualizuj nazwê
        if (nameUI != null)
        {
            var text = nameUI.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = itemData.itemName;
            }
        }

        // Usuñ dzieci mesha i dodaj nowy mesh
        if (visualMeshContainer != null && itemData.mesh != null)
        {
            foreach (Transform child in visualMeshContainer)
                Destroy(child.gameObject);

            GameObject meshInstance = Instantiate(itemData.mesh, visualMeshContainer);
            meshInstance.transform.localPosition = Vector3.zero;
            meshInstance.transform.localRotation = Quaternion.identity;
        }
    }

}
