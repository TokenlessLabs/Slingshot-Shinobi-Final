using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; 
    public Button restartButton; 
    public Button quitButton; 

    void Start()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel); 
        quitButton.onClick.AddListener(QuitToTitleScreen);
    }

    public void ShowGameOverPanel()
    {
        GameplayState.BeginTerminalState();
        GameplayState.StopGameplayAudio();
        AudioListener.pause = false;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true); 
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
