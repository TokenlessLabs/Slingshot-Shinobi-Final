//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class GameManager : MonoBehaviour
//{
//    public static int enemiesKilled = 0; // Static variable to store kill count
//    public Text killCountText; // Reference to the UI Text component

//    void Start()
//    {
//        // Initialize the kill count
//        enemiesKilled = 0;
//        UpdateKillCountText();
//    }

//    // Method to update the UI text
//    public void UpdateKillCountText()
//    {
//        killCountText.text = "Enemies Killed: " + enemiesKilled;
//    }

//    // Method to be called when an enemy is killed
//    public void EnemyKilled()
//    {
//        enemiesKilled++;
//        UpdateKillCountText();
//    }
//}

