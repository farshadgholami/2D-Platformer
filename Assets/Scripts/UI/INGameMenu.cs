using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class INGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstPage;
    [SerializeField] private GameObject optionPage;
    [SerializeField] private Image wallJumpState;
    [SerializeField] private Image doubleJumpState;
    [SerializeField] private Image sprintState;
    private bool active;
    public static INGameMenu sSingletone;
    private GameObject _player;
    private CharacterJump _characterJump;
    private CharacterMovement _characterMovement;

    private void Awake()
    {
        FindPlayer();
        SetOptionState();
        
        if (sSingletone == null)
            sSingletone = this;
        else
            Destroy(gameObject);

        active = false;
        gameObject.SetActive(active);
    }

    private void FindPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterJump = _player.GetComponent<CharacterJump>();
        _characterMovement = _player.GetComponent<CharacterMovement>();
    }

    private void SetOptionState()
    {
        wallJumpState.color = _characterJump.HasWallJump ? Color.yellow : Color.gray;
        doubleJumpState.color = _characterJump.HasDoubleJump ? Color.yellow : Color.gray;
        sprintState.color = _characterMovement.HasSprint ? Color.yellow : Color.gray;
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
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    
    public void Option()
    {
        firstPage.SetActive(false);
        optionPage.SetActive(true);
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

    public void ChangeWallJump()
    {
        _characterJump.HasWallJump = !_characterJump.HasWallJump;
        wallJumpState.color = _characterJump.HasWallJump ? Color.yellow : Color.gray;
    }
    
    public void ChangeDoubleJump()
    {
        _characterJump.HasDoubleJump = !_characterJump.HasDoubleJump;
        doubleJumpState.color = _characterJump.HasDoubleJump ? Color.yellow : Color.gray;
    }
    
    public void ChangeSprint()
    {
        _characterMovement.HasSprint = !_characterMovement.HasSprint;
        sprintState.color = _characterMovement.HasSprint ? Color.yellow : Color.gray;
    }
    
    public void Back()
    {
        firstPage.SetActive(true);
        optionPage.SetActive(false);
    }

    private void OnDisable()
    {
        Back();
    }
}
