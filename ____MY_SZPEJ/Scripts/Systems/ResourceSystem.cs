using System;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{
    [SerializeField] private PlayerStatsManager statsManager;

    private int maxResource;
    private int currentResource;
    private float regenerationBuffer = 0f;

    public event Action<int, int> OnResourceChanged;

    private void Awake()
    {
        if (statsManager == null)
        {
            Debug.LogError("Brak przypisanego PlayerStatsManager w ResourceSystem!");
        }
    }

    private void Start()
    {
        RefreshMaxFromStats();
    }

    private void Update()
    {
        if (statsManager == null) return;

        float regenAmount = statsManager.GetResourceRegen() * Time.deltaTime;
        regenerationBuffer += regenAmount;

        if (regenerationBuffer >= 1f)
        {
            int toAdd = Mathf.FloorToInt(regenerationBuffer);
            regenerationBuffer -= toAdd;
            Regenerate(toAdd);
        }
    }

    public int GetCurrent() => currentResource;
    public int GetMax() => maxResource;

    public bool TrySpend(int amount)
    {
        if (currentResource >= amount)
        {
            currentResource -= amount;
            OnResourceChanged?.Invoke(currentResource, maxResource);
            return true;
        }

        return false;
    }

    public void Regenerate(int amount)
    {
        if (amount <= 0 || currentResource >= maxResource) return;

        currentResource = Mathf.Min(currentResource + amount, maxResource);
        OnResourceChanged?.Invoke(currentResource, maxResource);
    }

    public void RefreshMaxFromStats()
    {
        if (statsManager == null) return;

        maxResource = statsManager.GetMaxResource();
        currentResource = maxResource;
        OnResourceChanged?.Invoke(currentResource, maxResource);
    }

    public float GetResourceNormalized() => (float)currentResource / maxResource;
}
