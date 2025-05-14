using UnityEngine;

public class DashHandler : MonoBehaviour
{
    [SerializeField] private DashSO currentDash;

    private int currentCharges;
    private float lastDashTime;
    private IDash dashInstance;

    private UnitSO_Container unitContainer;
    private PlayerStatsManager statsManager;

    public System.Action<DashSO> OnDashChanged;

    private void Awake()
    {
        unitContainer = GetComponent<UnitSO_Container>();
        statsManager = GetComponent<PlayerStatsManager>();

        if (currentDash != null)
        {
            dashInstance = CreateDashInstance(currentDash);
            currentCharges = currentDash.maxCharges;
        }
    }

    private void Update()
    {
        RegenerateCharges();
    }

    private void RegenerateCharges()
    {
        if (currentDash == null || currentCharges >= currentDash.maxCharges) return;

        if (Time.time - lastDashTime >= currentDash.cooldown)
        {
            currentCharges++;
            lastDashTime = Time.time;
        }
    }

    public void SetDash(DashSO newDash)
    {
        if (newDash == null) return;

        currentDash = newDash;
        dashInstance = CreateDashInstance(newDash);
        currentCharges = newDash.maxCharges;

        OnDashChanged?.Invoke(newDash);
    }

    private IDash CreateDashInstance(DashSO data)
    {
        switch (data.dashType)
        {
            case DashType.Phase:
                return new PhaseDash(data, this);
            case DashType.ShieldCharge:
                return new ShieldChargeDash(data, this);
            default:
                Debug.LogWarning("Nieobs³ugiwany typ dasza!");
                return null;
        }
    }

    public void TryDash(Transform user)
    {
        if (currentDash == null || currentCharges <= 0 || dashInstance == null) return;

        currentCharges--;
        lastDashTime = Time.time;
        dashInstance.Execute(user);
    }

    public DashSO GetCurrentDash() => currentDash;
    public int GetCurrentCharges() => currentCharges;

    public int GetCalculatedDamage()
    {
        if (currentDash == null || statsManager == null)
            return 0;

        float baseDamage = statsManager.GetBaseDamage();
        return Mathf.RoundToInt(baseDamage * currentDash.damageMultiplier);
    }
}
