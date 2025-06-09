using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        LoadUnlockedLevels();
    }

    private void LoadUnlockedLevels()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); 

        Debug.Log($"Loaded unlocked level: {unlockedLevel}");

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = (i + 1 <= unlockedLevel);
            Debug.Log($"Button {i + 1} interactable: {buttons[i].interactable}");
        }
    }

    public void OpenLevel(int levelID)
    {
        string levelName = "Level " + levelID;
        Debug.Log($"Loading level: {levelName}");
        SceneManager.LoadScene(levelName);
    }
}