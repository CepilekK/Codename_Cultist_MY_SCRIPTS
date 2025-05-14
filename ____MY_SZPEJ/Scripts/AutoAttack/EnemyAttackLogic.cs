using UnityEngine;

public class EnemyAttackLogic : MonoBehaviour
{
    [SerializeField] private AutoAttackSO attackData;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private Animator animator;

    private UnitSO_Container unitContainer;

    private void Awake()
    {
        unitContainer = GetComponentInParent<UnitSO_Container>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void TriggerAttack()
    {
        if (attackData == null || unitContainer == null)
        {
            Debug.LogWarning("Brak danych ataku lub UnitSO_Container!");
            return;
        }

        int damage = Mathf.RoundToInt(unitContainer.GetUnitSO().baseDamage * attackData.damageMultiplier);

        switch (attackData.attackType)
        {
            case AutoAttackType.Cleave:
                SpawnCleaveHitbox(damage);
                break;

            case AutoAttackType.SingleShot:
                SpawnProjectile(damage);
                break;

            // TU MOZNA DODAC WIECEJ ATAKOW TYPOW
            default:
                Debug.LogWarning("Nieobs³ugiwany typ ataku u wroga.");
                break;
        }
    }

    private void SpawnCleaveHitbox(int damage)
    {
        if (attackData.effectPrefab == null) return;

        Vector3 spawnPos = attackOrigin != null ? attackOrigin.position : transform.position;
        Quaternion rotation = Quaternion.LookRotation(transform.forward);

        GameObject vfx = Instantiate(attackData.effectPrefab, spawnPos, rotation);
        vfx.transform.localScale = Vector3.one * (attackData.radius * 2f);
        Destroy(vfx, attackData.effectDuration);

        AttackHitbox hitbox = vfx.GetComponent<AttackHitbox>();
        if (hitbox != null)
        {
            hitbox.Initialize(damage, transform, AttackSource.Enemy);
        }
    }

    private void SpawnProjectile(int damage)
    {
        if (attackData.projectilePrefab == null) return;

        Vector3 dir = (GetTargetDirection() != Vector3.zero) ? GetTargetDirection() : transform.forward;
        Vector3 spawnPos = attackOrigin != null ? attackOrigin.position : transform.position + dir * 1f;
        spawnPos.y = 1f; // Ustawiamy y na wysokoœci r¹k

        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject projObj = Instantiate(attackData.projectilePrefab, spawnPos, rot);
        Projectile proj = projObj.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(dir, attackData.projectileSpeed, attackData.projectileDuration, damage, AttackSource.Enemy);
        }
    }


    private Vector3 GetTargetDirection()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 toPlayer = player.transform.position - transform.position;
            toPlayer.y = 0f;
            return toPlayer.normalized;
        }
        return Vector3.zero;
    }
}
