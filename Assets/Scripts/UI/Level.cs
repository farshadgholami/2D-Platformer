using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private TextMeshPro levelTitle;
    [SerializeField] private TextMeshPro bestScore;
    [SerializeField] private TextMeshPro bestTime;
    [SerializeField] private TextMeshPro guideText;
    
    private bool _isOnDoor;
    private bool _isLevelLock;
    private bool _isLevelDone;

    private void Awake()
    {
        CheckLevelProgress();
        levelTitle.SetText($"LEVEL {levelNumber}");
    }

    private void Update()
    {
        if (!_isOnDoor || _isLevelLock) return;
        if(Input.GetKeyDown(KeyCode.E))
            LoadLevel();
    }

    private void LoadLevel()
    {
        GameManager.lastLevelLoaded = levelNumber;
        SceneManager.LoadScene($"Level {levelNumber - 1}");
    }

    public void SetOnDoor(bool value)
    {
        _isOnDoor = value;
        SetShowGuide(value);
    }

    private void SetShowGuide(bool value)
    {
        guideText.gameObject.SetActive(value);
        guideText.text = _isLevelLock ? "IS LOCK" : "PRESS \"E\"";
        guideText.color = _isLevelLock ? Color.black : Color.white;
    }

    private void CheckLevelProgress()
    {
        var lastLevel = !PlayerPrefs.HasKey("LastLevel") ? 0 : PlayerPrefs.GetInt("LastLevel");
        _isLevelLock = levelNumber > lastLevel + 1; 
        
        var levelData = LevelData.Load(levelNumber);
        if (levelData == null) return;
        
        bestScore.text = levelData.bestScore.ToString();
        TimeSpan time = TimeSpan.FromSeconds(levelData.bestTime);
        bestTime.text = time.ToString(@"mm\:ss");
    }
}
