using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    public void PlayRunAnimation()
    {
        animator.SetBool(IsRunning, true);
    }

    public void StopRunAnimation()
    {
        animator.SetBool(IsRunning, false);
    }
}
