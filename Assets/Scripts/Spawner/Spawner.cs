using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected bool isActive = true;
    [SerializeField] private string[] enemiesTag;
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private float generateTime;

    protected WaitForSeconds GenerateWaitTime;
    protected readonly List<GameObject> ActiveEnemies = new List<GameObject>();
    private CharacterStats _player;

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        foreach (var point in spawnPoints)
        {
            if (!point) continue;
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(point.transform.position, 0.1f);
        }
    }

    protected virtual void Awake()
    {
        _player = FindObjectOfType<PlayerStats>();
        _player.OnDead += ResetSpawner;
        GenerateWaitTime = new WaitForSeconds(generateTime);
    }

    private void OnEnable()
    {
        StartGenerating();
        GameManager.LoadSceneAction += StartGenerating;
    }

    private void OnDisable()
    {
        GameManager.LoadSceneAction -= StartGenerating;
    }

    protected abstract void StartGenerating();

    private void StopGenerating()
    {
        StopAllCoroutines();
    }

    protected virtual void ResetSpawner(GameObject deadCharacter)
    {
        RemoveAllEnemy();
        StopGenerating();
    }

    private void RemoveAllEnemy()
    {
        foreach (var enemy in ActiveEnemies)
        {
            enemy.SetActive(false);
            enemy.GetComponent<CharacterStats>().OnDead -= UpdateEnemyCount;
        }
        ActiveEnemies.Clear();
    }

    private void Spawn()
    {
        var newEnemy = EnemyPool.Instance.GetObject(GetRandomEnemiesTag(), transform);
        var spawnPoint = GetNewEnemyPosition();
        newEnemy.GetComponent<EnemyBrain>().Direction = spawnPoint.DefualtMoveSide;
        newEnemy.transform.position = spawnPoint.transform.position;
        newEnemy.GetComponent<CharacterStats>().OnDead += UpdateEnemyCount;
        ActiveEnemies.Add(newEnemy);
        newEnemy.SetActive(true);
    }

    private string GetRandomEnemiesTag()
    {
        return enemiesTag[Random.Range(0, enemiesTag.Length)];
    }

    protected virtual void UpdateEnemyCount(GameObject deadCharacter)
    {
        deadCharacter.GetComponent<CharacterStats>().OnDead -= UpdateEnemyCount;
        ActiveEnemies.Remove(deadCharacter);
        deadCharacter.SetActive(false);
    }

    private SpawnPoint GetNewEnemyPosition()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    protected IEnumerator SpawnWithDelay(WaitForSeconds waitTime)
    {
        yield return waitTime;
        if (!isActive) yield break;
        Spawn();
    }
}
