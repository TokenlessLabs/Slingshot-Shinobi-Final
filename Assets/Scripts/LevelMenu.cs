using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private const string UnlockedLevelKey = "UnlockedLevel";
    private const string ProgressVersionKey = "ProgressVersion";
    private const int CurrentProgressVersion = 2;

    public Button[] buttons;

    private void Awake()
    {
        MigrateProgress();
        LoadUnlockedLevels();
    }

    private void MigrateProgress()
    {
        if (PlayerPrefs.GetInt(ProgressVersionKey, 0) == CurrentProgressVersion)
        {
            return;
        }

        PlayerPrefs.SetInt(UnlockedLevelKey, 1);
        PlayerPrefs.SetInt(ProgressVersionKey, CurrentProgressVersion);
        PlayerPrefs.Save();
    }

    private void LoadUnlockedLevels()
    {
        int unlockedLevel = Mathf.Clamp(PlayerPrefs.GetInt(UnlockedLevelKey, 1), 1, buttons.Length);

        Debug.Log($"Loaded unlocked level: {unlockedLevel}");

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null)
            {
                continue;
            }

            buttons[i].interactable = i + 1 <= unlockedLevel;
            Debug.Log($"Button {i + 1} interactable: {buttons[i].interactable}");
        }
    }

    public void OpenLevel(int levelID)
    {
        int unlockedLevel = PlayerPrefs.GetInt(UnlockedLevelKey, 1);
        if (levelID < 1 || levelID > buttons.Length || levelID > unlockedLevel)
        {
            Debug.LogWarning($"Level {levelID} is locked or invalid.");
            return;
        }

        string levelName = "Level " + levelID;
        Debug.Log($"Loading level: {levelName}");
        SceneManager.LoadScene(levelName);
    }
}