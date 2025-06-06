using System.Collections.Generic;
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
public enum ImplicitEffectType
{
    None,
    BonusHealth,
    BonusMoveSpeed
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


    [Header("Armor Stats")]
    public int armor;
    public int evasion;
    public int energyShield;

    [Header("Weapon Stats")]
    public int minDamage;
    public int maxDamage;
    public float attackSpeed;      // np. 1.25 ataku na sek
    [Range(0f, 100f)]
    public float critChance;       // np. 5% = 5f

    [Header("Dynamic Modifiers")]
    public List<ItemModifierSO> prefixes = new();
    public List<ItemModifierSO> suffixes = new();

    [Header("Bonus Stats (from Affixes)")]
    public int healthBonus;
    public float moveSpeedBonus;

    [Header("Implicit")]
    public ImplicitEffectType implicitEffect = ImplicitEffectType.None;
    public float implicitMinValue;
    public float implicitMaxValue;
    [HideInInspector] public float implicitFinalValue;
}
