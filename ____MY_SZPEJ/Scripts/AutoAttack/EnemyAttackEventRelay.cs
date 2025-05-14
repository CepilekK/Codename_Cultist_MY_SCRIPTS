using UnityEngine;

public class EnemyAttackEventRelay : MonoBehaviour
{
    [SerializeField] private EnemyAttackLogic attackLogic;
    [SerializeField] private EnemyStateMachine stateMachine;

    // Pocz�tek animacji � zatrzymuje agenta i blokuje update
    public void StartAttackAnimation()
    {
        stateMachine.SetIsAttackingNow(true);
    }

    // �rodek animacji � w�a�ciwy moment ataku
    public void TriggerAttack()
    {
        attackLogic?.TriggerAttack();
    }

    // Koniec animacji � odblokowuje AI
    public void EndAttackAnimation()
    {
        stateMachine.EndAttackAnimation();
    }
}
