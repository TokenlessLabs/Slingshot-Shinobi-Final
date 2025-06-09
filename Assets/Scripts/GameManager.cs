using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text killCountText; // Reference to the TMP_Text component
    private int enemyKillCount = 0;

    private void Start()
    {
        UpdateKillCountText();
    }
    public void EnemyKilled()
    {
        enemyKillCount++;
        UpdateKillCountText();
    }
    private void UpdateKillCountText()
    {
        killCountText.text = enemyKillCount.ToString();
    }
}
