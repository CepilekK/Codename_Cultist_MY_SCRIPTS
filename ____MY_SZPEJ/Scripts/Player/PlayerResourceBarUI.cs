using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceBarUI : MonoBehaviour
{
    [SerializeField] private ResourceSystem resourceSystem;
    [SerializeField] private Image resourceFillImage;

    private void Start()
    {
        if (resourceSystem != null)
        {
            resourceSystem.OnResourceChanged += OnResourceChanged;
        }

        UpdateUI();
    }

    private void OnResourceChanged(int current, int max)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (resourceSystem != null)
        {
            resourceFillImage.fillAmount = resourceSystem.GetResourceNormalized();
        }
    }

    private void OnDestroy()
    {
        if (resourceSystem != null)
        {
            resourceSystem.OnResourceChanged -= OnResourceChanged;
        }
    }
}
