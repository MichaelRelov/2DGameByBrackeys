using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _fireRate = 0f;
    [SerializeField] private LayerMask _whatToHit;
    [SerializeField] private Transform _bulletTrailPrefab;
    [SerializeField] private Transform _muzzleFlashPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] string weaponShootSound = "DefaultShoot";
    private int _damage = 35;
    public Transform hitPrefab;
    private float _timeToFire = 0;
    private float _timeToSpawnEffect = 0;
    private float _effectSpawnRate = 10f;
    public float camShakeAmt = 0.05f;
    public float camShakeLenght = 0.1f;
    CameraShake camShake;
    AudioManager audioManager;
    
    private void Awake()
    {
        _firePoint = transform.Find("FirePoint");
        if(_firePoint == null)
        {
            Debug.Log("!");
        }
    }

    private void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null)
        {
            Debug.LogError("No cameraShake!");
        }
        audioManager = AudioManager.instance;
    }

    private void Update()
    {
        if(_fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time > _timeToFire)
            {
                _timeToFire = Time.time + 1 / _fireRate;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(_firePoint.position.x, _firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100f, _whatToHit);

        if(hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(_damage);
            }
        }

        if (Time.time >= _timeToSpawnEffect)
        {
            Vector3 hitPosition;
            Vector3 hitNormal;
            if(hit.collider == null)
            {
                hitPosition = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPosition = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPosition, hitNormal);
            _timeToSpawnEffect = Time.time + 1 / _effectSpawnRate;
        }
    }

    private void Effect(Vector3 hitPosition, Vector3 hitNormal)
    {
        Transform trail = Instantiate(_bulletTrailPrefab, _firePoint.position, _firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null)
        {
            lr.SetPosition(0, _firePoint.position);
            lr.SetPosition(1, hitPosition);
        }

        Destroy(trail.gameObject, 0.04f);

        if(hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(hitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform clone = Instantiate(_muzzleFlashPrefab, _firePoint.position, _firePoint.rotation) as Transform;
        clone.parent = _firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.03f);

        camShake.Shake(camShakeAmt, camShakeLenght);
        audioManager.PlaySound(weaponShootSound);
    }
}
