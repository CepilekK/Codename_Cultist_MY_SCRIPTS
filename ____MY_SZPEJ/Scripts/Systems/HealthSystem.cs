using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    public event Action<int, int> OnHealthChanged;

    private int health;
    private int healthMax;

    private float regenBuffer = 0f;
    private UnitSO_Container unitContainer;
   [SerializeField] private PlayerStatsManager playerStatsManager;

    private void Awake()
    {
        unitContainer = GetComponent<UnitSO_Container>();
       

        if (playerStatsManager != null)
        {
            healthMax = playerStatsManager.GetMaxHealth();
        }
        else if (unitContainer != null && unitContainer.GetUnitSO() != null)
        {
            healthMax = unitContainer.GetUnitSO().maxHealthPoints;
        }
        else
        {
            Debug.LogError("Brak PlayerStatsManager i UnitSO_Container! HealthSystem nie mo¿e dzia³aæ.");
            return;
        }

        health = healthMax;
        OnHealthChanged?.Invoke(health, healthMax);
    }

    private void Update()
    {
        if (playerStatsManager == null || IsDead()) return;

        float regenPerSecond = playerStatsManager.GetHealthRegen();
        float regenAmount = regenPerSecond * Time.deltaTime;
        regenBuffer += regenAmount;

        if (regenBuffer >= 1f)
        {
            int toHeal = Mathf.FloorToInt(regenBuffer);
            regenBuffer -= toHeal;
            Heal(toHeal);
        }
    }

    public void Damage(int amount)
    {
        if (IsDead()) return;

        health -= amount;
        health = Mathf.Max(health, 0);

        OnDamaged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(health, healthMax);

        if (health == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead()) return;

        health += amount;
        health = Mathf.Min(health, healthMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(health, healthMax);
    }

    public void Kill()
    {
        health = 0;
        OnHealthChanged?.Invoke(health, healthMax);
        Die();
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public void HealOverTime(int totalAmount, float duration)
    {
        StartCoroutine(HealOverTimeCoroutine(totalAmount, duration));
    }

    private IEnumerator HealOverTimeCoroutine(int totalAmount, float duration)
    {
        float tickRate = 0.1f;
        int ticks = Mathf.FloorToInt(duration / tickRate);
        if (ticks <= 0) yield break;

        float amountPerTick = (float)totalAmount / ticks;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (IsDead()) yield break;

            int amount = Mathf.CeilToInt(amountPerTick);
            Heal(amount);

            elapsed += tickRate;
            yield return new WaitForSeconds(tickRate);
        }
    }

    public void RefreshMaxHealth()
    {
        if (playerStatsManager != null)
            healthMax = playerStatsManager.GetMaxHealth();
        else if (unitContainer != null && unitContainer.GetUnitSO() != null)
            healthMax = unitContainer.GetUnitSO().maxHealthPoints;

        health = Mathf.Clamp(health, 0, healthMax);
        OnHealthChanged?.Invoke(health, healthMax);
    }


    public bool IsDead() => health <= 0;
    public int GetHealth() => health;
    public int GetMaxHealth() => healthMax;
    public float GetHealthNormalized() => (float)health / healthMax;
}
