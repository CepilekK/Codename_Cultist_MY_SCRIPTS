#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Buffers.Text;

public class EnemyStateMachine : MonoBehaviour
{
    private enum EnemyState { Idle, Chasing, Attacking }

    private EnemyState currentState = EnemyState.Idle;

    [SerializeField] private Transform player;

    [SerializeField] private float chasingTime = 5f;
    private float chasingTimer;
    private bool isChasingCooldownActive = false;


    private NavMeshAgent agent;
    private Animator animator;
    private UnitSO_Container unitContainer;
    private UnitSO stats;
    private EnemyAttackHandler attackHandler;
    private float lastAttackTime;

    private HealthSystem healthSystem;

    private bool isAttackingNow = false;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.OnDamaged += HandleDamaged;

            healthSystem.OnDead += HandleEnemyDeath;

        }
    }

    private void Start()
    {
        attackHandler = GetComponent<EnemyAttackHandler>();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                Debug.LogWarning("Enemy nie mo¿e znaleŸæ gracza! Upewnij siê, ¿e gracz ma tag 'Player'");
            }
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        unitContainer = GetComponent<UnitSO_Container>();
        stats = unitContainer.GetUnitSO();

        if (agent != null && stats != null)
            agent.speed = stats.moveSpeed;
    }

    private void Update()
    {
        if (player == null || stats == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
    
        switch (currentState)
        {
            case EnemyState.Idle:
                if (distanceToPlayer <= stats.aggroRange)
                    SwitchState(EnemyState.Chasing);
                break;

            case EnemyState.Chasing:
                if (distanceToPlayer <= stats.attackRange)
                {
                    agent.ResetPath();
                    animator.SetBool("isRunning", false);
                    SwitchState(EnemyState.Attacking);
                }
                else if (distanceToPlayer > stats.chaseRange)
                {
                    if (!isChasingCooldownActive)
                    {
                        chasingTimer = 0f;
                        isChasingCooldownActive = true;
                    }

                    chasingTimer += Time.deltaTime;
                    if (chasingTimer >= chasingTime)
                    {
                        agent.ResetPath();
                        animator.SetBool("isRunning", false);
                        SwitchState(EnemyState.Idle);
                        isChasingCooldownActive = false;
                    }
                }
                else
                {
                    isChasingCooldownActive = false; 
                    if (agent.enabled && agent.isOnNavMesh && !agent.isStopped)
                    {
                        agent.SetDestination(player.position);
                    }

                    animator.SetBool("isRunning", true);
                }
                break;


            case EnemyState.Attacking:

                if (isAttackingNow) return;
                transform.LookAt(player);

                if (distanceToPlayer > stats.attackRange)
                {
                    SwitchState(EnemyState.Chasing);
                }
                else if (Time.time - lastAttackTime >= stats.attackCooldown)
                {
                    animator.SetTrigger("isAttacking");
                    lastAttackTime = Time.time;

                    attackHandler?.ExecuteAttack(); 
                }
                break;
        }
    }

    private void HandleEnemyDeath(object sender, EventArgs e)
    {
        if (PotionSlotUI.Instance != null)// odnawianie potki
        {
            PotionSlotUI.Instance.AddCharge();
        }

        if (ItemDropManager.Instance != null)// Drop Itemów
        {
            ItemDropManager.Instance.DropItems(transform.position, stats.minDropAmount, stats.maxDropAmount);

        }

        if (PlayerStatsManager.Instance != null && AreaSettingsManager.Instance != null)// DAWABUE GRACZOWI PUNKTÓW DOŒWIOADCZENIA PD/XP
        {
            int playerLevel = PlayerStatsManager.Instance.Level;
            int areaLevel = AreaSettingsManager.Instance.GetAreaLevel();
            int baseXp = stats.baseXpValue;

            int xpToGive = baseXp;

            if (areaLevel < playerLevel)
            {
                int diff = playerLevel - areaLevel;
                xpToGive = Mathf.RoundToInt(baseXp / (float)diff);
            }
           

            PlayerStatsManager.Instance.AddXPFromEnemy(stats.baseXpValue);
        }


        // Zniszczenie wroga
        Destroy(gameObject);
    }


    private void SwitchState(EnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Idle:
            case EnemyState.Chasing:
                if (agent != null && agent.isOnNavMesh)
                    agent.isStopped = false;
                break;

            case EnemyState.Attacking:
                if (agent != null && agent.isOnNavMesh)
                    agent.isStopped = true;
                break;
        }
    }

    public void ResumeAfterAttack()
    {
        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = false;

        SwitchState(EnemyState.Chasing); // powrót do œledzenia
    }






    public void ApplyKnockback(Vector3 direction, float force, float duration = 0.2f)
    {
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float force, float duration)
    {
        if (agent == null) yield break;

        agent.enabled = false;

        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(duration);

       
        TryPlaceOnNavMesh();
        agent.enabled = true;

    }
    private bool TryPlaceOnNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            return true;
        }

        return false;
    }

    public void EndAttackAnimation()
    {
        isAttackingNow = false;
        SwitchState(EnemyState.Chasing); // wróæ do œledzenia po zakoñczeniu animacji
    }
    public void SetIsAttackingNow(bool value)
    {
        isAttackingNow = value;
    }

    private void HandleDamaged(object sender, EventArgs e)
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        SwitchState(EnemyState.Chasing);
        isChasingCooldownActive = false;

        if (agent != null && agent.isOnNavMesh && player != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
        }
    }





    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDead -= HandleEnemyDeath;
            healthSystem.OnDamaged -= HandleDamaged;
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (unitContainer == null)
            unitContainer = GetComponent<UnitSO_Container>();

        if (unitContainer == null) return;

        UnitSO unit = unitContainer.GetUnitSO();
        if (unit == null) return;

        // Aggro Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, unit.aggroRange);

        // Chase Range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, unit.chaseRange);

        // Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unit.attackRange);

#if UNITY_EDITOR
        Handles.color = Color.yellow;
        Handles.Label(transform.position + Vector3.right * unit.aggroRange, "Aggro Range");

        Handles.color = Color.cyan;
        Handles.Label(transform.position + Vector3.forward * unit.chaseRange, "Chase Range");

        Handles.color = Color.red;
        Handles.Label(transform.position + Vector3.left * unit.attackRange, "Attack Range");
#endif
    }
#endif
}
