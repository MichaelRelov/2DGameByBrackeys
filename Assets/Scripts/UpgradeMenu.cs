using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text speedText;
    [SerializeField] private int healthMultiplier = 20;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private int upgradeHealthCost = 30;
    [SerializeField] private int upgradeSpeedCost = 50;

    private PlayerStats stats;

    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    public void UpdateValues()
    {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
    }

    public void UpgradeHealth()
    {
        if(GameMaster.Money < upgradeHealthCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        stats.maxHealth += healthMultiplier;
        GameMaster.Money -= upgradeHealthCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        if (GameMaster.Money < upgradeSpeedCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        stats.movementSpeed += speedMultiplier;
        GameMaster.Money -= upgradeSpeedCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }
}
