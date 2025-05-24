using UnityEngine;

[CreateAssetMenu(fileName = "NewPerk", menuName = "ScriptableObject/SkillTree/Perk")]
public class PerkSO : ScriptableObject
{
    public string perkName;
    public Sprite icon;
    public string description;

    public enum EffectType { BonusDamage, BonusHealth, BonusSpeed, UnlockSkill /* ... */ }
    public EffectType effect;

    public float value;

    public void Apply()
    {
        switch (effect)
        {
            case EffectType.BonusDamage:
                PlayerStatsManager.Instance.ApplyBonusDamage(value);
                break;
            case EffectType.BonusHealth:
                PlayerStatsManager.Instance.ApplyBonusHealth(value);
                break;
            case EffectType.BonusSpeed:
                PlayerStatsManager.Instance.ApplyBonusSpeed(value);
                break;
            case EffectType.UnlockSkill:
                // np. PlayerSkillManager.Instance.UnlockSkill(id);
                break;
        }

        Debug.Log($"Aktywowano perka: {perkName}");
    }

}
