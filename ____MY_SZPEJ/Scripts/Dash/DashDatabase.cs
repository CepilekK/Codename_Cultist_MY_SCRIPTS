using UnityEngine;

public class DashDatabase : MonoBehaviour
{
    [SerializeField] private DashSO[] dashAssets;

    public DashSO GetDashById(int id)
    {
        foreach (var dash in dashAssets)
        {
            if (dash != null && dash.GetInstanceID() == id)
                return dash;
        }
        return null;
    }

    public DashSO[] GetAllDashes()
    {
        return dashAssets;
    }
}
