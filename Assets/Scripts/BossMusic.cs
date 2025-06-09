using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMusic : MonoBehaviour
{
    public AudioClip levelMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && levelMusic != null)
        {
            audioSource.clip = levelMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Level 1") 
        {
            StopMusic();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
