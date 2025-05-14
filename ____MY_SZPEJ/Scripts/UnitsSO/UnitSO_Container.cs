using UnityEngine;

public class UnitSO_Container : MonoBehaviour
{
    [SerializeField] private UnitSO unitSO;

    public UnitSO GetUnitSO()
    {
        return unitSO;
    }

    public void SetUnitSO(UnitSO newUnitSO)
    {
        unitSO = newUnitSO;
    }
}
