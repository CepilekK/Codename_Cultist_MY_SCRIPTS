using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    public Animator Animator => animator; // <- potrzebne jeœli bêdziesz resetowaæ speed z zewn¹trz

    public void PlayRunAnimation()
    {
        animator.SetBool(IsRunning, true);
    }

    public void StopRunAnimation()
    {
        animator.SetBool(IsRunning, false);
    }

    public void PlayAttackAnimation(AttackTriggerType triggerType, float targetDuration)
    {
        float baseDuration = GetBaseAttackAnimationLength(triggerType); // np. 0.5s
        float speed = baseDuration / targetDuration;

        animator.speed = speed;

        switch (triggerType)
        {
            case AttackTriggerType.Melee:
                animator.SetTrigger("isAttacking");
                break;
            case AttackTriggerType.Ranged:
                animator.SetTrigger("isAttackingRange");
                break;
            case AttackTriggerType.Slam:
                animator.SetTrigger("isAttackingSlam");
                break;
            case AttackTriggerType.Custom:
                animator.SetTrigger("isAttackingCustom");
                break;
        }
    }



    public void PlayCastAnimation()
    {
        animator.SetTrigger("IsCastingSpell");
    }

    private float GetClipLengthForTrigger(AttackTriggerType triggerType)
    {
        string clipName = triggerType switch
        {
            AttackTriggerType.Melee => "MeleeAttack",
            AttackTriggerType.Ranged => "RangedAttack",
            AttackTriggerType.Slam => "SlamAttack",
            AttackTriggerType.Custom => "CustomAttack",
            _ => ""
        };

        if (string.IsNullOrEmpty(clipName)) return 0f;

        // Przeszukaj clipy animatora
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
                return clip.length;
        }

        return 0f;
    }
    private float GetBaseAttackAnimationLength(AttackTriggerType type)
    {
        return type switch
        {
            AttackTriggerType.Melee => 0.5f,
            AttackTriggerType.Ranged => 0.6f,
            AttackTriggerType.Slam => 0.8f,
            AttackTriggerType.Custom => 1f,
            _ => 0.5f,
        };
    }

    public void ResetSpeed()
    {
        animator.speed = 1f;
    }

}
