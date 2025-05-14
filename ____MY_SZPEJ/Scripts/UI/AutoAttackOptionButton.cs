using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoAttackOptionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image icon;

    private AutoAttackSO autoAttackData;
    private AutoAttackPanel panel;

    public void Setup(AutoAttackSO data, AutoAttackPanel parentPanel)
    {
        autoAttackData = data;
        panel = parentPanel;

        if (label != null)
            label.text = data.attackName;

        if (icon != null)
            icon.sprite = data.icon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        panel.SelectAutoAttack(autoAttackData);
    }
}
