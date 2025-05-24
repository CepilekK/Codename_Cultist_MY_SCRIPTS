using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float duration;
    private Vector3 direction;
    private int damage;
    private AttackSource source;

    public void Init(Vector3 dir, float spd, float dur, int dmg, AttackSource src)
    {
        direction = dir.normalized;
        speed = spd;
        duration = dur;
        damage = dmg;
        source = src;

        Destroy(gameObject, duration);
    }
    private void Start()
    {
        if (direction == Vector3.zero)
            Debug.LogWarning("Projectile has zero direction!");

        if (speed <= 0)
            Debug.LogWarning("Projectile speed is zero or less!");
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitValidTarget = false;

        if (source == AttackSource.Player && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            hitValidTarget = true;
        }
        else if (source == AttackSource.Enemy && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hitValidTarget = true;
        }

        if (hitValidTarget)
        {
            var health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.Damage(damage);
             //   Debug.Log($"Pocisk ({source}) trafi³ za {damage}! HP celu: {health.GetHealth()}");

              
            }

            Destroy(gameObject);
        }
    }
}
