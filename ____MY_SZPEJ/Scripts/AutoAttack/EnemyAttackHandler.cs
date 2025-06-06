using UnityEngine;

public class EnemyAttackHandler : AutoAttackHandler
{
    public override int GetCalculatedDamage()
    {
        UnitSO_Container unitContainer = GetComponent<UnitSO_Container>();
        if (unitContainer == null || GetAutoAttack() == null)
            return 0;

        float baseDamage = unitContainer.GetUnitSO().baseDamage;
        float multiplier = GetAutoAttack().damageMultiplier;

        return Mathf.RoundToInt(baseDamage * multiplier);
    }
}
