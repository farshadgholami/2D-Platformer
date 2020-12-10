using System.Collections;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    private bool _isActive = true;

    private void Start()
    {
        StartCoroutine(CalculateTime());
    }

    public void Stop()
    {
        _isActive = false;
    }

    private IEnumerator CalculateTime()
    {
        levelData.data.bestTime = 0;
        while (_isActive)
        {
            levelData.data.bestTime += Time.deltaTime;
            yield return null;
        }
    }
}
