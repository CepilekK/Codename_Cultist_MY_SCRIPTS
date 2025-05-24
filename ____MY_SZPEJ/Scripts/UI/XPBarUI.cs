using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private PlayerStatsManager statsManager;

    private void Start()
    {
        if (statsManager == null)
        {
            Debug.LogError("Brak PlayerStatsManager w XPBarUI!");
            return;
        }

        statsManager.OnXPChanged += UpdateXPUI;
        statsManager.OnLevelUp += OnLevelUpHandler;

        // Wymuœ pierwszy update
        UpdateXPUI(statsManager.CurrentXP, statsManager.RequiredXP);
    }

    private void UpdateXPUI(int currentXP, int requiredXP)
    {
        if (requiredXP <= 0) requiredXP = 1;

        float fillAmount = (float)currentXP / requiredXP;
        fillImage.fillAmount = fillAmount;

        levelText.text = $"Level {statsManager.Level}";
        xpText.text = $"{currentXP}/{requiredXP}";
    }

    private void OnLevelUpHandler(object sender, EventArgs e)
    {
        // Odœwie¿ UI po awansie – u¿yj aktualnych wartoœci
        UpdateXPUI(statsManager.CurrentXP, statsManager.RequiredXP);
    }

    private void OnDestroy()
    {
        if (statsManager != null)
        {
            statsManager.OnXPChanged -= UpdateXPUI;
            statsManager.OnLevelUp -= OnLevelUpHandler;
        }
    }
}
