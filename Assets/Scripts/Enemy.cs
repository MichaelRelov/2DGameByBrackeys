using UnityEngine;
using System;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{

    [Serializable] public class EnemyStats
    {
        public int maxHealth = 100;
        
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 20;

        public void Init()
        {
            curHealth = maxHealth;
        }
    }


    public EnemyStats enemyStats = new EnemyStats();
    public Transform deathParticle;
    public float shakeAmt = 0.1f;
    public float shakeLenght = 0.1f;
    public string deathSoundName = "Explosion";
    public int moneyDrop = 10;
    public int _fallBoundary = -20;

    [Header("Optional: ")]
    [SerializeField] private StatusIndicator _statusIndicator;

    private void Start()
    {
        enemyStats.Init();

        if(_statusIndicator != null)
        {
            _statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }
        GameMaster.gm.onToggleUpgradeMenu += OnUpgaradeMenuToggle;

        if(deathParticle == null)
        {
            Debug.LogError("No particle");
        }
    }

    private void OnUpgaradeMenuToggle(bool active)
    {
        GetComponent<EnemyAI>().enabled = !active;
    }

    public void DamageEnemy(int damage)
    {
        enemyStats.curHealth -= damage;
        if (enemyStats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (_statusIndicator != null)
        {
            _statusIndicator.SetHealth(enemyStats.curHealth, enemyStats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer(enemyStats.damage);
            DamageEnemy(9999999);
        }
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgaradeMenuToggle;
    }
}
