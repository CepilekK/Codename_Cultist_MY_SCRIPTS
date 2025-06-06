using UnityEngine;
using System.Collections;

public class PlayerAutoAttackHandler : AutoAttackHandler
{
    [SerializeField] private PlayerAnimationController animationController;
    private bool isAttackingNow = false;

    private void Start()
    {
        statsManager = PlayerStatsManager.Instance;
    }

    public override void UseAttack()
    {
        if (currentAttack == null || !currentAttack.isUnlocked || currentAttackInstance == null)
            return;

        currentAttackInstance.Execute(transform); 
    }



    protected override void RotateTowardsMouse()
    {
        base.RotateTowardsMouse();
    }


    private IEnumerator DelayedAttackExecution(float delay)
    {
        yield return new WaitForSeconds(delay);

        currentAttackInstance.Execute(transform);
        isAttackingNow = false;
        //reset predkosci animacjin
        if (animationController?.Animator != null)
        {
          animationController.Animator.speed = 1f;
        }
    }
    private float GetAttackDelay()
    {
        float attackSpeed = statsManager != null ? statsManager.GetAttackSpeed() : 1f;
        return 1f / Mathf.Max(attackSpeed, 0.01f);
    }
    public AutoAttackSO CurrentAttack => currentAttack;

}
