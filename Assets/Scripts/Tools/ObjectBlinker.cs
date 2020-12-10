using System;
using System.Collections;
using UnityEngine;

public class ObjectBlinker : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private float enableDuration;
    [SerializeField] private float disableDuration;
    [SerializeField] private Renderer visualRenderer;
    [SerializeField] private Behaviour[] components;

    private WaitForSeconds _enableWaitTime;
    private WaitForSeconds _disableWaitTime;

    private void Awake()
    {
        _enableWaitTime = new WaitForSeconds(enableDuration);
        _disableWaitTime = new WaitForSeconds(disableDuration);
    }

    private void OnEnable()
    {
        StartCoroutine(Blink());
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
        visualRenderer.enabled = value;
        foreach (var component in components) component.enabled = value;
    }
}