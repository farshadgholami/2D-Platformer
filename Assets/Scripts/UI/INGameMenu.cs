using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class INGameMenu : MonoBehaviour
{
    private bool active;
    public static INGameMenu sSingletone;

    private void Awake()
    {
        if (sSingletone == null)
            sSingletone = this;
        else
            Destroy(gameObject);

        active = false;
        gameObject.SetActive(active);
    }

    public void show()
    {
        active = !active;
        gameObject.SetActive(active);
        if (active)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        active = false;
        Time.timeScale = 1;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void LastCheckpoint()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        active = false;
        GameManager.LoadLastCheckPoint();
    }
}
