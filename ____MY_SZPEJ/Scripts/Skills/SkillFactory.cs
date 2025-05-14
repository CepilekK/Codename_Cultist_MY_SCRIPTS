using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillSO skillData)
    {
        switch (skillData.id)
        {
            case 1:
                return new FireballSkill(skillData);
            case 2:
                return new WindNovaSkill(skillData);
            default:
                Debug.LogWarning($"SkillFactory: Brak implementacji skilla o ID {skillData.id}");
                return null;
        }
    }
}
