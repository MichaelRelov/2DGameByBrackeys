using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    [SerializeField] private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    [SerializeField] private int startingMoney = 0;
    public static int Money;
    public Transform _playerPrefab;
    public Transform _spawnPoint;
    public int _spawnDelay = 3;
    public Transform _spawnPrefab;
    public string respawnSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";
    public CameraShake cameraShake;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private WaveSpawner waveSpawner;
    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;
    private AudioManager audioManager;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    private void Start()
    {
        if(cameraShake == null)
        {
            Debug.LogError("No camera shake");
        }
        _remainingLives = maxLives;
        Money = startingMoney;

        audioManager = AudioManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public void EndGame()
    {
        audioManager.PlaySound(gameOverSoundName);
        gameOverUI.SetActive(true);
    }

    public IEnumerator _RespawnPlayer()
    {
        audioManager.PlaySound(respawnSoundName);
        yield return new WaitForSeconds(_spawnDelay);
        Transform clone = Instantiate(_spawnPrefab, _spawnPoint.position, _spawnPoint.rotation);
        Instantiate(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
        audioManager.PlaySound(spawnSoundName);
        Destroy(clone.gameObject, 3f);
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        _remainingLives--;
        if(_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        audioManager.PlaySound(_enemy.deathSoundName);
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");
        Transform _clone = Instantiate(_enemy.deathParticle, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 2f);
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLenght);
        Destroy(_enemy.gameObject);
    }
}
