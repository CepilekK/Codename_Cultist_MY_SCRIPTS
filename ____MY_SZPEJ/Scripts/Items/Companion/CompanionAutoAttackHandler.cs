using UnityEngine;

public class CompanionAutoAttackHandler : AutoAttackHandler
{
    private PlayerStatsManager playerStats;

    private void Start()
    {
        playerStats = PlayerStatsManager.Instance;

        if (playerStats == null)
        {
            Debug.LogWarning("Brak PlayerStatsManager.Instance – companion nie mo¿e skalowaæ obra¿eñ!");
        }
    }

    public override int GetCalculatedDamage()
    {
        if (GetAutoAttack() == null || playerStats == null)
            return 0;

        UnitSO_Container unitContainer = GetComponent<UnitSO_Container>();
        if (unitContainer == null || unitContainer.GetUnitSO() == null)
        {
            Debug.LogWarning("Brak UnitSO_Container na Companionie!");
            return 0;
        }

        int companionBaseDamage = unitContainer.GetUnitSO().baseDamage;
        int playerBonusFlat = playerStats.PlayerMinionsBaseDMG;
        float playerMultiplier = playerStats.PlayerMinionsDMGMultiplier;
        float attackMultiplier = GetAutoAttack().damageMultiplier;

        float raw = (companionBaseDamage + playerBonusFlat) * playerMultiplier;
        int final = Mathf.RoundToInt(raw * attackMultiplier);

        return final;
    }
}
