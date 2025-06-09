using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
    private AsyncOperation asyncOperation;

    void Start()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene (assuming scenes are added in order)
        StartCoroutine(LoadSceneAsync(currentSceneIndex + 1));
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Update progress bar
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.value = progress;

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                // Update progress bar to full
                progressBar.value = 1f;

                // Optionally wait for a few seconds
                yield return new WaitForSeconds(2);

                // Activate the scene
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
