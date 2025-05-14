using UnityEngine;
using UnityEngine.UI;

public class PotionSlotUI : MonoBehaviour, IItemSlot

{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMPro.TextMeshProUGUI chargeText;

    private ItemSO currentPotion;
    private int currentCharges;
    private GameObject currentItemUI;

    public static PotionSlotUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public ItemSO ExtractItem()
    {
        ItemSO temp = currentPotion;
        currentPotion = null;
        currentCharges = 0;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (chargeText != null)
            chargeText.text = "";

        return temp;
    }



    public void SetPotion(ItemSO potion)
    {
        if (potion.itemType != ItemType.Potion)
        {
            Debug.LogWarning("Trying to assign non-potion to potion slot!");
            return;
        }

        if (currentPotion != null)
        {
            BackpackUI.Instance.TryAddItem(currentPotion);
        }

        currentPotion = potion;
        currentCharges = 0;

        
        if (currentItemUI != null)
        {
            Destroy(currentItemUI);
        }

       
        currentItemUI = new GameObject(potion.itemName);
        currentItemUI.transform.SetParent(this.transform, false);

        var image = currentItemUI.AddComponent<Image>();
        image.sprite = potion.icon;
        image.raycastTarget = true;

        currentItemUI.AddComponent<ItemUIData>().itemData = potion;
        currentItemUI.AddComponent<DraggableItemUI>();

        UpdateChargeUI();
    }



    public void AddCharge()
    {
        if (currentPotion != null)
        {
            currentCharges = Mathf.Min(currentCharges + 1, currentPotion.charges);
            UpdateChargeUI();
        }
    }

    public bool TryUsePotion(GameObject player)
    {
        if (currentPotion == null || currentCharges < currentPotion.chargesRequired)
            return false;

        currentCharges -= currentPotion.chargesRequired;
        UpdateChargeUI();

        player.GetComponent<HealthSystem>()?.HealOverTime(
            Mathf.RoundToInt(currentPotion.healAmount),
            currentPotion.duration
        );

        Debug.Log($"Using potion  Heal: {currentPotion.healAmount}, Duration: {currentPotion.duration}");

        return true;
    }


    private void UpdateChargeUI()
    {
        if (currentPotion != null)
        {
            chargeText.text = $"{currentCharges}/{currentPotion.charges}";
        }
        else
        {
            chargeText.text = "";
        }


    }
    public void ClearPotion()
    {
        currentPotion = null;
        currentCharges = 0;

        iconImage.sprite = null;
        iconImage.enabled = false;
        UpdateChargeUI();
    }



    public ItemSO GetPotion() => currentPotion;

    public bool HasPotion() => currentPotion != null;
}