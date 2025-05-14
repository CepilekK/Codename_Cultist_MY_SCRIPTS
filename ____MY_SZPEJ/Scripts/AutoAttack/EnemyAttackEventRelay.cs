using UnityEngine;

public class EnemyAttackEventRelay : MonoBehaviour
{
    [SerializeField] private EnemyAttackLogic attackLogic;
    [SerializeField] private EnemyStateMachine stateMachine;

    // Pocz¹tek animacji – zatrzymuje agenta i blokuje update
    public void StartAttackAnimation()
    {
        stateMachine.SetIsAttackingNow(true);
    }

    // Œrodek animacji – w³aœciwy moment ataku
    public void TriggerAttack()
    {
        attackLogic?.TriggerAttack();
    }

    // Koniec animacji – odblokowuje AI
    public void EndAttackAnimation()
    {
        stateMachine.EndAttackAnimation();
    }
}
