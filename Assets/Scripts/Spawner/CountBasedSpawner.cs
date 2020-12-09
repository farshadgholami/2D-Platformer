using System.Collections;
using UnityEngine;

public class CountBasedSpawner : Spawner
{
    [SerializeField] private int maxCount;
    [SerializeField] private float regenerateTime;
    
    private bool _isFullInitiated;
    private WaitForSeconds _regenerateWaitTime;

    protected override void Awake()
    {
        base.Awake();
        _regenerateWaitTime = new WaitForSeconds(regenerateTime);
    }

    protected override void StartGenerating()
    {
        StartCoroutine(InitialGeneration());
    }

    private IEnumerator InitialGeneration()
    {
        while (ActiveEnemies.Count < maxCount)
        {
            yield return SpawnWithDelay(GenerateWaitTime);
        }

        _isFullInitiated = true;
    }

    protected override void ResetSpawner(GameObject deadCharacter)
    {
        base.ResetSpawner(deadCharacter);
        _isFullInitiated = false;
    }

    protected override void UpdateEnemyCount(GameObject deadCharacter)
    {
        base.UpdateEnemyCount(deadCharacter);
        if (!_isFullInitiated) return;
        StartCoroutine(SpawnWithDelay(_regenerateWaitTime));
    }
}
