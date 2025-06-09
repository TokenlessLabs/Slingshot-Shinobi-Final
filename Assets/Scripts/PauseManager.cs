using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class PauseManager : MonoBehaviour
{
    public GameObject pauseImage; 
    public Button resumeButton; 
    public Button pauseButton; 
    public Button restartButton; 
    public Button quitButton; 

    private bool isPaused = false;
    private List<AudioSource> audioSources;

    void Start()
    {
        Debug.Log("PauseManager script has started.");
        resumeButton.onClick.AddListener(ResumeGame);
        pauseButton.onClick.AddListener(TogglePause); 
        restartButton.onClick.AddListener(RestartLevel); 
        quitButton.onClick.AddListener(QuitToTitleScreen); 
        pauseImage.SetActive(false);
        audioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
    }

    void TogglePause()
    {
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
        pauseImage.SetActive(true); 
        isPaused = true;
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; 
        pauseImage.SetActive(false); 
        isPaused = false;
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
        }
    }

    void RestartLevel()
    {
        Time.timeScale = 1f;
        SpawnManager script = FindObjectOfType<SpawnManager>();
        script.disabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void QuitToTitleScreen()
    {
        Time.timeScale = 1f;
        SpawnManager script = FindObjectOfType<SpawnManager>();
        script.disabled = true;
        SceneManager.LoadScene("TitleScreen");
    }
}
