using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI pointsText;
    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Player.OnTakeDamage += UpdateHealthText;
        Player.OnGainPoints += UpdatePointsText;
    }

    public void Dispose()
    {
        Player.OnTakeDamage -= UpdateHealthText;
        Player.OnGainPoints -= UpdatePointsText;
    }

    public void UpdateHealthText(int newHealth)
    {
        healthText.SetText("{0}", newHealth);
        if(newHealth == 0)
        {
            Dispose();
        }
    }

    public void UpdatePointsText(int newPoints)
    {
        pointsText.SetText("{0}", newPoints);
    }

    
}
