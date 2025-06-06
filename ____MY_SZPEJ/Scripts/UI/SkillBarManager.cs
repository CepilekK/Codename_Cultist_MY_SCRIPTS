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
        int slotIndex = 0;
        foreach (SkillSO skillSO in skillDatabase.GetAllSkills())
        {
            if (skillSO.isUnlocked && slotIndex < skillButtons.Length)
            {
                ISkill skillInstance = SkillFactory.CreateSkill(skillSO); // użyj własnej fabryki, np. z pliku SkillFactory.cs
                AssignSkillToSlot(slotIndex, skillInstance);
                slotIndex++;
            }
        }
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

        // cooldown check — jeśli jest na cooldownie, nie używaj skilla
        if (PlayerSkillManager.Instance.IsSkillOnCooldown(skill))
        {
            Debug.Log("Skill jest na cooldownie!");
            return;
        }

        ResourceSystem resourceSystem = playerStateMachine.GetComponent<ResourceSystem>();
        if (resourceSystem == null) return;

        int cost = skill.GetResourceCost();

        // najpierw sprawdź czy masz zasoby
        if (!resourceSystem.HasEnough(cost))
        {
            Debug.Log("Za mało zasobów!");
            return;
        }

        // wszystko OK — teraz już możesz rzucać skill
        playerStateMachine.CastSkill(skill);
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
