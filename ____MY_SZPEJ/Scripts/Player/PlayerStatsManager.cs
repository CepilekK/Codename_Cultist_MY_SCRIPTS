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
    // SKILL DAMAGE
    public int SkillBaseDamage { get; private set; }
    public float SkillBaseDamageMultiplier { get; private set; }
    // Minions Damage
    public int PlayerMinionsBaseDMG { get; private set; } = 0;
    public float PlayerMinionsDMGMultiplier { get; private set; } = 1f;

    // Bonusy z drzewka umiejêtnoœci (resetowane przy przeliczeniu statystyk)
    private float bonusDamageFromSkillTree = 0f;
    private int bonusMaxHealthFromSkillTree = 0;
    private float bonusSpeedFromSkillTree = 0f;
    private int bonusMaxResourcePointsSkillTree = 0;
    //////////////////////////////////////////////////

    //EQUIPPED ITEM

    // Defense
    public int Armor { get; private set; }
    public int Evasion { get; private set; }
    public int EnergyShield { get; private set; }

    // Weapon
    public int MinWeaponDamage { get; private set; }
    public int MaxWeaponDamage { get; private set; }
    public float AttackSpeed { get; private set; }
    public float CritChance { get; private set; }

    private int bonusMaxHealthFromItems = 0;
    private float bonusMoveSpeedFromItems = 0f;
  
    ///

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

        if (ItemStatsManager.Instance != null)
        {
            ItemStatsManager.Instance.OnItemStatsChanged += () =>
            {
                CalculateFinalStats();
            };
        }
        CalculateFinalStats();
    }
    private void Start()
    {
        if (CharacterEquipmentUI.Instance != null)
            CharacterEquipmentUI.Instance.OnEquipmentChanged += () => CalculateFinalStats();

        if (ItemStatsManager.Instance != null)
            ItemStatsManager.Instance.OnItemStatsChanged += () => CalculateFinalStats();

        OnXPChanged?.Invoke(CurrentXP, RequiredXP);
    }


    public void CalculateFinalStats()
    {
        MaxHealth = baseStats.maxHealthPoints + bonusMaxHealthFromSkillTree + bonusMaxHealthFromItems;
        MaxResource = baseStats.maxResourcePoints + bonusMaxResourcePointsSkillTree;
        MoveSpeed = baseStats.moveSpeed + bonusSpeedFromSkillTree + bonusMoveSpeedFromItems;
        BaseDamage = baseStats.baseDamage + bonusDamageFromSkillTree;
        HealthRegen = baseStats.maxHealthPoints * (baseStats.healthRegenPercent / 100f);
        ResourceRegen = 2f;
        CooldownReduction = 0f;

        //Skille
        SkillBaseDamage = baseStats.skillBaseDamage;
        SkillBaseDamageMultiplier = baseStats.skillBaseDamageMultiplier;
        //Itemy
        PullItemStats();

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
    public float GetAttackSpeed()
    {
        // Jeœli gracz ma broñ z atak speed — u¿yj tego
        if (ItemStatsManager.Instance != null && ItemStatsManager.Instance.HasWeaponEquipped())
        {
            return ItemStatsManager.Instance.AttackSpeed;
        }

        return baseStats.attackSpeed;  // z UnitSO
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

    /// ITEMS
    public void ApplyItemBonuses(int hpBonus, float speedBonus)
    {
        bonusMaxHealthFromItems = hpBonus;
        bonusMoveSpeedFromItems = speedBonus;
        OnMaxHealthChange?.Invoke(this, EventArgs.Empty);
    }



    private void PullItemStats()
    {
        Armor = (ItemStatsManager.Instance?.Armor ?? 0);
        Evasion = (ItemStatsManager.Instance?.Evasion ?? 0);
        EnergyShield = (ItemStatsManager.Instance?.EnergyShield ?? 0);

        MinWeaponDamage = baseStats.baseDamage + (ItemStatsManager.Instance?.MinDamage ?? 0);
        MaxWeaponDamage = baseStats.baseDamage + (ItemStatsManager.Instance?.MaxDamage ?? 0);

        AttackSpeed = (ItemStatsManager.Instance?.AttackSpeed ?? 0f);
        CritChance = (ItemStatsManager.Instance?.CritChance ?? 0f);
    }


    //END ITEMS

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
