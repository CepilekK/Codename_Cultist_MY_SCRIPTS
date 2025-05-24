using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField] private AutoAttackSO attackData;
    private IAutoAttack attackInstance;

    private void Awake()
    {
        if (attackData != null && attackData.isUnlocked)
        {
            attackInstance = CreateInstance();
        }
    }

    private IAutoAttack CreateInstance()
    {
        switch (attackData.attackType)
        {
            case AutoAttackType.Cleave:
                return new CleaveAttack(attackData, null);

            default:
                Debug.LogWarning("Nieobs³ugiwany typ autoataku dla wroga!");
                return null;
        }
    }

    public void ExecuteAttack()
    {
        if (attackInstance != null)
        {
            attackInstance.Execute(transform);
        }
    }
}
