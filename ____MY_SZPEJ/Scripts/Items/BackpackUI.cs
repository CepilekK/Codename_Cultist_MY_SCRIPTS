using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{
    [SerializeField] private Transform uiCanvasRoot; 
    [SerializeField] private PlayerSO playerData;      
    [SerializeField] private GameObject slotPrefab;    // Prefab pojedynczego slotu plecaka
    [SerializeField] private Transform slotContainer;  // Kontener (GridLayout) na sloty w UI
    [SerializeField] private GameObject backpackPanel; 
    [SerializeField] private GameObject itemWorldPrefab;

    public static BackpackUI Instance { get; private set; }
    private GameObject currentDraggedItem;  

    public Transform GetCanvasRoot() => uiCanvasRoot;
    public bool IsOpen() => backpackPanel.activeSelf;
    public bool HasDraggedItem() => currentDraggedItem != null;

    private void Awake()
    {
     
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

     
        WorldItemClickCatcher.OnItemPicked += HandleWorldItemPicked;
    }

    private void OnDestroy()
    {
      
        WorldItemClickCatcher.OnItemPicked -= HandleWorldItemPicked;
    }

    private void Start()
    {
        GenerateBackpackGrid();
        backpackPanel.SetActive(false);  
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleBackpack();
        }
    }

    
    public bool TryAddItem(ItemSO item)
    {
        foreach (Transform slot in slotContainer)
        {
            BackpackSlot slotScript = slot.GetComponent<BackpackSlot>();
            if (slotScript != null && !slotScript.HasItem())
            {
                // Utwórz obiekt ikony przedmiotu w UI
                GameObject itemObj = new GameObject(item.itemName);
                itemObj.transform.SetParent(slot, worldPositionStays: false);

                // Dodaj komponenty UI i danych
                Image image = itemObj.AddComponent<Image>();
                image.sprite = item.icon;
                image.raycastTarget = true;             
                itemObj.AddComponent<ItemUIData>().itemData = item;             
                itemObj.AddComponent<DraggableItemUI>();
                itemObj.AddComponent<ItemTooltipUIHover>();

                slotScript.SetItem(itemObj);
                return true;
            }
        }
        Debug.Log("Brak miejsca w plecaku!");
        return false;
    }


    public void DropItemToWorld(ItemSO item)
    {
        if (item == null || itemWorldPrefab == null)
        {
            Debug.LogWarning("Nie można zdropić itemu – brak danych lub prefab itemWorld!");
            return;
        }

        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector3 spawnPos = playerPos + Vector3.forward * 1.5f;

        GameObject worldObj = Instantiate(itemWorldPrefab, spawnPos, Quaternion.identity);

       
        ItemWorld worldItem = worldObj.GetComponent<ItemWorld>();
        if (worldItem != null)
        {
            worldItem.SetItem(item); 
        }
    }



    public void StartDraggingItem(ItemSO item)
    {
        if (currentDraggedItem != null) return; 

      
        GameObject dragItemObj = new GameObject(item.itemName);
        dragItemObj.transform.SetParent(uiCanvasRoot, worldPositionStays: false);

        // Dodaj komponent graficzny ikony
        Image img = dragItemObj.AddComponent<Image>();
        img.sprite = item.icon;
        img.raycastTarget = true;

        // Przypisz dane przedmiotu do obiektu UI
        dragItemObj.AddComponent<ItemUIData>().itemData = item;
        dragItemObj.AddComponent<ItemTooltipUIHover>();
        // Dodaj komponent obsługujący drag & drop i rozpocznij przeciąganie
        DraggableItemUI drag = dragItemObj.AddComponent<DraggableItemUI>();
        drag.BeginDragManually();

        currentDraggedItem = dragItemObj;

    }

   
    public void ClearDraggedItem()
    {
        currentDraggedItem = null;
    }

    private void ToggleBackpack()
    {
        backpackPanel.SetActive(!backpackPanel.activeSelf);
    }

 
    private void GenerateBackpackGrid()
    {
 
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

      
        GridLayoutGroup layout = slotContainer.GetComponent<GridLayoutGroup>();
        if (layout != null)
        {
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = playerData.backpackWidth;
        }

       
        int totalSlots = playerData.backpackWidth * playerData.backpackHeight;
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
          
            if (slotGO.GetComponent<BackpackSlot>() == null)
            {
                slotGO.AddComponent<BackpackSlot>();
            }
        }
    }


    private void HandleWorldItemPicked(ItemSO item)
    {
        if (!IsOpen())
        {
            TryAddItem(item);
        }
        else
        {
            StartDraggingItem(item);
        }
    }
}

