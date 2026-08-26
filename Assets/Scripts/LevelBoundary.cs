using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public static class LevelBoundary
{
    public static bool TryGetBounds(string levelName, out Bounds bounds)
    {
        if (SceneManager.GetActiveScene().name != levelName)
        {
            bounds = default;
            return false;
        }

        Tilemap tilemap = GameObject.Find("Tilemap")?.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            bounds = default;
            return false;
        }

        BoundsInt cells = tilemap.cellBounds;
        Vector3 minimum = tilemap.CellToWorld(cells.min);
        Vector3 maximum = tilemap.CellToWorld(cells.max);
        bounds = new Bounds((minimum + maximum) * 0.5f, maximum - minimum);
        return bounds.size.x > 0f && bounds.size.y > 0f;
    }
}
