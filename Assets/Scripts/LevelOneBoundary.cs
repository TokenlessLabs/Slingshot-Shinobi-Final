using UnityEngine;
using UnityEngine.Tilemaps;

public static class LevelOneBoundary
{
    public static bool TryGetBounds(out Bounds bounds)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Level 1")
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
