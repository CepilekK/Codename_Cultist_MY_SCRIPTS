using UnityEngine;

public enum ItemRarity
{
    Common,
    Magic,
    Rare,
    Epic,
    Set,
    Legendary
}

public enum ItemType
{
    OneHandedWeapon,
    TwoHandedWeapon,
    Bow,
    Shield,
    Wand,
    Staff,
    Spear,
    Quiver,
    Helmet,
    Shoulders,
    Gloves,
    Ring,
    Amulet,
    ChestArmor,
    Pants,
    Boots,
    Companion,
    Potion,
    Currency,
    QuestItem
}

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject mesh;
    public GameObject worldPrefab;

    [Header("Drop Settings")]
    public float dropWeight = 1f;

    [Header("Properties")]
    public ItemRarity rarity = ItemRarity.Common;
    public ItemType itemType;

    [Header("Potion Stats")]
    public bool isHealthPotion; 
    public int charges;
    public int chargesRequired;
    public float healAmount;
    public float duration;

    [Header("Companion Settings")]
    public GameObject companionPrefab;
    public UnitSO companionStats;

}
