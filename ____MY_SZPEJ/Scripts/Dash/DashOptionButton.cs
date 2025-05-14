using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashOptionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;

    private DashSO dashData;
    private DashPanel parentPanel;

    public void Setup(DashSO data, DashPanel panel)
    {
        dashData = data;
        parentPanel = panel;

        if (iconImage != null) iconImage.sprite = data.icon;
        if (nameText != null) nameText.text = data.dashName;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        parentPanel.SelectDash(dashData);
    }
}
