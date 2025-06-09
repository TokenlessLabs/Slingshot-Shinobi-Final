using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDestroyed();
    public event EnemyDestroyed OnEnemyDestroyed;

    void OnDestroy()
    {
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed.Invoke();
        }
    }
}
