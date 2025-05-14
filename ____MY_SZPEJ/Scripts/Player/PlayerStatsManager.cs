using System;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private UnitSO baseStats;


    // Koñcowe, przeliczone statystyki
    public int MaxHealth { get; private set; }
    public int MaxResource { get; private set; }
    public float ResourceRegen { get; private set; }
    public float HealthRegen { get; private set; }
    public float MoveSpeed { get; private set; }
    public float CooldownReduction { get; private set; }
    public int Level { get; private set; }
    public float BaseDamage { get; private set; }

    private void Awake()
    {
        CalculateFinalStats();


    }

   
    /// Przelicz wszystkie statystyki – w przysz³oœci uwzglêdnij modyfikatory z itemów, perków itp.
  
    public void CalculateFinalStats()
    {
        MaxHealth = baseStats.maxHealthPoints;
        MaxResource = baseStats.maxResourcePoints;
        MoveSpeed = baseStats.moveSpeed;
        BaseDamage = baseStats.baseDamage;
        HealthRegen = baseStats.maxHealthPoints * (baseStats.healthRegenPercent / 100f);

       
        ResourceRegen = 2f;
        CooldownReduction = 0f;
        Level = 1;
    }

    // === GETTERY ===

    public int GetStrength() => baseStats.strength;
    public int GetDexterity() => baseStats.dexterity;
    public int GetIntelligence() => baseStats.intelligence;

    public UnitSO GetBaseStats() => baseStats;
    public int GetMaxHealth() => MaxHealth;

    public float GetBaseDamage() => BaseDamage;
    public float GetMoveSpeed() => MoveSpeed;
    public int GetMaxResource() => MaxResource;
    public float GetResourceRegen() => ResourceRegen;
    public float GetHealthRegen() => HealthRegen;



}
