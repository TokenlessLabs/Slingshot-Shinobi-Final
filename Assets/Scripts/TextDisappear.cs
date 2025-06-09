using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextDisappear : MonoBehaviour
{
    public Text textComponent;
    public float disappearAfterSeconds = 3.0f; 
    public float fadeDuration = 2.0f; 

    void Start()
    {
        if (textComponent != null)
        {
            StartCoroutine(HideTextAfterDelay());
        }
    }

    IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(disappearAfterSeconds);
        Color originalColor = textComponent.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        textComponent.enabled = false;
    }
}
