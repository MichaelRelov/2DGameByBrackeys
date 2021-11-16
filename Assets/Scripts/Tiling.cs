using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    private int _offsetX = 2;
    private bool _hasARightBuddy = false;
    private bool _hasALeftBuddy = false;
    private float _spriteWidht = 0f;
    private Camera _camera;
    private Transform _myTransform;
    [SerializeField] private bool _reverseScale = false;

    private void Awake()
    {
        _camera = Camera.main;
        _myTransform = transform;
    }
    
    private void Start()
    {
        SpriteRenderer _sRenderer = GetComponent<SpriteRenderer>();
        _spriteWidht = _sRenderer.sprite.bounds.size.x;
    }

    private void Update()
    {
        if(_hasALeftBuddy == false || _hasARightBuddy == false)
        {
            float _camHorizontalExtend = _camera.orthographicSize * Screen.width / Screen.height;
            float _edgeVisiblePositionRight = (_myTransform.position.x + _spriteWidht / 2) - _camHorizontalExtend;
            float _edgeVisiblePositionLeft = (_myTransform.position.x - _spriteWidht / 2) + _camHorizontalExtend;

            if(_camera.transform.position.x >= _edgeVisiblePositionRight - _offsetX && _hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                _hasARightBuddy = true;
            }
            else if(_camera.transform.position.x <= _edgeVisiblePositionLeft + _offsetX && _hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                _hasALeftBuddy = true;
            }
        }
    }

    private void MakeNewBuddy(int rightOrLeft)
    {
        Vector3 newPosition = new Vector3(_myTransform.position.x + _spriteWidht * rightOrLeft, _myTransform.position.y, _myTransform.position.z);
        Transform newBuddy = Instantiate(_myTransform, newPosition, _myTransform.rotation) as Transform;
        if(_reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = _myTransform.parent;

        if(rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>()._hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>()._hasARightBuddy = true;
        }
    }
}
