using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("AnimationController: Nie znaleziono Animatora w dzieciach!");
        }
    }

    public void PlayAttackAnimation(AutoAttackSO attackData)
    {
        if (animator == null || attackData == null)
        {
            Debug.LogWarning("AnimationController: brak animatora lub danych ataku.");
            return;
        }

        string triggerName = GetTriggerName(attackData.attackTriggerType);

        if (!string.IsNullOrEmpty(triggerName))
        {
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning($"Brak przypisanego triggera dla typu: {attackData.attackTriggerType}");
        }
    }

    private string GetTriggerName(AttackTriggerType triggerType)
    {
        switch (triggerType)
        {
            case AttackTriggerType.Melee:
                return "isAttacking";
            case AttackTriggerType.Ranged:
                return "isAttackingRange";
            case AttackTriggerType.Slam:
                return "isAttackingSlam";
            case AttackTriggerType.Custom:
            default:
                return "";
        }
    }
}
