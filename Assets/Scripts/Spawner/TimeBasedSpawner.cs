using System.Collections;

public class TimeBasedSpawner : Spawner
{
    protected override void StartGenerating()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return SpawnWithDelay(GenerateWaitTime);
        }
    }
}
