using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    private int _moveSpeed = 230;

    private void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _moveSpeed);
        Destroy(gameObject, 1f);
    }
}