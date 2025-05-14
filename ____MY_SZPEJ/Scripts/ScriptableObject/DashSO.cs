using UnityEngine;

[CreateAssetMenu(fileName = "NewDash", menuName = "ScriptableObject/Dash")]
public class DashSO : ScriptableObject
{
    [Header("Meta")]
    public string dashName;
    public Sprite icon;

    [Header("Ustawienia dasza")]
    public float maxDistance = 5f;
    public float castTime = 0.1f;
    public float cooldown = 2f;
    public int maxCharges = 2;

    [Header("Zachowanie")]
    public bool canPhaseThroughWalls = false;
    public DashType dashType;

    [Header("Damage Settings")]
    public float damageMultiplier = 1f;
    public float knockbackForce = 5f;

}
