using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSpawnManager : MonoBehaviour
{
    public GameObject bossBar;
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public GameObject bossPrefab; // Reference to the boss prefab
    public Transform player; // Reference to the player
    public float spawnRadius = 10f; // Radius around the player to spawn enemies
    public float timeBetweenWaves = 10f; // Time between waves
    public int enemiesPerWave = 5; // Initial number of enemies to spawn per wave
    public int wavesPerLevel = 3; // Number of waves per level
    private int currentWave = 0;
    private bool spawning = false;
    public Camera mainCamera;
    public float offScreenBuffer = 5f;
    public GameObject waveIncomingText;
    private int activeEnemies = 0; // Number of active enemies
    private bool waveInProgress = false; // Track if a wave is in progress
    private bool levelCompleted = false; // Track if the level has been completed
    private bool bossSpawned = false; // Track if the boss has been spawned
    // Random spawn settings
    public float randomSpawnInterval = 5f; // Time between random spawns
    public int minRandomEnemies = 1; // Minimum number of random enemies to spawn
    public int maxRandomEnemies = 3; // Maximum number of random enemies to spawn

    void Start()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy prefabs are not assigned in SpawnManager.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference is not assigned in SpawnManager.");
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Start the RandomSpawner coroutine and delay the start of WaveSpawner
        StartCoroutine(RandomSpawner());
        StartCoroutine(StartWaveSpawnerAfterDelay());
    }

    IEnumerator StartWaveSpawnerAfterDelay()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Delay before starting the first wave
        StartCoroutine(WaveSpawner()); // Start the wave spawner
    }

    IEnumerator WaveSpawner()
    {
        while (currentWave < wavesPerLevel)
        {
            if (!spawning)
            {
                spawning = true;
                currentWave++;
                waveInProgress = true;

                // Display the Wave Incoming text 5 seconds before the wave spawns
                waveIncomingText.SetActive(true);

                // Wait for 4 seconds (1 second remaining before the wave spawns)
                yield return new WaitForSeconds(4f);

                // Hide the Wave Incoming text
                waveIncomingText.SetActive(false);

                // Calculate the total number of enemies for the current wave
                int totalEnemies = enemiesPerWave + (currentWave - 1) * 2;

                // Spawn enemies for the current wave
                SpawnEnemies(totalEnemies);

                // Check if it's the final wave
                if (currentWave == wavesPerLevel)
                {
                    // Only spawn the boss if it has not been spawned yet
                    if (!bossSpawned)
                    {
                        // Wait for a brief moment to make sure enemies are in place
                        yield return new WaitForSeconds(2f);
                        SpawnBoss();
                    }
                }

                spawning = false;
                yield return new WaitForSeconds(timeBetweenWaves); // Wait before starting the next wave
            }
            yield return null; // Wait until the next frame
        }

        // After all waves are completed
        waveInProgress = false;
        CheckForLevelCompletion();
    }

    IEnumerator RandomSpawner()
    {
        while (true)
        {
            if (!levelCompleted)
            {
                if (!waveInProgress || currentWave < wavesPerLevel)
                {
                    // Randomly determine the number of enemies to spawn
                    int randomEnemyCount = Random.Range(minRandomEnemies, maxRandomEnemies + 1);

                    // Spawn random enemies off-screen
                    for (int i = 0; i < randomEnemyCount; i++)
                    {
                        SpawnRandomEnemy();
                    }

                    // Wait before spawning the next batch of random enemies
                    yield return new WaitForSeconds(randomSpawnInterval);
                }
                else
                {
                    yield return null; // Wait until the next frame if wave is in progress or it's the last wave
                }
            }
            else
            {
                yield return null; // Wait until the next frame if level is completed
            }
        }
    }

    void SpawnEnemies(int totalEnemies)
    {
        activeEnemies += totalEnemies;
        for (int i = 0; i < totalEnemies; i++)
        {
            SpawnEnemy();
        }
        Debug.Log($"Spawned {totalEnemies} wave enemies. Total active enemies: {activeEnemies}");
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length > 0)
        {
            // Calculate a random angle around the player
            float angle = Random.Range(0f, Mathf.PI * 2);

            // Calculate a random distance within the spawn radius
            float distance = Random.Range(spawnRadius, spawnRadius + 5f);

            // Calculate the spawn position around the player
            Vector3 spawnPosition = new Vector3(
                player.position.x + Mathf.Cos(angle) * distance,
                player.position.y + Mathf.Sin(angle) * distance,
                0
            );
            GameObject enemy = Instantiate(enemyPrefabs[currentWave % 2 == 0 ? 1 : 0], spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;
        }
        else
        {
            Debug.LogError("No enemy prefabs assigned. Cannot spawn enemy.");
        }
    }

    void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Length > 0)
        {
            // Get the screen bounds in world space
            Vector3 screenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 screenTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            // Expand the bounds to allow spawning a bit off-screen
            screenBottomLeft -= new Vector3(offScreenBuffer, offScreenBuffer, 0);
            screenTopRight += new Vector3(offScreenBuffer, offScreenBuffer, 0);

            // Generate a random position within these expanded bounds
            float randomX = Random.Range(screenBottomLeft.x, screenTopRight.x);
            float randomY = Random.Range(screenBottomLeft.y, screenTopRight.y);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

            // Randomly select an enemy prefab to instantiate
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;

            // Increment active enemies count for random enemies
            activeEnemies++;
        }
        else
        {
            Debug.LogError("No enemy prefabs assigned. Cannot spawn enemy.");
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && !bossSpawned)
        {
            // Calculate a random angle around the player
            float angle = Random.Range(0f, Mathf.PI * 2);

            // Calculate a random distance within the spawn radius
            float distance = Random.Range(spawnRadius, spawnRadius + 5f);

            // Calculate the spawn position around the player
            Vector3 spawnPosition = new Vector3(
                player.position.x + Mathf.Cos(angle) * distance,
                player.position.y + Mathf.Sin(angle) * distance,
                0
            );
            bossBar.SetActive(true);
            GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            boss.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;
            bossSpawned = true;
            activeEnemies++;
            Debug.Log("Boss spawned.");
        }
        else
        {
            Debug.LogError("No boss prefab assigned. Cannot spawn boss.");
        }
    }

    void HandleEnemyDestroyed()
    {
        activeEnemies--;
        Debug.Log($"Enemy destroyed. Remaining active enemies: {activeEnemies}");
        if (activeEnemies <= 0 && !waveInProgress)
        {
            // All enemies defeated and no wave in progress, move to the next level
            Debug.Log("All enemies defeated. Moving to the next level.");
            CheckForLevelCompletion();
        }
    }

    void CheckForLevelCompletion()
    {
        if (!levelCompleted && activeEnemies <= 0)
        {
            levelCompleted = true;
            Debug.Log("All enemies defeated. Moving to the next level.");
            LoadNextLevel();
        }
    }

    void SaveProgress()
    {
        try
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextLevelIndex = currentSceneIndex + 1;

            int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

            Debug.Log($"Current unlocked level: {unlockedLevel}. Next level index: {nextLevelIndex}");

            if (nextLevelIndex > unlockedLevel)
            {
                PlayerPrefs.SetInt("UnlockedLevel", nextLevelIndex);
                PlayerPrefs.Save();
                Debug.Log($"Progress saved: Level {nextLevelIndex} unlocked.");
            }
            else
            {
                Debug.Log($"No update needed. Current unlocked level ({unlockedLevel}) is higher than or equal to the next level index ({nextLevelIndex}).");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving progress: {e.Message}");
        }
    }

    void LoadNextLevel()
    {
        // Handle end-of-game logic here
        Debug.Log("Game Over. No more levels.");
    }
}
