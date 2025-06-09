using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class Level5GameOver : MonoBehaviour
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
        Time.timeScale = 0f; 
        gameOverPanel.SetActive(true); 
    }

    void RestartLevel()
    {
        Debug.Log("RestartLevel method called."); 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    void QuitToTitleScreen()
    {
        Debug.Log("QuitToTitleScreen method called."); 
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen"); 
    }
}
