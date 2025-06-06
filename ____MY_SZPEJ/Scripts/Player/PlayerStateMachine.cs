using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerStateMachine : MonoBehaviour
{
    private enum PlayerState { Idle, Moving, Attacking, CastingSkill, Dashing }
   
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private PlayerAutoAttackHandler autoAttackHandler;
    [SerializeField] private DashHandler dashHandler;
    [SerializeField] private PotionSlotUI potionSlot;
    [SerializeField] private PlayerStatsManager statsManager;
    private HealthSystem healthSystem;
    private PlayerState currentState = PlayerState.Idle;
    private NavMeshAgent agent;
    private Camera mainCamera;
    private PlayerAnimationController animationController;
    private void Awake()
    {
     
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.OnDead += HandlePlayerDeath;
        }

    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        animationController = GetComponent<PlayerAnimationController>();

        agent.acceleration = 999f;

    

        PlayerStatsManager.Instance.OnMaxHealthChange += (_, _) => UpdateMoveSpeed(); // używamy tego eventu jako proxy
        UpdateMoveSpeed();

        SwitchState(PlayerState.Idle);
    }

    private void Update()
    {

        switch (currentState)
        {
            case PlayerState.Idle:
                HandleMovementInput();
                break;
            case PlayerState.Moving:
                HandleMovement();
                HandleMovementInput();
                break;
            case PlayerState.Attacking:
            case PlayerState.CastingSkill:
            case PlayerState.Dashing:
                break;
        }

        if (currentState != PlayerState.Attacking && currentState != PlayerState.CastingSkill
            && !agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            if (currentState != PlayerState.Idle)
                SwitchState(PlayerState.Idle);
        }

        if (Input.GetMouseButtonDown(1) && !IsClickOverUI())
        {
            StartAutoAttack();
        }

        if (Input.GetKeyDown(KeyCode.E) && !IsClickOverUI())
        {
            StartDash();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            int StartHp = gameObject.GetComponent<HealthSystem>().GetHealth();
            Debug.Log($"przed potko: {StartHp}");
            if (potionSlot.TryUsePotion(gameObject))
            {
                Debug.Log($"Used potion! HP: {GetComponent<HealthSystem>().GetHealth()}");
            }
        }


    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButton(0) && !IsClickOverUI())
        {
            MoveToMousePosition();
        }
    }

    private void HandleMovement()
    {
        if (agent.remainingDistance <= stoppingDistance && agent.hasPath)
        {
            agent.ResetPath();
            SwitchState(PlayerState.Idle);
        }
    }

    private void MoveToMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookPos = hit.point - transform.position;
            lookPos.y = 0f;

            if (lookPos.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookPos);
                transform.rotation = targetRotation;
            }

            agent.SetDestination(hit.point);
            SwitchState(PlayerState.Moving);
        }
    }

    public void StartAutoAttack()
    {
       
        if (currentState == PlayerState.Attacking) return;

        RotateTowardsMouse();

        float attackSpeed = PlayerStatsManager.Instance != null
      ? PlayerStatsManager.Instance.GetAttackSpeed()
      : 1f;

        float delay = 1f / Mathf.Max(attackSpeed, 0.01f);

        animationController?.PlayAttackAnimation(autoAttackHandler.CurrentAttack.attackTriggerType, delay);

        SwitchState(PlayerState.Attacking);
        StartCoroutine(ExecuteAutoAttackAfterDelay(delay));

    }

    public void CastSkill(ISkill skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("Skill jest null!");
            return;
        }

        // Sprawdzenie cooldownu PRZED castowaniem
        if (PlayerSkillManager.Instance.IsSkillOnCooldown(skill))
        {
            Debug.Log("Skill na cooldownie!");
            return;
        }

        // Sprawdzenie many PRZED castowaniem
        ResourceSystem resourceSystem = GetComponent<ResourceSystem>();
        int cost = skill.GetResourceCost();
        if (resourceSystem != null && !resourceSystem.HasEnough(cost))
        {
            Debug.Log("Za mało many!");
            return;
        }

        // Płacimy za skill tylko jeśli warunki spełnione
        if (!resourceSystem.TrySpend(cost))
        {
            Debug.LogWarning("Nie udało się odjąć many mimo wcześniejszego sprawdzenia.");
            return;
        }

        // Rozpocznij cooldown natychmiast
        PlayerSkillManager.Instance.StartCooldown(skill);

        // Obrót i rozpoczęcie castowania
        RotateTowardsMouse();
        SwitchState(PlayerState.CastingSkill);
        animationController?.PlayCastAnimation();
        StartCoroutine(CastSkillWithDelay(skill));
    }



    public void StartDash()
    {
        if (dashHandler == null) return;

        RotateTowardsMouse();

        dashHandler.TryDash(transform);
        SwitchState(PlayerState.Dashing);
        StartCoroutine(EndActionAfterSeconds(dashHandler.GetCurrentDash().castTime));
    }

    private IEnumerator EndActionAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SwitchState(PlayerState.Idle);
    }

    private void SwitchState(PlayerState newState)
    {
        if (currentState == newState) return;

        if (currentState == PlayerState.Moving && newState != PlayerState.Moving)
        {
            StopAgentMovement();
        }

        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                animationController?.StopRunAnimation();
                break;
            case PlayerState.Moving:
                animationController?.PlayRunAnimation();
                break;
            case PlayerState.Attacking:
                break;
            case PlayerState.CastingSkill:
                break;
            case PlayerState.Dashing:
                break;
        }
    }

    private void StopAgentMovement()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }

    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookPos = hit.point - transform.position;
            lookPos.y = 0f;

            if (lookPos.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookPos);
                transform.rotation = targetRotation;
            }
        }
    }

    private bool IsClickOverUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }
    private void HandlePlayerDeath(object sender, EventArgs e)
    {
        Debug.Log("Gracz zginął!");

        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDead -= HandlePlayerDeath;
        }
    }

    private void UpdateMoveSpeed()
    {
        if (agent != null && PlayerStatsManager.Instance != null)
        {
            agent.speed = PlayerStatsManager.Instance.GetMoveSpeed();
        }
    }
    private IEnumerator CastSkillWithDelay(ISkill skill)
    {
        float castTime = skill.GetData().castTime;
        yield return new WaitForSeconds(castTime);

        skill.Activate(transform);
        SwitchState(PlayerState.Idle);
    }

    private IEnumerator ExecuteAutoAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        autoAttackHandler.UseAttack(); // Skill faktycznie się aktywuje dopiero teraz
        animationController?.ResetSpeed();
        SwitchState(PlayerState.Idle);
    }



}
