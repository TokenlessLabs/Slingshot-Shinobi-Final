using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMusic : MonoBehaviour
{
    public AudioClip levelMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            if (levelMusic != null)
            {
                audioSource.clip = levelMusic;
            }

            audioSource.loop = true;
            if (audioSource.clip != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
