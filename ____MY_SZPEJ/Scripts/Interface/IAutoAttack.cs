using UnityEngine;

public interface IAutoAttack
{
    void Execute(Transform user);
    AutoAttackSO GetData();
}
