using UnityEngine;

public class AutoAttackHandler : MonoBehaviour
{
    [SerializeField] private AutoAttackSO currentAttack;

    private UnitSO_Container unitContainer;
    private IAutoAttack currentAttackInstance;

    [SerializeField]private PlayerStatsManager statsManager;

    public event System.Action<AutoAttackSO> OnAutoAttackChanged;

    private void Awake()
    {
        unitContainer = GetComponent<UnitSO_Container>();
       

        if (currentAttack != null && currentAttack.isUnlocked)
        {
            currentAttackInstance = CreateAttackInstance(currentAttack);
        }
    }

    public void SetAutoAttack(AutoAttackSO newAttack)
    {
        if (newAttack != null && newAttack.isUnlocked)
        {
            currentAttack = newAttack;
            currentAttackInstance = CreateAttackInstance(newAttack);

            OnAutoAttackChanged?.Invoke(currentAttack);
        }
        else
        {
            Debug.LogWarning("Próba ustawienia zablokowanego lub pustego autoataku!");
        }
    }

    private IAutoAttack CreateAttackInstance(AutoAttackSO data)
    {
        switch (data.attackType)
        {
            case AutoAttackType.Cleave:
                return new CleaveAttack(data, this);
            case AutoAttackType.SingleShot:
                return new SingleShotAttack(data, this);
            case AutoAttackType.ChainHook:
                return new ChainHookAttack(data, this);
            default:
                Debug.LogWarning("Nieobsługiwany typ autoataku!");
                return null;
        }
    }

    public void UseAttack()
    {
        if (currentAttack == null || !currentAttack.isUnlocked)
        {
            Debug.LogWarning("Autoatak nieustawiony lub zablokowany.");
            return;
        }

        if (currentAttackInstance == null)
        {
            Debug.LogWarning("Brak instancji logiki ataku!");
            return;
        }

        currentAttackInstance.Execute(transform);
    }

    public int GetCalculatedDamage()
    {
        if (currentAttack == null || statsManager == null)
            return 0;

        float baseDamage = statsManager.GetBaseDamage();
        return Mathf.RoundToInt(baseDamage * currentAttack.damageMultiplier);
    }

    public AutoAttackSO GetAutoAttack()
    {
        return currentAttack;
    }
}
