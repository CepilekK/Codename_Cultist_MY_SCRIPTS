using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public event EventHandler OnMaxHealthChange;

    public static PlayerStatsManager Instance { get; private set; }

    [SerializeField] private UnitSO baseStats;

    public int MaxHealth { get; private set; }
    public int MaxResource { get; private set; }
    public float ResourceRegen { get; private set; }
    public float HealthRegen { get; private set; }
    public float MoveSpeed { get; private set; }
    public float CooldownReduction { get; private set; }
    public float BaseDamage { get; private set; }

    public int Level { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;
    public int RequiredXP => Level * 2;

    private int spentSkillPoints = 0;

    // Bonusy z drzewka umiejêtnoœci (resetowane przy przeliczeniu statystyk)
    private float bonusDamageFromSkillTree = 0f;
    private int bonusMaxHealthFromSkillTree = 0;
    private float bonusSpeedFromSkillTree = 0f;
    private int bonusMaxResourcePointsSkillTree = 0;
    //////////////////////////////////////////////////

    public event EventHandler OnLevelUp;
    public event Action<int, int> OnXPChanged; // (current, required)

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CalculateFinalStats();
        OnXPChanged?.Invoke(CurrentXP, RequiredXP);
    }

    public void CalculateFinalStats()
    {
        MaxHealth = baseStats.maxHealthPoints + bonusMaxHealthFromSkillTree;
        MaxResource = baseStats.maxResourcePoints + bonusMaxResourcePointsSkillTree;
        MoveSpeed = baseStats.moveSpeed + bonusSpeedFromSkillTree;
        BaseDamage = baseStats.baseDamage + bonusDamageFromSkillTree;
        HealthRegen = baseStats.maxHealthPoints * (baseStats.healthRegenPercent / 100f);
        ResourceRegen = 2f;
        CooldownReduction = 0f;
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;



        CurrentXP += amount;

        while (CurrentXP >= RequiredXP)
        {
            CurrentXP -= RequiredXP;
            Level++;
            CalculateFinalStats();

            OnLevelUp?.Invoke(this, EventArgs.Empty);
        }

        OnXPChanged?.Invoke(CurrentXP, RequiredXP);
    }

    public void AddXPFromEnemy(int baseXP)
    {
        int areaLevel = AreaSettingsManager.Instance?.GetAreaLevel() ?? Level;
        int xpToAdd;

        if (areaLevel >= Level)
        {
            xpToAdd = baseXP;
        }
        else
        {
            xpToAdd = Mathf.FloorToInt(baseXP / (1f + (Level - areaLevel)));
        }


        AddXP(xpToAdd);
    }


    public void ResetLevel()
    {
        Level = 1;
        CurrentXP = 0;
        CalculateFinalStats();
        OnLevelUp?.Invoke(this, EventArgs.Empty);
        OnXPChanged?.Invoke(CurrentXP, RequiredXP);
    }


    /// SKILL TREE
    private readonly HashSet<string> unlockedSkills = new HashSet<string>();

    public void UnlockSkill(string skillId)
    {
        if (!unlockedSkills.Contains(skillId))
        {
            unlockedSkills.Add(skillId);
            Debug.Log($"Odblokowano umiejêtnoœæ: {skillId}");
            // TODO: np. aktywacja w SkillHandler / UI
        }
    }

    public bool IsSkillUnlocked(string skillId) => unlockedSkills.Contains(skillId);

    public bool TrySpendSkillPoint()
    {
        if (AvailableSkillPoints > 0)
        {
            spentSkillPoints++;
            return true;
        }

        return false;
    }
    public void ResetSkillPoints()
    {
        spentSkillPoints = 0;
    }
    public void ApplyBonusDamage(float amount)
    {
        bonusDamageFromSkillTree += amount;
        CalculateFinalStats();
    }

    public void ApplyBonusHealth(float amount)
    {
        bonusMaxHealthFromSkillTree += Mathf.RoundToInt(amount);
        CalculateFinalStats();
        OnMaxHealthChange?.Invoke(this, EventArgs.Empty);//W Health System jest ten eveny EVENT!!!!!!!!!!
    }
    public void ApplyResourceSpeed(float amount)
    {
        bonusMaxResourcePointsSkillTree += Mathf.RoundToInt(amount);
        CalculateFinalStats();
    }

    public void ApplyBonusSpeed(float amount)
    {
        bonusSpeedFromSkillTree += amount;
        CalculateFinalStats();
    }

    //SKILL TREE END


    // Gettery
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
    public int AvailableSkillPoints => Mathf.Max(0, Level - 1 - spentSkillPoints);

}
