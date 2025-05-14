using UnityEngine;

public class WindNovaSkill : ISkill
{
    private SkillSO data;

    public WindNovaSkill(SkillSO skillData)
    {
        data = skillData;
    }

    public void Activate(Transform playerTransform)
    {
       
        if (data.EffectPrefab != null)
        {
            GameObject vfx = GameObject.Instantiate(data.EffectPrefab, playerTransform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * (data.radius * 2f);
            GameObject.Destroy(vfx, data.EffectDuration);
        }

      
        Collider[] hitEnemies = Physics.OverlapSphere(playerTransform.position, data.radius, LayerMask.GetMask("Enemy"));

        foreach (Collider enemy in hitEnemies)
        {
            HealthSystem health = enemy.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.Damage(data.skillBaseDamage);
                Debug.Log($"WindNova trafi³a wroga za {data.skillBaseDamage} obra¿eñ! Aktualne HP: {health.GetHealth()}");

                if (health.IsDead())
                {
                    Debug.Log("Wróg zgin¹³ po WindNova!");
                    GameObject.Destroy(enemy.gameObject);
                    continue;
                }

             
                EnemyStateMachine esm = enemy.GetComponent<EnemyStateMachine>();
                if (esm != null)
                {
                    Vector3 knockDir = (enemy.transform.position - playerTransform.position).normalized;
                    knockDir.y = 0.2f; 
                    esm.ApplyKnockback(knockDir, data.knockbackDistance, 0.2f);
                }
            }
        }
    }

    public float GetCooldown() => data.cooldown;
    public SkillSO GetData() => data;

    public int GetResourceCost()
    {
        return data.resourceCost;
    }
}
