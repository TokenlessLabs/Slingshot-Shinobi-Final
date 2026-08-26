using UnityEngine;
using UnityEngine.SceneManagement;

public struct LevelDifficulty
{
    public float timeBetweenWaves;
    public int enemiesPerWave;
    public int wavesPerLevel;
    public float randomSpawnInterval;
    public int minRandomEnemies;
    public int maxRandomEnemies;
    public int totalRandomEnemies;
}

public static class LevelDifficultySettings
{
    public static bool TryGetCurrent(out LevelDifficulty difficulty)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 1":
                difficulty = Create(25f, 10, 1, 5f, 1, 2, 20);
                return true;
            case "Level 2":
                difficulty = Create(22f, 12, 2, 8f, 2, 4, 30);
                return true;
            case "Level 3":
                difficulty = Create(20f, 16, 2, 7f, 3, 5, 40);
                return true;
            case "Level 4":
                difficulty = Create(18f, 20, 3, 6f, 4, 6, 50);
                return true;
            case "Level 5":
                difficulty = Create(16f, 24, 4, 5f, 5, 7, 60);
                return true;
            default:
                difficulty = default;
                return false;
        }
    }

    private static LevelDifficulty Create(float waveInterval, int enemies, int waves, float randomInterval, int minimumRandom, int maximumRandom, int totalRandom)
    {
        return new LevelDifficulty
        {
            timeBetweenWaves = waveInterval,
            enemiesPerWave = enemies,
            wavesPerLevel = waves,
            randomSpawnInterval = randomInterval,
            minRandomEnemies = minimumRandom,
            maxRandomEnemies = maximumRandom,
            totalRandomEnemies = totalRandom
        };
    }
}
