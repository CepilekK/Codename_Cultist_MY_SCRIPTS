using System;
using UnityEngine;

public class SkillTreeNode : MonoBehaviour
{
    public static event EventHandler OnAnyNodeUnlocked;


    [SerializeField] private PerkSO perkData;
    [SerializeField] private SkillTreeNode[] requiredNodes;

    private SpriteRenderer spriteRenderer;
    private bool isUnlocked = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("Brak SpriteRenderer na SkillTreeNode!");
            return;
        }

        if (perkData != null && perkData.icon != null)
        {
            spriteRenderer.sprite = perkData.icon;
        }
    }
    private void OnEnable()
    {
        OnAnyNodeUnlocked += HandleAnyNodeUnlocked;
    }

    private void OnDisable()
    {
        OnAnyNodeUnlocked -= HandleAnyNodeUnlocked;
    }

    private void Start()
    {
        UpdateVisual();
    }
   

    private void OnMouseDown()
    {
        if (isUnlocked) return;
        if (!AreRequirementsMet()) return;

        if (PlayerStatsManager.Instance.TrySpendSkillPoint())
        {
            isUnlocked = true;
            perkData.Apply();
            OnAnyNodeUnlocked?.Invoke(this, EventArgs.Empty);
        }
       
    }

    private bool AreRequirementsMet()
    {
        foreach (SkillTreeNode node in requiredNodes)
        {
            if (!node.IsUnlocked())
                return false;
        }
        return true;
    }

    private void UpdateVisual()
    {
        if (spriteRenderer == null) return;

        if (isUnlocked)
        {
            spriteRenderer.color = Color.green;
        }
        else if (AreRequirementsMet())
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.gray;
        }
    }
    private void HandleAnyNodeUnlocked(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    public bool IsUnlocked() => isUnlocked;
}
