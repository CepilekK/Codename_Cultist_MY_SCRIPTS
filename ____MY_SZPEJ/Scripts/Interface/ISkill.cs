using UnityEngine;

public interface ISkill
{
    void Activate(Transform playerTransform);
    float GetCooldown();
    SkillSO GetData();
    int GetResourceCost();


}
