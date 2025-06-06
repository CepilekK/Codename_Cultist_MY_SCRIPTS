using UnityEngine;

public enum ModifierType { Prefix, Suffix }
public enum AffectedStat
{
    None,
    Armor,
    Evasion,
    EnergyShield,
    MinDamage,
    MaxDamage,
    AttackSpeed,
    CritChance,
    Health,
    MoveSpeed,
    FireResist,
    ColdResist,
    // ... wiêcej
}

[CreateAssetMenu(fileName = "NewModifier", menuName = "ScriptableObject/ItemModifier")]
public class ItemModifierSO : ScriptableObject
{
    public ModifierType type;
    public AffectedStat affectedStat;
    public float minValue;
    public float maxValue;
    public string modifierName; 
    public float weight = 1f;   // szansa przy losowaniu
}
