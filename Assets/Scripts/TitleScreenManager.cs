using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject titleImage;
    private AudioSource audioSource; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true; 
            audioSource.Play(); 
        }
        if (titleImage != null)
        {
            titleImage.SetActive(true);
        }
    }
    public void LoadNextLevel()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}