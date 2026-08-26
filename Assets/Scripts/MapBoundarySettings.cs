using UnityEngine;
using UnityEngine.SceneManagement;

public static class MapBoundarySettings
{
    public static bool TryGetWalkableBounds(out Bounds bounds)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Level 2":
                bounds = CreateBounds(-88f, 120f, -60f, 62f);
                return true;
            case "Level 3":
                bounds = CreateBounds(-66f, 60f, -42f, 34f);
                return true;
            case "Level 4":
                bounds = CreateBounds(-94f, 96f, -66f, 72f);
                return true;
            case "Level 5":
                bounds = CreateBounds(-92f, 94f, -66f, 68f);
                return true;
            default:
                bounds = default;
                return false;
        }
    }

    private static Bounds CreateBounds(float minX, float maxX, float minY, float maxY)
    {
        Vector3 minimum = new Vector3(minX, minY, 0f);
        Vector3 maximum = new Vector3(maxX, maxY, 0f);
        return new Bounds((minimum + maximum) * 0.5f, maximum - minimum);
    }
}
