using System.Collections.Generic;
using UnityEngine;

public class AutoAttackDatabase : MonoBehaviour
{
    [SerializeField] private AutoAttackSO[] attackAssets;

    private Dictionary<int, AutoAttackSO> attackMap = new Dictionary<int, AutoAttackSO>();

    private void Awake()
    {
        foreach (var attack in attackAssets)
        {
            if (!attackMap.ContainsKey(attack.id))
            {
                attackMap.Add(attack.id, attack);
            }
            else
            {
                Debug.LogWarning($"AutoAttack ID {attack.id} ju¿ istnieje w bazie danych!");
            }
        }
    }

    public AutoAttackSO GetAttackById(int id)
    {
        attackMap.TryGetValue(id, out var attack);
        return attack;
    }

    public AutoAttackSO[] GetAllAttacks() => attackAssets;
}
