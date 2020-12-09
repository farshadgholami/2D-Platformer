using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    private Dictionary<string, GameObject> _pool;
    private readonly List<GameObject> _poolList = new List<GameObject>();

    private static EnemyPool _instance;
    public static EnemyPool Instance
    {
        get
        {
            if (!_instance) _instance = FindObjectOfType<EnemyPool>();
            return _instance;
        }
    }
    
    private void Awake()
    {
        FillPrefabssList();
    }

    private void FillPrefabssList()
    {
        _pool = new Dictionary<string, GameObject>();
        foreach (var obj in objects) _pool.Add(obj.tag, obj);
    }

    public GameObject GetObject(string objectTag, Transform parent)
    {
        foreach (var obj in _poolList)
        {
            if (obj.activeSelf || !obj.CompareTag(objectTag)) continue;
            return obj;
        }

        return CreateNewObject(objectTag, parent);
    }

    private GameObject CreateNewObject(string objectTag, Transform parent)
    {
        if (_pool == null) FillPrefabssList();
        var newObj = Instantiate(_pool[objectTag], parent);
        _poolList.Add(newObj);
        return newObj;
    }
}
