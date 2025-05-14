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
        // Ignoruj samego atakuj�cego
        if (other.transform == attacker) return;

        // Sprawd� warstw� celu
        if ((source == AttackSource.Player && other.gameObject.layer == LayerMask.NameToLayer("Enemy")) ||
            (source == AttackSource.Enemy && other.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.Damage(damage);
                Debug.Log($"[{source}] trafi� {other.name} za {damage} dmg");
            }
        }
    }
}
