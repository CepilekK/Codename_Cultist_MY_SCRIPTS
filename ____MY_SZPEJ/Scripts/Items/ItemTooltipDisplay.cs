using UnityEngine;
using TMPro;

public class ItemTooltipDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panelBackground;
    [SerializeField] private TextMeshProUGUI tooltipText;

    [Header("Setings")]
    [SerializeField] private Vector3 offset = new Vector3(20f, -20f);

    private Canvas canvas;
    private RectTransform panelRect;
    private ItemSO currentItem;
    private bool isHovering;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        panelRect = panelBackground.GetComponent<RectTransform>();
        HideTooltip();
    }

    private void Update()
    {
        if (isHovering && currentItem != null && Input.GetKey(KeyCode.LeftAlt))
        {
            ShowTooltipNow();
        }
        else
        {
            HideTooltip();
        }
    }

    private void ShowTooltipNow()
    {
        panelBackground.SetActive(true);
        tooltipText.text = TooltipGenerator.Generate(currentItem);

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition + offset,
            canvas.worldCamera,
            out mousePos
        );

        panelRect.anchoredPosition = mousePos;
    }

    public void ShowTooltip(ItemSO item)
    {
        currentItem = item;
        isHovering = true;
    }

    public void HideTooltip()
    {
        panelBackground.SetActive(false);
        isHovering = false;
        currentItem = null;
    }
}
