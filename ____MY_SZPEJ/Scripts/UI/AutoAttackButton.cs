using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoAttackButton : MonoBehaviour
{
    [SerializeField] private AutoAttackHandler autoAttackHandler;
    [SerializeField] private GameObject autoAttackPanel;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;

    private void Awake()
    {
        button.onClick.AddListener(TogglePanel);
    }

    private void Start()
    {
         autoAttackHandler.OnAutoAttackChanged += UpdateUI;
        AutoAttackSO attack = autoAttackHandler.GetAutoAttack();

        if (attack != null)
        {
            UpdateUI(attack);
        }
        else
        {
            Debug.LogWarning("Brak aktywnego autoataku w AutoAttackHandler!");
            ClearUI();
        }
    }

    public void UpdateUI(AutoAttackSO attack)
    {
        if (attack == null) return;
        iconImage.sprite = attack.icon;
        nameText.text = attack.attackName;
    }

    private void ClearUI()
    {
        iconImage.sprite = null;
        nameText.text = "";
    }

    private void TogglePanel()
    {
        if (autoAttackPanel != null)
        {
            autoAttackPanel.SetActive(!autoAttackPanel.activeSelf);
        }
    }

    private void OnDestroy()
    {
        if (autoAttackHandler != null)
        {
            autoAttackHandler.OnAutoAttackChanged -= UpdateUI;
        }
    }

}