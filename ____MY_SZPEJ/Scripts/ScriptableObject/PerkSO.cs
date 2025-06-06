using UnityEngine;

[CreateAssetMenu(fileName = "NewPerk", menuName = "ScriptableObject/SkillTree/Perk")]
public class PerkSO : ScriptableObject
{
    public string perkName;
    public Sprite icon;
    public string description;
    public SkillSO skillToUnlock;
    public enum EffectType { BonusDamage, BonusHealth, BonusSpeed, UnlockSkill /* ... */ }
    public EffectType effect;

    public float value;

    public void Apply()
    {
        Debug.Log($"Perk '{perkName}' Apply wywołane. Typ efektu: {effect}");

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
                if (skillToUnlock != null)
                {
                    Debug.Log($"Próba odblokowania skilla: {skillToUnlock.skillName}");
                    PlayerSkillManager.Instance.UnlockSkill(skillToUnlock);
                }
                else
                {
                    Debug.LogWarning("Perk ma efekt UnlockSkill, ale skillToUnlock jest null!");
                }
                break;
        }

        Debug.Log($"Perk '{perkName}' został zastosowany.");
    }


   

}
