using UnityEngine;


public class SkillBarManager : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private SkillButton[] skillButtons; 
    [SerializeField] private SkillDatabase skillDatabase;

    private ISkill[] assignedSkills = new ISkill[10]; 

    private void Awake()
    {
        // Automatyczne przypisanie indeksów slotów
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].SetSlotIndex(i);
        }
    }

    private void Start()
    {
        SkillSO fireball = skillDatabase.GetSkillData(1); 
        ISkill fireballInstance = new FireballSkill(fireball);
        AssignSkillToSlot(0, fireballInstance);

        SkillSO windNova = skillDatabase.GetSkillData(2); 
        ISkill windNovaInstance = new WindNovaSkill(windNova);
        AssignSkillToSlot(1, windNovaInstance); 
    }



    private void Update()
    {
        HandleSkillShortcuts();
    }

    private void HandleSkillShortcuts()
    {
        for (int i = 0; i < assignedSkills.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseSkillFromSlot(i);
            }
        }
    }

    public void UseSkillFromSlot(int index)
    {
        if (index < 0 || index >= assignedSkills.Length) return;

        ISkill skill = assignedSkills[index];
        if (skill == null) return;

        ResourceSystem resourceSystem = playerStateMachine.GetComponent<ResourceSystem>();
        if (resourceSystem != null && resourceSystem.TrySpend(skill.GetResourceCost()))
        {
            playerStateMachine.CastSkill(skill);
        }
        else
        {
            Debug.Log("Za mało zasobów!");
        }
    }




    public void AssignSkillToSlot(int index, ISkill skill)
    {
        if (index < 0 || index >= assignedSkills.Length || skill == null) return;

        assignedSkills[index] = skill;

        if (skillButtons != null && index < skillButtons.Length)
        {
            skillButtons[index].UpdateUI(skill);
        }

      
    }


  
    public SkillSO GetAssignedSkill(int index)
    {
        if (index >= 0 && index < assignedSkills.Length && assignedSkills[index] != null)
            return assignedSkills[index].GetData(); 
        return null;
    }


}
