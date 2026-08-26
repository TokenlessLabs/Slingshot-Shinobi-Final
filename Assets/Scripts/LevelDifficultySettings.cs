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
}

public static class LevelDifficultySettings
{
    public static bool TryGetCurrent(out LevelDifficulty difficulty)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 1":
                difficulty = Create(25f, 7, 1, 12f, 1, 2);
                return true;
            case "Level 2":
                difficulty = Create(20f, 9, 2, 10f, 1, 3);
                return true;
            case "Level 3":
                difficulty = Create(18f, 10, 3, 8f, 2, 4);
                return true;
            case "Level 4":
                difficulty = Create(16f, 11, 4, 6f, 3, 5);
                return true;
            case "Level 5":
                difficulty = Create(14f, 12, 5, 5f, 4, 6);
                return true;
            default:
                difficulty = default;
                return false;
        }
    }

    private static LevelDifficulty Create(float waveInterval, int enemies, int waves, float randomInterval, int minimumRandom, int maximumRandom)
    {
        return new LevelDifficulty
        {
            timeBetweenWaves = waveInterval,
            enemiesPerWave = enemies,
            wavesPerLevel = waves,
            randomSpawnInterval = randomInterval,
            minRandomEnemies = minimumRandom,
            maxRandomEnemies = maximumRandom
        };
    }
}
