using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBarRect;
    [SerializeField] private Text _healthText;

    private void Start()
    {
        if(_healthBarRect == null)
        {
            Debug.LogError("Not Bar");
        }
        if (_healthText == null)
        {
            Debug.LogError("Not Text");
        }
    }

    public void SetHealth(int _cur, int _max)
    {
        float _value = (float)_cur / _max;
        _healthBarRect.localScale = new Vector3(_value, _healthBarRect.localScale.y, _healthBarRect.localScale.z);
        _healthText.text = _cur + "/" + _max + "HP";
    }
}