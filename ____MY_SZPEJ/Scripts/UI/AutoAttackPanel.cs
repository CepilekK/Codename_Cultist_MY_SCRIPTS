using UnityEngine;

public class AutoAttackPanel : MonoBehaviour
{
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private AutoAttackDatabase autoAttackDatabase;
    [SerializeField] private PlayerAutoAttackHandler autoAttackHandler;
    [SerializeField] private GameObject panelRoot; 

    private void OnEnable()
    {
        GenerateOptions();
    }

    private void GenerateOptions()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        AutoAttackSO[] allAttacks = autoAttackDatabase.GetAllAttacks();
        if (allAttacks == null) return;

        foreach (AutoAttackSO attack in allAttacks)
        {
            if (attack == null || !attack.isUnlocked) continue;

            GameObject btnObj = Instantiate(optionButtonPrefab, buttonContainer);
            AutoAttackOptionButton btn = btnObj.GetComponent<AutoAttackOptionButton>();
            btn.Setup(attack, this);
        }
    }

    public void SelectAutoAttack(AutoAttackSO selected)
    {
        autoAttackHandler.SetAutoAttack(selected);
        ClosePanel();
    }

    public void ClosePanel()
    {
        panelRoot.SetActive(false);
    }
}
