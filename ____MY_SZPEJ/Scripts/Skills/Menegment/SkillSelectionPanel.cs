using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionPanel : MonoBehaviour
{
    [SerializeField] private SkillBarManager skillBarManager;
    [SerializeField] private SkillDatabase skillDatabase;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject skillOptionButtonPrefab;

    private int currentSlotIndex;

    public void ShowPanel(int slotIndex)
    {
        currentSlotIndex = slotIndex;
        gameObject.SetActive(true);
        GenerateButtons();
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        ClearButtons();
    }

    private void GenerateButtons()
    {
        ClearButtons();

        List<SkillSO> unlockedSkills = PlayerSkillManager.Instance.GetUnlockedSkills();
        foreach (SkillSO skillSO in unlockedSkills)
        {
            GameObject obj = Instantiate(skillOptionButtonPrefab, buttonContainer);
            SkillOptionButton option = obj.GetComponent<SkillOptionButton>();
            option.Setup(skillSO, this);
        }
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }
    public void SelectSkill(SkillSO skill)
    {
        ISkill instance = CreateSkillInstance(skill);
        skillBarManager.AssignSkillToSlot(currentSlotIndex, instance);
        HidePanel();
    }


    private ISkill CreateSkillInstance(SkillSO skill)
    {
        switch (skill.id)
        {
            case 1: return new FireballSkill(skill);
            case 2: return new WindNovaSkill(skill);
            // Miejsce no nowe skille
            default:
                Debug.LogWarning("Nieobs³ugiwany typ skilla!");
                return null;
        }
    }

    private void OnEnable()
    {
        if (PlayerSkillManager.Instance != null)
        {
            PlayerSkillManager.Instance.OnSkillsChanged += HandleSkillsChanged;
        }
    }

    private void OnDisable()
    {
        if (PlayerSkillManager.Instance != null)
        {
            PlayerSkillManager.Instance.OnSkillsChanged -= HandleSkillsChanged;
        }
    }

    private void HandleSkillsChanged(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            GenerateButtons(); 
        }
    }


}
