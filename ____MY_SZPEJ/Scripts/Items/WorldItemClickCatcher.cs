using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldItemClickCatcher : MonoBehaviour
{
    public static event Action<ItemSO> OnItemPicked;

    private void OnMouseDown()
    {
        TryPickup();
    }


    private void TryPickup()
    {
        var itemWorld = GetComponent<ItemWorld>();
        if (itemWorld == null || itemWorld.GetItemData() == null) return;

        OnItemPicked?.Invoke(itemWorld.GetItemData());
        Destroy(gameObject);

    }
}
