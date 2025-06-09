using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCollecting : MonoBehaviour
{
    public Image collectionBar; // Reference to the UI Image for the collection bar
    public int totalShurikens = 3; // Total number of shurikens to collect
    public int collectedShurikens = 0; // Number of shurikens collected
    private int level = 3;
    public GameObject fullCollectionPanel; // Reference to the panel that appears when the bar is full
    public float fillSpeed = 2f; // Speed at which the bar fills
    private Coroutine fillCoroutine;
    public AudioSource collectionAudioSource;
    public AudioClip collectionSound;

    void Start()
    {
        ResetCollectionBar(); 
        if (collectionAudioSource == null)
        {
            Debug.LogError("Collection AudioSource component not assigned on the player!");
        }
    }

    public void CollectShuriken()
    {
        collectedShurikens++;
        if (collectionAudioSource != null && collectionSound != null)
        {
            collectionAudioSource.clip = collectionSound;
            collectionAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("CollectionAudioSource or CollectionSound not assigned!");
        }

        UpdateCollectionBar();
    }

    void UpdateCollectionBar()
    {
        float targetFillAmount = (float)collectedShurikens / totalShurikens;
        targetFillAmount = Mathf.Clamp01(targetFillAmount);

        if (targetFillAmount > collectionBar.fillAmount)
        {
            if (fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
            }
            fillCoroutine = StartCoroutine(AnimateFill(targetFillAmount));
        }
    }

    IEnumerator AnimateFill(float targetFillAmount)
    {
        while (collectionBar.fillAmount < targetFillAmount)
        {
            collectionBar.fillAmount = Mathf.MoveTowards(collectionBar.fillAmount, targetFillAmount, fillSpeed * Time.deltaTime);
            yield return null;
        }
        collectionBar.fillAmount = targetFillAmount;
        if (collectionBar.fillAmount >= 1) ShowFullCollectionPanel();
    }

    void ShowFullCollectionPanel()
    {
        fullCollectionPanel.SetActive(true); 
        Time.timeScale = 0f; 
        PowerupPanel panel = fullCollectionPanel.GetComponent<PowerupPanel>();
        panel.DisplayRandomPowerups();
    }

    public void CloseFullCollectionPanel()
    {
        fullCollectionPanel.SetActive(false); 
        Time.timeScale = 1f; 
        ResetCollectionBar(); 
    }

    public void ResetCollectionBar()
    {
        collectedShurikens = 0; 
        totalShurikens = level * level /2;
        level++;
        collectionBar.fillAmount = 0f; 
    }
}