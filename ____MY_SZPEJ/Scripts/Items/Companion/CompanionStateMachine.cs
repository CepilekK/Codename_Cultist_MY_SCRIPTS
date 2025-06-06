using UnityEngine;
using UnityEngine.AI;

public class CompanionStateMachine : MonoBehaviour
{
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float agroRange = 6f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Transform player;
    private NavMeshAgent agent;
    private GameObject currentTarget;
    private float lastAttackTime;
    private AutoAttackHandler attackHandler;
    private Animator animator;
    private AnimationController animationController;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        attackHandler = GetComponent<AutoAttackHandler>();
        animator = GetComponentInChildren<Animator>();
        animationController = GetComponentInChildren<AnimationController>();
    }

    private void Update()
    {
        currentTarget = FindNearestEnemy();

        if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (dist > attackRange)
            {
                agent.SetDestination(currentTarget.transform.position);
                SetRunningAnim(true);
            }
            else
            {
                agent.ResetPath();
                SetRunningAnim(false);
                transform.LookAt(currentTarget.transform);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    animationController?.PlayAttackAnimation(attackHandler?.GetAutoAttack());
                    lastAttackTime = Time.time;
                    attackHandler?.UseAttack();

                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.position) > followDistance)
            {
                agent.SetDestination(player.position);
                SetRunningAnim(true);
            }
            else
            {
                agent.ResetPath();
                SetRunningAnim(false);
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float closest = agroRange;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closest)
            {
                closest = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }

    private void SetRunningAnim(bool isRunning)
    {
        if (animator != null)
            animator.SetBool("isRunning", isRunning);
    }
}
