using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance { get; private set; }

    public event EventHandler OnSkillsChanged;

    [SerializeField] private SkillDatabase skillDatabase;

    private Dictionary<ISkill, float> skillCooldownTimers = new Dictionary<ISkill, float>();

    private HashSet<int> unlockedSkillIds = new HashSet<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (skillDatabase == null)
        {
            Debug.LogError("PlayerSkillManager: Brak przypisanego SkillDatabase!");
            return;
        }

        // Odblokuj te skille, które maj¹ isUnlocked = true
        foreach (SkillSO skill in skillDatabase.GetAllSkills())
        {
            if (skill.isUnlocked)
                unlockedSkillIds.Add(skill.id);
        }
    }
    private void Update()
    {
        List<ISkill> keys = new List<ISkill>(skillCooldownTimers.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ISkill skill = keys[i];
            skillCooldownTimers[skill] -= Time.deltaTime;
            if (skillCooldownTimers[skill] <= 0f)
            {
                skillCooldownTimers.Remove(skill);
            }
        }
    }

    public void UnlockSkill(SkillSO skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("UnlockSkill: skill jest null!");
            return;
        }

        if (!unlockedSkillIds.Contains(skill.id))
        {
            unlockedSkillIds.Add(skill.id);
            OnSkillsChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log($"Skill odblokowany: {skill.skillName}");
        }
    }

    public bool IsSkillUnlocked(SkillSO skill) =>
        skill != null && unlockedSkillIds.Contains(skill.id);

    public List<SkillSO> GetUnlockedSkills()
    {
        List<SkillSO> result = new List<SkillSO>();

        foreach (SkillSO skill in skillDatabase.GetAllSkills())
        {
            if (IsSkillUnlocked(skill))
                result.Add(skill);
        }

        return result;
    }
    public bool IsSkillOnCooldown(ISkill skill)
    {
        return skillCooldownTimers.ContainsKey(skill);
    }
    public void StartCooldown(ISkill skill)
    {
        if (skillCooldownTimers.ContainsKey(skill)) return;

        skillCooldownTimers[skill] = skill.GetCooldown();
    }

}
