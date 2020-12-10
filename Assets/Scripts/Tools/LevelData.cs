using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "New Level Data")]
public class LevelData : ScriptableObject
{
    public DataModel data;

    public void Save(int levelNumber)
    {
        var bestLevelData = Load(levelNumber);
        data.bestScore = Mathf.Max(data.bestScore, bestLevelData.bestScore);
        data.bestTime = Mathf.Max((float) data.bestTime, (float)bestLevelData.bestTime);
        var levelData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"Level{levelNumber}", levelData);
        PlayerPrefs.Save();
    }

    public static DataModel Load(int levelNumber)
    {
        if (PlayerPrefs.HasKey($"Level{levelNumber}"))
        {
            var levelDataJson = PlayerPrefs.GetString($"Level{levelNumber}");
            return JsonUtility.FromJson<DataModel>(levelDataJson);
        }

        return null;
    }
    
    [Serializable]
    public class DataModel
    {
        public double bestTime;
        public int bestScore;
    }
}
