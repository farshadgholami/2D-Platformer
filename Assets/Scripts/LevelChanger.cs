using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    private void OnFadeComplete()
    {
        levelData.Save(GameManager.lastLevelLoaded);
        GameManager.instance.SaveLastLevel();
        GameManager.lastLevelLoaded++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnFadeComplete();
        }
    }
}
