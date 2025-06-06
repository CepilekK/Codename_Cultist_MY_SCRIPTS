using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image healthFillImage;
    [SerializeField] private TextMeshProUGUI healthText;



    private void Start()
    {
       

        if (healthSystem != null)
        {
            healthSystem.OnDamaged += OnHealthDamaged;
            healthSystem.OnHealthChanged += OnHealthChanged;
        }

        UpdateUI();
    }
   




    private void OnHealthDamaged(object sender, System.EventArgs e)
    {
        UpdateUI();
    }

    private void OnHealthChanged(int current, int max)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthSystem != null)
        {
            healthFillImage.fillAmount = healthSystem.GetHealthNormalized();
            if (healthText != null)
            {
                int current = healthSystem.GetHealth();
                int max = healthSystem.GetMaxHealth();
                healthText.text = $"{current} / {max}";
            }
        }
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDamaged -= OnHealthDamaged;
            healthSystem.OnHealthChanged -= OnHealthChanged;
        }
    }
}
