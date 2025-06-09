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
        CompletionPanel.SetActive(true);
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
