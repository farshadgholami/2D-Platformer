using System.Collections;
using UnityEngine;

public class ObjectBlinker : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private float enableDuration;
    [SerializeField] private float disableDuration;

    private WaitForSeconds _enableWaitTime;
    private WaitForSeconds _disableWaitTime;
    private Renderer _renderer;
    private Collider2D _collider2D;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider2D = GetComponent<Collider2D>();
        StartCoroutine(Blink());
        _enableWaitTime = new WaitForSeconds(enableDuration);
        _disableWaitTime = new WaitForSeconds(disableDuration);
    }

    private IEnumerator Blink()
    {
        while (active)
        {
            yield return _enableWaitTime;
            SetVisiblity(false);
            yield return _disableWaitTime;
            SetVisiblity(true);
        }
    }

    private void SetVisiblity(bool value)
    {
        _renderer.enabled = value;
        _collider2D.enabled = value;
    }
}