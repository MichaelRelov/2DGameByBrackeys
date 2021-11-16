using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    private Vector3 _difference;
    private float _rotationZ;
    void Update()
    {
        _difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _difference.Normalize();

        _rotationZ = Mathf.Atan2(_difference.y, _difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _rotationZ);
    }
}