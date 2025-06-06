using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "ScriptableObject/Skill")]
public class SkillSO : ScriptableObject
{
    [Header("Meta")]
    public int id;
    public string skillName;
    public Sprite icon;
    public float cooldown = 1f;
    public float castTime = 0.4f;
    public bool isUnlocked = true;

    [Header("Visuals")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float effectDuration = 2f;

    public GameObject ProjectilePrefab => projectilePrefab;
    public GameObject EffectPrefab => effectPrefab;
    public float EffectDuration => effectDuration;

    [Header("Combat")]
    public int skillBaseDamage = 20;
    public int resourceCost = 20;
    [Range(1f, 1000f)]
    public float damageScalePercent = 100f; // 100% = neutralnie

    [Header("Area of Effect")]
    public float radius = 3f;
    public float knockbackDistance = 2f;
}
