using System.Text;
using UnityEngine;

public static class TooltipGenerator
{
    public static string Generate(ItemSO item)
    {
        StringBuilder sb = new();
        //IMPLIKTY
        if (item.implicitEffect != ImplicitEffectType.None)
        {
            string effectLine = item.implicitEffect switch
            {
                ImplicitEffectType.BonusHealth => $"+{item.implicitFinalValue} HP (Implicit)",
                ImplicitEffectType.BonusMoveSpeed => $"+{item.implicitFinalValue}% Move Speed (Implicit)",
                _ => ""
            };

            if (!string.IsNullOrEmpty(effectLine))
                sb.AppendLine(effectLine);
        }

        // NAZWA + PREFIX/SUFFIX
        string prefix = item.prefixes.Count > 0 ? item.prefixes[0].modifierName + " " : "";
        string suffix = item.suffixes.Count > 0 ? " " + item.suffixes[0].modifierName : "";

        sb.AppendLine($"<b>{prefix}{item.itemName}{suffix}</b>");
        sb.AppendLine($"<i>{item.rarity}</i>");
        sb.AppendLine("");

        // STATY PODSTAWOWE
        if (item.minDamage > 0 || item.maxDamage > 0)
            sb.AppendLine($"Damage: {item.minDamage} - {item.maxDamage}");

        if (item.attackSpeed > 0)
            sb.AppendLine($"Attack Speed: {item.attackSpeed:F2}/s");

        if (item.critChance > 0)
            sb.AppendLine($"Crit Chance: {item.critChance}%");

        if (item.armor > 0)
            sb.AppendLine($"Armor: {item.armor}");

        if (item.evasion > 0)
            sb.AppendLine($"Evasion: {item.evasion}");

        if (item.energyShield > 0)
            sb.AppendLine($"Energy Shield: {item.energyShield}");

        if (item.healthBonus > 0)
            sb.AppendLine($"+{item.healthBonus} to Maximum Health");

        if (item.moveSpeedBonus > 0)
            sb.AppendLine($"+{item.moveSpeedBonus}% Movement Speed");

        // PREFIXY/SUFFIXY - ROZSZERZONE
        if (item.prefixes.Count > 1)
        {
            foreach (var p in item.prefixes.GetRange(1, item.prefixes.Count - 1))
            {
                sb.AppendLine($"{p.modifierName} (Prefix)");
            }
        }

        if (item.suffixes.Count > 1)
        {
            foreach (var s in item.suffixes.GetRange(1, item.suffixes.Count - 1))
            {
                sb.AppendLine($"{s.modifierName} (Suffix)");
            }
        }

        return sb.ToString();
    }
}
