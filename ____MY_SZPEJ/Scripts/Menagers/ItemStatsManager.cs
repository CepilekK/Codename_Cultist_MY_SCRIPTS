using UnityEngine;
using System;
using System.Collections.Generic;

public class ItemStatsManager : MonoBehaviour
{
    public event Action OnItemStatsChanged;

    public static ItemStatsManager Instance { get; private set; }

    public int Armor { get; private set; }
    public int Evasion { get; private set; }
    public int EnergyShield { get; private set; }

    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }
    public float AttackSpeed { get; private set; }
    public float CritChance { get; private set; }

    // Dodatkowe bonusy do przekazania dalej
    private int healthBonus;
    private float moveSpeedBonusPercent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (CharacterEquipmentUI.Instance != null)
        {
            CharacterEquipmentUI.Instance.OnEquipmentChanged += RecalculateItemStats;
        }

        RecalculateItemStats();
    }

    public void RecalculateItemStats()
    {
        Armor = 0;
        Evasion = 0;
        EnergyShield = 0;
        MinDamage = 0;
        MaxDamage = 0;
        AttackSpeed = 0f;
        CritChance = 0f;

        healthBonus = 0;
        moveSpeedBonusPercent = 0f;

        IItemSlot[] slots = CharacterEquipmentUI.Instance.GetComponentsInChildren<IItemSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            EquipmentSlotUI eqSlot = slots[i] as EquipmentSlotUI;
            WeaponSlotUI weaponSlot = slots[i] as WeaponSlotUI;

            ItemSO item = eqSlot != null ? eqSlot.GetEquippedItem() : weaponSlot != null ? weaponSlot.GetEquippedItem() : null;
            if (item == null) continue;

            Armor += item.armor;
            Evasion += item.evasion;
            EnergyShield += item.energyShield;

            MinDamage += item.minDamage;
            MaxDamage += item.maxDamage;
            AttackSpeed += item.attackSpeed;
            CritChance += item.critChance;

            healthBonus += item.healthBonus;
            moveSpeedBonusPercent += item.moveSpeedBonus;

            switch (item.implicitEffect)
            {
                case ImplicitEffectType.BonusHealth:
                    healthBonus += Mathf.RoundToInt(item.implicitFinalValue);
                    break;
                case ImplicitEffectType.BonusMoveSpeed:
                    moveSpeedBonusPercent += item.implicitFinalValue;
                    break;
            }

        }

        PlayerStatsManager.Instance?.ApplyItemBonuses(healthBonus, moveSpeedBonusPercent);
        PlayerStatsManager.Instance?.CalculateFinalStats();
        OnItemStatsChanged?.Invoke();
    }

    public void ForceRecalculateWithNotify()
    {
        RecalculateItemStats();
        OnItemStatsChanged?.Invoke();
    }

    public bool HasWeaponEquipped()
    {
        IItemSlot[] slots = CharacterEquipmentUI.Instance.GetComponentsInChildren<IItemSlot>();
        foreach (IItemSlot slot in slots)
        {
            WeaponSlotUI weaponSlot = slot as WeaponSlotUI;
            if (weaponSlot != null && weaponSlot.GetEquippedItem() != null)
                return true;
        }
        return false;
    }



}
