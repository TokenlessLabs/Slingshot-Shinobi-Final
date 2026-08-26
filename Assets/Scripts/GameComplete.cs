using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComplete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Completion()
    {
        GameObject Panel = GameObject.FindGameObjectWithTag("CompletionPanel");
        GameplayState.BeginTerminalState();
        GameplayState.DisablePlayerGameplay();
        GameplayState.StopGameplayAudio();
        Time.timeScale = 0f;
        AudioListener.pause = false;
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }
}
