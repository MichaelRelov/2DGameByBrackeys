using UnityEngine;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{ 
    public int _fallBoundary = -20;
    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";
    private AudioManager audioManager;
    private PlayerStats stats;
    [SerializeField] private StatusIndicator statusIndicator;

    private void Start()
    {
        stats = PlayerStats.instance;
        stats.curHealth = stats.maxHealth;
       
        if(statusIndicator == null)
        {
            Debug.LogError("No indicator");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgaradeMenuToggle;
        audioManager = AudioManager.instance;
        InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate); 
    }

    private void RegenHealth()
    {
        stats.curHealth += 1;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    private void Update()
    {
        if(transform.position.y <= _fallBoundary)
        {
            DamagePlayer(99999);
        }
    }

    public void OnUpgaradeMenuToggle(bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if(_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }

    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        if(stats.curHealth <= 0)
        {
            audioManager.PlaySound(deathSoundName);
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(damageSoundName);
        }
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgaradeMenuToggle;
    }
}
