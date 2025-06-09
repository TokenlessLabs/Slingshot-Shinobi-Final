using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pressed : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponentInChildren<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found!");
        }
    }

    void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        SceneManager.LoadScene("TitleScreen");
    }
}
