using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private SkillBarManager skillBarManager;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;
    [SerializeField] private SkillSelectionPanel selectionPanel;
    private int slotIndex = -1;
    private ISkill assignedSkill;


    private void Start()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(() => selectionPanel.ShowPanel(slotIndex));
        else
            Debug.LogError("SkillButton.cs: Brak przypisanego Buttona!");
    }


    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void UpdateUI(ISkill skill)
    {
        if (skill == null || skill.GetData() == null)
        {
            iconImage.sprite = null;
            nameText.text = "";
            return;
        }

        SkillSO data = skill.GetData();
        iconImage.sprite = data.icon;
        nameText.text = data.skillName;
    }


    private void OnButtonClicked()
    {
        if (skillBarManager != null && slotIndex >= 0)
        {
            skillBarManager.UseSkillFromSlot(slotIndex);
        }
        else
        {
            Debug.LogWarning("SkillBarManager lub slotIndex nie przypisany!");
        }
    }
}
