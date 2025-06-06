using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifierDatabase", menuName = "ScriptableObject/Modifier Database")]
public class ModifierDatabase : ScriptableObject
{
    [System.Serializable]
    public class ModifierGroup
    {
        public ItemType itemType;
        public List<ItemModifierSO> possiblePrefixes;
        public List<ItemModifierSO> possibleSuffixes;
    }

    public List<ModifierGroup> modifierGroups = new();

    public List<ItemModifierSO> GetPrefixesForType(ItemType type)
    {
        ModifierGroup group = modifierGroups.Find(g => g.itemType == type);
        return group != null ? group.possiblePrefixes : new List<ItemModifierSO>();
    }

    public List<ItemModifierSO> GetSuffixesForType(ItemType type)
    {
        ModifierGroup group = modifierGroups.Find(g => g.itemType == type);
        return group != null ? group.possibleSuffixes : new List<ItemModifierSO>();
    }
}
