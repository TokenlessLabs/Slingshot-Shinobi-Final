using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialPlayerCollecting : MonoBehaviour
{
    public Image collectionBar;
    public int totalShurikens = 3; 
    public int collectedShurikens = 0; 
    private int level = 2;
    public GameObject fullCollectionPanel;
    public float fillSpeed = 2f; 
    public GameObject manager;
    private TutorialManager script;

    private Coroutine fillCoroutine;

    private void Start()
    {
        script = manager.GetComponent<TutorialManager>();
        ResetCollectionBar(); 
    }

    public void CollectShuriken()
    {
        collectedShurikens++;
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
        TutorialPowerupPanell panel = fullCollectionPanel.GetComponent<TutorialPowerupPanell>();
        panel.DisplayRandomPowerups();
    }

    public void CloseFullCollectionPanel()
    {
        fullCollectionPanel.SetActive(false); 
        script.currStep=9;
        ResetCollectionBar(); 
    }

    public void ResetCollectionBar()
    {
        collectedShurikens = 0; 
        totalShurikens = level * level;
        level++;
        collectionBar.fillAmount = 0f; 
    }
}
