using UnityEngine;

public class SingleShotAttack : IAutoAttack
{
    private AutoAttackSO data;
    private AutoAttackHandler handler;

    public SingleShotAttack(AutoAttackSO data, AutoAttackHandler handler)
    {
        this.data = data;
        this.handler = handler;
    }

    public void Execute(Transform user)
    {
        int damage = handler.GetCalculatedDamage();
        Vector3 direction = Vector3.forward;

        if (handler is PlayerAutoAttackHandler)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            {
                direction = (hit.point - user.position).normalized;
                direction.y = 0f;
            }
        }
        else
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                direction = (player.transform.position - user.position).normalized;
                direction.y = 0f;
            }
        }

        if (direction == Vector3.zero)
        {
            direction = user.forward;
        }

        Vector3 spawnPos = user.position + direction * 1f + Vector3.up * 1f;
        Quaternion rot = Quaternion.LookRotation(direction);

        if (data.projectilePrefab != null)
        {
            GameObject projectileObj = GameObject.Instantiate(data.projectilePrefab, spawnPos, rot);
            Projectile proj = projectileObj.GetComponent<Projectile>();
            if (proj != null)
            {
                AttackSource source = user.CompareTag("Player") ? AttackSource.Player : AttackSource.Enemy;
                float speed = Mathf.Max(0.01f, data.projectileSpeed);
                proj.Init(direction, speed, data.projectileDuration, damage, source);
            }
        }
        else
        {
            Debug.LogWarning("[SingleShot] Brak przypiętego prefab'u pocisku!");
        }
    }

    public AutoAttackSO GetData() => data;
}
