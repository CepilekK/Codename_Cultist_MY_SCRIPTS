using System.Collections.Generic;
using UnityEngine;

public static class ItemAffixGenerator
{
    public static void ApplyAffixes(ItemSO item, List<ItemModifierSO> allModifiers)
    {
        if (item == null || allModifiers == null) return;

        int prefixCount = 0;
        int suffixCount = 0;

        switch (item.rarity)
        {
            case ItemRarity.Common:
                prefixCount = 0;
                suffixCount = 0;
                break;
            case ItemRarity.Magic:
                prefixCount = 1;
                suffixCount = 1;
                break;
            case ItemRarity.Rare:
                prefixCount = 2;
                suffixCount = 2;
                break;
            case ItemRarity.Epic:
                prefixCount = 2;
                suffixCount = 2;
                break;
             
        }

        // Filtrowanie dostêpnych prefixów i suffixów
        List<ItemModifierSO> availablePrefixes = allModifiers.FindAll(m => m.type == ModifierType.Prefix);
        List<ItemModifierSO> availableSuffixes = allModifiers.FindAll(m => m.type == ModifierType.Suffix);

        item.prefixes = GetRandomModifiers(availablePrefixes, prefixCount);
        item.suffixes = GetRandomModifiers(availableSuffixes, suffixCount);

        ApplyModifierStats(item, item.prefixes);
        ApplyModifierStats(item, item.suffixes);
    }

    private static List<ItemModifierSO> GetRandomModifiers(List<ItemModifierSO> pool, int count)
    {
        List<ItemModifierSO> result = new();
        List<ItemModifierSO> available = new(pool);

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int index = Random.Range(0, available.Count);
            result.Add(available[index]);
            available.RemoveAt(index);
        }
        return result;
    }

    private static void ApplyModifierStats(ItemSO item, List<ItemModifierSO> modifiers)///Lista Dostepnych modów
    {
        foreach (var mod in modifiers)
        {
            float value = Random.Range(mod.minValue, mod.maxValue);

            switch (mod.affectedStat)
            {
                case AffectedStat.Armor:
                    item.armor += Mathf.RoundToInt(value);
                    break;
                case AffectedStat.Evasion:
                    item.evasion += Mathf.RoundToInt(value);
                    break;
                case AffectedStat.EnergyShield:
                    item.energyShield += Mathf.RoundToInt(value);
                    break;
                case AffectedStat.MinDamage:
                    item.minDamage += Mathf.RoundToInt(value);
                    break;
                case AffectedStat.MaxDamage:
                    item.maxDamage += Mathf.RoundToInt(value);
                    break;
                case AffectedStat.AttackSpeed:
                    item.attackSpeed += value;
                    break;
                case AffectedStat.CritChance:
                    item.critChance += value;
                    break;
                case AffectedStat.Health:
                    item.healthBonus += Mathf.RoundToInt(value);
                    break;
                case AffectedStat.MoveSpeed:
                    item.moveSpeedBonus += value;
                    break;
                    
            }
        }
    }

    public static void ApplyAffixes(ItemSO item, List<ItemModifierSO> allModifiers, ItemRarity rarity)
    {
        if (item == null || allModifiers == null) return;

        int prefixCount = 0;
        int suffixCount = 0;

        switch (rarity)
        {
            case ItemRarity.Common:
                prefixCount = 0;
                suffixCount = 0;
                break;

            case ItemRarity.Magic:
                {
                    int roll = Random.Range(0, 3); // 0 = 1P, 1 = 1S, 2 = 1+1
                    if (roll == 0) prefixCount = 1;
                    else if (roll == 1) suffixCount = 1;
                    else { prefixCount = 1; suffixCount = 1; }
                }
                break;

            case ItemRarity.Rare:
            case ItemRarity.Epic:
                {
                    int roll = Random.Range(0, 3); // 0 = 2P1S, 1 = 1P2S, 2 = 2P2S
                    if (roll == 0) { prefixCount = 2; suffixCount = 1; }
                    else if (roll == 1) { prefixCount = 1; suffixCount = 2; }
                    else { prefixCount = 2; suffixCount = 2; }
                }
                break;
        }

        List<ItemModifierSO> availablePrefixes = allModifiers.FindAll(m => m.type == ModifierType.Prefix);
        List<ItemModifierSO> availableSuffixes = allModifiers.FindAll(m => m.type == ModifierType.Suffix);

        item.prefixes = GetRandomModifiers(availablePrefixes, prefixCount);
        item.suffixes = GetRandomModifiers(availableSuffixes, suffixCount);

        ApplyModifierStats(item, item.prefixes);
        ApplyModifierStats(item, item.suffixes);
    }

}
