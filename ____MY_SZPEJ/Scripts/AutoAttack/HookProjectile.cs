using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HookProjectile : MonoBehaviour
{
    private float speed;
    private float duration;
    private int damage;
    private Transform player;
    private float pullSpeed;
    private Vector3 direction;
    private LineRenderer lineRenderer;

    private bool pulling = false;
    private Vector3 hookPoint;
    private float stopDistance = 2f; // Odleg³oœæ, w której gracz siê zatrzymuje przed wrogiem

    public void Init(Vector3 dir, float spd, float dur, int dmg, Transform user, float pullSpd)
    {
        direction = dir.normalized;
        speed = spd;
        duration = dur;
        damage = dmg;
        player = user;
        pullSpeed = pullSpd;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, player.position + Vector3.up * 1f);
        lineRenderer.SetPosition(1, transform.position);

        Invoke(nameof(SelfDestructIfNotPulling), duration);
    }

    private void SelfDestructIfNotPulling()
    {
        if (!pulling)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (pulling)
        {
            Vector3 targetPos = hookPoint;
            targetPos.y = player.position.y; 

            float distance = Vector3.Distance(player.position, targetPos);
            if (distance > stopDistance)
            {
                player.position = Vector3.MoveTowards(player.position, targetPos, pullSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }

            lineRenderer.SetPosition(0, player.position + Vector3.up * 1f);
            return;
        }

        Vector3 move = new Vector3(direction.x, 0f, direction.z);
        transform.position += move * speed * Time.deltaTime;

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, player.position + Vector3.up * 1f);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pulling && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.Damage(damage);
                Debug.Log($"Hook trafi³ wroga za {damage} obra¿eñ! Aktualne HP: {health.GetHealth()}");

                if (health.IsDead())
                {
                    Debug.Log("Wróg zgin¹³ od haka!");
                    Destroy(other.gameObject);
                }
            }

            pulling = true;
            hookPoint = other.transform.position;
        }
    }
}
