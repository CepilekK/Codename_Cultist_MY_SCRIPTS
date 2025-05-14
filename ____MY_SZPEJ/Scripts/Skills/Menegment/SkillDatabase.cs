using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    [SerializeField] private SkillSO[] skillAssets;
    public SkillSO[] GetAllSkills() => skillAssets;
    private Dictionary<int, SkillSO> skillDataMap = new Dictionary<int, SkillSO>();

    private void Awake()
    {
        foreach (SkillSO so in skillAssets)
        {
            if (!skillDataMap.ContainsKey(so.id))
            {
                skillDataMap.Add(so.id, so);
            }
            else
            {
                Debug.LogWarning($"Skill ID {so.id} ju¿ istnieje!");
            }
        }
    }

    public SkillSO GetSkillData(int id)
    {
        skillDataMap.TryGetValue(id, out SkillSO data);
        return data;
    }
}
