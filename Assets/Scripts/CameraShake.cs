using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCamera;
    private float shakeAmount = 0f;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.1f, 0.2f);
        }
    }

    public void Shake(float amt, float lenght)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", lenght);
    }

    private void BeginShake()
    {
        if(shakeAmount > 0)
        {
            Vector3 camPos = mainCamera.transform.position;

            float offsetx = Random.value * shakeAmount * 2 - shakeAmount;
            float offsety = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetx;
            camPos.y += offsety;

            mainCamera.transform.position = camPos;
        }
    }

    private void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCamera.transform.localPosition = Vector3.zero;
    }
}
