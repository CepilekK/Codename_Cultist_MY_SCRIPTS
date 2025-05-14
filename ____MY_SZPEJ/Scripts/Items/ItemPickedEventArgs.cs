using System;

public class ItemPickedEventArgs : EventArgs
{
    public ItemSO itemData;

    public ItemPickedEventArgs(ItemSO itemData)
    {
        this.itemData = itemData;
    }
}
