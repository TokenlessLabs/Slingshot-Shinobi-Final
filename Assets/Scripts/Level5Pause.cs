using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class Level5Pause : MonoBehaviour
{
    public GameObject pauseImage; 
    public Button resumeButton; 
    public Button pauseButton; 
    public Button restartButton; 
    public Button quitButton; 

    private bool isPaused = false;

    void Start()
    {
        GameplayState.Reset();
        Time.timeScale = 1f;
        AudioListener.pause = false;
        resumeButton.onClick.AddListener(ResumeGame);
        pauseButton.onClick.AddListener(TogglePause); 
        restartButton.onClick.AddListener(RestartLevel); 
        quitButton.onClick.AddListener(QuitToTitleScreen); 

        pauseImage.SetActive(false);
    }

    void TogglePause()
    {
        if (GameplayState.IsTerminal || GameplayState.IsPowerupOpen)
        {
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; 
        AudioListener.pause = true;
        GameplayState.SetPaused(true);
        pauseImage.SetActive(true); 
        isPaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; 
        AudioListener.pause = false;
        GameplayState.SetPaused(false);
        pauseImage.SetActive(false); 
        isPaused = false;
    }

    void RestartLevel()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameplayState.SetPaused(false);
        GameplayState.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void QuitToTitleScreen()
    {
        Debug.Log("QuitToTitleScreen method called.");
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameplayState.SetPaused(false);
        GameplayState.Reset();
        SceneManager.LoadScene("TitleScreen");
    }


}