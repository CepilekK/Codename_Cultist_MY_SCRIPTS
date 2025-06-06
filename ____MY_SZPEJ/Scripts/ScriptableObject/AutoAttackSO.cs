using UnityEngine;

[CreateAssetMenu(fileName = "NewAutoAttack", menuName = "ScriptableObject/AutoAttack")]
public class AutoAttackSO : ScriptableObject
{
    [Header("Meta")]
    public int id;
    public string attackName;
    public bool isUnlocked = true;
    public Sprite icon;
    [Header("Animation")]
    public AttackTriggerType attackTriggerType = AttackTriggerType.Melee;

    [Header("Damage Scaling")]
    [Tooltip("Mno¿nik bazowych obra¿eñ z UnitSO")]
    public float damageMultiplier = 1f;

    [Header("Attack Settings")]
    public AutoAttackType attackType;
    public float range = 3f;
    public float radius = 2f; // dla Cleave
    public float projectileSpeed = 10f; // dla ataków dystansowych
    public float projectileDuration = 5f;
    public float pullSpeed = 12f; // dla hooków

    [Header("Optional VFX")]
    public GameObject projectilePrefab; 
    public GameObject effectPrefab;     // efekt obszaru (np. dla Cleave)
    public float effectDuration = 2f;

    
    public GameObject EffectPrefab => effectPrefab;
    public float EffectDuration => effectDuration;
    public float Radius => radius;
}
