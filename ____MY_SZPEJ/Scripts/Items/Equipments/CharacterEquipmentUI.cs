using UnityEngine;
using System;

public class CharacterEquipmentUI : MonoBehaviour
{
    public event Action OnEquipmentChanged;

    public static CharacterEquipmentUI Instance { get; private set; }

    [Header("Kontener ze slotami")]
    [SerializeField] private GameObject container;

    [Header("Sloty broni")]
    [SerializeField] private WeaponSlotUI mainHandSlot;
    [SerializeField] private WeaponSlotUI offHandSlot;

    [Header("Sloty zbroi i bi¿uterii")]
    [SerializeField] private EquipmentSlotUI helmetSlot;
    [SerializeField] private EquipmentSlotUI shoulderSlot;
    [SerializeField] private EquipmentSlotUI glovesSlot;
    [SerializeField] private EquipmentSlotUI chestSlot;
    [SerializeField] private EquipmentSlotUI pantsSlot;
    [SerializeField] private EquipmentSlotUI bootsSlot;
    [SerializeField] private EquipmentSlotUI amuletSlot;
    [SerializeField] private EquipmentSlotUI ringSlot1;
    [SerializeField] private EquipmentSlotUI ringSlot2;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        container.SetActive(false); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            container.SetActive(!container.activeSelf);
        }
    }

    public IItemSlot GetSlotFor(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Helmet => helmetSlot,
            ItemType.Shoulders => shoulderSlot,
            ItemType.Gloves => glovesSlot,
            ItemType.ChestArmor => chestSlot,
            ItemType.Pants => pantsSlot,
            ItemType.Boots => bootsSlot,
            ItemType.Amulet => amuletSlot,
            ItemType.Ring => ringSlot1.IsEmpty() ? ringSlot1 : ringSlot2,

            // Obs³uga typów broni
            ItemType.TwoHandedWeapon => mainHandSlot,
            ItemType.Staff => mainHandSlot,
            ItemType.Bow => mainHandSlot,
            ItemType.OneHandedWeapon => mainHandSlot.IsEmpty() ? mainHandSlot : offHandSlot,
            ItemType.Wand => mainHandSlot.IsEmpty() ? mainHandSlot : offHandSlot,
            ItemType.Spear => mainHandSlot,
            ItemType.Shield => offHandSlot,
            ItemType.Quiver => offHandSlot,

            _ => null
        };
    }

    public void RaiseEquipmentChanged()
    {
        OnEquipmentChanged?.Invoke();
    }

    public WeaponSlotUI GetMainHandSlot() => mainHandSlot;
    public WeaponSlotUI GetOffHandSlot() => offHandSlot;
}
