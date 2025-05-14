using UnityEngine;
using UnityEngine.UI;

public class CompanionSlotUI : MonoBehaviour, IItemSlot

{
    [SerializeField] private Image iconImage;
    [SerializeField] private Transform spawnPoint;
    private GameObject currentCompanion;
    private ItemSO currentItem;

    public static CompanionSlotUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }
    public ItemSO ExtractItem()
    {
        ItemSO temp = currentItem;
        currentItem = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (currentCompanion != null)
        {
            Destroy(currentCompanion);
            currentCompanion = null;
        }

        return temp;
    }


    public void SetCompanion(ItemSO item)
    {
        if (item.itemType != ItemType.Companion)
        {
            Debug.LogWarning("Not a companion item");
            return;
        }

        // Jeœli by³ poprzedni companion – usuñ
        if (currentCompanion != null)
            Destroy(currentCompanion);

        // Jeœli by³ poprzedni companion item – odeœlij do plecaka
        if (currentItem != null)
            BackpackUI.Instance.TryAddItem(currentItem);

        currentItem = item;

        // Usuñ stare dzieci (ikona UI)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Stwórz nowy GameObject z draggable ikonk¹
        GameObject itemObj = new GameObject(item.itemName);
        itemObj.transform.SetParent(this.transform, false);

        var image = itemObj.AddComponent<Image>();
        image.sprite = item.icon;
        image.raycastTarget = true;

        itemObj.AddComponent<ItemUIData>().itemData = item;
        itemObj.AddComponent<DraggableItemUI>();

        // Spawn companion w œwiecie
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 spawnPos = player.transform.position + Vector3.right * 2f;

        currentCompanion = Instantiate(item.companionPrefab, spawnPos, Quaternion.identity);

        var statsContainer = currentCompanion.GetComponent<UnitSO_Container>();
        if (statsContainer != null)
            statsContainer.SetUnitSO(item.companionStats);
    }


    public void ClearCompanion()
    {
        if (currentCompanion != null)
        {
            Destroy(currentCompanion);
            currentCompanion = null;
        }

        if (currentItem != null)
        {
            currentItem = null;
        }

        iconImage.sprite = null;
        iconImage.enabled = false;
    }



    public bool HasCompanion() => currentItem != null;
    public ItemSO GetItem() => currentItem;

}