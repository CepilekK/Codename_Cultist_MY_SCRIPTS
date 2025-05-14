using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private AttackSource source;
    private int damage;
    private Transform attacker;

    public void Initialize(int dmg, Transform user, AttackSource src)
    {
        damage = dmg;
        attacker = user;
        source = src;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignoruj samego atakuj¹cego
        if (other.transform == attacker) return;

        // SprawdŸ warstwê celu
        if ((source == AttackSource.Player && other.gameObject.layer == LayerMask.NameToLayer("Enemy")) ||
            (source == AttackSource.Enemy && other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.Damage(damage);
                Debug.Log($"[{source}] trafi³ {other.name} za {damage} dmg");
            }
        }
    }
}
