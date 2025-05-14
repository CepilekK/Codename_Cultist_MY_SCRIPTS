using UnityEngine;

public class DashPanel : MonoBehaviour
{
    [SerializeField] private DashHandler dashHandler;
    [SerializeField] private DashDatabase dashDatabase;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject dashOptionButtonPrefab;

    public void ShowPanel()
    {
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

        foreach (DashSO dash in dashDatabase.GetAllDashes())
        {
            GameObject obj = Instantiate(dashOptionButtonPrefab, buttonContainer);
            DashOptionButton option = obj.GetComponent<DashOptionButton>();
            option.Setup(dash, this);
        }
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectDash(DashSO dash)
    {
        dashHandler.SetDash(dash);
        HidePanel();
    }
}
