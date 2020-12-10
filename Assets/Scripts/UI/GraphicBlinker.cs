using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GraphicBlinker : MonoBehaviour
{
    [SerializeField] private float enableDuration = 2;
    [SerializeField] private float disableDuration = 1;
    private WaitForSeconds _enableWaitTime;
    private WaitForSeconds _disableWaitTime;
    private Graphic _graphics;
    
    private void Start()
    {
        _graphics = GetComponent<Graphic>();
        StartCoroutine(Blink());
        _enableWaitTime = new WaitForSeconds(enableDuration);
        _disableWaitTime = new WaitForSeconds(disableDuration);
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return _enableWaitTime;
            _graphics.enabled = false;
            yield return _disableWaitTime;
            _graphics.enabled = true;
        }
    }
}
