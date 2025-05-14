using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager Instance { get; private set; }

    [System.Serializable]
    public class ItemTypeDropChance
    {
        public ItemType itemType;
        [Range(0, 100)] public float chance;
    }

    [System.Serializable]
    public class RarityDropChance
    {
        public ItemRarity rarity;
        [Range(0, 100)] public float chance;
    }

    [Header("Szanse typu przedmiotu")]
    [SerializeField] private List<ItemTypeDropChance> typeChances = new List<ItemTypeDropChance>();

    [Header("Szanse rzadkoœci przedmiotu")]
    [SerializeField] private List<RarityDropChance> rarityChances = new List<RarityDropChance>();

    [Header("Wszystkie mo¿liwe itemy")]
    [SerializeField] private List<ItemSO> allItems = new List<ItemSO>();

    [Header("Prefab world itemu")]
    [SerializeField] private GameObject worldItemPrefab;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public void DropItems(Vector3 position, int minAmount, int maxAmount)
    {
        int dropCount = Random.Range(minAmount, maxAmount + 1);

        for (int i = 0; i < dropCount; i++)
        {
            ItemType? chosenType = RollItemType();
            if (chosenType == null) continue;

            ItemRarity? chosenRarity = RollItemRarity();
            if (chosenRarity == null) continue;

            ItemSO selectedItem = RollItemOfTypeAndRarity(chosenType.Value, chosenRarity.Value);
            if (selectedItem == null) continue;

            SpawnItem(selectedItem, position);
        }
    }

    private ItemType? RollItemType()
    {
        float total = 0f;
        foreach (ItemTypeDropChance entry in typeChances)
        {
            total += entry.chance;
        }

        float roll = Random.Range(0f, total);
        foreach (ItemTypeDropChance entry in typeChances)
        {
            if (roll < entry.chance)
                return entry.itemType;
            roll -= entry.chance;
        }

        return null;
    }

    private ItemRarity? RollItemRarity()
    {
        float total = 0f;
        foreach (RarityDropChance entry in rarityChances)
        {
            total += entry.chance;
        }

        float roll = Random.Range(0f, total);
        foreach (RarityDropChance entry in rarityChances)
        {
            if (roll < entry.chance)
                return entry.rarity;
            roll -= entry.chance;
        }

        return null;
    }

    private ItemSO RollItemOfTypeAndRarity(ItemType type, ItemRarity rarity)
    {
        List<ItemSO> candidates = new List<ItemSO>();

        foreach (ItemSO item in allItems)
        {
            if (item.itemType == type && item.rarity == rarity)
            {
                candidates.Add(item);
            }
        }

        if (candidates.Count == 0) return null;

        float totalWeight = 0f;
        foreach (ItemSO item in candidates)
        {
            totalWeight += item.dropWeight;
        }

        float roll = Random.Range(0f, totalWeight);
        foreach (ItemSO item in candidates)
        {
            if (roll < item.dropWeight)
                return item;
            roll -= item.dropWeight;
        }

        return null;
    }

    private void SpawnItem(ItemSO item, Vector3 origin)
    {
        if (worldItemPrefab == null)
        {
            Debug.LogWarning("Brak przypisanego worldItemPrefab w ItemDropManager!");
            return;
        }

        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        GameObject go = Instantiate(worldItemPrefab, origin + randomOffset, Quaternion.identity);

        ItemWorld worldComp = go.GetComponent<ItemWorld>();
        if (worldComp != null)
        {
            worldComp.SetItem(item);
        }
    }
    

}
