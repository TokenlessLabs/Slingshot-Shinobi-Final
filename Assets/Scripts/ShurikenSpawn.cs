using UnityEngine;

public class ShurikenSpawner : MonoBehaviour
{
    public GameObject shurikenPrefab;
    public int numberOfShurikens = 10;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    void Start()
    {
        for (int i = 0; i < numberOfShurikens; i++)
        {
            SpawnShuriken();
        }
    }

    void SpawnShuriken()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(x, y, 0);
        Instantiate(shurikenPrefab, spawnPosition, Quaternion.identity);
    }
}
