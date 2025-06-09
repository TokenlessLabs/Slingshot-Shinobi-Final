using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerupPanel : MonoBehaviour
{
    public GameObject cardPrefab; 
    public Transform[] cardSlots; 
    public Powerup[] powerups; 

    private void Start()
    {
        DisplayRandomPowerups();
    }

    public void DisplayRandomPowerups()
    {
        foreach (Transform slot in cardSlots)
        {
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }
        }
        Powerup[] availablePowerups = System.Array.FindAll(powerups, p => p.stage <= p.maxStage);

        int powerupCount = Mathf.Min(cardSlots.Length, availablePowerups.Length);
        Powerup[] weightedPowerups = GetWeightedPowerups(availablePowerups);

        for (int i = 0; i < cardSlots.Length; i++)
        {
            GameObject card = Instantiate(cardPrefab, cardSlots[i]);

            Image cardImage = card.GetComponentInChildren<Image>();
            Text cardText = card.GetComponentInChildren<Text>();
            Button cardButton = card.GetComponent<Button>();

            if (i < powerupCount)
            {
                Powerup randomPowerup = weightedPowerups[i];
                cardImage.sprite = randomPowerup.sprite;
                cardText.text = $"{randomPowerup.description}";
                if (randomPowerup.id > 0 && randomPowerup.id < 7) cardText.text += $"{randomPowerup.stage}0%";
                else if (randomPowerup.id == 7) cardText.text += $"{availablePowerups[availablePowerups.Length - 1].stage * 4} seconds";
                cardImage.gameObject.SetActive(true);
                cardButton.onClick.AddListener(() => OnCardSelected(randomPowerup.id));
            }
            else
            {
                cardImage.gameObject.SetActive(false);
            }
        }
    }

    Powerup[] GetWeightedPowerups(Powerup[] powerups)
    {
        int[] weights = new int[powerups.Length];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = (i == weights.Length - 1) ? 1 : 10; 
        }

        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        Powerup[] weightedPowerups = new Powerup[powerups.Length];
        for (int i = 0; i < weightedPowerups.Length; i++)
        {
            int randomWeight = Random.Range(0, totalWeight);
            int weightSum = 0;

            for (int j = 0; j < powerups.Length; j++)
            {
                weightSum += weights[j];
                if (randomWeight < weightSum)
                {
                    weightedPowerups[i] = powerups[j];
                    totalWeight -= weights[j];
                    weights[j] = 0;
                    break;
                }
            }
        }
        return weightedPowerups;
    }

    void OnCardSelected(int index)
    {
        Powerup selectedPowerup = powerups[index];
        Debug.Log($"Selected Powerup: {selectedPowerup.description}, Stage: {selectedPowerup.stage}");
        GameObject playerObject = GameObject.Find("Player");
        ApplyPowerup(selectedPowerup, index, playerObject);

        if (playerObject != null)
        {
            PlayerCollecting collecting = playerObject.GetComponent<PlayerCollecting>();
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

    void ApplyPowerup(Powerup powerup, int index, GameObject playerObject)
    {
        if (index == 0) // Health full heal
        {
            PlayerHealth health = playerObject.GetComponent<PlayerHealth>();
            health.currentHealth = health.maxHealth;
            if (powerup.stage == powerup.maxStage) powerup.stage--;
        }
        else if (index == 1) // Slow Down Enemies
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
                movement.speed *= 0.9f; 
            }
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            foreach (GameObject enemyPrefab in spawnManager.enemyPrefabs)
            {
                EnemyMovement movement = enemyPrefab.GetComponent<EnemyMovement>();
                movement.speed *= 0.9f;
            }
        }
        else if (index == 2) // Reduce Dash Cooldown
        {
            Dash dash = playerObject.GetComponent<Dash>();
            dash.cooldown *= 0.9f;
        }
        else if (index == 3) // Increase Dash AoE
        {
            Dash dash = playerObject.GetComponent<Dash>();
            dash.aoeRadius *= 1.1f;
        }
        else if (index == 4) // Increase Dash Distance
        {
            Dash dash = playerObject.GetComponent<Dash>();
            dash.dashSpeed *= 1.1f;
        }
        else if (index == 5) // Increase Pebble Speed
        {
            PlayerShooting shooting = playerObject.GetComponent<PlayerShooting>();
            shooting.shootInterval *= 0.9f;
        }
        else if (index == 6) // Increase Player Speed
        {
            PlayerMovementWithJoystick movement = playerObject.GetComponent<PlayerMovementWithJoystick>();
            movement.speed *= 1.1f;
        }
        else if (index == 7) // Get Infinite Dashes
        {
            Dash dash = playerObject.GetComponent<Dash>();
            float infiniteDashDuration = powerup.stage * 4f;
            if (powerup.stage == powerup.maxStage) powerup.stage--;
            dash.ResetCooldown(infiniteDashDuration);
        }
        powerup.stage++;
        if (powerup.stage >= powerup.maxStage)
        {
            powerups = System.Array.FindAll(powerups, p => p.stage <= p.maxStage);
        }
    }
}