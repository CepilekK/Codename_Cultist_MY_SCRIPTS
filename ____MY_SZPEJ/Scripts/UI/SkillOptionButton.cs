using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillOptionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image icon;

    private SkillSO skillData;
    private SkillSelectionPanel parentPanel;

    public void Setup(SkillSO data, SkillSelectionPanel panel)
    {
        skillData = data;
        parentPanel = panel;

        if (label != null) label.text = data.skillName;
        if (icon != null) icon.sprite = data.icon;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        parentPanel.SelectSkill(skillData);
    }
}
