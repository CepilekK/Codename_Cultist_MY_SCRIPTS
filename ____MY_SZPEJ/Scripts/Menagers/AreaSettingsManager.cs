using UnityEngine;

public class AreaSettingsManager : MonoBehaviour
{
    public static AreaSettingsManager Instance { get; private set; }

    [SerializeField] private int areaLevel = 1;
    public int GetAreaLevel() => areaLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}

