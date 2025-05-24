using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "ScriptableObject/Unit")]
public class UnitSO : ScriptableObject
{
    [Header("Unit Info")]
    public string unitName;

    [Header("Base Stats")]
    public int maxHealthPoints = 100; 
    [Tooltip("Procent maksymalnego HP, który odnawia siê co sekundê.")]
    public float healthRegenPercent = 1f; //  1 oznacza 1% HP/sek
    public int baseDamage = 10; 
    public int maxResourcePoints = 200;
    [Range(0.1f, 3f)]
    public float moveSpeedModifier = 1f; // Modyfikator prêdkoœci ruchu (procentowy)

    [Header("Attributes")]
    public int strength = 0;
    public int dexterity = 0; 
    public int intelligence = 0;

    [Header("AI Settings")]
    public float aggroRange = 10f;
    public float chaseRange = 15f;
    public float attackRange = 2f;
    public float moveSpeed = 3.5f;
    public float attackCooldown = 1.5f;
    [Header("Drop Settings")]
    public int minDropAmount = 0;
    public int maxDropAmount = 4;
    public int baseXpValue = 10;

}