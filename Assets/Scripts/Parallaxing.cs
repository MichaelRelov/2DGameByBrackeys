using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    [SerializeField] private Transform[] _backgrounds;
    private float[] _parallaxScales;
    private float _smoothing = 10f;
    private Transform _camera;
    private Vector3 _previousCameraPosition;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    private void Start()
    {
        _previousCameraPosition = _camera.position;
        _parallaxScales = new float[_backgrounds.Length];

        for (int i = 0; i < _backgrounds.Length; i++)
        {
            _parallaxScales[i] = _backgrounds[i].position.z * -1;
        }
    }

    private void Update()
    {
        for(int i = 0; i < _backgrounds.Length; i++)
        {
            float parallax = (_previousCameraPosition.x - _camera.position.x) * _parallaxScales[i];
            float backgroundTargetPositionX = _backgrounds[i].position.x + parallax;
            Vector3 backgroundsTargetPosition = new Vector3(backgroundTargetPositionX, _backgrounds[i].position.y, _backgrounds[i].position.z);
            _backgrounds[i].position = Vector3.Lerp(_backgrounds[i].position, backgroundsTargetPosition, _smoothing * Time.deltaTime);
        }
        _previousCameraPosition = _camera.position;
    }
}