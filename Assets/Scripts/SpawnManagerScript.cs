using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For UI elements
using System.Collections;
public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform player;
    public float spawnRadius = 10f;
    public float timeBetweenWaves = 10f;
    public int enemiesPerWave = 5;
    public int wavesPerLevel = 3;
    private int currentWave = 0;
    private bool spawning = false;
    public Camera mainCamera;
    public float offScreenBuffer = 5f;
    public GameObject waveIncomingText;
    private int activeEnemies = 0;
    private bool waveInProgress = false;
    private bool levelCompleted = false;
    private int randomEnemiesSpawned = 0;
    public float randomSpawnInterval = 5f;
    public int minRandomEnemies = 1;
    public int maxRandomEnemies = 3;
    public bool disabled = false;

    public GameObject levelCompletionPanel; // Reference to the level completion panel
    public Button nextLevelButton; // Reference to the button on the completion panel

    void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Enemy prefabs are not assigned in SpawnManager.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference is not assigned in SpawnManager.");
            return;
        }

        if (LevelDifficultySettings.TryGetCurrent(out LevelDifficulty difficulty))
        {
            timeBetweenWaves = difficulty.timeBetweenWaves;
            enemiesPerWave = difficulty.enemiesPerWave;
            wavesPerLevel = difficulty.wavesPerLevel;
            randomSpawnInterval = difficulty.randomSpawnInterval;
            minRandomEnemies = difficulty.minRandomEnemies;
            maxRandomEnemies = difficulty.maxRandomEnemies;
            randomEnemiesSpawned = 0;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            EnemyMovement movement = enemyPrefabs[i].GetComponent<EnemyMovement>();
            if (movement != null)
                movement.speed = 2 + i;
        }

        StartCoroutine(RandomSpawner());
        StartCoroutine(StartWaveSpawnerAfterDelay());

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(GoToLevelSelector);
        }
    }

    IEnumerator StartWaveSpawnerAfterDelay()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(WaveSpawner());
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

                waveIncomingText.SetActive(true);
                yield return new WaitForSeconds(4f);
                waveIncomingText.SetActive(false);

                int totalEnemies = enemiesPerWave + (currentWave - 1) * 2;
                SpawnEnemies(totalEnemies);

                spawning = false;
                if (currentWave < wavesPerLevel) yield return new WaitForSeconds(timeBetweenWaves);
            }
            yield return null;
        }

        waveInProgress = false;
        CheckForLevelCompletion();
    }

    IEnumerator RandomSpawner()
    {
        while (true)
        {
            if (!levelCompleted)
            {
                int remainingRandomEnemies = LevelDifficultySettings.TryGetCurrent(out LevelDifficulty difficulty)
                    ? difficulty.totalRandomEnemies - randomEnemiesSpawned
                    : 0;
                if (remainingRandomEnemies <= 0)
                {
                    yield return new WaitForSeconds(randomSpawnInterval);
                    continue;
                }

                int randomEnemyCount = Mathf.Min(Random.Range(minRandomEnemies, maxRandomEnemies + 1), remainingRandomEnemies);

                for (int i = 0; i < randomEnemyCount; i++)
                {
                    SpawnRandomEnemy();
                }

                yield return new WaitForSeconds(randomSpawnInterval);
            }
            else
            {
                yield return null;
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
            float angle = Random.Range(0f, Mathf.PI * 2);
            float distance = Random.Range(spawnRadius, spawnRadius + 5f);
            Vector3 spawnPosition = new Vector3(
                player.position.x + Mathf.Cos(angle) * distance,
                player.position.y + Mathf.Sin(angle) * distance,
                0
            );

            int prefabIndex = currentWave % enemyPrefabs.Length;
            GameObject enemy = Instantiate(enemyPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.OnEnemyDestroyed += HandleEnemyDestroyed;
            }
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
            Vector3 screenBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 screenTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

            screenBottomLeft -= new Vector3(offScreenBuffer, offScreenBuffer, 0);
            screenTopRight += new Vector3(offScreenBuffer, offScreenBuffer, 0);

            float randomX = Random.Range(screenBottomLeft.x, screenTopRight.x);
            float randomY = Random.Range(screenBottomLeft.y, screenTopRight.y);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().OnEnemyDestroyed += HandleEnemyDestroyed;

            activeEnemies++;
            randomEnemiesSpawned++;
        }
        else
        {
            Debug.LogError("No enemy prefabs assigned. Cannot spawn enemy.");
        }
    }

    void HandleEnemyDestroyed()
    {
        activeEnemies--;
        Debug.Log($"Enemy destroyed. Remaining active enemies: {activeEnemies}");
        if (activeEnemies <= 0 && !waveInProgress && !disabled)
        {
            Debug.Log("All enemies defeated. Moving to the next level.");
            CheckForLevelCompletion();
        }
    }

    void CheckForLevelCompletion()
    {
        if (!levelCompleted && activeEnemies <= 0)
        {
            levelCompleted = true;
            Debug.Log("Level completed.");
            SaveProgress();
            ShowLevelCompletionPanel();
        }
    }

    void SaveProgress()
    {
        try
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (!sceneName.StartsWith("Level ") || !int.TryParse(sceneName.Substring(6), out int currentLevel))
            {
                return;
            }

            int nextLevel = currentLevel + 1;

            int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

            Debug.Log($"Current unlocked level: {unlockedLevel}. Next level: {nextLevel}");

            if (nextLevel > unlockedLevel)
            {
                PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
                PlayerPrefs.Save();
                Debug.Log($"Progress saved: Level {nextLevel} unlocked.");
            }
            else
            {
                Debug.Log($"No update needed. Current unlocked level ({unlockedLevel}) is higher than or equal to the next level ({nextLevel}).");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving progress: {e.Message}");
        }
    }

    //void LoadNextLevel()
    //{
    //    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    Debug.Log($"Loading next level: {currentSceneIndex + 1}");
    //    SceneManager.LoadScene(currentSceneIndex + 1);
    //}

    void ShowLevelCompletionPanel()
    {
        GameplayState.BeginTerminalState();
        GameplayState.DisablePlayerGameplay();
        GameplayState.StopGameplayAudio();
        Time.timeScale = 0f;
        AudioListener.pause = false;
        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = true;
        }
        else if (levelCompletionPanel != null)
        {
            nextLevelButton = levelCompletionPanel.GetComponentInChildren<Button>(true);
            if (nextLevelButton != null)
            {
                nextLevelButton.onClick.AddListener(GoToLevelSelector);
            }
        }

        PauseManager pauseManager = FindObjectOfType<PauseManager>();
        if (pauseManager != null && pauseManager.pauseButton != null)
        {
            pauseManager.pauseButton.interactable = false;
        }
        Level5Pause level5Pause = FindObjectOfType<Level5Pause>();
        if (level5Pause != null && level5Pause.pauseButton != null)
        {
            level5Pause.pauseButton.interactable = false;
        }

        // Deactivate the gameplay elements
        if (waveIncomingText != null)
        {
            waveIncomingText.SetActive(false);
        }
        // Activate the level completion panel
        if (levelCompletionPanel != null)
        {
            levelCompletionPanel.SetActive(true);
        }
    }

    void GoToLevelSelector()
    {
        // Load the level selector scene
        Debug.Log("Going to level selector scene.");
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameplayState.Reset();
        SceneManager.LoadScene("Level Selector"); // Ensure "LevelSelector" is the name of your level selector scene
    }
}