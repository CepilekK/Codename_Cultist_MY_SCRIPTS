using UnityEngine;

public class CleaveAttack : IAutoAttack
{
    private AutoAttackSO data;
    private AutoAttackHandler handler;

    public CleaveAttack(AutoAttackSO data, AutoAttackHandler handler)
    {
        this.data = data;
        this.handler = handler;
    }

    public void Execute(Transform user)
    {
        if (data.effectPrefab != null)
        {
            GameObject vfx = GameObject.Instantiate(data.effectPrefab, user.position, Quaternion.LookRotation(user.forward));
            vfx.transform.localScale = Vector3.one * (data.radius * 2f);
            GameObject.Destroy(vfx, data.effectDuration);

            
            AttackHitbox hitbox = vfx.GetComponent<AttackHitbox>();
            if (hitbox != null)
            {
                hitbox.Initialize(handler.GetCalculatedDamage(), user, AttackSource.Player);
            }
        }
    }


    public AutoAttackSO GetData() => data;
}

