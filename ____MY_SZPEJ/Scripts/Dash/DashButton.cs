using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashButton : MonoBehaviour
{
    [SerializeField] private DashHandler dashHandler;
    [SerializeField] private DashPanel dashPanel;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(TogglePanel);
        }
        else
        {
            Debug.LogError("DashButton: Brak przypisanego Buttona!");
        }
    }

    private void Start()
    {
        DashSO dash = dashHandler.GetCurrentDash();
        if (dash != null)
        {
            UpdateUI(dash);
        }
        else
        {
            ClearUI();
        }

        dashHandler.OnDashChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        if (dashHandler != null)
        {
            dashHandler.OnDashChanged -= UpdateUI;
        }
    }

    private void TogglePanel()
    {
        if (dashPanel != null)
        {
            if (dashPanel.gameObject.activeSelf)
                dashPanel.HidePanel();
            else
                dashPanel.ShowPanel();
        }
    }

    private void UpdateUI(DashSO dash)
    {
        if (dash == null)
        {
            ClearUI();
            return;
        }

        iconImage.sprite = dash.icon;
        nameText.text = dash.dashName;
    }

    private void ClearUI()
    {
        iconImage.sprite = null;
        nameText.text = "";
    }
}
