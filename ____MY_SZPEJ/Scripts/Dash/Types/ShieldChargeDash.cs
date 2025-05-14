using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class ShieldChargeDash : IDash
{
    private DashSO data;
    private DashHandler handler;
    private float knockForce = 5f;

    public ShieldChargeDash(DashSO data, DashHandler handler)
    {
        this.data = data;
        this.handler = handler;
    }

    public void Execute(Transform user)
    {
        if (user.TryGetComponent(out NavMeshAgent agent))
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }

        user.GetComponent<MonoBehaviour>().StartCoroutine(PerformCharge(user));
    }

    private IEnumerator PerformCharge(Transform user)
    {
        float duration = data.castTime;
        float speed = data.maxDistance / duration;
        float timer = 0f;
        Vector3 direction = user.forward;

        HashSet<Collider> alreadyHit = new HashSet<Collider>();

        while (timer < duration)
        {
            float step = speed * Time.deltaTime;
            user.position += direction * step;

            Collider[] hits = Physics.OverlapSphere(user.position, 0.8f, LayerMask.GetMask("Enemy"));
            foreach (var hit in hits)
            {
                if (alreadyHit.Contains(hit)) continue;
                alreadyHit.Add(hit);

                HealthSystem health = hit.GetComponent<HealthSystem>();
                if (health != null)
                {
                    health.Damage(handler.GetCalculatedDamage());
                }

                EnemyStateMachine esm = hit.GetComponent<EnemyStateMachine>();
                if (esm != null)
                {
                    Vector3 knockDir = (hit.transform.position - user.position).normalized;
                    esm.ApplyKnockback(knockDir, knockForce, 0.2f);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public DashSO GetData() => data;
}
