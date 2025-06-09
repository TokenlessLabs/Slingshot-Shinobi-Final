using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialPowerupPanell : MonoBehaviour
{
    public GameObject cardPrefab; // Assign your card prefab in the Inspector
    public Transform[] cardSlots; // Assign the card slots in the Inspector
    public Powerup[] powerups; // Assign your powerups in the Inspector
    public GameObject playerObject;

    private void Start()
    {
        DisplayRandomPowerups();
    }

    public void DisplayRandomPowerups()
    {
        // Clear existing cards
        foreach (Transform slot in cardSlots)
        {
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }
        }

        int powerupCount = Mathf.Min(cardSlots.Length, powerups.Length);

        for (int i = 0; i < cardSlots.Length; i++)
        {
            GameObject card = Instantiate(cardPrefab, cardSlots[i]);

            Image cardImage = card.GetComponentInChildren<Image>();
            Text cardText = card.GetComponentInChildren<Text>();
            Button cardButton = card.GetComponent<Button>();

            if (i < powerupCount)
            {
                cardImage.sprite = powerups[i].sprite;
                cardText.text = $"{powerups[i].description}";
                cardImage.gameObject.SetActive(true);

                // Add onClick listener
                cardButton.onClick.AddListener(() => OnCardSelected(i));
            }
            else
            {
                cardImage.gameObject.SetActive(false);
            }
        }
    }

    void OnCardSelected(int index)
    {
        if (playerObject != null)
        {
           TutorialPlayerCollecting collecting = playerObject.GetComponent<TutorialPlayerCollecting>();
            if (collecting != null)
            {
                collecting.CloseFullCollectionPanel();
            }
            else
            {
                Debug.LogWarning("PlayerCollecting component not found on the GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerObject not found in the scene.");
        }
    }
}
