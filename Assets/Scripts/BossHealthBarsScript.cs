using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarsScript : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public GameObject CompletionPanel;
    private AudioSource backgroundMusic;
    public AudioSource completionAudio; // Reference to the completion audio

    private void Start()
    {
        GameObject musicObject = GameObject.Find("Music");
        if (musicObject != null)
        {
            backgroundMusic = musicObject.GetComponent<AudioSource>();
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void GameComplete()
    {
        if (GameplayState.IsTerminal)
        {
            return;
        }

        GameplayState.BeginTerminalState();
        GameplayState.DisablePlayerGameplay();
        GameplayState.StopGameplayAudio();
        Time.timeScale = 0f;
        AudioListener.pause = false;
        PauseManager pauseManager = FindObjectOfType<PauseManager>();
        if (pauseManager != null && pauseManager.pauseButton != null)
        {
            pauseManager.pauseButton.interactable = false;
        }
        Level5Pause level5Pause = FindObjectOfType<Level5Pause>();
        if (level5Pause != null && level5Pause.pauseButton != null)
        {
            level5Pause.pauseButton.interactable = false;
        }

        if (CompletionPanel != null)
        {
            CompletionPanel.SetActive(true);
        }
        StopMusic();
        PlayCompletionAudio();
    }

    private void StopMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
    }

    private void PlayCompletionAudio()
    {
        if (completionAudio != null)
        {
            completionAudio.Play();
        }
    }
}
